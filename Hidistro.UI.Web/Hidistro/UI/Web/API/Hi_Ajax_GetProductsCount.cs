namespace Hidistro.UI.Web.API
{
    using Hidistro.ControlPanel.Commodities;
    using Hidistro.Core;
    using Hidistro.Entities.Members;
    using Hidistro.SaleSystem.Vshop;
    using System;
    using System.Web;
    using System.Web.SessionState;
    public class Hi_Ajax_GetProductsCount : IHttpHandler, IRequiresSessionState
    {
        protected string wid;
        public string GetCountJson()
        {
            string siteName = "";
            this.wid = Globals.GetCurrentWid();
            HttpCookie cookie = HttpContext.Current.Request.Cookies["Vshop-ReferralId"];
            if ((cookie == null) || (cookie.Value == "0"))
            {
                siteName = SettingsManager.GetMasterSettings(true,this.wid).SiteName;
                return string.Concat(new object[] { "{\"count\":", ProductHelper.GetProductsCount(this.wid), ",\"storeName\":\"", siteName, "\"}" });
            }
            DistributorsInfo currentDistributors = DistributorsBrower.GetCurrentDistributors(Convert.ToInt32(cookie.Value));
            if (currentDistributors != null)
            {
                siteName = currentDistributors.StoreName;
            }
            return string.Concat(new object[] { "{\"count\":", ProductHelper.GetProductsCountByDistributor(Convert.ToInt32(cookie.Value)), ",\"storeName\":\"", siteName, "\"}" });
        }

        public void ProcessRequest(HttpContext context)
        {
            //wid = context.Session[DTKeys.SESSION_WEB_ID] as string;
            //if (string.IsNullOrEmpty(wid))
            //{
            //    return;
            //}
            context.Response.ContentType = "text/plain";
            context.Response.Write(this.GetCountJson());
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

