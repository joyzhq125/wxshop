namespace Hidistro.UI.Web.Admin.vshop
{
    using Hidistro.ControlPanel.Store;
    using Hidistro.Entities.VShop;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Collections.Generic;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;
    using System.Linq;

    public class EditBanner : AdminPage
    {
        protected Button btnEditBanner;
        protected DropDownList ddlSubType;
        protected DropDownList ddlThridType;
        protected DropDownList ddlType;
        protected HtmlInputHidden fmSrc;
        private int id;
        protected HtmlGenericControl liParent;
        protected HtmlImage littlepic;
        protected HtmlInputHidden locationUrl;
        protected HtmlGenericControl navigateDesc;
        protected TextBox Tburl;
        protected TextBox txtBannerDesc;

        protected EditBanner() : base("", "")
        {
        }

        protected void btnEditBanner_Click(object sender, EventArgs e)
        {
            TplCfgInfo tplCfgById = VShopHelper.GetTplCfgById(this.id);
            tplCfgById.IsDisable = false;
            tplCfgById.ImageUrl = this.fmSrc.Value;
            tplCfgById.ShortDesc = this.txtBannerDesc.Text;
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
                    this.Restore();
                }
            }
            else
            {
                base.Response.Redirect("ManageBanner.aspx");
            }
        }

        protected void Restore()
        {
            TplCfgInfo tplCfgById = VShopHelper.GetTplCfgById(this.id);
            this.txtBannerDesc.Text = tplCfgById.ShortDesc;
            this.ddlType.SelectedValue = tplCfgById.LocationType.ToString();
            this.littlepic.Src = tplCfgById.ImageUrl;
            this.fmSrc.Value = tplCfgById.ImageUrl;
            switch (tplCfgById.LocationType)
            {
                case LocationType.Vote:
                {
                    this.ddlSubType.Attributes.CssStyle.Remove("display");
                    IList<Hidistro.Entities.Promotions.VoteInfo> voteList = StoreHelper.GetVoteList();
                    if ((voteList != null) && (voteList.Count > 0))
                    {
                        this.ddlSubType.DataSource = voteList;
                        this.ddlSubType.DataTextField = "VoteName";
                        this.ddlSubType.DataValueField = "VoteId";
                        this.ddlSubType.DataBind();
                        this.ddlSubType.SelectedValue = tplCfgById.Url;
                    }
                    return;
                }
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

