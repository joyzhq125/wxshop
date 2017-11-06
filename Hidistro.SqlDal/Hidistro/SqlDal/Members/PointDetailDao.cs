namespace Hidistro.SqlDal.Members
{
    using Hidistro.Entities.Members;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Data;
    using System.Data.Common;
    using System.IO;

    public class PointDetailDao
    {
        private Database database = DatabaseFactory.CreateDatabase();

        public bool AddPointDetail(PointDetailInfo point)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Hishop_PointDetails (OrderId,UserId, TradeDate, TradeType, Increased, Reduced, Points, Remark)VALUES(@OrderId,@UserId, @TradeDate, @TradeType, @Increased, @Reduced, @Points, @Remark)");
            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, point.OrderId);
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, point.UserId);
            this.database.AddInParameter(sqlStringCommand, "TradeDate", DbType.DateTime, point.TradeDate);
            this.database.AddInParameter(sqlStringCommand, "TradeType", DbType.Int32, (int) point.TradeType);
            this.database.AddInParameter(sqlStringCommand, "Increased", DbType.Int32, point.Increased.HasValue ? point.Increased.Value : 0);
            this.database.AddInParameter(sqlStringCommand, "Reduced", DbType.Int32, point.Reduced.HasValue ? point.Reduced.Value : 0);
            this.database.AddInParameter(sqlStringCommand, "Points", DbType.Int32, point.Points);
            this.database.AddInParameter(sqlStringCommand, "Remark", DbType.String, point.Remark);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public int GetHistoryPoint(int userId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT SUM(Increased) FROM Hishop_PointDetails WHERE UserId = @UserId");
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
            return (int) this.database.ExecuteScalar(sqlStringCommand);
        }

        public decimal GetIntegral(int userId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT SUM(IntegralChange) FROM vshop_IntegralDetail WHERE Userid = @Userid and IntegralChange>0");
            this.database.AddInParameter(sqlStringCommand, "Userid", DbType.Int32, userId);
            object obj2 = this.database.ExecuteScalar(sqlStringCommand);
            if ((obj2 != null) && !string.IsNullOrEmpty(obj2.ToString()))
            {
                return (decimal) obj2;
            }
            return 0M;
        }

        public bool UpdateMembersPoint(int UserId, int addpoint)
        {
            if ((UserId == 0) || (addpoint == 0))
            {
                return false;
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE aspnet_Members set Points=Points+@Points where UserId=@UserId ");
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, UserId);
            this.database.AddInParameter(sqlStringCommand, "Points", DbType.Int32, addpoint);
            int num = this.database.ExecuteNonQuery(sqlStringCommand);
            File.WriteAllText(@"D:\t.txt", num.ToString());
            return (num > 0);
        }
    }
}

