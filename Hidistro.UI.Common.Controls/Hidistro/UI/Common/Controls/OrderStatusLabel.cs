namespace Hidistro.UI.Common.Controls
{
    using Hidistro.Entities.Orders;
    using System;
    using System.Runtime.CompilerServices;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class OrderStatusLabel : Literal
    {
        protected override void Render(HtmlTextWriter writer)
        {
            OrderStatus orderStatusCode = (OrderStatus) this.OrderStatusCode;
            if (((this.Gateway != null) && (this.Gateway.ToString() == "hishop.plugins.payment.podrequest")) && (orderStatusCode == OrderStatus.WaitBuyerPay))
            {
                base.Text = "等待发货";
            }
            else
            {
                base.Text = OrderInfo.GetOrderStatusName(orderStatusCode);
            }
            base.Render(writer);
        }

        public object Gateway { get; set; }

        public object OrderStatusCode
        {
            get
            {
                return this.ViewState["OrderStatusCode"];
            }
            set
            {
                this.ViewState["OrderStatusCode"] = value;
            }
        }
    }
}

