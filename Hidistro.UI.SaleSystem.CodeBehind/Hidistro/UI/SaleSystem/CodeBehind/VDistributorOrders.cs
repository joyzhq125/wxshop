﻿namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.Core;
    using Hidistro.Entities.Orders;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Data;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    [ParseChildren(true)]
    public class VDistributorOrders : VMemberTemplatedWebControl
    {
        private Literal litallnum;
        private Literal litfinishnum;
        private VshopTemplatedRepeater vshoporders;

        protected override void AttachChildControls()
        {
            if (DistributorsBrower.GetUserIdDistributors(Globals.GetCurrentMemberUserId()).ReferralStatus != 0)
            {
                HttpContext.Current.Response.Redirect("MemberCenter.aspx");
            }
            else
            {
                this.vshoporders = (VshopTemplatedRepeater) this.FindControl("vshoporders");
                this.litfinishnum = (Literal) this.FindControl("litfinishnum");
                this.litallnum = (Literal) this.FindControl("litallnum");
                PageTitle.AddSiteNameTitle("店铺订单");
                int result = 0;
                int.TryParse(HttpContext.Current.Request.QueryString.Get("status"), out result);
                OrderQuery query = new OrderQuery {
                    UserId = new int?(Globals.GetCurrentMemberUserId())
                };
                if (result != 5)
                {
                    query.Status = OrderStatus.Finished;
                    this.litfinishnum.Text = DistributorsBrower.GetDistributorOrderCount(query).ToString();
                    query.Status = OrderStatus.All;
                    this.litallnum.Text = DistributorsBrower.GetDistributorOrderCount(query).ToString();
                }
                else
                {
                    this.litallnum.Text = DistributorsBrower.GetDistributorOrderCount(query).ToString();
                    query.Status = OrderStatus.Finished;
                    this.litfinishnum.Text = DistributorsBrower.GetDistributorOrderCount(query).ToString();
                }
                this.vshoporders.ItemDataBound += new RepeaterItemEventHandler(this.vshoporders_ItemDataBound);
                this.vshoporders.DataSource = DistributorsBrower.GetDistributorOrder(query);
                this.vshoporders.DataBind();
            }
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-DistributorOrders.html";
            }
            base.OnInit(e);
        }

        private void vshoporders_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            DataTable table = ((DataView) DataBinder.Eval(e.Item.DataItem, "OrderItems")).ToTable();
            object obj2 = table.Compute("sum(ItemAdjustedCommssion)", "OrderItemsStatus<>9 and OrderItemsStatus<>10 and IsAdminModify=0");
            decimal num = (obj2 != DBNull.Value) ? ((decimal) obj2) : 0M;
            obj2 = table.Compute("sum(itemsCommission)", "OrderItemsStatus<>9 and OrderItemsStatus<>10");
            decimal num2 = (obj2 != DBNull.Value) ? ((decimal) obj2) : 0M;
            Literal literal = (Literal) e.Item.Controls[0].FindControl("litCommission");
            literal.Text = (num2 - num).ToString("F2");
        }
    }
}

