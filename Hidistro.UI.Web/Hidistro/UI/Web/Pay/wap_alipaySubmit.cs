namespace Hidistro.UI.Web.Pay
{
    using Hidistro.ControlPanel.OutPay.App;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Orders;
    using Hidistro.SaleSystem.Vshop;
    using System;
    using System.Collections.Generic;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;

    public class wap_alipaySubmit : Page
    {
        protected HtmlGenericControl infos;
        public string wid = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            string str = base.Request.QueryString.Get("orderId");
            if (string.IsNullOrEmpty(str))
            {
                this.infos.InnerText = "订单号为空，请返回";
            }
            else
            {
                List<OrderInfo> orderMarkingOrderInfo = ShoppingProcessor.GetOrderMarkingOrderInfo(str);
                if (orderMarkingOrderInfo.Count == 0)
                {
                    this.infos.InnerText = "订单信息未找到！";
                }
                else
                {
                    decimal num = 0M;
                    foreach (OrderInfo info in orderMarkingOrderInfo)
                    {
                        num += info.GetTotal();
                    }
                    SiteSettings masterSettings = SettingsManager.GetMasterSettings(false,wid);
                    string partner = masterSettings.Alipay_Pid;
                    string str3 = masterSettings.Alipay_Key;
                    string str4 = "utf-8";
                    Core.setConfig(partner, "MD5", str3, str4);
                    string str5 = "1";
                    string str6 = Globals.FullPath("/Pay/wap_alipay_return_url.aspx");
                    string str7 = Globals.FullPath("/Pay/wap_alipay_notify_url.aspx");
                    string str8 = str;
                    string str9 = "订单支付";
                    decimal num2 = num;
                    string str10 = Globals.FullPath("/Vshop/MemberOrderDetails.aspx?orderId=") + orderMarkingOrderInfo[0].OrderId;
                    string str11 = "订单号-" + orderMarkingOrderInfo[0].OrderId + " ...";
                    string str12 = "1m";
                    string str13 = "";
                    SortedDictionary<string, string> sParaTemp = new SortedDictionary<string, string>();
                    sParaTemp.Add("partner", partner);
                    sParaTemp.Add("seller_id", partner);
                    sParaTemp.Add("_input_charset", str4);
                    sParaTemp.Add("service", "alipay.wap.create.direct.pay.by.user");
                    sParaTemp.Add("payment_type", str5);
                    sParaTemp.Add("notify_url", str6);
                    sParaTemp.Add("return_url", str7);
                    sParaTemp.Add("out_trade_no", str8);
                    sParaTemp.Add("subject", str9);
                    sParaTemp.Add("total_fee", num2.ToString("F"));
                    sParaTemp.Add("show_url", str10);
                    sParaTemp.Add("body", str11);
                    sParaTemp.Add("it_b_pay", str12);
                    sParaTemp.Add("extern_token", str13);
                    string s = Core.BuildRequest(sParaTemp, "get", "确认");
                    base.Response.Write(s);
                }
            }
        }
    }
}

