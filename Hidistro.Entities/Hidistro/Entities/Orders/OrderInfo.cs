﻿namespace Hidistro.Entities.Orders
{
    using Hidistro.Entities.Promotions;
    using Hishop.Components.Validation.Validators;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Threading;

    public class OrderInfo
    {
        private Dictionary<string, LineItemInfo> lineItems;

        public static  event EventHandler<EventArgs> Closed;

        public static  event EventHandler<EventArgs> Created;

        public static  event EventHandler<EventArgs> Deliver;

        public static  event EventHandler<EventArgs> Payment;

        public static  event EventHandler<EventArgs> Refund;

        public OrderInfo()
        {
            this.OrderStatus = Hidistro.Entities.Orders.OrderStatus.WaitBuyerPay;
            this.RefundStatus = Hidistro.Entities.Orders.RefundStatus.None;
        }

        public bool CheckAction(OrderActions action)
        {
            if ((this.OrderStatus != Hidistro.Entities.Orders.OrderStatus.Finished) && (this.OrderStatus != Hidistro.Entities.Orders.OrderStatus.Closed))
            {
                switch (action)
                {
                    case OrderActions.BUYER_PAY:
                    case OrderActions.SUBSITE_SELLER_MODIFY_DELIVER_ADDRESS:
                    case OrderActions.SUBSITE_SELLER_MODIFY_PAYMENT_MODE:
                    case OrderActions.SUBSITE_SELLER_MODIFY_SHIPPING_MODE:
                    case OrderActions.SELLER_CONFIRM_PAY:
                    case OrderActions.SELLER_MODIFY_TRADE:
                    case OrderActions.SELLER_CLOSE:
                    case OrderActions.SUBSITE_SELLER_MODIFY_GIFTS:
                        return (this.OrderStatus == Hidistro.Entities.Orders.OrderStatus.WaitBuyerPay);

                    case OrderActions.BUYER_CONFIRM_GOODS:
                    case OrderActions.SELLER_FINISH_TRADE:
                        return (this.OrderStatus == Hidistro.Entities.Orders.OrderStatus.SellerAlreadySent);

                    case OrderActions.SELLER_SEND_GOODS:
                        if (this.OrderStatus == Hidistro.Entities.Orders.OrderStatus.BuyerAlreadyPaid)
                        {
                            return true;
                        }
                        if (this.OrderStatus != Hidistro.Entities.Orders.OrderStatus.WaitBuyerPay)
                        {
                            return false;
                        }
                        return (this.Gateway == "hishop.plugins.payment.podrequest");

                    case OrderActions.SELLER_REJECT_REFUND:
                        return ((this.OrderStatus == Hidistro.Entities.Orders.OrderStatus.BuyerAlreadyPaid) || (this.OrderStatus == Hidistro.Entities.Orders.OrderStatus.SellerAlreadySent));

                    case OrderActions.MASTER_SELLER_MODIFY_DELIVER_ADDRESS:
                    case OrderActions.MASTER_SELLER_MODIFY_PAYMENT_MODE:
                    case OrderActions.MASTER_SELLER_MODIFY_SHIPPING_MODE:
                    case OrderActions.MASTER_SELLER_MODIFY_GIFTS:
                        return ((this.OrderStatus == Hidistro.Entities.Orders.OrderStatus.WaitBuyerPay) || (this.OrderStatus == Hidistro.Entities.Orders.OrderStatus.BuyerAlreadyPaid));

                    case OrderActions.SUBSITE_CREATE_PURCHASEORDER:
                        if ((this.GroupBuyId <= 0) || (this.GroupBuyStatus != Hidistro.Entities.Promotions.GroupBuyStatus.Success))
                        {
                            return false;
                        }
                        return (this.OrderStatus == Hidistro.Entities.Orders.OrderStatus.BuyerAlreadyPaid);
                }
            }
            return false;
        }

        public string GetProductNames()
        {
            string productName = string.Empty;
            foreach (LineItemInfo info in this.LineItems.Values)
            {
                if (info.Type == 0)
                {
                    productName = info.ItemDescription;
                    break;
                }
            }
            if (this.LineItems.Count > 1)
            {
                productName += "...";
            }
            return productName;
        }

        public virtual decimal GetAdjustCommssion()
        {
            decimal num = 0M;
            foreach (LineItemInfo info in this.LineItems.Values)
            {
                if (info.Type == 0)
                {
                    num += info.ItemAdjustedCommssion;
                }
            }
            return num;
        }

        public decimal GetAmount()
        {
            decimal num = 0M;
            foreach (LineItemInfo info in this.LineItems.Values)
            {
                if (info.Type == 0)
                {
                    num += info.GetSubTotal();
                }
            }
            return num;
        }

        public virtual decimal GetCostPrice()
        {
            decimal num = 0M;
            foreach (LineItemInfo info in this.LineItems.Values)
            {
                num += info.ItemCostPrice * info.ShipmentQuantity;
            }
            return num;
        }

        public int GetGroupBuyProductQuantity()
        {
            if (this.GroupBuyId > 0)
            {
                foreach (LineItemInfo info in this.LineItems.Values)
                {
                    return info.Quantity;
                }
            }
            return 0;
        }

        public static string GetOrderItemStatusName(Hidistro.Entities.Orders.OrderStatus orderStatus)
        {
            string str = "";
            switch (orderStatus)
            {
                case Hidistro.Entities.Orders.OrderStatus.WaitBuyerPay:
                case Hidistro.Entities.Orders.OrderStatus.BuyerAlreadyPaid:
                case Hidistro.Entities.Orders.OrderStatus.SellerAlreadySent:
                case Hidistro.Entities.Orders.OrderStatus.Closed:
                case Hidistro.Entities.Orders.OrderStatus.Finished:
                    return str;

                case Hidistro.Entities.Orders.OrderStatus.ApplyForRefund:
                    return "退款中";

                case Hidistro.Entities.Orders.OrderStatus.ApplyForReturns:
                    return "退货中";

                case Hidistro.Entities.Orders.OrderStatus.ApplyForReplacement:
                    return "换货中";

                case Hidistro.Entities.Orders.OrderStatus.Refunded:
                    return "已退款";

                case Hidistro.Entities.Orders.OrderStatus.Returned:
                    return "已退货";

                case Hidistro.Entities.Orders.OrderStatus.History:
                    return str;
            }
            return str;
        }

        public static string GetOrderStatusName(Hidistro.Entities.Orders.OrderStatus orderStatus)
        {
            switch (orderStatus)
            {
                case Hidistro.Entities.Orders.OrderStatus.WaitBuyerPay:
                    return "等待付款";

                case Hidistro.Entities.Orders.OrderStatus.BuyerAlreadyPaid:
                    return "等待发货";

                case Hidistro.Entities.Orders.OrderStatus.SellerAlreadySent:
                    return "已发货";

                case Hidistro.Entities.Orders.OrderStatus.Closed:
                    return "已关闭";

                case Hidistro.Entities.Orders.OrderStatus.Finished:
                    return "交易成功";

                case Hidistro.Entities.Orders.OrderStatus.ApplyForRefund:
                    return "申请退款";

                case Hidistro.Entities.Orders.OrderStatus.ApplyForReturns:
                    return "申请退货";

                case Hidistro.Entities.Orders.OrderStatus.ApplyForReplacement:
                    return "申请换货";

                case Hidistro.Entities.Orders.OrderStatus.Refunded:
                    return "已退款";

                case Hidistro.Entities.Orders.OrderStatus.Returned:
                    return "已退货";

                case Hidistro.Entities.Orders.OrderStatus.History:
                    return "历史订单";
            }
            return "-";
        }

        public virtual decimal GetProfit()
        {
            return ((this.GetTotal() - this.RefundAmount) - this.GetCostPrice());
        }

        public decimal GetReturnAmount()
        {
            decimal num = 0M;
            foreach (LineItemInfo info in this.LineItems.Values)
            {
                if ((info.Type == 0) && ((info.OrderItemsStatus == Hidistro.Entities.Orders.OrderStatus.Returned) || (info.OrderItemsStatus == Hidistro.Entities.Orders.OrderStatus.Refunded)))
                {
                    num += info.ReturnMoney;
                }
            }
            return num;
        }

        public virtual decimal GetSecondTotalCommssion()
        {
            decimal num = 0M;
            foreach (LineItemInfo info in this.LineItems.Values)
            {
                if ((info.OrderItemsStatus != Hidistro.Entities.Orders.OrderStatus.Refunded) && (info.OrderItemsStatus != Hidistro.Entities.Orders.OrderStatus.Returned))
                {
                    num += info.SecondItemsCommission;
                }
            }
            return num;
        }

        public virtual decimal GetThirdTotalCommssion()
        {
            decimal num = 0M;
            foreach (LineItemInfo info in this.LineItems.Values)
            {
                if ((info.OrderItemsStatus != Hidistro.Entities.Orders.OrderStatus.Refunded) && (info.OrderItemsStatus != Hidistro.Entities.Orders.OrderStatus.Returned))
                {
                    num += info.ThirdItemsCommission;
                }
            }
            return num;
        }

        public decimal GetTotal()
        {
            decimal num = 0M;
            num = this.GetAmount() - this.GetTotalDiscountAverage();
            num -= this.GetReturnAmount();
            num -= this.GetAdjustCommssion();
            return (num + this.AdjustedFreight);
        }

        public virtual decimal GetTotalCommssion()
        {
            decimal num = 0M;
            foreach (LineItemInfo info in this.LineItems.Values)
            {
                if ((info.OrderItemsStatus != Hidistro.Entities.Orders.OrderStatus.Refunded) && (info.OrderItemsStatus != Hidistro.Entities.Orders.OrderStatus.Returned))
                {
                    num += info.ItemsCommission;
                }
            }
            return num;
        }

        public virtual decimal GetTotalDiscountAverage()
        {
            decimal num = 0M;
            foreach (LineItemInfo info in this.LineItems.Values)
            {
                num += info.DiscountAverage;
            }
            return num;
        }

        public void OnClosed()
        {
            if (Closed != null)
            {
                Closed(this, new EventArgs());
            }
        }

        public static void OnClosed(OrderInfo order)
        {
            if (Closed != null)
            {
                Closed(order, new EventArgs());
            }
        }

        public void OnCreated()
        {
            if (Created != null)
            {
                Created(this, new EventArgs());
            }
        }

        public static void OnCreated(OrderInfo order)
        {
            if (Created != null)
            {
                Created(order, new EventArgs());
            }
        }

        public void OnDeliver()
        {
            if (Deliver != null)
            {
                Deliver(this, new EventArgs());
            }
        }

        public static void OnDeliver(OrderInfo order)
        {
            if (Deliver != null)
            {
                Deliver(order, new EventArgs());
            }
        }

        public void OnPayment()
        {
            if (Payment != null)
            {
                Payment(this, new EventArgs());
            }
        }

        public static void OnPayment(OrderInfo order)
        {
            if (Payment != null)
            {
                Payment(order, new EventArgs());
            }
        }

        public void OnRefund()
        {
            if (Refund != null)
            {
                Refund(this, new EventArgs());
            }
        }

        public static void OnRefund(OrderInfo order)
        {
            if (Refund != null)
            {
                Refund(order, new EventArgs());
            }
        }

        public string ActivitiesId { get; set; }

        public string ActivitiesName { get; set; }

        public string Address { get; set; }

        public string wid { get; set; }

        [RangeValidator(typeof(decimal), "-10000000", RangeBoundaryType.Inclusive, "10000000", RangeBoundaryType.Inclusive, Ruleset="ValOrder", MessageTemplate="订单折扣不能为空，金额大小负1000万-1000万之间")]
        public decimal AdjustedDiscount { get; set; }

        public decimal AdjustedFreight { get; set; }

        public int BundlingID { get; set; }

        public int? BundlingNum { get; set; }

        public decimal BundlingPrice { get; set; }

        public string CellPhone { get; set; }

        public string CloseReason { get; set; }

        public int CountDownBuyId { get; set; }

        public decimal CouponAmount { get; set; }

        public string CouponCode { get; set; }

        public string CouponName { get; set; }

        public decimal CouponValue { get; set; }

        public decimal DiscountAmount { get; set; }

        public string EmailAddress { get; set; }

        public string ExpressCompanyAbb { get; set; }

        public string ExpressCompanyName { get; set; }

        public DateTime? FinishDate { get; set; }

        public decimal FirstCommission { get; set; }

        public decimal Freight { get; set; }

        public int FreightFreePromotionId { get; set; }

        public string FreightFreePromotionName { get; set; }

        public string Gateway { get; set; }

        public string GatewayOrderId { get; set; }

        public int GroupBuyId { get; set; }

        public Hidistro.Entities.Promotions.GroupBuyStatus GroupBuyStatus { get; set; }

        public string InvoiceTitle { get; set; }

        public bool IsFreightFree { get; set; }

        public bool IsPrinted { get; set; }

        public bool IsReduced { get; set; }

        public bool IsSendTimesPoint { get; set; }

        public Dictionary<string, LineItemInfo> LineItems
        {
            get
            {
                if (this.lineItems == null)
                {
                    this.lineItems = new Dictionary<string, LineItemInfo>();
                }
                return this.lineItems;
            }
        }

        public OrderMark? ManagerMark { get; set; }

        public string ManagerRemark { get; set; }

        public string ModeName { get; set; }

        public string MSN { get; set; }

        public decimal NeedPrice { get; set; }

        public string OldAddress { get; set; }

        public DateTime OrderDate { get; set; }

        public string OrderId { get; set; }

        public string OrderMarking { get; set; }

        public Hidistro.Entities.Orders.OrderStatus OrderStatus { get; set; }

        public decimal PayCharge { get; set; }

        public DateTime? PayDate { get; set; }

        public string PaymentType { get; set; }

        public int PaymentTypeId { get; set; }

        public int PointExchange { get; set; }

        public int Points { get; set; }

        public decimal PointToCash { get; set; }

        public string QQ { get; set; }

        public string RealModeName { get; set; }

        public string RealName { get; set; }

        public int RealShippingModeId { get; set; }

        public string RedPagerActivityName { get; set; }

        public decimal RedPagerAmount { get; set; }

        public int? RedPagerID { get; set; }

        public decimal RedPagerOrderAmountCanUse { get; set; }

        public decimal ReducedPromotionAmount { get; set; }

        public int ReducedPromotionId { get; set; }

        public string ReducedPromotionName { get; set; }

        public int ReferralUserId { get; set; }

        public decimal RefundAmount { get; set; }

        public string RefundRemark { get; set; }

        public Hidistro.Entities.Orders.RefundStatus RefundStatus { get; set; }

        public int RegionId { get; set; }

        public string Remark { get; set; }

        public decimal SecondCommission { get; set; }

        public string Sender { get; set; }

        public int SentTimesPointPromotionId { get; set; }

        public string SentTimesPointPromotionName { get; set; }

        public string ShipOrderNumber { get; set; }

        public DateTime? ShippingDate { get; set; }

        public int ShippingModeId { get; set; }

        public string ShippingRegion { get; set; }

        public string ShipTo { get; set; }

        public string ShipToDate { get; set; }

        public decimal Tax { get; set; }

        public string TelPhone { get; set; }

        public decimal ThirdCommission { get; set; }

        public decimal TimesPoint { get; set; }

        public decimal TotalPrice
        {
            get
            {
                return this.GetTotal();
            }
        }

        public int UserId { get; set; }

        public string Username { get; set; }

        public string Wangwang { get; set; }

        public decimal Weight
        {
            get
            {
                decimal num = 0M;
                foreach (LineItemInfo info in this.LineItems.Values)
                {
                    num += info.ItemWeight * info.ShipmentQuantity;
                }
                return num;
            }
            set
            {
            }
        }

        public string ZipCode { get; set; }
    }
}

