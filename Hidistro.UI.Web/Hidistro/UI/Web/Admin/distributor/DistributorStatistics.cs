namespace Hidistro.UI.Web.Admin.distributor
{
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Core.Enums;
    using Hidistro.Entities.Members;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Data;
    using System.Web.UI.WebControls;

    public class DistributorStatistics : AdminPage
    {
        private int i;
        protected Repeater reDistributor;

        protected DistributorStatistics() : base("", "")
        {
        }

        private void BindData()
        {
            DistributorsQuery entity = new DistributorsQuery {
                GradeId = 0,
                StoreName = "",
                CellPhone = "",
                RealName = "",
                MicroSignal = "",
                ReferralStatus = 0,
                PageIndex = 1,
                PageSize = 15,
                SortOrder = SortAction.Desc,
                SortBy = "OrdersTotal"
            };
            Globals.EntityCoding(entity, true);
            DataTable data = (DataTable) VShopHelper.GetDistributors(entity, null, null).Data;
            this.reDistributor.DataSource = data;
            this.reDistributor.DataBind();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.reDistributor.ItemDataBound += new RepeaterItemEventHandler(this.reDistributor_ItemDataBound);
            if (!base.IsPostBack)
            {
                this.BindData();
            }
        }

        private void reDistributor_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                Literal literal = (Literal) e.Item.FindControl("litph");
                this.i++;
                if (this.i == 1)
                {
                    literal.Text = "<img src=\"../images/0001.gif\"></img>";
                }
                else if (this.i == 2)
                {
                    literal.Text = "<img src=\"../images/0002.gif\"></img>";
                }
                else if (this.i == 3)
                {
                    literal.Text = "<img src=\"../images/0003.gif\"></img>";
                }
                else
                {
                    literal.Text = "<span style=\"padding-left:10px;\">" + ((int.Parse(literal.Text) + this.i)).ToString() + "</span>";
                }
            }
        }
    }
}

