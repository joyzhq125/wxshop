namespace Hidistro.UI.Web.Admin.demo
{
    using Hidistro.ControlPanel.VShop;
    using Hidistro.Entities.StatisticsReport;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class NotifierDemo : AdminPage
    {
        protected Button Button1;
        protected Button Button2;
        protected Button Button3;
        protected HtmlForm form1;
        private UpdateStatistics myEvent;
        private StatisticNotifier myNotifier;

        protected NotifierDemo() : base("", "")
        {
            this.myNotifier = new StatisticNotifier();
            this.myEvent = new UpdateStatistics();
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            this.myNotifier.updateAction = UpdateAction.OrderUpdate;
            this.myNotifier.actionDesc = "订单更新";
            this.myNotifier.RecDateUpdate = DateTime.Today;
            this.myNotifier.DataUpdated += new StatisticNotifier.DataUpdatedEventHandler(this.myEvent.Update);
            this.myNotifier.UpdateDB();
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            this.myNotifier.updateAction = UpdateAction.MemberUpdate;
            this.myNotifier.actionDesc = "会员更新";
            this.myNotifier.RecDateUpdate = DateTime.Today;
            this.myNotifier.DataUpdated += new StatisticNotifier.DataUpdatedEventHandler(this.myEvent.Update);
            this.myNotifier.UpdateDB();
        }

        protected void Button3_Click(object sender, EventArgs e)
        {
            this.myNotifier.updateAction = UpdateAction.ProductUpdate;
            this.myNotifier.actionDesc = "产品更新";
            this.myNotifier.RecDateUpdate = DateTime.Today;
            this.myNotifier.DataUpdated += new StatisticNotifier.DataUpdatedEventHandler(this.myEvent.Update);
            this.myNotifier.UpdateDB();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }
    }
}

