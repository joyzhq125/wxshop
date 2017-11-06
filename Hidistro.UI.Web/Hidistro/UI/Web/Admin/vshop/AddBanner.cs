namespace Hidistro.UI.Web.Admin.vshop
{
    using Hidistro.ControlPanel.Store;
    using Hidistro.Entities.VShop;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class AddBanner : AdminPage
    {
        protected Button btnAddBanner;
        protected DropDownList ddlSubType;
        protected DropDownList ddlThridType;
        protected DropDownList ddlType;
        protected HtmlInputHidden fmSrc;
        protected HtmlGenericControl liParent;
        protected HtmlInputHidden locationUrl;
        protected HtmlGenericControl navigateDesc;
        protected TextBox Tburl;
        protected TextBox txtBannerDesc;

        protected AddBanner() : base("", "")
        {
        }

        protected void btnAddBanner_Click(object sender, EventArgs e)
        {
            if (VShopHelper.GetCountBanner() < 4)
            {
                TplCfgInfo info = new BannerInfo {
                    IsDisable = false,
                    ImageUrl = this.fmSrc.Value,
                    ShortDesc = this.txtBannerDesc.Text,
                    LocationType = (LocationType) Enum.Parse(typeof(LocationType), this.ddlType.SelectedValue),
                    Url = this.locationUrl.Value
                };
                if (VShopHelper.SaveTplCfg(info))
                {
                    this.CloseWindow();
                }
                else
                {
                    this.ShowMsg("添加错误！", false);
                }
            }
            else
            {
                this.ShowMsg("最多只能添加4条记录！", false);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                this.ddlType.BindEnum<LocationType>("VipCard");
            }
        }

        private void Reset()
        {
            this.txtBannerDesc.Text = string.Empty;
            this.Tburl.Text = string.Empty;
            this.ddlType.SelectedValue = LocationType.Link.ToString();
        }
    }
}

