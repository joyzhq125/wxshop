namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.Entities.Commodities;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    [ParseChildren(true)]
    public class VProductList : VshopTemplatedWebControl
    {
        private int categoryId;
        private HiImage imgUrl;
        private string keyWord;
        private Literal litContent;
        private string pIds;
        private VshopTemplatedRepeater rptCategories;
        private VshopTemplatedRepeater rptCategoryList;
        private VshopTemplatedRepeater rptProducts;
        private HtmlInputHidden txtTotalPages;
        protected override void AttachChildControls()
        {
            int num;
            int num2;
            int num3;
            int.TryParse(this.Page.Request.QueryString["categoryId"], out this.categoryId);
            this.keyWord = this.Page.Request.QueryString["keyWord"];
            this.pIds = this.Page.Request.QueryString["pIds"];
            if (!string.IsNullOrWhiteSpace(this.keyWord))
            {
                this.keyWord = this.keyWord.Trim();
            }
            this.imgUrl = (HiImage) this.FindControl("imgUrl");
            this.litContent = (Literal) this.FindControl("litContent");
            this.rptProducts = (VshopTemplatedRepeater) this.FindControl("rptProducts");
            this.rptCategories = (VshopTemplatedRepeater) this.FindControl("rptCategories");
            this.rptCategoryList = (VshopTemplatedRepeater) this.FindControl("rptCategoryList");
            this.txtTotalPages = (HtmlInputHidden) this.FindControl("txtTotal");
            string str = this.Page.Request.QueryString["sort"];
            if (string.IsNullOrWhiteSpace(str))
            {
                str = "DisplaySequence";
            }
            string str2 = this.Page.Request.QueryString["order"];
            if (string.IsNullOrWhiteSpace(str2))
            {
                str2 = "desc";
            }
            if (!int.TryParse(this.Page.Request.QueryString["page"], out num))
            {
                num = 1;
            }
            if (!int.TryParse(this.Page.Request.QueryString["size"], out num2))
            {
                num2 = 20;
            }
            IList<CategoryInfo> maxSubCategories = CategoryBrowser.GetMaxSubCategories(this.categoryId, wid,0x3e8);
            this.rptCategories.DataSource = maxSubCategories;
            this.rptCategories.DataBind();
            DataSet categoryList = CategoryBrowser.GetCategoryList(wid);
            this.rptCategoryList.ItemDataBound += new RepeaterItemEventHandler(this.rptCategoryList_ItemDataBound);
            this.rptCategoryList.DataSource = categoryList;
            this.rptCategoryList.DataBind();
            this.rptProducts.DataSource = ProductBrowser.GetProducts(this.wid,MemberProcessor.GetCurrentMember(), null, new int?(this.categoryId), this.keyWord, num, num2, out num3, str, str2, this.pIds);
            this.rptProducts.DataBind();
            this.txtTotalPages.SetWhenIsNotNull(num3.ToString());
            PageTitle.AddSiteNameTitle("商品列表");
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-VProductList.html";
            }
            base.OnInit(e);
        }

        private void rptCategoryList_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            DataView view = (DataView) DataBinder.Eval(e.Item.DataItem, "SubCategories");
            DataRowView dataItem = (DataRowView) e.Item.DataItem;
            Convert.ToInt32(dataItem["CategoryId"]);
            Literal literal = (Literal) e.Item.Controls[0].FindControl("litPlus");
            if ((view == null) || (view.ToTable().Rows.Count == 0))
            {
                literal.Visible = false;
            }
            else
            {
                literal.Visible = true;
            }
        }
    }
}

