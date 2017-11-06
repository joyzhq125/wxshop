namespace Hidistro.ControlPanel.Sales
{
    using Hidistro.ControlPanel.Members;
    using Hidistro.ControlPanel.Promotions;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Commodities;
    using Hidistro.Entities.Members;
    using Hidistro.Entities.Orders;
    using Hidistro.Entities.Promotions;
    using Hidistro.Entities.Store;
    using Hidistro.Messages;
    using Hidistro.SqlDal.Commodities;
    using Hidistro.SqlDal.Members;
    using Hidistro.SqlDal.Orders;
    using Hidistro.SqlDal.VShop;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Globalization;
    using System.Linq;
    using System.Runtime.InteropServices;

    public static class OrderHelper
    {
        public static bool CloseTransaction(OrderInfo order)
        {
            ManagerHelper.CheckPrivilege(Privilege.EditOrders);
            if (!order.CheckAction(OrderActions.SELLER_CLOSE))
            {
                return false;
            }
            order.OrderStatus = OrderStatus.Closed;
            bool flag = new OrderDao().UpdateOrder(order, null);
            if (order.GroupBuyId > 0)
            {
                GroupBuyInfo groupBuy = GroupBuyHelper.GetGroupBuy(order.GroupBuyId);
                groupBuy.SoldCount -= order.GetGroupBuyProductQuantity();
                GroupBuyHelper.UpdateGroupBuy(groupBuy);
            }
            string itemStr = "'" + string.Join("','", order.LineItems.Keys) + "'";
            new OrderDao().UpdateItemsStatus(order.OrderId, 4, itemStr);
            if (flag)
            {
                EventLogs.WriteOperationLog(Privilege.EditOrders, string.Format(CultureInfo.InvariantCulture, "关闭了订单“{0}”", new object[] { order.OrderId }));
            }
            return flag;
        }

        //确认收货
        public static bool ConfirmOrderFinish(OrderInfo order)
        {
            ManagerHelper.CheckPrivilege(Privilege.EditOrders);
            bool flag = false;
            if (order.CheckAction(OrderActions.SELLER_FINISH_TRADE))
            {
                order.OrderStatus = OrderStatus.Finished;
                order.FinishDate = new DateTime?(DateTime.Now);
                //货到付款(确认收货时更新付款时间)
                if(order.Gateway== "hishop.plugins.payment.podrequest")
                {
                    order.PayDate = new DateTime?(DateTime.Now);
                }
                flag = new OrderDao().UpdateOrder(order, null);
                if (flag)
                {
                    EventLogs.WriteOperationLog(Privilege.EditOrders, string.Format(CultureInfo.InvariantCulture, "完成编号为\"{0}\"的订单", new object[] { order.OrderId }));
                }
            }
            return flag;
        }

        public static bool ConfirmPay(OrderInfo order)
        {
            ManagerHelper.CheckPrivilege(Privilege.CofimOrderPay);
            bool flag = false;
            if (order.CheckAction(OrderActions.SELLER_CONFIRM_PAY))
            {
                OrderDao dao = new OrderDao();
                order.OrderStatus = OrderStatus.BuyerAlreadyPaid;
                order.PayDate = new DateTime?(DateTime.Now);
                flag = dao.UpdateOrder(order, null);
                string str = "";
                if (!flag)
                {
                    return flag;
                }
                dao.UpdatePayOrderStock(order.OrderId);
                foreach (LineItemInfo info in order.LineItems.Values)
                {
                    ProductDao dao2 = new ProductDao();
                    str = str + "'" + info.SkuId + "',";
                    ProductInfo productDetails = dao2.GetProductDetails(info.ProductId);
                    productDetails.SaleCounts += info.Quantity;
                    productDetails.ShowSaleCounts += info.Quantity;
                    dao2.UpdateProduct(productDetails, null);
                }
                if (!string.IsNullOrEmpty(str))
                {
                    dao.UpdateItemsStatus(order.OrderId, 2, str.Substring(0, str.Length - 1));
                }
                if (!string.IsNullOrEmpty(order.ActivitiesId))
                {
                    new ActivitiesDao().UpdateActivitiesTakeEffect(order.ActivitiesId);
                }
                Messenger.OrderPayment(new MemberDao().GetMember(order.UserId), order.OrderId, order.GetTotal(),order.wid);
                EventLogs.WriteOperationLog(Privilege.CofimOrderPay, string.Format(CultureInfo.InvariantCulture, "确认收款编号为\"{0}\"的订单", new object[] { order.OrderId }));
            }
            return flag;
        }

        public static bool DelDebitNote(string[] noteIds, out int count)
        {
            ManagerHelper.CheckPrivilege(Privilege.DeleteOrder);
            bool flag = true;
            count = 0;
            foreach (string str in noteIds)
            {
                if (!string.IsNullOrEmpty(str))
                {
                    flag &= new DebitNoteDao().DelDebitNote(str);
                    if (flag)
                    {
                        count++;
                    }
                }
            }
            return flag;
        }

        public static bool DeleteLineItem(string sku, OrderInfo order)
        {
            bool flag;
            ManagerHelper.CheckPrivilege(Privilege.EditOrders);
            using (DbConnection connection = DatabaseFactory.CreateDatabase().CreateConnection())
            {
                connection.Open();
                DbTransaction dbTran = connection.BeginTransaction();
                try
                {
                    order.LineItems.Remove(sku);
                    if (!new LineItemDao().DeleteLineItem(sku, order.OrderId, dbTran))
                    {
                        dbTran.Rollback();
                        return false;
                    }
                    if (!new OrderDao().UpdateOrder(order, dbTran))
                    {
                        dbTran.Rollback();
                        return false;
                    }
                    dbTran.Commit();
                    flag = true;
                }
                catch
                {
                    dbTran.Rollback();
                    flag = false;
                }
                finally
                {
                    connection.Close();
                }
            }
            if (flag)
            {
                EventLogs.WriteOperationLog(Privilege.EditOrders, string.Format(CultureInfo.InvariantCulture, "删除了订单号为\"{0}\"的订单商品", new object[] { order.OrderId }));
            }
            return flag;
        }

        public static int DeleteOrders(string orderIds)
        {
            ManagerHelper.CheckPrivilege(Privilege.DeleteOrder);
            int num = new OrderDao().DeleteOrders(orderIds);
            if (num > 0)
            {
                EventLogs.WriteOperationLog(Privilege.DeleteOrder, string.Format(CultureInfo.InvariantCulture, "删除了编号为\"{0}\"的订单", new object[] { orderIds }));
            }
            return num;
        }

        public static bool DelSendNote(string[] noteIds, out int count)
        {
            ManagerHelper.CheckPrivilege(Privilege.DeleteOrder);
            bool flag = true;
            count = 0;
            foreach (string str in noteIds)
            {
                if (!string.IsNullOrEmpty(str))
                {
                    flag &= new SendNoteDao().DelSendNote(str);
                    if (flag)
                    {
                        count++;
                    }
                }
            }
            return flag;
        }

        public static bool EditOrderShipNumber(string orderid, string shipnumber)
        {
            return new OrderDao().EditOrderShipNumber(orderid, shipnumber);
        }

        public static DbQueryResult GetAllDebitNote(DebitNoteQuery query)
        {
            return new DebitNoteDao().GetAllDebitNote(query);
        }

        public static DataTable GetAllOrderID(string wid)
        {
            return new OrderDao().GetAllOrderID(wid);
        }

        public static DbQueryResult GetAllSendNote(RefundApplyQuery query)
        {
            return new SendNoteDao().GetAllSendNote(query);
        }

        public static OrderInfo GetCalculadtionCommission(OrderInfo order)
        {
            DistributorsDao dao = new DistributorsDao();
            DistributorGradeDao dao2 = new DistributorGradeDao();
            DistributorsInfo distributorInfo = dao.GetDistributorInfo(order.ReferralUserId);
            if (distributorInfo != null)
            {
                decimal num = 0M;
                decimal num2 = 0M;
                decimal num3 = 0M;
                DataView defaultView = dao2.GetAllDistributorGrade().DefaultView;
                if (distributorInfo.DistriGradeId.ToString() != "0")
                {
                    defaultView.RowFilter = " GradeId=" + distributorInfo.DistriGradeId;
                    if (defaultView.Count > 0)
                    {
                        num = decimal.Parse(defaultView[0]["FirstCommissionRise"].ToString());
                    }
                }
                if (!string.IsNullOrEmpty(distributorInfo.ReferralPath) && (distributorInfo.ReferralPath != "0"))
                {
                    string[] strArray = distributorInfo.ReferralPath.Split(new char[] { '|' });
                    if (strArray.Length == 1)
                    {
                        DistributorsInfo info2 = dao.GetDistributorInfo(int.Parse(strArray[0]));
                        if (info2.DistriGradeId.ToString() != "0")
                        {
                            defaultView.RowFilter = " GradeId=" + info2.DistriGradeId;
                            if (defaultView.Count > 0)
                            {
                                num2 = decimal.Parse(defaultView[0]["SecondCommissionRise"].ToString());
                            }
                        }
                    }
                    else
                    {
                        DistributorsInfo info3 = dao.GetDistributorInfo(int.Parse(strArray[1]));
                        if (info3.DistriGradeId.ToString() != "0")
                        {
                            defaultView.RowFilter = " GradeId=" + info3.DistriGradeId;
                            if (defaultView.Count > 0)
                            {
                                num2 = decimal.Parse(defaultView[0]["SecondCommissionRise"].ToString());
                            }
                        }
                        DistributorsInfo info4 = dao.GetDistributorInfo(int.Parse(strArray[0]));
                        if (info4.DistriGradeId.ToString() != "0")
                        {
                            defaultView.RowFilter = " GradeId=" + info4.DistriGradeId;
                            if (defaultView.Count > 0)
                            {
                                num3 = decimal.Parse(defaultView[0]["ThirdCommissionRise"].ToString());
                            }
                        }
                    }
                }
                Dictionary<string, LineItemInfo> lineItems = order.LineItems;
                LineItemInfo info5 = new LineItemInfo();
                DataView view2 = new CategoryDao().GetCategories(order.wid).DefaultView;
                string str = null;
                string str2 = null;
                string str3 = null;
                foreach (KeyValuePair<string, LineItemInfo> pair in lineItems)
                {
                    info5 = pair.Value;
                    if (info5.Type == 0)
                    {
                        info5.ItemsCommission = num;
                        info5.SecondItemsCommission = num2;
                        info5.ThirdItemsCommission = num3;
                        decimal num4 = (info5.GetSubTotal() - info5.DiscountAverage) - info5.ItemAdjustedCommssion;
                        if (num4 > 0M)
                        {
                            if (info5.IsSetCommission)
                            {
                                info5.ItemsCommission = ((info5.FirstCommission + num) / 100M) * num4;
                                info5.SecondItemsCommission = ((info5.SecondCommission + num2) / 100M) * num4;
                                info5.ThirdItemsCommission = ((info5.ThirdCommission + num3) / 100M) * num4;
                            }
                            else
                            {
                                DataTable productCategories = new ProductDao().GetProductCategories(info5.ProductId);
                                if ((productCategories.Rows.Count > 0) && (productCategories.Rows[0][0].ToString() != "0"))
                                {
                                    view2.RowFilter = " CategoryId=" + productCategories.Rows[0][0];
                                    str = view2[0]["FirstCommission"].ToString();
                                    str2 = view2[0]["SecondCommission"].ToString();
                                    str3 = view2[0]["ThirdCommission"].ToString();
                                    if ((!string.IsNullOrEmpty(str) && !string.IsNullOrEmpty(str2)) && !string.IsNullOrEmpty(str3))
                                    {
                                        info5.ItemsCommission = ((decimal.Parse(str) + num) / 100M) * num4;
                                        info5.SecondItemsCommission = ((decimal.Parse(str2) + num2) / 100M) * num4;
                                        info5.ThirdItemsCommission = ((decimal.Parse(str3) + num3) / 100M) * num4;
                                    }
                                }
                            }
                        }
                        else
                        {
                            info5.ItemsCommission = 0M;
                            info5.SecondItemsCommission = 0M;
                            info5.ThirdItemsCommission = 0M;
                        }
                        if (!string.IsNullOrEmpty(distributorInfo.ReferralPath) && (distributorInfo.ReferralPath != "0"))
                        {
                            if (distributorInfo.ReferralPath.Split(new char[] { '|' }).Length == 1)
                            {
                                info5.ThirdItemsCommission = 0M;
                            }
                        }
                        else
                        {
                            info5.SecondItemsCommission = 0M;
                            info5.ThirdItemsCommission = 0M;
                        }
                    }
                }
            }
            return order;
        }

        public static int GetCountOrderIDByStatus(OrderStatus? orderstatus, OrderStatus? itemstatus,string wid)
        {
            return new OrderDao().GetCountOrderIDByStatus(orderstatus, itemstatus,wid);
        }

        private static string getEMSLastNum(string emsno)
        {
            List<char> list = emsno.ToList<char>();
            char ch = list[2];
            int num = int.Parse(ch.ToString()) * 8;
            char ch2 = list[3];
            num += int.Parse(ch2.ToString()) * 6;
            char ch3 = list[4];
            num += int.Parse(ch3.ToString()) * 4;
            char ch4 = list[5];
            num += int.Parse(ch4.ToString()) * 2;
            char ch5 = list[6];
            num += int.Parse(ch5.ToString()) * 3;
            char ch6 = list[7];
            num += int.Parse(ch6.ToString()) * 5;
            char ch7 = list[8];
            num += int.Parse(ch7.ToString()) * 9;
            char ch8 = list[9];
            num += int.Parse(ch8.ToString()) * 7;
            num = 11 - (num % 11);
            switch (num)
            {
                case 10:
                    num = 0;
                    break;

                case 11:
                    num = 5;
                    break;
            }
            return num.ToString();
        }

        private static string getEMSNext(string emsno)
        {
            long num = Convert.ToInt64(emsno.Substring(2, 8));
            if (num < 0x5f5e0ffL)
            {
                num += 1L;
            }
            string str = num.ToString().PadLeft(8, '0');
            string str2 = emsno.Substring(0, 2) + str + emsno.Substring(10, 1);
            return (emsno.Substring(0, 2) + str + getEMSLastNum(str2) + emsno.Substring(11, 2));
        }

        private static string GetNextExpress(string ExpressCom, string strno)
        {
            switch (ExpressCom.ToLower())
            {
                case "ems":
                    return getEMSNext(strno);

                case "顺丰快递":
                case "shunfeng":
                    return getSFNext(strno);

                case "宅急送":
                case "zhaijisong":
                    return getZJSNext(strno);
            }
            long num = long.Parse(strno) + 1L;
            return num.ToString();
        }

        public static DataSet GetOrderGoods(string orderIds)
        {
            return new OrderDao().GetOrderGoods(orderIds);
        }

        public static OrderInfo GetOrderInfo(string orderId)
        {
            return new OrderDao().GetOrderInfo(orderId);
        }

        public static DbQueryResult GetOrders(OrderQuery query)
        {
            return new OrderDao().GetOrders(query);
        }

        public static DataSet GetOrdersAndLines(string orderIds)
        {
            return new OrderDao().GetOrdersAndLines(orderIds);
        }

        public static DataSet GetOrdersByOrderIDList(string orderIds)
        {
            return new OrderDao().GetOrdersByOrderIDList(orderIds);
        }

        public static int GetPoint(decimal money,string wid)
        {
            if (money == 0M)
            {
                return 0;
            }
            int num = 0;
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(false,wid);
            if (!masterSettings.shopping_score_Enable)
            {
                return 0;
            }
            if (masterSettings.PointsRate != 0M)
            {
                num = (int)Math.Round((decimal)(money / masterSettings.PointsRate), 0);
                if (num > 0x7fffffff)
                {
                    return 0x7fffffff;
                }
            }
            if (masterSettings.shopping_reward_Enable && (money >= ((decimal)masterSettings.shopping_reward_OrderValue)))
            {
                num += masterSettings.shopping_reward_Score;
                if (num > 0x7fffffff)
                {
                    num = 0x7fffffff;
                }
            }
            return num;
        }

        public static DataSet GetProductGoods(string orderIds)
        {
            return new OrderDao().GetProductGoods(orderIds);
        }

        public static DataTable GetSendGoodsOrders(string orderIds)
        {
            return new OrderDao().GetSendGoodsOrders(orderIds);
        }

        private static string getSFNext(string sfno)
        {
            int[] numArray = new int[12];
            int[] numArray2 = new int[12];
            List<char> list = sfno.ToList<char>();
            string str = sfno.Substring(0, 11);
            string source = string.Empty;
            if (sfno.Substring(0, 1) == "0")
            {
                source = "0" + ((Convert.ToInt64(str) + 1L)).ToString();
            }
            else
            {
                source = (Convert.ToInt64(str) + 1L).ToString();
            }
            for (int i = 0; i < 12; i++)
            {
                numArray[i] = int.Parse(list[i].ToString());
            }
            source.ToList<char>();
            for (int j = 0; j < 11; j++)
            {
                numArray2[j] = int.Parse(source[j].ToString());
            }
            if (((numArray2[8] - numArray[8]) == 1) && ((numArray[8] % 2) == 1))
            {
                if ((numArray[11] - 8) >= 0)
                {
                    numArray2[11] = numArray[11] - 8;
                }
                else
                {
                    numArray2[11] = (numArray[11] - 8) + 10;
                }
            }
            else if (((numArray2[8] - numArray[8]) == 1) && ((numArray[8] % 2) == 0))
            {
                if ((numArray[11] - 7) >= 0)
                {
                    numArray2[11] = numArray[11] - 7;
                }
                else
                {
                    numArray2[11] = (numArray[11] - 7) + 10;
                }
            }
            else if (((numArray[9] == 3) || (numArray[9] == 6)) && (numArray[10] == 9))
            {
                if ((numArray[11] - 5) >= 0)
                {
                    numArray2[11] = numArray[11] - 5;
                }
                else
                {
                    numArray2[11] = (numArray[11] - 5) + 10;
                }
            }
            else if (numArray[10] == 9)
            {
                if ((numArray[11] - 4) >= 0)
                {
                    numArray2[11] = numArray[11] - 4;
                }
                else
                {
                    numArray2[11] = (numArray[11] - 4) + 10;
                }
            }
            else if ((numArray[11] - 1) >= 0)
            {
                numArray2[11] = numArray[11] - 1;
            }
            else
            {
                numArray2[11] = (numArray[11] - 1) + 10;
            }
            return (source + numArray2[11].ToString());
        }

        public static int GetSkuStock(string skuId)
        {
            return new SkuDao().GetSkuItem(skuId).Stock;
        }

        private static string getZJSNext(string zjsno)
        {
            long num = Convert.ToInt64(zjsno) + 11L;
            if ((num % 10L) > 6L)
            {
                num -= 7L;
            }
            return num.ToString().PadLeft(zjsno.Length, '0');
        }

        public static bool MondifyAddress(OrderInfo order)
        {
            ManagerHelper.CheckPrivilege(Privilege.EditOrders);
            if (!order.CheckAction(OrderActions.MASTER_SELLER_MODIFY_DELIVER_ADDRESS))
            {
                return false;
            }
            bool flag = new OrderDao().UpdateOrder(order, null);
            if (flag)
            {
                EventLogs.WriteOperationLog(Privilege.EditOrders, string.Format(CultureInfo.InvariantCulture, "修改了订单“{0}”的收货地址", new object[] { order.OrderId }));
            }
            return flag;
        }

        private static void ReducedPoint(OrderInfo order, MemberInfo member)
        {
            PointDetailInfo info = new PointDetailInfo();
            info.OrderId = order.OrderId;
            info.UserId = member.UserId;
            info.TradeDate = DateTime.Now;
            info.TradeType = PointTradeType.Refund;
            info.Reduced = new int?(order.Points);
            info.Points = member.Points - info.Reduced.Value;
            new PointDetailDao().AddPointDetail(info);
        }

        public static bool SaveDebitNote(DebitNoteInfo note)
        {
            return new DebitNoteDao().SaveDebitNote(note);
        }

        public static bool SaveRemark(OrderInfo order)
        {
            ManagerHelper.CheckPrivilege(Privilege.RemarkOrder);
            bool flag = new OrderDao().UpdateOrder(order, null);
            if (flag)
            {
                EventLogs.WriteOperationLog(Privilege.RemarkOrder, string.Format(CultureInfo.InvariantCulture, "对订单“{0}”进行了备注", new object[] { order.OrderId }));
            }
            return flag;
        }

        public static bool SaveSendNote(SendNoteInfo note)
        {
            return new SendNoteDao().SaveSendNote(note);
        }

        public static bool SendGoods(OrderInfo order)
        {
            ManagerHelper.CheckPrivilege(Privilege.OrderSendGoods);
            bool flag = false;
            if (order.CheckAction(OrderActions.SELLER_SEND_GOODS))
            {
                OrderDao dao = new OrderDao();
                order.OrderStatus = OrderStatus.SellerAlreadySent;
                order.ShippingDate = new DateTime?(DateTime.Now);
                //线下支付，发货时更新付款时间
                if(order.Gateway== "hishop.plugins.payment.offlinerequest")
                {
                    order.PayDate = new DateTime?(DateTime.Now);
                }
                flag = dao.UpdateOrder(order, null);
                string str = "";
                if (!flag)
                {
                    return flag;
                }
                bool flag2 = false;
                foreach (LineItemInfo info in order.LineItems.Values)
                {
                    OrderStatus orderItemsStatus = info.OrderItemsStatus;
                    switch (orderItemsStatus)
                    {
                        case OrderStatus.WaitBuyerPay:
                        case OrderStatus.BuyerAlreadyPaid:
                            break;

                        default:
                            {
                                if (orderItemsStatus == OrderStatus.ApplyForRefund)
                                {
                                    flag2 = true;
                                    str = str + "'" + info.SkuId + "',";
                                }
                                continue;
                            }
                    }
                    str = str + "'" + info.SkuId + "',";
                }
                if (flag2)
                {
                    dao.DeleteReturnRecordForSendGoods(order.OrderId);
                }
                if (!string.IsNullOrEmpty(str))
                {
                    dao.UpdateItemsStatus(order.OrderId, 3, str.Substring(0, str.Length - 1));
                }
                //货到付款 更新库存
                if (order.Gateway.ToLower() == "hishop.plugins.payment.podrequest")
                {
                    dao.UpdatePayOrderStock(order.OrderId);
                    foreach (LineItemInfo info2 in order.LineItems.Values)
                    {
                        str = str + info2.SkuId + ",";
                        ProductDao dao2 = new ProductDao();
                        ProductInfo productDetails = dao2.GetProductDetails(info2.ProductId);
                        productDetails.SaleCounts += info2.Quantity;
                        productDetails.ShowSaleCounts += info2.Quantity;
                        dao2.UpdateProduct(productDetails, null);
                    }
                }
                MemberInfo member = MemberHelper.GetMember(order.UserId);
                Messenger.OrderShipping(order, member,order.wid);
                EventLogs.WriteOperationLog(Privilege.OrderSendGoods, string.Format(CultureInfo.InvariantCulture, "发货编号为\"{0}\"的订单", new object[] { order.OrderId }));
            }
            return flag;
        }

        public static bool SetOrderExpressComputerpe(string purchaseOrderIds, string expressCompanyName, string expressCompanyAbb)
        {
            return new OrderDao().SetOrderExpressComputerpe(purchaseOrderIds, expressCompanyName, expressCompanyAbb);
        }

        public static void SetOrderPrinted(string[] orderIds)
        {
            foreach (string str in orderIds)
            {
                OrderInfo orderInfo = new OrderDao().GetOrderInfo(str);
                orderInfo.IsPrinted = true;
                new OrderDao().UpdateOrder(orderInfo, null);
            }
        }

        public static bool SetOrderShipNumber(string orderId, string startNumber)
        {
            OrderInfo orderInfo = new OrderDao().GetOrderInfo(orderId);
            orderInfo.ShipOrderNumber = startNumber;
            return new OrderDao().UpdateOrder(orderInfo, null);
        }

        public static void SetOrderShipNumber(string[] orderIds, string startNumber, string ExpressCom = "")
        {
            string strno = startNumber;
            OrderDao dao = new OrderDao();
            for (int i = 0; i < orderIds.Length; i++)
            {
                if (i != 0)
                {
                    strno = GetNextExpress(ExpressCom, strno);
                }
                else
                {
                    GetNextExpress(ExpressCom, strno);
                }
                dao.EditOrderShipNumber(orderIds[i], strno);
            }
        }

        public static bool SetOrderShippingMode(string orderIds, int realShippingModeId, string realModeName)
        {
            return new OrderDao().SetOrderShippingMode(orderIds, realShippingModeId, realModeName);
        }

        public static void SetOrderState(string orderId, OrderStatus orderStatus)
        {
            OrderDao dao = new OrderDao();
            OrderInfo orderInfo = dao.GetOrderInfo(orderId);
            orderInfo.OrderStatus = orderStatus;
            dao.UpdateOrder(orderInfo, null);
        }

        public static bool SetPrintOrderExpress(string orderId, string expressCompanyName, string expressCompanyAbb, string shipOrderNumber)
        {
            return new OrderDao().SetPrintOrderExpress(orderId, expressCompanyName, expressCompanyAbb, shipOrderNumber);
        }

        public static bool UpdateAdjustCommssions(string orderId, string skuId, decimal adjustcommssion)
        {
            bool flag = false;
            using (DbConnection connection = DatabaseFactory.CreateDatabase().CreateConnection())
            {
                connection.Open();
                DbTransaction dbTran = connection.BeginTransaction();
                try
                {
                    OrderInfo orderInfo = GetOrderInfo(orderId);
                    if (orderInfo == null)
                    {
                        return false;
                    }
                    LineItemInfo lineItem = orderInfo.LineItems[skuId];
                    lineItem.ItemAdjustedCommssion = adjustcommssion;
                    if (!new LineItemDao().UpdateLineItem(orderId, lineItem, dbTran))
                    {
                        dbTran.Rollback();
                    }
                    if (!new OrderDao().UpdateOrder(orderInfo, dbTran))
                    {
                        dbTran.Rollback();
                        return false;
                    }
                    dbTran.Commit();
                    flag = true;
                }
                catch (Exception)
                {
                    dbTran.Rollback();
                }
                finally
                {
                    connection.Close();
                }
                return flag;
            }
        }

        public static bool UpdateCalculadtionCommission(string orderid)
        {
            OrderInfo calculadtionCommission = GetCalculadtionCommission(GetOrderInfo(orderid));
            new OrderDao().UpdateOrder(calculadtionCommission, null);
            return new OrderDao().UpdateCalculadtionCommission(calculadtionCommission, null);
        }

        public static bool UpdateLineItem(string sku, OrderInfo order, int quantity)
        {
            bool flag;
            ManagerHelper.CheckPrivilege(Privilege.EditOrders);
            using (DbConnection connection = DatabaseFactory.CreateDatabase().CreateConnection())
            {
                connection.Open();
                DbTransaction dbTran = connection.BeginTransaction();
                try
                {
                    order.LineItems[sku].Quantity = quantity;
                    order.LineItems[sku].ShipmentQuantity = quantity;
                    order.LineItems[sku].ItemAdjustedPrice = order.LineItems[sku].ItemListPrice;
                    if (!new LineItemDao().UpdateLineItem(order.OrderId, order.LineItems[sku], dbTran))
                    {
                        dbTran.Rollback();
                        return false;
                    }
                    if (!new OrderDao().UpdateOrder(order, dbTran))
                    {
                        dbTran.Rollback();
                        return false;
                    }
                    dbTran.Commit();
                    flag = true;
                }
                catch (Exception)
                {
                    dbTran.Rollback();
                    flag = false;
                }
                finally
                {
                    connection.Close();
                }
            }
            if (flag)
            {
                EventLogs.WriteOperationLog(Privilege.EditOrders, string.Format(CultureInfo.InvariantCulture, "修改了订单号为\"{0}\"的订单商品数量", new object[] { order.OrderId }));
            }
            return flag;
        }

        public static bool UpdateOrder(OrderInfo order)
        {
            return new OrderDao().UpdateOrder(order, null);
        }

        public static bool UpdateOrderAmount(OrderInfo order)
        {
            ManagerHelper.CheckPrivilege(Privilege.EditOrders);
            bool flag = false;
            if (order.CheckAction(OrderActions.SELLER_MODIFY_TRADE))
            {
                flag = new OrderDao().UpdateOrder(order, null);
                if (flag)
                {
                    EventLogs.WriteOperationLog(Privilege.EditOrders, string.Format(CultureInfo.InvariantCulture, "修改了编号为\"{0}\"订单的金额", new object[] { order.OrderId }));
                }
            }
            return flag;
        }

        public static bool UpdateOrderCompany(string orderId, string companycode, string companyname, string shipNumber)
        {
            return new OrderDao().UpdateOrderCompany(orderId, companycode, companyname, shipNumber);
        }

        public static bool UpdateOrderPaymentType(OrderInfo order)
        {
            ManagerHelper.CheckPrivilege(Privilege.EditOrders);
            if (!order.CheckAction(OrderActions.MASTER_SELLER_MODIFY_PAYMENT_MODE))
            {
                return false;
            }
            bool flag = new OrderDao().UpdateOrder(order, null);
            if (flag)
            {
                EventLogs.WriteOperationLog(Privilege.EditOrders, string.Format(CultureInfo.InvariantCulture, "修改了订单“{0}”的支付方式", new object[] { order.OrderId }));
            }
            return flag;
        }

        public static bool UpdateOrderShippingMode(OrderInfo order)
        {
            ManagerHelper.CheckPrivilege(Privilege.EditOrders);
            if (!order.CheckAction(OrderActions.MASTER_SELLER_MODIFY_SHIPPING_MODE))
            {
                return false;
            }
            bool flag = new OrderDao().UpdateOrder(order, null);
            if (flag)
            {
                EventLogs.WriteOperationLog(Privilege.EditOrders, string.Format(CultureInfo.InvariantCulture, "修改了订单“{0}”的配送方式", new object[] { order.OrderId }));
            }
            return flag;
        }
    }
}

