namespace Hidistro.UI.Web.Admin.Shop.api
{
    using Hidistro.ControlPanel.Store;
    using System;
    using System.Web;

    public class Hi_Ajax_RenameImg : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Write(this.ReName(context));
        }

        public string ReName(HttpContext context)
        {
            GalleryHelper.RenamePhoto(Convert.ToInt32(context.Request.Form["file_id"]), context.Request.Form["file_name"]);
            return "{\"status\": 1,\"msg\":\"\"}";
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

