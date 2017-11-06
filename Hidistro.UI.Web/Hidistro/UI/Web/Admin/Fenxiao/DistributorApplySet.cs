namespace Hidistro.UI.Web.Admin.Fenxiao
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using Hidistro.UI.Web.hieditor.ueditor.controls;
    using System;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class DistributorApplySet : AdminPage
    {
        protected Button btnSave;
        protected Button Button1;
        protected ucUeditor fckDescription;
        protected HtmlInputRadioButton HasConditions;
        protected HtmlInputRadioButton NoConditions;
        protected HtmlInputCheckBox radioCommissionon;
        protected HtmlInputCheckBox radiorequeston;
        protected Script Script4;
        protected Hidistro.UI.Common.Controls.Style Style1;
        public string tabnum;
        protected HtmlForm thisForm;
        protected HtmlInputText txtrequestmoney;

        protected DistributorApplySet() : base("m05", "fxp02")
        {
            this.tabnum = "0";
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            this.tabnum = "0";
            int result = 0;
            if (this.NoConditions.Checked)
            {
                this.txtrequestmoney.Value = "0";
            }
            if (this.HasConditions.Checked && (!int.TryParse(this.txtrequestmoney.Value.Trim(), out result) || (result < 1)))
            {
                this.ShowMsg("累计消费金额必须为大于0的整数金额", false);
            }
            else
            {
                SiteSettings masterSettings = SettingsManager.GetMasterSettings(false,wid);
                masterSettings.IsRequestDistributor = this.radiorequeston.Checked;
                masterSettings.EnableCommission = this.radioCommissionon.Checked;
                masterSettings.FinishedOrderMoney = result;
                SettingsManager.Save(masterSettings);
                this.ShowMsg("修改成功", true);
            }
        }

        protected void btnSave_Description(object sender, EventArgs e)
        {
            this.tabnum = "1";
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(false,wid);
            masterSettings.DistributorDescription = this.fckDescription.Text.Trim();
            SettingsManager.Save(masterSettings);
            this.ShowMsg("分销说明修改成功", true);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            wid = GetCurWebId();
            if (string.IsNullOrEmpty(wid)) return;
            if (!this.Page.IsPostBack)
            {
                SiteSettings masterSettings = SettingsManager.GetMasterSettings(false,wid);
                this.tabnum = base.Request.QueryString["tabnum"];
                if (string.IsNullOrEmpty(this.tabnum))
                {
                    this.tabnum = "0";
                }
                this.txtrequestmoney.Value = masterSettings.FinishedOrderMoney.ToString();
                this.fckDescription.Text = masterSettings.DistributorDescription;
                this.radiorequeston.Checked = true;
                if (!masterSettings.IsRequestDistributor)
                {
                    this.radiorequeston.Checked = false;
                }
                this.radioCommissionon.Checked = false;
                if (masterSettings.EnableCommission)
                {
                    this.radioCommissionon.Checked = true;
                }
                this.NoConditions.Checked = true;
                if (masterSettings.FinishedOrderMoney > 0)
                {
                    this.HasConditions.Checked = true;
                }
            }
        }
    }
}

