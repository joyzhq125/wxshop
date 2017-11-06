namespace Hidistro.SqlDal.Promotions
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities;
    using Hidistro.Entities.Orders;
    using Hidistro.Entities.Promotions;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Runtime.InteropServices;
    using System.Text;

    public class CouponDao
    {
        private Database database = DatabaseFactory.CreateDatabase();

        public bool AddCouponProducts(int couponId, int productID)
        {
            try
            {
                CouponInfo couponDetails = this.GetCouponDetails(couponId);
                if (couponDetails != null)
                {
                    StringBuilder builder = new StringBuilder();
                    builder.AppendFormat(" insert into Hishop_Coupon_Products(couponId , ProductId) values({0} , {1}) ", couponDetails.CouponId, productID);
                    DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
                    this.database.ExecuteNonQuery(sqlStringCommand);
                    sqlStringCommand = this.database.GetSqlStringCommand("update Hishop_Coupon_Coupons set IsAllProduct=0 , ProductNumber=(select count(productId) from Hishop_Coupon_Products  where CouponId = @CouponId) WHERE CouponId = @CouponId");
                    this.database.AddInParameter(sqlStringCommand, "CouponId", DbType.Int32, couponId);
                    this.database.ExecuteNonQuery(sqlStringCommand);
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool AddCouponProducts(int couponId, bool IsAllProduct, IList<string> productIDs)
        {
            try
            {
                CouponInfo couponDetails = this.GetCouponDetails(couponId);
                if (couponDetails != null)
                {
                    if (IsAllProduct)
                    {
                        couponDetails.IsAllProduct = true;
                        couponDetails.ProductNumber = 0;
                    }
                    else
                    {
                        couponDetails.IsAllProduct = false;
                        couponDetails.ProductNumber = productIDs.Count;
                    }
                    if (IsAllProduct)
                    {
                        DbCommand command = this.database.GetSqlStringCommand("update Hishop_Coupon_Coupons set IsAllProduct=@IsAllProduct , ProductNumber=@ProductNumber WHERE CouponId = @CouponId");
                        this.database.AddInParameter(command, "CouponId", DbType.Int32, couponId);
                        this.database.AddInParameter(command, "ProductNumber", DbType.Int32, couponDetails.ProductNumber);
                        this.database.AddInParameter(command, "IsAllProduct", DbType.Boolean, couponDetails.IsAllProduct);
                        this.database.ExecuteNonQuery(command);
                        command = this.database.GetSqlStringCommand("Delete from  Hishop_Coupon_Products WHERE CouponId = @CouponId");
                        this.database.AddInParameter(command, "CouponId", DbType.Int32, couponId);
                        this.database.ExecuteNonQuery(command);
                        return true;
                    }
                    string str = "";
                    if (productIDs.Count > 0)
                    {
                        foreach (string str2 in productIDs)
                        {
                            str = str + "," + str2;
                        }
                    }
                    str = str.Substring(1);
                    DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("Delete from  Hishop_Coupon_Products WHERE CouponId ={0} and ProductId in ( {1} )", couponId, str));
                    this.database.ExecuteNonQuery(sqlStringCommand);
                    StringBuilder builder = new StringBuilder();
                    foreach (string str3 in productIDs)
                    {
                        builder.AppendFormat(" insert into Hishop_Coupon_Products(couponId , ProductId) values({0} , {1}) ", couponDetails.CouponId, str3);
                    }
                    sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
                    this.database.ExecuteNonQuery(sqlStringCommand);
                    sqlStringCommand = this.database.GetSqlStringCommand("update Hishop_Coupon_Coupons set IsAllProduct=@IsAllProduct , ProductNumber=(select count(productId) from Hishop_Coupon_Products  where CouponId = @CouponId) WHERE CouponId = @CouponId");
                    this.database.AddInParameter(sqlStringCommand, "CouponId", DbType.Int32, couponId);
                    this.database.AddInParameter(sqlStringCommand, "IsAllProduct", DbType.Boolean, couponDetails.IsAllProduct);
                    this.database.ExecuteNonQuery(sqlStringCommand);
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool AddCouponUseRecord(OrderInfo orderinfo, DbTransaction dbTran)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("update  Hishop_CouponItems  set userName=@UserName,Userid=@Userid,Orderid=@Orderid,CouponStatus=@CouponStatus,EmailAddress=@EmailAddress,UsedTime=@UsedTime WHERE ClaimCode=@ClaimCode and CouponStatus!=1");
            this.database.AddInParameter(sqlStringCommand, "ClaimCode", DbType.String, orderinfo.CouponCode);
            this.database.AddInParameter(sqlStringCommand, "userName", DbType.String, orderinfo.Username);
            this.database.AddInParameter(sqlStringCommand, "userid", DbType.Int32, orderinfo.UserId);
            this.database.AddInParameter(sqlStringCommand, "CouponStatus", DbType.Int32, 1);
            this.database.AddInParameter(sqlStringCommand, "UsedTime", DbType.DateTime, DateTime.Now);
            this.database.AddInParameter(sqlStringCommand, "EmailAddress", DbType.String, orderinfo.EmailAddress);
            this.database.AddInParameter(sqlStringCommand, "Orderid", DbType.String, orderinfo.OrderId);
            if (dbTran != null)
            {
                return (this.database.ExecuteNonQuery(sqlStringCommand, dbTran) > 0);
            }
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public CouponActionStatus CreateCoupon(CouponInfo coupon)
        {
            try
            {
                DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT CouponId  FROM Hishop_Coupon_Coupons WHERE CouponName=@CouponName");
                this.database.AddInParameter(sqlStringCommand, "CouponName", DbType.String, coupon.CouponName);
                if (Convert.ToInt32(this.database.ExecuteScalar(sqlStringCommand)) >= 1)
                {
                    return CouponActionStatus.DuplicateName;
                }
                sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO [Hishop_Coupon_Coupons] ([CouponName],[CouponValue],[ConditionValue],[BeginDate],[EndDate],[StockNum],[ReceiveNum],[UsedNum],[MemberGrades],[ImgUrl],[ProductNumber],[Finished],[IsAllProduct],maxReceivNum) VALUES (@CouponName,@CouponValue,@ConditionValue,@BeginDate,@EndDate,@StockNum,@ReceiveNum,@UsedNum,@MemberGrades,@ImgUrl,@ProductNumber,@Finished,@IsAllProduct,@maxReceivNum)");
                this.database.AddInParameter(sqlStringCommand, "CouponName", DbType.String, coupon.CouponName);
                this.database.AddInParameter(sqlStringCommand, "CouponValue", DbType.Decimal, coupon.CouponValue);
                this.database.AddInParameter(sqlStringCommand, "ConditionValue", DbType.Decimal, coupon.ConditionValue);
                this.database.AddInParameter(sqlStringCommand, "BeginDate", DbType.DateTime, coupon.BeginDate);
                this.database.AddInParameter(sqlStringCommand, "EndDate", DbType.DateTime, coupon.EndDate);
                this.database.AddInParameter(sqlStringCommand, "StockNum", DbType.Int32, coupon.StockNum);
                this.database.AddInParameter(sqlStringCommand, "ReceiveNum", DbType.Int32, coupon.ReceiveNum);
                this.database.AddInParameter(sqlStringCommand, "UsedNum", DbType.Int32, coupon.UsedNum);
                this.database.AddInParameter(sqlStringCommand, "MemberGrades", DbType.String, coupon.MemberGrades);
                this.database.AddInParameter(sqlStringCommand, "ImgUrl", DbType.String, coupon.ImgUrl);
                this.database.AddInParameter(sqlStringCommand, "ProductNumber", DbType.Int32, coupon.ProductNumber);
                this.database.AddInParameter(sqlStringCommand, "Finished", DbType.Boolean, coupon.Finished);
                this.database.AddInParameter(sqlStringCommand, "IsAllProduct", DbType.Boolean, coupon.IsAllProduct);
                this.database.AddInParameter(sqlStringCommand, "maxReceivNum", DbType.Int32, coupon.maxReceivNum);
                this.database.ExecuteScalar(sqlStringCommand);
                return CouponActionStatus.Success;
            }
            catch (Exception)
            {
                return CouponActionStatus.CreateClaimCodeError;
            }
        }

        public bool CreateMemberCouponsInfo(MemberCouponsInfo mCoupons)
        {
            return false;
        }

        public bool DeleteCoupon(int couponId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_Coupon_Coupons WHERE CouponId = @CouponId");
            this.database.AddInParameter(sqlStringCommand, "CouponId", DbType.Int32, couponId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool DeleteProducts(int couponId, string ProductIds)
        {
            try
            {
                DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("Delete from  Hishop_Coupon_Products WHERE CouponId ={0} and ProductId in ( {1} )", couponId, ProductIds.ReplaceSingleQuoteMark()));
                this.database.ExecuteNonQuery(sqlStringCommand);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public DataTable GetCoupon(decimal orderAmount)
        {
            DataTable table = new DataTable();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT Name, ClaimCode,Amount,DiscountValue FROM Hishop_Coupon_Coupons c INNER  JOIN Hishop_CouponItems ci ON ci.CouponId = c.CouponId Where  @DateTime>c.StartTime and @DateTime <c.ClosingTime AND ((Amount>0 and @orderAmount>=Amount) or (Amount=0 and @orderAmount>=DiscountValue))    and  CouponStatus=0  AND UserId=@UserId");
            this.database.AddInParameter(sqlStringCommand, "DateTime", DbType.DateTime, DateTime.UtcNow);
            this.database.AddInParameter(sqlStringCommand, "orderAmount", DbType.Decimal, orderAmount);
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, Globals.GetCurrentMemberUserId());
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        public DataTable GetCouponByProducts(int couponId, int ProductId)
        {
            DataTable table = new DataTable();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select  ProductId   from Hishop_Coupon_Products where CouponId=@CouponId and status=0 and ProductId=@ProductId");
            this.database.AddInParameter(sqlStringCommand, "CouponId", DbType.Int32, couponId);
            this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, ProductId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        public CouponInfo GetCouponDetails(int couponId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_Coupon_Coupons WHERE CouponId = @CouponId");
            this.database.AddInParameter(sqlStringCommand, "CouponId", DbType.Int32, couponId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return ReaderConvert.ReaderToModel<CouponInfo>(reader);
            }
        }

        public CouponInfo GetCouponDetails(string couponName)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_Coupon_Coupons WHERE CouponName = @CouponName");
            this.database.AddInParameter(sqlStringCommand, "CouponName", DbType.String, couponName);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return ReaderConvert.ReaderToModel<CouponInfo>(reader);
            }
        }

        public DbQueryResult GetCouponInfos(CouponsSearch search)
        {
            try
            {
                StringBuilder builder = new StringBuilder(" 1=1 ");
                if (!string.IsNullOrEmpty(search.CouponName))
                {
                    builder.Append(" and CouponName like '%" + search.CouponName.ReplaceSingleQuoteMark() + "%'  ");
                }
                if (search.beginDate.HasValue)
                {
                    builder.Append(" and beginDate<='" + search.beginDate.Value.ToString("yyyy-MM-dd") + "' ");
                }
                if (search.endDate.HasValue)
                {
                    builder.Append(" and endDate>='" + search.endDate.Value.ToString("yyyy-MM-dd") + "' ");
                }
                if (search.Finished.HasValue)
                {
                    builder.Append(" and Finished=" + (search.Finished.Value ? 1 : 0).ToString());
                }
                if (search.minValue.HasValue)
                {
                    builder.Append(" and CouponValue>=" + search.minValue.Value);
                }
                if (search.maxValue.HasValue)
                {
                    builder.Append(" and CouponValue<=" + search.maxValue.Value);
                }
                return DataHelper.PagingByRownumber(search.PageIndex, search.PageSize, search.SortBy, search.SortOrder, search.IsCount, "Hishop_Coupon_Coupons", "CouponId", builder.ToString(), "*");
            }
            catch (Exception)
            {
                return null;
            }
        }

        public DataTable GetCouponProducts(int couponId)
        {
            DataTable table = new DataTable();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select couponId, a.ProductId , b.ProductName , a.status from Hishop_Coupon_Products a\tjoin Hishop_Products b on a.ProductId=b.ProductId where a.couponId=@couponId");
            this.database.AddInParameter(sqlStringCommand, "couponId", DbType.Int32, couponId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        public DbQueryResult GetCouponsList(CouponItemInfoQuery query)
        {
            StringBuilder builder = new StringBuilder();
            if (query.CouponId.HasValue)
            {
                builder.AppendFormat("CouponId = {0}", query.CouponId.Value);
            }
            if (!string.IsNullOrEmpty(query.CounponName))
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat("Name = '{0}'", query.CounponName);
            }
            if (!string.IsNullOrEmpty(query.UserName))
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat("UserName='{0}'", DataHelper.CleanSearchString(query.UserName));
            }
            if (!string.IsNullOrEmpty(query.OrderId))
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat("Orderid='{0}'", DataHelper.CleanSearchString(query.OrderId));
            }
            if (query.CouponStatus.HasValue)
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat(" CouponStatus={0} ", query.CouponStatus);
            }
            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Hishop_CouponInfo", "ClaimCode", builder.ToString(), "*");
        }

        public string GetCouponsProductIds(int CouponId)
        {
            string str = "select ProductId from Hishop_Coupon_Products where CouponId=" + CouponId.ToString();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(str);
            DataTable table = null;
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                table = DataHelper.ConverDataReaderToDataTable(reader);
            }
            string str2 = "";
            if ((table != null) && (table.Rows.Count > 0))
            {
                foreach (DataRow row in table.Rows)
                {
                    str2 = str2 + "_" + row["ProductId"].ToString();
                }
            }
            if (str2 != "")
            {
                str2 = str2.Remove(0, 1);
            }
            return str2;
        }

        public DbQueryResult GetMemberCoupons(MemberCouponsSearch search)
        {
            try
            {
                StringBuilder builder = new StringBuilder(" 1=1 ");
                if (!string.IsNullOrEmpty(search.CouponName))
                {
                    builder.Append(" and CouponName like '% " + search.CouponName.ReplaceSingleQuoteMark() + " %'  ");
                }
                if (!string.IsNullOrEmpty(search.OrderNo))
                {
                    builder.Append(" and OrderNo='" + search.OrderNo.ReplaceSingleQuoteMark() + "' ");
                }
                return DataHelper.PagingByRownumber(search.PageIndex, search.PageSize, search.SortBy, search.SortOrder, search.IsCount, "Hishop_Coupon_MemberCoupons", "Id", builder.ToString(), "*");
            }
            catch (Exception)
            {
                return null;
            }
        }

        public DataTable GetMemberCoupons(MemberCouponsSearch search, ref int total)
        {
            DataTable table3;
            try
            {
                StringBuilder builder = new StringBuilder(" 1=1 ");
                total = 0;
                if (!string.IsNullOrEmpty(search.CouponName))
                {
                    builder.Append(" and a.CouponName like '%" + search.CouponName.ReplaceSingleQuoteMark() + "%'  ");
                }
                if (!string.IsNullOrEmpty(search.OrderNo))
                {
                    builder.Append(" and OrderNo='" + search.OrderNo.ReplaceSingleQuoteMark() + "' ");
                }
                if (search.MemberId > 0)
                {
                    builder.Append(" and MemberId='" + search.MemberId.ToString() + "' ");
                }
                int result = 0;
                if (!string.IsNullOrEmpty(search.Status) && int.TryParse(search.Status, out result))
                {
                    builder.Append(" and a.Status='" + result.ToString() + "' ");
                }
                string str = "select count(id) as total from Hishop_Coupon_MemberCoupons a where  " + builder.ToString();
                DbCommand sqlStringCommand = this.database.GetSqlStringCommand(str);
                using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
                {
                    DataTable table = DataHelper.ConverDataReaderToDataTable(reader);
                    total = int.Parse(table.Rows[0][0].ToString());
                }
                if (total <= 0)
                {
                    return null;
                }
                int pageSize = 0;
                int num3 = search.PageIndex * search.PageSize;
                if ((search.PageIndex != 0) && (search.PageSize != 0))
                {
                    pageSize = search.PageSize;
                    if (search.PageIndex >= Math.Ceiling((double) (((double) total) / ((double) search.PageSize))))
                    {
                        search.PageIndex = int.Parse(Math.Ceiling((double) (((double) total) / ((double) search.PageSize))).ToString());
                    }
                    int num4 = search.PageIndex * search.PageSize;
                    if (num4 > total)
                    {
                        pageSize = search.PageSize - (num4 - total);
                    }
                }
                string str2 = "order by  a.CouponId desc ";
                string str3 = "order by  CouponId desc";
                string str4 = "order by  CouponId desc ";
                string str5 = string.Format("select top {0} a.* , b.userName,c.IsAllProduct  from Hishop_Coupon_MemberCoupons a inner join Hishop_Coupon_Coupons c on  c.CouponId=a.CouponId left join aspnet_Members b on a.memberid=b.userId  where ", num3);
                string str6 = string.Format("select * from ( select top {0} * from ( {1} ) as t1 {2} ) as t2 {3} ", new object[] { pageSize, str5 + builder.ToString() + str2, str4, str3 });
                sqlStringCommand = this.database.GetSqlStringCommand(str6);
                using (IDataReader reader2 = this.database.ExecuteReader(sqlStringCommand))
                {
                    table3 = DataHelper.ConverDataReaderToDataTable(reader2);
                }
            }
            catch (Exception)
            {
                table3 = null;
            }
            return table3;
        }

        public int GetMemberCouponsNumbyUserId(int UserId)
        {
            string str = "select count(id) as total from Hishop_Coupon_MemberCoupons where getdate()<=EndDate and Status=0 and MemberId=" + UserId.ToString();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(str);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return int.Parse(DataHelper.ConverDataReaderToDataTable(reader).Rows[0][0].ToString());
            }
        }

        public DataTable GetMemberWaitCoupons(int userid, int GradeId)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("SELECT c.*,c.EndDate as EndTime,c.BeginDate as StartTime FROM Hishop_Coupon_Coupons c");
            builder.AppendLine(" where GETDATE()<=c.EndDate and c.StockNum>0 and c.MemberGrades<=" + GradeId.ToString() + " and c.CouponId not in ");
            builder.AppendLine(" (select CouponId from Hishop_Coupon_MemberCoupons where MemberId=" + userid + ")");
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        public int GetMemberWaitCouponsNum(int userid, int GradeId)
        {
            DataTable table = null;
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("SELECT count(c.CouponId) FROM Hishop_Coupon_Coupons c ");
            builder.AppendLine(" where GETDATE()<=c.EndDate and  c.StockNum>0  and c.MemberGrades<=" + GradeId.ToString() + " and c.CouponId not in ");
            builder.AppendLine(" (select CouponId from Hishop_Coupon_MemberCoupons where  MemberId=" + userid + ")");
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                table = DataHelper.ConverDataReaderToDataTable(reader);
            }
            if ((table != null) && (table.Rows.Count > 0))
            {
                return int.Parse(table.Rows[0][0].ToString());
            }
            return 0;
        }

        public DbQueryResult GetNewCoupons(Pagination page)
        {
            return DataHelper.PagingByRownumber(page.PageIndex, page.PageSize, page.SortBy, page.SortOrder, page.IsCount, "Hishop_Coupon_Coupons", "CouponId", string.Empty, "*");
        }

        public DataTable GetUnFinishedCoupon(DateTime end)
        {
            string str = string.Format("select * from Hishop_Coupon_Coupons where EndDate>'{0}' and Finished=0 ", end.ToString("yyyy-MM-dd HH:mm:ss"));
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(str);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        public DataTable GetUserCoupons()
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT m.*,c.IsAllProduct,c.MemberGrades from  Hishop_Coupon_MemberCoupons as m left join Hishop_Coupon_Coupons as c on c.CouponId=m.CouponId WHERE m.MemberId = @UserId and m.BeginDate<=getdate() and getdate()<=m.EndDate and m.Status=0");
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, Globals.GetCurrentMemberUserId());
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        public DataTable GetUserCoupons(int userId, int useType = 0)
        {
            string str = "";
            if (useType == 1)
            {
                str = "AND ci.CouponStatus = 0 AND ci.UsedTime is NULL and c.ClosingTime > @ClosingTime";
            }
            else if (useType == 2)
            {
                str = " AND ci.UsedTime is not NULL and c.ClosingTime > @ClosingTime";
            }
            else if (useType == 3)
            {
                str = " AND c.ClosingTime<getdate()";
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT c.*, ci.ClaimCode,ci.CouponStatus  FROM Hishop_CouponItems ci INNER JOIN Hishop_Coupon_Coupons c ON c.CouponId = ci.CouponId WHERE ci.UserId = @UserId " + str);
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
            this.database.AddInParameter(sqlStringCommand, "ClosingTime", DbType.DateTime, DateTime.Now);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        public SendCouponResult IsCanSendCouponToMember(int couponId, int userId)
        {
            DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_MemberCanReceiveCoupon");
            this.database.AddInParameter(storedProcCommand, "@CouponsId", DbType.Int32, couponId);
            this.database.AddInParameter(storedProcCommand, "@UserId", DbType.Int32, userId);
            this.database.AddOutParameter(storedProcCommand, "@Result", DbType.Int32, 4);
            try
            {
                this.database.ExecuteNonQuery(storedProcCommand);
                object obj2 = storedProcCommand.Parameters["@Result"].Value;
                if ((obj2 != null) && !string.IsNullOrEmpty(obj2.ToString()))
                {
                    return (SendCouponResult) int.Parse(obj2.ToString());
                }
            }
            catch (Exception)
            {
                throw;
            }
            return SendCouponResult.其它错误;
        }

        public bool SendClaimCodes(int couponId, CouponItemInfo couponItem)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Hishop_CouponItems(CouponId, ClaimCode,LotNumber, GenerateTime, UserId,UserName,EmailAddress,CouponStatus) VALUES(@CouponId, @ClaimCode,@LotNumber, @GenerateTime, @UserId, @UserName,@EmailAddress,@CouponStatus)");
            this.database.AddInParameter(sqlStringCommand, "CouponId", DbType.Int32, couponId);
            this.database.AddInParameter(sqlStringCommand, "ClaimCode", DbType.String, couponItem.ClaimCode);
            this.database.AddInParameter(sqlStringCommand, "GenerateTime", DbType.DateTime, couponItem.GenerateTime);
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32);
            this.database.AddInParameter(sqlStringCommand, "UserName", DbType.String);
            this.database.AddInParameter(sqlStringCommand, "LotNumber", DbType.Guid, Guid.NewGuid());
            if (couponItem.UserId.HasValue)
            {
                this.database.SetParameterValue(sqlStringCommand, "UserId", couponItem.UserId.Value);
            }
            else
            {
                this.database.SetParameterValue(sqlStringCommand, "UserId", DBNull.Value);
            }
            if (!string.IsNullOrEmpty(couponItem.UserName))
            {
                this.database.SetParameterValue(sqlStringCommand, "UserName", couponItem.UserName);
            }
            else
            {
                this.database.SetParameterValue(sqlStringCommand, "UserName", DBNull.Value);
            }
            this.database.AddInParameter(sqlStringCommand, "EmailAddress", DbType.String, couponItem.EmailAddress);
            this.database.AddInParameter(sqlStringCommand, "CouponStatus", DbType.String, 0);
            return (this.database.ExecuteNonQuery(sqlStringCommand) >= 1);
        }

        public SendCouponResult SendCouponToMember(int couponId, int userId)
        {
            DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_SendCouponToMember");
            this.database.AddInParameter(storedProcCommand, "@CouponsId", DbType.Int32, couponId);
            this.database.AddInParameter(storedProcCommand, "@UserId", DbType.Int32, userId);
            this.database.AddOutParameter(storedProcCommand, "@Result", DbType.Int32, 4);
            try
            {
                this.database.ExecuteNonQuery(storedProcCommand);
                object obj2 = storedProcCommand.Parameters["@Result"].Value;
                if ((obj2 != null) && !string.IsNullOrEmpty(obj2.ToString()))
                {
                    return (SendCouponResult) int.Parse(obj2.ToString());
                }
            }
            catch (Exception)
            {
                throw;
            }
            return SendCouponResult.其它错误;
        }

        public bool setCouponFinished(int couponId, bool bfinished)
        {
            try
            {
                DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT CouponId  FROM Hishop_Coupon_Coupons WHERE  CouponId=@CouponId ");
                this.database.AddInParameter(sqlStringCommand, "CouponId", DbType.Int32, couponId);
                if (Convert.ToInt32(this.database.ExecuteScalar(sqlStringCommand)) >= 1)
                {
                    sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_Coupon_Coupons SET Finished=@Finished WHERE CouponId=@CouponId");
                    this.database.AddInParameter(sqlStringCommand, "Finished", DbType.Boolean, bfinished);
                    this.database.AddInParameter(sqlStringCommand, "CouponId", DbType.Int32, couponId);
                    this.database.ExecuteNonQuery(sqlStringCommand);
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool SetProductsStatus(int couponId, int status, string ProductIds)
        {
            try
            {
                DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("Update Hishop_Coupon_Products set status={0}   WHERE CouponId ={1} and ProductId in ( {2} )", status, couponId, ProductIds.ReplaceSingleQuoteMark()));
                return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
            }
            catch
            {
                return false;
            }
        }

        public bool UpdateCoupon(int couponId, CouponEdit content, ref string msg)
        {
            try
            {
                CouponInfo couponDetails = this.GetCouponDetails(couponId);
                int num = 0;
                if (couponDetails != null)
                {
                    int receiveNum = couponDetails.ReceiveNum;
                    string str = "";
                    if (content.maxReceivNum.HasValue)
                    {
                        str = str + string.Format(", maxReceivNum={0}", content.maxReceivNum);
                    }
                    if (content.totalNum.HasValue)
                    {
                        num = content.totalNum.Value - (couponDetails.StockNum + couponDetails.ReceiveNum);
                        if ((num + couponDetails.StockNum) <= 0)
                        {
                            msg = "修改的优惠券总量少于已发放的数量";
                            return false;
                        }
                        str = str + string.Format(",  StockNum=StockNum + {0}", num);
                    }
                    if (content.begin.HasValue)
                    {
                        str = str + string.Format(", BeginDate='{0}'", content.begin.Value.ToString("yyyy-MM-dd HH:mm:ss"));
                    }
                    if (content.end.HasValue)
                    {
                        str = str + string.Format(", EndDate='{0}'", content.end.Value.ToString("yyyy-MM-dd HH:mm:ss"));
                    }
                    if (str.Length > 1)
                    {
                        DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("UPDATE Hishop_Coupon_Coupons SET {0} WHERE CouponId={1}", str.Substring(1), couponId));
                        this.database.ExecuteNonQuery(sqlStringCommand);
                        if (num < 0)
                        {
                            sqlStringCommand = this.database.GetSqlStringCommand(string.Format("UPDATE Hishop_Coupon_Coupons SET stockNum=0 WHERE stockNum<0  and CouponId={0}", couponId));
                            this.database.ExecuteNonQuery(sqlStringCommand);
                        }
                        return true;
                    }
                    msg = "没有找到编辑内容";
                    return false;
                }
                msg = "没有这个优惠券";
                return false;
            }
            catch (Exception exception)
            {
                msg = "编辑优惠券失败(" + exception.Message + ")";
                return false;
            }
        }
    }
}

