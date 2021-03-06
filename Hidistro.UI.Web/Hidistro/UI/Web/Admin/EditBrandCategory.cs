﻿namespace Hidistro.UI.Web.Admin
{
    using Hidistro.ControlPanel.Commodities;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Entities.Commodities;
    using Hidistro.Entities.Store;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using Hishop.Components.Validation;
    using kindeditor.Net;
    using System;
    using System.Collections.Generic;
    using System.Web.UI.WebControls;

    [PrivilegeCheck(Privilege.BrandCategories)]
    public class EditBrandCategory : AdminPage
    {
        private int brandId;
        protected ImageLinkButton btnDeleteLogo;
        protected Button btnUpdateBrandCategory;
        protected Button btnUpoad;
        protected ProductTypesCheckBoxList chlistProductTypes;
        protected KindeditorControl fckDescription;
        protected FileUpload fileUpload;
        protected HiImage imgLogo;
        protected TextBox txtBrandName;
        protected TextBox txtCompanyUrl;
        protected TextBox txtkeyword;
        protected TextBox txtMetaDescription;
        protected TextBox txtReUrl;

        protected EditBrandCategory() : base("", "")
        {
        }

        private void btnDeleteLogo_Click(object sender, EventArgs e)
        {
            BrandCategoryInfo brandCategoryInfo = this.GetBrandCategoryInfo();
            try
            {
                ResourcesHelper.DeleteImage(brandCategoryInfo.Logo);
                brandCategoryInfo.Logo = null;
                this.ViewState["Logo"] = null;
                CatalogHelper.UpdateBrandCategory(brandCategoryInfo);
            }
            catch
            {
                this.ShowMsg("删除失败", false);
                return;
            }
            this.loadData();
        }

        protected void btnUpdateBrandCategory_Click(object sender, EventArgs e)
        {
            BrandCategoryInfo brandCategoryInfo = this.GetBrandCategoryInfo();
            if (string.IsNullOrEmpty(brandCategoryInfo.Logo))
            {
                this.ShowMsg("请上传一张品牌LOGO图片", false);
            }
            else if (this.ValidationBrandCategory(brandCategoryInfo))
            {
                if (CatalogHelper.UpdateBrandCategory(brandCategoryInfo))
                {
                    base.Response.Redirect(Globals.GetAdminAbsolutePath("/product/BrandCategories.aspx"), true);
                }
                else
                {
                    this.ShowMsg("编辑品牌分类失败", true);
                }
            }
        }

        private void btnUpoad_Click(object sender, EventArgs e)
        {
            BrandCategoryInfo brandCategoryInfo = this.GetBrandCategoryInfo();
            if (this.fileUpload.HasFile)
            {
                try
                {
                    ResourcesHelper.DeleteImage(brandCategoryInfo.Logo);
                    brandCategoryInfo.Logo = CatalogHelper.UploadBrandCategorieImage(this.fileUpload.PostedFile);
                    this.ViewState["Logo"] = brandCategoryInfo.Logo;
                }
                catch
                {
                    this.ShowMsg("图片上传失败，您选择的不是图片类型的文件，或者网站的虚拟目录没有写入文件的权限", false);
                    return;
                }
                CatalogHelper.UpdateBrandCategory(brandCategoryInfo);
            }
            this.loadData();
        }

        private BrandCategoryInfo GetBrandCategoryInfo()
        {
            BrandCategoryInfo info = new BrandCategoryInfo {
                BrandId = this.brandId
            };
            if (this.ViewState["Logo"] != null)
            {
                info.Logo = (string) this.ViewState["Logo"];
            }
            info.BrandName = Globals.HtmlEncode(this.txtBrandName.Text.Trim());
            if (!string.IsNullOrEmpty(this.txtCompanyUrl.Text))
            {
                info.CompanyUrl = this.txtCompanyUrl.Text.Trim();
            }
            else
            {
                info.CompanyUrl = null;
            }
            info.RewriteName = Globals.HtmlEncode(this.txtReUrl.Text.Trim());
            info.MetaKeywords = Globals.HtmlEncode(this.txtkeyword.Text.Trim());
            info.MetaDescription = Globals.HtmlEncode(this.txtMetaDescription.Text.Trim());
            info.Description = (!string.IsNullOrEmpty(this.fckDescription.Text) && (this.fckDescription.Text.Length > 0)) ? this.fckDescription.Text : null;
            IList<int> list = new List<int>();
            foreach (ListItem item in this.chlistProductTypes.Items)
            {
                if (item.Selected)
                {
                    list.Add(int.Parse(item.Value));
                }
            }
            info.ProductTypes = list;
            return info;
        }

        private void loadData()
        {
            BrandCategoryInfo brandCategory = CatalogHelper.GetBrandCategory(this.brandId);
            if (brandCategory == null)
            {
                base.GotoResourceNotFound();
            }
            else
            {
                this.ViewState["Logo"] = brandCategory.Logo;
                foreach (ListItem item in this.chlistProductTypes.Items)
                {
                    if (brandCategory.ProductTypes.Contains(int.Parse(item.Value)))
                    {
                        item.Selected = true;
                    }
                }
                this.txtBrandName.Text = Globals.HtmlDecode(brandCategory.BrandName);
                this.txtCompanyUrl.Text = brandCategory.CompanyUrl;
                this.txtReUrl.Text = Globals.HtmlDecode(brandCategory.RewriteName);
                this.txtkeyword.Text = Globals.HtmlDecode(brandCategory.MetaKeywords);
                this.txtMetaDescription.Text = Globals.HtmlDecode(brandCategory.MetaDescription);
                this.fckDescription.Text = brandCategory.Description;
                if (string.IsNullOrEmpty(brandCategory.Logo))
                {
                    this.btnDeleteLogo.Visible = false;
                }
                else
                {
                    this.btnDeleteLogo.Visible = true;
                }
                this.imgLogo.ImageUrl = brandCategory.Logo;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!int.TryParse(this.Page.Request.QueryString["brandId"], out this.brandId))
            {
                base.GotoResourceNotFound();
            }
            else
            {
                this.btnUpdateBrandCategory.Click += new EventHandler(this.btnUpdateBrandCategory_Click);
                this.btnUpoad.Click += new EventHandler(this.btnUpoad_Click);
                this.btnDeleteLogo.Click += new EventHandler(this.btnDeleteLogo_Click);
                if (!this.Page.IsPostBack)
                {
                    this.chlistProductTypes.DataBind();
                    this.loadData();
                }
            }
        }

        private bool ValidationBrandCategory(BrandCategoryInfo brandCategory)
        {
            ValidationResults results = Hishop.Components.Validation.Validation.Validate<BrandCategoryInfo>(brandCategory, new string[] { "ValBrandCategory" });
            string msg = string.Empty;
            if (!results.IsValid)
            {
                foreach (ValidationResult result in (IEnumerable<ValidationResult>) results)
                {
                    msg = msg + Formatter.FormatErrorMessage(result.Message);
                }
                this.ShowMsg(msg, false);
            }
            return results.IsValid;
        }
    }
}

