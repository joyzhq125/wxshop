namespace Hidistro.UI.Web.Admin.vshop
{
    using ASPNET.WebControls;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Entities.VShop;
    using Hidistro.UI.ControlPanel.Utility;
    using kindeditor.Net;
    using System;
    using System.Data;
    using System.Web.UI.WebControls;

    public class AddActivities : AdminPage
    {
        protected Button btnAddActivity;
        protected ProductCategoriesDropDownList dropCategories;
        protected KindeditorControl txtDescription;
        protected WebCalendar txtEndDate;
        protected TextBox txtMeetMoney;
        protected TextBox txtName;
        protected TextBox txtReductionMoney;
        protected WebCalendar txtStartDate;

        protected AddActivities() : base("", "")
        {
        }

        private void btnAddActivity_Click(object sender, EventArgs e)
        {
            int result = 0;
            if (!this.txtStartDate.SelectedDate.HasValue)
            {
                this.ShowMsg("请选择开始日期！", false);
            }
            else if (!this.txtEndDate.SelectedDate.HasValue)
            {
                this.ShowMsg("请选择结束日期！", false);
            }
            else if (this.txtStartDate.SelectedDate.Value.CompareTo(this.txtEndDate.SelectedDate.Value) > 0)
            {
                this.ShowMsg("开始日期不能晚于结束日期！", false);
            }
            else if ((this.txtReductionMoney.Text.Trim() == "") || !int.TryParse(this.txtReductionMoney.Text.Trim(), out result))
            {
                this.ShowMsg("减免金额请输入整数！", false);
            }
            else if ((this.txtMeetMoney.Text.Trim() == "") || !int.TryParse(this.txtMeetMoney.Text.Trim(), out result))
            {
                this.ShowMsg("满足金额请输入整数！", false);
            }
            else if (int.Parse(this.txtReductionMoney.Text.Trim()) >= int.Parse(this.txtMeetMoney.Text.Trim()))
            {
                this.ShowMsg("减免金额不能大于等于满足金额！", false);
            }
            else
            {
                ActivitiesInfo activity = new ActivitiesInfo {
                    ActivitiesName = this.txtName.Text.Trim(),
                    ActivitiesDescription = this.txtDescription.Text.Trim(),
                    StartTime = this.txtStartDate.SelectedDate.Value,
                    EndTIme = this.txtEndDate.SelectedDate.Value,
                    MeetMoney = decimal.Parse(this.txtMeetMoney.Text.Trim()),
                    ReductionMoney = decimal.Parse(this.txtReductionMoney.Text.Trim())
                };
                if ((this.dropCategories.SelectedValue.ToString() == "0") || (this.dropCategories.SelectedValue.ToString() == ""))
                {
                    activity.ActivitiesType = 0;
                    activity.Type = 1;
                }
                else
                {
                    activity.ActivitiesType = int.Parse(this.dropCategories.SelectedValue.ToString());
                    activity.Type = 0;
                }
                DataTable type = new DataTable();
                if (activity.Type == 1)
                {
                    type = VShopHelper.GetType(0);
                }
                else
                {
                    type = VShopHelper.GetType(1);
                }
                if (type.Rows.Count > 0)
                {
                    this.ShowMsg("类目和全部不能同时参加活动！", false);
                }
                else if (VShopHelper.AddActivities(activity) > 0)
                {
                    base.Response.Redirect("ActivitiesList.aspx");
                }
                else
                {
                    this.ShowMsg("添加失败", false);
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                this.dropCategories.IsTopCategory = true;
                this.dropCategories.IsUnclassified = false;
                this.dropCategories.DataBind();
            }
            this.btnAddActivity.Click += new EventHandler(this.btnAddActivity_Click);
        }
    }
}

