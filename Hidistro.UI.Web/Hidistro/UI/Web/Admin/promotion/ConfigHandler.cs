namespace Hidistro.UI.Web.Admin.promotion
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using System;
    using System.Web;
    using System.Web.SessionState;
    public class ConfigHandler : IHttpHandler, IRequiresSessionState
    {
        public string wid = string.Empty;
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
                switch (str)
                {
                    case "0":
                    {
                        bool flag = bool.Parse(context.Request["enable"].ToString());
                        masterSettings.PonitToCash_Enable = flag;
                        break;
                    }
                    case "1":
                    {
                        bool flag2 = bool.Parse(context.Request["enable"].ToString());
                        masterSettings.ShareAct_Enable = flag2;
                        break;
                    }
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

