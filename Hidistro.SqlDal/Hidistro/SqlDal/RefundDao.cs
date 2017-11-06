namespace Hidistro.SqlDal
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities;
    using Hidistro.Entities.Orders;
    using Hidistro.SqlDal.Orders;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Data;
    using System.Data.Common;
    using System.Runtime.InteropServices;
    using System.Text;

    public class RefundDao
    {
        private Database database = DatabaseFactory.CreateDatabase();

        public void AddRefund(RefundInfo refundInfo)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("insert into Hishop_OrderRefund(OrderId,ApplyForTime,RefundRemark,HandleStatus,Account) values('{0}','{1}','{2}',{3},{4})", new object[] { refundInfo.OrderId, refundInfo.ApplyForTime, refundInfo.RefundRemark, (int) refundInfo.HandleStatus, refundInfo.Account });
            this.database.ExecuteNonQuery(CommandType.Text, builder.ToString());
        }

        public bool DelRefundApply(int ReturnsId)
        {
            string str = string.Format("DELETE FROM Hishop_OrderReturns WHERE ReturnsId={0} and (HandleStatus=2 or HandleStatus=5 or HandleStatus=7 or HandleStatus=8) ", ReturnsId);
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(str);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public RefundInfo GetByOrderId(string orderId)
        {
            string str = "select * from Hishop_OrderRefund where OrderId='" + orderId + "'";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(str);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return ReaderConvert.ReaderToModel<RefundInfo>(reader);
            }
        }

        public RefundInfo GetByOrderIdAndProductID(string orderId, int productid)
        {
            string str = "select * from Hishop_OrderReturns where OrderId=@OrderId and ProductId=" + productid;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(str);
            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return ReaderConvert.ReaderToModel<RefundInfo>(reader);
            }
        }

        public DataTable GetOrderItemsReFundByOrderID(string orderid)
        {
            string str = "select a.*,b.ItemDescription,b.SKUContent,b.ThumbnailsUrl from Hishop_OrderReturns a left join Hishop_OrderItems b on a.OrderId=b.orderid and a.ProductId=b.ProductId where a.OrderId =@OrderId";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(str);
            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderid);
            return this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
        }

        public DataTable GetOrderReturnTable(int userid, string ReturnsId)
        {
            string str = "";
            if (!string.IsNullOrEmpty(ReturnsId))
            {
                str = " and ReturnsId=" + ReturnsId;
            }
            string str2 = string.Empty;
            str2 = "select Hishop_OrderReturns.*,isnull(((select ProductName from Hishop_Products where ProductId=Hishop_OrderReturns.ProductId)),'该商品已删除') as ProductName from Hishop_OrderReturns where UserId=@UserId " + str + " order by ApplyForTime desc";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(str2);
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userid);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        public void GetRefundType(string orderId, out int refundType, out string remark)
        {
            refundType = 0;
            remark = "";
            string str = "select RefundType,RefundRemark from Hishop_OrderRefund where OrderId='" + orderId + "'";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(str);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    refundType = (reader["RefundType"] != DBNull.Value) ? ((int) reader["RefundType"]) : 0;
                    remark = (string) reader["RefundRemark"];
                }
            }
        }

        public void GetRefundTypeFromReturn(string orderId, out int refundType, out string remark)
        {
            refundType = 0;
            remark = "";
            string str = "select RefundType,Comments from Hishop_OrderReturns where HandleStatus=0 and OrderId='" + orderId + "'";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(str);
            IDataReader reader = this.database.ExecuteReader(sqlStringCommand);
            if (reader.Read())
            {
                refundType = (int) reader["RefundType"];
                remark = (string) reader["Comments"];
            }
        }

        public bool GetReturnInfo(int userid, string OrderId, int ProductId)
        {
            DataTable table;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select ReturnsId from Hishop_OrderReturns where OrderId=@OrderId and ProductId=@ProductId and UserId=@UserId");
            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, OrderId);
            this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, ProductId);
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userid);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                table = DataHelper.ConverDataReaderToDataTable(reader);
            }
            return (table.Rows.Count > 0);
        }

        public bool GetReturnMes(int userid, string OrderId, int ProductId, int HandleStatus)
        {
            DataTable table;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select ReturnsId from Hishop_OrderReturns where OrderId=@OrderId and ProductId=@ProductId and UserId=@UserId and HandleStatus=@HandleStatus");
            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, OrderId);
            this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, ProductId);
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userid);
            this.database.AddInParameter(sqlStringCommand, "HandleStatus", DbType.Int32, HandleStatus);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                table = DataHelper.ConverDataReaderToDataTable(reader);
            }
            return (table.Rows.Count > 0);
        }

        public DbQueryResult GetReturnOrderAll(ReturnsApplyQuery returnsapplyquery)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat(" 1=1 ", new object[0]);
            if (!string.IsNullOrEmpty(returnsapplyquery.HandleStatus.ToString()))
            {
                if (returnsapplyquery.HandleStatus > 0)
                {
                    builder.AppendFormat(" AND HandleStatus = {0}", returnsapplyquery.HandleStatus);
                }
                else if (returnsapplyquery.HandleStatus == -1)
                {
                    builder.AppendFormat(" AND (HandleStatus=6 or HandleStatus=2 or HandleStatus=8 ) ", new object[0]);
                }
                else
                {
                    builder.AppendFormat(" AND (HandleStatus=4 or HandleStatus=5 or HandleStatus=7 ) ", new object[0]);
                }
            }
            if (!string.IsNullOrEmpty(returnsapplyquery.OrderId))
            {
                builder.AppendFormat(" AND OrderId LIKE '%{0}%'", DataHelper.CleanSearchString(returnsapplyquery.OrderId));
            }
            if (!string.IsNullOrEmpty(returnsapplyquery.ReturnsId))
            {
                builder.AppendFormat(" AND ReturnsId ={0}", DataHelper.CleanSearchString(returnsapplyquery.ReturnsId));
            }
            return DataHelper.PagingByRownumber(returnsapplyquery.PageIndex, returnsapplyquery.PageSize, returnsapplyquery.SortBy, returnsapplyquery.SortOrder, returnsapplyquery.IsCount, "Hishop_OrderReturns", "ReturnsId", builder.ToString(), "Hishop_OrderReturns.*,(select Username from aspnet_Members where userid=Hishop_OrderReturns.userid)as Username,(select Username from aspnet_Managers where userid=Hishop_OrderReturns.Operator)as OperatorName,(select ProductName from Hishop_Products where ProductId=Hishop_OrderReturns.ProductId)as ProductName");
        }

        public bool InsertOrderRefund(RefundInfo refundInfo)
        {
            bool flag = false;
            decimal num = 0M;
            LineItemInfo returnMoneyByOrderIDAndProductID = new LineItemDao().GetReturnMoneyByOrderIDAndProductID(refundInfo.OrderId, refundInfo.ProductId);
            if (returnMoneyByOrderIDAndProductID == null)
            {
                return flag;
            }
            num = (returnMoneyByOrderIDAndProductID.GetSubTotal() - returnMoneyByOrderIDAndProductID.DiscountAverage) - returnMoneyByOrderIDAndProductID.ItemAdjustedCommssion;
            if (num < 0M)
            {
                num = 0M;
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("insert into Hishop_OrderReturns(OrderId,ApplyForTime,Comments,HandleStatus,Account,RefundMoney,RefundType,ProductId,UserId,AuditTime) values(@OrderId,@ApplyForTime,@Comments,@HandleStatus,@Account,@RefundMoney,@RefundType,@ProductId,@UserId,@AuditTime)");
            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, refundInfo.OrderId);
            this.database.AddInParameter(sqlStringCommand, "ApplyForTime", DbType.DateTime, refundInfo.ApplyForTime);
            this.database.AddInParameter(sqlStringCommand, "Comments", DbType.String, refundInfo.RefundRemark);
            this.database.AddInParameter(sqlStringCommand, "HandleStatus", DbType.Int32, (int) refundInfo.HandleStatus);
            this.database.AddInParameter(sqlStringCommand, "Account", DbType.String, refundInfo.Account);
            this.database.AddInParameter(sqlStringCommand, "RefundMoney", DbType.Decimal, num);
            this.database.AddInParameter(sqlStringCommand, "RefundType", DbType.Int32, refundInfo.RefundType);
            this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, refundInfo.ProductId);
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, refundInfo.UserId);
            this.database.AddInParameter(sqlStringCommand, "AuditTime", DbType.String, refundInfo.AuditTime);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool UpdateByAuditReturnsId(RefundInfo refundInfo)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("update Hishop_OrderReturns set AdminRemark=@AdminRemark,HandleStatus=@HandleStatus,HandleTime=@HandleTime,Operator=@Operator,AuditTime=@AuditTime,RefundMoney=@RefundMoney where ReturnsId =@ReturnsId");
            this.database.AddInParameter(sqlStringCommand, "AdminRemark", DbType.String, refundInfo.AdminRemark);
            this.database.AddInParameter(sqlStringCommand, "HandleStatus", DbType.Int32, (int) refundInfo.HandleStatus);
            this.database.AddInParameter(sqlStringCommand, "HandleTime", DbType.DateTime, refundInfo.HandleTime);
            this.database.AddInParameter(sqlStringCommand, "Operator", DbType.String, refundInfo.Operator);
            this.database.AddInParameter(sqlStringCommand, "AuditTime", DbType.String, refundInfo.AuditTime);
            this.database.AddInParameter(sqlStringCommand, "RefundMoney", DbType.Decimal, refundInfo.RefundMoney);
            this.database.AddInParameter(sqlStringCommand, "ReturnsId", DbType.Int32, refundInfo.RefundId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public void UpdateByOrderId(RefundInfo refundInfo)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("update Hishop_OrderRefund set AdminRemark=@AdminRemark,ApplyForTime=@ApplyForTime,HandleStatus=@HandleStatus,HandleTime=@HandleTime,Operator=@Operator,RefundRemark=@RefundRemark where OrderId =@OrderId");
            this.database.AddInParameter(sqlStringCommand, "AdminRemark", DbType.String, refundInfo.AdminRemark);
            this.database.AddInParameter(sqlStringCommand, "ApplyForTime", DbType.String, refundInfo.ApplyForTime);
            this.database.AddInParameter(sqlStringCommand, "HandleStatus", DbType.Int32, refundInfo.HandleStatus);
            this.database.AddInParameter(sqlStringCommand, "HandleTime", DbType.DateTime, refundInfo.HandleTime);
            this.database.AddInParameter(sqlStringCommand, "Operator", DbType.String, refundInfo.Operator);
            this.database.AddInParameter(sqlStringCommand, "RefundRemark", DbType.String, refundInfo.RefundRemark);
            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, refundInfo.OrderId);
            this.database.ExecuteNonQuery(sqlStringCommand);
        }

        public bool UpdateByReturnsId(RefundInfo refundInfo)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("update Hishop_OrderItems set IsHandled=1,ItemsCommission=0,SecondItemsCommission=0,ThirdItemsCommission=0,ReturnMoney=@RefundMoney where OrderId =@OrderId and ProductId=@ProductId;update Hishop_OrderReturns set AdminRemark=@AdminRemark,HandleStatus=@HandleStatus,HandleTime=@HandleTime,Operator=@Operator,RefundTime=@RefundTime,RefundMoney=@RefundMoney where ReturnsId =@ReturnsId");
            this.database.AddInParameter(sqlStringCommand, "AdminRemark", DbType.String, refundInfo.AdminRemark);
            this.database.AddInParameter(sqlStringCommand, "HandleStatus", DbType.Int32, (int) refundInfo.HandleStatus);
            this.database.AddInParameter(sqlStringCommand, "HandleTime", DbType.DateTime, refundInfo.HandleTime);
            this.database.AddInParameter(sqlStringCommand, "Operator", DbType.String, refundInfo.Operator);
            this.database.AddInParameter(sqlStringCommand, "RefundTime", DbType.String, refundInfo.RefundTime);
            this.database.AddInParameter(sqlStringCommand, "RefundMoney", DbType.Decimal, refundInfo.RefundMoney);
            this.database.AddInParameter(sqlStringCommand, "ReturnsId", DbType.Int32, refundInfo.RefundId);
            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, refundInfo.OrderId);
            this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, refundInfo.ProductId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool UpdateOrderGoodStatu(string orderid, string skuid, int OrderItemsStatus)
        {
            string str = string.Empty;
            if (5 == OrderItemsStatus)
            {
                str = "delete from Hishop_OrderReturns where orderid=@orderid and HandleStatus<>2 and HandleStatus<>8;";
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(str + "update Hishop_OrderItems set OrderItemsStatus=@OrderItemsStatus where orderid=@orderid and skuid=@skuid");
            this.database.AddInParameter(sqlStringCommand, "OrderItemsStatus", DbType.Int32, OrderItemsStatus);
            this.database.AddInParameter(sqlStringCommand, "skuid", DbType.String, skuid);
            this.database.AddInParameter(sqlStringCommand, "orderid", DbType.String, orderid);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool UpdateRefundOrderStock(string Stock, string SkuId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("Update Hishop_SKUs Set Stock = Stock + " + Stock + " where SkuId='" + SkuId + "'");
            return (this.database.ExecuteNonQuery(sqlStringCommand) >= 1);
        }
    }
}

