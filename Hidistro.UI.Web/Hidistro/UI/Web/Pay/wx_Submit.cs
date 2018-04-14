namespace Hidistro.UI.Web.Pay
{
    using Hidistro.ControlPanel.Promotions;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Members;
    using Hidistro.Entities.Orders;
    using Hidistro.SaleSystem.Vshop;
    using Hishop.Weixin.Pay;
    using Hishop.Weixin.Pay.Domain;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Web.UI;

    public class wx_Submit : Page
    {
        public string pay_json = string.Empty;
        public int shareid;
        protected string wid;

        public string ConvertPayJson(PayRequestInfo req)
        {
            string str = "{";
            return (((((((str + "\"appId\":\"" + req.appId + "\",") + "\"timeStamp\":\"" + req.timeStamp + "\",") + "\"nonceStr\":\"" + req.nonceStr + "\",") + "\"package\":\"" + req.package + "\",") + "\"signType\":\"" + req.signType + "\",") + "\"paySign\":\"" + req.paySign + "\"") + "}");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            string str = base.Request.QueryString.Get("orderId");
            if (!string.IsNullOrEmpty(str))
            {
                List<OrderInfo> orderMarkingOrderInfo = ShoppingProcessor.GetOrderMarkingOrderInfo(str);
                if (orderMarkingOrderInfo.Count != 0)
                {
                    decimal num = 0M;
                    foreach (OrderInfo info in orderMarkingOrderInfo)
                    {
                        num += info.GetTotal();
                    }
                    PackageInfo package = new PackageInfo {
                        Body = str,
                        NotifyUrl = string.Format("http://{0}/pay/wx_Pay.aspx", base.Request.Url.Host),
                        OutTradeNo = str,
                        TotalFee = (int) (num * 100M)
                    };
                    if (package.TotalFee < 1M)
                    {
                        package.TotalFee = 1M;
                    }
                    string openId = "";
                    MemberInfo currentMember = MemberProcessor.GetCurrentMember();
                    if (currentMember != null)
                    {
                        openId = currentMember.OpenId;
                    }
                    package.OpenId = openId;
                    wid = currentMember.wid;
                    //SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
                    Chenduo.Model.sf_website website=new Chenduo.BLL.sf_website().GetModelByWid(wid);
                    //PayRequestInfo req = new PayClient(masterSettings.WeixinAppId, masterSettings.WeixinAppSecret, masterSettings.WeixinPartnerID, masterSettings.WeixinPartnerKey, masterSettings.WeixinPaySignKey).BuildPayRequest(package);
                    PayRequestInfo req = new PayClient(wid,website.appid, website.appsecret, website.weixin_pay_account, website.account_pay_key,"").BuildPayRequest(package);
                    this.pay_json = this.ConvertPayJson(req);
                    DataTable shareActivity = ShareActHelper.GetShareActivity();
                    int num2 = 0;
                    decimal num3 = 0M;
                    if (shareActivity.Rows.Count > 0)
                    {
                        for (int i = 0; i < shareActivity.Rows.Count; i++)
                        {
                            if ((num != 0M) && (num >= decimal.Parse(shareActivity.Rows[shareActivity.Rows.Count - 1]["MeetValue"].ToString())))
                            {
                                num2 = int.Parse(shareActivity.Rows[shareActivity.Rows.Count - 1]["Id"].ToString());
                                num3 = decimal.Parse(shareActivity.Rows[shareActivity.Rows.Count - 1]["MeetValue"].ToString());
                                break;
                            }
                            if ((num != 0M) && (num <= decimal.Parse(shareActivity.Rows[0]["MeetValue"].ToString())))
                            {
                                num2 = int.Parse(shareActivity.Rows[0]["Id"].ToString());
                                num3 = decimal.Parse(shareActivity.Rows[0]["MeetValue"].ToString());
                                break;
                            }
                            if ((num != 0M) && (num >= decimal.Parse(shareActivity.Rows[i]["MeetValue"].ToString())))
                            {
                                num2 = int.Parse(shareActivity.Rows[i]["Id"].ToString());
                                num3 = decimal.Parse(shareActivity.Rows[i]["MeetValue"].ToString());
                            }
                        }
                        if (num >= num3)
                        {
                            this.shareid = num2;
                        }
                    }
                }
            }
        }
    }
}

