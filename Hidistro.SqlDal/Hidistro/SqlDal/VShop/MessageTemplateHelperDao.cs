namespace Hidistro.SqlDal.VShop
{
    using Hidistro.Entities.VShop;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;

    public class MessageTemplateHelperDao
    {
        private Database database = DatabaseFactory.CreateDatabase();

        public MessageTemplate GetMessageTemplate(string messageType,string wid)
        {
            MessageTemplate template = null;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_MessageTemplates WHERE LOWER(MessageType) = LOWER(@MessageType) and wid=@wid");
            this.database.AddInParameter(sqlStringCommand, "MessageType", DbType.String, messageType);
            this.database.AddInParameter(sqlStringCommand, "wid", DbType.String, wid);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                while (reader.Read())
                {
                    template = this.PopulateEmailTempletFromIDataReader(reader);
                }
                reader.Close();
            }
            return template;
        }

        public IList<MessageTemplate> GetMessageTemplates(string wid)
        {
            IList<MessageTemplate> list = new List<MessageTemplate>();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_MessageTemplates where wid=@wid");
            this.database.AddInParameter(sqlStringCommand, "wid", DbType.String, wid);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                while (reader.Read())
                {
                    list.Add(this.PopulateEmailTempletFromIDataReader(reader));
                }
                reader.Close();
            }
            return list;
        }

        public MessageTemplate PopulateEmailTempletFromIDataReader(IDataReader reader)
        {
            if (reader == null)
            {
                return null;
            }
            return new MessageTemplate((string) reader["TagDescription"], (string) reader["Name"]) { MessageType = (string) reader["MessageType"], SendInnerMessage = (bool) reader["SendInnerMessage"], SendWeixin = (bool) reader["SendWeixin"], SendSMS = (bool) reader["SendSMS"], SendEmail = (bool) reader["SendEmail"], EmailSubject = (string) reader["EmailSubject"], EmailBody = (string) reader["EmailBody"], InnerMessageSubject = (string) reader["InnerMessageSubject"], InnerMessageBody = (string) reader["InnerMessageBody"], SMSBody = (string) reader["SMSBody"], WeixinTemplateId = (reader["WeixinTemplateId"] != DBNull.Value) ? ((string) reader["WeixinTemplateId"]) : "" };
        }

        public int IsExistSettings(string wid)
        {
            string str = "SELECT count(*) FROM Hishop_MessageTemplates WHERE wid=@wid";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(str);
            this.database.AddInParameter(sqlStringCommand, "wid", DbType.String, wid);
            return Convert.ToInt32(this.database.ExecuteScalar(sqlStringCommand));
        }

        public bool AddSettings(string wid)
        {
            string sql = @"insert Hishop_MessageTemplates
            select messagetype,@wid,name,sendemail,sendsms,sendinnermessage,sendweixin,WeixinTemplateId,TagDescription,EmailSubject,
            EmailBody,InnerMessageSubject,InnerMessageBody,SMSBody
            from Hishop_MessageTemplates where wid is null";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(sql);
            this.database.AddInParameter(sqlStringCommand, "wid", DbType.String, wid);
            try
            {
                return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
            }
            catch
            {
                return false;
            }
        }

        public void UpdateSettings(IList<MessageTemplate> templates,string wid)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_MessageTemplates SET SendEmail = @SendEmail, SendSMS = @SendSMS, SendInnerMessage = @SendInnerMessage,SendWeixin = @SendWeixin WHERE LOWER(MessageType) = LOWER(@MessageType) and wid=@wid");
            this.database.AddInParameter(sqlStringCommand, "SendEmail", DbType.Boolean);
            this.database.AddInParameter(sqlStringCommand, "SendSMS", DbType.Boolean);
            this.database.AddInParameter(sqlStringCommand, "SendInnerMessage", DbType.Boolean);
            this.database.AddInParameter(sqlStringCommand, "MessageType", DbType.String);
            this.database.AddInParameter(sqlStringCommand, "SendWeixin", DbType.Boolean);
            this.database.AddInParameter(sqlStringCommand, "wid", DbType.String);
            foreach (MessageTemplate template in templates)
            {
                this.database.SetParameterValue(sqlStringCommand, "SendEmail", template.SendEmail);
                this.database.SetParameterValue(sqlStringCommand, "SendSMS", template.SendSMS);
                this.database.SetParameterValue(sqlStringCommand, "SendInnerMessage", template.SendInnerMessage);
                this.database.SetParameterValue(sqlStringCommand, "MessageType", template.MessageType);
                this.database.SetParameterValue(sqlStringCommand, "SendWeixin", template.SendWeixin);
                this.database.SetParameterValue(sqlStringCommand, "wid", wid);
                this.database.ExecuteNonQuery(sqlStringCommand);
            }
        }

        public void UpdateTemplate(MessageTemplate template,string wid)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_MessageTemplates SET EmailSubject = @EmailSubject, EmailBody = @EmailBody, InnerMessageSubject = @InnerMessageSubject, InnerMessageBody = @InnerMessageBody,WeixinTemplateId=@WeixinTemplateId, SMSBody = @SMSBody WHERE LOWER(MessageType) = LOWER(@MessageType) and wid=@wid");
            this.database.AddInParameter(sqlStringCommand, "EmailSubject", DbType.String, template.EmailSubject);
            this.database.AddInParameter(sqlStringCommand, "EmailBody", DbType.String, template.EmailBody);
            this.database.AddInParameter(sqlStringCommand, "InnerMessageSubject", DbType.String, template.InnerMessageSubject);
            this.database.AddInParameter(sqlStringCommand, "InnerMessageBody", DbType.String, template.InnerMessageBody);
            this.database.AddInParameter(sqlStringCommand, "SMSBody", DbType.String, template.SMSBody);
            this.database.AddInParameter(sqlStringCommand, "MessageType", DbType.String, template.MessageType);
            this.database.AddInParameter(sqlStringCommand, "WeixinTemplateId", DbType.String, template.WeixinTemplateId);
            this.database.AddInParameter(sqlStringCommand, "wid", DbType.String, wid);
            this.database.ExecuteNonQuery(sqlStringCommand);
        }
    }
}

