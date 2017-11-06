namespace Hidistro.UI.Web.Admin
{
    using ASPNET.WebControls;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Entities.VShop;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Web.UI.WebControls;

    public class EditActivity : AdminPage
    {
        protected Button btnEditActivity;
        protected TextBox txtCloseRemark;
        protected TextBox txtDescription;
        protected WebCalendar txtEndDate;
        protected TextBox txtItem1;
        protected TextBox txtItem2;
        protected TextBox txtItem3;
        protected TextBox txtItem4;
        protected TextBox txtItem5;
        protected TextBox txtKeys;
        protected TextBox txtMaxValue;
        protected TextBox txtName;
        protected WebCalendar txtStartDate;
        protected UpImg uploader1;
        protected string wid;
        protected EditActivity() : base("", "")
        {
        }

        private void btnEditActivity_Click(object sender, EventArgs e)
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
            else if (this.txtStartDate.SelectedDate.Value.CompareTo(this.txtEndDate.SelectedDate.Value) >= 0)
            {
                this.ShowMsg("开始日期不能晚于结束日期！", false);
            }
            else if ((this.txtMaxValue.Text != "") && !int.TryParse(this.txtMaxValue.Text, out result))
            {
                this.ShowMsg("人数上限格式错误！", false);
            }
            else
            {
                ActivityInfo activity = VShopHelper.GetActivity(base.GetUrlIntParam("id"));
                if ((activity.Keys != this.txtKeys.Text.Trim()) && ReplyHelper.HasReplyKey(this.txtKeys.Text.Trim(),this.wid))
                {
                    this.ShowMsg("关键字重复!", false);
                }
                else
                {
                    activity.CloseRemark = this.txtCloseRemark.Text.Trim();
                    activity.Description = this.txtDescription.Text.Trim();
                    activity.EndDate = this.txtEndDate.SelectedDate.Value.AddMinutes(59.0).AddSeconds(59.0);
                    activity.Item1 = this.txtItem1.Text.Trim();
                    activity.Item2 = this.txtItem2.Text.Trim();
                    activity.Item3 = this.txtItem3.Text.Trim();
                    activity.Item4 = this.txtItem4.Text.Trim();
                    activity.Item5 = this.txtItem5.Text.Trim();
                    activity.Keys = this.txtKeys.Text.Trim();
                    activity.MaxValue = result;
                    activity.Name = this.txtName.Text.Trim();
                    activity.PicUrl = this.uploader1.UploadedImageUrl;
                    activity.StartDate = this.txtStartDate.SelectedDate.Value;
                    if (VShopHelper.UpdateActivity(activity))
                    {
                        base.Response.Redirect("ManageActivity.aspx");
                    }
                    else
                    {
                        this.ShowMsg("更新失败", false);
                    }
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            wid = GetCurWebId();
            if (string.IsNullOrEmpty(wid)) return;
            this.btnEditActivity.Click += new EventHandler(this.btnEditActivity_Click);
            if (!this.Page.IsPostBack)
            {
                ActivityInfo activity = VShopHelper.GetActivity(base.GetUrlIntParam("id"));
                this.txtCloseRemark.Text = activity.CloseRemark;
                this.txtDescription.Text = activity.Description;
                this.txtEndDate.SelectedDate = new DateTime?(activity.EndDate);
                this.txtKeys.Text = activity.Keys;
                if (activity.MaxValue != 0)
                {
                    this.txtMaxValue.Text = activity.MaxValue.ToString();
                }
                this.txtName.Text = activity.Name;
                this.txtStartDate.SelectedDate = new DateTime?(activity.StartDate);
                this.uploader1.UploadedImageUrl = activity.PicUrl;
                this.txtItem1.Text = activity.Item1;
                this.txtItem2.Text = activity.Item2;
                this.txtItem3.Text = activity.Item3;
                this.txtItem4.Text = activity.Item4;
                this.txtItem5.Text = activity.Item5;
            }
        }
    }
}

