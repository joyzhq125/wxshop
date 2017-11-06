namespace Hidistro.SqlDal.Members
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities;
    using Hidistro.Entities.Members;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Runtime.InteropServices;
    using System.Text;

    public class MemberDao
    {
        private Database database = DatabaseFactory.CreateDatabase();

        public bool BindUserName(int UserId, string UserBindName, string Password)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE aspnet_Members SET UserBindName = @UserBindName, Password = @Password WHERE UserId = @UserId");
            this.database.AddInParameter(sqlStringCommand, "UserBindName", DbType.String, UserBindName);
            this.database.AddInParameter(sqlStringCommand, "Password", DbType.String, Password);
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, UserId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool CreateMember(MemberInfo member)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO aspnet_Members(GradeId,ReferralUserId,UserName,CreateDate,OrderNumber,Expenditure,Points,TopRegionId, RegionId,OpenId, SessionId, SessionEndTime,Password,UserHead,UserBindName,Status,wid) VALUES(@GradeId,@ReferralUserId,@UserName,@CreateDate,0,0,0,0,0,@OpenId, @SessionId, @SessionEndTime,@Password,@UserHead,@UserBindName,@Status,@wid)");
            this.database.AddInParameter(sqlStringCommand, "GradeId", DbType.Int32, member.GradeId);
            this.database.AddInParameter(sqlStringCommand, "ReferralUserId", DbType.Int32, member.ReferralUserId);
            this.database.AddInParameter(sqlStringCommand, "UserName", DbType.String, member.UserName);
            this.database.AddInParameter(sqlStringCommand, "CreateDate", DbType.DateTime, member.CreateDate);
            this.database.AddInParameter(sqlStringCommand, "OpenId", DbType.String, member.OpenId);
            this.database.AddInParameter(sqlStringCommand, "SessionId", DbType.String, member.SessionId);
            this.database.AddInParameter(sqlStringCommand, "SessionEndTime", DbType.DateTime, member.SessionEndTime);
            this.database.AddInParameter(sqlStringCommand, "Password", DbType.String, member.Password);
            this.database.AddInParameter(sqlStringCommand, "UserHead", DbType.String, member.UserHead);
            this.database.AddInParameter(sqlStringCommand, "UserBindName", DbType.String, member.UserBindName);
            this.database.AddInParameter(sqlStringCommand, "Status", DbType.Int32, Convert.ToInt32(UserStatus.Normal));
            this.database.AddInParameter(sqlStringCommand, "wid", DbType.String, member.wid);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool Delete(int userId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM aspnet_Members WHERE UserId = @UserId");
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool Delete2(int userId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE  aspnet_Members SET Status=7 WHERE UserId = @UserId");
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool Deletes(string userId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE  aspnet_Members SET Status=7 WHERE UserId in (" + userId + ")");
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.String, userId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool DelUserMessage(int userid, string openid, string userhead, int olduserid)
        {
            string str = "";
            object obj2 = str + "begin try  " + "  begin tran TranUpdate";
            object obj3 = string.Concat(new object[] { obj2, " DELETE FROM aspnet_Members WHERE UserId =", userid, "; " });
            object obj4 = string.Concat(new object[] { obj3, " DELETE FROM Hishop_ShoppingCarts WHERE UserId =", userid, "; " });
            object obj5 = string.Concat(new object[] { obj4, " DELETE FROM Hishop_UserShippingAddresses WHERE UserId =", userid, "; " });
            object obj6 = string.Concat(new object[] { obj5, " DELETE FROM vshop_ActivitySignUp WHERE UserId =", userid, "; " });
            object obj7 = string.Concat(new object[] { obj6, " DELETE FROM Vshop_PrizeRecord WHERE UserId =", userid, "; " });
            str = string.Concat(new object[] { obj7, " Update  aspnet_Members set OpenId='", openid, "',UserHead='", userhead, "' WHERE UserId =", olduserid, "; " }) + " COMMIT TRAN TranUpdate" + "  end try \r\n                    begin catch \r\n                        ROLLBACK TRAN TranUpdate\r\n                    end catch ";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(str);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        private int GetActiveDay()
        {
            string str = string.Format("select top 1   isnull(ActiveDay,1)  from Hishop_UserGroupSet", new object[0]);
            int num = 1;
            try
            {
                num = Convert.ToInt32(this.database.ExecuteScalar(CommandType.Text, str));
            }
            catch
            {
            }
            return num;
        }

        public MemberInfo GetBindusernameMember(string UserBindName,string wid)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM aspnet_Members WHERE UserBindName = @UserBindName AND Status=@Status and wid=@wid");
            this.database.AddInParameter(sqlStringCommand, "UserBindName", DbType.String, UserBindName);
            this.database.AddInParameter(sqlStringCommand, "Status", DbType.Int32, Convert.ToInt32(UserStatus.Normal));
            this.database.AddInParameter(sqlStringCommand, "wid", DbType.String, wid);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return ReaderConvert.ReaderToModel<MemberInfo>(reader);
            }
        }

        public string GetCurrentParentUserId(int? userId)
        {
            string str = "";
            string str2 = "SELECT ReferralPath FROM aspnet_Distributors WHERE UserId=@UserId";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(str2);
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int64, userId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                str = userId.ToString();
                if (reader["ReferralUserId"].ToString() != "0")
                {
                    str = reader["ReferralUserId"].ToString() + "|" + userId.ToString();
                }
            }
            return str;
        }

        public MemberInfo GetMember(int userId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM aspnet_Members WHERE UserId = @UserId");
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return ReaderConvert.ReaderToModel<MemberInfo>(reader);
            }
        }

        public MemberInfo GetMember(string sessionId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM aspnet_Members WHERE SessionId = @SessionId AND Status=@Status");
            this.database.AddInParameter(sqlStringCommand, "SessionId", DbType.String, sessionId);
            this.database.AddInParameter(sqlStringCommand, "Status", DbType.Int32, Convert.ToInt32(UserStatus.Normal));
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return ReaderConvert.ReaderToModel<MemberInfo>(reader);
            }
        }

        public Dictionary<int, MemberClientSet> GetMemberClientSet()
        {
            Dictionary<int, MemberClientSet> dictionary = new Dictionary<int, MemberClientSet>();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_MemberClientSet");
            MemberClientSet set = null;
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                while (reader.Read())
                {
                    set = DataMapper.PopulateMemberClientSet(reader);
                    dictionary.Add(set.ClientTypeId, set);
                }
            }
            return dictionary;
        }

        public DbQueryResult GetMembers(MemberQuery query)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("wid = '{0}'", query.wid);
            if (query.HasVipCard.HasValue)
            {
                if (query.HasVipCard.Value)
                {
                    builder.Append("VipCardNumber is not null");
                }
                else
                {
                    builder.Append("VipCardNumber is null");
                }
            }
            if (query.GradeId.HasValue)
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat("GradeId = {0}", query.GradeId.Value);
            }
            if (query.IsApproved.HasValue)
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat("IsApproved = '{0}'", query.IsApproved.Value);
            }
            if (!string.IsNullOrEmpty(query.CellPhone))
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat("CellPhone = '{0}'", query.CellPhone);
            }
            if (query.Stutas.HasValue)
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat("Status = {0}", Convert.ToInt32(query.Stutas));
            }
            if (!string.IsNullOrEmpty(query.Username))
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat("UserName LIKE '%{0}%'", DataHelper.CleanSearchString(query.Username));
            }
            if (!string.IsNullOrEmpty(query.Realname))
            {
                if (builder.Length > 0)
                {
                    builder.AppendFormat(" AND ", new object[0]);
                }
                builder.AppendFormat("RealName LIKE '%{0}%'", DataHelper.CleanSearchString(query.Realname));
            }
            if (!string.IsNullOrEmpty(query.UserBindName))
            {
                if (builder.Length > 0)
                {
                    builder.AppendFormat(" AND ", new object[0]);
                }
                builder.AppendFormat("UserBindName LIKE '%{0}%'", DataHelper.CleanSearchString(query.UserBindName));
            }
            string str = "";
            int activeDay = this.GetActiveDay();
            if (!string.IsNullOrEmpty(query.ClientType))
            {
                string clientType = query.ClientType;
                if (clientType == null)
                {
                    goto Label_036D;
                }
                if (!(clientType == "new"))
                {
                    if (clientType == "activy")
                    {
                        str = string.Format("select a.UserId\r\n\t\t                    from vw_VShop_FinishOrder_Main\t a\r\n\t\t                    left join aspnet_Members b on a.UserId= b.UserId \r\n\t\t                    where b.Status=1\r\n\t\t                    and  CONVERT(varchar(10), PayDate + {0} , 120 ) >= CONVERT(varchar(10), GETDATE()  , 120 )\r\n\t\t                    group by a.UserId\r\n                        ", activeDay);
                        if (builder.Length > 0)
                        {
                            builder.AppendFormat(" AND ", new object[0]);
                        }
                        builder.AppendFormat("UserId IN (" + str + ")", new object[0]);
                        goto Label_0421;
                    }
                    if (clientType == "sleep")
                    {
                        string str2 = string.Format("select a.UserId\r\n\t\t                    from vw_VShop_FinishOrder_Main\t a\r\n\t\t                    left join aspnet_Members b on a.UserId= b.UserId \r\n\t\t                    where b.Status=1\r\n\t\t                    and  CONVERT(varchar(10), PayDate + {0} , 120 ) >= CONVERT(varchar(10), GETDATE()  , 120 )\r\n\t\t                    group by a.UserId\r\n                        ", activeDay);
                        string str3 = string.Format("select a.UserId\r\n\t\t                    from vw_VShop_FinishOrder_Main\t a\r\n\t\t                    left join aspnet_Members b on a.UserId= b.UserId \r\n\t\t                    where b.Status=1\r\n\t\t                    group by a.UserId\r\n                            ", new object[0]);
                        if (builder.Length > 0)
                        {
                            builder.AppendFormat(" AND ", new object[0]);
                        }
                        builder.AppendFormat("UserId  not IN (" + str2 + ") and  UserId in ( " + str3 + ") ", new object[0]);
                        goto Label_0421;
                    }
                    goto Label_036D;
                }
                str = "SELECT UserId FROM  dbo.Hishop_Orders";
                if (builder.Length > 0)
                {
                    builder.AppendFormat(" AND ", new object[0]);
                }
                builder.Append("UserId NOT IN (" + str + ")");
            }
            goto Label_0421;
        Label_036D:
            str = "SELECT UserId FROM Hishop_Orders WHERE 1=1 ";
            object obj2 = str;
            str = string.Concat(new object[] { obj2, " AND PayDate BETWEEN '", query.StartTime.Value.Date, "' AND '", query.EndTime.Value.Date, "'" });
            if (builder.Length > 0)
            {
                builder.AppendFormat(" AND ", new object[0]);
            }
            builder.AppendFormat("UserId NOT IN (" + str + ") AND  UserId  IN (SELECT UserId FROM dbo.aspnet_Members WHERE UserId  IN (SELECT UserId FROM Hishop_Orders ))", new object[0]);
        Label_0421:
            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "aspnet_Members m", "UserId", (builder.Length > 0) ? builder.ToString() : null, "*, (SELECT Name FROM aspnet_MemberGrades WHERE GradeId = m.GradeId) AS GradeName ,(select COUNT(*) from  dbo.vw_VShop_FinishOrder_Main where UserId=m.UserId) as OrderCount ,(select SUM(ValidOrderTotal) from  dbo.vw_VShop_FinishOrder_Main where UserId=m.UserId) as OrderTotal,(select StoreName from  dbo.aspnet_Distributors where UserId=m.ReferralUserId) as StoreName");
        }

        public IList<MemberInfo> GetMembersByRank(int? gradeId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM aspnet_Members");
            if (gradeId.HasValue && (gradeId.Value > 0))
            {
                sqlStringCommand.CommandText = sqlStringCommand.CommandText + string.Format(" WHERE GradeId={0} AND Status={1}", gradeId.Value, Convert.ToInt32(UserStatus.Normal));
            }
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return ReaderConvert.ReaderToList<MemberInfo>(reader);
            }
        }

        public DataTable GetMembersNopage(MemberQuery query, IList<string> fields)
        {
            if (fields.Count == 0)
            {
                return null;
            }
            DataTable table = null;
            string str = string.Empty;
            foreach (string str2 in fields)
            {
                str = str + str2 + ",";
            }
            str = str.Substring(0, str.Length - 1);
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("SELECT {0} FROM aspnet_Members WHERE   Status={1} and wid='{2}' ", str, Convert.ToInt32(UserStatus.Normal),query.wid);
            if (!string.IsNullOrEmpty(query.Username))
            {
                builder.AppendFormat(" AND UserName LIKE '%{0}%'", query.Username);
            }
            if (query.GradeId.HasValue)
            {
                builder.AppendFormat(" AND GradeId={0}", query.GradeId);
            }
            if (query.HasVipCard.HasValue)
            {
                if (query.HasVipCard.Value)
                {
                    builder.Append(" AND VipCardNumber is not null");
                }
                else
                {
                    builder.Append(" AND VipCardNumber is null");
                }
            }
            if (!string.IsNullOrEmpty(query.Realname))
            {
                builder.AppendFormat(" AND Realname LIKE '%{0}%'", query.Realname);
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                table = DataHelper.ConverDataReaderToDataTable(reader);
                reader.Close();
            }
            return table;
        }

        public IList<MemberInfo> GetMemdersByCardNumbers(string cards)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("SELECT * FROM aspnet_Members WHERE VipCardNumber IN ({0}) AND Status={1} ", cards, Convert.ToInt32(UserStatus.Normal)));
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return ReaderConvert.ReaderToList<MemberInfo>(reader);
            }
        }

        public IList<MemberInfo> GetMemdersByOpenIds(string openids)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("SELECT * FROM aspnet_Members where openid IN ({0}) AND Status={1}", openids, Convert.ToInt32(UserStatus.Normal)));
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return ReaderConvert.ReaderToList<MemberInfo>(reader);
            }
        }

        public MemberInfo GetOpenIdMember(string openId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM aspnet_Members WHERE openId = @openId");
            this.database.AddInParameter(sqlStringCommand, "openId", DbType.String, openId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return ReaderConvert.ReaderToModel<MemberInfo>(reader);
            }
        }

        public MemberInfo GetusernameMember(string username,string wid)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM aspnet_Members WHERE UserBindName = @UserBindName and wid=@wid ");
            this.database.AddInParameter(sqlStringCommand, "UserBindName", DbType.String, username);
            this.database.AddInParameter(sqlStringCommand, "wid", DbType.String, wid);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return ReaderConvert.ReaderToModel<MemberInfo>(reader);
            }
        }

        public bool InsertClientSet(Dictionary<int, MemberClientSet> clientsets)
        {
            StringBuilder builder = new StringBuilder("DELETE FROM  [Hishop_MemberClientSet];");
            foreach (KeyValuePair<int, MemberClientSet> pair in clientsets)
            {
                string str = "";
                string str2 = "";
                if (pair.Value.StartTime.HasValue)
                {
                    str = pair.Value.StartTime.Value.ToString("yyyy-MM-dd");
                }
                if (pair.Value.EndTime.HasValue)
                {
                    str2 = pair.Value.EndTime.Value.ToString("yyyy-MM-dd");
                }
                builder.AppendFormat(string.Concat(new object[] { "INSERT INTO Hishop_MemberClientSet(ClientTypeId,StartTime,EndTime,LastDay,ClientChar,ClientValue) VALUES (", pair.Key, ",'", str, "','", str2, "',", pair.Value.LastDay, ",'", pair.Value.ClientChar, "',", pair.Value.ClientValue, ");" }), new object[0]);
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool IsExitOpenId(string openId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT Count(*) FROM aspnet_Members WHERE OpenId = @OpenId and OpenId!=null");
            this.database.AddInParameter(sqlStringCommand, "OpenId", DbType.String, openId);
            return (((int) this.database.ExecuteScalar(sqlStringCommand)) > 0);
        }

        public bool ReSetUserHead(string userid, string wxName, string wxHead, string Openid = "")
        {
            string str = "";
            if (!string.IsNullOrEmpty(Openid))
            {
                str = ",OpenId=@OpenId ";
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE aspnet_Members SET UserName = @UserName,UserHead = @UserHead " + str + " WHERE UserId = @UserId");
            this.database.AddInParameter(sqlStringCommand, "UserName", DbType.String, wxName);
            this.database.AddInParameter(sqlStringCommand, "UserHead", DbType.String, wxHead);
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userid);
            if (!string.IsNullOrEmpty(Openid))
            {
                this.database.AddInParameter(sqlStringCommand, "UserId", DbType.String, Openid);
            }
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool SetMemberSessionId(string sessionId, DateTime sessionEndTime, string openId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE aspnet_Members SET SessionId = @SessionId, SessionEndTime = @SessionEndTime WHERE OpenId = @OpenId");
            this.database.AddInParameter(sqlStringCommand, "SessionId", DbType.String, sessionId);
            this.database.AddInParameter(sqlStringCommand, "SessionEndTime", DbType.DateTime, sessionEndTime);
            this.database.AddInParameter(sqlStringCommand, "OpenId", DbType.String, openId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public int SetMultiplePwd(string userids, string pwd)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE aspnet_Members SET Password = @Password  WHERE UserId in(" + userids + ")");
            this.database.AddInParameter(sqlStringCommand, "Password", DbType.String, pwd);
            return this.database.ExecuteNonQuery(sqlStringCommand);
        }

        public bool SetPwd(string userid, string pwd)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE aspnet_Members SET Password = @Password  WHERE UserId = @UserId");
            this.database.AddInParameter(sqlStringCommand, "Password", DbType.String, pwd);
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userid);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public int SetRegion(string userID, int regionId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE aspnet_Members SET ReferralUserId = @ReferralUserId  WHERE UserId =" + userID);
            this.database.AddInParameter(sqlStringCommand, "ReferralUserId", DbType.Int32, regionId);
            return this.database.ExecuteNonQuery(sqlStringCommand);
        }

        public int SetRegions(string userIDs, int regionId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE aspnet_Members SET ReferralUserId = @ReferralUserId  WHERE UserId in(" + userIDs + ")");
            this.database.AddInParameter(sqlStringCommand, "ReferralUserId", DbType.Int32, regionId);
            return this.database.ExecuteNonQuery(sqlStringCommand);
        }

        public int SetUsersGradeId(string userId, int gradeId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Concat(new object[] { "UPDATE  aspnet_Members SET GradeId=", gradeId, "  WHERE UserId in (", userId, ")" }));
            return this.database.ExecuteNonQuery(sqlStringCommand);
        }

        public bool Update(MemberInfo member)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE aspnet_Members SET GradeId = @GradeId,OpenId = @OpenId,UserName = @UserName, RealName = @RealName, TopRegionId = @TopRegionId, RegionId = @RegionId,VipCardNumber = @VipCardNumber, VipCardDate = @VipCardDate, Email = @Email, CellPhone = @CellPhone, QQ = @QQ, Address = @Address, Expenditure = @Expenditure, OrderNumber = @OrderNumber,MicroSignal=@MicroSignal,UserHead=@UserHead,wid=@wid WHERE UserId = @UserId");
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, member.UserId);
            this.database.AddInParameter(sqlStringCommand, "GradeId", DbType.Int32, member.GradeId);
            this.database.AddInParameter(sqlStringCommand, "OpenId", DbType.String, member.OpenId);
            this.database.AddInParameter(sqlStringCommand, "UserName", DbType.String, member.UserName);
            this.database.AddInParameter(sqlStringCommand, "RealName", DbType.String, member.RealName);
            this.database.AddInParameter(sqlStringCommand, "TopRegionId", DbType.Int32, member.TopRegionId);
            this.database.AddInParameter(sqlStringCommand, "RegionId", DbType.Int32, member.RegionId);
            this.database.AddInParameter(sqlStringCommand, "Email", DbType.String, member.Email);
            this.database.AddInParameter(sqlStringCommand, "VipCardNumber", DbType.String, member.VipCardNumber);
            this.database.AddInParameter(sqlStringCommand, "VipCardDate", DbType.DateTime, member.VipCardDate);
            this.database.AddInParameter(sqlStringCommand, "CellPhone", DbType.String, member.CellPhone);
            this.database.AddInParameter(sqlStringCommand, "QQ", DbType.String, member.QQ);
            this.database.AddInParameter(sqlStringCommand, "Address", DbType.String, member.Address);
            this.database.AddInParameter(sqlStringCommand, "Expenditure", DbType.Currency, member.Expenditure);
            this.database.AddInParameter(sqlStringCommand, "OrderNumber", DbType.Int32, member.OrderNumber);
            this.database.AddInParameter(sqlStringCommand, "MicroSignal", DbType.String, member.MicroSignal);
            this.database.AddInParameter(sqlStringCommand, "UserHead", DbType.String, member.UserHead);
            this.database.AddInParameter(sqlStringCommand, "wid", DbType.String, member.wid);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool UpdateOpenid(MemberInfo member)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE aspnet_Members SET OpenId=@OpenId,UserHead=@UserHead WHERE UserId = @UserId");
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, member.UserId);
            this.database.AddInParameter(sqlStringCommand, "OpenId", DbType.String, member.OpenId);
            this.database.AddInParameter(sqlStringCommand, "UserHead", DbType.String, member.UserHead);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }
    }
}

