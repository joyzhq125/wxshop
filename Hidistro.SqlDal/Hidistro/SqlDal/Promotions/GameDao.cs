namespace Hidistro.SqlDal.Promotions
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities;
    using Hidistro.Entities.Promotions;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Runtime.InteropServices;
    using System.Text;

    public class GameDao
    {
        private Database _database = DatabaseFactory.CreateDatabase();

        public bool Create(GameInfo model, out int gameId)
        {
            gameId = -1;
            StringBuilder builder = new StringBuilder();
            builder.Append("INSERT INTO [Hishop_PromotionGame]\r\n                       ([GameType]\r\n                       ,[GameTitle]\r\n                       ,[Description]\r\n                       ,[BeginTime]\r\n                       ,[EndTime]\r\n                       ,[ApplyMembers]\r\n                       ,[NeedPoint]\r\n                       ,[GivePoint]\r\n                       ,[OnlyGiveNotPrizeMember]\r\n                       ,[PlayType]\r\n                       ,[NotPrzeDescription]\r\n                       ,[GameUrl]\r\n                       ,[GameQRCodeAddress]\r\n                       ,[Status],[KeyWork])\r\n                        VALUES ");
            builder.Append("(@GameType\r\n                       ,@GameTitle\r\n                       ,@Description\r\n                       ,@BeginTime\r\n                       ,@EndTime\r\n                       ,@ApplyMembers\r\n                       ,@NeedPoint\r\n                       ,@GivePoint\r\n                       ,@OnlyGiveNotPrizeMember\r\n                       ,@PlayType\r\n                       ,@NotPrzeDescription\r\n                       ,@GameUrl\r\n                       ,@GameQRCodeAddress\r\n                       ,@Status,@KeyWork);");
            builder.Append("select @@identity;");
            DbCommand sqlStringCommand = this._database.GetSqlStringCommand(builder.ToString());
            this._database.AddInParameter(sqlStringCommand, "@GameType", DbType.Int32, (int) model.GameType);
            this._database.AddInParameter(sqlStringCommand, "@GameTitle", DbType.String, model.GameTitle);
            this._database.AddInParameter(sqlStringCommand, "@Description", DbType.String, model.Description);
            this._database.AddInParameter(sqlStringCommand, "@BeginTime", DbType.DateTime, model.BeginTime);
            this._database.AddInParameter(sqlStringCommand, "@EndTime", DbType.DateTime, model.EndTime);
            this._database.AddInParameter(sqlStringCommand, "@ApplyMembers", DbType.String, model.ApplyMembers);
            this._database.AddInParameter(sqlStringCommand, "@NeedPoint", DbType.Int32, model.NeedPoint);
            this._database.AddInParameter(sqlStringCommand, "@GivePoint", DbType.Int32, model.GivePoint);
            this._database.AddInParameter(sqlStringCommand, "@OnlyGiveNotPrizeMember", DbType.Boolean, model.OnlyGiveNotPrizeMember);
            this._database.AddInParameter(sqlStringCommand, "@PlayType", DbType.Int32, (int) model.PlayType);
            this._database.AddInParameter(sqlStringCommand, "@NotPrzeDescription", DbType.String, model.NotPrzeDescription);
            this._database.AddInParameter(sqlStringCommand, "@GameUrl", DbType.String, model.GameUrl);
            this._database.AddInParameter(sqlStringCommand, "@GameQRCodeAddress", DbType.String, model.GameQRCodeAddress);
            this._database.AddInParameter(sqlStringCommand, "@Status", DbType.Int32, 0);
            this._database.AddInParameter(sqlStringCommand, "@KeyWork", DbType.String, model.KeyWork);
            try
            {
                object obj2 = this._database.ExecuteScalar(sqlStringCommand);
                if ((obj2 != null) && !int.TryParse(obj2.ToString(), out gameId))
                {
                    gameId = -1;
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return (gameId > 0);
        }

        public bool Delete(params int[] gameIds)
        {
            string str = string.Join<int>(",", gameIds);
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("Delete From Hishop_PromotionGameResultMembersLog where GameId in({0});", str);
            builder.AppendFormat("Delete From Hishop_PromotionGamePrizes where GameId in({0});", str);
            builder.AppendFormat("Delete From Hishop_PromotionGame where GameId in({0});", str);
            DbCommand sqlStringCommand = this._database.GetSqlStringCommand(builder.ToString());
            return (this._database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool DeletePrizesDelivery(int[] ids)
        {
            string str = "delete from Hishop_PromotionGameResultMembersLog where LogId in(select LogId from Hishop_PrizesDeliveryRecord where id in(" + string.Join<int>(",", ids) + "));delete from Hishop_PrizesDeliveryRecord where id in(" + string.Join<int>(",", ids) + ")";
            DbCommand sqlStringCommand = this._database.GetSqlStringCommand(str);
            return (this._database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public DbQueryResult GetGameList(GameSearch search)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(" 1=1");
            if (search.GameType.HasValue)
            {
                builder.AppendFormat(" and [GameType]={0}", search.GameType);
            }
            if (!string.IsNullOrEmpty(search.Status))
            {
                builder.AppendFormat(" and [Status]={0}", search.Status);
            }
            if (search.BeginTime.HasValue)
            {
                builder.AppendFormat(" and [BeginTime]>='{0}'", search.BeginTime);
            }
            if (search.EndTime.HasValue)
            {
                builder.AppendFormat(" and [EndTime]<'{0}'", search.EndTime);
            }
            string selectFields = "GameId, GameType, GameTitle, Description, BeginTime, EndTime, ApplyMembers, NeedPoint, GivePoint, OnlyGiveNotPrizeMember, PlayType, NotPrzeDescription, GameUrl, GameQRCodeAddress, Status";
            return DataHelper.PagingByTopnotin(search.PageIndex, search.PageSize, search.SortBy, search.SortOrder, search.IsCount, "Hishop_PromotionGame", "GameId", builder.ToString(), selectFields);
        }

        public DbQueryResult GetGameListByView(GameSearch search)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(" 1=1");
            if (search.GameType.HasValue)
            {
                builder.AppendFormat(" and [GameType]={0}", search.GameType);
            }
            if (!string.IsNullOrEmpty(search.Status))
            {
                builder.AppendFormat(" and [Status]={0}", search.Status);
            }
            if (search.BeginTime.HasValue)
            {
                builder.AppendFormat(" and [BeginTime]>='{0}'", search.BeginTime.Value.ToString("yyyy-MM-dd"));
            }
            if (search.EndTime.HasValue)
            {
                builder.AppendFormat(" and [EndTime]<'{0}'", search.EndTime.Value.AddDays(1.0).ToString("yyyy-MM-dd"));
            }
            string selectFields = " GameID, GameType,GameTitle, BeginTime ,EndTime,PlayType,GameUrl,GameQRCodeAddress ,Status,TotalCount,PrizeCount ";
            return DataHelper.PagingByTopnotin(search.PageIndex, search.PageSize, search.SortBy, search.SortOrder, search.IsCount, "vw_Hishop_PromotionGame", "GameId", builder.ToString(), selectFields);
        }

        public IEnumerable<GameInfo> GetLists(GameSearch search)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("SELECT * FROM [vw_Hishop_PromotionGame] where 1=1 ");
            if (!string.IsNullOrEmpty(search.Status))
            {
                builder.AppendFormat(" and [Status]={0}", search.Status);
            }
            if (search.BeginTime.HasValue)
            {
                builder.AppendFormat(" and [BeginTime]>='{0}'", search.BeginTime);
            }
            if (search.EndTime.HasValue)
            {
                builder.AppendFormat(" and [EndTime]<'{0}'", search.EndTime);
            }
            DbCommand sqlStringCommand = this._database.GetSqlStringCommand(builder.ToString());
            using (IDataReader reader = this._database.ExecuteReader(sqlStringCommand))
            {
                return ReaderConvert.ReaderToList<GameInfo>(reader);
            }
        }

        public GameInfo GetModelByGameId(int gameId)
        {
            string str = "SELECT * FROM [Hishop_PromotionGame] where GameId=@GameId";
            DbCommand sqlStringCommand = this._database.GetSqlStringCommand(str);
            this._database.AddInParameter(sqlStringCommand, "@GameId", DbType.Int32, gameId);
            using (IDataReader reader = this._database.ExecuteReader(sqlStringCommand))
            {
                return ReaderConvert.ReaderToModel<GameInfo>(reader);
            }
        }

        public GameInfo GetModelByGameId(string keyWord)
        {
            string str = "SELECT * FROM [Hishop_PromotionGame] where KeyWork=@KeyWork";
            DbCommand sqlStringCommand = this._database.GetSqlStringCommand(str);
            this._database.AddInParameter(sqlStringCommand, "@KeyWork", DbType.String, keyWord);
            using (IDataReader reader = this._database.ExecuteReader(sqlStringCommand))
            {
                return ReaderConvert.ReaderToModel<GameInfo>(reader);
            }
        }

        public DbQueryResult GetPrizesDeliveryList(PrizesDeliveQuery query, string ExtendLimits = "", string selectFields = "*")
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(" 1=1 ");
            if (query.PrizeType > -1)
            {
                builder.Append(" AND ");
                builder.AppendFormat(" PrizeType = {0}", query.PrizeType);
            }
            if (query.Id > 0)
            {
                builder.Append(" AND ");
                builder.AppendFormat(" Id = {0}", query.Id);
            }
            if (query.UserId > 0)
            {
                builder.Append(" AND ");
                builder.AppendFormat(" UserId = {0}", query.UserId);
            }
            if (!string.IsNullOrEmpty(query.LogId))
            {
                builder.Append(" AND ");
                builder.AppendFormat(" LogId = {0}", query.LogId);
            }
            if (query.Status >= 0)
            {
                builder.Append(" AND ");
                builder.AppendFormat("status = {0}", query.Status);
            }
            if (!string.IsNullOrEmpty(query.ProductName))
            {
                builder.Append(" AND ");
                builder.AppendFormat(" ProductName like '%{0}%'", DataHelper.CleanSearchString(query.ProductName));
            }
            if (!string.IsNullOrEmpty(query.Receiver))
            {
                builder.Append(" AND ");
                builder.AppendFormat("( Receiver = '{0}'", DataHelper.CleanSearchString(query.Receiver));
                builder.AppendFormat(" OR RealName = '{0}' )", DataHelper.CleanSearchString(query.Receiver));
            }
            if (!string.IsNullOrEmpty(query.ReggionId))
            {
                builder.Append(" AND ");
                builder.AppendFormat(" ',' + ReggionPath + ',' like '%,{0},%'", DataHelper.CleanSearchString(query.ReggionId));
            }
            DateTime now = DateTime.Now;
            if (DateTime.TryParse(query.StartDate, out now))
            {
                builder.Append(" AND ");
                builder.AppendFormat(" PlayTime>='{0}'", now.ToString("yyyy-MM-dd 00:00:00"));
            }
            if (DateTime.TryParse(query.EndDate, out now))
            {
                builder.Append(" AND ");
                builder.AppendFormat(" PlayTime<='{0}'", now.ToString("yyyy-MM-dd 23:59:59"));
            }
            if (ExtendLimits != "")
            {
                builder.Append(ExtendLimits);
            }
            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Hishop_PrizesDeliveryRecord", "LogId", (builder.Length > 0) ? builder.ToString() : null, selectFields);
        }

        public DataTable GetPrizesDeliveryNum()
        {
            DbCommand sqlStringCommand = this._database.GetSqlStringCommand("Select (SELECT count(LogId) FROM vw_Hishop_PrizesDeliveryRecord where status=0 and PrizeType=2) st0,(SELECT count(LogId) FROM vw_Hishop_PrizesDeliveryRecord where status=1 and  PrizeType=2) st1,(SELECT count(LogId) FROM vw_Hishop_PrizesDeliveryRecord where status=2 and  PrizeType=2) st2,(SELECT count(LogId) FROM vw_Hishop_PrizesDeliveryRecord where status=3 and  PrizeType=2) st3");
            using (IDataReader reader = this._database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        public bool Update(GameInfo model)
        {
            bool flag;
            StringBuilder builder = new StringBuilder();
            builder.Append("Update [Hishop_PromotionGame] set [GameTitle]=@GameTitle\r\n                       ,[Description]=@Description\r\n                       ,[BeginTime]=@BeginTime\r\n                       ,[EndTime]=@EndTime\r\n                       ,[ApplyMembers]=@ApplyMembers\r\n                       ,[NeedPoint]=@NeedPoint\r\n                       ,[GivePoint]=@GivePoint\r\n                       ,[OnlyGiveNotPrizeMember]=@OnlyGiveNotPrizeMember\r\n                       ,[PlayType]=@PlayType\r\n                       ,[NotPrzeDescription]=@NotPrzeDescription\r\n                       ,[GameUrl]=@GameUrl\r\n                       ,[GameQRCodeAddress]=@GameQRCodeAddress\r\n                      \r\n                       Where GameId=@GameId\r\n                        ");
            DbCommand sqlStringCommand = this._database.GetSqlStringCommand(builder.ToString());
            this._database.AddInParameter(sqlStringCommand, "@GameTitle", DbType.String, model.GameTitle);
            this._database.AddInParameter(sqlStringCommand, "@Description", DbType.String, model.Description);
            this._database.AddInParameter(sqlStringCommand, "@BeginTime", DbType.DateTime, model.BeginTime);
            this._database.AddInParameter(sqlStringCommand, "@EndTime", DbType.DateTime, model.EndTime);
            this._database.AddInParameter(sqlStringCommand, "@ApplyMembers", DbType.String, model.ApplyMembers);
            this._database.AddInParameter(sqlStringCommand, "@NeedPoint", DbType.Int32, model.NeedPoint);
            this._database.AddInParameter(sqlStringCommand, "@GivePoint", DbType.Int32, model.GivePoint);
            this._database.AddInParameter(sqlStringCommand, "@OnlyGiveNotPrizeMember", DbType.Boolean, model.OnlyGiveNotPrizeMember);
            this._database.AddInParameter(sqlStringCommand, "@PlayType", DbType.Int32, (int) model.PlayType);
            this._database.AddInParameter(sqlStringCommand, "@NotPrzeDescription", DbType.String, model.NotPrzeDescription);
            this._database.AddInParameter(sqlStringCommand, "@GameUrl", DbType.String, model.GameUrl);
            this._database.AddInParameter(sqlStringCommand, "@GameQRCodeAddress", DbType.String, model.GameQRCodeAddress);
            this._database.AddInParameter(sqlStringCommand, "@GameId", DbType.Int32, model.GameId);
            try
            {
                flag = this._database.ExecuteNonQuery(sqlStringCommand) > 0;
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return flag;
        }

        public bool UpdatePrizesDelivery(PrizesDeliveQuery query)
        {
            DbCommand sqlStringCommand;
            StringBuilder builder = new StringBuilder();
            if (query.Id == 0)
            {
                builder.Append("insert into Hishop_PrizesDeliveryRecord");
                builder.Append("(Receiver,Tel,LogId,ReggionPath,Address,status)VALUES");
                builder.Append("(@Receiver,@Tel,@LogId,@ReggionPath,@Address,@Status)");
                sqlStringCommand = this._database.GetSqlStringCommand(builder.ToString());
                this._database.AddInParameter(sqlStringCommand, "@Receiver", DbType.String, query.Receiver);
                this._database.AddInParameter(sqlStringCommand, "@Tel", DbType.String, query.Tel);
                this._database.AddInParameter(sqlStringCommand, "@LogId", DbType.Int32, query.LogId);
                this._database.AddInParameter(sqlStringCommand, "@ReggionPath", DbType.String, query.ReggionPath);
                this._database.AddInParameter(sqlStringCommand, "@Address", DbType.String, query.Address);
                if (((query.Address.Length > 5) && (query.ReggionPath.Length > 0)) && ((query.Receiver.Length > 1) && (query.Tel.Length > 7)))
                {
                    this._database.AddInParameter(sqlStringCommand, "@Status", DbType.Int16, 1);
                }
                else
                {
                    this._database.AddInParameter(sqlStringCommand, "@Status", DbType.Int16, 0);
                }
            }
            else
            {
                builder.Append("Update Hishop_PrizesDeliveryRecord ");
                if (query.Status == 0)
                {
                    builder.Append("set status=status");
                }
                else
                {
                    builder.Append("set Status=@Status");
                }
                if (!string.IsNullOrEmpty(query.Receiver))
                {
                    builder.Append(",Receiver=@Receiver");
                }
                if (!string.IsNullOrEmpty(query.Tel))
                {
                    builder.Append(",Tel=@Tel");
                }
                if (!string.IsNullOrEmpty(query.ReggionPath))
                {
                    builder.Append(",ReggionPath=@ReggionPath");
                }
                if (!string.IsNullOrEmpty(query.ExpressName))
                {
                    builder.Append(",ExpressName=@ExpressName");
                }
                if (!string.IsNullOrEmpty(query.CourierNumber))
                {
                    builder.Append(",CourierNumber=@CourierNumber");
                }
                if (!string.IsNullOrEmpty(query.Address))
                {
                    builder.Append(",Address=@Address");
                }
                DateTime now = DateTime.Now;
                if (DateTime.TryParse(query.DeliveryTime, out now))
                {
                    builder.AppendFormat(",DeliveryTime='{0}'", now.ToString("yyyy-MM-dd 00:00:00"));
                }
                if (DateTime.TryParse(query.ReceiveTime, out now))
                {
                    builder.AppendFormat(",ReceiveTime='{0}'", now.ToString("yyyy-MM-dd 00:00:00"));
                }
                builder.Append(" where Id=@Id");
                if (query.Status == 3)
                {
                    builder.Append(";update Hishop_PromotionGameResultMembersLog set IsUsed=1 where LogId=" + query.LogId);
                }
                sqlStringCommand = this._database.GetSqlStringCommand(builder.ToString());
                this._database.AddInParameter(sqlStringCommand, "@Status", DbType.Int16, query.Status);
                this._database.AddInParameter(sqlStringCommand, "@Address", DbType.String, query.Address);
                this._database.AddInParameter(sqlStringCommand, "@ExpressName", DbType.String, query.ExpressName);
                this._database.AddInParameter(sqlStringCommand, "@CourierNumber", DbType.String, query.CourierNumber);
                this._database.AddInParameter(sqlStringCommand, "@ReggionPath", DbType.String, query.ReggionPath);
                this._database.AddInParameter(sqlStringCommand, "@Receiver", DbType.String, query.Receiver);
                this._database.AddInParameter(sqlStringCommand, "@Tel", DbType.String, query.Tel);
                this._database.AddInParameter(sqlStringCommand, "@Id", DbType.Int32, query.Id);
            }
            return (this._database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool UpdateStatus(int gameId, GameStatus status)
        {
            string str = "Update Hishop_PromotionGame set [Status]=@Status where GameId=@GameId";
            DbCommand sqlStringCommand = this._database.GetSqlStringCommand(str);
            this._database.AddInParameter(sqlStringCommand, "@Status", DbType.Int32, (int) status);
            this._database.AddInParameter(sqlStringCommand, "@GameId", DbType.Int32, gameId);
            return (this._database.ExecuteNonQuery(sqlStringCommand) > 0);
        }
    }
}

