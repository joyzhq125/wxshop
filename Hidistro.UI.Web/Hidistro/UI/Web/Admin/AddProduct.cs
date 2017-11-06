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
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    [PrivilegeCheck(Privilege.AddProducts)]
    public class AddProduct : ProductBasePage
    {
        protected Button btnAdd;
        private int categoryId;
        protected CheckBox ChkisfreeShipping;
        protected CheckBox chkSkuEnabled;
        protected CheckBox ckbIsDownPic;
        protected BrandCategoriesDropDownList dropBrandCategories;
        protected ProductTypeDownList dropProductTypes;
        protected KindeditorControl editDescription;
        protected HtmlGenericControl l_tags;
        protected Literal litCategoryName;
        protected ProductTagsLiteral litralProductTag;
        protected HyperLink lnkEditCategory;
        protected RadioButton radInStock;
        protected RadioButton radOnSales;
        protected RadioButton radUnSales;
        protected Script Script1;
        protected Script Script2;
        protected Script Script3;
        protected TrimTextBox txtAttributes;
        protected TrimTextBox txtCostPrice;
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

        private void btnAdd_Click(object sender, EventArgs e)
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
            if (this.ValidateConverts(this.chkSkuEnabled.Checked, out num4, out nullable, out nullable2, out num2, out nullable3, out num3))
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
                string text = this.editDescription.Text;
                if (this.ckbIsDownPic.Checked)
                {
                    text = base.DownRemotePic(text);
                }
                ProductInfo target = new ProductInfo {
                    CategoryId = this.categoryId,
                    TypeId = this.dropProductTypes.SelectedValue,
                    ProductName = this.txtProductName.Text,
                    ProductCode = this.txtProductCode.Text,
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
                    BrandId = this.dropBrandCategories.SelectedValue,
                    MainCategoryPath = CatalogHelper.GetCategory(this.categoryId).Path + "|"
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
                        Stock = num2,
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
                ValidationResults validateResults = Hishop.Components.Validation.Validation.Validate<ProductInfo>(target, new string[] { "AddProduct" });
                if (!validateResults.IsValid)
                {
                    this.ShowMsg(validateResults);
                }
                else
                {
                    IList<int> tagsId = new List<int>();
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
                            tagsId.Add(Convert.ToInt32(str4));
                        }
                    }
                    switch (ProductHelper.AddProduct(target, skus, attrs, tagsId,wid))
                    {
                        case ProductActionStatus.Success:
                            this.ShowMsg("添加商品成功", true);
                            base.Response.Redirect(Globals.GetAdminAbsolutePath(string.Format("/product/AddProductComplete.aspx?categoryId={0}&productId={1}", this.categoryId, target.ProductId)), true);
                            return;

                        case ProductActionStatus.AttributeError:
                            this.ShowMsg("添加商品失败，保存商品属性时出错", false);
                            return;

                        case ProductActionStatus.DuplicateName:
                            this.ShowMsg("添加商品失败，商品名称不能重复", false);
                            return;

                        case ProductActionStatus.DuplicateSKU:
                            this.ShowMsg("添加商品失败，商家编码不能重复", false);
                            return;

                        case ProductActionStatus.SKUError:
                            this.ShowMsg("添加商品失败，商家编码不能重复", false);
                            return;

                        case ProductActionStatus.ProductTagEroor:
                            this.ShowMsg("添加商品失败，保存商品标签时出错", false);
                            return;
                    }
                    this.ShowMsg("添加商品失败，未知错误", false);
                }
            }
        }

        protected override void OnInitComplete(EventArgs e)
        {
            base.OnInitComplete(e);
            this.btnAdd.Click += new EventHandler(this.btnAdd_Click);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            wid = GetCurWebId();
            if (string.IsNullOrEmpty(wid)) return;
            if (!string.IsNullOrEmpty(base.Request.QueryString["isCallback"]) && (base.Request.QueryString["isCallback"] == "true"))
            {
                base.DoCallback();
            }
            else if (!int.TryParse(base.Request.QueryString["categoryId"], out this.categoryId))
            {
                base.GotoResourceNotFound();
            }
            else if (!this.Page.IsPostBack)
            {
                this.litCategoryName.Text = CatalogHelper.GetFullCategory(this.categoryId);
                CategoryInfo category = CatalogHelper.GetCategory(this.categoryId);
                if (category == null)
                {
                    base.GotoResourceNotFound();
                }
                else
                {
                    if (!string.IsNullOrEmpty(this.litralProductTag.Text))
                    {
                        this.l_tags.Visible = true;
                    }
                    this.lnkEditCategory.NavigateUrl = "SelectCategory.aspx?categoryId=" + this.categoryId.ToString(CultureInfo.InvariantCulture);
                    this.dropProductTypes.DataBind();
                    this.dropProductTypes.SelectedValue = category.AssociatedProductType;
                    this.dropBrandCategories.DataBind();
                    this.txtProductCode.Text = this.txtSku.Text = category.SKUPrefix + new Random(DateTime.Now.Millisecond).Next(1, 0x1869f).ToString(CultureInfo.InvariantCulture).PadLeft(5, '0');
                }
            }
        }

        private bool ValidateConverts(bool skuEnabled, out decimal salePrice, out decimal? costPrice, out decimal? marketPrice, out int stock, out decimal? weight, out int lineId)
        {
            string str = string.Empty;
            costPrice = 0;
            marketPrice = 0;
            weight = 0;
            stock = lineId = 0;
            salePrice = 0M;
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

