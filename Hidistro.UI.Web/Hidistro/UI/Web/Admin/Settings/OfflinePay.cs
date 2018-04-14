namespace Hidistro.UI.Web.Admin.Settings
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.UI.ControlPanel.Utility;
    using Hidistro.UI.Web.hieditor.ueditor.controls;
    using System;
    using System.Web.UI.HtmlControls;

    public class OfflinePay : AdminPage
    {
        protected string _content;
        protected bool _enable;
        protected bool _podenable;
        protected ucUeditor fkContent;
        private SiteSettings siteSettings;
        protected HtmlForm thisForm;

        protected OfflinePay() : base("m09", "szp04")
        {
            //this._content = "";
            //this.siteSettings = SettingsManager.GetMasterSettings(false,wid);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            wid = GetCurWebId();
            if (string.IsNullOrEmpty(wid)) return;

            this._content = "";
            this.siteSettings = SettingsManager.GetMasterSettings(false, wid);

            if (!base.IsPostBack)
            {
                this.fkContent.Text = this.siteSettings.OffLinePayContent;
                this._podenable = this.siteSettings.EnablePodRequest;
            }
            this._enable = this.siteSettings.EnableOffLineRequest;
        }

        private void SaveData()
        {
            if (string.IsNullOrEmpty(this.fkContent.Text))
            {
                this.ShowMsg("请输入内容！", false);
            }
            Chenduo.BLL.sf_website bll = new Chenduo.BLL.sf_website();
            Chenduo.Model.sf_website website = bll.GetModelByWid(wid);

            //this.siteSettings.OffLinePayContent = this.fkContent.Text;
            //SettingsManager.Save(this.siteSettings);
            website.OffLinePayContent = this.fkContent.Text;
            if (bll.Update(website))
            {
                this.ShowMsgAndReUrl("保存成功", true, "OfflinePay.aspx");
            }
            else
            {
                this.ShowMsgAndReUrl("保存失败", false, "");
            }
        }

        protected void Unnamed_Click(object sender, EventArgs e)
        {
            this.SaveData();
        }
    }
}

