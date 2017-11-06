namespace Hidistro.UI.Web.Admin.distributor
{
    using ASPNET.WebControls;
    using Hidistro.ControlPanel.Sales;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities.Store;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.IO;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class FriendExtension : AdminPage
    {
        protected Pager pager;
        protected Repeater reFriend;

        protected FriendExtension() : base("", "")
        {
        }

        private void BindData()
        {
            FriendExtensionQuery entity = new FriendExtensionQuery {
                PageIndex = this.pager.PageIndex,
                PageSize = this.pager.PageSize,
                SortOrder = SortAction.Desc,
                SortBy = "ExtensionId"
            };
            Globals.EntityCoding(entity, true);
            DbQueryResult result = ProductCommentHelper.FriendExtensionList(entity);
            this.reFriend.DataSource = result.Data;
            this.reFriend.DataBind();
            this.pager.TotalRecords = result.TotalRecords;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.reFriend.ItemCommand += new RepeaterCommandEventHandler(this.reFriend_ItemCommand);
            this.reFriend.ItemDataBound += new RepeaterItemEventHandler(this.reFriend_ItemDataBound);
            if (!this.Page.IsPostBack)
            {
                this.BindData();
            }
        }

        private void reFriend_ItemCommand(object sender, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Del")
            {
                if (ProductCommentHelper.DeleteFriendExtension(int.Parse(e.CommandArgument.ToString())) > 0)
                {
                    this.BindData();
                    this.ShowMsg("删除成功", true);
                    Literal literal = (Literal) e.Item.FindControl("Literal1");
                    string text = literal.Text;
                    if (!string.IsNullOrEmpty(text))
                    {
                        foreach (string str2 in text.Split(new char[] { '|' }))
                        {
                            string path = str2;
                            path = base.Server.MapPath(path);
                            if (File.Exists(path))
                            {
                                File.Delete(path);
                            }
                        }
                    }
                }
                else
                {
                    this.ShowMsg("删除失败", false);
                }
            }
        }

        private void reFriend_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                Literal literal = (Literal) e.Item.FindControl("ImgPic");
                if (!string.IsNullOrEmpty(DataBinder.Eval(e.Item.DataItem, "ExensionImg").ToString()))
                {
                    string[] strArray = DataBinder.Eval(e.Item.DataItem, "ExensionImg").ToString().Split(new char[] { '|' });
                    string str = "";
                    foreach (string str2 in strArray)
                    {
                        if (!string.IsNullOrEmpty(str2))
                        {
                            str = str + "<img src='" + str2 + "' width='60' height='60'/>";
                        }
                    }
                    literal.Text = str;
                }
            }
        }
    }
}

