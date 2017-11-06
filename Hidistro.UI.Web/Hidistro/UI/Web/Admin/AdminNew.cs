namespace Hidistro.UI.Web.Admin
{
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Store;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class AdminNew : MasterPage
    {
        protected ContentPlaceHolder ContentPlaceHolder1;
        protected int CurrentUserId;
        protected ContentPlaceHolder head;
        protected Literal leftMenu;
        protected Literal litSitename;
        protected Literal litUsername;
        protected PageTitle PageTitle1;
        protected Literal topMenu;
        public string wid = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            ManagerInfo currentManager = ManagerHelper.GetCurrentManager();
            this.CurrentUserId = currentManager.UserId;

            wid = Session[DTKeys.SESSION_WEB_ID] as string;
            if (string.IsNullOrEmpty(wid)) return;

            if (!this.Page.IsPostBack)
            {
                AdminPage page = this.Page as AdminPage;
                Navigation navigation = Navigation.GetNavigation();
                this.topMenu.Text = navigation.RenderTopMenu(page.ModuleId);
                this.leftMenu.Text = navigation.RenderLeftMenu(page.ModuleId, page.PageId);
                SiteSettings masterSettings = SettingsManager.GetMasterSettings(true,wid);
                PageTitle.wid = this.wid;
                this.litSitename.Text = masterSettings.SiteName;
                this.litUsername.Text = currentManager.UserName;
            }
        }
    }
}

