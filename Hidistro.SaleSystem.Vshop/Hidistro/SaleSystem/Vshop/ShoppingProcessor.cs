namespace Hidistro.SaleSystem.Vshop
{
    using Hidistro.Core;
    using Hidistro.Entities;
    using Hidistro.Entities.Commodities;
    using Hidistro.Entities.Members;
    using Hidistro.Entities.Orders;
    using Hidistro.Entities.Promotions;
    using Hidistro.Entities.Sales;
    using Hidistro.SqlDal;
    using Hidistro.SqlDal.Commodities;
    using Hidistro.SqlDal.Members;
    using Hidistro.SqlDal.Orders;
    using Hidistro.SqlDal.Promotions;
    using Hidistro.SqlDal.Sales;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Linq;

    public static class ShoppingProcessor
    {
        private static object createOrderLocker = new object();

        public static decimal CalcFreight(int regionId, decimal totalWeight, ShippingModeInfo shippingModeInfo)
        {
            decimal price = 0M;
            int topRegionId = RegionHelper.GetTopRegionId(regionId);
            decimal num3 = totalWeight;
            int num4 = 1;
            if (((num3 > shippingModeInfo.Weight) && shippingModeInfo.AddWeight.HasValue) && (shippingModeInfo.AddWeight.Value > 0M))
            {
                decimal num5 = num3 - shippingModeInfo.Weight;
                if ((num5 % shippingModeInfo.AddWeight) == 0M)
                {
                    num4 = Convert.ToInt32(Math.Truncate((decimal)((num3 - shippingModeInfo.Weight) / shippingModeInfo.AddWeight.Value)));
                }
                else
                {
                    num4 = Convert.ToInt32(Math.Truncate((decimal)((num3 - shippingModeInfo.Weight) / shippingModeInfo.AddWeight.Value))) + 1;
                }
            }
            if ((shippingModeInfo.ModeGroup == null) || (shippingModeInfo.ModeGroup.Count == 0))
            {
                if ((num3 > shippingModeInfo.Weight) && shippingModeInfo.AddPrice.HasValue)
                {
                    return ((num4 * shippingModeInfo.AddPrice.Value) + shippingModeInfo.Price);
                }
                return shippingModeInfo.Price;
            }
            int? nullable = null;
            foreach (ShippingModeGroupInfo info in shippingModeInfo.ModeGroup)
            {
                foreach (ShippingRegionInfo info2 in info.ModeRegions)
                {
                    if (topRegionId == info2.RegionId)
                    {
                        nullable = new int?(info2.GroupId);
                        break;
                    }
                }
                if (nullable.HasValue)
                {
                    if (num3 > shippingModeInfo.Weight)
                    {
                        price = (num4 * info.AddPrice) + info.Price;
                    }
                    else
                    {
                        price = info.Price;
                    }
                    break;
                }
            }
            if (nullable.HasValue)
            {
                return price;
            }
            if ((num3 > shippingModeInfo.Weight) && shippingModeInfo.AddPrice.HasValue)
            {
                return ((num4 * shippingModeInfo.AddPrice.Value) + shippingModeInfo.Price);
            }
            return shippingModeInfo.Price;
        }

        public static decimal CalcPayCharge(decimal cartMoney, PaymentModeInfo paymentModeInfo)
        {
            if (!paymentModeInfo.IsPercent)
            {
                return paymentModeInfo.Charge;
            }
            return (cartMoney * (paymentModeInfo.Charge / 100M));
        }

        private static void checkCanGroupBuy(int quantity, int groupBuyId)
        {
            GroupBuyInfo groupBuy = GroupBuyBrowser.GetGroupBuy(groupBuyId);
            if (groupBuy.Status != GroupBuyStatus.UnderWay)
            {
                throw new OrderException("当前团购状态不允许购买");
            }
            if ((groupBuy.StartDate > DateTime.Now) || (groupBuy.EndDate < DateTime.Now))
            {
                throw new OrderException("当前不在团购时间范围内");
            }
            int num = groupBuy.MaxCount - groupBuy.SoldCount;
            if (quantity > num)
            {
                throw new OrderException("剩余可购买团购数量不够");
            }
        }

        public static OrderInfo ConvertShoppingCartToOrder(ShoppingCartInfo shoppingCart, bool isCountDown, bool isSignBuy,string wid)
        {
            if (shoppingCart.LineItems.Count == 0)
            {
                return null;
            }
            OrderInfo info = new OrderInfo
            {
                Points = shoppingCart.GetPoint(wid),
                ReducedPromotionId = shoppingCart.ReducedPromotionId,
                ReducedPromotionName = shoppingCart.ReducedPromotionName,
                ReducedPromotionAmount = shoppingCart.ReducedPromotionAmount,
                IsReduced = shoppingCart.IsReduced,
                SentTimesPointPromotionId = shoppingCart.SentTimesPointPromotionId,
                SentTimesPointPromotionName = shoppingCart.SentTimesPointPromotionName,
                IsSendTimesPoint = shoppingCart.IsSendTimesPoint,
                TimesPoint = shoppingCart.TimesPoint,
                FreightFreePromotionId = shoppingCart.FreightFreePromotionId,
                FreightFreePromotionName = shoppingCart.FreightFreePromotionName,
                IsFreightFree = shoppingCart.IsFreightFree
            };
            string str = string.Empty;
            if (shoppingCart.LineItems.Count > 0)
            {
                foreach (ShoppingCartItemInfo info2 in shoppingCart.LineItems)
                {
                    str = str + string.Format("'{0}',", info2.SkuId);
                }
            }
            if (shoppingCart.LineItems.Count > 0)
            {
                foreach (ShoppingCartItemInfo info3 in shoppingCart.LineItems)
                {
                    LineItemInfo info4 = new LineItemInfo();

                    info4.SkuId = info3.SkuId;
                    info4.ProductId = info3.ProductId;
                    info4.SKU = info3.SKU;
                    info4.Quantity = info3.Quantity;
                    info4.ShipmentQuantity = info3.ShippQuantity;
                    info4.ItemCostPrice = new SkuDao().GetSkuItem(info3.SkuId).CostPrice;
                    info4.ItemListPrice = info3.MemberPrice;
                    info4.ItemAdjustedPrice = info3.AdjustedPrice;
                    info4.ItemDescription = info3.Name;
                    info4.ThumbnailsUrl = info3.ThumbnailUrl60;
                    info4.ItemWeight = info3.Weight;
                    info4.SKUContent = info3.SkuContent;
                    info4.PromotionId = info3.PromotionId;
                    info4.PromotionName = info3.PromotionName;
                    info4.MainCategoryPath = info3.MainCategoryPath;
                    info4.Type = info3.Type;
                    info4.ExchangeId = info3.ExchangeId;
                    info4.PointNumber = info3.PointNumber * info4.Quantity;
                    info4.ThirdCommission = info3.ThirdCommission;
                    info4.SecondCommission = info3.SecondCommission;
                    info4.FirstCommission = info3.FirstCommission;
                    info4.IsSetCommission = info3.IsSetCommission;
                    info.LineItems.Add(info4.SkuId + info4.Type, info4);
                }
            }
            info.Tax = 0.00M;
            info.InvoiceTitle = "";
            return info;
        }

        public static bool CreatOrder(OrderInfo orderInfo)
        {
            bool flag = false;
            Database database = DatabaseFactory.CreateDatabase();
            int quantity = orderInfo.LineItems.Sum<KeyValuePair<string, LineItemInfo>>((Func<KeyValuePair<string, LineItemInfo>, int>)(item => item.Value.Quantity));
            lock (createOrderLocker)
            {
                if (orderInfo.GroupBuyId > 0)
                {
                    checkCanGroupBuy(quantity, orderInfo.GroupBuyId);
                }
                using (DbConnection connection = database.CreateConnection())
                {
                    connection.Open();
                    DbTransaction dbTran = connection.BeginTransaction();
                    try
                    {
                        try
                        {
                            if (!new OrderDao().CreatOrder(orderInfo, dbTran))
                            {
                                dbTran.Rollback();
                                return false;
                            }
                            if ((orderInfo.LineItems.Count > 0) && !new LineItemDao().AddOrderLineItems(orderInfo.OrderId, orderInfo.LineItems.Values, dbTran))
                            {
                                dbTran.Rollback();
                                return false;
                            }
                            if (!string.IsNullOrEmpty(orderInfo.CouponCode) && !new CouponDao().AddCouponUseRecord(orderInfo, dbTran))
                            {
                                dbTran.Rollback();
                                return false;
                            }
                            ICollection values = orderInfo.LineItems.Values;
                            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
                            foreach (LineItemInfo info2 in values)
                            {
                                if ((info2.Type == 1) && (info2.ExchangeId > 0))
                                {
                                    PointExchangeChangedInfo info3 = new PointExchangeChangedInfo();
                                    info3.exChangeId = info2.ExchangeId;
                                    info3.exChangeName = new OrderDao().GetexChangeName(info3.exChangeId);
                                    info3.ProductId = info2.ProductId;
                                    info3.PointNumber = info2.PointNumber;
                                    info3.MemberID = orderInfo.UserId;
                                    info3.Date = DateTime.Now;
                                    info3.MemberGrades = currentMember.GradeId;

                                    if (!new OrderDao().InsertPointExchange_Changed(info3, dbTran, info2.Quantity))
                                    {
                                        dbTran.Rollback();
                                        return false;
                                    }
                                    IntegralDetailInfo point = new IntegralDetailInfo
                                    {
                                        IntegralChange = -info2.PointNumber,
                                        IntegralSource = "积分兑换商品-订单号：" + orderInfo.OrderMarking,
                                        IntegralSourceType = 2,
                                        Remark = "积分兑换商品",
                                        Userid = orderInfo.UserId,
                                        GoToUrl = Globals.ApplicationPath + "/Vshop/MemberOrderDetails.aspx?OrderId=" + orderInfo.OrderId,
                                        IntegralStatus = Convert.ToInt32(IntegralDetailStatus.IntegralExchange)
                                    };
                                    if (!new IntegralDetailDao().AddIntegralDetail(point, dbTran))
                                    {
                                        dbTran.Rollback();
                                        return false;
                                    }
                                }
                            }
                            if (orderInfo.PointExchange > 0)
                            {
                                IntegralDetailInfo info5 = new IntegralDetailInfo
                                {
                                    IntegralChange = -orderInfo.PointExchange,
                                    IntegralSource = "积分抵现-订单号：" + orderInfo.OrderMarking,
                                    IntegralSourceType = 2,
                                    Remark = "积分抵现",
                                    Userid = orderInfo.UserId,
                                    GoToUrl = Globals.ApplicationPath + "/Vshop/MemberOrderDetails.aspx?OrderId=" + orderInfo.OrderId,
                                    IntegralStatus = Convert.ToInt32(IntegralDetailStatus.NowArrived)
                                };
                                if (!new IntegralDetailDao().AddIntegralDetail(info5, dbTran))
                                {
                                    dbTran.Rollback();
                                    return false;
                                }
                            }
                            if ((orderInfo.RedPagerID > 0) && !new OrderDao().UpdateCoupon_MemberCoupons(orderInfo, dbTran))
                            {
                                dbTran.Rollback();
                                return false;
                            }
                            if (orderInfo.GroupBuyId > 0)
                            {
                                GroupBuyDao dao = new GroupBuyDao();
                                GroupBuyInfo groupBuy = dao.GetGroupBuy(orderInfo.GroupBuyId, dbTran);
                                groupBuy.SoldCount += quantity;
                                dao.UpdateGroupBuy(groupBuy, dbTran);
                                dao.RefreshGroupBuyFinishState(orderInfo.GroupBuyId, dbTran);
                            }
                            dbTran.Commit();
                            flag = true;
                        }
                        catch
                        {
                            dbTran.Rollback();
                            throw;
                        }
                        return flag;
                    }
                    finally
                    {
                        connection.Close();
                    }
                    return flag;
                }
            }
            return flag;
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
            return order;
        }

        public static DataTable GetCoupon(decimal orderAmount)
        {
            return null;
        }

        public static CouponInfo GetCoupon(string couponCode)
        {
            return new CouponDao().GetCouponDetails(int.Parse(couponCode));
        }

        public static OrderInfo GetOrderInfo(string orderId)
        {
            return new OrderDao().GetOrderInfoForLineItems(orderId);
        }

        public static List<OrderInfo> GetOrderMarkingOrderInfo(string OrderMarking)
        {
            List<OrderInfo> list = new List<OrderInfo>();
            DataTable orderMarkingAllOrderID = new OrderDao().GetOrderMarkingAllOrderID(OrderMarking);
            for (int i = 0; i < orderMarkingAllOrderID.Rows.Count; i++)
            {
                list.Add(new OrderDao().GetOrderInfo(orderMarkingAllOrderID.Rows[i]["OrderId"].ToString()));
            }
            return list;
        }

        public static DataTable GetOrderReturnTable(int userid, string ReturnsId)
        {
            return new RefundDao().GetOrderReturnTable(userid, ReturnsId);
        }

        public static PaymentModeInfo GetPaymentMode(int modeId)
        {
            return new PaymentModeDao().GetPaymentMode(modeId);
        }

        public static IList<PaymentModeInfo> GetPaymentModes(string wid)
        {
            return new PaymentModeDao().GetPaymentModes(wid);
        }

        public static SKUItem GetProductAndSku(MemberInfo member, int productId, string options)
        {
            return new SkuDao().GetProductAndSku(member, productId, options);
        }

        public static bool GetReturnInfo(int userid, string OrderId, int ProductId)
        {
            return new RefundDao().GetReturnInfo(userid, OrderId, ProductId);
        }

        public static bool GetReturnMes(int userid, string OrderId, int ProductId, int HandleStatus)
        {
            return new RefundDao().GetReturnMes(userid, OrderId, ProductId, HandleStatus);
        }

        public static ShippingModeInfo GetShippingMode(int modeId, bool includeDetail)
        {
            return new ShippingModeDao().GetShippingMode(modeId, includeDetail);
        }

        public static IList<ShippingModeInfo> GetShippingModes()
        {
            return new ShippingModeDao().GetShippingModes();
        }

        public static int GetUserOrders(int userId)
        {
            return new OrderDao().GetUserOrders(userId);
        }

        public static bool InsertCalculationCommission(ArrayList UserIdList, ArrayList ReferralBlanceList, string orderid, ArrayList OrdersTotalList, string userid)
        {
            return new OrderDao().InsertCalculationCommission(UserIdList, ReferralBlanceList, orderid, OrdersTotalList, userid);
        }

        public static bool InsertOrderRefund(RefundInfo refundInfo)
        {
            return new RefundDao().InsertOrderRefund(refundInfo);
        }

        public static bool UpdateAdjustCommssions(string orderId, string skuId, decimal commssionmoney, decimal adjustcommssion)
        {
            bool flag = false;
            using (DbConnection connection = DatabaseFactory.CreateDatabase().CreateConnection())
            {
                connection.Open();
                DbTransaction dbTran = connection.BeginTransaction();
                try
                {
                    OrderInfo orderInfo = GetOrderInfo(orderId);
                    if (orderId == null)
                    {
                        return false;
                    }
                    int userId = DistributorsBrower.GetCurrentDistributors().UserId;
                    if ((orderInfo.ReferralUserId != userId) || (orderInfo.OrderStatus != OrderStatus.WaitBuyerPay))
                    {
                        return false;
                    }
                    LineItemInfo lineItem = orderInfo.LineItems[skuId];
                    if ((lineItem == null) || (lineItem.ItemsCommission < adjustcommssion))
                    {
                        return false;
                    }
                    lineItem.ItemAdjustedCommssion = adjustcommssion;
                    lineItem.IsAdminModify = false;
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

        public static bool UpdateCalculadtionCommission(OrderInfo order)
        {
            return new OrderDao().UpdateCalculadtionCommission(order, null);
        }

        public static bool UpdateOrderGoodStatu(string orderid, string skuid, int OrderItemsStatus)
        {
            return new RefundDao().UpdateOrderGoodStatu(orderid, skuid, OrderItemsStatus);
        }

        public static CouponInfo UseCoupon(decimal orderAmount, string claimCode)
        {
            if (!string.IsNullOrEmpty(claimCode))
            {
                CouponInfo coupon = GetCoupon(claimCode);
                if (coupon.ConditionValue <= orderAmount)
                {
                    return coupon;
                }
            }
            return null;
        }
    }
}

