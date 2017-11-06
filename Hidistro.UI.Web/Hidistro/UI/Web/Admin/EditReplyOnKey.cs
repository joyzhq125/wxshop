namespace Hidistro.UI.Web.Admin
{
    using Hidistro.ControlPanel.Store;
    using Hidistro.Entities.VShop;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Web.UI.WebControls;

    public class EditReplyOnKey : AdminPage
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
        protected EditReplyOnKey() : base("", "")
        {
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            TextReplyInfo reply = ReplyHelper.GetReply(base.GetUrlIntParam("id")) as TextReplyInfo;
            if ((!string.IsNullOrEmpty(this.txtKeys.Text) && (reply.Keys != this.txtKeys.Text.Trim())) && ReplyHelper.HasReplyKey(this.txtKeys.Text.Trim(),this.wid))
            {
                this.ShowMsg("关键字重复!", false);
            }
            else
            {
                reply.IsDisable = !this.radDisable.SelectedValue;
                if (this.chkKeys.Checked && !string.IsNullOrWhiteSpace(this.txtKeys.Text))
                {
                    reply.Keys = this.txtKeys.Text.Trim();
                }
                else
                {
                    reply.Keys = string.Empty;
                }
                reply.Text = this.fcContent.Text.Trim();
                reply.MatchType = this.radMatch.SelectedValue ? MatchType.Like : MatchType.Equal;
                reply.ReplyType = ReplyType.None;
                if (this.chkKeys.Checked)
                {
                    reply.ReplyType |= ReplyType.Keys;
                }
                if (this.chkSub.Checked)
                {
                    reply.ReplyType |= ReplyType.Subscribe;
                }
                if (this.chkNo.Checked)
                {
                    reply.ReplyType |= ReplyType.NoMatch;
                }
                if (reply.ReplyType == ReplyType.None)
                {
                    this.ShowMsg("请选择回复类型", false);
                }
                else if (ReplyHelper.UpdateReply(reply))
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
                TextReplyInfo reply = ReplyHelper.GetReply(base.GetUrlIntParam("id")) as TextReplyInfo;
                if (reply == null)
                {
                    base.GotoResourceNotFound();
                }
                else
                {
                    this.fcContent.Text = reply.Text;
                    this.txtKeys.Text = reply.Keys;
                    this.radMatch.SelectedValue = reply.MatchType == MatchType.Like;
                    this.radDisable.SelectedValue = !reply.IsDisable;
                    this.chkKeys.Checked = ReplyType.Keys == (reply.ReplyType & ReplyType.Keys);
                    this.chkSub.Checked = ReplyType.Subscribe == (reply.ReplyType & ReplyType.Subscribe);
                    this.chkNo.Checked = ReplyType.NoMatch == (reply.ReplyType & ReplyType.NoMatch);
                    if (this.chkNo.Checked)
                    {
                        this.chkNo.Enabled = true;
                        this.chkNo.ToolTip = "";
                    }
                    if (this.chkSub.Checked)
                    {
                        this.chkSub.Enabled = true;
                        this.chkSub.ToolTip = "";
                    }
                }
            }
        }
    }
}

