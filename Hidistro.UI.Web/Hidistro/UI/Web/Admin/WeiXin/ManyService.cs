namespace Hidistro.UI.Web.Admin.WeiXin
{
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Store;
    using Hidistro.UI.ControlPanel.Utility;
    using System;

    [PrivilegeCheck(Privilege.ProductCategory)]
    public class ManyService : AdminPage
    {
        private string action;
        protected bool enableManyService;
        private SiteSettings siteSettings;
        SF.BLL.sf_website bll;
        SF.Model.sf_website website;
        protected ManyService() : base("m06", "wxp05")
        {
            this.siteSettings = SettingsManager.GetMasterSettings(false,wid);
            this.action = Globals.RequestFormStr("action");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            wid = GetCurWebId();
            if (string.IsNullOrEmpty(wid)) return;
            bll = new SF.BLL.sf_website();
            website = bll.GetModelByWid(wid);

            if (!base.IsPostBack && (this.action == "setenable"))
            {
                base.Response.Clear();
                base.Response.ContentType = "application/json";
                string s = "{\"type\":\"1\",\"tips\":\"操作成功！\"}";
                try
                {
                    this.siteSettings.OpenManyService = Globals.RequestFormNum("enable") == 1;
                    website.OpenManyService = Globals.RequestFormStr("enable");
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
            this.enableManyService = this.siteSettings.OpenManyService;
        }
    }
}

