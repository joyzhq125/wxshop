namespace Hidistro.UI.Web.Admin.distributor
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.IO;
    using System.Web.UI.WebControls;

    public class AddDistributorBackgroundPic : AdminPage
    {
        protected Button btnSave;
        protected HiddenField hidpic;
        protected HiddenField hidpicdel;

        protected AddDistributorBackgroundPic() : base("", "")
        {
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(false,wid);
            masterSettings.DistributorBackgroundPic = this.hidpic.Value;
            SettingsManager.Save(masterSettings);
            if (!string.IsNullOrEmpty(this.hidpicdel.Value))
            {
                foreach (string str in this.hidpicdel.Value.Split(new char[] { '|' }))
                {
                    string path = str;
                    path = base.Server.MapPath(path);
                    if (File.Exists(path))
                    {
                        File.Delete(path);
                    }
                }
            }
            this.hidpicdel.Value = "";
            this.ShowMsg("修改成功", true);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            wid = GetCurWebId();
            if (string.IsNullOrEmpty(wid)) return;
            this.btnSave.Click += new EventHandler(this.btnSave_Click);
            if (!base.IsPostBack)
            {
                SiteSettings masterSettings = SettingsManager.GetMasterSettings(false,wid);
                this.hidpic.Value = masterSettings.DistributorBackgroundPic;
            }
        }
    }
}

