namespace Hidistro.UI.Web.Admin.distributor
{
    using ASPNET.WebControls;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities.Members;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Collections.Specialized;
    using System.Globalization;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class DistributorList : AdminPage
    {
        protected Button btnSearchButton;
        private string CellPhone;
        protected DistributorGradeDropDownList DrGrade;
        protected DropDownList DrStatus;
        private string Grade;
        protected PageSize hrefPageSize;
        private string MicroSignal;
        protected Pager pager;
        private string RealName;
        protected Repeater reDistributor;
        private string Status;
        private string StoreName;
        protected TextBox txtCellPhone;
        protected TextBox txtMicroSignal;
        protected TextBox txtRealName;
        protected TextBox txtStoreName;

        protected DistributorList() : base("", "")
        {
            this.StoreName = "";
            this.Grade = "0";
            this.Status = "0";
            this.RealName = "";
            this.CellPhone = "";
            this.MicroSignal = "";
        }

        private void BindData()
        {
            DistributorsQuery entity = new DistributorsQuery {
                GradeId = int.Parse(this.Grade),
                StoreName = this.StoreName,
                CellPhone = this.CellPhone,
                RealName = this.RealName,
                MicroSignal = this.MicroSignal,
                ReferralStatus = int.Parse(this.Status),
                PageIndex = this.pager.PageIndex,
                PageSize = this.pager.PageSize,
                SortOrder = SortAction.Desc,
                SortBy = "userid"
            };
            Globals.EntityCoding(entity, true);
            DbQueryResult result = VShopHelper.GetDistributors(entity, null, null);
            this.reDistributor.DataSource = result.Data;
            this.reDistributor.DataBind();
            this.pager.TotalRecords = result.TotalRecords;
        }

        private void btnSearchButton_Click(object sender, EventArgs e)
        {
            this.ReBind(true);
        }

        private void LoadParameters()
        {
            if (!this.Page.IsPostBack)
            {
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["StoreName"]))
                {
                    this.StoreName = base.Server.UrlDecode(this.Page.Request.QueryString["StoreName"]);
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["Grade"]))
                {
                    this.Grade = base.Server.UrlDecode(this.Page.Request.QueryString["Grade"]);
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["Status"]))
                {
                    this.Status = base.Server.UrlDecode(this.Page.Request.QueryString["Status"]);
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["RealName"]))
                {
                    this.RealName = base.Server.UrlDecode(this.Page.Request.QueryString["RealName"]);
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["CellPhone"]))
                {
                    this.CellPhone = base.Server.UrlDecode(this.Page.Request.QueryString["CellPhone"]);
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["MicroSignal"]))
                {
                    this.MicroSignal = base.Server.UrlDecode(this.Page.Request.QueryString["MicroSignal"]);
                }
                this.DrStatus.SelectedValue = this.Status;
                this.txtStoreName.Text = this.StoreName;
                this.DrGrade.SelectedValue = new int?(int.Parse(this.Grade));
                this.txtCellPhone.Text = this.CellPhone;
                this.txtMicroSignal.Text = this.MicroSignal;
                this.txtRealName.Text = this.RealName;
            }
            else
            {
                this.Status = this.DrStatus.SelectedValue;
                this.StoreName = this.txtStoreName.Text;
                this.Grade = this.DrGrade.SelectedValue.ToString();
                this.CellPhone = this.txtCellPhone.Text;
                this.RealName = this.txtRealName.Text;
                this.MicroSignal = this.txtMicroSignal.Text;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.reDistributor.ItemDataBound += new RepeaterItemEventHandler(this.reDistributor_ItemDataBound);
            this.reDistributor.ItemCommand += new RepeaterCommandEventHandler(this.reDistributor_ItemCommand);
            this.btnSearchButton.Click += new EventHandler(this.btnSearchButton_Click);
            this.LoadParameters();
            if (!base.IsPostBack)
            {
                this.BindData();
                this.DrGrade.DataBind();
                this.DrGrade.SelectedValue = new int?(int.Parse(this.Grade));
            }
        }

        private void ReBind(bool isSearch)
        {
            NameValueCollection queryStrings = new NameValueCollection();
            queryStrings.Add("Grade", this.DrGrade.Text);
            queryStrings.Add("StoreName", this.txtStoreName.Text);
            queryStrings.Add("CellPhone", this.txtCellPhone.Text);
            queryStrings.Add("RealName", this.txtRealName.Text);
            queryStrings.Add("MicroSignal", this.txtMicroSignal.Text);
            queryStrings.Add("Status", this.DrStatus.SelectedValue);
            queryStrings.Add("pageSize", this.pager.PageSize.ToString(CultureInfo.InvariantCulture));
            if (!isSearch)
            {
                queryStrings.Add("pageIndex", this.pager.PageIndex.ToString(CultureInfo.InvariantCulture));
            }
            base.ReloadPage(queryStrings);
        }

        private void reDistributor_ItemCommand(object sender, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Frozen")
            {
                if (!DistributorsBrower.FrozenCommision(int.Parse(e.CommandArgument.ToString()), "1"))
                {
                    this.ShowMsg("冻结失败", false);
                    return;
                }
                this.ShowMsg("冻结成功", true);
                this.ReBind(true);
            }
            if (e.CommandName == "Thaw")
            {
                if (DistributorsBrower.FrozenCommision(int.Parse(e.CommandArgument.ToString()), "0"))
                {
                    this.ShowMsg("解冻成功", true);
                    this.ReBind(true);
                }
                else
                {
                    this.ShowMsg("解冻失败", false);
                }
            }
        }

        private void reDistributor_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                ImageLinkButton button = (ImageLinkButton) e.Item.FindControl("btnFrozen");
                if (((int) DataBinder.Eval(e.Item.DataItem, "ReferralStatus")) == 0)
                {
                    button.Text = "冻结";
                    button.CommandName = "Frozen";
                    button.DeleteMsg = "确定要冻结分销商";
                }
                else
                {
                    button.Text = "解冻";
                    button.CommandName = "Thaw";
                    button.DeleteMsg = "确定要解冻分销商";
                }
            }
        }
    }
}

