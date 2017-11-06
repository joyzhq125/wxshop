namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.ControlPanel.Promotions;
    using Hidistro.Core;
    using Hidistro.Entities.Members;
    using Hidistro.Entities.Sales;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;
    using Tags;

    [ParseChildren(true)]
    public class VSubmmitOrder : VMemberTemplatedWebControl
    {
        private HtmlAnchor aLinkToShipping;
        private int buyAmount;
        private DataTable dtActivities = ActivityHelper.GetActivities();
        public DataTable GetUserCoupons;
        private HtmlInputControl groupbuyHiddenBox;
        private Literal litAddAddress;
        private Literal litAddress;
        private Literal litCellPhone;
        private Literal litDisplayPointNumber;
        private Literal litOrderTotal;
        private Literal litPointNumber;
        private Literal litShipTo;
        private Literal litShowMes;
        public MemberInfo member = MemberProcessor.GetCurrentMember();
        private string productSku;
        private HtmlInputHidden regionId;
        private VshopTemplatedRepeater rptAddress;
        private VshopTemplatedRepeater rptCartProducts;
        private HtmlInputHidden selectShipTo;

        private Common_PaymentTypeSelect paymentTypeSel;

        protected override void AttachChildControls()
        {
            this.litShipTo = (Literal) this.FindControl("litShipTo");
            this.litCellPhone = (Literal) this.FindControl("litCellPhone");
            this.litAddress = (Literal) this.FindControl("litAddress");
            this.litShowMes = (Literal) this.FindControl("litShowMes");
            this.GetUserCoupons = MemberProcessor.GetUserCoupons();
            this.rptCartProducts = (VshopTemplatedRepeater) this.FindControl("rptCartProducts");
            this.rptCartProducts.ItemDataBound += new RepeaterItemEventHandler(this.rptCartProducts_ItemDataBound);
            this.litOrderTotal = (Literal) this.FindControl("litOrderTotal");
            this.litPointNumber = (Literal) this.FindControl("litPointNumber");
            this.litDisplayPointNumber = (Literal) this.FindControl("litDisplayPointNumber");
            this.aLinkToShipping = (HtmlAnchor) this.FindControl("aLinkToShipping");
            this.groupbuyHiddenBox = (HtmlInputControl) this.FindControl("groupbuyHiddenBox");
            this.rptAddress = (VshopTemplatedRepeater) this.FindControl("rptAddress");

            //显示调用支付方式控件
            this.paymentTypeSel = (Common_PaymentTypeSelect)this.FindControl("paymenttypesel");
            paymentTypeSel.wid = this.wid;

            this.selectShipTo = (HtmlInputHidden) this.FindControl("selectShipTo");
            this.regionId = (HtmlInputHidden) this.FindControl("regionId");
            this.litAddAddress = (Literal) this.FindControl("litAddAddress");



            IList<ShippingAddressInfo> shippingAddresses = MemberProcessor.GetShippingAddresses();
            this.rptAddress.DataSource = from item in shippingAddresses
                orderby item.IsDefault
                select item;
            this.rptAddress.DataBind();
            ShippingAddressInfo info = shippingAddresses.FirstOrDefault<ShippingAddressInfo>(item => item.IsDefault);
            if (info == null)
            {
                info = (shippingAddresses.Count > 0) ? shippingAddresses[0] : null;
            }
            if (info != null)
            {
                this.litShipTo.Text = info.ShipTo;
                this.litCellPhone.Text = info.CellPhone;
                this.litAddress.Text = info.Address;
                this.selectShipTo.SetWhenIsNotNull(info.ShippingId.ToString());
                this.regionId.SetWhenIsNotNull(info.RegionId.ToString());
            }
            this.litAddAddress.Text = " href='/Vshop/AddShippingAddress.aspx?returnUrl=" + Globals.UrlEncode(HttpContext.Current.Request.Url.ToString()) + "'";
            if ((shippingAddresses == null) || (shippingAddresses.Count == 0))
            {
                this.Page.Response.Redirect(Globals.ApplicationPath + "/Vshop/AddShippingAddress.aspx?returnUrl=" + Globals.UrlEncode(HttpContext.Current.Request.Url.ToString()));
            }
            else
            {
                this.aLinkToShipping.HRef = Globals.ApplicationPath + "/Vshop/ShippingAddresses.aspx?returnUrl=" + Globals.UrlEncode(HttpContext.Current.Request.Url.ToString());
                List<ShoppingCartInfo> listShoppingCart = new List<ShoppingCartInfo>();
                if (((int.TryParse(this.Page.Request.QueryString["buyAmount"], out this.buyAmount) && !string.IsNullOrEmpty(this.Page.Request.QueryString["productSku"])) && !string.IsNullOrEmpty(this.Page.Request.QueryString["from"])) && ((this.Page.Request.QueryString["from"] == "signBuy") || (this.Page.Request.QueryString["from"] == "groupBuy")))
                {
                    this.productSku = this.Page.Request.QueryString["productSku"];
                    listShoppingCart = ShoppingCartProcessor.GetListShoppingCart(this.productSku, this.buyAmount);
                }
                else
                {
                    listShoppingCart = ShoppingCartProcessor.GetOrderSummitCart();
                }
                if (listShoppingCart == null)
                {
                    this.ShowMessage("没有需要结算的订单！", false);
                }
                else
                {
                    if (listShoppingCart.Count > 1)
                    {
                        this.litShowMes.Text = "<div style=\"color: #F60; \"><img  src=\"/Utility/pics/u77.png\">您所购买的商品不支持同一个物流规则发货，系统自动拆分成多个子订单处理</div>";
                    }
                    if (listShoppingCart != null)
                    {
                        this.rptCartProducts.DataSource = listShoppingCart;
                        this.rptCartProducts.DataBind();
                        decimal num = 0M;
                        decimal num2 = 0M;
                        decimal num3 = 0M;
                        int num4 = 0;
                        foreach (ShoppingCartInfo info2 in listShoppingCart)
                        {
                            num4 += info2.GetPointNumber;
                            num += info2.Total;
                            num2 += info2.Exemption;
                            num3 += info2.ShipCost;
                        }
                        decimal num5 = num2;
                        this.litOrderTotal.Text = (num - num5).ToString("F2");
                        if (num4 == 0)
                        {
                            this.litDisplayPointNumber.Text = "style=\"display:none;\"";
                        }
                        this.litPointNumber.Text = num4.ToString();
                    }
                    else
                    {
                        this.Page.Response.Redirect("/Vshop/ShoppingCart.aspx");
                    }
                    PageTitle.AddSiteNameTitle("订单确认");
                }
            }
        }

        public decimal DiscountMoney(List<ShoppingCartInfo> infoList)
        {
            decimal num = 0M;
            decimal num2 = 0M;
            decimal num3 = 0M;
            decimal num4 = 0M;
            int num5 = 0;
            foreach (ShoppingCartInfo info in infoList)
            {
                foreach (ShoppingCartItemInfo info2 in info.LineItems)
                {
                    if (info2.Type == 0)
                    {
                        num4 += info2.SubTotal;
                        num5 += info2.Quantity;
                    }
                }
            }
            DataTable activities = ActivityHelper.GetActivities();
            for (int i = 0; i < activities.Rows.Count; i++)
            {
                decimal num7 = 0M;
                int num8 = 0;
                DataTable table2 = ActivityHelper.GetActivities_Detail(int.Parse(activities.Rows[i]["ActivitiesId"].ToString()));
                foreach (ShoppingCartInfo info3 in infoList)
                {
                    foreach (ShoppingCartItemInfo info4 in info3.LineItems)
                    {
                        if ((info4.Type == 0) && (ActivityHelper.GetActivitiesProducts(int.Parse(activities.Rows[i]["ActivitiesId"].ToString()), info4.ProductId).Rows.Count > 0))
                        {
                            num7 += info4.SubTotal;
                            num8 += info4.Quantity;
                        }
                    }
                }
                if (table2.Rows.Count > 0)
                {
                    for (int j = 0; j < table2.Rows.Count; j++)
                    {
                        if (bool.Parse(activities.Rows[i]["isAllProduct"].ToString()))
                        {
                            if (decimal.Parse(table2.Rows[j]["MeetMoney"].ToString()) > 0M)
                            {
                                if (num4 >= decimal.Parse(table2.Rows[table2.Rows.Count - 1]["MeetMoney"].ToString()))
                                {
                                    num2 = decimal.Parse(table2.Rows[table2.Rows.Count - 1]["MeetMoney"].ToString());
                                    num = decimal.Parse(table2.Rows[table2.Rows.Count - 1]["ReductionMoney"].ToString());
                                    break;
                                }
                                if (num4 <= decimal.Parse(table2.Rows[0]["MeetMoney"].ToString()))
                                {
                                    num2 = decimal.Parse(table2.Rows[0]["MeetMoney"].ToString());
                                    num = decimal.Parse(table2.Rows[0]["ReductionMoney"].ToString());
                                    break;
                                }
                                if (num4 >= decimal.Parse(table2.Rows[j]["MeetMoney"].ToString()))
                                {
                                    num2 = decimal.Parse(table2.Rows[j]["MeetMoney"].ToString());
                                    num = decimal.Parse(table2.Rows[j]["ReductionMoney"].ToString());
                                }
                            }
                            else
                            {
                                if (num5 >= int.Parse(table2.Rows[table2.Rows.Count - 1]["MeetNumber"].ToString()))
                                {
                                    num2 = decimal.Parse(table2.Rows[table2.Rows.Count - 1]["MeetMoney"].ToString());
                                    num3 = decimal.Parse(table2.Rows[table2.Rows.Count - 1]["ReductionMoney"].ToString());
                                    break;
                                }
                                if (num5 <= int.Parse(table2.Rows[0]["MeetNumber"].ToString()))
                                {
                                    num2 = decimal.Parse(table2.Rows[0]["MeetMoney"].ToString());
                                    num3 = decimal.Parse(table2.Rows[0]["ReductionMoney"].ToString());
                                    break;
                                }
                                if (num5 >= int.Parse(table2.Rows[j]["MeetNumber"].ToString()))
                                {
                                    num2 = decimal.Parse(table2.Rows[j]["MeetMoney"].ToString());
                                    num3 = decimal.Parse(table2.Rows[j]["ReductionMoney"].ToString());
                                }
                            }
                        }
                        else
                        {
                            num4 = num7;
                            num5 = num8;
                            if (decimal.Parse(table2.Rows[j]["MeetMoney"].ToString()) > 0M)
                            {
                                if (num4 >= decimal.Parse(table2.Rows[table2.Rows.Count - 1]["MeetMoney"].ToString()))
                                {
                                    num2 = decimal.Parse(table2.Rows[table2.Rows.Count - 1]["MeetMoney"].ToString());
                                    num = decimal.Parse(table2.Rows[table2.Rows.Count - 1]["ReductionMoney"].ToString());
                                    break;
                                }
                                if (num4 <= decimal.Parse(table2.Rows[0]["MeetMoney"].ToString()))
                                {
                                    num2 = decimal.Parse(table2.Rows[0]["MeetMoney"].ToString());
                                    num = decimal.Parse(table2.Rows[0]["ReductionMoney"].ToString());
                                    break;
                                }
                                if (num4 >= decimal.Parse(table2.Rows[j]["MeetMoney"].ToString()))
                                {
                                    num2 = decimal.Parse(table2.Rows[j]["MeetMoney"].ToString());
                                    num = decimal.Parse(table2.Rows[j]["ReductionMoney"].ToString());
                                }
                            }
                            else
                            {
                                if (num5 >= int.Parse(table2.Rows[table2.Rows.Count - 1]["MeetNumber"].ToString()))
                                {
                                    num2 = decimal.Parse(table2.Rows[table2.Rows.Count - 1]["MeetMoney"].ToString());
                                    num = decimal.Parse(table2.Rows[table2.Rows.Count - 1]["ReductionMoney"].ToString());
                                    break;
                                }
                                if (num5 <= int.Parse(table2.Rows[0]["MeetNumber"].ToString()))
                                {
                                    num2 = decimal.Parse(table2.Rows[0]["MeetMoney"].ToString());
                                    num = decimal.Parse(table2.Rows[0]["ReductionMoney"].ToString());
                                    break;
                                }
                                if (num5 >= int.Parse(table2.Rows[j]["MeetNumber"].ToString()))
                                {
                                    num2 = decimal.Parse(table2.Rows[j]["MeetMoney"].ToString());
                                    num = decimal.Parse(table2.Rows[j]["ReductionMoney"].ToString());
                                }
                            }
                        }
                    }
                    if ((num4 >= num2) || (num2 == 0M))
                    {
                        num3 += num;
                    }
                }
            }
            return num3;
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-VSubmmitOrder.html";
            }
            base.OnInit(e);
        }

        private void rptCartProducts_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                List<ShoppingCartItemInfo> list = (List<ShoppingCartItemInfo>) DataBinder.Eval(e.Item.DataItem, "LineItems");
                Literal literal = (Literal) e.Item.Controls[0].FindControl("LitCoupon");
                Literal literal2 = (Literal) e.Item.Controls[0].FindControl("litExemption");
                Literal literal3 = (Literal) e.Item.Controls[0].FindControl("litoldExemption");
                Literal literal4 = (Literal) e.Item.Controls[0].FindControl("litoldTotal");
                Literal literal5 = (Literal) e.Item.Controls[0].FindControl("litTotal");
                Literal literal6 = (Literal) e.Item.Controls[0].FindControl("litbFreeShipping");
                string str = "";
                string str2 = " <div class=\"btn-group coupon\">";
                object obj2 = str2;
                object obj3 = string.Concat(new object[] { obj2, "<button type=\"button\" class=\"btn btn-default dropdown-toggle coupondropdown\" data-toggle=\"dropdown\"   id='coupondropdown", DataBinder.Eval(e.Item.DataItem, "TemplateId"), "'>不使用优惠券<span class=\"caret\"></span></button>" });
                str2 = string.Concat(new object[] { obj3, "<ul id=\"coupon", DataBinder.Eval(e.Item.DataItem, "TemplateId"), "\" class=\"dropdown-menu\" role=\"menu\">" });
                for (int i = 0; i < this.GetUserCoupons.Rows.Count; i++)
                {
                    if ((this.GetUserCoupons.Rows[i]["MemberGrades"].ToString() == "0") || (this.GetUserCoupons.Rows[i]["MemberGrades"].ToString() == this.member.GradeId.ToString()))
                    {
                        if (bool.Parse(this.GetUserCoupons.Rows[i]["IsAllProduct"].ToString()))
                        {
                            object obj4 = str;
                            str = string.Concat(new object[] { 
                                obj4, "<li><a onclick=\"Couponasetselect('", DataBinder.Eval(e.Item.DataItem, "TemplateId"), "','", this.GetUserCoupons.Rows[i]["CouponValue"], "元现金券','", this.GetUserCoupons.Rows[i]["CouponValue"], "',", this.GetUserCoupons.Rows[i]["Id"], ",'", this.GetUserCoupons.Rows[i]["CouponValue"], "元现金券|", this.GetUserCoupons.Rows[i]["Id"], "|", this.GetUserCoupons.Rows[i]["ConditionValue"], "|", 
                                this.GetUserCoupons.Rows[i]["CouponValue"], "')\" id=\"acoupon", DataBinder.Eval(e.Item.DataItem, "TemplateId"), this.GetUserCoupons.Rows[i]["Id"], "\" value=\"", this.GetUserCoupons.Rows[i]["Id"], "\">", this.GetUserCoupons.Rows[i]["CouponValue"], "元现金券</a></li>"
                             });
                        }
                        else
                        {
                            decimal num2 = 0M;
                            foreach (ShoppingCartItemInfo info in list)
                            {
                                if ((info.Type == 0) && (MemberProcessor.GetCouponByProducts(int.Parse(this.GetUserCoupons.Rows[i]["CouponId"].ToString()), info.ProductId).Rows.Count > 0))
                                {
                                    num2 += info.SubTotal;
                                }
                            }
                            if (decimal.Parse(this.GetUserCoupons.Rows[i]["ConditionValue"].ToString()) <= num2)
                            {
                                object obj5 = str;
                                str = string.Concat(new object[] { 
                                    obj5, "<li><a onclick=\"Couponasetselect('", DataBinder.Eval(e.Item.DataItem, "TemplateId"), "','", this.GetUserCoupons.Rows[i]["CouponValue"], "元现金券','", this.GetUserCoupons.Rows[i]["CouponValue"], "',", this.GetUserCoupons.Rows[i]["Id"], ",'", this.GetUserCoupons.Rows[i]["CouponValue"], "元现金券|", this.GetUserCoupons.Rows[i]["Id"], "|", this.GetUserCoupons.Rows[i]["ConditionValue"], "|", 
                                    this.GetUserCoupons.Rows[i]["CouponValue"], "')\" id=\"acoupon", DataBinder.Eval(e.Item.DataItem, "TemplateId"), this.GetUserCoupons.Rows[i]["Id"], "\" value=\"", this.GetUserCoupons.Rows[i]["Id"], "\">", this.GetUserCoupons.Rows[i]["CouponValue"], "元现金券</a></li>"
                                 });
                            }
                        }
                    }
                }
                object obj6 = str2 + str;
                str2 = string.Concat(new object[] { obj6, "</ul></div><input type=\"hidden\"  class=\"ClassCoupon\"   id=\"selectCoupon", DataBinder.Eval(e.Item.DataItem, "TemplateId"), "\"/>  " });
                if (!string.IsNullOrEmpty(str))
                {
                    literal.Text = string.Concat(new object[] { str2, "<input type=\"hidden\"   id='selectCouponValue", DataBinder.Eval(e.Item.DataItem, "TemplateId"), "' class=\"selectCouponValue\" />" });
                }
                else
                {
                    literal.Text = "<input type=\"hidden\"   id='selectCouponValue" + DataBinder.Eval(e.Item.DataItem, "TemplateId") + "' class=\"selectCouponValue\" />";
                }
                decimal num3 = 0M;
                decimal num4 = 0M;
                decimal num5 = 0M;
                decimal num6 = 0M;
                decimal num7 = 0M;
                int num8 = 0;
                foreach (ShoppingCartItemInfo info2 in list)
                {
                    if (info2.Type == 0)
                    {
                        num7 += info2.SubTotal;
                        num8 += info2.Quantity;
                    }
                }
                num6 = num7;
                for (int j = 0; j < this.dtActivities.Rows.Count; j++)
                {
                    if ((int.Parse(this.dtActivities.Rows[j]["attendTime"].ToString()) != 0) && (int.Parse(this.dtActivities.Rows[j]["attendTime"].ToString()) <= ActivityHelper.GetActivitiesMember(this.member.UserId, int.Parse(this.dtActivities.Rows[j]["ActivitiesId"].ToString()))))
                    {
                        continue;
                    }
                    decimal num10 = 0M;
                    int num11 = 0;
                    DataTable table2 = ActivityHelper.GetActivities_Detail(int.Parse(this.dtActivities.Rows[j]["ActivitiesId"].ToString()));
                    foreach (ShoppingCartItemInfo info3 in list)
                    {
                        if ((info3.Type == 0) && (ActivityHelper.GetActivitiesProducts(int.Parse(this.dtActivities.Rows[j]["ActivitiesId"].ToString()), info3.ProductId).Rows.Count > 0))
                        {
                            num10 += info3.SubTotal;
                            num11 += info3.Quantity;
                        }
                    }
                    bool flag = false;
                    if (table2.Rows.Count > 0)
                    {
                        for (int k = 0; k < table2.Rows.Count; k++)
                        {
                            if ((table2.Rows[k]["MemberGrades"].ToString() == "0") || (table2.Rows[k]["MemberGrades"].ToString() == this.member.GradeId.ToString()))
                            {
                                if (bool.Parse(this.dtActivities.Rows[j]["isAllProduct"].ToString()))
                                {
                                    if (decimal.Parse(table2.Rows[k]["MeetMoney"].ToString()) > 0M)
                                    {
                                        if ((num7 != 0M) && (num7 >= decimal.Parse(table2.Rows[table2.Rows.Count - 1]["MeetMoney"].ToString())))
                                        {
                                            num4 = decimal.Parse(table2.Rows[table2.Rows.Count - 1]["MeetMoney"].ToString());
                                            num3 = decimal.Parse(table2.Rows[table2.Rows.Count - 1]["ReductionMoney"].ToString());
                                            literal6.Text = table2.Rows[table2.Rows.Count - 1]["bFreeShipping"].ToString();
                                            break;
                                        }
                                        if ((num7 != 0M) && (num7 <= decimal.Parse(table2.Rows[0]["MeetMoney"].ToString())))
                                        {
                                            num4 = decimal.Parse(table2.Rows[0]["MeetMoney"].ToString());
                                            num3 = decimal.Parse(table2.Rows[0]["ReductionMoney"].ToString());
                                            literal6.Text = table2.Rows[0]["bFreeShipping"].ToString();
                                            break;
                                        }
                                        if ((num7 != 0M) && (num7 >= decimal.Parse(table2.Rows[k]["MeetMoney"].ToString())))
                                        {
                                            num4 = decimal.Parse(table2.Rows[k]["MeetMoney"].ToString());
                                            num3 = decimal.Parse(table2.Rows[k]["ReductionMoney"].ToString());
                                            literal6.Text = table2.Rows[k]["bFreeShipping"].ToString();
                                        }
                                    }
                                    else
                                    {
                                        if ((num8 != 0) && (num8 >= int.Parse(table2.Rows[table2.Rows.Count - 1]["MeetNumber"].ToString())))
                                        {
                                            num4 = decimal.Parse(table2.Rows[table2.Rows.Count - 1]["MeetMoney"].ToString());
                                            num5 = decimal.Parse(table2.Rows[table2.Rows.Count - 1]["ReductionMoney"].ToString());
                                            flag = true;
                                            literal6.Text = table2.Rows[table2.Rows.Count - 1]["bFreeShipping"].ToString();
                                            break;
                                        }
                                        if ((num8 != 0) && (num8 <= int.Parse(table2.Rows[0]["MeetNumber"].ToString())))
                                        {
                                            num4 = decimal.Parse(table2.Rows[0]["MeetMoney"].ToString());
                                            num5 = decimal.Parse(table2.Rows[0]["ReductionMoney"].ToString());
                                            flag = true;
                                            literal6.Text = table2.Rows[0]["bFreeShipping"].ToString();
                                            break;
                                        }
                                        if ((num8 != 0) && (num8 >= int.Parse(table2.Rows[k]["MeetNumber"].ToString())))
                                        {
                                            num4 = decimal.Parse(table2.Rows[k]["MeetMoney"].ToString());
                                            num5 = decimal.Parse(table2.Rows[k]["ReductionMoney"].ToString());
                                            flag = true;
                                            literal6.Text = table2.Rows[k]["bFreeShipping"].ToString();
                                        }
                                    }
                                }
                                else
                                {
                                    num7 = num10;
                                    num8 = num11;
                                    if (decimal.Parse(table2.Rows[k]["MeetMoney"].ToString()) > 0M)
                                    {
                                        if ((num7 != 0M) && (num7 >= decimal.Parse(table2.Rows[table2.Rows.Count - 1]["MeetMoney"].ToString())))
                                        {
                                            num4 = decimal.Parse(table2.Rows[table2.Rows.Count - 1]["MeetMoney"].ToString());
                                            num3 = decimal.Parse(table2.Rows[table2.Rows.Count - 1]["ReductionMoney"].ToString());
                                            literal6.Text = table2.Rows[table2.Rows.Count - 1]["bFreeShipping"].ToString();
                                            break;
                                        }
                                        if ((num7 != 0M) && (num7 <= decimal.Parse(table2.Rows[0]["MeetMoney"].ToString())))
                                        {
                                            num4 = decimal.Parse(table2.Rows[0]["MeetMoney"].ToString());
                                            num3 = decimal.Parse(table2.Rows[0]["ReductionMoney"].ToString());
                                            literal6.Text = table2.Rows[0]["bFreeShipping"].ToString();
                                            break;
                                        }
                                        if ((num7 != 0M) && (num7 >= decimal.Parse(table2.Rows[k]["MeetMoney"].ToString())))
                                        {
                                            num4 = decimal.Parse(table2.Rows[k]["MeetMoney"].ToString());
                                            num3 = decimal.Parse(table2.Rows[k]["ReductionMoney"].ToString());
                                            literal6.Text = table2.Rows[k]["bFreeShipping"].ToString();
                                        }
                                    }
                                    else
                                    {
                                        if ((num8 != 0) && (num8 >= int.Parse(table2.Rows[table2.Rows.Count - 1]["MeetNumber"].ToString())))
                                        {
                                            num4 = decimal.Parse(table2.Rows[table2.Rows.Count - 1]["MeetMoney"].ToString());
                                            num3 = decimal.Parse(table2.Rows[table2.Rows.Count - 1]["ReductionMoney"].ToString());
                                            flag = true;
                                            literal6.Text = table2.Rows[table2.Rows.Count - 1]["bFreeShipping"].ToString();
                                            break;
                                        }
                                        if ((num8 != 0) && (num8 <= int.Parse(table2.Rows[0]["MeetNumber"].ToString())))
                                        {
                                            num4 = decimal.Parse(table2.Rows[0]["MeetMoney"].ToString());
                                            num3 = decimal.Parse(table2.Rows[0]["ReductionMoney"].ToString());
                                            flag = true;
                                            literal6.Text = table2.Rows[0]["bFreeShipping"].ToString();
                                            break;
                                        }
                                        if ((num8 != 0) && (num8 >= int.Parse(table2.Rows[k]["MeetNumber"].ToString())))
                                        {
                                            num4 = decimal.Parse(table2.Rows[k]["MeetMoney"].ToString());
                                            num3 = decimal.Parse(table2.Rows[k]["ReductionMoney"].ToString());
                                            flag = true;
                                            literal6.Text = table2.Rows[k]["bFreeShipping"].ToString();
                                        }
                                    }
                                }
                            }
                        }
                        if (flag)
                        {
                            if (num8 > 0)
                            {
                                num5 += num3;
                            }
                        }
                        else if (((num7 != 0M) && (num4 != 0M)) && (num7 >= num4))
                        {
                            num5 += num3;
                        }
                    }
                }
                literal2.Text = num5.ToString("F2");
                literal3.Text = num5.ToString("F2");
                literal5.Text = (num6 - num5).ToString("F2");
                literal4.Text = (num6 - num5).ToString("F2");
            }
        }
    }
}

