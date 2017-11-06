namespace Hidistro.UI.Web.Admin.vshop
{
    using Hidistro.Core;
    using System;
    using System.Web.UI;

    public class ShowActivityUrl : Page
    {
        protected string htmlActivityUrl = string.Empty;

        public string GetUrl(string type, int voteId)
        {
            switch (type)
            {
                case "vote":
                    return string.Concat(new object[] { "http://", Globals.DomainName, Globals.ApplicationPath, "/Vshop/Vote.aspx?voteId=", voteId });

                case "baoming":
                    return string.Concat(new object[] { "http://", Globals.DomainName, Globals.ApplicationPath, "/Vshop/Activity.aspx?id=", voteId });

                case "choujiang":
                    return string.Concat(new object[] { "http://", Globals.DomainName, Globals.ApplicationPath, "/Vshop/Ticket.aspx?id=", voteId });
            }
            return string.Concat(new object[] { "http://", Globals.DomainName, Globals.ApplicationPath, "/Vshop/Vote.aspx?voteId=", voteId });
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            string str = base.Request.QueryString["url"];
            if (!string.IsNullOrEmpty(str))
            {
                this.htmlActivityUrl = str;
            }
            else
            {
                base.Response.Write("参数错误！");
                base.Response.End();
            }
        }
    }
}

