namespace Hidistro.UI.Web.Admin.Shop.api
{
    using Hidistro.ControlPanel.Commodities;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities.Sales;
    using System;
    using System.Data;
    using System.Text;
    using System.Web;
    using System.Web.SessionState;
    public class Hi_Ajax_Categories : IHttpHandler, IRequiresSessionState
    {
        protected string wid;
        public CategoriesQuery GetCategoriesSearch(HttpContext context)
        {
            return new CategoriesQuery { wid=this.wid,Name = (context.Request.Form["title"] == null) ? "" : context.Request.Form["title"], PageIndex = (context.Request.Form["p"] == null) ? 1 : Convert.ToInt32(context.Request.Form["p"]), SortOrder = SortAction.Desc, SortBy = "CategoryId" };
        }

        public DbQueryResult GetCategoriesTable(HttpContext context)
        {
            return CatalogHelper.Query(this.GetCategoriesSearch(context));
        }

        public string GetGraphicesListJson(DbQueryResult GraphicesTable, HttpContext context)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("\"list\":[");
            DataTable data = (DataTable) GraphicesTable.Data;
            for (int i = 0; i < data.Rows.Count; i++)
            {
                builder.Append("{");
                builder.Append("\"item_id\":\"" + data.Rows[i]["CategoryId"] + "\",");
                builder.Append("\"title\":\"" + data.Rows[i]["Name"] + "\",");
                builder.Append("\"create_time\":\"" + DateTime.Now + "\",");
                builder.Append("\"link\":\"/ProductList.aspx?categoryId=" + data.Rows[i]["CategoryId"] + "\",");
                builder.Append("\"pic\":\"\"");
                builder.Append("},");
            }
            return (builder.ToString().TrimEnd(new char[] { ',' }) + "]");
        }

        public string GetModelJson(HttpContext context)
        {
            DbQueryResult categoriesTable = this.GetCategoriesTable(context);
            int pageCount = TemplatePageControl.GetPageCount(categoriesTable.TotalRecords, 10);
            if (categoriesTable != null)
            {
                string str = "{\"status\":1,";
                return (((str + this.GetGraphicesListJson(categoriesTable, context) + ",") + "\"page\":\"" + this.GetPageHtml(pageCount, context) + "\"") + "}");
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
            wid = context.Session[DTKeys.SESSION_WEB_ID] as string;
            if (string.IsNullOrEmpty(wid))
            {
                return;
            }
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

