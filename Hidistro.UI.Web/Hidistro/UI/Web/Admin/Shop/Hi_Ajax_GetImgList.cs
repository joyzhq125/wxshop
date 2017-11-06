namespace Hidistro.UI.Web.Admin.Shop
{
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using System;
    using System.Data;
    using System.Text;
    using System.Web;

    public class Hi_Ajax_GetImgList : IHttpHandler
    {
        public string GetImgItemJson()
        {
            return "";
        }

        public string GetImgItemsJson(DbQueryResult mamagerRecordset, HttpContext context)
        {
            StringBuilder builder = new StringBuilder();
            DataTable data = (DataTable) mamagerRecordset.Data;
            for (int i = 0; i < data.Rows.Count; i++)
            {
                builder.Append("{");
                builder.Append("\"id\":\"" + data.Rows[i]["PhotoId"] + "\",");
                builder.Append(string.Concat(new object[] { "\"file\":\"http://", context.Request.Url.Authority, data.Rows[i]["PhotoPath"], "\"," }));
                builder.Append("\"name\":\"" + data.Rows[i]["PhotoName"] + "\"");
                builder.Append("},");
            }
            return builder.ToString().TrimEnd(new char[] { ',' });
        }

        public string GetListJson(HttpContext context)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("{\"status\":1,");
            builder.Append("\"data\":[");
            DbQueryResult mamagerRecordset = GalleryHelper.GetPhotoList(context.Request.Form["file_Name"], new int?(Convert.ToInt32(context.Request.Form["id"])), Convert.ToInt32(context.Request.Form["p"]), 0x1c, PhotoListOrder.UploadTimeDesc);
            int pageCount = TemplatePageControl.GetPageCount(mamagerRecordset.TotalRecords, 0x1c);
            builder.Append(this.GetImgItemsJson(mamagerRecordset, context));
            return (((builder.ToString().TrimEnd(new char[] { ',' }) + "],") + "\"page\": \"" + this.GetPageHtml(pageCount, context) + "\",") + "\"msg\": \"\"" + "}");
        }

        public string GetPageHtml(int pageCount, HttpContext context)
        {
            int pageIndex = (context.Request.Form["p"] == null) ? 1 : Convert.ToInt32(context.Request.Form["p"]);
            return TemplatePageControl.GetPageHtml(pageCount, pageIndex);
        }

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string listJson = this.GetListJson(context);
            context.Response.Write(listJson);
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

