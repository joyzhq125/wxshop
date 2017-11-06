namespace Hidistro.UI.Web.Admin.Settings
{
    using Hidistro.ControlPanel.WeiXin;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class WeixinPay : AdminPage
    {
        protected bool _enable;
        protected Script Script4;
        //private SiteSettings siteSettings;
        protected HtmlForm thisForm;
        protected TextBox txt_appid;
        protected TextBox txt_appsecret;
        protected TextBox txt_key;
        protected TextBox txt_mch_id;
        SF.BLL.sf_website bll;
        SF.Model.sf_website website;
        protected WeixinPay() : base("m06", "wxp08")
        {
            //this.siteSettings = SettingsManager.GetMasterSettings(false);
            //SF.Model.sf_website website = new SF.BLL.sf_website().GetModelByWid(wid);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            wid = GetCurWebId();
            if (string.IsNullOrEmpty(wid)) return;
            bll = new SF.BLL.sf_website();
            website = bll.GetModelByWid(wid);
            if (website == null) return;

            if (!base.IsPostBack)
            {
                this.txt_appid.Text = website.appid;//this.siteSettings.WeixinAppId;
                this.txt_appsecret.Text = website.appsecret;//this.siteSettings.WeixinAppSecret;
                this.txt_key.Text = website.account_pay_key; //this.siteSettings.WeixinPartnerKey;
                this.txt_mch_id.Text = website.weixin_pay_account;//this.siteSettings.WeixinPartnerID;
                //this._enable = website.Enableweixinrequest.Equals("1") ? true : false;//website.state==0?true:false;
            }
            //this._enable = website.state;//this.siteSettings.EnableWeiXinRequest;
            this._enable = website.Enableweixinrequest=="1" ? true : false;
        }

        private void saveData()
        {
            if (string.IsNullOrEmpty(this.txt_appid.Text.Trim()))
            {
                this.ShowMsg("请输入appid！", false);
            }
            if (string.IsNullOrEmpty(this.txt_appsecret.Text.Trim()))
            {
                this.ShowMsg("请输入appsecret！", false);
            }
            if (string.IsNullOrEmpty(this.txt_key.Text.Trim()))
            {
                this.ShowMsg("请输入Key！", false);
            }
            if (string.IsNullOrEmpty(this.txt_mch_id.Text.Trim()))
            {
                this.ShowMsg("请输入mch_id！", false);
            }
            if (this.website.appid != this.txt_appid.Text.Trim())
            {
                WeiXinHelper.ClearWeiXinMediaID(this.wid);
            }
            //this.siteSettings.WeixinAppId = this.txt_appid.Text.Trim();
            //this.siteSettings.WeixinAppSecret = this.txt_appsecret.Text.Trim();
            //this.siteSettings.WeixinPartnerKey = this.txt_key.Text.Trim();
            //this.siteSettings.WeixinPartnerID = this.txt_mch_id.Text.Trim();
            this.website.appid = this.txt_appid.Text.Trim();
            this.website.appsecret = this.txt_appsecret.Text.Trim();
            this.website.account_pay_key = this.txt_key.Text.Trim();
            this.website.weixin_pay_account = this.txt_mch_id.Text.Trim();
            //SettingsManager.Save(this.siteSettings);
            if (bll.Update(this.website))
            {
                this.ShowMsg("保存成功！", true);
            }
            else
            {
                this.ShowMsg("保存失败！", false);
            }
        }

        protected void Unnamed_Click(object sender, EventArgs e)
        {
            this.saveData();
        }
    }
}

