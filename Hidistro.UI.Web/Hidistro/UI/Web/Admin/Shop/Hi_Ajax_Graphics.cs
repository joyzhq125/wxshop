namespace Hidistro.UI.Web.Admin.Shop
{
    using Hidistro.ControlPanel.WeiBo;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities.Weibo;
    using System;
    using System.Data;
    using System.Text;
    using System.Web;

    public class Hi_Ajax_Graphics : IHttpHandler
    {
        public ArticleQuery GetGraphiceSearch(HttpContext context)
        {
            return new ArticleQuery { Title = (context.Request.Form["title"] == null) ? "" : context.Request.Form["title"], PageIndex = (context.Request.Form["p"] == null) ? 1 : Convert.ToInt32(context.Request.Form["p"]), SortOrder = SortAction.Desc, SortBy = "PubTime" };
        }

        public string GetGraphicesListJson(DbQueryResult GraphicesTable, HttpContext context)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("\"list\":[");
            DataTable data = (DataTable) GraphicesTable.Data;
            for (int i = 0; i < data.Rows.Count; i++)
            {
                builder.Append("{");
                builder.Append("\"item_id\":\"" + data.Rows[i]["ArticleId"] + "\",");
                builder.Append("\"title\":\"" + data.Rows[i]["Title"] + "\",");
                builder.Append("\"create_time\":\"" + Convert.ToDateTime(data.Rows[i]["PubTime"]).ToString("yyyy-MM-dd HH:mm:ss") + "\",");
                builder.Append("\"link\":\"" + data.Rows[i]["Url"] + "\",");
                builder.Append(string.Concat(new object[] { "\"pic\":\"http://", context.Request.Url.Authority, data.Rows[i]["ImageUrl"], "\"" }));
                builder.Append("},");
            }
            return (builder.ToString().TrimEnd(new char[] { ',' }) + "]");
        }

        public DbQueryResult GetGraphicesTable(HttpContext context)
        {
            return ArticleHelper.GetArticleRequest(this.GetGraphiceSearch(context));
        }

        public string GetModelJson(HttpContext context)
        {
            DbQueryResult graphicesTable = this.GetGraphicesTable(context);
            int pageCount = TemplatePageControl.GetPageCount(graphicesTable.TotalRecords, 10);
            if (graphicesTable != null)
            {
                string str = "{\"status\":1,";
                return (((str + this.GetGraphicesListJson(graphicesTable, context) + ",") + "\"page\":\"" + this.GetPageHtml(pageCount, context) + "\"") + "}");
            }
            return "{\"status\":1,\"list\":[],\"page\":\"\"}";
        }

        public string GetPageHtml(int pageCount, HttpContext context)
        {
            int pageIndex = (context.Request.Form["p"] == null) ? 1 : Convert.ToInt32(context.Request.Form["p"]);
            return TemplatePageControl.GetPageHtml(pageCount, pageIndex);
        }

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Write(this.GetModelJson(context));
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

