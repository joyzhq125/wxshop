namespace Hidistro.UI.Web.Admin.Shop
{
    using Core;
    using System;
    using System.IO;
    using System.Web;
    using System.Web.SessionState;
    public class Hi_Ajax_GoodsGourp : IHttpHandler, IRequiresSessionState
    {
        protected string wid;
        public string GetGoodsGroupJson(HttpContext context)
        {
            return File.ReadAllText(context.Server.MapPath("/Data/GoodsGroupJson.json"));
        }

        public void ProcessRequest(HttpContext context)
        {
            wid = context.Session[DTKeys.SESSION_WEB_ID] as string;
            if (string.IsNullOrEmpty(wid))
            {
                return;
            }
            context.Response.ContentType = "text/plain";
            context.Response.Write(this.GetGoodsGroupJson(context));
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

