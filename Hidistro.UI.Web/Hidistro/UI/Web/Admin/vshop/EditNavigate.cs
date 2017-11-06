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
    using System.Linq;

    public class EditNavigate : AdminPage
    {
        protected Button btnEditBanner;
        protected DropDownList ddlSubType;
        protected DropDownList ddlThridType;
        protected DropDownList ddlType;
        protected HtmlInputHidden fmSrc;
        protected string iconpath;
        private int id;
        protected HtmlGenericControl liParent;
        protected HtmlImage littlepic;
        protected HtmlInputHidden locationUrl;
        protected HtmlGenericControl navigateDesc;
        protected Repeater RpIcon;
        protected TextBox Tburl;
        protected string tplpath;
        protected TextBox txtNavigateDesc;

        protected EditNavigate() : base("", "")
        {
            this.tplpath = Globals.GetVshopSkinPath(null) + "/images/deskicon/";
            this.iconpath = string.Empty;
        }

        protected void BindIcons()
        {
            string str2;
            using (StreamReader reader = new StreamReader(base.Server.MapPath("/Utility/icomoon") + "/icomoon.font"))
            {
                str2 = reader.ReadToEnd();
            }
            this.RpIcon.DataSource = str2.Split(new char[] { ',' });
            this.RpIcon.DataBind();
        }

        protected void btnEditBanner_Click(object sender, EventArgs e)
        {
            TplCfgInfo tplCfgById = VShopHelper.GetTplCfgById(this.id);
            tplCfgById.IsDisable = false;
            tplCfgById.ImageUrl = this.fmSrc.Value;
            tplCfgById.ShortDesc = this.txtNavigateDesc.Text;
            tplCfgById.LocationType = (LocationType) Enum.Parse(typeof(LocationType), this.ddlType.SelectedValue);
            tplCfgById.Url = this.locationUrl.Value;
            if (VShopHelper.UpdateTplCfg(tplCfgById))
            {
                this.CloseWindow();
            }
            else
            {
                this.ShowMsg("修改失败！", false);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (int.TryParse(base.Request.QueryString["Id"], out this.id))
            {
                if (!this.Page.IsPostBack)
                {
                    this.ddlType.BindEnum<LocationType>("VipCard");
                    this.BindIcons();
                    this.Restore();
                }
            }
            else
            {
                base.Response.Redirect("ManageNavigate.aspx");
            }
        }

        protected void Restore()
        {
            TplCfgInfo tplCfgById = VShopHelper.GetTplCfgById(this.id);
            this.txtNavigateDesc.Text = tplCfgById.ShortDesc;
            this.ddlType.SelectedValue = tplCfgById.LocationType.ToString();
            if (!tplCfgById.ImageUrl.ToLower().Contains("storage/master/navigate"))
            {
                this.iconpath = tplCfgById.ImageUrl;
            }
            this.littlepic.Src = tplCfgById.ImageUrl;
            this.fmSrc.Value = tplCfgById.ImageUrl;
            switch (tplCfgById.LocationType)
            {
                case LocationType.Vote:
                    this.ddlSubType.Attributes.CssStyle.Remove("display");
                    this.ddlSubType.DataSource = StoreHelper.GetVoteList();
                    this.ddlSubType.DataTextField = "VoteName";
                    this.ddlSubType.DataValueField = "VoteId";
                    this.ddlSubType.DataBind();
                    this.ddlSubType.SelectedValue = tplCfgById.Url;
                    return;

                case LocationType.Activity:
                {
                    this.ddlSubType.Attributes.CssStyle.Remove("display");
                    this.ddlSubType.BindEnum<LotteryActivityType>("");
                    this.ddlSubType.SelectedValue = tplCfgById.Url.Split(new char[] { ',' })[0];
                    this.ddlThridType.Attributes.CssStyle.Remove("display");
                    LotteryActivityType type = (LotteryActivityType) Enum.Parse(typeof(LotteryActivityType), tplCfgById.Url.Split(new char[] { ',' })[0]);
                    if (type != LotteryActivityType.SignUp)
                    {
                        this.ddlThridType.DataSource = VShopHelper.GetLotteryActivityByType(type);
                        break;
                    }
                    this.ddlThridType.DataSource = from item in VShopHelper.GetAllActivity() select new { ActivityId = item.ActivityId, ActivityName = item.Name };
                    break;
                }
                case LocationType.Home:
                case LocationType.Category:
                case LocationType.ShoppingCart:
                case LocationType.OrderCenter:
                case (LocationType.OrderCenter | LocationType.Vote):
                    return;

                case LocationType.Link:
                    this.Tburl.Text = tplCfgById.Url;
                    this.Tburl.Attributes.CssStyle.Remove("display");
                    return;

                case LocationType.Phone:
                    this.Tburl.Text = tplCfgById.Url;
                    this.Tburl.Attributes.CssStyle.Remove("display");
                    return;

                case LocationType.Address:
                    this.Tburl.Attributes.CssStyle.Remove("display");
                    this.navigateDesc.Attributes.CssStyle.Remove("display");
                    this.Tburl.Text = tplCfgById.Url;
                    return;

                default:
                    return;
            }
            this.ddlThridType.DataTextField = "ActivityName";
            this.ddlThridType.DataValueField = "Activityid";
            this.ddlThridType.DataBind();
            this.ddlThridType.SelectedValue = tplCfgById.Url.Split(new char[] { ',' })[1];
        }
    }
}

