namespace Hidistro.UI.Web.Admin.vshop
{
    using ASPNET.WebControls;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities.VShop;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Collections.Specialized;
    using System.Globalization;
    using System.Web.UI.WebControls;

    public class ActivitiesList : AdminPage
    {
        private string activities;
        private string ActivitiesName;
        protected Button btnSearchButton;
        protected HyperLink hlinkActivitesing;
        protected HyperLink hlinkAllActivities;
        protected HyperLink hlinkNotStartActivites;
        protected HyperLink hlinkOverActivites;
        protected Label lblStatus;
        protected Pager pager;
        protected Repeater reActivities;
        protected TextBox txtActivitiesName;

        protected ActivitiesList() : base("", "")
        {
            this.ActivitiesName = "";
            this.activities = "";
        }

        private void BindData()
        {
            ActivitiesQuery entity = new ActivitiesQuery {
                ActivitiesName = this.ActivitiesName,
                PageIndex = this.pager.PageIndex,
                PageSize = this.pager.PageSize,
                SortOrder = SortAction.Desc,
                SortBy = "ActivitiesId",
                State = this.activities
            };
            this.lblStatus.Text = this.activities;
            Globals.EntityCoding(entity, true);
            DbQueryResult activitiesList = VShopHelper.GetActivitiesList(entity);
            this.reActivities.DataSource = activitiesList.Data;
            this.reActivities.DataBind();
            this.pager.TotalRecords = activitiesList.TotalRecords;
        }

        private void btnSearchButton_Click(object sender, EventArgs e)
        {
            this.ReBind(true);
        }

        private void LoadParameters()
        {
            if (!this.Page.IsPostBack)
            {
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["ActivitiesName"]))
                {
                    this.ActivitiesName = base.Server.UrlDecode(this.Page.Request.QueryString["ActivitiesName"]);
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["activities"]))
                {
                    this.activities = base.Server.UrlDecode(this.Page.Request.QueryString["activities"]);
                    this.lblStatus.Text = this.activities;
                }
                this.txtActivitiesName.Text = this.ActivitiesName;
            }
            else
            {
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["activities"]))
                {
                    this.activities = base.Server.UrlDecode(this.Page.Request.QueryString["activities"]);
                    this.lblStatus.Text = this.activities;
                }
                this.ActivitiesName = this.txtActivitiesName.Text;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.btnSearchButton.Click += new EventHandler(this.btnSearchButton_Click);
            this.reActivities.ItemCommand += new RepeaterCommandEventHandler(this.reActivities_ItemCommand);
            this.LoadParameters();
            if (!base.IsPostBack)
            {
                this.BindData();
            }
        }

        private void reActivities_ItemCommand(object sender, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
                if (VShopHelper.DeleteActivities(int.Parse(e.CommandArgument.ToString())))
                {
                    this.BindData();
                    this.ShowMsg("删除成功", true);
                }
                else
                {
                    this.ShowMsg("删除失败", false);
                }
            }
        }

        private void ReBind(bool isSearch)
        {
            NameValueCollection queryStrings = new NameValueCollection();
            queryStrings.Add("ActivitiesName", this.txtActivitiesName.Text);
            queryStrings.Add("activities", this.activities);
            this.lblStatus.Text = this.activities;
            queryStrings.Add("pageSize", this.pager.PageSize.ToString(CultureInfo.InvariantCulture));
            if (!isSearch)
            {
                queryStrings.Add("pageIndex", this.pager.PageIndex.ToString(CultureInfo.InvariantCulture));
            }
            base.ReloadPage(queryStrings);
        }
    }
}

