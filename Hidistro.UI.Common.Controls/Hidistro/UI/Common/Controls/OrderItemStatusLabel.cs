namespace Hidistro.UI.Common.Controls
{
    using Hidistro.Entities.Orders;
    using System;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class OrderItemStatusLabel : Literal
    {
        private string _DetailUrl = string.Empty;

        protected override void Render(HtmlTextWriter writer)
        {
            OrderStatus orderStatusCode = (OrderStatus) this.OrderStatusCode;
            if (!string.IsNullOrEmpty(this._DetailUrl) && (((orderStatusCode == OrderStatus.Refunded) || (orderStatusCode == OrderStatus.Returned)) || ((orderStatusCode == OrderStatus.ApplyForReturns) || (orderStatusCode == OrderStatus.ApplyForRefund))))
            {
                base.Text = "<a href='" + this._DetailUrl + "' target='_blank'>" + OrderInfo.GetOrderItemStatusName(orderStatusCode) + "</a>";
            }
            else
            {
                base.Text = OrderInfo.GetOrderItemStatusName(orderStatusCode);
            }
            base.Render(writer);
        }

        public string DetailUrl
        {
            get
            {
                return this._DetailUrl;
            }
            set
            {
                this._DetailUrl = value;
            }
        }

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

