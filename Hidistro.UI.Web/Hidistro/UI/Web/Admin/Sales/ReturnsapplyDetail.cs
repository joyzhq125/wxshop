namespace Hidistro.UI.Web.Admin.Sales
{
    using Hidistro.ControlPanel.Sales;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Orders;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Data;
    using System.Web.UI.WebControls;

    public class ReturnsapplyDetail : AdminPage
    {
        protected Button btnSubmit;
        protected Literal litAccount;
        protected Literal litAdminRemark;
        protected Literal litApplyForTime;
        protected Literal litAuditTime;
        protected Literal litComments;
        protected Literal litHandleStatus;
        protected Literal litHandleTime;
        protected Literal litOperator;
        protected Literal litOrderid;
        protected Literal litProductName;
        protected Literal litRefundMoney;
        protected Literal litRefundTime;
        protected Literal litUserName;
        private int ReturnsId;

        protected ReturnsapplyDetail() : base("", "")
        {
        }

        private void BindRefund()
        {
            ReturnsApplyQuery returnsapplyquery = new ReturnsApplyQuery {
                PageIndex = 1,
                PageSize = 1,
                ReturnsId = base.Request.QueryString["ReturnsId"]
            };
            DbQueryResult returnOrderAll = RefundHelper.GetReturnOrderAll(returnsapplyquery);
            if (returnOrderAll.Data != null)
            {
                DataTable data = (DataTable) returnOrderAll.Data;
                this.litOrderid.Text = data.Rows[0]["OrderId"].ToString();
                this.litUserName.Text = data.Rows[0]["Username"].ToString();
                this.litRefundMoney.Text = data.Rows[0]["RefundMoney"].ToString();
                this.litApplyForTime.Text = data.Rows[0]["ApplyForTime"].ToString();
                this.litComments.Text = data.Rows[0]["Comments"].ToString();
                this.litHandleStatus.Text = this.GetStatus(int.Parse(data.Rows[0]["HandleStatus"].ToString()));
                this.litHandleTime.Text = data.Rows[0]["HandleTime"].ToString();
                this.litAdminRemark.Text = data.Rows[0]["AdminRemark"].ToString();
                this.litAccount.Text = data.Rows[0]["Account"].ToString();
                this.litProductName.Text = data.Rows[0]["ProductName"].ToString();
                this.litAuditTime.Text = data.Rows[0]["AuditTime"].ToString();
                this.litRefundTime.Text = data.Rows[0]["RefundTime"].ToString();
                this.litOperator.Text = data.Rows[0]["OperatorName"].ToString();
            }
            else
            {
                this.ShowMsg("退款单信息不存在！", false);
            }
        }

        private void btn_Submint(object sender, EventArgs e)
        {
            this.Page.Response.Redirect("Default.aspx");
        }

        public string GetStatus(int status)
        {
            string str = null;
            switch (status)
            {
                case 2:
                    return "已退款";

                case 3:
                    return str;

                case 4:
                    return "未审核";

                case 5:
                    return "已审核";

                case 6:
                    return "未退款";

                case 7:
                    return "审核未通过";

                case 8:
                    return "拒绝退款";
            }
            return str;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.btnSubmit.Click += new EventHandler(this.btn_Submint);
            if (!base.IsPostBack)
            {
                if (int.TryParse(this.Page.Request.QueryString["ReturnsId"], out this.ReturnsId))
                {
                    this.BindRefund();
                }
                else
                {
                    this.Page.Response.Redirect("Default.aspx");
                }
            }
        }
    }
}

