namespace Hidistro.UI.Web.Admin.vshop
{
    using ASPNET.WebControls;
    using Hidistro.ControlPanel.Commodities;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Commodities;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Collections.Specialized;
    using System.Data;
    using System.Web.UI.WebControls;

    public class SearchHomeProduct : AdminPage
    {
        private int? brandId;
        protected ImageLinkButton btnAdd;
        protected Button btnSearch;
        private int? categoryId;
        protected BrandCategoriesDropDownList dropBrandList;
        protected ProductCategoriesDropDownList dropCategories;
        protected Grid grdproducts;
        protected PageSize hrefPageSize;
        protected Pager pager;
        protected Pager pager1;
        private string productName;
        protected TextBox txtSearchText;

        protected SearchHomeProduct() : base("", "")
        {
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            string str = base.Request.Form["CheckBoxGroup"];
            if (string.IsNullOrEmpty(str))
            {
                this.ShowMsg("请选择一件商品！", false);
            }
            else
            {
                string[] strArray = str.Split(new char[] { ',' });
                int num = 0;
                foreach (string str2 in strArray)
                {
                    if (VShopHelper.AddHomeProdcut(Convert.ToInt32(str2)))
                    {
                        num++;
                    }
                }
                if (num > 0)
                {
                    this.CloseWindow();
                }
                else
                {
                    this.ShowMsg("添加首页商品失败！", false);
                }
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            this.ReloadProductOnSales(true);
        }

        protected void DoCallback()
        {
            this.LoadParameters();
            ProductQuery query = new ProductQuery {
                PageSize = this.pager.PageSize,
                PageIndex = this.pager.PageIndex,
                SaleStatus = ProductSaleStatus.OnSale,
                IsIncludeHomeProduct = false,
                Keywords = this.productName
            };
            if (this.brandId.HasValue)
            {
                query.BrandId = new int?(this.brandId.Value);
            }
            query.CategoryId = this.categoryId;
            if (this.categoryId.HasValue)
            {
                query.MaiCategoryPath = CatalogHelper.GetCategory(this.categoryId.Value).Path;
            }
            DbQueryResult products = ProductHelper.GetProducts(query);
            DataTable data = (DataTable) products.Data;
            this.pager1.TotalRecords = this.pager.TotalRecords = products.TotalRecords;
            this.grdproducts.DataSource = data;
            this.grdproducts.DataBind();
        }

        private void LoadParameters()
        {
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["productName"]))
            {
                this.productName = Globals.UrlDecode(this.Page.Request.QueryString["productName"]);
            }
            int result = 0;
            if (int.TryParse(this.Page.Request.QueryString["categoryId"], out result))
            {
                this.categoryId = new int?(result);
            }
            int num2 = 0;
            if (int.TryParse(this.Page.Request.QueryString["brandId"], out num2))
            {
                this.brandId = new int?(num2);
            }
            this.txtSearchText.Text = this.productName;
            this.dropCategories.DataBind();
            this.dropCategories.SelectedValue = this.categoryId;
            this.dropBrandList.DataBind();
            this.dropBrandList.SelectedValue = new int?(num2);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!base.IsPostBack)
            {
                this.DoCallback();
            }
            this.btnSearch.Click += new EventHandler(this.btnSearch_Click);
            this.btnAdd.Click += new EventHandler(this.btnAdd_Click);
            CheckBoxColumn.RegisterClientCheckEvents(this.Page, this.Page.Form.ClientID);
        }

        private void ReloadProductOnSales(bool isSearch)
        {
            NameValueCollection queryStrings = new NameValueCollection();
            queryStrings.Add("productName", Globals.UrlEncode(this.txtSearchText.Text.Trim()));
            if (this.dropCategories.SelectedValue.HasValue)
            {
                queryStrings.Add("categoryId", this.dropCategories.SelectedValue.ToString());
            }
            queryStrings.Add("pageSize", this.pager.PageSize.ToString());
            if (!isSearch)
            {
                queryStrings.Add("pageIndex", this.pager.PageIndex.ToString());
            }
            if (this.dropBrandList.SelectedValue.HasValue)
            {
                queryStrings.Add("brandId", this.dropBrandList.SelectedValue.ToString());
            }
            base.ReloadPage(queryStrings);
        }
    }
}

