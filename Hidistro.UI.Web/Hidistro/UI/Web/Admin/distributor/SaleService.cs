namespace Hidistro.UI.Web.Admin.distributor
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.UI.ControlPanel.Utility;
    using kindeditor.Net;
    using System;
    using System.Web.UI.WebControls;

    public class SaleService : AdminPage
    {
        protected Button btnSave;
        protected KindeditorControl fkContent;

        protected SaleService() : base("", "")
        {
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(false,wid);
            masterSettings.SaleService = this.fkContent.Text;
            SettingsManager.Save(masterSettings);
            this.ShowMsg("修改成功", true);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            wid = GetCurWebId();
            if (string.IsNullOrEmpty(wid)) return;
            if (!this.Page.IsPostBack)
            {
                SiteSettings masterSettings = SettingsManager.GetMasterSettings(false,wid);
                this.fkContent.Text = masterSettings.SaleService;
            }
        }
    }
}

