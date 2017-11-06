namespace Hidistro.UI.Web.Admin
{
    using ASPNET.WebControls;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Entities.VShop;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class ReplyOnKey : AdminPage
    {
        protected Grid grdArticleCategories;
        protected string wid;
        protected ReplyOnKey() : base("", "")
        {
        }

        private void BindArticleCategory()
        {
            List<ReplyInfo> list = ReplyHelper.GetAllReply(this.wid).ToList<ReplyInfo>().FindAll(a => a.ReplyType < ReplyType.Wheel);
            this.grdArticleCategories.DataSource = list;
            this.grdArticleCategories.DataBind();
        }

        protected string GetReplyTypeName(object obj)
        {
            ReplyType en = (ReplyType) obj;
            string str = string.Empty;
            bool flag = false;
            if (ReplyType.Subscribe == (en & ReplyType.Subscribe))
            {
                str = str + "[关注时回复]";
                flag = true;
            }
            if (ReplyType.NoMatch == (en & ReplyType.NoMatch))
            {
                str = str + "[无匹配回复]";
                flag = true;
            }
            if (ReplyType.Keys == (en & ReplyType.Keys))
            {
                str = str + "[关键字回复]";
                flag = true;
            }
            if (!flag)
            {
                str = en.ToShowText();
            }
            return str;
        }

        private void grdArticleCategories_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int rowIndex = ((GridViewRow) ((Control) e.CommandSource).NamingContainer).RowIndex;
            int id = (int) this.grdArticleCategories.DataKeys[rowIndex].Value;
            if (e.CommandName == "Delete")
            {
                ReplyHelper.DeleteReply(id);
                base.Response.Redirect(HttpContext.Current.Request.Url.ToString(), true);
            }
            else if (e.CommandName == "Release")
            {
                ReplyHelper.UpdateReplyRelease(id);
                base.Response.Redirect(HttpContext.Current.Request.Url.ToString(), true);
            }
            else if (e.CommandName == "Edit")
            {
                ReplyInfo reply = ReplyHelper.GetReply(id);
                if (reply != null)
                {
                    switch (reply.MessageType)
                    {
                        case MessageType.Text:
                            base.Response.Redirect(string.Format("EditReplyOnKey.aspx?id={0}", id));
                            return;

                        case MessageType.News:
                            base.Response.Redirect(string.Format("EditSingleArticle.aspx?id={0}", id));
                            return;

                        case (MessageType.News | MessageType.Text):
                            return;

                        case MessageType.List:
                            base.Response.Redirect(string.Format("EditMultiArticle.aspx?id={0}", id));
                            return;
                    }
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            wid = GetCurWebId();
            if (string.IsNullOrEmpty(wid)) return;

            this.grdArticleCategories.RowCommand += new GridViewCommandEventHandler(this.grdArticleCategories_RowCommand);
            if (!this.Page.IsPostBack)
            {
                this.BindArticleCategory();
            }
        }
    }
}

