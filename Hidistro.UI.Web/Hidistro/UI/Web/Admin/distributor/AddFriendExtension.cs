namespace Hidistro.UI.Web.Admin.distributor
{
    using Hidistro.ControlPanel.Sales;
    using Hidistro.Entities.Store;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Web.UI.WebControls;

    public class AddFriendExtension : AdminPage
    {
        protected Button btnSave;
        protected HiddenField hidpic;
        protected TextBox txtName;

        protected AddFriendExtension() : base("", "")
        {
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            FriendExtensionInfo query = new FriendExtensionInfo {
                ExensionImg = this.hidpic.Value,
                ExensiontRemark = this.txtName.Text.Trim()
            };
            if (ProductCommentHelper.InsertFriendExtension(query))
            {
                this.ShowMsg("保存成功", true);
                this.hidpic.Value = "";
                this.txtName.Text = "";
            }
            else
            {
                this.ShowMsg("保存失败", false);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.btnSave.Click += new EventHandler(this.btnSave_Click);
        }
    }
}

