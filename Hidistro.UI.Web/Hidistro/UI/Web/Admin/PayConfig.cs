namespace Hidistro.UI.Web.Admin
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.IO;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class PayConfig : AdminPage
    {
        private string _dataPath;
        protected Button btnOK;
        protected FileUpload fileUploader;
        protected HtmlGenericControl labfilename;
        protected YesNoRadioButtonList radEnableHtmRewrite;
        protected TextBox txtAppId;
        protected TextBox txtAppSecret;
        protected TextBox txtCertPassword;
        protected TextBox txtPartnerID;
        protected TextBox txtPartnerKey;
        protected TextBox txtPaySignKey;

        protected PayConfig() : base("", "")
        {
        }

        protected void btnOK_Click(object sender, EventArgs e)
        {
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(false,wid);
            masterSettings.WeixinAppId = this.txtAppId.Text;
            masterSettings.WeixinAppSecret = this.txtAppSecret.Text;
            masterSettings.WeixinPartnerID = this.txtPartnerID.Text;
            masterSettings.WeixinPartnerKey = this.txtPartnerKey.Text;
            masterSettings.WeixinPaySignKey = this.txtPaySignKey.Text;
            masterSettings.EnableWeiXinRequest = this.radEnableHtmRewrite.SelectedValue;
            if (this.fileUploader.PostedFile.FileName != "")
            {
                if (!this.IsAllowableFileType(this.fileUploader.PostedFile.FileName))
                {
                    this.ShowMsg("请上传正确的文件", false);
                    return;
                }
                string str = DateTime.Now.ToString("yyyyMMddhhmmss") + Path.GetFileName(this.fileUploader.PostedFile.FileName);
                this.fileUploader.PostedFile.SaveAs(Path.Combine(this._dataPath, str));
                masterSettings.WeixinCertPath = Path.Combine(this._dataPath, str);
            }
            masterSettings.WeixinCertPassword = this.txtCertPassword.Text;
            SettingsManager.Save(masterSettings);
            this.ShowMsg("设置成功", true);
        }

        protected bool IsAllowableFileType(string FileName)
        {
            string str = ".p12";
            return (str.IndexOf(Path.GetExtension(FileName).ToLower()) != -1);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            wid = GetCurWebId();
            if (string.IsNullOrEmpty(wid)) return;
            this.btnOK.Click += new EventHandler(this.btnOK_Click);
            this._dataPath = this.Page.Request.MapPath("~/Pay/Cert");
            if (!base.IsPostBack)
            {
                SiteSettings masterSettings = SettingsManager.GetMasterSettings(false,wid);
                this.txtAppId.Text = masterSettings.WeixinAppId;
                this.txtAppSecret.Text = masterSettings.WeixinAppSecret;
                this.txtPartnerID.Text = masterSettings.WeixinPartnerID;
                this.txtPartnerKey.Text = masterSettings.WeixinPartnerKey;
                this.txtPaySignKey.Text = masterSettings.WeixinPaySignKey;
                this.radEnableHtmRewrite.SelectedValue = masterSettings.EnableWeiXinRequest;
                this.labfilename.InnerText = (masterSettings.WeixinCertPath != "") ? ("已上传：" + masterSettings.WeixinCertPath.Substring(masterSettings.WeixinCertPath.LastIndexOf(@"\") + 1, (masterSettings.WeixinCertPath.Length - masterSettings.WeixinCertPath.LastIndexOf(@"\")) - 1)) : "";
                this.txtCertPassword.Text = masterSettings.WeixinCertPassword;
            }
        }
    }
}

