namespace Hidistro.UI.Web.Pay
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Orders;
    using Hidistro.SaleSystem.Vshop;
    using Hishop.Weixin.Pay;
    using Hishop.Weixin.Pay.Notify;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Web.UI;

    public class wx_Pay : Page
    {
        //protected string OrderId;
        protected List<OrderInfo> orderlist;
        protected string wid = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            //StreamWriter writer = File.AppendText(Server.MapPath("~/log/" + DateTime.Now.Ticks + "WxNotify.txt"));
            try
            {
                //NotifyClient notifyClient = new NotifyClient();
                //PayNotify payNotify = notifyClient.GetPayNotify(base.Request.InputStream);
                //PayNotify payNotify = new NotifyClient(masterSettings.WeixinAppId, masterSettings.WeixinAppSecret, masterSettings.WeixinPartnerID, masterSettings.WeixinPartnerKey, masterSettings.WeixinPaySignKey).GetPayNotify(base.Request.InputStream);
                //StreamWriter writer = File.AppendText(Server.MapPath("~/log/" + DateTime.Now.Ticks + "WxNotify.txt"));
                //writer.WriteLine("wx_pay");
                Log.Debug(this.GetType().ToString(), "wx_Pay>>");
                ResponseHandler resHandler = new ResponseHandler(Context);
                RequestHandler res = new RequestHandler(null);
                if (resHandler != null)
                {
                    //wid = payNotify.PayInfo.Attach;
                    //PayAccount account = new PayAccount
                    //{
                    //    AppId = website.appid,
                    //    AppSecret = website.appsecret,
                    //    PartnerId = website.weixin_pay_account,
                    //    PartnerKey = website.account_pay_key,
                    //    PaySignKey = ""
                    //};
                    //notifyClient._payAccount = account;

                    //writer.WriteLine("out_trade_no_1:"+ payNotify.out_trade_no);
                    //writer.WriteLine("paykey:" + website.account_pay_key);
                    //writer.WriteLine("wid:"+wid);

                    //string servicesign = "";
                    //if (!notifyClient.ValidPaySign(payNotify, out servicesign))
                    //{
                    //    writer.WriteLine("signerror,servicesign:"+ servicesign);
                    //    writer.Flush();
                    //    writer.Close();
                    //    return;
                    //}

                    //this.OrderId = payNotify.PayInfo.OutTradeNo;


                    //1.判断return_code
                    if (resHandler.GetParameter("return_code").ToUpper() != "SUCCESS")
                    {
                        Log.Error(this.GetType().ToString(), "wx_Pay>> return_code!=SUCCESS");
                        //writer.WriteLine("wx_pay return_code!=SUCCESS");
                        //writer.WriteLine(DateTime.Now);
                        //writer.Flush();
                        //writer.Close();


                        res.SetParameter("return_code", "FAIL");
                        res.SetParameter("return_msg", "return_code fail");
                        Response.Write(res.ParseXML());
                        Response.End();
                        return;
                    }
                    //2.判断result_code
                    if (resHandler.GetParameter("result_code").ToUpper() != "SUCCESS")
                    {
                        // 支付失败
                        //res.SetParameter("return_code", "FAIL");
                        //res.SetParameter("return_msg", "result_code fail");


                        //writer.WriteLine("wx_pay result_code!=SUCCESS");
                        //writer.WriteLine(DateTime.Now);
                        //writer.Flush();
                        //writer.Close();

                        Log.Error(this.GetType().ToString(), "wx_Pay>> result_code!=SUCCESS");

                        res.SetParameter("return_code", "SUCCESS");
                        res.SetParameter("return_msg", "");
                        Response.Write(res.ParseXML());
                        Response.End();
                        return;
                    }

                    //交易成功
                    string wid = resHandler.GetParameter("attach");
                    SF.Model.sf_website website = new SF.BLL.sf_website().GetModelByWid(wid);
                    resHandler.SetKey(website.account_pay_key);

                    //writer.WriteLine("wx_pay wid:" + wid);
                    Log.Debug(this.GetType().ToString(), "wx_Pay>> wid:" + wid);

                    //3.判断签名
                    if (resHandler.IsTenpaySign())
                    {
                        string ls_appid = resHandler.GetParameter("appid");
                        string ls_mch_id = resHandler.GetParameter("mch_id");
                        string ls_nonce_str = resHandler.GetParameter("nonce_str");
                        string ls_sign = resHandler.GetParameter("sign");
                        string ls_openid = resHandler.GetParameter("openid");
                        string ls_is_subscribe = resHandler.GetParameter("is_subscribe");
                        string ls_trade_type = resHandler.GetParameter("trade_type");
                        string ls_bank_type = resHandler.GetParameter("bank_type");
                        
                        int li_total_fee = int.Parse(resHandler.GetParameter("total_fee"));
                        string ls_transaction_id = resHandler.GetParameter("transaction_id");
                        string ls_out_trade_no = resHandler.GetParameter("out_trade_no");
                        string ls_time_end = resHandler.GetParameter("time_end");

                        //writer.WriteLine("wx_pay out_trade_no:" + ls_out_trade_no);
                        Log.Debug(this.GetType().ToString(), "wx_Pay>> out_trade_no:" + ls_out_trade_no);

                        //this.orderlist = ShoppingProcessor.GetOrderMarkingOrderInfo(ls_out_trade_no);
                        //if (this.orderlist.Count == 0)
                        //{
                        //    //base.Response.Write("success");
                        //}
                        //else
                        //{
                        //    foreach (OrderInfo info in this.orderlist)
                        //    {
                        //        info.GatewayOrderId = ls_transaction_id;//payNotify.PayInfo.TransactionId;
                        //    }
                        //    this.UserPayOrder();
                        //}

                        //wxOrderTmpMgr Totbll = wxOrderTmpMgr.instance();
                        string ret = new wxOrderTmpMgr().ProcessPaySuccess_wx(wid,ls_out_trade_no, ls_transaction_id);
                    }
                    else
                    {
                        //writer.WriteLine("签名失败");
                        Log.Error(this.GetType().ToString(), "wx_Pay>> 签名失败");
                    }
                    res.SetParameter("return_code", "SUCCESS");
                    res.SetParameter("return_msg", "");

                }
                //writer.WriteLine(DateTime.Now);
                //writer.Flush();
                //writer.Close();

                Response.Write(res.ParseXML());
                Response.End();
            }
            catch (System.Threading.ThreadAbortException)
            {
            }
            catch (Exception exception)
            {
                //StreamWriter writerexption = File.AppendText(Server.MapPath("~/log/" + DateTime.Now.Ticks + "WxNotifyExp.txt"));
                //writerexption.WriteLine(exception.Message);
                //writerexption.WriteLine(exception.StackTrace);
                //writerexption.WriteLine(DateTime.Now);
                //writerexption.Flush();
                //writerexption.Close();

                //writer.WriteLine(DateTime.Now);
                //writer.Flush();
                //writer.Close();

                Log.Error(this.GetType().ToString(), "wx_Pay>> exception:"+ exception.Message+" stackTrace:"+ exception.StackTrace);

            }
        }

        //private void UserPayOrder()
        //{
        //    foreach (OrderInfo info in this.orderlist)
        //    {
        //        if (info.OrderStatus == OrderStatus.BuyerAlreadyPaid)
        //        {
        //            //base.Response.Write("success");
        //            return;
        //        }
        //    }
        //    foreach (OrderInfo info2 in this.orderlist)
        //    {
        //        if (info2.CheckAction(OrderActions.BUYER_PAY) && MemberProcessor.UserPayOrder(info2,wid))
        //        {
        //            info2.OnPayment();
        //            //base.Response.Write("success");
        //        }
        //    }
        //}
    }
}

