namespace Hidistro.UI.Web.Admin
{
    using ASPNET.WebControls;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core.Entities;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Globalization;
    using System.Web.UI.WebControls;

    public class AlarmNotify : AdminPage
    {
        protected Button btnSearch;
        protected DataList dlstPtReviews;
        protected ProductCategoriesDropDownList dropCategories;
        protected PageSize hrefPageSize;
        protected Pager pager;
        protected Pager pager1;
        protected TextBox txtSearchText;
        protected TextBox txtSKU;

        protected AlarmNotify() : base("", "")
        {
        }

        private void BindPtReview()
        {
            DbQueryResult alarms = VShopHelper.GetAlarms(this.pager.PageIndex, this.pager.PageSize);
            this.dlstPtReviews.DataSource = alarms.Data;
            this.dlstPtReviews.DataBind();
            this.pager.TotalRecords = alarms.TotalRecords;
            this.pager1.TotalRecords = alarms.TotalRecords;
        }

        private void dlstPtReviews_DeleteCommand(object source, DataListCommandEventArgs e)
        {
            if (VShopHelper.DeleteAlarm(Convert.ToInt32(e.CommandArgument, CultureInfo.InvariantCulture)))
            {
                this.ShowMsg("删除成功", true);
                this.BindPtReview();
            }
            else
            {
                this.ShowMsg("删除失败", false);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.dlstPtReviews.DeleteCommand += new DataListCommandEventHandler(this.dlstPtReviews_DeleteCommand);
            if (!base.IsPostBack)
            {
                this.BindPtReview();
            }
        }
    }
}

