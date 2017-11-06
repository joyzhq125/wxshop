namespace Hidistro.UI.Web.Pay
{
    using Entities.Members;
    using Hidistro.ControlPanel.OutPay.App;
    using Hidistro.Entities.Orders;
    using Hidistro.SaleSystem.Vshop;
    using System;
    using System.Collections.Generic;
    using System.Web.UI;

    public class wap_alipay_notify_url : Page
    {
        protected string OrderId;
        protected List<OrderInfo> orderlist;

        private SortedDictionary<string, string> GetRequestPost()
        {
            int index = 0;
            SortedDictionary<string, string> dictionary = new SortedDictionary<string, string>();
            string[] allKeys = base.Request.Form.AllKeys;
            for (index = 0; index < allKeys.Length; index++)
            {
                dictionary.Add(allKeys[index], base.Request.Form[allKeys[index]]);
            }
            return dictionary;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            SortedDictionary<string, string> requestPost = this.GetRequestPost();
            if (requestPost.Count > 0)
            {
                Notify notify = new Notify();
                if (notify.Verify(requestPost, base.Request.Form["notify_id"], base.Request.Form["sign"]))
                {
                    string str = base.Request.Form["out_trade_no"];
                    string str2 = base.Request.Form["trade_no"];
                    switch (base.Request.Form["trade_status"])
                    {
                        case "TRADE_SUCCESS":
                        case "TRADE_FINISHED":
                            this.OrderId = str;
                            this.orderlist = ShoppingProcessor.GetOrderMarkingOrderInfo(this.OrderId);
                            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
                            if (this.orderlist.Count == 0)
                            {
                                base.Response.Write("success");
                                return;
                            }
                            foreach (OrderInfo info in this.orderlist)
                            {
                                info.GatewayOrderId = str2;
                            }
                            foreach (OrderInfo info2 in this.orderlist)
                            {
                                if (info2.OrderStatus == OrderStatus.BuyerAlreadyPaid)
                                {
                                    base.Response.Write("success");
                                    return;
                                }
                            }
                            foreach (OrderInfo info3 in this.orderlist)
                            {
                                if (info3.CheckAction(OrderActions.BUYER_PAY) && MemberProcessor.UserPayOrder(info3, currentMember.wid))
                                {
                                    info3.OnPayment();
                                    base.Response.Write("success");
                                }
                            }
                            break;
                    }
                    base.Response.Write("success");
                }
                else
                {
                    base.Response.Write("fail");
                }
            }
            else
            {
                base.Response.Write("无通知参数");
            }
        }
    }
}

