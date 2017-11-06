namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.Core;
    using Hidistro.Entities.Orders;
    using Hidistro.Entities.Sales;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.Common.Controls;
    using Hishop.Plugins;
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Web;
    using System.Web.UI;

    [PersistChildren(false), ParseChildren(true)]
    public abstract class PaymentTemplatedWebControl : VshopTemplatedWebControl
    {
        protected decimal Amount;
        protected string Gateway;
        private readonly bool isBackRequest;
        protected PaymentNotify Notify;
        protected OrderInfo Order;
        protected string OrderId;
        protected List<OrderInfo> orderlist;

        public PaymentTemplatedWebControl(bool _isBackRequest)
        {
            this.isBackRequest = _isBackRequest;
        }

        protected override void CreateChildControls()
        {
            this.Controls.Clear();
            if (!this.isBackRequest)
            {
                if (!base.LoadHtmlThemedControl())
                {
                    throw new SkinNotFoundException(this.SkinPath);
                }
                this.AttachChildControls();
            }
            this.DoValidate();
        }

        protected abstract void DisplayMessage(string status);
        private void DoValidate()
        {
            NameValueCollection values2 = new NameValueCollection();
            values2.Add(this.Page.Request.Form);
            values2.Add(this.Page.Request.QueryString);
            NameValueCollection parameters = values2;
            this.Gateway = "hishop.plugins.payment.ws_wappay.wswappayrequest";
            this.Notify = PaymentNotify.CreateInstance(this.Gateway, parameters);
            if (this.isBackRequest)
            {
                this.Notify.ReturnUrl = Globals.FullPath("/pay/PaymentReturn_url.aspx") + "?" + this.Page.Request.Url.Query;
            }
            this.OrderId = this.Notify.GetOrderId();
            this.orderlist = ShoppingProcessor.GetOrderMarkingOrderInfo(this.OrderId);
            if (this.orderlist.Count != 0)
            {
                int modeId = 0;
                foreach (OrderInfo info in this.orderlist)
                {
                    this.Amount += info.GetAmount();
                    info.GatewayOrderId = this.Notify.GetGatewayOrderId();
                    modeId = info.PaymentTypeId;
                }
                PaymentModeInfo paymentMode = ShoppingProcessor.GetPaymentMode(modeId);
                if (paymentMode == null)
                {
                    this.ResponseStatus(true, "gatewaynotfound");
                }
                else
                {
                    this.Notify.Finished += new EventHandler<FinishedEventArgs>(this.Notify_Finished);
                    this.Notify.NotifyVerifyFaild += new EventHandler(this.Notify_NotifyVerifyFaild);
                    this.Notify.Payment += new EventHandler(this.Notify_Payment);
                    this.Notify.VerifyNotify(0x7530, HiCryptographer.Decrypt(paymentMode.Settings));
                }
            }
        }

        private void FinishOrder()
        {
            foreach (OrderInfo info in this.orderlist)
            {
                if (info.OrderStatus == OrderStatus.Finished)
                {
                    this.ResponseStatus(true, "success");
                    return;
                }
            }
            foreach (OrderInfo info2 in this.orderlist)
            {
                if (info2.CheckAction(OrderActions.BUYER_CONFIRM_GOODS) && MemberProcessor.ConfirmOrderFinish(info2))
                {
                    this.ResponseStatus(true, "success");
                }
                else
                {
                    this.ResponseStatus(false, "fail");
                }
            }
        }

        private void Notify_Finished(object sender, FinishedEventArgs e)
        {
            if (e.IsMedTrade)
            {
                this.FinishOrder();
            }
            else
            {
                this.UserPayOrder();
            }
        }

        private void Notify_NotifyVerifyFaild(object sender, EventArgs e)
        {
            this.ResponseStatus(false, "verifyfaild");
        }

        private void Notify_Payment(object sender, EventArgs e)
        {
            this.UserPayOrder();
        }

        private void ResponseStatus(bool success, string status)
        {
            if (this.isBackRequest)
            {
                this.Notify.WriteBack(HttpContext.Current, success);
            }
            else
            {
                this.DisplayMessage(status);
            }
        }

        private void UserPayOrder()
        {
            foreach (OrderInfo info in this.orderlist)
            {
                if (info.OrderStatus == OrderStatus.BuyerAlreadyPaid)
                {
                    this.ResponseStatus(true, "success");
                    return;
                }
            }
            foreach (OrderInfo info2 in this.orderlist)
            {
                if (info2.CheckAction(OrderActions.BUYER_PAY) && MemberProcessor.UserPayOrder(info2,wid))
                {
                    info2.OnPayment();
                    this.ResponseStatus(true, "success");
                }
                else
                {
                    this.ResponseStatus(false, "fail");
                }
            }
        }
    }
}

