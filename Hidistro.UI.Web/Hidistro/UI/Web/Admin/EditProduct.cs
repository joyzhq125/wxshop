namespace Hidistro.UI.Web.Admin
{
    using Hidistro.ControlPanel.Commodities;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Entities.Commodities;
    using Hidistro.Entities.Store;
    using Hidistro.UI.Common.Controls;
    using Hishop.Components.Validation;
    using kindeditor.Net;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    [PrivilegeCheck(Privilege.EditProducts)]
    public class EditProduct : ProductBasePage
    {
        protected Button btnSave;
        private int categoryId;
        protected CheckBox ChkisfreeShipping;
        protected CheckBox chkSkuEnabled;
        protected CheckBox ckbIsDownPic;
        protected BrandCategoriesDropDownList dropBrandCategories;
        protected ProductTypeDownList dropProductTypes;
        protected KindeditorControl fckDescription;
        protected HtmlGenericControl l_tags;
        protected Literal litCategoryName;
        protected ProductTagsLiteral litralProductTag;
        protected HyperLink lnkEditCategory;
        private int productId;
        protected RadioButton radInStock;
        protected RadioButton radOnSales;
        protected RadioButton radUnSales;
        protected string ReUrl = "productonsales.aspx";
        protected Script Script1;
        protected Script Script2;
        protected Script Script3;
        private string toline = "";
        protected TrimTextBox txtAttributes;
        protected TrimTextBox txtCostPrice;
        protected TrimTextBox txtDisplaySequence;
        protected TrimTextBox txtMarketPrice;
        protected TrimTextBox txtMemberPrices;
        protected TrimTextBox txtProductCode;
        protected TrimTextBox txtProductName;
        protected TrimTextBox txtProductTag;
        protected TrimTextBox txtSalePrice;
        protected TrimTextBox txtShortDescription;
        protected TrimTextBox txtSku;
        protected TrimTextBox txtSkus;
        protected TrimTextBox txtStock;
        protected TrimTextBox txtUnit;
        protected TrimTextBox txtWeight;
        protected ProductFlashUpload ucFlashUpload1;

        private void btnSave_Click(object sender, EventArgs e)
        {
            int num2;
            int num3;
            decimal num4;
            decimal? nullable;
            decimal? nullable2;
            decimal? nullable3;
            string str = this.ucFlashUpload1.Value.Trim();
            this.ucFlashUpload1.Value = str;
            string[] strArray = str.Split(new char[] { ',' });
            string[] strArray2 = new string[] { "", "", "", "", "" };
            for (int i = 0; (i < strArray.Length) && (i < 5); i++)
            {
                strArray2[i] = strArray[i];
            }
            if (this.categoryId == 0)
            {
                this.categoryId = (int) this.ViewState["ProductCategoryId"];
            }
            if (this.ValidateConverts(this.chkSkuEnabled.Checked, out num2, out num4, out nullable, out nullable2, out num3, out nullable3))
            {
                if (!this.chkSkuEnabled.Checked)
                {
                    if (num4 <= 0M)
                    {
                        this.ShowMsg("商品一口价必须大于0", false);
                        return;
                    }
                    if (nullable.HasValue && (nullable.Value >= num4))
                    {
                        this.ShowMsg("商品成本价必须小于商品一口价", false);
                        return;
                    }
                }
                string text = this.fckDescription.Text;
                if (this.ckbIsDownPic.Checked)
                {
                    text = base.DownRemotePic(text);
                }
                ProductInfo target = new ProductInfo {
                    ProductId = this.productId,
                    CategoryId = this.categoryId,
                    TypeId = this.dropProductTypes.SelectedValue,
                    ProductName = this.txtProductName.Text,
                    ProductCode = this.txtProductCode.Text,
                    DisplaySequence = num2,
                    MarketPrice = nullable2,
                    Unit = this.txtUnit.Text,
                    ImageUrl1 = strArray2[0],
                    ImageUrl2 = strArray2[1],
                    ImageUrl3 = strArray2[2],
                    ImageUrl4 = strArray2[3],
                    ImageUrl5 = strArray2[4],
                    ThumbnailUrl40 = strArray2[0].Replace("/images/", "/thumbs40/40_"),
                    ThumbnailUrl60 = strArray2[0].Replace("/images/", "/thumbs60/60_"),
                    ThumbnailUrl100 = strArray2[0].Replace("/images/", "/thumbs100/100_"),
                    ThumbnailUrl160 = strArray2[0].Replace("/images/", "/thumbs160/160_"),
                    ThumbnailUrl180 = strArray2[0].Replace("/images/", "/thumbs180/180_"),
                    ThumbnailUrl220 = strArray2[0].Replace("/images/", "/thumbs220/220_"),
                    ThumbnailUrl310 = strArray2[0].Replace("/images/", "/thumbs310/310_"),
                    ThumbnailUrl410 = strArray2[0].Replace("/images/", "/thumbs410/410_"),
                    ShortDescription = this.txtShortDescription.Text,
                    IsfreeShipping = this.ChkisfreeShipping.Checked,
                    Description = (!string.IsNullOrEmpty(text) && (text.Length > 0)) ? text : null,
                    AddedDate = DateTime.Now,
                    BrandId = this.dropBrandCategories.SelectedValue
                };
                ProductSaleStatus onSale = ProductSaleStatus.OnSale;
                if (this.radInStock.Checked)
                {
                    onSale = ProductSaleStatus.OnStock;
                }
                if (this.radUnSales.Checked)
                {
                    onSale = ProductSaleStatus.UnSale;
                }
                if (this.radOnSales.Checked)
                {
                    onSale = ProductSaleStatus.OnSale;
                }
                target.SaleStatus = onSale;
                CategoryInfo category = CatalogHelper.GetCategory(this.categoryId);
                if (category != null)
                {
                    target.MainCategoryPath = category.Path + "|";
                }
                Dictionary<string, SKUItem> skus = null;
                Dictionary<int, IList<int>> attrs = null;
                if (this.chkSkuEnabled.Checked)
                {
                    target.HasSKU = true;
                    skus = base.GetSkus(this.txtSkus.Text);
                }
                else
                {
                    Dictionary<string, SKUItem> dictionary3 = new Dictionary<string, SKUItem>();
                    SKUItem item = new SKUItem {
                        SkuId = "0",
                        SKU = this.txtSku.Text,
                        SalePrice = num4,
                        CostPrice = nullable.HasValue ? nullable.Value : 0M,
                        Stock = num3,
                        Weight = nullable3.HasValue ? nullable3.Value : 0M
                    };
                    dictionary3.Add("0", item);
                    skus = dictionary3;
                    if (this.txtMemberPrices.Text.Length > 0)
                    {
                        base.GetMemberPrices(skus["0"], this.txtMemberPrices.Text);
                    }
                }
                if (!string.IsNullOrEmpty(this.txtAttributes.Text) && (this.txtAttributes.Text.Length > 0))
                {
                    attrs = base.GetAttributes(this.txtAttributes.Text);
                }
                ValidationResults validateResults = Hishop.Components.Validation.Validation.Validate<ProductInfo>(target);
                if (!validateResults.IsValid)
                {
                    this.ShowMsg(validateResults);
                }
                else
                {
                    IList<int> tagIds = new List<int>();
                    if (!string.IsNullOrEmpty(this.txtProductTag.Text.Trim()))
                    {
                        string str3 = this.txtProductTag.Text.Trim();
                        string[] strArray3 = null;
                        if (str3.Contains(","))
                        {
                            strArray3 = str3.Split(new char[] { ',' });
                        }
                        else
                        {
                            strArray3 = new string[] { str3 };
                        }
                        foreach (string str4 in strArray3)
                        {
                            tagIds.Add(Convert.ToInt32(str4));
                        }
                    }
                    ProductInfo productBaseInfo = ProductHelper.GetProductBaseInfo(this.productId);
                    target.SaleCounts = productBaseInfo.SaleCounts;
                    target.ShowSaleCounts = productBaseInfo.ShowSaleCounts;
                    ProductActionStatus status2 = ProductHelper.UpdateProduct(target, skus, attrs, tagIds,wid);
                    if (status2 == ProductActionStatus.Success)
                    {
                        this.litralProductTag.SelectedValue = tagIds;
                        if (base.Request.QueryString["reurl"] != null)
                        {
                            this.ReUrl = base.Request.QueryString["reurl"].ToString();
                        }
                        this.ShowMsgAndReUrl("修改商品成功", true, this.ReUrl);
                    }
                    else
                    {
                        switch (status2)
                        {
                            case ProductActionStatus.AttributeError:
                                this.ShowMsg("修改商品失败，保存商品属性时出错", false);
                                return;

                            case ProductActionStatus.DuplicateName:
                                this.ShowMsg("修改商品失败，商品名称不能重复", false);
                                return;

                            case ProductActionStatus.DuplicateSKU:
                                this.ShowMsg("修改商品失败，商家编码不能重复", false);
                                return;

                            case ProductActionStatus.SKUError:
                                this.ShowMsg("修改商品失败，商家编码不能重复", false);
                                return;

                            case ProductActionStatus.OffShelfError:
                                this.ShowMsg("修改商品失败， 子站没在零售价范围内的商品无法下架", false);
                                return;

                            case ProductActionStatus.ProductTagEroor:
                                this.ShowMsg("修改商品失败，保存商品标签时出错", false);
                                return;
                        }
                        this.ShowMsg("修改商品失败，未知错误", false);
                    }
                }
            }
        }

        private void LoadProduct(ProductInfo product, Dictionary<int, IList<int>> attrs)
        {
            this.dropProductTypes.SelectedValue = product.TypeId;
            this.dropBrandCategories.SelectedValue = product.BrandId;
            this.txtDisplaySequence.Text = product.DisplaySequence.ToString();
            this.txtProductName.Text = Globals.HtmlDecode(product.ProductName);
            this.txtProductCode.Text = product.ProductCode;
            this.txtUnit.Text = product.Unit;
            if (product.MarketPrice.HasValue)
            {
                this.txtMarketPrice.Text = product.MarketPrice.Value.ToString("F2");
            }
            this.txtShortDescription.Text = product.ShortDescription;
            this.fckDescription.Text = product.Description;
            if (product.SaleStatus == ProductSaleStatus.OnSale)
            {
                this.radOnSales.Checked = true;
            }
            else if (product.SaleStatus == ProductSaleStatus.UnSale)
            {
                this.radUnSales.Checked = true;
            }
            else
            {
                this.radInStock.Checked = true;
            }
            this.ChkisfreeShipping.Checked = product.IsfreeShipping;
            string str = product.ImageUrl1 + "," + product.ImageUrl2 + "," + product.ImageUrl3 + "," + product.ImageUrl4 + "," + product.ImageUrl5;
            this.ucFlashUpload1.Value = str.Replace(",,", ",").Replace(",,", ",").Trim(new char[] { ',' });
            if ((attrs != null) && (attrs.Count > 0))
            {
                StringBuilder builder = new StringBuilder();
                builder.Append("<xml><attributes>");
                foreach (int num in attrs.Keys)
                {
                    builder.Append("<item attributeId=\"").Append(num.ToString(CultureInfo.InvariantCulture)).Append("\" usageMode=\"").Append(((int) ProductTypeHelper.GetAttribute(num).UsageMode).ToString()).Append("\" >");
                    foreach (int num2 in attrs[num])
                    {
                        builder.Append("<attValue valueId=\"").Append(num2.ToString(CultureInfo.InvariantCulture)).Append("\" />");
                    }
                    builder.Append("</item>");
                }
                builder.Append("</attributes></xml>");
                this.txtAttributes.Text = builder.ToString();
            }
            this.chkSkuEnabled.Checked = product.HasSKU;
            if (product.HasSKU)
            {
                StringBuilder builder2 = new StringBuilder();
                builder2.Append("<xml><productSkus>");
                foreach (string str2 in product.Skus.Keys)
                {
                    SKUItem item = product.Skus[str2];
                    string str3 = ("<item skuCode=\"" + item.SKU + "\" salePrice=\"" + item.SalePrice.ToString("F2") + "\" costPrice=\"" + ((item.CostPrice > 0M) ? item.CostPrice.ToString("F2") : "") + "\" qty=\"" + item.Stock.ToString(CultureInfo.InvariantCulture) + "\" weight=\"" + ((item.Weight > 0M) ? item.Weight.ToString("F2") : "") + "\">") + "<skuFields>";
                    foreach (int num3 in item.SkuItems.Keys)
                    {
                        string[] strArray3 = new string[] { "<sku attributeId=\"", num3.ToString(CultureInfo.InvariantCulture), "\" valueId=\"", item.SkuItems[num3].ToString(CultureInfo.InvariantCulture), "\" />" };
                        string str4 = string.Concat(strArray3);
                        str3 = str3 + str4;
                    }
                    str3 = str3 + "</skuFields>";
                    if (item.MemberPrices.Count > 0)
                    {
                        str3 = str3 + "<memberPrices>";
                        foreach (int num4 in item.MemberPrices.Keys)
                        {
                            decimal num14 = item.MemberPrices[num4];
                            str3 = str3 + string.Format("<memberGrande id=\"{0}\" price=\"{1}\" />", num4.ToString(CultureInfo.InvariantCulture), num14.ToString("F2"));
                        }
                        str3 = str3 + "</memberPrices>";
                    }
                    str3 = str3 + "</item>";
                    builder2.Append(str3);
                }
                builder2.Append("</productSkus></xml>");
                this.txtSkus.Text = builder2.ToString();
            }
            SKUItem defaultSku = product.DefaultSku;
            this.txtSku.Text = product.SKU;
            this.txtSalePrice.Text = defaultSku.SalePrice.ToString("F2");
            this.txtCostPrice.Text = (defaultSku.CostPrice > 0M) ? defaultSku.CostPrice.ToString("F2") : "";
            this.txtStock.Text = defaultSku.Stock.ToString(CultureInfo.InvariantCulture);
            this.txtWeight.Text = (defaultSku.Weight > 0M) ? defaultSku.Weight.ToString("F2") : "";
            if (defaultSku.MemberPrices.Count > 0)
            {
                this.txtMemberPrices.Text = "<xml><gradePrices>";
                foreach (int num5 in defaultSku.MemberPrices.Keys)
                {
                    decimal num19 = defaultSku.MemberPrices[num5];
                    this.txtMemberPrices.Text = this.txtMemberPrices.Text + string.Format("<grande id=\"{0}\" price=\"{1}\" />", num5.ToString(CultureInfo.InvariantCulture), num19.ToString("F2"));
                }
                this.txtMemberPrices.Text = this.txtMemberPrices.Text + "</gradePrices></xml>";
            }
        }

        protected override void OnInitComplete(EventArgs e)
        {
            base.OnInitComplete(e);
            this.btnSave.Click += new EventHandler(this.btnSave_Click);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            wid = GetCurWebId();
            if (string.IsNullOrEmpty(wid)) return;
            int.TryParse(base.Request.QueryString["productId"], out this.productId);
            int.TryParse(base.Request.QueryString["categoryId"], out this.categoryId);
            if (!this.Page.IsPostBack)
            {
                Dictionary<int, IList<int>> dictionary;
                IList<int> tagsId = null;
                ProductInfo product = ProductHelper.GetProductDetails(this.productId, out dictionary, out tagsId);
                if (product == null)
                {
                    base.GotoResourceNotFound();
                }
                else
                {
                    if (!string.IsNullOrEmpty(base.Request.QueryString["categoryId"]))
                    {
                        this.litCategoryName.Text = CatalogHelper.GetFullCategory(this.categoryId);
                        this.ViewState["ProductCategoryId"] = this.categoryId;
                        this.lnkEditCategory.NavigateUrl = "SelectCategory.aspx?categoryId=" + this.categoryId.ToString(CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        this.litCategoryName.Text = CatalogHelper.GetFullCategory(product.CategoryId);
                        this.ViewState["ProductCategoryId"] = product.CategoryId;
                        this.lnkEditCategory.NavigateUrl = "SelectCategory.aspx?categoryId=" + product.CategoryId.ToString(CultureInfo.InvariantCulture);
                    }
                    this.lnkEditCategory.NavigateUrl = this.lnkEditCategory.NavigateUrl + "&productId=" + product.ProductId.ToString(CultureInfo.InvariantCulture);
                    this.litralProductTag.SelectedValue = tagsId;
                    if (tagsId.Count > 0)
                    {
                        foreach (int num in tagsId)
                        {
                            this.txtProductTag.Text = this.txtProductTag.Text + num.ToString() + ",";
                        }
                        this.txtProductTag.Text = this.txtProductTag.Text.Substring(0, this.txtProductTag.Text.Length - 1);
                    }
                    this.dropProductTypes.DataBind();
                    this.dropBrandCategories.DataBind();
                    this.LoadProduct(product, dictionary);
                }
            }
        }

        private bool ValidateConverts(bool skuEnabled, out int displaySequence, out decimal salePrice, out decimal? costPrice, out decimal? marketPrice, out int stock, out decimal? weight)
        {
            string str = string.Empty;
            costPrice = 0;
            marketPrice = 0;
            weight = 0;
            displaySequence = stock = 0;
            salePrice = 0M;
            if (string.IsNullOrEmpty(this.txtDisplaySequence.Text) || !int.TryParse(this.txtDisplaySequence.Text, out displaySequence))
            {
                str = str + Formatter.FormatErrorMessage("请正确填写商品排序");
            }
            if (this.txtProductCode.Text.Length > 20)
            {
                str = str + Formatter.FormatErrorMessage("商家编码的长度不能超过20个字符");
            }
            if (!string.IsNullOrEmpty(this.txtMarketPrice.Text))
            {
                decimal num;
                if (decimal.TryParse(this.txtMarketPrice.Text, out num))
                {
                    marketPrice = new decimal?(num);
                }
                else
                {
                    str = str + Formatter.FormatErrorMessage("请正确填写商品的市场价");
                }
            }
            if (!skuEnabled)
            {
                if (string.IsNullOrEmpty(this.txtSalePrice.Text) || !decimal.TryParse(this.txtSalePrice.Text, out salePrice))
                {
                    str = str + Formatter.FormatErrorMessage("请正确填写商品一口价");
                }
                if (!string.IsNullOrEmpty(this.txtCostPrice.Text))
                {
                    decimal num2;
                    if (decimal.TryParse(this.txtCostPrice.Text, out num2))
                    {
                        costPrice = new decimal?(num2);
                    }
                    else
                    {
                        str = str + Formatter.FormatErrorMessage("请正确填写商品的成本价");
                    }
                }
                if (string.IsNullOrEmpty(this.txtStock.Text) || !int.TryParse(this.txtStock.Text, out stock))
                {
                    str = str + Formatter.FormatErrorMessage("请正确填写商品的库存数量");
                }
                if (!string.IsNullOrEmpty(this.txtWeight.Text))
                {
                    decimal num3;
                    if (decimal.TryParse(this.txtWeight.Text, out num3))
                    {
                        weight = new decimal?(num3);
                    }
                    else
                    {
                        str = str + Formatter.FormatErrorMessage("请正确填写商品的重量");
                    }
                }
            }
            if (!string.IsNullOrEmpty(str))
            {
                this.ShowMsg(str, false);
                return false;
            }
            return true;
        }
    }
}

