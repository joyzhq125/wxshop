namespace Hidistro.UI.Web.Admin.vshop
{
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Entities.VShop;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.IO;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class AddNavigate : AdminPage
    {
        protected Button btnAddBanner;
        protected DropDownList ddlSubType;
        protected DropDownList ddlThridType;
        protected DropDownList ddlType;
        protected HtmlInputHidden fmSrc;
        protected Repeater FontIcon;
        protected HtmlGenericControl liParent;
        protected HtmlImage littlepic;
        protected HtmlInputHidden locationUrl;
        protected HtmlGenericControl navigateDesc;
        protected TextBox Tburl;
        protected string tplpath;
        protected TextBox txtNavigateDesc;

        protected AddNavigate() : base("", "")
        {
            this.tplpath = Globals.GetVshopSkinPath(null) + "/images/deskicon/";
        }

        protected void BindIcons()
        {
            string str2;
            using (StreamReader reader = new StreamReader(base.Server.MapPath("/Utility/icomoon") + "/icomoon.font"))
            {
                str2 = reader.ReadToEnd();
            }
            this.FontIcon.DataSource = str2.Split(new char[] { ',' });
            this.FontIcon.DataBind();
        }

        protected void btnAddBanner_Click(object sender, EventArgs e)
        {
            TplCfgInfo info = new NavigateInfo {
                IsDisable = false,
                ImageUrl = this.fmSrc.Value,
                ShortDesc = this.txtNavigateDesc.Text,
                LocationType = (LocationType) Enum.Parse(typeof(LocationType), this.ddlType.SelectedValue),
                Url = this.locationUrl.Value
            };
            if (string.IsNullOrWhiteSpace(this.locationUrl.Value))
            {
                this.ShowMsg("跳转页不能为空！", false);
            }
            else if (VShopHelper.SaveTplCfg(info))
            {
                this.CloseWindow();
            }
            else
            {
                this.ShowMsg("添加错误！", false);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                this.ddlType.BindEnum<LocationType>("VipCard");
                this.BindIcons();
            }
        }

        private void Reset()
        {
            this.txtNavigateDesc.Text = string.Empty;
            this.Tburl.Text = string.Empty;
            this.ddlType.SelectedValue = LocationType.Link.ToString();
        }
    }
}

