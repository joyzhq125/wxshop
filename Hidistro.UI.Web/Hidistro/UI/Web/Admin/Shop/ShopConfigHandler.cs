namespace Hidistro.UI.Web.Admin.Shop
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using System;
    using System.Web;
    using System.Web.SessionState;
    public class ShopConfigHandler : IHttpHandler, IRequiresSessionState
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
                SF.BLL.sf_website bll = new SF.BLL.sf_website();
                SF.Model.sf_website website = bll.GetModelByWid(wid);
                string str = context.Request["type"].ToString();
                SiteSettings masterSettings = SettingsManager.GetMasterSettings(false,wid);
                if (str == "0")
                {
                    bool flag = bool.Parse(context.Request["enable"].ToString());
                    masterSettings.EnableSaleService = flag;
                    website.EnableSaleService = flag;
                }
                //SettingsManager.Save(masterSettings);
                bll.Update(website);
                context.Response.Write("{\"type\":\"success\",\"data\":\"\"}");
            }
            catch (Exception exception)
            {
                context.Response.Write("{\"type\":\"error\",\"data\":\"" + exception.Message + "\"}");
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

