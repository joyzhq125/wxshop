namespace Hidistro.UI.Web.Admin
{
    using ASPNET.WebControls;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities.VShop;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Web.UI.WebControls;

    public class ManageLotteryActivity : AdminPage
    {
        protected Pager pager;
        protected Repeater rpMaterial;

        protected ManageLotteryActivity() : base("", "")
        {
        }

        protected void BindMaterial()
        {
            LotteryActivityQuery page = new LotteryActivityQuery {
                PageIndex = this.pager.PageIndex,
                PageSize = this.pager.PageSize,
                SortBy = "ActivityId",
                SortOrder = SortAction.Desc
            };
            DbQueryResult lotteryTicketList = VShopHelper.GetLotteryTicketList(page);
            this.rpMaterial.DataSource = lotteryTicketList.Data;
            this.rpMaterial.DataBind();
            this.pager.TotalRecords = lotteryTicketList.TotalRecords;
        }

        public string GetUrl(object activityId)
        {
            return string.Concat(new object[] { "http://", Globals.DomainName, Globals.ApplicationPath, "/Vshop/SignUp.aspx?id=", activityId });
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                this.BindMaterial();
            }
        }

        protected void rpMaterial_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "del")
            {
                if (VShopHelper.DelteLotteryTicket(Convert.ToInt32(e.CommandArgument)))
                {
                    this.ShowMsg("删除成功", true);
                    this.BindMaterial();
                }
                else
                {
                    this.ShowMsg("删除失败", false);
                }
            }
            else if (e.CommandName == "start")
            {
                LotteryTicketInfo lotteryTicket = VShopHelper.GetLotteryTicket(Convert.ToInt32(e.CommandArgument));
                if (lotteryTicket.OpenTime > DateTime.Now)
                {
                    lotteryTicket.OpenTime = DateTime.Now;
                    VShopHelper.UpdateLotteryTicket(lotteryTicket);
                    this.BindMaterial();
                }
            }
        }
    }
}

