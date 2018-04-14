namespace Hidistro.UI.Web.Admin.WeiXin
{
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Store;
    using Hidistro.UI.ControlPanel.Utility;
    using System;

    [PrivilegeCheck(Privilege.ProductCategory)]
    public class ValidationService : AdminPage
    {
        private string action;
        protected bool enableValidationService;
        private SiteSettings siteSettings;
        Chenduo.BLL.sf_website bll;
        Chenduo.Model.sf_website website;

        protected ValidationService() : base("m06", "wxp07")
        {
            this.siteSettings = SettingsManager.GetMasterSettings(false,wid);
            this.action = Globals.RequestFormStr("action");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            wid = GetCurWebId();
            if (string.IsNullOrEmpty(wid)) return;
            bll = new Chenduo.BLL.sf_website();
            website = bll.GetModelByWid(wid);


            this.siteSettings = SettingsManager.GetMasterSettings(false, wid);
            this.action = Globals.RequestFormStr("action");
            if (!base.IsPostBack && (this.action == "setenable"))
            {
                base.Response.Clear();
                base.Response.ContentType = "application/json";
                string s = "{\"type\":\"1\",\"tips\":\"操作成功！\"}";
                try
                {
                    this.siteSettings.IsValidationService = Globals.RequestFormNum("enable") == 1;
                    website.IsValidationService = Globals.RequestFormStr("enable");
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
            this.enableValidationService = this.siteSettings.IsValidationService;
        }
    }
}

