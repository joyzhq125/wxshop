namespace Hidistro.SqlDal.Orders
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities;
    using Hidistro.Entities.Members;
    using Hidistro.Entities.Orders;
    using Hidistro.Entities.Promotions;
    using Hidistro.SqlDal.Members;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Collections;
    using System.Data;
    using System.Data.Common;
    using System.Runtime.InteropServices;
    using System.Text;

    public class OrderDao
    {
        private Database database = DatabaseFactory.CreateDatabase();

        public bool AddMemberPointNumber(int PointNumber, OrderInfo orderInfo, DbTransaction dbTran)
        {
            IntegralDetailInfo point = new IntegralDetailInfo {
                IntegralChange = PointNumber,
                IntegralSource = "获取积分-订单号：" + orderInfo.OrderId,
                IntegralSourceType = 1,
                Remark = "购物获取积分",
                Userid = orderInfo.UserId,
                GoToUrl = Globals.ApplicationPath + "/Vshop/MemberOrderDetails.aspx?OrderId=" + orderInfo.OrderId,
                IntegralStatus = Convert.ToInt32(IntegralDetailStatus.OrderToIntegral)
            };
            if (!new IntegralDetailDao().AddIntegralDetail(point, dbTran))
            {
                dbTran.Rollback();
                return false;
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("update aspnet_Members set Points=Points+@PointNumber WHERE UserId=@UserId");
            this.database.AddInParameter(sqlStringCommand, "PointNumber", DbType.Int32, PointNumber);
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, orderInfo.UserId);
            if (dbTran != null)
            {
                return (this.database.ExecuteNonQuery(sqlStringCommand, dbTran) > 0);
            }
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool CheckRefund(string orderId, string Operator, string adminRemark, int refundType, bool accept)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("UPDATE Hishop_Orders SET OrderStatus = @OrderStatus WHERE OrderId = @OrderId;");
            builder.Append(" update Hishop_OrderRefund set Operator=@Operator,AdminRemark=@AdminRemark,HandleStatus=@HandleStatus,HandleTime=@HandleTime where HandleStatus=0 and OrderId = @OrderId;");
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            if (accept)
            {
                this.database.AddInParameter(sqlStringCommand, "OrderStatus", DbType.Int32, 9);
                this.database.AddInParameter(sqlStringCommand, "HandleStatus", DbType.Int32, 1);
            }
            else
            {
                this.database.AddInParameter(sqlStringCommand, "OrderStatus", DbType.Int32, 2);
                this.database.AddInParameter(sqlStringCommand, "HandleStatus", DbType.Int32, 2);
            }
            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
            this.database.AddInParameter(sqlStringCommand, "Operator", DbType.String, Operator);
            this.database.AddInParameter(sqlStringCommand, "AdminRemark", DbType.String, adminRemark);
            this.database.AddInParameter(sqlStringCommand, "HandleTime", DbType.DateTime, DateTime.Now);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool CreatOrder(OrderInfo orderInfo, DbTransaction dbTran)
        {
            DbCommand storedProcCommand = this.database.GetStoredProcCommand("ss_CreateOrder");
            this.database.AddInParameter(storedProcCommand, "OrderId", DbType.String, orderInfo.OrderId);
            this.database.AddInParameter(storedProcCommand, "OrderMarking", DbType.String, orderInfo.OrderMarking);
            this.database.AddInParameter(storedProcCommand, "OrderDate", DbType.DateTime, orderInfo.OrderDate);
            this.database.AddInParameter(storedProcCommand, "UserId", DbType.Int32, orderInfo.UserId);
            this.database.AddInParameter(storedProcCommand, "UserName", DbType.String, orderInfo.Username);
            this.database.AddInParameter(storedProcCommand, "Wangwang", DbType.String, orderInfo.Wangwang);
            this.database.AddInParameter(storedProcCommand, "RealName", DbType.String, orderInfo.RealName);
            this.database.AddInParameter(storedProcCommand, "EmailAddress", DbType.String, orderInfo.EmailAddress);
            this.database.AddInParameter(storedProcCommand, "Remark", DbType.String, orderInfo.Remark);
            this.database.AddInParameter(storedProcCommand, "AdjustedDiscount", DbType.Currency, orderInfo.AdjustedDiscount);
            this.database.AddInParameter(storedProcCommand, "OrderStatus", DbType.Int32, (int) orderInfo.OrderStatus);
            this.database.AddInParameter(storedProcCommand, "ShippingRegion", DbType.String, orderInfo.ShippingRegion);
            this.database.AddInParameter(storedProcCommand, "Address", DbType.String, orderInfo.Address);
            this.database.AddInParameter(storedProcCommand, "ZipCode", DbType.String, orderInfo.ZipCode);
            this.database.AddInParameter(storedProcCommand, "ShipTo", DbType.String, orderInfo.ShipTo);
            this.database.AddInParameter(storedProcCommand, "TelPhone", DbType.String, orderInfo.TelPhone);
            this.database.AddInParameter(storedProcCommand, "CellPhone", DbType.String, orderInfo.CellPhone);
            this.database.AddInParameter(storedProcCommand, "ShipToDate", DbType.String, orderInfo.ShipToDate);
            this.database.AddInParameter(storedProcCommand, "ShippingModeId", DbType.Int32, orderInfo.ShippingModeId);
            this.database.AddInParameter(storedProcCommand, "ModeName", DbType.String, orderInfo.ModeName);
            this.database.AddInParameter(storedProcCommand, "RegionId", DbType.Int32, orderInfo.RegionId);
            this.database.AddInParameter(storedProcCommand, "Freight", DbType.Currency, orderInfo.Freight);
            this.database.AddInParameter(storedProcCommand, "AdjustedFreight", DbType.Currency, orderInfo.AdjustedFreight);
            this.database.AddInParameter(storedProcCommand, "ShipOrderNumber", DbType.String, orderInfo.ShipOrderNumber);
            this.database.AddInParameter(storedProcCommand, "Weight", DbType.Int32, orderInfo.Weight);
            this.database.AddInParameter(storedProcCommand, "ExpressCompanyName", DbType.String, orderInfo.ExpressCompanyName);
            this.database.AddInParameter(storedProcCommand, "ExpressCompanyAbb", DbType.String, orderInfo.ExpressCompanyAbb);
            this.database.AddInParameter(storedProcCommand, "PaymentTypeId", DbType.Int32, orderInfo.PaymentTypeId);
            this.database.AddInParameter(storedProcCommand, "PaymentType", DbType.String, orderInfo.PaymentType);
            this.database.AddInParameter(storedProcCommand, "PayCharge", DbType.Currency, orderInfo.PayCharge);
            this.database.AddInParameter(storedProcCommand, "RefundStatus", DbType.Int32, (int) orderInfo.RefundStatus);
            this.database.AddInParameter(storedProcCommand, "Gateway", DbType.String, orderInfo.Gateway);
            this.database.AddInParameter(storedProcCommand, "OrderTotal", DbType.Currency, orderInfo.GetTotal());
            this.database.AddInParameter(storedProcCommand, "OrderPoint", DbType.Int32, orderInfo.Points);
            this.database.AddInParameter(storedProcCommand, "OrderCostPrice", DbType.Currency, orderInfo.GetCostPrice());
            this.database.AddInParameter(storedProcCommand, "OrderProfit", DbType.Currency, orderInfo.GetProfit());
            this.database.AddInParameter(storedProcCommand, "Amount", DbType.Currency, orderInfo.GetAmount());
            this.database.AddInParameter(storedProcCommand, "ReducedPromotionId", DbType.Int32, orderInfo.ReducedPromotionId);
            this.database.AddInParameter(storedProcCommand, "ReducedPromotionName", DbType.String, orderInfo.ReducedPromotionName);
            this.database.AddInParameter(storedProcCommand, "ReducedPromotionAmount", DbType.Currency, orderInfo.ReducedPromotionAmount);
            this.database.AddInParameter(storedProcCommand, "IsReduced", DbType.Boolean, orderInfo.IsReduced);
            this.database.AddInParameter(storedProcCommand, "SentTimesPointPromotionId", DbType.Int32, orderInfo.SentTimesPointPromotionId);
            this.database.AddInParameter(storedProcCommand, "SentTimesPointPromotionName", DbType.String, orderInfo.SentTimesPointPromotionName);
            this.database.AddInParameter(storedProcCommand, "TimesPoint", DbType.Currency, orderInfo.TimesPoint);
            this.database.AddInParameter(storedProcCommand, "IsSendTimesPoint", DbType.Boolean, orderInfo.IsSendTimesPoint);
            this.database.AddInParameter(storedProcCommand, "FreightFreePromotionId", DbType.Int32, orderInfo.FreightFreePromotionId);
            this.database.AddInParameter(storedProcCommand, "FreightFreePromotionName", DbType.String, orderInfo.FreightFreePromotionName);
            this.database.AddInParameter(storedProcCommand, "IsFreightFree", DbType.Boolean, orderInfo.IsFreightFree);
            this.database.AddInParameter(storedProcCommand, "CouponName", DbType.String, orderInfo.CouponName);
            this.database.AddInParameter(storedProcCommand, "CouponCode", DbType.String, orderInfo.CouponCode);
            this.database.AddInParameter(storedProcCommand, "CouponAmount", DbType.Currency, orderInfo.CouponAmount);
            this.database.AddInParameter(storedProcCommand, "CouponValue", DbType.Currency, orderInfo.CouponValue);
            this.database.AddInParameter(storedProcCommand, "RedPagerActivityName", DbType.String, orderInfo.RedPagerActivityName);
            this.database.AddInParameter(storedProcCommand, "RedPagerID", DbType.String, orderInfo.RedPagerID);
            this.database.AddInParameter(storedProcCommand, "RedPagerOrderAmountCanUse", DbType.Currency, orderInfo.RedPagerOrderAmountCanUse);
            this.database.AddInParameter(storedProcCommand, "RedPagerAmount", DbType.Currency, orderInfo.RedPagerAmount);
            if (orderInfo.GroupBuyId > 0)
            {
                this.database.AddInParameter(storedProcCommand, "GroupBuyId", DbType.Int32, orderInfo.GroupBuyId);
                this.database.AddInParameter(storedProcCommand, "NeedPrice", DbType.Currency, orderInfo.NeedPrice);
                this.database.AddInParameter(storedProcCommand, "GroupBuyStatus", DbType.Int32, 1);
            }
            else
            {
                this.database.AddInParameter(storedProcCommand, "GroupBuyId", DbType.Int32, DBNull.Value);
                this.database.AddInParameter(storedProcCommand, "NeedPrice", DbType.Currency, DBNull.Value);
                this.database.AddInParameter(storedProcCommand, "GroupBuyStatus", DbType.Int32, DBNull.Value);
            }
            if (orderInfo.CountDownBuyId > 0)
            {
                this.database.AddInParameter(storedProcCommand, "CountDownBuyId ", DbType.Int32, orderInfo.CountDownBuyId);
            }
            else
            {
                this.database.AddInParameter(storedProcCommand, "CountDownBuyId ", DbType.Int32, DBNull.Value);
            }
            if (orderInfo.BundlingID > 0)
            {
                this.database.AddInParameter(storedProcCommand, "BundlingID ", DbType.Int32, orderInfo.BundlingID);
                this.database.AddInParameter(storedProcCommand, "BundlingPrice", DbType.Currency, orderInfo.BundlingPrice);
            }
            else
            {
                this.database.AddInParameter(storedProcCommand, "BundlingID ", DbType.Int32, DBNull.Value);
                this.database.AddInParameter(storedProcCommand, "BundlingPrice", DbType.Currency, DBNull.Value);
            }
            this.database.AddInParameter(storedProcCommand, "Tax", DbType.Currency, orderInfo.Tax);
            this.database.AddInParameter(storedProcCommand, "InvoiceTitle", DbType.String, orderInfo.InvoiceTitle);
            this.database.AddInParameter(storedProcCommand, "ReferralUserId", DbType.Int32, orderInfo.ReferralUserId);
            this.database.AddInParameter(storedProcCommand, "DiscountAmount", DbType.Decimal, orderInfo.DiscountAmount);
            this.database.AddInParameter(storedProcCommand, "ActivitiesId", DbType.String, orderInfo.ActivitiesId);
            this.database.AddInParameter(storedProcCommand, "ActivitiesName", DbType.String, orderInfo.ActivitiesName);
            this.database.AddInParameter(storedProcCommand, "FirstCommission", DbType.Decimal, orderInfo.FirstCommission);
            this.database.AddInParameter(storedProcCommand, "SecondCommission", DbType.Decimal, orderInfo.SecondCommission);
            this.database.AddInParameter(storedProcCommand, "ThirdCommission", DbType.Decimal, orderInfo.ThirdCommission);
            this.database.AddInParameter(storedProcCommand, "PointToCash", DbType.Decimal, orderInfo.PointToCash);
            this.database.AddInParameter(storedProcCommand, "PointExchange", DbType.Int32, orderInfo.PointExchange);
            this.database.AddInParameter(storedProcCommand, "wid", DbType.String, orderInfo.wid);
            return (this.database.ExecuteNonQuery(storedProcCommand, dbTran) == 1);
        }

        public int DeleteOrders(string orderIds)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("DELETE FROM Hishop_Orders WHERE OrderId IN({0})", orderIds));
            return this.database.ExecuteNonQuery(sqlStringCommand);
        }

        public bool DeleteReturnRecordForSendGoods(string orderid)
        {
            string[] strArray = new string[] { "delete from Hishop_OrderReturns where OrderID=@OrderID and (HandleStatus=", 1.ToString(), " or HandleStatus=", 6.ToString(), ")" };
            string str = string.Concat(strArray);
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(str);
            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderid);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool EditOrderShipNumber(string orderId, string shipNumber)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_Orders SET ShipOrderNumber=@ShipOrderNumber WHERE OrderId =@OrderId");
            this.database.AddInParameter(sqlStringCommand, "ShipOrderNumber", DbType.String, shipNumber);
            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public DataTable GetAllOrderID(string wid)
        {
            string str = "select OrderId,IsPrinted,OrderStatus,Gateway from Hishop_Orders with (nolock) where wid=@wid";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(str);
            this.database.AddInParameter(sqlStringCommand, "wid", DbType.String, wid);
            return this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
        }

        public decimal GetCommossionByOrderId(string orderId, int userId)
        {
            string str = "select CommTotal from Hishop_Commissions WHERE OrderId=@OrderId AND UserId=@UserId";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(str);
            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int16, userId);
            object obj2 = this.database.ExecuteScalar(sqlStringCommand);
            if ((obj2 != null) && !(obj2 is DBNull))
            {
                return (decimal) obj2;
            }
            return 0M;
        }

        public int GetCountOrderIDByStatus(OrderStatus? orderstatus, OrderStatus? itemstatus,string wid)
        {
            string str = string.Empty;
            str = " wid='" + wid + "'";
            if (orderstatus.HasValue)
            {
                str = " and OrderStatus=" + ((int) orderstatus.Value);
            }
            if (itemstatus.HasValue)
            {
                if (!string.IsNullOrEmpty(str))
                {
                    str = str + " and ";
                }
                object obj2 = str;
                str = string.Concat(new object[] { obj2, " OrderId in(SELECT OrderId FROM Hishop_OrderItems WHERE OrderItemsStatus=", (int) itemstatus.Value, ")" });
            }
            if (!string.IsNullOrEmpty(str))
            {
                str = " where " + str;
            }
            string str2 = "select count(0) from Hishop_Orders with (nolock) " + str;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(str2);
            return Globals.ToNum(this.database.ExecuteScalar(sqlStringCommand));
        }

        public DataSet GetDistributorOrder(OrderQuery query)
        {
            string str = string.Empty;
            if (query.Status == OrderStatus.Finished)
            {
                str = str + " AND OrderStatus=" + ((int) query.Status);
            }
            string str2 = "SELECT OrderId, OrderDate,FinishDate, OrderStatus,PaymentTypeId, OrderTotal,Gateway,FirstCommission,SecondCommission,ThirdCommission FROM Hishop_Orders o WHERE ReferralUserId = @UserId";
            str2 = (str2 + str + " ORDER BY OrderDate DESC") + " SELECT OrderId,SkuId, ThumbnailsUrl, ItemDescription, SKUContent, SKU, ProductId,Quantity,ItemListPrice,ItemAdjustedCommssion,OrderItemsStatus,ItemsCommission,Type,ReturnMoney,IsAdminModify FROM Hishop_OrderItems WHERE OrderId IN (SELECT OrderId FROM Hishop_Orders WHERE ReferralUserId = @UserId" + str + ")";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(str2);
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, query.UserId);
            DataSet set = this.database.ExecuteDataSet(sqlStringCommand);
            DataColumn parentColumn = set.Tables[0].Columns["OrderId"];
            DataColumn childColumn = set.Tables[1].Columns["OrderId"];
            DataRelation relation = new DataRelation("OrderItems", parentColumn, childColumn);
            set.Relations.Add(relation);
            return set;
        }

        public int GetDistributorOrderCount(OrderQuery query)
        {
            string str = string.Empty;
            switch (query.Status)
            {
                case OrderStatus.Finished:
                    str = str + " AND OrderStatus=" + ((int) query.Status);
                    break;

                case OrderStatus.Today:
                {
                    string str2 = DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00";
                    str = str + " AND OrderDate>='" + str2 + "'";
                    break;
                }
            }
            string str3 = "SELECT COUNT(*)  FROM Hishop_Orders o WHERE ReferralUserId = @ReferralUserId";
            str3 = str3 + str;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(str3);
            sqlStringCommand.CommandType = CommandType.Text;
            this.database.AddInParameter(sqlStringCommand, "ReferralUserId", DbType.Int32, query.UserId);
            return (int) this.database.ExecuteScalar(sqlStringCommand);
        }

        public string GetexChangeName(int exChangeId)
        {
            string str = "select Name from Hishop_PointExChange_PointExChanges where id=" + exChangeId;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(str);
            object obj2 = this.database.ExecuteScalar(sqlStringCommand);
            if ((obj2 != null) && !(obj2 is DBNull))
            {
                return obj2.ToString();
            }
            return "";
        }

        public DataSet GetOrderGoods(string orderIds)
        {
            this.database = DatabaseFactory.CreateDatabase();
            StringBuilder builder = new StringBuilder();
            builder.Append("SELECT OrderId, ItemDescription AS ProductName, SKU, SKUContent, ShipmentQuantity,");
            builder.Append(" (SELECT Stock FROM Hishop_SKUs WHERE SkuId = oi.SkuId) + oi.ShipmentQuantity AS Stock, (SELECT Remark FROM Hishop_Orders WHERE OrderId = oi.OrderId) AS Remark");
            builder.Append(" FROM Hishop_OrderItems oi WHERE OrderId IN (SELECT OrderId FROM Hishop_Orders WHERE OrderStatus = 2 or (OrderStatus = 1 AND Gateway='hishop.plugins.payment.podrequest'))");
            builder.Append(" AND (OrderItemsStatus=2 OR OrderItemsStatus=1)");
            builder.AppendFormat(" AND OrderId IN ({0}) ORDER BY OrderId;", orderIds);
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            return this.database.ExecuteDataSet(sqlStringCommand);
        }

        public OrderInfo GetOrderInfo(string orderId)
        {
            OrderInfo info = null;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_Orders Where OrderId = @OrderId;  SELECT * FROM Hishop_OrderItems Where OrderId = @OrderId ");
            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    info = DataMapper.PopulateOrder(reader);
                }
                reader.NextResult();
                while (reader.Read())
                {
                    info.LineItems.Add(reader["Id"].ToString(), DataMapper.PopulateLineItem(reader));
                }
            }
            return info;
        }

        public OrderInfo GetOrderInfoForLineItems(string orderId)
        {
            OrderInfo info = null;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_Orders Where OrderId = @OrderId;  SELECT * FROM Hishop_OrderItems Where OrderId = @OrderId ");
            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    info = DataMapper.PopulateOrder(reader);
                }
                reader.NextResult();
                while (reader.Read())
                {
                    info.LineItems.Add(reader["SkuId"].ToString(), DataMapper.PopulateLineItem(reader));
                }
            }
            return info;
        }

        public DataTable GetOrderMarkingAllOrderID(string OrderMarking)
        {
            string str = "select OrderId from Hishop_Orders where OrderStatus=1 and OrderMarking='" + OrderMarking + "'";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(str);
            return this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
        }

        public DbQueryResult GetOrders(OrderQuery query)
        {
            StringBuilder builder = new StringBuilder("1=1");
            builder.AppendFormat(" AND wid = '{0}'", query.wid);
            if (query.Type.HasValue)
            {
                if (((OrderQuery.OrderType) query.Type.Value) == OrderQuery.OrderType.GroupBuy)
                {
                    builder.Append(" And GroupBuyId > 0 ");
                }
                else
                {
                    builder.Append(" And GroupBuyId is null ");
                }
            }
            if ((query.OrderId != string.Empty) && (query.OrderId != null))
            {
                builder.AppendFormat(" AND OrderId = '{0}'", DataHelper.CleanSearchString(query.OrderId));
            }
            if (query.UserId.HasValue)
            {
                builder.AppendFormat(" AND UserId = '{0}'", query.UserId.Value);
            }
            if (query.PaymentType.HasValue)
            {
                builder.AppendFormat(" AND PaymentTypeId = '{0}'", query.PaymentType.Value);
            }
            if (query.GroupBuyId.HasValue)
            {
                builder.AppendFormat(" AND GroupBuyId = {0}", query.GroupBuyId.Value);
            }
            if (!string.IsNullOrEmpty(query.ProductName))
            {
                builder.AppendFormat(" AND OrderId IN (SELECT OrderId FROM Hishop_OrderItems WHERE ItemDescription LIKE '%{0}%')", DataHelper.CleanSearchString(query.ProductName));
            }
            if (query.OrderItemsStatus.HasValue)
            {
                if (((OrderStatus) query.OrderItemsStatus.Value) == OrderStatus.ApplyForRefund)
                {
                    builder.AppendFormat(" AND OrderId IN (SELECT OrderId FROM Hishop_OrderItems WHERE OrderItemsStatus in({0},{1}))", (int) query.OrderItemsStatus.Value, 7);
                }
                else
                {
                    builder.AppendFormat(" AND OrderId IN (SELECT OrderId FROM Hishop_OrderItems WHERE OrderItemsStatus={0})", (int) query.OrderItemsStatus.Value);
                }
            }
            if (!string.IsNullOrEmpty(query.ShipTo))
            {
                builder.AppendFormat(" AND (ShipTo LIKE '%{0}%' or CellPhone='{0}')", DataHelper.CleanSearchString(query.ShipTo));
            }
            if (query.RegionId.HasValue)
            {
                builder.AppendFormat(" AND ShippingRegion like '%{0}%'", DataHelper.CleanSearchString(RegionHelper.GetFullRegion(query.RegionId.Value, "，")));
            }
            if (!string.IsNullOrEmpty(query.UserName))
            {
                builder.AppendFormat(" AND  UserName  = '{0}' ", DataHelper.CleanSearchString(query.UserName));
            }
            if (query.Status == OrderStatus.History)
            {
                builder.AppendFormat(" AND OrderStatus != {0} AND OrderStatus != {1} AND OrderStatus != {2} AND OrderDate < '{3}'", new object[] { 1, 4, 9, DateTime.Now.AddMonths(-3) });
            }
            else if (query.Status == OrderStatus.BuyerAlreadyPaid)
            {
                builder.AppendFormat(" AND (OrderStatus = {0} OR (OrderStatus = 1 AND Gateway = 'hishop.plugins.payment.podrequest'))", (int) query.Status);
            }
            else if (query.Status != OrderStatus.All)
            {
                builder.AppendFormat(" AND OrderStatus = {0}", (int) query.Status);
            }
            if (query.StartDate.HasValue)
            {
                builder.AppendFormat(" AND datediff(dd,'{0}',OrderDate)>=0", DataHelper.GetSafeDateTimeFormat(query.StartDate.Value));
            }
            if (query.EndDate.HasValue)
            {
                builder.AppendFormat(" AND datediff(dd,'{0}',OrderDate)<=0", DataHelper.GetSafeDateTimeFormat(query.EndDate.Value));
            }
            if (query.ShippingModeId.HasValue)
            {
                builder.AppendFormat(" AND ShippingModeId = {0}", query.ShippingModeId.Value);
            }
            if (query.IsPrinted.HasValue)
            {
                builder.AppendFormat(" AND ISNULL(IsPrinted, 0)={0}", query.IsPrinted.Value);
            }
            if (query.ShippingModeId > 0)
            {
                builder.AppendFormat(" AND ShippingModeId={0}", query.ShippingModeId);
            }
            if (!string.IsNullOrEmpty(query.StoreName))
            {
                builder.AppendFormat(" AND StoreName like '%{0}%' ", DataHelper.CleanSearchString(query.StoreName));
            }
            if (!string.IsNullOrEmpty(query.Gateway))
            {
                builder.AppendFormat(" AND Gateway='{0}' ", DataHelper.CleanSearchString(query.Gateway));
            }
            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Hishop_Order", "OrderId", builder.ToString(), "*");
        }

        public DataSet GetOrdersAndLines(string orderIds)
        {
            this.database = DatabaseFactory.CreateDatabase();
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("SELECT * FROM Hishop_Orders WHERE  OrderId IN ({0}) order by  ShipOrderNumber asc,OrderDate desc ", orderIds);
            builder.AppendFormat(" SELECT * FROM Hishop_OrderItems WHERE OrderId IN ({0});", orderIds);
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            return this.database.ExecuteDataSet(sqlStringCommand);
        }

        public DataSet GetOrdersByOrderIDList(string orderIds)
        {
            this.database = DatabaseFactory.CreateDatabase();
            string str = string.Empty;
            string str2 = " OrderId,ShipTo,RegionId,ExpressCompanyName,ExpressCompanyAbb,ShipOrderNumber,Remark,OrderStatus,ShippingRegion,Address";
            str = string.Format("with v as (SELECT " + str2 + ", row_number() over (partition by ShipTo+CONVERT(VARCHAR(11), RegionId)+ExpressCompanyAbb+[Address]+CellPhone order by  RegionId desc) as rownumber from Hishop_Orders where   OrderId in ({0})) select " + str2 + ",OrderStatus,rownumber from v", orderIds);
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(str);
            return this.database.ExecuteDataSet(sqlStringCommand);
        }

        public DataSet GetProductGoods(string orderIds)
        {
            this.database = DatabaseFactory.CreateDatabase();
            StringBuilder builder = new StringBuilder();
            builder.Append("SELECT ItemDescription AS ProductName, SKU, SKUContent, sum(ShipmentQuantity) as ShipmentQuantity,");
            builder.Append(" (SELECT Stock FROM Hishop_SKUs WHERE SkuId = oi.SkuId) + sum(ShipmentQuantity) AS Stock FROM Hishop_OrderItems oi");
            builder.Append(" WHERE OrderId IN (SELECT OrderId FROM Hishop_Orders WHERE OrderStatus = 2 or (OrderStatus = 1 AND Gateway='hishop.plugins.payment.podrequest'))");
            builder.Append(" AND OrderItemsStatus=2");
            builder.AppendFormat(" AND OrderId in ({0}) GROUP BY ItemDescription, SkuId, SKU, SKUContent;", orderIds);
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            return this.database.ExecuteDataSet(sqlStringCommand);
        }

        public string GetReplaceComments(string orderId)
        {
            string str = "select Comments from Hishop_OrderReplace where HandleStatus=0 and OrderId='" + orderId + "'";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(str);
            object obj2 = this.database.ExecuteScalar(sqlStringCommand);
            if ((obj2 != null) && !(obj2 is DBNull))
            {
                return obj2.ToString();
            }
            return "";
        }

        public DataTable GetSendGoodsOrders(string orderIds)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("SELECT * FROM Hishop_Orders WHERE (OrderStatus = 2 OR (OrderStatus = 1 AND Gateway = 'hishop.plugins.payment.podrequest')) AND OrderId IN ({0}) order by OrderDate desc", orderIds));
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        public DataSet GetUserOrder(int userId, OrderQuery query)
        {
            string str = string.Empty;
            if (query.Status == OrderStatus.WaitBuyerPay)
            {
                str = str + " AND OrderStatus = 1 AND Gateway <> 'hishop.plugins.payment.podrequest'";
            }
            else if (query.Status == OrderStatus.SellerAlreadySent)
            {
                str = str + " AND (OrderStatus = 2 OR OrderStatus = 3 OR (OrderStatus = 1 AND Gateway = 'hishop.plugins.payment.podrequest'))";
            }
            string str2 = "SELECT OrderId,OrderMarking, OrderDate, OrderStatus,PaymentTypeId, OrderTotal,   Gateway,(SELECT count(0) FROM vshop_OrderRedPager WHERE OrderId = o.OrderId and ExpiryDays<getdate() and AlreadyGetTimes<MaxGetTimes) as HasRedPage,(SELECT SUM(Quantity) FROM Hishop_OrderItems WHERE OrderId = o.OrderId) as ProductSum FROM Hishop_Orders o WHERE UserId = @UserId";
            str2 = (str2 + str + " ORDER BY OrderDate DESC") + " SELECT OrderId, ThumbnailsUrl, ItemDescription, SKUContent, SKU,OrderItemsStatus, ProductId,Quantity,ReturnMoney FROM Hishop_OrderItems WHERE OrderId IN (SELECT OrderId FROM Hishop_Orders WHERE UserId = @UserId" + str + ")";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(str2);
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
            DataSet set = this.database.ExecuteDataSet(sqlStringCommand);
            DataColumn parentColumn = set.Tables[0].Columns["OrderId"];
            DataColumn childColumn = set.Tables[1].Columns["OrderId"];
            DataRelation relation = new DataRelation("OrderItems", parentColumn, childColumn);
            set.Relations.Add(relation);
            return set;
        }

        public int GetUserOrderCount(int userId, OrderQuery query)
        {
            string str = string.Empty;
            if (query.Status == OrderStatus.WaitBuyerPay)
            {
                str = str + " AND OrderStatus = 1 AND Gateway <> 'hishop.plugins.payment.podrequest'";
            }
            else if (query.Status == OrderStatus.SellerAlreadySent)
            {
                str = str + " AND (OrderStatus = 2 OR OrderStatus = 3 OR (OrderStatus = 1 AND Gateway = 'hishop.plugins.payment.podrequest'))";
            }
            string str2 = "SELECT COUNT(1)  FROM Hishop_Orders o WHERE UserId = @UserId";
            str2 = str2 + str;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(str2);
            sqlStringCommand.CommandType = CommandType.Text;
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
            return (int) this.database.ExecuteScalar(sqlStringCommand);
        }

        public DataSet GetUserOrderReturn(int userId, OrderQuery query)
        {
            string str = string.Empty + " AND (OrderStatus = 2 OR OrderStatus = 3) ";
            string str2 = "SELECT OrderId, OrderDate, OrderStatus,PaymentTypeId, OrderTotal, (SELECT SUM(Quantity) FROM Hishop_OrderItems WHERE OrderId = o.OrderId) as ProductSum FROM Hishop_Orders o WHERE UserId = @UserId";
            str2 = (str2 + str + " ORDER BY OrderDate DESC") + " SELECT OrderId, ThumbnailsUrl,Quantity, ItemDescription,OrderItemsStatus, SKUContent, SKU, ProductId FROM Hishop_OrderItems WHERE IsHandled=0 and Type=0 and (OrderItemsStatus=2 OR OrderItemsStatus=3) AND OrderId IN (SELECT OrderId FROM Hishop_Orders WHERE UserId = @UserId " + str + ") ";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(str2);
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
            DataSet set = this.database.ExecuteDataSet(sqlStringCommand);
            DataColumn parentColumn = set.Tables[0].Columns["OrderId"];
            DataColumn childColumn = set.Tables[1].Columns["OrderId"];
            DataRelation relation = new DataRelation("OrderItems", parentColumn, childColumn);
            set.Relations.Add(relation);
            return set;
        }

        public int GetUserOrderReturnCount(int userId)
        {
            object obj2 = string.Empty;
            string str = string.Concat(new object[] { obj2, " AND (OrderItemsStatus = ", 6, " OR OrderItemsStatus =", 7, ")" });
            string str2 = "SELECT COUNT(*) FROM Hishop_OrderItems WHERE OrderId IN (SELECT OrderId FROM Hishop_Orders WHERE UserId=@UserId)";
            str2 = str2 + str;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(str2);
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
            return (int) this.database.ExecuteScalar(sqlStringCommand);
        }

        public int GetUserOrders(int userId)
        {
            string str = "select count(OrderId) from Hishop_Orders WHERE UserId=@UserId";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(str);
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int16, userId);
            object obj2 = this.database.ExecuteScalar(sqlStringCommand);
            if ((obj2 != null) && !(obj2 is DBNull))
            {
                return (int) obj2;
            }
            return 0;
        }

        public bool InsertCalculationCommission(ArrayList UserIdList, ArrayList ReferralBlanceList, string orderid, ArrayList OrdersTotalList, string userid)
        {
            string str = "";
            str = str + "begin try  " + "  begin tran TranUpdate";
            for (int i = 0; i < UserIdList.Count; i++)
            {
                object obj2 = str;
                str = string.Concat(new object[] { obj2, " INSERT INTO [Hishop_Commissions]([UserId],[ReferralUserId],[OrderId],[OrderTotal],[CommTotal],[CommType],[State])VALUES(", UserIdList[i], ",", userid, ",'", orderid, "',", OrdersTotalList[i], ",", ReferralBlanceList[i], ",1,0);" });
            }
            str = str + " COMMIT TRAN TranUpdate" + "  end try \r\n                    begin catch \r\n                        ROLLBACK TRAN TranUpdate\r\n                    end catch ";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(str);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool InsertPointExchange_Changed(PointExchangeChangedInfo info, DbTransaction dbTran, int itemCount = 1)
        {
            if (itemCount < 1)
            {
                return false;
            }
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < itemCount; i++)
            {
                builder.Append("INSERT INTO  Hishop_PointExchange_Changed ([exChangeId],[exChangeName],[ProductId],[PointNumber],[Date],[MemberID],[MemberGrades]) VALUES (@exChangeId,@exChangeName,@ProductId,@PointNumber,@Date,@MemberID,@MemberGrades);");
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            this.database.AddInParameter(sqlStringCommand, "exChangeId", DbType.Int32, info.exChangeId);
            this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, info.ProductId);
            this.database.AddInParameter(sqlStringCommand, "exChangeName", DbType.String, info.exChangeName);
            this.database.AddInParameter(sqlStringCommand, "PointNumber", DbType.Int32, info.PointNumber);
            this.database.AddInParameter(sqlStringCommand, "Date", DbType.DateTime, DateTime.Now);
            this.database.AddInParameter(sqlStringCommand, "MemberID", DbType.Int32, info.MemberID);
            this.database.AddInParameter(sqlStringCommand, "MemberGrades", DbType.String, info.MemberGrades);
            if (dbTran != null)
            {
                return (this.database.ExecuteNonQuery(sqlStringCommand, dbTran) > 0);
            }
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool SetOrderExpressComputerpe(string orderIds, string expressCompanyName, string expressCompanyAbb)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("UPDATE Hishop_Orders SET ExpressCompanyName=@ExpressCompanyName,ExpressCompanyAbb=@ExpressCompanyAbb WHERE (OrderStatus = 2 OR (OrderStatus = 1 AND Gateway='hishop.plugins.payment.podrequest')) AND OrderId IN ({0})", orderIds));
            this.database.AddInParameter(sqlStringCommand, "ExpressCompanyName", DbType.String, expressCompanyName);
            this.database.AddInParameter(sqlStringCommand, "ExpressCompanyAbb", DbType.String, expressCompanyAbb);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool SetOrderShippingMode(string orderIds, int realShippingModeId, string realModeName)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("UPDATE Hishop_Orders SET RealShippingModeId=@RealShippingModeId,RealModeName=@RealModeName WHERE (OrderStatus = 2 OR (OrderStatus = 1 AND Gateway='hishop.plugins.payment.podrequest')) AND OrderId IN ({0})", orderIds));
            this.database.AddInParameter(sqlStringCommand, "RealShippingModeId", DbType.Int32, realShippingModeId);
            this.database.AddInParameter(sqlStringCommand, "RealModeName", DbType.String, realModeName);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool SetPrintOrderExpress(string orderId, string expressCompanyName, string expressCompanyAbb, string shipOrderNumber)
        {
            string str = string.Empty;
            if (string.IsNullOrEmpty(shipOrderNumber))
            {
                str = "UPDATE Hishop_Orders SET ExpressCompanyName=@ExpressCompanyName,ExpressCompanyAbb=@ExpressCompanyAbb WHERE  OrderId=@OrderId";
            }
            else
            {
                str = "UPDATE Hishop_Orders SET IsPrinted=1,ShipOrderNumber=@ShipOrderNumber,ExpressCompanyName=@ExpressCompanyName,ExpressCompanyAbb=@ExpressCompanyAbb WHERE  OrderId=@OrderId";
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(str);
            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
            this.database.AddInParameter(sqlStringCommand, "ShipOrderNumber", DbType.String, shipOrderNumber);
            this.database.AddInParameter(sqlStringCommand, "ExpressCompanyName", DbType.String, expressCompanyName);
            this.database.AddInParameter(sqlStringCommand, "ExpressCompanyAbb", DbType.String, expressCompanyAbb);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool UpdateCalculadtionCommission(OrderInfo orderinfo, DbTransaction dbTran = null)
        {
            foreach (LineItemInfo info in orderinfo.LineItems.Values)
            {
                if ((info.OrderItemsStatus == OrderStatus.Refunded) || (info.OrderItemsStatus == OrderStatus.Returned))
                {
                    new LineItemDao().UpdateCommissionItem(info.ID, 0M, 0M, 0M, dbTran);
                }
                else
                {
                    new LineItemDao().UpdateCommissionItem(info.ID, info.ItemsCommission, info.SecondItemsCommission, info.ThirdItemsCommission, dbTran);
                }
            }
            return true;
        }

        public bool UpdateCoupon_MemberCoupons(OrderInfo orderinfo, DbTransaction dbTran)
        {
            string str = "update Hishop_Coupon_MemberCoupons set OrderNo=@OrderNo, Status=@Status,UsedDate=@UsedDate WHERE Id=@Id;\r\n                        update Hishop_Coupon_Coupons set UsedNum=isnull(UsedNum,0)+1 where CouponId=(select top 1 CouponId From Hishop_Coupon_MemberCoupons where Id=@Id);";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(str);
            this.database.AddInParameter(sqlStringCommand, "OrderNo", DbType.String, orderinfo.OrderId);
            this.database.AddInParameter(sqlStringCommand, "Id", DbType.Int32, orderinfo.RedPagerID);
            this.database.AddInParameter(sqlStringCommand, "Status", DbType.Int32, 1);
            this.database.AddInParameter(sqlStringCommand, "UsedDate", DbType.DateTime, DateTime.Now);
            if (dbTran != null)
            {
                return (this.database.ExecuteNonQuery(sqlStringCommand, dbTran) > 0);
            }
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public void UpdateItemsStatus(string orderId, int status, string ItemStr)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("Update Hishop_OrderItems Set OrderItemsStatus=@OrderItemsStatus Where OrderId =@OrderId and SkuId IN (" + ItemStr + ")");
            this.database.AddInParameter(sqlStringCommand, "OrderItemsStatus", DbType.Int32, status);
            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
            this.database.ExecuteNonQuery(sqlStringCommand);
        }

        public bool UpdateMemberPointNumber(int PointNumber, int UserId, DbTransaction dbTran)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("update aspnet_Members set Points=Points-@PointNumber WHERE UserId=@UserId");
            this.database.AddInParameter(sqlStringCommand, "PointNumber", DbType.Int32, PointNumber);
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, UserId);
            if (dbTran != null)
            {
                return (this.database.ExecuteNonQuery(sqlStringCommand, dbTran) > 0);
            }
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool UpdateOrder(OrderInfo order, DbTransaction dbTran = null)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_Orders SET  OrderStatus = @OrderStatus, CloseReason=@CloseReason, PayDate = @PayDate, ShippingDate=@ShippingDate, FinishDate = @FinishDate, RegionId = @RegionId, ShippingRegion = @ShippingRegion, Address = @Address, ZipCode = @ZipCode,ShipTo = @ShipTo, TelPhone = @TelPhone, CellPhone = @CellPhone, ShippingModeId=@ShippingModeId ,ModeName=@ModeName, RealShippingModeId = @RealShippingModeId, RealModeName = @RealModeName, ShipOrderNumber = @ShipOrderNumber,  ExpressCompanyName = @ExpressCompanyName,ExpressCompanyAbb = @ExpressCompanyAbb, PaymentTypeId=@PaymentTypeId,PaymentType=@PaymentType, Gateway = @Gateway, ManagerMark=@ManagerMark,ManagerRemark=@ManagerRemark,IsPrinted=@IsPrinted, OrderTotal = @OrderTotal, OrderProfit=@OrderProfit,Amount=@Amount,OrderCostPrice=@OrderCostPrice, AdjustedFreight = @AdjustedFreight, PayCharge = @PayCharge, AdjustedDiscount=@AdjustedDiscount,OrderPoint=@OrderPoint,GatewayOrderId=@GatewayOrderId,OldAddress=@OldAddress WHERE OrderId = @OrderId");
            this.database.AddInParameter(sqlStringCommand, "OrderStatus", DbType.Int32, (int) order.OrderStatus);
            this.database.AddInParameter(sqlStringCommand, "CloseReason", DbType.String, order.CloseReason);
            this.database.AddInParameter(sqlStringCommand, "PayDate", DbType.DateTime, order.PayDate);
            this.database.AddInParameter(sqlStringCommand, "ShippingDate", DbType.DateTime, order.ShippingDate);
            this.database.AddInParameter(sqlStringCommand, "FinishDate", DbType.DateTime, order.FinishDate);
            this.database.AddInParameter(sqlStringCommand, "RegionId", DbType.String, order.RegionId);
            this.database.AddInParameter(sqlStringCommand, "ShippingRegion", DbType.String, order.ShippingRegion);
            this.database.AddInParameter(sqlStringCommand, "Address", DbType.String, order.Address);
            this.database.AddInParameter(sqlStringCommand, "ZipCode", DbType.String, order.ZipCode);
            this.database.AddInParameter(sqlStringCommand, "ShipTo", DbType.String, order.ShipTo);
            this.database.AddInParameter(sqlStringCommand, "TelPhone", DbType.String, order.TelPhone);
            this.database.AddInParameter(sqlStringCommand, "CellPhone", DbType.String, order.CellPhone);
            this.database.AddInParameter(sqlStringCommand, "ShippingModeId", DbType.Int32, order.ShippingModeId);
            this.database.AddInParameter(sqlStringCommand, "ModeName", DbType.String, order.ModeName);
            this.database.AddInParameter(sqlStringCommand, "RealShippingModeId", DbType.Int32, order.RealShippingModeId);
            this.database.AddInParameter(sqlStringCommand, "RealModeName", DbType.String, order.RealModeName);
            this.database.AddInParameter(sqlStringCommand, "ShipOrderNumber", DbType.String, order.ShipOrderNumber);
            this.database.AddInParameter(sqlStringCommand, "ExpressCompanyName", DbType.String, order.ExpressCompanyName);
            this.database.AddInParameter(sqlStringCommand, "ExpressCompanyAbb", DbType.String, order.ExpressCompanyAbb);
            this.database.AddInParameter(sqlStringCommand, "PaymentTypeId", DbType.Int32, order.PaymentTypeId);
            this.database.AddInParameter(sqlStringCommand, "PaymentType", DbType.String, order.PaymentType);
            this.database.AddInParameter(sqlStringCommand, "Gateway", DbType.String, order.Gateway);
            this.database.AddInParameter(sqlStringCommand, "ManagerMark", DbType.Int32, order.ManagerMark);
            this.database.AddInParameter(sqlStringCommand, "ManagerRemark", DbType.String, order.ManagerRemark);
            this.database.AddInParameter(sqlStringCommand, "IsPrinted", DbType.Boolean, order.IsPrinted);
            this.database.AddInParameter(sqlStringCommand, "OrderTotal", DbType.Currency, order.GetTotal());
            this.database.AddInParameter(sqlStringCommand, "OrderProfit", DbType.Currency, order.GetProfit());
            this.database.AddInParameter(sqlStringCommand, "Amount", DbType.Currency, order.GetAmount());
            this.database.AddInParameter(sqlStringCommand, "OrderCostPrice", DbType.Currency, order.GetCostPrice());
            this.database.AddInParameter(sqlStringCommand, "AdjustedFreight", DbType.Currency, order.AdjustedFreight);
            this.database.AddInParameter(sqlStringCommand, "PayCharge", DbType.Currency, order.PayCharge);
            this.database.AddInParameter(sqlStringCommand, "AdjustedDiscount", DbType.Currency, order.AdjustedDiscount);
            this.database.AddInParameter(sqlStringCommand, "OrderPoint", DbType.Int32, order.Points);
            this.database.AddInParameter(sqlStringCommand, "GatewayOrderId", DbType.String, order.GatewayOrderId);
            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, order.OrderId);
            this.database.AddInParameter(sqlStringCommand, "OldAddress", DbType.String, order.OldAddress);
            if (dbTran != null)
            {
                return (this.database.ExecuteNonQuery(sqlStringCommand, dbTran) > 0);
            }
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool UpdateOrderCompany(string orderId, string companycode, string companyname, string shipNumber)
        {
            string str = "UPDATE Hishop_Orders SET ShipOrderNumber=@ShipOrderNumber,ExpressCompanyAbb=@ExpressCompanyAbb,ExpressCompanyName=@ExpressCompanyName WHERE OrderId =@OrderId";
            if (string.IsNullOrEmpty(shipNumber))
            {
                str = "UPDATE Hishop_Orders SET ExpressCompanyAbb=@ExpressCompanyAbb,ExpressCompanyName=@ExpressCompanyName WHERE OrderId =@OrderId";
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(str);
            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
            this.database.AddInParameter(sqlStringCommand, "ExpressCompanyAbb", DbType.String, companycode);
            this.database.AddInParameter(sqlStringCommand, "ShipOrderNumber", DbType.String, shipNumber);
            this.database.AddInParameter(sqlStringCommand, "ExpressCompanyName", DbType.String, companyname);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public void UpdatePayOrderStock(string orderId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("Update Hishop_SKUs Set Stock = CASE WHEN (Stock - (SELECT SUM(oi.ShipmentQuantity) FROM Hishop_OrderItems oi Where oi.SkuId =Hishop_SKUs.SkuId AND OrderId =@OrderId))<=0 Then 0 ELSE Stock - (SELECT SUM(oi.ShipmentQuantity) FROM Hishop_OrderItems oi  Where oi.SkuId =Hishop_SKUs.SkuId AND OrderId =@OrderId) END WHERE Hishop_SKUs.SkuId  IN (Select SkuId FROM Hishop_OrderItems Where OrderId =@OrderId)");
            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
            this.database.ExecuteNonQuery(sqlStringCommand);
        }

        public bool UpdateRefundOrderStock(string orderId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("Update Hishop_SKUs Set Stock = Stock + (SELECT SUM(oi.ShipmentQuantity) FROM Hishop_OrderItems oi  Where oi.SkuId =Hishop_SKUs.SkuId AND OrderId =@OrderId) WHERE Hishop_SKUs.SkuId  IN (Select SkuId FROM Hishop_OrderItems Where OrderId =@OrderId)");
            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) >= 1);
        }

        public int UpdateRemark(string orderIds)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("DELETE FROM Hishop_Orders WHERE OrderId IN({0})", orderIds));
            return this.database.ExecuteNonQuery(sqlStringCommand);
        }
    }
}

