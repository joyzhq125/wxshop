namespace Hidistro.UI.Web.Admin.vshop
{
    using Hidistro.ControlPanel.Sales;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Sales;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Web.UI.WebControls;
    using System.Xml;

    public class ShengPaySet : AdminPage
    {
        protected Button btnOK;
        protected YesNoRadioButtonList radEnableWapShengPay;
        protected TextBox txtKey;
        protected TextBox txtPartner;

        protected ShengPaySet() : base("", "")
        {
        }

        protected void btnOK_Click(object sender, EventArgs e)
        {
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(false,wid);
            masterSettings.EnableWapShengPay = this.radEnableWapShengPay.SelectedValue;
            SettingsManager.Save(masterSettings);
            string text = string.Format("<xml><SenderId>{0}</SenderId><SellerKey>{1}</SellerKey><Seller_account_name></Seller_account_name></xml>", this.txtPartner.Text, this.txtKey.Text);
            PaymentModeInfo paymentMode = SalesHelper.GetPaymentMode("Hishop.Plugins.Payment.ShengPayMobile.ShengPayMobileRequest",this.wid);
            if (paymentMode == null)
            {
                PaymentModeInfo info2 = new PaymentModeInfo {
                    Name = "盛付通手机网页支付",
                    Gateway = "Hishop.Plugins.Payment.ShengPayMobile.ShengPayMobileRequest",
                    Description = string.Empty,
                    IsUseInpour = false,
                    Charge = 0M,
                    IsPercent = false,
                    ApplicationType = PayApplicationType.payOnWAP,
                    Settings = HiCryptographer.Encrypt(text)
                };
                SalesHelper.CreatePaymentMode(info2);
            }
            else
            {
                PaymentModeInfo info4 = paymentMode;
                info4.Settings = HiCryptographer.Encrypt(text);
                info4.ApplicationType = PayApplicationType.payOnWAP;
                SalesHelper.UpdatePaymentMode(info4);
            }
            this.ShowMsg("修改成功", true);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            wid = GetCurWebId();
            if (string.IsNullOrEmpty(wid)) return;
            this.btnOK.Click += new EventHandler(this.btnOK_Click);
            if (!base.IsPostBack)
            {
                SiteSettings masterSettings = SettingsManager.GetMasterSettings(false,wid);
                this.radEnableWapShengPay.SelectedValue = masterSettings.EnableWapShengPay;
                PaymentModeInfo paymentMode = SalesHelper.GetPaymentMode("Hishop.Plugins.Payment.ShengPayMobile.ShengPayMobileRequest",this.wid);
                if (paymentMode != null)
                {
                    string xml = HiCryptographer.Decrypt(paymentMode.Settings);
                    XmlDocument document = new XmlDocument();
                    document.LoadXml(xml);
                    try
                    {
                        this.txtPartner.Text = document.GetElementsByTagName("SenderId")[0].InnerText;
                        this.txtKey.Text = document.GetElementsByTagName("SellerKey")[0].InnerText;
                    }
                    catch
                    {
                        this.txtPartner.Text = "";
                        this.txtKey.Text = "";
                    }
                }
                PaymentModeInfo info2 = SalesHelper.GetPaymentMode("Hishop.Plugins.Payment.ShengPayMobile.ShengPayMobileRequest",this.wid);
                if (info2 != null)
                {
                    string str2 = HiCryptographer.Decrypt(info2.Settings);
                    new XmlDocument().LoadXml(str2);
                }
            }
        }
    }
}

