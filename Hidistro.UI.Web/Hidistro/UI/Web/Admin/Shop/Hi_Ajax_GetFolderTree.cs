namespace Hidistro.UI.Web.Admin.Shop
{
    using Hidistro.ControlPanel.Store;
    using System;
    using System.Data;
    using System.Text;
    using System.Web;

    public class Hi_Ajax_GetFolderTree : IHttpHandler
    {
        public string GetImgTypeJson()
        {
            string str = "";
            DataTable photoCategories = GalleryHelper.GetPhotoCategories();
            for (int i = 0; i < photoCategories.Rows.Count; i++)
            {
                object obj2 = str + "{";
                object obj3 = string.Concat(new object[] { obj2, "\"name\":\"", photoCategories.Rows[i]["CategoryName"], "\"," }) + "\"parent_id\":0,";
                str = (string.Concat(new object[] { obj3, "\"id\":", photoCategories.Rows[i]["CategoryId"], "," }) + "\"picNum\":" + GalleryHelper.GetPhotoList("", new int?(Convert.ToInt32(photoCategories.Rows[i]["CategoryId"])), 10, PhotoListOrder.UploadTimeDesc).TotalRecords) + "},";
            }
            return str.TrimEnd(new char[] { ',' });
        }

        public string GetTreeListJson()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("{\"status\":1,");
            builder.Append("\"data\":{");
            builder.Append("\"total\":" + GalleryHelper.GetPhotoList("", 0, 10, PhotoListOrder.UploadTimeDesc).TotalRecords + ",");
            builder.Append("\"tree\":[");
            builder.Append(this.GetImgTypeJson());
            builder.Append("]");
            builder.Append("},");
            builder.Append("\"msg\":\"\"");
            builder.Append("}");
            return builder.ToString();
        }

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Write(this.GetTreeListJson());
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

