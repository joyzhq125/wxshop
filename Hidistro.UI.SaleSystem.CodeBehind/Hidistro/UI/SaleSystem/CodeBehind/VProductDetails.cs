namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Commodities;
    using Hidistro.Entities.Members;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.SaleSystem.Tags;
    using System;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;
    using System.Linq;

    [ParseChildren(true)]
    public class VProductDetails : VshopTemplatedWebControl
    {
        private Common_ExpandAttributes expandAttr;
        private HyperLink linkDescription;
        private HtmlInputHidden litCategoryId;
        private Literal litConsultationsCount;
        private Literal litDescription;
        private HtmlInputHidden litHasCollected;
        private Literal litItemParams;
        private Literal litMarketPrice;
        private Literal litProdcutName;
        private HtmlInputHidden litproductid;
        private Literal litReviewsCount;
        private Literal litSalePrice;
        private Literal litShortDescription;
        private Literal litSoldCount;
        private Literal litStock;
        private HtmlInputHidden litTemplate;
        private int productId;
        private VshopTemplatedRepeater rptProductImages;
        private Common_SKUSelector skuSelector;

        protected override void AttachChildControls()
        {
            if (!int.TryParse(this.Page.Request.QueryString["productId"], out this.productId))
            {
                base.GotoResourceNotFound("");
            }
            this.rptProductImages = (VshopTemplatedRepeater) this.FindControl("rptProductImages");
            this.litItemParams = (Literal) this.FindControl("litItemParams");
            this.litProdcutName = (Literal) this.FindControl("litProdcutName");
            this.litSalePrice = (Literal) this.FindControl("litSalePrice");
            this.litMarketPrice = (Literal) this.FindControl("litMarketPrice");
            this.litShortDescription = (Literal) this.FindControl("litShortDescription");
            this.litDescription = (Literal) this.FindControl("litDescription");
            this.litStock = (Literal) this.FindControl("litStock");
            this.skuSelector = (Common_SKUSelector) this.FindControl("skuSelector");
            this.linkDescription = (HyperLink) this.FindControl("linkDescription");
            this.expandAttr = (Common_ExpandAttributes) this.FindControl("ExpandAttributes");
            this.litSoldCount = (Literal) this.FindControl("litSoldCount");
            this.litConsultationsCount = (Literal) this.FindControl("litConsultationsCount");
            this.litReviewsCount = (Literal) this.FindControl("litReviewsCount");
            this.litHasCollected = (HtmlInputHidden) this.FindControl("litHasCollected");
            this.litCategoryId = (HtmlInputHidden) this.FindControl("litCategoryId");
            this.litproductid = (HtmlInputHidden) this.FindControl("litproductid");
            this.litTemplate = (HtmlInputHidden) this.FindControl("litTemplate");
            ProductInfo product = ProductBrowser.GetProduct(MemberProcessor.GetCurrentMember(), this.productId);
            if (product != null)
            {
                this.litproductid.Value = this.productId.ToString();
                this.litTemplate.Value = product.FreightTemplateId.ToString();
                if (product == null)
                {
                    base.GotoResourceNotFound("此商品已不存在");
                }
                if (product.SaleStatus != ProductSaleStatus.OnSale)
                {
                    base.GotoResourceNotFound("此商品已下架");
                }
                if (this.rptProductImages != null)
                {
                    string locationUrl = "javascript:;";
                    SlideImage[] imageArray = new SlideImage[] { new SlideImage(product.ImageUrl1, locationUrl), new SlideImage(product.ImageUrl2, locationUrl), new SlideImage(product.ImageUrl3, locationUrl), new SlideImage(product.ImageUrl4, locationUrl), new SlideImage(product.ImageUrl5, locationUrl) };
                    this.rptProductImages.DataSource = from item in imageArray
                        where !string.IsNullOrWhiteSpace(item.ImageUrl)
                        select item;
                    this.rptProductImages.DataBind();
                }
                string mainCategoryPath = product.MainCategoryPath;
                if (!string.IsNullOrEmpty(mainCategoryPath))
                {
                    this.litCategoryId.Value = mainCategoryPath.Split(new char[] { '|' })[0];
                }
                else
                {
                    this.litCategoryId.Value = "0";
                }
                this.litProdcutName.Text = product.ProductName;
                this.litSalePrice.Text = product.MinSalePrice.ToString("F2");
                if (product.MarketPrice.HasValue)
                {
                    this.litMarketPrice.SetWhenIsNotNull(product.MarketPrice.GetValueOrDefault(0M).ToString("F2"));
                }
                this.litShortDescription.Text = product.ShortDescription;
                if (this.litDescription != null)
                {
                    this.litDescription.Text = product.Description;
                }
                this.litSoldCount.SetWhenIsNotNull(product.ShowSaleCounts.ToString());
                this.litStock.Text = product.Stock.ToString();
                this.skuSelector.ProductId = this.productId;
                if (this.expandAttr != null)
                {
                    this.expandAttr.ProductId = this.productId;
                }
                if (this.linkDescription != null)
                {
                    this.linkDescription.NavigateUrl = "/Vshop/ProductDescription.aspx?productId=" + this.productId;
                }

                MemberInfo currentMember = MemberProcessor.GetCurrentMember();
                int UserId = 0;
                if (currentMember != null)
                {
                    int productConsultationsCount = ProductBrowser.GetProductConsultationsCount(this.productId, false, UserId);
                    this.litConsultationsCount.SetWhenIsNotNull(productConsultationsCount.ToString());
                    this.litReviewsCount.SetWhenIsNotNull(ProductBrowser.GetProductReviewsCount(this.productId, UserId).ToString());
                }
                else
                {
                    this.litConsultationsCount.SetWhenIsNotNull("0");
                    this.litReviewsCount.SetWhenIsNotNull("0");
                }
           
                bool flag = false;
                if (currentMember != null)
                {
                    flag = ProductBrowser.CheckHasCollect(currentMember.UserId, this.productId);
                }
                this.litHasCollected.SetWhenIsNotNull(flag ? "1" : "0");
                ProductBrowser.UpdateVisitCounts(this.productId);
                PageTitle.AddSiteNameTitle("商品详情");
                SiteSettings masterSettings = SettingsManager.GetMasterSettings(false,wid);
                string str3 = "";
                if (!string.IsNullOrEmpty(masterSettings.GoodsPic))
                {
                    str3 = Globals.HostPath(HttpContext.Current.Request.Url) + masterSettings.GoodsPic;
                }
                this.litItemParams.Text = string.Concat(new object[] { str3, "|", masterSettings.GoodsName, "|", masterSettings.GoodsDescription, "$", Globals.HostPath(HttpContext.Current.Request.Url), product.ImageUrl1, "|", this.litProdcutName.Text, "|", product.ShortDescription, "|", HttpContext.Current.Request.Url });
            }
            else
            {
                HttpContext.Current.Response.Redirect("/vshop/");
                HttpContext.Current.Response.End();
            }
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-VProductDetails.html";
            }
            base.OnInit(e);
        }
    }
}

