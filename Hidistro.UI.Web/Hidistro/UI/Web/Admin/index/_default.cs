namespace Hidistro.UI.Web.Admin.index
{
    using Hidistro.ControlPanel.Sales;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Entities.Sales;
    using Hidistro.Entities.Store;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Web.UI.WebControls;

    [PrivilegeCheck(Privilege.Summary)]
    public class _default : AdminPage
    {
        protected HiControls HiControlsId;
        protected ClassShowOnDataLitl lblOrderPriceMonth;
        protected ClassShowOnDataLitl lblOrderPriceYesterDay;
        protected ClassShowOnDataLitl lblTodayFinishOrder;
        protected ClassShowOnDataLitl lblTodayOrderAmout;
        protected ClassShowOnDataLitl lblTotalMembers;
        protected ClassShowOnDataLitl lblTotalProducts;
        protected ClassShowOnDataLitl lblUserNewAddYesterToday;
        protected ClassShowOnDataLitl lblYesterdayFinishOrder;
        protected ClassShowOnDataLitl ltrTodayAddMemberNumber;
        protected HyperLink ltrWaitSendOrdersNumber;
        protected HyperLink WaitForRefund;

        public _default() : base("m01", "dpp01")
        {
        }

        private void BindStatistics(StatisticsInfo statistics)
        {
            ManagerHelper.GetCurrentManager();
            if (statistics.OrderNumbWaitConsignment > 0)
            {
                this.ltrWaitSendOrdersNumber.NavigateUrl = "javascript:ShowSecondMenuLeft('微订单','sales/manageorder.aspx','" + Globals.ApplicationPath + "/Admin/sales/ManageOrder.aspx?orderStatus=2')";
                this.ltrWaitSendOrdersNumber.Text = statistics.OrderNumbWaitConsignment.ToString() + "条";
            }
            else
            {
                this.ltrWaitSendOrdersNumber.Text = "<font style=\"color:#2d2d2d\">0条</font>";
            }
            if (statistics.GroupBuyNumWaitRefund > 0)
            {
                this.WaitForRefund.NavigateUrl = "javascript:ShowSecondMenuLeft('微营销','promotion/groupbuys.aspx','" + Globals.ApplicationPath + "/Admin/promotion/groupbuys.aspx?state=5')";
                this.WaitForRefund.Text = statistics.GroupBuyNumWaitRefund.ToString() + "个";
            }
            else
            {
                this.WaitForRefund.Text = "<font style=\"color:#2d2d2d\">0个</font>";
            }
            this.lblTodayOrderAmout.Text = (statistics.OrderProfitToday > 0M) ? ("￥" + Globals.FormatMoney(statistics.OrderProfitToday)) : string.Empty;
            this.ltrTodayAddMemberNumber.Text = (statistics.UserNewAddToday > 0) ? statistics.UserNewAddToday.ToString() : string.Empty;
            this.lblTodayFinishOrder.Text = (statistics.TodayFinishOrder > 0) ? statistics.TodayFinishOrder.ToString() : string.Empty;
            this.lblYesterdayFinishOrder.Text = (statistics.YesterdayFinishOrder > 0) ? statistics.YesterdayFinishOrder.ToString() : string.Empty;
            this.lblOrderPriceYesterDay.Text = (statistics.OrderPriceYesterday > 0M) ? ("￥" + statistics.OrderPriceYesterday.ToString("F2")) : string.Empty;
            this.lblUserNewAddYesterToday.Text = (statistics.UserNewAddYesterToday > 0) ? (statistics.UserNewAddYesterToday.ToString() + "位") : string.Empty;
            this.lblTotalMembers.Text = (statistics.TotalMembers > 0) ? (statistics.TotalMembers.ToString() + "位") : string.Empty;
            this.lblTotalProducts.Text = (statistics.TotalProducts > 0) ? (statistics.TotalProducts.ToString() + "条") : string.Empty;
            this.lblOrderPriceMonth.Text = (statistics.OrderPriceMonth > 0M) ? ("￥" + statistics.OrderPriceMonth.ToString("F2")) : string.Empty;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!base.IsPostBack)
            {
                StatisticsInfo statistics = SalesHelper.GetStatistics();
                this.BindStatistics(statistics);
            }
        }
    }
}

