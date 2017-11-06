namespace Hidistro.UI.Web.Admin.WeiXin
{
    using Hidistro.ControlPanel.WeiXin;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Store;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Security.Cryptography;
    using System.Text;
    using System.Web.UI.WebControls;

    [PrivilegeCheck(Privilege.ProductCategory)]
    public class WXConfig : AdminPage
    {
        protected Button btnSave;
        protected HiddenField hdfCopyToken;
        protected HiddenField hdfCopyUrl;
        //private SiteSettings siteSettings;
        protected TextBox txtAppId;
        protected TextBox txtAppSecret;
        protected Literal txtToken;
        protected Literal txtUrl;
        SF.BLL.sf_website bll;
        SF.Model.sf_website website;
        //string wid;
        protected WXConfig() : base("m06", "wxp01")
        {
            //this.siteSettings = SettingsManager.GetMasterSettings(false);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            //SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
            //if (masterSettings.WeixinAppId != this.txtAppId.Text.Trim())
            //{
            //    WeiXinHelper.ClearWeiXinMediaID();
            //}
            //masterSettings.WeixinAppId = this.txtAppId.Text.Trim();
            //masterSettings.WeixinAppSecret = this.txtAppSecret.Text.Trim();
            //SettingsManager.Save(masterSettings);
            //this.ShowMsg("修改成功", true);

            if (website.appid != this.txtAppId.Text.Trim())
            {
                WeiXinHelper.ClearWeiXinMediaID(this.wid);
            }
            website.appid = this.txtAppId.Text.Trim();
            website.appsecret = this.txtAppSecret.Text.Trim();
            bll.Update(website);
            this.ShowMsg("修改成功", true);

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
            bll = new SF.BLL.sf_website();
            website = bll.GetModelByWid(wid);
            if (website == null) return;

            if (!this.Page.IsPostBack)
            {
                //int mid = Globals.GetCurrentManagerUserId();
                //SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
                //if (string.IsNullOrEmpty(masterSettings.WeixinToken))
                if (string.IsNullOrEmpty(website.token_value))
                {
                    website.token_value = this.CreateKey(8);
                    //SettingsManager.Save(masterSettings);
                    bll.Update(website);
                }

                //if (string.IsNullOrWhiteSpace(masterSettings.CheckCode))
                //{
                //    masterSettings.CheckCode = this.CreateKey(20);
                //    SettingsManager.Save(masterSettings);
                //}

                this.hdfCopyUrl.Value = this.txtUrl.Text = string.Format("http://{0}/api/wx.ashx?wid={1}", base.Request.Url.Host, this.wid/*this.txtToken.Text*/);
                this.hdfCopyToken.Value = this.txtToken.Text = website.token_value;
                this.txtAppId.Text = website.appid;
                this.txtAppSecret.Text = website.appsecret;
            }
        }
    }
}

