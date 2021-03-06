﻿namespace Hidistro.UI.Web.Admin.WeiXin
{
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Store;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.IO;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    [PrivilegeCheck(Privilege.ProductCategory)]
    public class RedpackSetting : AdminPage
    {
        private string _dataPath;
        private string action;
        protected Button btnOK;
        protected bool enableWXRequest;
        protected FileUpload fileUploader;
        protected HtmlGenericControl labfilename;
        private SiteSettings siteSettings;
        protected TextBox txtCertPassword;

        Chenduo.BLL.sf_website bll;
        Chenduo.Model.sf_website website;

        protected RedpackSetting() : base("m06", "wxp09")
        {
            //this.siteSettings = SettingsManager.GetMasterSettings(false,wid);
            //this.action = Globals.RequestFormStr("action");
        }

        protected void btnOK_Click(object sender, EventArgs e)
        {
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(false,wid);
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
                website.WeixinCertPath = Path.Combine(this._dataPath, str);
            }
            masterSettings.WeixinCertPassword = this.txtCertPassword.Text;
            website.WeixinCertPassword = this.txtCertPassword.Text;
            //SettingsManager.Save(masterSettings);
            bll.Update(website);
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
            bll = new Chenduo.BLL.sf_website();
            website = bll.GetModelByWid(wid);

            this.siteSettings = SettingsManager.GetMasterSettings(false,wid);
            this.action = Globals.RequestFormStr("action");

            this._dataPath = this.Page.Request.MapPath("~/Pay/Cert");
            if (!base.IsPostBack)
            {
                if (this.action == "setenable")
                {
                    base.Response.Clear();
                    base.Response.ContentType = "application/json";
                    string s = "{\"type\":\"1\",\"tips\":\"操作成功！\"}";
                    try
                    {
                        this.siteSettings.EnableWeiXinRequest = Globals.RequestFormNum("enable") == 1;
                        website.Enableweixinrequest = Globals.RequestFormStr("enable");
                        //SettingsManager.Save(this.siteSettings);
                        bll.Update(website);
                    }
                    catch
                    {
                        s = "{\"type\":\"0\",\"tips\":\"操作失败！\"}";
                    }
                    base.Response.Write(s);
                    base.Response.End();
                }
                this.labfilename.InnerText = (this.siteSettings.WeixinCertPath != "") ? ("已上传：" + this.siteSettings.WeixinCertPath.Substring(this.siteSettings.WeixinCertPath.LastIndexOf(@"\") + 1, (this.siteSettings.WeixinCertPath.Length - this.siteSettings.WeixinCertPath.LastIndexOf(@"\")) - 1)) : "";
                this.txtCertPassword.Text = this.siteSettings.WeixinCertPassword;
            }
            this.enableWXRequest = this.siteSettings.EnableWeiXinRequest;
        }
    }
}

