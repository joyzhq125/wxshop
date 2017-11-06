namespace Hidistro.UI.Web.Admin.tools
{
    using ASPNET.WebControls;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.VShop;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Collections.Generic;
    using System.Web.UI.WebControls;

    public class SendMessageTemplets : AdminPage
    {
        protected Button btnSave;
        protected Button btnSaveSendSetting;
        protected Grid grdEmailTemplets;
        protected TextBox txtManageOpenID;

        protected SendMessageTemplets() : base("", "")
        {
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(false,wid);
            masterSettings.ManageOpenID = this.txtManageOpenID.Text.Trim();
            SettingsManager.Save(masterSettings);
            this.ShowMsg("修改成功", true);
        }

        private void btnSaveSendSetting_Click(object sender, EventArgs e)
        {
            List<MessageTemplate> templates = new List<MessageTemplate>();
            foreach (GridViewRow row in this.grdEmailTemplets.Rows)
            {
                MessageTemplate item = new MessageTemplate();
                CheckBox box = (CheckBox) row.FindControl("chkWeixinMessage");
                item.SendWeixin = box.Checked;
                item.MessageType = (string) this.grdEmailTemplets.DataKeys[row.RowIndex].Value;
                templates.Add(item);
            }
            VShopHelper.UpdateSettings(templates,wid);
            this.ShowMsg("保存设置成功", true);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            wid = GetCurWebId();
            if (string.IsNullOrEmpty(wid)) return;
            this.btnSaveSendSetting.Click += new EventHandler(this.btnSaveSendSetting_Click);
            if (!this.Page.IsPostBack)
            {
                this.grdEmailTemplets.DataSource = VShopHelper.GetMessageTemplates(wid);
                this.grdEmailTemplets.DataBind();
                SiteSettings masterSettings = SettingsManager.GetMasterSettings(false,wid);
                this.txtManageOpenID.Text = masterSettings.ManageOpenID;
            }
        }
    }
}

