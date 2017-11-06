namespace Hidistro.UI.Web.API
{
    using Hidistro.ControlPanel.Commodities;
    using Hidistro.Core;
    using Hidistro.Entities.Commodities;
    using HiTemplate.Model;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.SessionState;
    public class Hi_Ajax_GoodsList : IHttpHandler, IRequiresSessionState
    {
        protected string wid;
        public IList<ProductInfo> GetGoods(HttpContext context, string ids)
        {
            return ProductHelper.GetProducts((from s in ids.Split(new char[] { ',' }) select int.Parse(s)).ToList<int>());
        }

        public string GoodGroupJson(HttpContext context)
        {
            Hi_Json_GoodGourpContent content = new Hi_Json_GoodGourpContent();
            content.showPrice=context.Request.Form["ShowPrice"] != null ? Convert.ToBoolean(context.Request.Form["ShowPrice"]) : true;
            content.layout=context.Request.Form["Layout"] != null ? Convert.ToInt32(context.Request.Form["Layout"]) : 1;
            content.showName=context.Request.Form["showName"] != null ? Convert.ToBoolean(Convert.ToInt32(context.Request.Form["showName"])) : true;
            content.showIco=context.Request.Form["ShowIco"] != null ? Convert.ToBoolean(context.Request.Form["ShowIco"]) : true;
            string str = context.Request.Form["IDs"] != null ? context.Request.Form["IDs"] : "";
            List<HiShop_Model_Good> list = new List<HiShop_Model_Good>();
            if (!string.IsNullOrEmpty(str))
            {
                foreach (ProductInfo info in this.GetGoods(context, str))
                {
                    HiShop_Model_Good item = new HiShop_Model_Good();
                    item.item_id=info.ProductId.ToString();
                    item.title=info.ProductName.ToString();
                    item.price=Convert.ToDouble(info.MinShowPrice).ToString("f2");
                    item.original_price=Convert.ToDouble(info.MarketPrice).ToString("f2");
                    item.link=string.Concat(new object[] { "http://", Globals.DomainName, ":", HttpContext.Current.Request.Url.Port, "/ProductDetails.aspx?productId=", info.ProductId.ToString() ,"&wid=" , this.wid });
                    item.pic=info.ThumbnailUrl310.ToString();
                    list.Add(item);
                }
            }
            content.goodslist=list;
            return JsonConvert.SerializeObject(content);
        }

        public void ProcessRequest(HttpContext context)
        {
            this.wid = context.Request.Form["wid"];
            if (string.IsNullOrEmpty(wid)) return;
            context.Response.ContentType = "text/plain";
            context.Response.Write(this.GoodGroupJson(context));
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

