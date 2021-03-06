﻿namespace Hidistro.UI.Web.Admin
{
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using Hishop.Components.Validation;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Runtime.InteropServices;
    using System.Web;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;
    using System.Xml;

    [AdministerCheck(true)]
    public class SetOrderOption : AdminPage
    {
        protected Button btnOK;
        protected TextBox txtCloseOrderDays;
        protected HtmlGenericControl txtCloseOrderDaysTip;
        protected TextBox txtFinishOrderDays;
        protected HtmlGenericControl txtFinishOrderDaysTip;
        protected TextBox txtShowDays;
        protected HtmlGenericControl txtShowDaysTip;
        protected TextBox txtTaxRate;
        protected HtmlGenericControl txtTaxRateTip;

        protected SetOrderOption() : base("", "")
        {
        }

        protected void btnOK_Click(object sender, EventArgs e)
        {
            int num;
            int num2;
            int num3;
            decimal num4;
            if (this.ValidateValues(out num, out num2, out num3, out num4))
            {
                SiteSettings masterSettings = SettingsManager.GetMasterSettings(false,wid);
                masterSettings.OrderShowDays = num;
                masterSettings.CloseOrderDays = num2;
                masterSettings.FinishOrderDays = num3;
                masterSettings.TaxRate = num4;
                if (this.ValidationSettings(masterSettings))
                {
                    Globals.EntityCoding(masterSettings, true);
                    SettingsManager.Save(masterSettings);
                    this.SavaKuaidi100Key();
                    this.ShowMsg("保存成功", true);
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            wid = GetCurWebId();
            if (string.IsNullOrEmpty(wid)) return;
            if (!this.Page.IsPostBack)
            {
                SiteSettings masterSettings = SettingsManager.GetMasterSettings(false,wid);
                this.txtShowDays.Text = masterSettings.OrderShowDays.ToString(CultureInfo.InvariantCulture);
                this.txtCloseOrderDays.Text = masterSettings.CloseOrderDays.ToString(CultureInfo.InvariantCulture);
                this.txtFinishOrderDays.Text = masterSettings.FinishOrderDays.ToString(CultureInfo.InvariantCulture);
                this.txtTaxRate.Text = masterSettings.TaxRate.ToString(CultureInfo.InvariantCulture);
                XmlDocument document = new XmlDocument();
                string filename = HttpContext.Current.Request.MapPath("~/Express.xml");
                document.Load(filename);
                document.SelectSingleNode("companys");
            }
            this.btnOK.Click += new EventHandler(this.btnOK_Click);
        }

        private void SavaKuaidi100Key()
        {
            XmlDocument document = new XmlDocument();
            string filename = HttpContext.Current.Request.MapPath("~/Express.xml");
            document.Load(filename);
            document.SelectSingleNode("companys");
            document.Save(filename);
        }

        private bool ValidateValues(out int showDays, out int closeOrderDays, out int finishOrderDays, out decimal taxRate)
        {
            string str = string.Empty;
            if (!int.TryParse(this.txtShowDays.Text, out showDays))
            {
                str = str + Formatter.FormatErrorMessage("订单显示设置不能为空,必须为正整数,范围在1-90之间");
            }
            if (!int.TryParse(this.txtCloseOrderDays.Text, out closeOrderDays))
            {
                str = str + Formatter.FormatErrorMessage("过期几天自动关闭订单不能为空,必须为正整数,范围在1-90之间");
            }
            if (!int.TryParse(this.txtFinishOrderDays.Text, out finishOrderDays))
            {
                str = str + Formatter.FormatErrorMessage("发货几天自动完成订单不能为空,必须为正整数,范围在1-90之间");
            }
            if (!decimal.TryParse(this.txtTaxRate.Text, out taxRate))
            {
                str = str + Formatter.FormatErrorMessage("订单发票税率不能为空,为非负数字,范围在0-100之间");
            }
            if (!string.IsNullOrEmpty(str))
            {
                this.ShowMsg(str, false);
                return false;
            }
            return true;
        }

        private bool ValidationSettings(SiteSettings setting)
        {
            ValidationResults results = Hishop.Components.Validation.Validation.Validate<SiteSettings>(setting, new string[] { "ValMasterSettings" });
            string msg = string.Empty;
            if (!results.IsValid)
            {
                foreach (ValidationResult result in (IEnumerable<ValidationResult>) results)
                {
                    msg = msg + Formatter.FormatErrorMessage(result.Message);
                }
                this.ShowMsg(msg, false);
            }
            return results.IsValid;
        }
    }
}

