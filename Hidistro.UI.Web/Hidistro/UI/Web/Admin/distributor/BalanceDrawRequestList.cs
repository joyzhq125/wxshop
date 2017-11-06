namespace Hidistro.UI.Web.Admin.distributor
{
    using ASPNET.WebControls;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities.Orders;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Collections.Specialized;
    using System.Globalization;
    using System.Web.UI.WebControls;

    public class BalanceDrawRequestList : AdminPage
    {
        protected Button btnSearchButton;
        private string CheckTime;
        protected Pager pager;
        protected Repeater reBalanceDrawRequest;
        private string RequestTime;
        private string StoreName;
        protected WebCalendar txtCheckTime;
        protected WebCalendar txtRequestTime;
        protected TextBox txtStoreName;

        protected BalanceDrawRequestList() : base("", "")
        {
            this.RequestTime = "";
            this.StoreName = "";
            this.CheckTime = "";
        }

        private void BindData()
        {
            BalanceDrawRequestQuery entity = new BalanceDrawRequestQuery {
                CheckTime = this.CheckTime,
                RequestTime = this.RequestTime,
                StoreName = this.StoreName,
                PageIndex = this.pager.PageIndex,
                PageSize = this.pager.PageSize,
                SortOrder = SortAction.Desc,
                SortBy = "RequestTime",
                RequestEndTime = "",
                RequestStartTime = "",
                IsCheck = ""
            };
            Globals.EntityCoding(entity, true);
            DbQueryResult balanceDrawRequest = VShopHelper.GetBalanceDrawRequest(entity);
            this.reBalanceDrawRequest.DataSource = balanceDrawRequest.Data;
            this.reBalanceDrawRequest.DataBind();
            this.pager.TotalRecords = balanceDrawRequest.TotalRecords;
        }

        private void btnSearchButton_Click(object sender, EventArgs e)
        {
            this.ReBind(true);
        }

        private void LoadParameters()
        {
            if (!this.Page.IsPostBack)
            {
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["StoreName"]))
                {
                    this.StoreName = base.Server.UrlDecode(this.Page.Request.QueryString["StoreName"]);
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["CheckTime"]))
                {
                    this.CheckTime = base.Server.UrlDecode(this.Page.Request.QueryString["CheckTime"]);
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["RequestTime"]))
                {
                    this.RequestTime = base.Server.UrlDecode(this.Page.Request.QueryString["RequestTime"]);
                }
                this.txtStoreName.Text = this.StoreName;
                this.txtRequestTime.Text = this.RequestTime;
                this.txtCheckTime.Text = this.CheckTime;
            }
            else
            {
                this.StoreName = this.txtStoreName.Text;
                this.RequestTime = this.txtRequestTime.Text;
                this.CheckTime = this.txtCheckTime.Text;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.btnSearchButton.Click += new EventHandler(this.btnSearchButton_Click);
            this.LoadParameters();
            if (!base.IsPostBack)
            {
                this.BindData();
            }
        }

        private void ReBind(bool isSearch)
        {
            NameValueCollection queryStrings = new NameValueCollection();
            queryStrings.Add("StoreName", this.txtStoreName.Text);
            queryStrings.Add("RequestTime", this.txtRequestTime.Text);
            queryStrings.Add("CheckTime", this.txtCheckTime.Text);
            queryStrings.Add("pageSize", this.pager.PageSize.ToString(CultureInfo.InvariantCulture));
            if (!isSearch)
            {
                queryStrings.Add("pageIndex", this.pager.PageIndex.ToString(CultureInfo.InvariantCulture));
            }
            base.ReloadPage(queryStrings);
        }
    }
}

