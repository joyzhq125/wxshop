namespace Hidistro.UI.Web.Admin
{
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Web.UI.HtmlControls;

    [AdministerCheck(true)]
    public class CnzzStatisticTotal : AdminPage
    {
        protected HtmlGenericControl framcnz;

        protected CnzzStatisticTotal() : base("", "")
        {
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(true,wid);
            if (!string.IsNullOrEmpty(masterSettings.CnzzPassword) && !string.IsNullOrEmpty(masterSettings.CnzzUsername))
            {
                this.framcnz.Attributes["src"] = "http://wss.cnzz.com/user/companion/92hi_login.php?site_id=" + masterSettings.CnzzUsername + "&password=" + masterSettings.CnzzPassword;
            }
            else
            {
                this.Page.Response.Redirect("cnzzstatisticsset.aspx");
            }
        }
    }
}

