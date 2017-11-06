using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading;
using System.Runtime.CompilerServices;
using Hidistro.SaleSystem.Vshop;
using Hidistro.Entities.Orders;

namespace Hidistro.UI.Web.Pay
{
    /// <summary>
    /// 单例模式 处理订单
    /// </summary>
    public class wxOrderTmpMgr
    {
        static wxOrderTmpMgr uniCounter;
        private int totNum = 0;
        //private wxOrderTmpMgr()
        //{
        //    Thread.Sleep(400); //假设多线程的时候因某种原因阻塞400毫秒 
        //}
        [MethodImpl(MethodImplOptions.Synchronized)] //方法的同步属性 
        static public wxOrderTmpMgr instance()
        {
            if (null == uniCounter)
            {
                uniCounter = new wxOrderTmpMgr();
            }
            return uniCounter;
        }

        /// <summary>
        /// 【微支付】 订单付款成功，处理订单：1将订单的状态改成付款完成；
        ///
        public string ProcessPaySuccess_wx(string wid,string out_trade_no, string transaction_id)
        {
            List<OrderInfo> orderlist = ShoppingProcessor.GetOrderMarkingOrderInfo(out_trade_no);
            if (orderlist.Count == 0)
            {
                //base.Response.Write("success");
            }
            else
            {
                foreach (OrderInfo info in orderlist)
                {
                    info.GatewayOrderId = transaction_id;//payNotify.PayInfo.TransactionId;
                }
                UserPayOrder(wid,orderlist);
            }
            return "";

        }
        private void UserPayOrder(string wid,List<OrderInfo> orderlist)
        {
            foreach (OrderInfo info in orderlist)
            {
                if (info.OrderStatus == OrderStatus.BuyerAlreadyPaid)
                {
                    //base.Response.Write("success");
                    return;
                }
            }
            foreach (OrderInfo info2 in orderlist)
            {
                if (info2.CheckAction(OrderActions.BUYER_PAY) && MemberProcessor.UserPayOrder(info2, wid))
                {
                    info2.OnPayment();
                    //base.Response.Write("success");
                }
            }
        }


        public void Inc() { totNum++; }
        public int GetCounter() { return totNum; }
    }
}