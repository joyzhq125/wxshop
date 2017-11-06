namespace Hidistro.UI.Web.Admin
{
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Globalization;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    [AdministerCheck(true)]
    public class SetShoppingScore : AdminPage
    {
        protected Button btnOK;
        protected TextBox txtProductPointSet;
        protected HtmlGenericControl txtProductPointSetTip;

        protected SetShoppingScore() : base("", "")
        {
        }

        protected void btnOK_Click(object sender, EventArgs e)
        {
            decimal num;
            if (!decimal.TryParse(this.txtProductPointSet.Text.Trim(), out num) || (this.txtProductPointSet.Text.Trim().Contains(".") && (this.txtProductPointSet.Text.Trim().Substring(this.txtProductPointSet.Text.Trim().IndexOf(".") + 1).Length > 2)))
            {
                this.ShowMsg("几元一积分不能为空,为非负数字,范围在0.1-10000000之间", false);
            }
            else
            {
                SiteSettings masterSettings = SettingsManager.GetMasterSettings(false,wid);
                masterSettings.PointsRate = num;
                Globals.EntityCoding(masterSettings, true);
                SettingsManager.Save(masterSettings);
                this.ShowMsg("保存成功", true);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            wid = GetCurWebId();
            if (string.IsNullOrEmpty(wid)) return;
            if (!this.Page.IsPostBack)
            {
                SiteSettings masterSettings = SettingsManager.GetMasterSettings(false,wid);
                this.txtProductPointSet.Text = masterSettings.PointsRate.ToString(CultureInfo.InvariantCulture);
            }
            this.btnOK.Click += new EventHandler(this.btnOK_Click);
        }
    }
}

