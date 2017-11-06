namespace Hidistro.Entities.Orders
{
    using System;

    public enum OrderStatus
    {
        All = 0,
        ApplyForRefund = 6,
        ApplyForReplacement = 8,
        ApplyForReturns = 7,
        BuyerAlreadyPaid = 2, //待发货
        Closed = 4,
        Finished = 5, //订单完成
        History = 0x63,
        Refunded = 9,
        Returned = 10,
        SellerAlreadySent = 3, //已发货，确认收货
        Today = 11,
        WaitBuyerPay = 1 //等待付款
    }
}

