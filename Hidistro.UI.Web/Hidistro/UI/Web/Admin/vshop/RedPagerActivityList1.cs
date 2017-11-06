namespace Hidistro.UI.Web.Admin.vshop
{
    using ASPNET.WebControls;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities.Commodities;
    using Hidistro.Entities.VShop;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Collections.Specialized;
    using System.Data;
    using System.Globalization;
    using System.Web.UI.WebControls;

    public class RedPagerActivityList1 : AdminPage
    {
        protected Button btnSearchButton;
        protected string LocalUrl;
        private string Name;
        protected Pager pager;
        protected Repeater rptList;
        protected TextBox txtName;

        protected RedPagerActivityList1() : base("", "")
        {
            this.Name = "";
            this.LocalUrl = string.Empty;
        }

        private void BindData()
        {
            RedPagerActivityQuery entity = new RedPagerActivityQuery {
                Name = this.Name,
                SortBy = "RedPagerActivityId",
                SortOrder = SortAction.Desc
            };
            Globals.EntityCoding(entity, true);
            entity.PageIndex = this.pager.PageIndex;
            entity.PageSize = this.pager.PageSize;
            DbQueryResult redPagerActivityRequest = RedPagerActivityBrower.GetRedPagerActivityRequest(entity);
            this.rptList.DataSource = redPagerActivityRequest.Data;
            this.rptList.DataBind();
            this.pager.TotalRecords = redPagerActivityRequest.TotalRecords;
        }

        private void btnSearchButton_Click(object sender, EventArgs e)
        {
            this.ReBind(true);
        }

        protected string FormatCommissionRise(object commissionrise)
        {
            decimal result = 0.00M;
            decimal.TryParse(commissionrise.ToString(), out result);
            if (result == 0.00M)
            {
                return "同类目佣金";
            }
            return ("+" + result + "%");
        }

        protected string GetCategoryName(object ocategoryid)
        {
            string name = "全部";
            int result = 0;
            int.TryParse(ocategoryid.ToString(), out result);
            CategoryInfo category = CategoryBrowser.GetCategory(result);
            if (category != null)
            {
                name = category.Name;
            }
            return name;
        }

        private void LoadParameters()
        {
            if (!this.Page.IsPostBack)
            {
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["Name"]))
                {
                    this.Name = base.Server.UrlDecode(this.Page.Request.QueryString["Name"]);
                }
                this.txtName.Text = this.Name;
            }
            else
            {
                this.Name = this.txtName.Text;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.LocalUrl = base.Server.UrlEncode(base.Request.Url.ToString());
            this.btnSearchButton.Click += new EventHandler(this.btnSearchButton_Click);
            this.LoadParameters();
            if (!base.IsPostBack)
            {
                this.BindData();
            }
        }

        private void ReBind(bool isSearch)
        {
            NameValueCollection queryStrings = new NameValueCollection();
            queryStrings.Add("Name", this.txtName.Text);
            queryStrings.Add("pageSize", this.pager.PageSize.ToString(CultureInfo.InvariantCulture));
            queryStrings.Add("pageIndex", this.pager.PageIndex.ToString(CultureInfo.InvariantCulture));
            base.ReloadPage(queryStrings);
        }

        protected void rptList_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            int result = 0;
            int.TryParse(e.CommandArgument.ToString(), out result);
            if (result > 0)
            {
                string commandName = e.CommandName;
                if (commandName != null)
                {
                    if (!(commandName == "del"))
                    {
                        if (commandName == "open")
                        {
                            RedPagerActivityBrower.SetIsOpen(result, true);
                        }
                        else if (commandName == "close")
                        {
                            RedPagerActivityBrower.SetIsOpen(result, false);
                        }
                    }
                    else
                    {
                        RedPagerActivityBrower.DelRedPagerActivity(result);
                    }
                }
                this.ReBind(true);
            }
        }

        protected void rptList_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                ImageButton button = (ImageButton) e.Item.FindControl("imgBtnIsOpen");
                if (((DataRowView) e.Item.DataItem).Row["IsOpen"].ToString() == "False")
                {
                    button.ImageUrl = "../images/ta.gif";
                    button.CommandName = "open";
                    button.ToolTip = "点击开启该代金券活动";
                }
                else
                {
                    button.ToolTip = "点击关闭该代金券活动";
                    button.CommandName = "close";
                    button.OnClientClick = @"return confirm('关闭代金券活动,不会影响该活动已经产生的代金券!\n确认要关闭该活动吗?')";
                }
            }
        }
    }
}

