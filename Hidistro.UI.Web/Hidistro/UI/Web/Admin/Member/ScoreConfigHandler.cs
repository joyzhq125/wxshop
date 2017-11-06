namespace Hidistro.UI.Web.Admin.Member
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using System;
    using System.Web;
    using System.Web.SessionState;
    public class ScoreConfigHandler : IHttpHandler, IRequiresSessionState
    {
        protected string wid;
        public void ProcessRequest(HttpContext context)
        {
            wid = context.Session[DTKeys.SESSION_WEB_ID] as string;
            if (string.IsNullOrEmpty(wid))
            {
                return;
            }
            context.Response.ContentType = "text/plain";
            try
            {
                string str = context.Request["type"].ToString();
                SiteSettings masterSettings = SettingsManager.GetMasterSettings(false,wid);
                bool flag = bool.Parse(context.Request["enable"].ToString());
                switch (str)
                {
                    case "0":
                        masterSettings.sign_score_Enable = flag;
                        break;

                    case "1":
                        masterSettings.shopping_score_Enable = flag;
                        break;

                    case "2":
                        masterSettings.share_score_Enable = flag;
                        break;
                }
                SettingsManager.Save(masterSettings);
                context.Response.Write("保存成功");
            }
            catch (Exception exception)
            {
                context.Response.Write("保存失败！（" + exception.Message + ")");
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}

