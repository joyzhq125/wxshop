namespace Hidistro.UI.Web.Admin.Fenxiao
{
    using ASPNET.WebControls;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities.Members;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.ControlPanel.Utility;
    using Hidistro.UI.Web.Admin.Ascx;
    using System;
    using System.Collections.Specialized;
    using System.Drawing;
    using System.Globalization;
    using System.Web.UI.WebControls;

    public class CommissionsList : AdminPage
    {
        protected Button btnQueryLogs;
        protected Button Button1;
        protected Button Button4;
        protected ucDateTimePicker calendarEndDate;
        protected ucDateTimePicker calendarStartDate;
        protected string CurrentStoreName;
        protected decimal CurrentTotal;
        private string EndTime;
        public int lastDay;
        protected Pager pager;
        protected Repeater reCommissions;
        private string StartTime;
        private int userid;

        protected CommissionsList() : base("m05", "fxp03")
        {
            this.StartTime = "";
            this.EndTime = "";
            this.CurrentStoreName = "";
        }

        private void BindData()
        {
            DateTime time;
            CommissionsQuery entity = new CommissionsQuery {
                UserId = int.Parse(this.Page.Request.QueryString["UserId"]),
                EndTime = this.EndTime,
                StartTime = this.StartTime,
                PageIndex = this.pager.PageIndex,
                PageSize = this.pager.PageSize,
                SortOrder = SortAction.Desc,
                SortBy = "CommId"
            };
            Globals.EntityCoding(entity, true);
            DbQueryResult commissionsWithStoreName = VShopHelper.GetCommissionsWithStoreName(entity);
            this.reCommissions.DataSource = commissionsWithStoreName.Data;
            this.reCommissions.DataBind();
            this.pager.TotalRecords = commissionsWithStoreName.TotalRecords;
            if (!DateTime.TryParse(this.StartTime, out time))
            {
                time = DateTime.Parse("2015-01-01");
            }
            this.CurrentTotal = DistributorsBrower.GetUserCommissions(entity.UserId, time, this.EndTime, null, null);
            DistributorsInfo userIdDistributors = DistributorsBrower.GetUserIdDistributors(entity.UserId);
            this.CurrentStoreName = userIdDistributors.StoreName;
        }

        protected void btnQueryLogs_Click(object sender, EventArgs e)
        {
            if (this.calendarEndDate.SelectedDate.HasValue)
            {
                this.EndTime = this.calendarEndDate.SelectedDate.Value.ToString("yyyy-MM-dd");
            }
            if (this.calendarStartDate.SelectedDate.HasValue)
            {
                this.StartTime = this.calendarStartDate.SelectedDate.Value.ToString("yyyy-MM-dd");
            }
            this.lastDay = 0;
            this.ReBind(true);
        }

        protected void Button1_Click1(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
            this.EndTime = now.ToString("yyyy-MM-dd");
            this.StartTime = now.AddDays(-6.0).ToString("yyyy-MM-dd");
            this.lastDay = 7;
            this.ReBind(true);
        }

        protected void Button4_Click1(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
            this.EndTime = now.ToString("yyyy-MM-dd");
            this.StartTime = now.AddDays(-29.0).ToString("yyyy-MM-dd");
            this.lastDay = 30;
            this.ReBind(true);
        }

        protected string getNextName(string StoreName, string uid, string rid, string rpath)
        {
            string str = "店铺销售";
            if ((uid == rid) || string.IsNullOrEmpty(rpath))
            {
                return "店铺销售";
            }
            if (uid == rpath)
            {
                return (StoreName + "（下一级）");
            }
            if (rpath.Contains("|"))
            {
                string[] strArray = rpath.Split(new char[] { '|' });
                if (strArray[0] == uid)
                {
                    str = StoreName + "（下二级）";
                }
                if (strArray[1] == uid)
                {
                    str = StoreName + "（下一级）";
                }
                return str;
            }
            return rpath;
        }

        private void LoadParameters()
        {
            if (!this.Page.IsPostBack)
            {
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["StartTime"]))
                {
                    this.StartTime = base.Server.UrlDecode(this.Page.Request.QueryString["StartTime"]);
                    this.calendarStartDate.SelectedDate = new DateTime?(DateTime.Parse(this.StartTime));
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["EndTime"]))
                {
                    this.EndTime = base.Server.UrlDecode(this.Page.Request.QueryString["EndTime"]);
                    this.calendarEndDate.SelectedDate = new DateTime?(DateTime.Parse(this.EndTime));
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["lastDay"]))
                {
                    int.TryParse(this.Page.Request.QueryString["lastDay"], out this.lastDay);
                    if (this.lastDay == 30)
                    {
                        this.Button1.BorderColor = ColorTranslator.FromHtml("");
                        this.Button4.BorderColor = ColorTranslator.FromHtml("#FF00CC");
                    }
                    else if (this.lastDay == 7)
                    {
                        this.Button1.BorderColor = ColorTranslator.FromHtml("#FF00CC");
                        this.Button4.BorderColor = ColorTranslator.FromHtml("");
                    }
                    else
                    {
                        this.Button1.BorderColor = ColorTranslator.FromHtml("");
                        this.Button4.BorderColor = ColorTranslator.FromHtml("");
                    }
                }
            }
            else
            {
                if (this.calendarStartDate.SelectedDate.HasValue)
                {
                    this.StartTime = this.calendarStartDate.SelectedDate.Value.ToString("yyyy-MM-dd");
                }
                if (this.calendarEndDate.SelectedDate.HasValue)
                {
                    this.EndTime = this.calendarEndDate.SelectedDate.Value.ToString("yyyy-MM-dd");
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.LoadParameters();
            if (int.TryParse(this.Page.Request.QueryString["UserId"], out this.userid))
            {
                this.BindData();
            }
            else
            {
                this.Page.Response.Redirect("DistributorList.aspx");
            }
        }

        private void ReBind(bool isSearch)
        {
            NameValueCollection queryStrings = new NameValueCollection();
            queryStrings.Add("UserId", this.Page.Request.QueryString["UserId"]);
            queryStrings.Add("StartTime", this.StartTime);
            queryStrings.Add("EndTime", this.EndTime);
            queryStrings.Add("pageSize", this.pager.PageSize.ToString(CultureInfo.InvariantCulture));
            if (!isSearch)
            {
                queryStrings.Add("pageIndex", this.pager.PageIndex.ToString(CultureInfo.InvariantCulture));
            }
            queryStrings.Add("lastDay", this.lastDay.ToString());
            base.ReloadPage(queryStrings);
        }
    }
}

