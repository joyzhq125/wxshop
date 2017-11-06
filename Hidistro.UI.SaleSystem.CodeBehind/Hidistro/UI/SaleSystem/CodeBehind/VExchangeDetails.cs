namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.ControlPanel.Promotions;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Commodities;
    using Hidistro.Entities.Members;
    using Hidistro.Entities.Promotions;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.SaleSystem.Tags;
    using System;
    using System.Data;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;
    using System.Linq;

    [ParseChildren(true)]
    public class VExchangeDetails : VshopTemplatedWebControl
    {
        private int exchangeId;
        private Common_ExpandAttributes expandAttr;
        private HtmlInputHidden hdCategoryId;
        private HtmlInputHidden hdEachCount;
        private HtmlInputHidden hdGradeId;
        private HtmlInputHidden hdHasCollected;
        private HtmlInputHidden hdIsActive;
        private HtmlInputHidden hdNeedGrade;
        private HtmlInputHidden hdPoint;
        private HtmlInputHidden hdProductId;
        private HtmlInputHidden hdStock;
        private HtmlInputHidden hdTemplateid;
        private HtmlInputHidden hdUserExchanged;
        private HyperLink linkDescription;
        private Literal litActivities;
        private Literal litConsultationsCount;
        private Literal litDescription;
        private Literal litEachCount;
        private Literal litItemParams;
        private Literal litMarketPrice;
        private Literal litProdcutName;
        private Literal litReviewsCount;
        private Literal litSalePoint;
        private Literal litShortDescription;
        private Literal litSoldCount;
        private Literal litStock;
        private Literal litSurplusTime;
        private int productId;
        private VshopTemplatedRepeater rptProductImages;
        private Common_SKUSelector skuSelector;

        protected override void AttachChildControls()
        {
            if (!int.TryParse(this.Page.Request.QueryString["productId"], out this.productId) || !int.TryParse(this.Page.Request.QueryString["exchangeId"], out this.exchangeId))
            {
                base.GotoResourceNotFound("");
            }
            this.rptProductImages = (VshopTemplatedRepeater) this.FindControl("rptProductImages");
            this.litItemParams = (Literal) this.FindControl("litItemParams");
            this.litProdcutName = (Literal) this.FindControl("litProdcutName");
            this.litActivities = (Literal) this.FindControl("litActivities");
            this.litSalePoint = (Literal) this.FindControl("litSalePoint");
            this.litMarketPrice = (Literal) this.FindControl("litMarketPrice");
            this.litShortDescription = (Literal) this.FindControl("litShortDescription");
            this.litSurplusTime = (Literal) this.FindControl("litSurplusTime");
            this.litDescription = (Literal) this.FindControl("litDescription");
            this.litStock = (Literal) this.FindControl("litStock");
            this.litEachCount = (Literal) this.FindControl("litEachCount");
            this.skuSelector = (Common_SKUSelector) this.FindControl("skuSelector");
            this.linkDescription = (HyperLink) this.FindControl("linkDescription");
            this.expandAttr = (Common_ExpandAttributes) this.FindControl("ExpandAttributes");
            this.litSoldCount = (Literal) this.FindControl("litSoldCount");
            this.litConsultationsCount = (Literal) this.FindControl("litConsultationsCount");
            this.litReviewsCount = (Literal) this.FindControl("litReviewsCount");
            this.hdHasCollected = (HtmlInputHidden) this.FindControl("hdHasCollected");
            this.hdCategoryId = (HtmlInputHidden) this.FindControl("hdCategoryId");
            this.hdEachCount = (HtmlInputHidden) this.FindControl("hdEachCount");
            this.hdProductId = (HtmlInputHidden) this.FindControl("hdProductId");
            this.hdStock = (HtmlInputHidden) this.FindControl("hdStock");
            this.hdIsActive = (HtmlInputHidden) this.FindControl("hdIsActive");
            this.hdNeedGrade = (HtmlInputHidden) this.FindControl("hdNeedGrade");
            this.hdGradeId = (HtmlInputHidden) this.FindControl("hdGradeId");
            this.hdPoint = (HtmlInputHidden) this.FindControl("hdPoint");
            this.hdTemplateid = (HtmlInputHidden) this.FindControl("hdTemplateid");
            this.hdUserExchanged = (HtmlInputHidden) this.FindControl("hdUserExchanged");
            PointExChangeInfo info = PointExChangeHelper.Get(this.exchangeId);
            PointExchangeProductInfo productInfo = PointExChangeHelper.GetProductInfo(this.exchangeId, this.productId);
            ProductInfo product = ProductBrowser.GetProduct(MemberProcessor.GetCurrentMember(), this.productId);
            if (((info != null) && (product != null)) && (productInfo != null))
            {
                MemberInfo currentMember = MemberProcessor.GetCurrentMember();
                if (currentMember != null)
                {
                    this.hdGradeId.Value = currentMember.GradeId.ToString();
                    this.hdPoint.Value = currentMember.Points.ToString();
                    this.hdNeedGrade.Value = info.MemberGrades;
                    if (info.EndDate < DateTime.Now)
                    {
                        this.litSurplusTime.Text = "已结束";
                        this.hdIsActive.Value = "0";
                    }
                    else if (info.BeginDate > DateTime.Now)
                    {
                        this.litSurplusTime.Text = "未开始";
                        this.hdIsActive.Value = "0";
                    }
                    else
                    {
                        this.hdIsActive.Value = "1";
                        TimeSpan span = (TimeSpan) (info.EndDate - DateTime.Now);
                        if (span.Days > 1)
                        {
                            this.litSurplusTime.Text = string.Concat(new object[] { "还剩", span.Days, "天", span.Hours, "小时" });
                        }
                        else
                        {
                            this.litSurplusTime.Text = "还剩" + span.Hours + "小时";
                        }
                    }
                    this.hdProductId.Value = this.productId.ToString();
                    if (!string.IsNullOrEmpty(product.MainCategoryPath))
                    {
                        DataTable allFull = ProductBrowser.GetAllFull(int.Parse(product.MainCategoryPath.Split(new char[] { '|' })[0].ToString()));
                        this.litActivities.Text = "<div class=\"price clearfix\"><span class=\"title\">促销活动：</span><div class=\"all-action\">";
                        if (allFull.Rows.Count > 0)
                        {
                            for (int i = 0; i < allFull.Rows.Count; i++)
                            {
                                if (i == 0)
                                {
                                    object text = this.litActivities.Text;
                                    this.litActivities.Text = string.Concat(new object[] { text, "<div class=\"action\"><span class=\"purchase\"><a href=\"/Vshop/ActivityDetail.aspx?ActivitiesId=", allFull.Rows[i]["ActivitiesId"], "&CategoryId=", allFull.Rows[i]["ActivitiesType"], "\">", allFull.Rows[i]["ActivitiesName"].ToString(), "满", decimal.Parse(allFull.Rows[i]["MeetMoney"].ToString()).ToString("0"), "减", decimal.Parse(allFull.Rows[i]["ReductionMoney"].ToString()).ToString("0"), "</a>&nbsp;&nbsp;</span></div>" });
                                }
                                else
                                {
                                    object obj3 = this.litActivities.Text;
                                    this.litActivities.Text = string.Concat(new object[] { obj3, "<div class=\"action actionnone\"><span class=\"purchase\"><a href=\"/Vshop/ActivityDetail.aspx?ActivitiesId=", allFull.Rows[i]["ActivitiesId"], "&CategoryId=", allFull.Rows[i]["ActivitiesType"], "\">", allFull.Rows[i]["ActivitiesName"].ToString(), "满", decimal.Parse(allFull.Rows[i]["MeetMoney"].ToString()).ToString("0"), "减", decimal.Parse(allFull.Rows[i]["ReductionMoney"].ToString()).ToString("0"), "</a>&nbsp;&nbsp;</span></div>" });
                                }
                            }
                            this.litActivities.Text = this.litActivities.Text + "</div><em>&nbsp;more</em></div>";
                        }
                        else
                        {
                            this.litActivities.Text = "";
                        }
                    }
                    if (!string.IsNullOrEmpty(this.litActivities.Text) && (product == null))
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
                        this.hdCategoryId.Value = mainCategoryPath.Split(new char[] { '|' })[0];
                    }
                    else
                    {
                        this.hdCategoryId.Value = "0";
                    }
                    this.litProdcutName.Text = product.ProductName;
                    this.hdTemplateid.Value = product.FreightTemplateId.ToString();
                    this.litSalePoint.Text = productInfo.PointNumber.ToString();
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
                    int productExchangedCount = PointExChangeHelper.GetProductExchangedCount(this.exchangeId, this.productId);
                    int num3 = ((productInfo.ProductNumber - productExchangedCount) >= 0) ? (productInfo.ProductNumber - productExchangedCount) : 0;
                    this.litStock.Text = num3.ToString();
                    this.hdStock.Value = num3.ToString();
                    this.litEachCount.Text = productInfo.EachMaxNumber.ToString();
                    this.hdEachCount.Value = productInfo.EachMaxNumber.ToString();
                    this.hdUserExchanged.Value = PointExChangeHelper.GetUserProductExchangedCount(this.exchangeId, this.productId, currentMember.UserId).ToString();
                    this.skuSelector.ProductId = this.productId;
                    if (this.expandAttr != null)
                    {
                        this.expandAttr.ProductId = this.productId;
                    }
                    if (this.linkDescription != null)
                    {
                        this.linkDescription.NavigateUrl = "/Vshop/ProductDescription.aspx?productId=" + this.productId;
                    }

                    int productConsultationsCount = ProductBrowser.GetProductConsultationsCount(this.productId, false,currentMember.UserId);
                    this.litConsultationsCount.SetWhenIsNotNull(productConsultationsCount.ToString());
                    this.litReviewsCount.SetWhenIsNotNull(ProductBrowser.GetProductReviewsCount(this.productId, currentMember.UserId).ToString());
                    bool flag = false;
                    flag = ProductBrowser.CheckHasCollect(currentMember.UserId, this.productId);
                    this.hdHasCollected.SetWhenIsNotNull(flag ? "1" : "0");
                    ProductBrowser.UpdateVisitCounts(this.productId);
                    PageTitle.AddSiteNameTitle("积分商品");
                    SiteSettings masterSettings = SettingsManager.GetMasterSettings(false,wid);
                    string str3 = "";
                    if (!string.IsNullOrEmpty(masterSettings.GoodsPic))
                    {
                        str3 = Globals.HostPath(HttpContext.Current.Request.Url) + masterSettings.GoodsPic;
                    }
                    this.litItemParams.Text = string.Concat(new object[] { str3, "|", masterSettings.GoodsName, "|", masterSettings.GoodsDescription, "$", Globals.HostPath(HttpContext.Current.Request.Url), product.ImageUrl1, "|", this.litProdcutName.Text, "|", product.ShortDescription, "|", HttpContext.Current.Request.Url });
                }
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
                this.SkinName = "Skin-VExchangeDetails.html";
            }
            base.OnInit(e);
        }
    }
}

