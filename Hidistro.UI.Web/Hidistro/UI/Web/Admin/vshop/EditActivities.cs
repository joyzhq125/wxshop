namespace Hidistro.UI.Web.Admin.vshop
{
    using ASPNET.WebControls;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Entities.VShop;
    using Hidistro.UI.ControlPanel.Utility;
    using kindeditor.Net;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Web.UI.WebControls;

    public class EditActivities : AdminPage
    {
        private int activitiesid;
        protected Button btnEditActivity;
        protected ProductCategoriesDropDownList dropCategories;
        protected KindeditorControl txtDescription;
        protected WebCalendar txtEndDate;
        protected TextBox txtMeetMoney;
        protected TextBox txtName;
        protected TextBox txtReductionMoney;
        protected WebCalendar txtStartDate;

        protected EditActivities() : base("", "")
        {
        }

        private void Bind(int activitiesid)
        {
            IList<ActivitiesInfo> activitiesInfo = VShopHelper.GetActivitiesInfo(activitiesid.ToString());
            if (activitiesInfo.Count > 0)
            {
                this.txtName.Text = activitiesInfo[0].ActivitiesName;
                this.txtDescription.Text = activitiesInfo[0].ActivitiesDescription;
                this.txtEndDate.Text = activitiesInfo[0].EndTIme.ToString("yyyy-MM-dd");
                this.txtStartDate.Text = activitiesInfo[0].StartTime.ToString("yyyy-MM-dd");
                this.txtMeetMoney.Text = activitiesInfo[0].MeetMoney.ToString("0.00");
                this.txtReductionMoney.Text = activitiesInfo[0].ReductionMoney.ToString("0.00");
                this.dropCategories.SelectedValue = new int?(activitiesInfo[0].ActivitiesType);
            }
        }

        private void btnEditActivity_Click(object sender, EventArgs e)
        {
            decimal result = 0M;
            decimal num2 = 0M;
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
            else if (this.txtReductionMoney.Text.Trim() == "")
            {
                this.ShowMsg("减免金额请输入整数！", false);
            }
            else if (!decimal.TryParse(this.txtReductionMoney.Text.Trim(), out result))
            {
                this.ShowMsg("减免金额请输入整数", false);
            }
            else if (this.txtMeetMoney.Text.Trim() == "")
            {
                this.ShowMsg("满足金额请输入整数！", false);
            }
            else if (!decimal.TryParse(this.txtMeetMoney.Text.Trim(), out num2))
            {
                this.ShowMsg("满足金额请输入整数", false);
            }
            else if (decimal.Parse(this.txtReductionMoney.Text.Trim()) >= decimal.Parse(this.txtMeetMoney.Text.Trim()))
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
                    activity.Type = 1;
                    activity.ActivitiesType = 0;
                }
                else
                {
                    activity.Type = 0;
                    activity.ActivitiesType = int.Parse(this.dropCategories.SelectedValue.ToString());
                }
                activity.ActivitiesId = this.activitiesid;
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
                else if (VShopHelper.UpdateActivities(activity))
                {
                    this.ShowMsg("修改成功", true);
                }
                else
                {
                    this.ShowMsg("修改失败", false);
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.activitiesid = base.GetUrlIntParam("activitiesid");
            this.btnEditActivity.Click += new EventHandler(this.btnEditActivity_Click);
            if (!this.Page.IsPostBack)
            {
                if (this.activitiesid == 0)
                {
                    this.Page.Response.Redirect("ActivitiesList.aspx");
                }
                else
                {
                    this.dropCategories.IsTopCategory = true;
                    this.dropCategories.IsUnclassified = false;
                    this.dropCategories.DataBind();
                    this.Bind(this.activitiesid);
                }
            }
        }
    }
}

