namespace Hidistro.UI.Web.Admin.Shop
{
    using Hidistro.ControlPanel.Settings;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Store;
    using Hidistro.UI.ControlPanel.Utility;
    using Hishop.MeiQia.Api.Api;
    using Hishop.MeiQia.Api.Util;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Web.Security;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class SaleService : AdminPage
    {
        protected Button ChangePwd;
        protected bool enable;
        protected Repeater grdCustomers;
        protected HtmlInputHidden htxtRoleId;
        protected Button OpenAccount;
        protected TextBox txt_cphone;
        protected TextBox txt_cpwd;
        protected TextBox txt_id;
        protected TextBox txt_name;
        protected TextBox txt_phone;
        protected TextBox txt_pwd;
        protected TextBox txt_entid;

        protected SaleService() : base("m01", "dpp05")
        {
        }

        private void BindCustomers(string unit)
        {
            DataTable customers = CustomerServiceHelper.GetCustomers(unit);
            this.grdCustomers.DataSource = customers;
            this.grdCustomers.DataBind();
        }

        protected void ChangePwd_Click(object sender, EventArgs e)
        {
            CustomerServiceSettings masterSettings = CustomerServiceManager.GetMasterSettings(false);
            if (string.IsNullOrEmpty(this.txt_phone.Text))
            {
                this.ShowMsg("请输入手机号码！", false);
            }
            if (string.IsNullOrEmpty(this.txt_pwd.Text))
            {
                this.ShowMsg("请输入密码！", false);
            }
            string tokenValue = TokenApi.GetTokenValue(masterSettings.AppId, masterSettings.AppSecret);
            if (!string.IsNullOrEmpty(tokenValue))
            {
                SiteSettings settings2 = SettingsManager.GetMasterSettings(false,wid);
                string str2 = string.Empty;
                if (!string.IsNullOrEmpty(settings2.DistributorLogoPic))
                {
                    str2 = Globals.DomainName + settings2.DistributorLogoPic;
                }
                string str3 = FormsAuthentication.HashPasswordForStoringInConfigFile(this.txt_pwd.Text, "MD5").ToLower();
                Dictionary<string, string> parameters = new Dictionary<string, string>();
                parameters.Add("unit", this.txt_phone.Text);
                parameters.Add("password", str3);
                parameters.Add("unitname", settings2.SiteName);
                parameters.Add("activated", "1");
                parameters.Add("logo", str2);
                parameters.Add("url", "");
                parameters.Add("tel", settings2.ShopTel);
                parameters.Add("contact", "");
                parameters.Add("location", "");
                string str4 = EnterpriseApi.UpdateEnterprise(tokenValue, parameters);
                if (!string.IsNullOrWhiteSpace(str4))
                {
                    string jsonValue = Common.GetJsonValue(str4, "errcode");
                    string str6 = Common.GetJsonValue(str4, "errmsg");
                    if (jsonValue == "0")
                    {
                        masterSettings.password = this.txt_pwd.Text;
                        CustomerServiceManager.Save(masterSettings);
                        this.ShowMsg("修改密码成功！", true);
                    }
                    else
                    {
                        this.ShowMsg("修改密码失败！(" + str6 + ")", false);
                    }
                }
                else
                {
                    this.ShowMsg("修改密码失败！", false);
                }
                this.enable = settings2.EnableSaleService;
            }
            else
            {
                this.ShowMsg("获取access_token失败！", false);
            }
        }
        protected void SaveBtn_Click(object sender, EventArgs e)
        {
            Chenduo.BLL.sf_website bll = new Chenduo.BLL.sf_website();
            Chenduo.Model.sf_website website = bll.GetModelByWid(wid);
            if (string.IsNullOrEmpty(this.txt_entid.Text))
            {
                this.ShowMsg("请输入ent号码！", false);
            }
            website.entId = this.txt_entid.Text;
            if(bll.Update(website))
            {
                this.ShowMsg("保存成功", true);
            }
            else
            {
                this.ShowMsg("保存失败", false);
            }

        }
        protected void OpenAccount_Click(object sender, EventArgs e)
        {
            CustomerServiceSettings masterSettings = CustomerServiceManager.GetMasterSettings(false);
            if (string.IsNullOrEmpty(this.txt_phone.Text))
            {
                this.ShowMsg("请输入手机号码！", false);
            }
            if (string.IsNullOrEmpty(this.txt_pwd.Text))
            {
                this.ShowMsg("请输入密码！", false);
            }
            string tokenValue = TokenApi.GetTokenValue(masterSettings.AppId, masterSettings.AppSecret);
            if (!string.IsNullOrEmpty(tokenValue))
            {
                SiteSettings settings2 = SettingsManager.GetMasterSettings(false,wid);
                string str2 = string.Empty;
                if (!string.IsNullOrEmpty(settings2.DistributorLogoPic))
                {
                    str2 = Globals.DomainName + settings2.DistributorLogoPic;
                }
                string str3 = FormsAuthentication.HashPasswordForStoringInConfigFile(this.txt_pwd.Text, "MD5").ToLower();
                Dictionary<string, string> parameters = new Dictionary<string, string>();
                parameters.Add("unit", this.txt_phone.Text);
                parameters.Add("password", str3);
                parameters.Add("unitname", settings2.SiteName);
                parameters.Add("activated", "1");
                parameters.Add("logo", str2);
                parameters.Add("url", "");
                parameters.Add("tel", settings2.ShopTel);
                parameters.Add("contact", "");
                parameters.Add("location", "");
                string str4 = EnterpriseApi.CreateEnterprise(tokenValue, parameters);
                if (!string.IsNullOrWhiteSpace(str4))
                {
                    string jsonValue = Common.GetJsonValue(str4, "errcode");
                    string str6 = Common.GetJsonValue(str4, "errmsg");
                    if (jsonValue == "0")
                    {
                        string unitId = EnterpriseApi.GetUnitId(tokenValue, this.txt_phone.Text);
                        if (!string.IsNullOrEmpty(unitId))
                        {
                            masterSettings.unitid = unitId;
                            masterSettings.unit = this.txt_phone.Text;
                            masterSettings.password = this.txt_pwd.Text;
                            CustomerServiceManager.Save(masterSettings);
                            this.ShowMsg("开通主账号成功！", true);
                        }
                        else
                        {
                            this.ShowMsg("获取主账号Id失败！", false);
                        }
                    }
                    else
                    {
                        this.ShowMsg("开通主账号失败！(" + str6 + ")", false);
                    }
                }
                else
                {
                    this.ShowMsg("开通主账号失败！", false);
                }
                this.enable = settings2.EnableSaleService;
            }
            else
            {
                this.ShowMsg("获取access_token失败！", false);
            }
        }

        [PrivilegeCheck(Privilege.Summary)]
        protected void Page_Load(object sender, EventArgs e)
        {
            wid = GetCurWebId();
            if (string.IsNullOrEmpty(wid)) return;

            SiteSettings masterSettings = SettingsManager.GetMasterSettings(false, wid);
            this.enable = masterSettings.EnableSaleService;
            if (!this.Page.IsPostBack)
            {
                CustomerServiceSettings settings2 = CustomerServiceManager.GetMasterSettings(false);
                this.txt_phone.Text = settings2.unit;
                this.txt_pwd.Attributes["Value"] = settings2.password;
                if (!string.IsNullOrEmpty(settings2.unit))
                {
                    this.txt_phone.Enabled = false;
                    this.OpenAccount.Visible = false;
                    this.ChangePwd.Visible = false;
                }
                else
                {
                    this.txt_phone.Enabled = false;/*true;*/
                    this.OpenAccount.Visible = false;/*true;*/
                    this.ChangePwd.Visible = false;
                }
                //this.BindCustomers(settings2.unit);
                this.txt_entid.Text = masterSettings.entId;
            }
            
        }
    }
}

