namespace Hidistro.UI.Web.Admin.vshop
{
    using ASPNET.WebControls;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class ManageNavigate : AdminPage
    {
        protected Grid grdNavigate;

        protected ManageNavigate() : base("", "")
        {
        }

        private void BindData()
        {
            this.grdNavigate.DataSource = VShopHelper.GetAllNavigate();
            this.grdNavigate.DataBind();
        }

        protected string GetImageUrl(string url)
        {
            string str = url;
            if (string.IsNullOrWhiteSpace(str))
            {
                return "/utility/pics/none.gif";
            }
            if (!url.ToLower().Contains("storage/master/navigate") && !url.ToLower().Contains("templates"))
            {
                str = Globals.GetVshopSkinPath(null) + "/images/deskicon/" + url;
            }
            return str;
        }

        protected void grdNavigate_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int rowIndex = ((GridViewRow) ((Control) e.CommandSource).NamingContainer).RowIndex;
            int bannerId = (int) this.grdNavigate.DataKeys[rowIndex].Value;
            int replaceBannerId = 0;
            if (e.CommandName == "Fall")
            {
                if (rowIndex < (this.grdNavigate.Rows.Count - 1))
                {
                    replaceBannerId = (int) this.grdNavigate.DataKeys[rowIndex + 1].Value;
                }
            }
            else if ((e.CommandName == "Rise") && (rowIndex > 0))
            {
                replaceBannerId = (int) this.grdNavigate.DataKeys[rowIndex - 1].Value;
            }
            if (replaceBannerId > 0)
            {
                VShopHelper.SwapTplCfgSequence(bannerId, replaceBannerId);
                base.ReloadPage(null);
            }
            if (e.CommandName == "Delete")
            {
                if (VShopHelper.DelTplCfg(bannerId))
                {
                    this.ShowMsg("删除成功！", true);
                    base.ReloadPage(null);
                }
                else
                {
                    this.ShowMsg("删除失败！", false);
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                this.BindData();
            }
        }
    }
}

