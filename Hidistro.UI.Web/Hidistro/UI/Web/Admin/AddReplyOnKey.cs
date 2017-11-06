namespace Hidistro.UI.Web.Admin
{
    using Hidistro.ControlPanel.Store;
    using Hidistro.Entities.VShop;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Web.UI.WebControls;

    public class AddReplyOnKey : AdminPage
    {
        protected Button btnAdd;
        protected CheckBox chkKeys;
        protected CheckBox chkNo;
        protected CheckBox chkSub;
        protected TextBox fcContent;
        protected YesNoRadioButtonList radDisable;
        protected YesNoRadioButtonList radMatch;
        protected TextBox txtKeys;
        protected string wid;
        protected AddReplyOnKey() : base("", "")
        {
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.txtKeys.Text) && ReplyHelper.HasReplyKey(this.txtKeys.Text.Trim(),this.wid))
            {
                this.ShowMsg("关键字重复!", false);
            }
            else
            {
                TextReplyInfo info;
                info = info = new TextReplyInfo();
                info.IsDisable = !this.radDisable.SelectedValue;
                if (this.chkKeys.Checked && !string.IsNullOrWhiteSpace(this.txtKeys.Text))
                {
                    info.Keys = this.txtKeys.Text.Trim();
                }
                info.Text = this.fcContent.Text.Trim();
                info.MatchType = this.radMatch.SelectedValue ? MatchType.Like : MatchType.Equal;
                if (this.chkKeys.Checked)
                {
                    info.ReplyType |= ReplyType.Keys;
                }
                if (this.chkSub.Checked)
                {
                    info.ReplyType |= ReplyType.Subscribe;
                }
                if (this.chkNo.Checked)
                {
                    info.ReplyType |= ReplyType.NoMatch;
                }
                if (info.ReplyType == ReplyType.None)
                {
                    this.ShowMsg("请选择回复类型", false);
                }
                else if (ReplyHelper.SaveReply(info))
                {
                    this.ShowMsg("添加成功", true);
                }
                else
                {
                    this.ShowMsg("添加失败", false);
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            wid = GetCurWebId();
            if (string.IsNullOrEmpty(wid)) return;
            this.btnAdd.Click += new EventHandler(this.btnSubmit_Click);
            this.radMatch.Items[0].Text = "模糊匹配";
            this.radMatch.Items[1].Text = "精确匹配";
            this.radDisable.Items[0].Text = "启用";
            this.radDisable.Items[1].Text = "禁用";
            this.chkNo.Enabled = ReplyHelper.GetMismatchReply(this.wid) == null;
            this.chkSub.Enabled = ReplyHelper.GetSubscribeReply(this.wid) == null;
            if (!this.chkNo.Enabled)
            {
                this.chkNo.ToolTip = "该类型已被使用";
            }
            if (!this.chkSub.Enabled)
            {
                this.chkSub.ToolTip = "该类型已被使用";
            }
            if (!base.IsPostBack)
            {
                this.chkKeys.Checked = true;
            }
        }
    }
}

