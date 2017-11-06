namespace Hidistro.UI.Web.Admin.vshop
{
    using ASPNET.WebControls;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Entities.VShop;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Web;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class SetTopicProducts : AdminPage
    {
        protected ImageLinkButton btnDeleteAll;
        protected LinkButton btnFinish;
        protected Grid grdTopicProducts;
        protected HtmlInputHidden hdtopic;
        protected Literal litPromotionName;
        private int topicId;

        protected SetTopicProducts() : base("", "")
        {
        }

        private void BindTopicProducts()
        {
            this.grdTopicProducts.DataSource = VShopHelper.GetRelatedTopicProducts(this.topicId);
            this.grdTopicProducts.DataBind();
        }

        private void btnDeleteAll_Click(object sender, EventArgs e)
        {
            if (VShopHelper.RemoveReleatesProductBytopicid(this.topicId))
            {
                base.Response.Redirect(HttpContext.Current.Request.Url.ToString(), true);
            }
        }

        protected void btnFinish_Click(object sender, EventArgs e)
        {
            foreach (GridViewRow row in this.grdTopicProducts.Rows)
            {
                int result = 0;
                TextBox box = (TextBox) row.FindControl("txtSequence");
                if (int.TryParse(box.Text.Trim(), out result))
                {
                    int relatedProductId = Convert.ToInt32(this.grdTopicProducts.DataKeys[row.DataItemIndex].Value);
                    VShopHelper.UpdateRelateProductSequence(this.topicId, relatedProductId, result);
                }
            }
            this.BindTopicProducts();
        }

        private void grdTopicProducts_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            if (VShopHelper.RemoveReleatesProductBytopicid(this.topicId, (int) this.grdTopicProducts.DataKeys[e.RowIndex].Value))
            {
                base.Response.Redirect(HttpContext.Current.Request.Url.ToString(), true);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!int.TryParse(this.Page.Request.QueryString["topicid"], out this.topicId))
            {
                base.GotoResourceNotFound();
            }
            else
            {
                this.hdtopic.Value = this.topicId.ToString();
                this.btnDeleteAll.Click += new EventHandler(this.btnDeleteAll_Click);
                this.grdTopicProducts.RowDeleting += new GridViewDeleteEventHandler(this.grdTopicProducts_RowDeleting);
                if (!this.Page.IsPostBack)
                {
                    TopicInfo topic = VShopHelper.Gettopic(this.topicId);
                    if (topic == null)
                    {
                        base.GotoResourceNotFound();
                    }
                    else
                    {
                        this.litPromotionName.Text = topic.Title;
                        this.BindTopicProducts();
                    }
                }
            }
        }
    }
}

