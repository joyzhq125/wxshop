namespace Hidistro.UI.Web.Admin.WeiXin
{
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Store;
    using Hidistro.Entities.VShop;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Collections.Generic;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    [PrivilegeCheck(Privilege.ProductCategory)]
    public class MessageTemplets : AdminPage
    {
        protected Button btnSave;
        protected Button btnSaveSendSetting;
        protected Repeater rptList;
        protected TextBox txtManageOpenID;

        protected MessageTemplets() : base("m06", "wxp06")
        {
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            SF.BLL.sf_website bll;
            SF.Model.sf_website website;
            bll =  new SF.BLL.sf_website();
            website = bll.GetModelByWid(wid);

            SiteSettings masterSettings = SettingsManager.GetMasterSettings(false,wid);
            masterSettings.ManageOpenID = this.txtManageOpenID.Text.Trim();
            website.ManageOpenID = this.txtManageOpenID.Text.Trim();
            if (bll.Update(website))
            //SettingsManager.Save(masterSettings);
            {
                this.ShowMsg("保存成功", true);
            }
            else
            {
                this.ShowMsg("保存失败", false);
            }
        }

        private void btnSaveSendSetting_Click(object sender, EventArgs e)
        {
            List<MessageTemplate> templates = new List<MessageTemplate>();
            for (int i = 0; i < this.rptList.Items.Count; i++)
            {
                MessageTemplate item = new MessageTemplate {
                    MessageType = ((HiddenField) this.rptList.Items[i].FindControl("hdfMessageType")).Value,
                    SendWeixin = ((CheckBox) this.rptList.Items[i].FindControl("chkWeixinMessage")).Checked,
                    wid = this.wid
                };
                templates.Add(item);
            }
            VShopHelper.UpdateSettings(templates,this.wid);
            this.ShowMsg("保存设置成功", true);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            wid = GetCurWebId();
            if (string.IsNullOrEmpty(wid)) return;
            this.btnSaveSendSetting.Click += new EventHandler(this.btnSaveSendSetting_Click);
            if (!this.Page.IsPostBack)
            {
                if(VShopHelper.IsExistMessageTemplate(wid) <= 0)
                {
                    //增加默认模板
                    VShopHelper.AddMessageTemplate(wid);
                }
                this.rptList.DataSource = VShopHelper.GetMessageTemplates(wid);
                this.rptList.DataBind();
                SiteSettings masterSettings = SettingsManager.GetMasterSettings(false,wid);
                this.txtManageOpenID.Text = masterSettings.ManageOpenID;
            }
        }

        protected void rptList_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                HiddenField field = e.Item.FindControl("hdfMessageType") as HiddenField;
                field.Value = DataBinder.Eval(e.Item.DataItem, "MessageType").ToString();
            }
        }
    }
}

