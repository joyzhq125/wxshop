namespace Hidistro.UI.Web.Admin.distributor
{
    using ASPNET.WebControls;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities.Orders;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Collections.Specialized;
    using System.Data;
    using System.Globalization;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class BalanceDrawApplyList : AdminPage
    {
        protected Button btapply;
        protected Button btnSearchButton;
        protected HtmlInputHidden hdapplyid;
        protected HtmlInputHidden hdredpackrecordnum;
        protected HtmlInputHidden hdreferralblance;
        protected HtmlInputHidden hduserid;
        protected Pager pager;
        protected Repeater reBalanceDrawRequest;
        private string RequestEndTime;
        private string RequestStartTime;
        private string StoreName;
        protected HtmlTextArea txtcontent;
        protected WebCalendar txtRequestEndTime;
        protected WebCalendar txtRequestStartTime;
        protected TextBox txtStoreName;

        protected BalanceDrawApplyList() : base("", "")
        {
            this.RequestStartTime = "";
            this.RequestEndTime = "";
            this.StoreName = "";
        }

        private void BindData()
        {
            BalanceDrawRequestQuery entity = new BalanceDrawRequestQuery {
                RequestTime = "",
                CheckTime = "",
                StoreName = this.StoreName,
                PageIndex = this.pager.PageIndex,
                PageSize = this.pager.PageSize,
                SortOrder = SortAction.Desc,
                SortBy = "SerialID",
                RequestEndTime = this.RequestEndTime,
                RequestStartTime = this.RequestStartTime,
                IsCheck = "0",
                UserId = ""
            };
            Globals.EntityCoding(entity, true);
            DbQueryResult balanceDrawRequest = DistributorsBrower.GetBalanceDrawRequest(entity, null);
            this.reBalanceDrawRequest.DataSource = balanceDrawRequest.Data;
            this.reBalanceDrawRequest.DataBind();
            this.pager.TotalRecords = balanceDrawRequest.TotalRecords;
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            int id = int.Parse(this.hdapplyid.Value);
            string remark = this.txtcontent.Value;
            int userId = int.Parse(this.hduserid.Value);
            decimal num3 = decimal.Parse(this.hdreferralblance.Value);
            int num4 = int.Parse("0" + this.hdredpackrecordnum.Value);
            if (VShopHelper.UpdateBalanceDrawRequest(id, remark))
            {
                decimal referralRequestBalance = num3;
                if (num4 > 0)
                {
                    int redPackTotalAmount = DistributorsBrower.GetRedPackTotalAmount(id, 0);
                    referralRequestBalance -= decimal.Parse(redPackTotalAmount.ToString()) / 100M;
                }
                if (VShopHelper.UpdateBalanceDistributors(userId, referralRequestBalance))
                {
                    this.ShowMsg("结算成功", true);
                    this.BindData();
                }
                else
                {
                    this.ShowMsg("结算失败", false);
                }
            }
            else
            {
                this.ShowMsg("结算失败", false);
            }
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
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["RequestEndTime"]))
                {
                    this.RequestEndTime = base.Server.UrlDecode(this.Page.Request.QueryString["RequestEndTime"]);
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["RequestStartTime"]))
                {
                    this.RequestStartTime = base.Server.UrlDecode(this.Page.Request.QueryString["RequestStartTime"]);
                }
                this.txtStoreName.Text = this.StoreName;
                this.txtRequestStartTime.Text = this.RequestStartTime;
                this.txtRequestEndTime.Text = this.RequestEndTime;
            }
            else
            {
                this.StoreName = this.txtStoreName.Text;
                this.RequestStartTime = this.txtRequestStartTime.Text;
                this.RequestEndTime = this.txtRequestEndTime.Text;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            wid = GetCurWebId();
            if (string.IsNullOrEmpty(wid)) return;
            this.btapply.Click += new EventHandler(this.btnApply_Click);
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
            queryStrings.Add("RequestStartTime", this.txtRequestStartTime.Text);
            queryStrings.Add("RequestEndTime", this.txtRequestEndTime.Text);
            queryStrings.Add("pageSize", this.pager.PageSize.ToString(CultureInfo.InvariantCulture));
            if (!isSearch)
            {
                queryStrings.Add("pageIndex", this.pager.PageIndex.ToString(CultureInfo.InvariantCulture));
            }
            base.ReloadPage(queryStrings);
        }

        protected void rptList_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            string str;
            string str2;
            int result = 0;
            int.TryParse(e.CommandArgument.ToString(), out result);
            if (((result > 0) && ((str2 = e.CommandName) != null)) && (str2 == "sendredpack"))
            {
                str = DistributorsBrower.SendRedPackToBalanceDrawRequest(result,wid);
                base.Response.Write(str);
                base.Response.End();
                string str3 = str;
                if (str3 == null)
                {
                    goto Label_00C4;
                }
                if (!(str3 == "-1"))
                {
                    if (str3 == "1")
                    {
                        base.Response.Redirect("SendRedpackRecord.aspx?serialid=" + result);
                        base.Response.End();
                        return;
                    }
                    goto Label_00C4;
                }
                base.Response.Redirect("SendRedpackRecord.aspx?serialid=" + result);
                base.Response.End();
            }
            return;
        Label_00C4:
            this.ShowMsg("生成红包失败，原因是：" + str, false);
        }

        protected void rptList_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                LinkButton button = (LinkButton) e.Item.FindControl("lkBtnSendRedPack");
                int num = int.Parse(((DataRowView) e.Item.DataItem).Row["serialid"].ToString());
                if (int.Parse(((DataRowView) e.Item.DataItem).Row["RedpackRecordNum"].ToString()) > 0)
                {
                    button.PostBackUrl = "SendRedpackRecord.aspx?serialid=" + num;
                    button.Text = "查看微信红包";
                }
                else
                {
                    int num3 = int.Parse(((DataRowView) e.Item.DataItem).Row["RequestType"].ToString());
                    button.OnClientClick = @"return confirm('提现金额将会拆分为最大金额为200元的微信红包，等待发送！\n确定生成微信红包吗？')";
                    if (num3 == 0)
                    {
                        button.Style.Add("color", "red");
                    }
                }
            }
        }
    }
}

