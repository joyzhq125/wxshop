namespace Hidistro.UI.Web.Admin
{
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Security.Cryptography;
    using System.Text;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class VServerConfig : AdminPage
    {
        protected Button btnAdd;
        protected ImageLinkButton btnPicDelete;
        protected Button btnUpoad;
        protected HtmlInputCheckBox chk_manyService;
        protected HtmlInputCheckBox chkIsValidationService;
        protected FileUpload fileUpload;
        protected HiImage imgPic;
        protected Literal litKeycode;
        protected HtmlGenericControl P1;
        protected HtmlGenericControl P2;
        protected Script Script4;
        protected HtmlForm thisForm;
        protected TextBox txtAppId;
        protected TextBox txtAppSecret;
        protected TextBox txtGuidePageSet;
        protected HtmlInputText txtorderpoints;
        protected HtmlInputText txtregisterpoints;
        protected TextBox txtShopIntroduction;
        protected HtmlGenericControl txtShopIntroductionTip;
        protected TextBox txtSiteName;
        protected HtmlGenericControl txtSiteNameTip;
        protected Literal txtToken;
        protected Literal txtUrl;
        protected TextBox txtWeixinLoginUrl;
        protected TextBox txtWeixinNumber;

        protected VServerConfig() : base("", "")
        {
        }

        protected void btnOK_Click(object sender, EventArgs e)
        {
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(false,wid);
            masterSettings.WeixinAppId = this.txtAppId.Text;
            masterSettings.WeixinAppSecret = this.txtAppSecret.Text;
            masterSettings.IsValidationService = this.chkIsValidationService.Checked;
            masterSettings.WeixinNumber = this.txtWeixinNumber.Text;
            masterSettings.WeixinLoginUrl = this.txtWeixinLoginUrl.Text;
            masterSettings.SiteName = this.txtSiteName.Text;
            masterSettings.OpenManyService = this.chk_manyService.Checked;
            masterSettings.ShopIntroduction = this.txtShopIntroduction.Text.Trim();
            masterSettings.GuidePageSet = this.txtGuidePageSet.Text.Trim();
            SettingsManager.Save(masterSettings);
            this.ShowMsg("修改成功", true);
        }

        private void btnPicDelete_Click(object sender, EventArgs e)
        {
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(false,wid);
            if (!string.IsNullOrEmpty(masterSettings.WeiXinCodeImageUrl))
            {
                ResourcesHelper.DeleteImage(masterSettings.WeiXinCodeImageUrl);
                this.btnPicDelete.Visible = false;
                masterSettings.WeiXinCodeImageUrl = this.imgPic.ImageUrl = string.Empty;
                SettingsManager.Save(masterSettings);
            }
        }

        private void btnUpoad_Click(object sender, EventArgs e)
        {
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(false,wid);
            if (this.fileUpload.HasFile)
            {
                try
                {
                    if (!string.IsNullOrEmpty(masterSettings.WeiXinCodeImageUrl))
                    {
                        ResourcesHelper.DeleteImage(masterSettings.WeiXinCodeImageUrl);
                    }
                    this.imgPic.ImageUrl = masterSettings.WeiXinCodeImageUrl = VShopHelper.UploadWeiXinCodeImage(this.fileUpload.PostedFile);
                    this.btnPicDelete.Visible = true;
                    SettingsManager.Save(masterSettings);
                }
                catch
                {
                    this.ShowMsg("图片上传失败，您选择的不是图片类型的文件，或者网站的虚拟目录没有写入文件的权限", false);
                }
            }
        }

        private string CreateKey(int len)
        {
            byte[] data = new byte[len];
            new RNGCryptoServiceProvider().GetBytes(data);
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                builder.Append(string.Format("{0:X2}", data[i]));
            }
            return builder.ToString();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            wid = GetCurWebId();
            if (string.IsNullOrEmpty(wid)) return;
            this.ShowMsg("图片上传失败，您选择的不是图片类型的文件，或者网站的虚拟目录没有写入文件的权限", false);
        }
    }
}

