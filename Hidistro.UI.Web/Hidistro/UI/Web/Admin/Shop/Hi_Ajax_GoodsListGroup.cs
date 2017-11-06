namespace Hidistro.UI.Web.Admin.Shop
{
    using Hidistro.ControlPanel.Commodities;
    using Hidistro.Core;
    using HiTemplate.Model;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Web;
    using System.Web.SessionState;
    public class Hi_Ajax_GoodsListGroup : IHttpHandler, IRequiresSessionState
    {
        protected string wid;
        public DataTable GetGoods(HttpContext context)
        {
            int top = (context.Request.Form["GoodListSize"] != null) ? Convert.ToInt32(context.Request.Form["GoodListSize"]) : 6;
            ProductShowOrderPriority show = (context.Request.Form["FirstPriority"] != null) ? ((ProductShowOrderPriority) Convert.ToInt32(context.Request.Form["FirstPriority"])) : ProductShowOrderPriority.NULLORDER;
            ProductShowOrderPriority priority2 = (context.Request.Form["SecondPriority"] != null) ? ((ProductShowOrderPriority) Convert.ToInt32(context.Request.Form["SecondPriority"])) : ProductShowOrderPriority.NULLORDER;
            string str = ProductTempSQLADD.ReturnShowOrder(show);
            if (!string.IsNullOrEmpty(str))
            {
                str = str + ",";
            }
            if (!string.IsNullOrEmpty(ProductTempSQLADD.ReturnShowOrder(priority2)))
            {
                str = str + ProductTempSQLADD.ReturnShowOrder(priority2);
            }
            return ProductHelper.GetTopProductOrder(top, str,this.wid);
        }

        public string GoodGroupJson(HttpContext context)
        {
            Hi_Json_GoodGourpContent content = new Hi_Json_GoodGourpContent();
            content.showPrice=context.Request.Form["ShowPrice"] != null ? Convert.ToBoolean(context.Request.Form["ShowPrice"]) : true;
            content.layout=context.Request.Form["Layout"] != null ? Convert.ToInt32(context.Request.Form["Layout"]) : 1;
            content.showName=context.Request.Form["showName"] != null ? Convert.ToBoolean(context.Request.Form["showName"] ): true;
            content.showIco=context.Request.Form["ShowIco"] != null ? Convert.ToBoolean(context.Request.Form["ShowIco"]) : true;
            content.goodsize=context.Request.Form["GoodListSize"] != null ? Convert.ToInt32(context.Request.Form["GoodListSize"]) : 6;
            List<HiShop_Model_Good> list = new List<HiShop_Model_Good>();
            DataTable goods = this.GetGoods(context);
            for (int i = 0; i < goods.Rows.Count; i++)
            {
                HiShop_Model_Good item = new HiShop_Model_Good();
                item.item_id=goods.Rows[i]["ProductId"].ToString();
                item.title=goods.Rows[i]["ProductName"].ToString();
                item.price=Convert.ToDouble(goods.Rows[i]["MinShowPrice"]).ToString("f2");
                item.original_price=Convert.ToDouble(goods.Rows[i]["MarketPrice"]).ToString("f2");
                item.link=string.Concat(new object[] { "http://", Globals.DomainName, ":", HttpContext.Current.Request.Url.Port, "/ProductDetails.aspx?productId=", goods.Rows[i]["ProductId"].ToString(), "&wid=",this.wid });
                item.pic=goods.Rows[i]["ThumbnailUrl310"].ToString();
                list.Add(item);
            }
            content.goodslist=list;
            return JsonConvert.SerializeObject(content);
        }

        public void ProcessRequest(HttpContext context)
        {
            //此处取不到cookie值
            //wid = GetCurWebId();
            //string wid = context.Session[DTKeys.SESSION_WEB_ID] as string;
            //this.wid = Globals.GetCurrentWid();
            //HttpCookie cookie = context.Request.Cookies.Get("Vshop-Wid");
            //this.wid = cookie.Value;
            context.Response.ContentType = "text/plain";
            string text1 = context.Request.Form["id"];

            this.wid = context.Request.Form["wid"];
            if (string.IsNullOrEmpty(wid)) return;

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

