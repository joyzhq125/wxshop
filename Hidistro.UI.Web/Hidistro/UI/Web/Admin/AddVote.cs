namespace Hidistro.UI.Web.Admin
{
    using ASPNET.WebControls;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Entities.Promotions;
    using Hidistro.Entities.Store;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using Hishop.Components.Validation;
    using System;
    using System.Collections.Generic;
    using System.Web.UI.WebControls;

    [PrivilegeCheck(Privilege.Votes)]
    public class AddVote : AdminPage
    {
        protected Button btnAddVote;
        protected WebCalendar calendarEndDate;
        protected WebCalendar calendarStartDate;
        protected CheckBox checkIsBackup;
        protected string ReUrl;
        protected TextBox txtAddVoteName;
        protected TextBox txtKeys;
        protected TextBox txtMaxCheck;
        protected TextBox txtValues;
        protected UpImg uploader1;
        protected string wid;
        protected AddVote() : base("", "")
        {
            this.ReUrl = "votes.aspx";
        }

        private void btnAddVote_Click(object sender, EventArgs e)
        {
            if (ReplyHelper.HasReplyKey(this.txtKeys.Text.Trim(),this.wid))
            {
                this.ShowMsg("关键字重复!", false);
            }
            else if (this.calendarStartDate.SelectedDate.Value.CompareTo(this.calendarEndDate.SelectedDate.Value) >= 0)
            {
                this.ShowMsg("开始时间不能晚于结束时间！", false);
            }
            else
            {
                string uploadedImageUrl = this.uploader1.UploadedImageUrl;
                if (string.IsNullOrEmpty(uploadedImageUrl))
                {
                    this.ShowMsg("请上传封面图片", false);
                }
                else if (!this.calendarStartDate.SelectedDate.HasValue)
                {
                    this.ShowMsg("请选择活动开始时间", false);
                }
                else if (!this.calendarEndDate.SelectedDate.HasValue)
                {
                    this.ShowMsg("请选择活动结束时间", false);
                }
                else
                {
                    VoteInfo vote = new VoteInfo {
                        VoteName = Globals.HtmlEncode(this.txtAddVoteName.Text.Trim()),
                        IsBackup = this.checkIsBackup.Checked
                    };
                    int result = 1;
                    if (int.TryParse(this.txtMaxCheck.Text.Trim(), out result))
                    {
                        vote.MaxCheck = result;
                    }
                    vote.ImageUrl = uploadedImageUrl;
                    vote.StartDate = this.calendarStartDate.SelectedDate.Value;
                    vote.EndDate = this.calendarEndDate.SelectedDate.Value;
                    IList<VoteItemInfo> list = null;
                    if (!string.IsNullOrEmpty(this.txtValues.Text.Trim()))
                    {
                        list = new List<VoteItemInfo>();
                        string[] strArray = this.txtValues.Text.Trim().Replace("\r\n", "\n").Replace("\n", "*").Split(new char[] { '*' });
                        for (int i = 0; i < strArray.Length; i++)
                        {
                            VoteItemInfo item = new VoteItemInfo();
                            if (strArray[i].Length > 60)
                            {
                                this.ShowMsg("投票选项长度限制在60个字符以内", false);
                                return;
                            }
                            item.VoteItemName = Globals.HtmlEncode(strArray[i]);
                            list.Add(item);
                        }
                    }
                    else
                    {
                        this.ShowMsg("投票选项不能为空", false);
                        return;
                    }
                    vote.VoteItems = list;
                    if (this.ValidationVote(vote))
                    {
                        if (StoreHelper.CreateVote(vote) > 0)
                        {
                            if (base.Request.QueryString["reurl"] != null)
                            {
                                this.ReUrl = base.Request.QueryString["reurl"].ToString();
                            }
                            this.ShowMsgAndReUrl("成功的添加了一个投票", true, this.ReUrl);
                        }
                        else
                        {
                            this.ShowMsg("添加投票失败", false);
                        }
                    }
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            wid = GetCurWebId();
            if (string.IsNullOrEmpty(wid)) return;
            this.btnAddVote.Click += new EventHandler(this.btnAddVote_Click);
        }

        private bool ValidationVote(VoteInfo vote)
        {
            ValidationResults results = Hishop.Components.Validation.Validation.Validate<VoteInfo>(vote, new string[] { "ValVote" });
            string msg = string.Empty;
            if (!results.IsValid)
            {
                foreach (ValidationResult result in (IEnumerable<ValidationResult>) results)
                {
                    msg = msg + Formatter.FormatErrorMessage(result.Message);
                }
                this.ShowMsg(msg, false);
            }
            return results.IsValid;
        }
    }
}

