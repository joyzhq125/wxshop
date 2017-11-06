namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.Entities.Sales;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Collections.Generic;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    [ParseChildren(true)]
    public class VShoppingCart : VMemberTemplatedWebControl
    {
        private HtmlAnchor aLink;
        private List<ShoppingCartInfo> cart;
        private List<ShoppingCartInfo> cartPoint;
        private HtmlGenericControl divShowTotal;
        private Literal litcount;
        private Literal litExemption;
        private Literal litStoreMoney;
        private Literal litTotal;
        private Literal litTotalPoint;
        private decimal ReductionMoneyALL;
        private VshopTemplatedRepeater rptCartPointProducts;
        private VshopTemplatedRepeater rptCartProducts;

        protected override void AttachChildControls()
        {
            this.rptCartProducts = (VshopTemplatedRepeater) this.FindControl("rptCartProducts");
            this.rptCartProducts.ItemDataBound += new RepeaterItemEventHandler(this.rptCartProducts_ItemDataBound);
            this.rptCartPointProducts = (VshopTemplatedRepeater) this.FindControl("rptCartPointProducts");
            this.litTotal = (Literal) this.FindControl("litTotal");
            this.litTotalPoint = (Literal) this.FindControl("litTotalPoint");
            this.litStoreMoney = (Literal) this.FindControl("litStoreMoney");
            this.litExemption = (Literal) this.FindControl("litExemption");
            this.litcount = (Literal) this.FindControl("litcount");
            this.divShowTotal = (HtmlGenericControl) this.FindControl("divShowTotal");
            this.aLink = (HtmlAnchor) this.FindControl("aLink");
            this.Page.Session["stylestatus"] = "0";
            this.litExemption.Text = "0.00";
            this.cart = ShoppingCartProcessor.GetShoppingCartAviti(0);
            this.cartPoint = ShoppingCartProcessor.GetShoppingCartAviti(1);
            if (this.cart != null)
            {
                this.rptCartProducts.DataSource = this.cart;
                this.rptCartProducts.DataBind();
                int num = 0;
                for (int i = 0; i < this.cart.Count; i++)
                {
                    num += this.cart[i].LineItems.Count;
                }
                this.litcount.Text = num.ToString();
            }
            if (this.cartPoint != null)
            {
                this.rptCartPointProducts.DataSource = this.cartPoint;
                this.rptCartPointProducts.DataBind();
            }
            if ((this.cart != null) || (this.cartPoint != null))
            {
                this.aLink.HRef = "/Vshop/SubmmitOrder.aspx";
            }
            else
            {
                this.aLink.Attributes.Add("onclick", "alert_h('购物车中没有需要结算的商品！');");
            }
            decimal num3 = 0M;
            if (this.cart != null)
            {
                foreach (ShoppingCartInfo info in this.cart)
                {
                    num3 += info.GetAmount();
                }
            }
            int num4 = 0;
            if (this.cartPoint != null)
            {
                foreach (ShoppingCartInfo info2 in this.cartPoint)
                {
                    num4 += info2.GetTotalPoint();
                }
            }
            PageTitle.AddSiteNameTitle("购物车");
            this.litStoreMoney.Text = "￥" + num3.ToString("0.00");
            this.litTotal.Text = "￥" + ((num3 - this.ReductionMoneyALL)).ToString("0.00");
            this.litTotalPoint.Text = num4.ToString();
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-VShoppingCart.html";
            }
            base.OnInit(e);
        }

        private void rptCartProducts_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item)
            {
                ListItemType itemType = e.Item.ItemType;
            }
        }
    }
}

