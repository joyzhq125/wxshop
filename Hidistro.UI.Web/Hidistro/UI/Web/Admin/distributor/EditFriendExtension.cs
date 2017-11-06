namespace Hidistro.UI.Web.Admin.distributor
{
    using Hidistro.ControlPanel.Sales;
    using Hidistro.Core;
    using Hidistro.Core.Enums;
    using Hidistro.Entities.Store;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Data;
    using System.IO;
    using System.Web.UI.WebControls;

    public class EditFriendExtension : AdminPage
    {
        protected Button btnSave;
        private int ExtensionId;
        protected HiddenField hidpic;
        protected HiddenField hidpicdel;
        protected TextBox txtName;

        protected EditFriendExtension() : base("", "")
        {
        }

        private void BindData()
        {
            FriendExtensionQuery entity = new FriendExtensionQuery {
                PageIndex = 1,
                PageSize = 1,
                SortOrder = SortAction.Desc,
                SortBy = "ExtensionId"
            };
            Globals.EntityCoding(entity, true);
            entity.ExtensionId = this.ExtensionId;
            DataTable data = new DataTable();
            data = (DataTable) ProductCommentHelper.FriendExtensionList(entity).Data;
            if (data.Rows.Count > 0)
            {
                this.txtName.Text = data.Rows[0]["ExensiontRemark"].ToString();
                this.hidpic.Value = data.Rows[0]["ExensionImg"].ToString();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            FriendExtensionInfo query = new FriendExtensionInfo {
                ExensionImg = this.hidpic.Value,
                ExensiontRemark = this.txtName.Text.Trim(),
                ExtensionId = int.Parse(base.Request.QueryString["ExtensionId"])
            };
            if (ProductCommentHelper.UpdateFriendExtension(query))
            {
                this.ShowMsg("修改成功", true);
                if (!string.IsNullOrEmpty(this.hidpicdel.Value))
                {
                    foreach (string str in this.hidpicdel.Value.Split(new char[] { '|' }))
                    {
                        string path = str;
                        path = base.Server.MapPath(path);
                        if (File.Exists(path))
                        {
                            File.Delete(path);
                        }
                    }
                }
                this.hidpicdel.Value = "";
            }
            else
            {
                this.ShowMsg("修改失败", false);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.btnSave.Click += new EventHandler(this.btnSave_Click);
            if (!base.IsPostBack && int.TryParse(base.Request.QueryString["ExtensionId"], out this.ExtensionId))
            {
                this.BindData();
            }
        }
    }
}

