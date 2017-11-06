namespace Hidistro.UI.Web.Admin
{
    using ASPNET.WebControls;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Entities.VShop;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Web.UI.WebControls;

    public class AddActivity : AdminPage
    {
        protected Button btnAddActivity;
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
        protected AddActivity() : base("", "")
        {
        }

        private void btnAddActivity_Click(object sender, EventArgs e)
        {
            if (ReplyHelper.HasReplyKey(this.txtKeys.Text.Trim(),this.wid))
            {
                this.ShowMsg("关键字重复!", false);
            }
            else
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
                else
                {
                    string uploadedImageUrl = this.uploader1.UploadedImageUrl;
                    if (string.IsNullOrEmpty(uploadedImageUrl))
                    {
                        this.ShowMsg("请上传封面图片", false);
                    }
                    else if ((this.txtMaxValue.Text != "") && !int.TryParse(this.txtMaxValue.Text, out result))
                    {
                        this.ShowMsg("人数上限格式错误！", false);
                    }
                    else
                    {
                        ActivityInfo activity = new ActivityInfo {
                            CloseRemark = this.txtCloseRemark.Text.Trim(),
                            Description = this.txtDescription.Text.Trim(),
                            EndDate = this.txtEndDate.SelectedDate.Value.AddMinutes(59.0).AddSeconds(59.0),
                            Item1 = this.txtItem1.Text.Trim(),
                            Item2 = this.txtItem2.Text.Trim(),
                            Item3 = this.txtItem3.Text.Trim(),
                            Item4 = this.txtItem4.Text.Trim(),
                            Item5 = this.txtItem5.Text.Trim(),
                            Keys = this.txtKeys.Text.Trim(),
                            MaxValue = result,
                            Name = this.txtName.Text.Trim(),
                            PicUrl = uploadedImageUrl,
                            StartDate = this.txtStartDate.SelectedDate.Value
                        };
                        if (VShopHelper.SaveActivity(activity))
                        {
                            base.Response.Redirect("ManageActivity.aspx");
                        }
                        else
                        {
                            this.ShowMsg("添加失败", false);
                        }
                    }
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            wid = GetCurWebId();
            if (string.IsNullOrEmpty(wid)) return;
            this.btnAddActivity.Click += new EventHandler(this.btnAddActivity_Click);
        }
    }
}

