namespace Hidistro.UI.Web.API
{
    using Hidistro.ControlPanel.Promotions;
    using Hidistro.Entities.Commodities;
    using Hidistro.Entities.Promotions;
    using Hidistro.SaleSystem.Vshop;
    using Newtonsoft.Json;
    using System;
    using System.Data;
    using System.Web;

    public class Hi_Ajax_ExchangeProducts : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            int result = 0;
            int.TryParse(context.Request.Params["id"], out result);
            if (result > 0)
            {
                int num3;
                int num4;
                PointExChangeInfo info = PointExChangeHelper.Get(result);
                int gradeId = MemberProcessor.GetCurrentMember().GradeId;
                string str = context.Request.Params["sort"];
                if (string.IsNullOrWhiteSpace(str))
                {
                    str = "ProductId";
                }
                string str2 = context.Request.Params["order"];
                if (string.IsNullOrWhiteSpace(str2))
                {
                    str2 = "asc";
                }
                if (!int.TryParse(context.Request.Params["page"], out num3))
                {
                    num3 = 1;
                }
                if (!int.TryParse(context.Request.Params["size"], out num4))
                {
                    num4 = 10;
                }
                bool flag = false;
                foreach (string str3 in info.MemberGrades.Split(new char[] { ',' }))
                {
                    if ((int.Parse(str3) == gradeId) || (str3 == "0"))
                    {
                        flag = true;
                    }
                }
                if ((flag && (info.BeginDate <= DateTime.Now)) && (info.EndDate >= DateTime.Now))
                {
                    int num5;
                    DataTable table = PointExChangeHelper.GetProducts(result, num3, num4, out num5, str, str2);
                    foreach (DataRow row in table.Rows)
                    {
                        if (row["ProductNumber"].ToString() == "0")
                        {
                            int num6 = 0;
                            int.TryParse(row["ProductId"].ToString(), out num6);
                            ProductInfo product = ProductBrowser.GetProduct(MemberProcessor.GetCurrentMember(), num6);
                            if ((product != null) && (product.SaleStatus == ProductSaleStatus.OnSale))
                            {
                                row["ProductNumber"] = product.Stock.ToString();
                            }
                        }
                    }
                    string s = JsonConvert.SerializeObject(table, Formatting.Indented);
                    context.Response.Write(s);
                }
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

