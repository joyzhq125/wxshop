namespace Hidistro.UI.Web.Admin.Shop
{
    using Hidistro.ControlPanel.WeiBo;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Entities.Store;
    using Hidistro.UI.ControlPanel.Utility;
    using System;

    public class Articles : AdminPage
    {
        protected int articleid;
        protected string ArticleTitle;
        protected int articletype;

        protected Articles() : base("m01", "dpp06")
        {
            this.ArticleTitle = string.Empty;
        }

        private void LoadParameters(string _stype)
        {
            int.TryParse(_stype, out this.articletype);
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["key"]))
            {
                this.ArticleTitle = base.Server.UrlDecode(this.Page.Request.QueryString["key"]);
            }
        }

        [PrivilegeCheck(Privilege.Summary)]
        protected void Page_Load(object sender, EventArgs e)
        {
            string str = base.Request.QueryString["articletype"];
            string str2 = base.Request.Form["posttype"];
            string s = base.Request.Form["id"];
            int.TryParse(s, out this.articleid);
            if ((str2 == "del") && (this.articleid > 0))
            {
                base.Response.ContentType = "application/json";
                string str4 = "{\"type\":\"0\",\"tips\":\"操作失败\"}";
                if (ArticleHelper.DeleteArticle(this.articleid))
                {
                    str4 = "{\"type\":\"1\",\"tips\":\"删除成功\"}";
                }
                base.Response.Write(str4);
                base.Response.End();
            }
            else
            {
                this.LoadParameters(str);
            }
        }
    }
}

