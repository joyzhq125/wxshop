namespace Hidistro.UI.Web.Admin
{
    using Hidistro.ControlPanel.Sales;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Sales;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Web.UI.WebControls;
    using System.Xml;

    public class BasicConfig : AdminPage
    {
        protected Button btnOK;
        protected TextBox txtAccount;
        protected TextBox txtKey;
        protected TextBox txtPartner;

        protected BasicConfig() : base("", "")
        {
        }

        protected void btnOK_Click(object sender, EventArgs e)
        {
            string text = string.Format("<xml><Partner>{0}</Partner><Key>{1}</Key><Seller_account_name>{2}</Seller_account_name></xml>", this.txtPartner.Text, this.txtKey.Text, this.txtAccount.Text);
            bool flag = (!string.IsNullOrWhiteSpace(this.txtPartner.Text) && !string.IsNullOrWhiteSpace(this.txtKey.Text)) && !string.IsNullOrWhiteSpace(this.txtAccount.Text);
            PaymentModeInfo paymentMode = SalesHelper.GetPaymentMode("hishop.plugins.payment.ws_wappay.wswappayrequest",this.wid);
            if (paymentMode == null)
            {
                PaymentModeInfo info2 = new PaymentModeInfo {
                    Name = "支付宝手机支付",
                    Gateway = "hishop.plugins.payment.ws_wappay.wswappayrequest",
                    Description = string.Empty,
                    IsUseInpour = true,
                    Charge = 0M,
                    IsPercent = false,
                    Settings = HiCryptographer.Encrypt(text)
                };
                if (SalesHelper.CreatePaymentMode(info2) == PaymentModeActionStatus.Success)
                {
                    SiteSettings masterSettings = SettingsManager.GetMasterSettings(false,wid);
                    masterSettings.EnableAlipayRequest = flag;
                    SettingsManager.Save(masterSettings);
                    this.ShowMsg("设置成功", true);
                }
                else
                {
                    this.ShowMsg("设置失败", false);
                }
            }
            else
            {
                paymentMode.Settings = HiCryptographer.Encrypt(text);
                if (SalesHelper.UpdatePaymentMode(paymentMode) == PaymentModeActionStatus.Success)
                {
                    SiteSettings settings = SettingsManager.GetMasterSettings(false,wid);
                    settings.EnableAlipayRequest = flag;
                    SettingsManager.Save(settings);
                    this.ShowMsg("设置成功", true);
                }
                else
                {
                    this.ShowMsg("设置失败", false);
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            wid = GetCurWebId();
            if (string.IsNullOrEmpty(wid)) return;
            this.btnOK.Click += new EventHandler(this.btnOK_Click);
            if (!base.IsPostBack)
            {
                PaymentModeInfo paymentMode = SalesHelper.GetPaymentMode("hishop.plugins.payment.ws_wappay.wswappayrequest",this.wid);
                if (paymentMode != null)
                {
                    string xml = HiCryptographer.Decrypt(paymentMode.Settings);
                    XmlDocument document = new XmlDocument();
                    document.LoadXml(xml);
                    this.txtPartner.Text = document.GetElementsByTagName("Partner")[0].InnerText;
                    this.txtKey.Text = document.GetElementsByTagName("Key")[0].InnerText;
                    this.txtAccount.Text = document.GetElementsByTagName("Seller_account_name")[0].InnerText;
                }
            }
        }
    }
}

