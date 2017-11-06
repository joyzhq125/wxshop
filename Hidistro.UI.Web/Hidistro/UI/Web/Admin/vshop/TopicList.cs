namespace Hidistro.UI.Web.Admin.vshop
{
    using ASPNET.WebControls;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities.VShop;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Web.UI.WebControls;

    public class TopicList : AdminPage
    {
        protected LinkButton Lksave;
        protected Pager pager;
        protected Repeater rpTopic;

        protected TopicList() : base("", "")
        {
        }

        protected void BindTopicList()
        {
            TopicQuery page = new TopicQuery {
                PageIndex = this.pager.PageIndex,
                PageSize = this.pager.PageSize,
                SortBy = "DisplaySequence",
                SortOrder = SortAction.Asc
            };
            DbQueryResult topicList = VShopHelper.GettopicList(page);
            this.rpTopic.DataSource = topicList.Data;
            this.rpTopic.DataBind();
            this.pager.TotalRecords = topicList.TotalRecords;
        }

        protected void Lksave_Click(object sender, EventArgs e)
        {
            foreach (RepeaterItem item in this.rpTopic.Items)
            {
                int result = 0;
                TextBox box = (TextBox) item.FindControl("txtSequence");
                if (int.TryParse(box.Text.Trim(), out result))
                {
                    Label label = (Label) item.FindControl("Lbtopicid");
                    int topicId = Convert.ToInt32(label.Text);
                    if (VShopHelper.Gettopic(topicId).DisplaySequence != result)
                    {
                        VShopHelper.SwapTopicSequence(topicId, result);
                    }
                }
            }
            this.BindTopicList();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                this.BindTopicList();
            }
        }

        protected void rpTopic_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "del")
            {
                int topicId = Convert.ToInt32(e.CommandArgument);
                if (VShopHelper.Deletetopic(topicId))
                {
                    VShopHelper.RemoveReleatesProductBytopicid(topicId);
                    this.ShowMsg("删除成功！", true);
                    this.BindTopicList();
                }
            }
        }
    }
}

