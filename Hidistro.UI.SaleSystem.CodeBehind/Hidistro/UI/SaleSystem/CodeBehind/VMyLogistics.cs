namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.Entities.Orders;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    [ParseChildren(true)]
    public class VMyLogistics : VMemberTemplatedWebControl
    {
        protected override void AttachChildControls()
        {
            string str = this.Page.Request.QueryString["orderId"];
            if (string.IsNullOrEmpty(str))
            {
                base.GotoResourceNotFound("");
            }
            OrderInfo orderInfo = ShoppingProcessor.GetOrderInfo(str);
            Literal literal = this.FindControl("litOrderID") as Literal;
            Literal control = this.FindControl("litCompanyName") as Literal;
            Literal literal3 = this.FindControl("litLogisticsNumber") as Literal;
            literal.Text = str;
            control.SetWhenIsNotNull(orderInfo.ExpressCompanyName);
            literal3.SetWhenIsNotNull(orderInfo.ShipOrderNumber);
            PageTitle.AddSiteNameTitle("我的物流");
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "skin-vMyLogistics.html";
            }
            base.OnInit(e);
        }
    }
}

