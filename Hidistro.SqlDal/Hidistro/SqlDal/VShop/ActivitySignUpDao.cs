namespace Hidistro.SqlDal.VShop
{
    using Hidistro.Entities;
    using Hidistro.Entities.VShop;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Text;

    public class ActivitySignUpDao
    {
        private Database database = DatabaseFactory.CreateDatabase();

        public IList<ActivitySignUpInfo> GetActivitySignUpById(int activityId)
        {
            string str = "SELECT * FROM vshop_ActivitySignUp WHERE ActivityId = @ActivityId";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(str);
            this.database.AddInParameter(sqlStringCommand, "ActivityId", DbType.Int32, activityId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return ReaderConvert.ReaderToList<ActivitySignUpInfo>(reader);
            }
        }

        public string SaveActivitySignUp(ActivitySignUpInfo info)
        {
            string str = string.Empty;
            ActivityInfo activity = new ActivityDao().GetActivity(info.ActivityId);
            if (activity == null)
            {
                return "活动不存在";
            }
            int maxValue = activity.MaxValue;
            if (maxValue > 0)
            {
                string str2 = "select count(0) from vshop_ActivitySignUp where  ActivityId=@ActivityId";
                DbCommand sqlStringCommand = this.database.GetSqlStringCommand(str2);
                this.database.AddInParameter(sqlStringCommand, "ActivityId", DbType.Int32, info.ActivityId);
                int num2 = Convert.ToInt32(this.database.ExecuteScalar(sqlStringCommand));
                if (maxValue <= num2)
                {
                    str = "已经达到了报名上限";
                }
            }
            if (string.IsNullOrEmpty(str))
            {
                StringBuilder builder = new StringBuilder();
                builder.Append("IF NOT EXISTS (select 1 from vshop_ActivitySignUp WHERE ActivityId=@ActivityId and UserId=@UserId) ").Append("INSERT INTO vshop_ActivitySignUp(").Append("ActivityId,UserId,UserName,RealName,SignUpDate").Append(",Item1,Item2,Item3,Item4,Item5)").Append(" VALUES (").Append("@ActivityId,@UserId,@UserName,@RealName,@SignUpDate").Append(",@Item1,@Item2,@Item3,@Item4,@Item5)");
                DbCommand command2 = this.database.GetSqlStringCommand(builder.ToString());
                this.database.AddInParameter(command2, "ActivityId", DbType.Int32, info.ActivityId);
                this.database.AddInParameter(command2, "UserId", DbType.Int32, info.UserId);
                this.database.AddInParameter(command2, "UserName", DbType.String, info.UserName);
                this.database.AddInParameter(command2, "RealName", DbType.String, info.RealName);
                this.database.AddInParameter(command2, "SignUpDate", DbType.DateTime, info.SignUpDate);
                this.database.AddInParameter(command2, "Item1", DbType.String, info.Item1);
                this.database.AddInParameter(command2, "Item2", DbType.String, info.Item2);
                this.database.AddInParameter(command2, "Item3", DbType.String, info.Item3);
                this.database.AddInParameter(command2, "Item4", DbType.String, info.Item4);
                this.database.AddInParameter(command2, "Item5", DbType.String, info.Item5);
                str = (this.database.ExecuteNonQuery(command2) > 0) ? "1" : "你已经报过名了,请勿重复报名";
            }
            return str;
        }
    }
}

