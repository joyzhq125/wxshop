namespace Hidistro.SqlDal.Weibo
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.VShop;
    using Hidistro.Entities.WeiXin;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Data;
    using System.Data.Common;
    using System.Text;

    public class SendAllDao
    {
        private Database database = DatabaseFactory.CreateDatabase();

        public string ClearWeiXinMediaID(string wid)
        {
            string str = "update vshop_Article set mediaid=null where len(mediaid)>0 and wid=@wid;update vshop_ArticleItems set mediaid=null where len(mediaid)>0;delete from WeiXin_RecentOpenID";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(str);
            this.database.AddInParameter(sqlStringCommand, "wid", DbType.String, wid);
            return this.database.ExecuteNonQuery(sqlStringCommand).ToString();
        }

        /// <summary>
        /// 24小时内访问公众号的openid
        /// </summary>
        /// <param name="topnum"></param>
        /// <param name="wid"></param>
        /// <returns></returns>
        public DataTable GetRencentOpenID(int topnum,string wid)
        {
            int num = topnum;
            if (num < 1)
            {
                num = 1;
            }
            string str = "select top " + num + " OpenID from WeiXin_RecentOpenID where DATEADD(day,2, PubTime)>getdate() and wid=@wid order by PubTime desc";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(str);
            this.database.AddInParameter(sqlStringCommand, "wid", DbType.String, wid);
            return this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
        }

        public SendAllInfo GetSendAllInfo(int sendID)
        {
            SendAllInfo info = null;
            if (sendID > 0)
            {
                DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM WeiXin_SendAll WHERE ID = @ID");
                this.database.AddInParameter(sqlStringCommand, "ID", DbType.Int32, sendID);
                using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
                {
                    info = new SendAllInfo();
                    if (!reader.Read())
                    {
                        return info;
                    }
                    info.Id = sendID;
                    info.Title = reader["Title"].ToString();
                    object obj2 = reader["MessageType"];
                    if ((obj2 != null) && (obj2 != DBNull.Value))
                    {
                        info.MessageType = (MessageType) obj2;
                    }
                    obj2 = reader["ArticleID"];
                    if ((obj2 != null) && (obj2 != DBNull.Value))
                    {
                        info.ArticleID = (int) obj2;
                    }
                    info.Content = reader["Content"].ToString();
                    obj2 = reader["SendState"];
                    if ((obj2 != null) && (obj2 != DBNull.Value))
                    {
                        info.SendState = (int) obj2;
                    }
                    obj2 = reader["SendTime"];
                    if ((obj2 != null) && (obj2 != DBNull.Value))
                    {
                        info.SendTime = (DateTime) obj2;
                    }
                    obj2 = reader["SendCount"];
                    if ((obj2 != null) && (obj2 != DBNull.Value))
                    {
                        info.SendCount = (int) obj2;
                    }
                    obj2 = reader["msgid"];
                    if ((obj2 != null) && (obj2 != DBNull.Value))
                    {
                        info.MsgID = obj2.ToString();
                    }
                }
            }
            return info;
        }

        public DbQueryResult GetSendAllRequest(SendAllQuery query)
        {
            StringBuilder builder = new StringBuilder("1=1");
            if (!string.IsNullOrEmpty(query.Title))
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat(" Title LIKE '%{0}%' )", DataHelper.CleanSearchString(query.Title));
            }
            builder.AppendFormat(" and wid = '{0}'", query.wid);
            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "WeiXin_SendAll ", "ID", (builder.Length > 0) ? builder.ToString() : null, "*");
        }

        public string SaveSendAllInfo(SendAllInfo sendAllInfo)
        {
            string str = string.Empty;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO WeiXin_SendAll (Title,MessageType,ArticleID,Content,SendState,SendTime,SendCount,wid) VALUES(@Title,@MessageType,@ArticleID,@Content,@SendState,@SendTime,@SendCount,@wid);select @@identity");
            this.database.AddInParameter(sqlStringCommand, "Title", DbType.String, sendAllInfo.Title);
            this.database.AddInParameter(sqlStringCommand, "MessageType", DbType.Int32, (int) sendAllInfo.MessageType);
            this.database.AddInParameter(sqlStringCommand, "ArticleID", DbType.Int32, sendAllInfo.ArticleID);
            this.database.AddInParameter(sqlStringCommand, "Content", DbType.String, sendAllInfo.Content);
            this.database.AddInParameter(sqlStringCommand, "SendState", DbType.Int32, sendAllInfo.SendState);
            this.database.AddInParameter(sqlStringCommand, "SendTime", DbType.DateTime, DateTime.Now.ToString());
            this.database.AddInParameter(sqlStringCommand, "SendCount", DbType.Int32, sendAllInfo.SendCount);
            this.database.AddInParameter(sqlStringCommand, "wid", DbType.String, sendAllInfo.wid);
            int num = Globals.ToNum(this.database.ExecuteScalar(sqlStringCommand));
            if (num > 0)
            {
                str = num.ToString();
            }
            return str;
        }

        public bool UpdateMsgId(int id, string msgid, int sendstate, int sendcount, int totalcount, string returnjsondata)
        {
            StringBuilder builder = new StringBuilder();
            if (id > 0)
            {
                builder.Append("UPDATE WeiXin_SendAll SET msgid=@msgid,sendstate=@sendstate,sendcount=@sendcount,totalcount=@totalcount,returnjsondata=@returnjsondata WHERE ID=@ID");
            }
            else if (msgid.Length > 0)
            {
                builder.Append("UPDATE WeiXin_SendAll SET sendstate=@sendstate,sendcount=@sendcount,totalcount=@totalcount,returnjsondata=@returnjsondata WHERE msgid=@msgid and sendcount=0");
            }
            if (!string.IsNullOrEmpty(builder.ToString()))
            {
                DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
                this.database.AddInParameter(sqlStringCommand, "ID", DbType.Int32, id);
                this.database.AddInParameter(sqlStringCommand, "msgid", DbType.String, msgid);
                this.database.AddInParameter(sqlStringCommand, "sendstate", DbType.Int32, sendstate);
                this.database.AddInParameter(sqlStringCommand, "sendcount", DbType.Int32, sendcount);
                this.database.AddInParameter(sqlStringCommand, "totalcount", DbType.Int32, totalcount);
                this.database.AddInParameter(sqlStringCommand, "returnjsondata", DbType.String, returnjsondata);
                return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
            }
            return false;
        }

        public int UpdateRencentOpenID(string openid,string wid)
        {
            string str = "delete from WeiXin_RecentOpenID where OpenID=@OpenID;insert into WeiXin_RecentOpenID(OpenID,wid)values(@OpenID,@wid)";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(str);
            this.database.AddInParameter(sqlStringCommand, "OpenID", DbType.String, openid);
            this.database.AddInParameter(sqlStringCommand, "wid", DbType.String, wid);
            return this.database.ExecuteNonQuery(sqlStringCommand);
        }
    }
}

