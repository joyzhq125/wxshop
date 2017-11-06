namespace Hidistro.UI.Web.Admin.vshop
{
    using ASPNET.WebControls;
    using Hidistro.ControlPanel.Store;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Web;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class HomeTopicSetting : AdminPage
    {
        protected ImageLinkButton btnDeleteAll;
        protected LinkButton btnFinish;
        protected Grid grdHomeTopics;
        protected HtmlInputHidden hdtopic;

        protected HomeTopicSetting() : base("", "")
        {
        }

        private void BindHomeProducts()
        {
            this.grdHomeTopics.DataSource = VShopHelper.GetHomeTopics();
            this.grdHomeTopics.DataBind();
        }

        private void btnDeleteAll_Click(object sender, EventArgs e)
        {
            if (VShopHelper.RemoveAllHomeTopics())
            {
                base.Response.Redirect(HttpContext.Current.Request.Url.ToString(), true);
            }
        }

        protected void btnFinish_Click(object sender, EventArgs e)
        {
            foreach (GridViewRow row in this.grdHomeTopics.Rows)
            {
                int result = 0;
                TextBox box = (TextBox) row.FindControl("txtSequence");
                if (int.TryParse(box.Text.Trim(), out result))
                {
                    VShopHelper.UpdateHomeTopicSequence(Convert.ToInt32(this.grdHomeTopics.DataKeys[row.DataItemIndex].Value), result);
                }
            }
            this.BindHomeProducts();
        }

        private void grdHomeProducts_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            if (VShopHelper.RemoveHomeTopic((int) this.grdHomeTopics.DataKeys[e.RowIndex].Value))
            {
                base.Response.Redirect(HttpContext.Current.Request.Url.ToString(), true);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.btnDeleteAll.Click += new EventHandler(this.btnDeleteAll_Click);
            this.grdHomeTopics.RowDeleting += new GridViewDeleteEventHandler(this.grdHomeProducts_RowDeleting);
            if (!this.Page.IsPostBack)
            {
                this.BindHomeProducts();
            }
        }
    }
}

