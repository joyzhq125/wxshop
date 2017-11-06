namespace Hidistro.UI.Web.Admin.vshop
{
    using Hidistro.ControlPanel.Store;
    using Hidistro.Entities.VShop;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using kindeditor.Net;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Web.UI.WebControls;

    public class EditMultiArticle : AdminPage
    {
        protected string articleJson;
        protected CheckBox chkKeys;
        protected CheckBox chkNo;
        protected CheckBox chkSub;
        protected KindeditorControl fkContent;
        protected int MaterialID;
        protected YesNoRadioButtonList radDisable;
        protected YesNoRadioButtonList radMatch;
        protected TextBox txtKeys;
        protected string wid;
        protected EditMultiArticle() : base("", "")
        {
        }

        protected void btnCreate_Click(object sender, EventArgs e)
        {
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            wid = GetCurWebId();
            if (string.IsNullOrEmpty(wid)) return;
            if (!int.TryParse(base.Request.QueryString["id"], out this.MaterialID))
            {
                base.Response.Redirect("ReplyOnKey.aspx");
            }
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
            if (!string.IsNullOrEmpty(base.Request.QueryString["cmd"]))
            {
                if (!string.IsNullOrEmpty(base.Request.Form["MultiArticle"]))
                {
                    string str = base.Request.Form["MultiArticle"];
                    List<ArticleList> list = JsonConvert.DeserializeObject<List<ArticleList>>(str);
                    if ((list != null) && (list.Count > 0))
                    {
                        NewsReplyInfo reply = ReplyHelper.GetReply(this.MaterialID) as NewsReplyInfo;
                        reply.IsDisable = base.Request.Form["radDisable"] != "true";
                        string str2 = base.Request.Form.Get("Keys");
                        if (base.Request.Form["chkKeys"] == "true")
                        {
                            if ((!string.IsNullOrEmpty(str2) && (reply.Keys != str2)) && ReplyHelper.HasReplyKey(str2,this.wid))
                            {
                                base.Response.Write("key");
                                base.Response.End();
                            }
                            reply.Keys = str2;
                        }
                        else
                        {
                            reply.Keys = string.Empty;
                        }
                        reply.MatchType = (base.Request.Form["radMatch"] == "true") ? MatchType.Like : MatchType.Equal;
                        reply.ReplyType = ReplyType.None;
                        if (base.Request.Form["chkKeys"] == "true")
                        {
                            reply.ReplyType |= ReplyType.Keys;
                        }
                        if (base.Request.Form["chkSub"] == "true")
                        {
                            reply.ReplyType |= ReplyType.Subscribe;
                        }
                        if (base.Request.Form["chkNo"] == "true")
                        {
                            reply.ReplyType |= ReplyType.NoMatch;
                        }
                        foreach (NewsMsgInfo info2 in reply.NewsMsg)
                        {
                            ReplyHelper.DeleteNewsMsg(info2.Id);
                        }
                        List<NewsMsgInfo> list2 = new List<NewsMsgInfo>();
                        foreach (ArticleList list3 in list)
                        {
                            if (list3.Status != "del")
                            {
                                NewsMsgInfo item = list3;
                                if (item != null)
                                {
                                    item.Reply = reply;
                                    list2.Add(item);
                                }
                            }
                        }
                        reply.NewsMsg = list2;
                        if (ReplyHelper.UpdateReply(reply))
                        {
                            base.Response.Write("true");
                            base.Response.End();
                        }
                    }
                }
            }
            else
            {
                NewsReplyInfo info4 = ReplyHelper.GetReply(this.MaterialID) as NewsReplyInfo;
                if (info4 != null)
                {
                    List<ArticleList> list4 = new List<ArticleList>();
                    if ((info4.NewsMsg != null) && (info4.NewsMsg.Count > 0))
                    {
                        int num = 1;
                        foreach (NewsMsgInfo info5 in info4.NewsMsg)
                        {
                            ArticleList list5 = new ArticleList {
                                PicUrl = info5.PicUrl,
                                Title = info5.Title,
                                Url = info5.Url,
                                Description = info5.Description,
                                Content = info5.Content
                            };
                            list5.BoxId = num++.ToString();
                            list5.Status = "";
                            list4.Add(list5);
                        }
                        this.articleJson = JsonConvert.SerializeObject(list4);
                    }
                    this.fkContent.Text = info4.NewsMsg[0].Content;
                    this.txtKeys.Text = info4.Keys;
                    this.radMatch.SelectedValue = info4.MatchType == MatchType.Like;
                    this.radDisable.SelectedValue = !info4.IsDisable;
                    this.chkKeys.Checked = ReplyType.Keys == (info4.ReplyType & ReplyType.Keys);
                    this.chkSub.Checked = ReplyType.Subscribe == (info4.ReplyType & ReplyType.Subscribe);
                    this.chkNo.Checked = ReplyType.NoMatch == (info4.ReplyType & ReplyType.NoMatch);
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

