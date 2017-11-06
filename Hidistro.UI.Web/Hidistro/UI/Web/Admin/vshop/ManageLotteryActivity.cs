namespace Hidistro.UI.Web.Admin.vshop
{
    using ASPNET.WebControls;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities.VShop;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class ManageLotteryActivity : AdminPage
    {
        protected HtmlAnchor addactivity;
        protected Literal Litdesc;
        protected Literal LitTitle;
        protected Pager pager;
        protected Repeater rpMaterial;
        protected int type;

        protected ManageLotteryActivity() : base("", "")
        {
        }

        protected void BindMaterial()
        {
            LotteryActivityQuery page = new LotteryActivityQuery {
                ActivityType = (LotteryActivityType) this.type,
                PageIndex = this.pager.PageIndex,
                PageSize = this.pager.PageSize,
                SortBy = "ActivityId",
                SortOrder = SortAction.Desc
            };
            DbQueryResult lotteryActivityList = VShopHelper.GetLotteryActivityList(page);
            this.rpMaterial.DataSource = lotteryActivityList.Data;
            this.rpMaterial.DataBind();
            this.pager.TotalRecords = lotteryActivityList.TotalRecords;
        }

        public string GetUrl(object activityId)
        {
            return string.Empty;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (int.TryParse(base.Request.QueryString["type"], out this.type))
            {
                this.addactivity.HRef = "VLotteryActivity.aspx?type=" + this.type;
                if (!this.Page.IsPostBack)
                {
                    this.BindMaterial();
                }
            }
            else
            {
                this.ShowMsg("参数错误", false);
            }
        }

        protected void rpMaterial_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "del")
            {
                if (VShopHelper.DeleteLotteryActivity(Convert.ToInt32(e.CommandArgument), ((LotteryActivityType) this.type).ToString()))
                {
                    this.ShowMsg("删除成功", true);
                    this.BindMaterial();
                }
                else
                {
                    this.ShowMsg("删除失败", false);
                }
            }
        }
    }
}

