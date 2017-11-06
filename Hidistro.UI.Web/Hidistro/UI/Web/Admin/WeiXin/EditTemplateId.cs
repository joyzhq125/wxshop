namespace Hidistro.UI.Web.Admin.WeiXin
{
    using Hidistro.ControlPanel.Store;
    using Hidistro.Entities.VShop;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Web.UI.WebControls;

    public class EditTemplateId : AdminPage
    {
        protected Button btnSaveEmailTemplet;
        protected TextBox txtTemplateId;

        protected EditTemplateId() : base("m06", "wxp06")
        {
        }

        private void btnSaveEmailTemplet_Click(object sender, EventArgs e)
        {
            string str = this.txtTemplateId.Text.Trim();
            string messageType = base.Request["MessageType"];
            MessageTemplate messageTemplate = VShopHelper.GetMessageTemplate(messageType,wid);
            messageTemplate.WeixinTemplateId = str;
            if (string.IsNullOrEmpty(str))
            {
                messageTemplate.SendWeixin = false;
            }
            else
            {
                messageTemplate.SendWeixin = true;
            }
            try
            {
                VShopHelper.UpdateTemplate(messageTemplate,wid);
                this.ShowMsgAndReUrl("保存模板Id成功", true, "messagetemplets.aspx");
            }
            catch
            {
                this.ShowMsg("保存模板Id失败", false);
            }
        }

        private void InitShow()
        {
            string messageType = base.Request["MessageType"];
            MessageTemplate messageTemplate = VShopHelper.GetMessageTemplate(messageType,wid);
            this.txtTemplateId.Text = messageTemplate.WeixinTemplateId;
        }

        protected override void OnInitComplete(EventArgs e)
        {
            base.OnInitComplete(e);
            this.btnSaveEmailTemplet.Click += new EventHandler(this.btnSaveEmailTemplet_Click);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            wid = GetCurWebId();
            if (string.IsNullOrEmpty(wid)) return;
            if (!this.Page.IsPostBack)
            {
                this.InitShow();
            }
        }
    }
}

