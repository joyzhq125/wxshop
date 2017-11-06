namespace Hidistro.UI.Web.Admin.Settings
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using System;
    using System.Web;
    using System.Web.SessionState;
    public class PayConfigHandler : IHttpHandler,IRequiresSessionState
    {
        protected string wid;
        public void ProcessRequest(HttpContext context)
        {
            wid = context.Session[DTKeys.SESSION_WEB_ID] as string;
            if (string.IsNullOrEmpty(wid))
            {
                return;
            }
            context.Response.ContentType = "text/plain";
            try
            {
                string str = context.Request["type"].ToString();
                //SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
                SF.BLL.sf_website bll = new SF.BLL.sf_website();
                SF.Model.sf_website website = bll.GetModelByWid(wid);
                switch (str)
                {
                    case "0":
                    {
                        string str2 = context.Request["name"].ToString();
                        string str3 = context.Request["card"].ToString();
                        string str4 = context.Request["bank"].ToString();
                        //masterSettings.OfflinePay_BankCard_Name = str2;
                        //masterSettings.OfflinePay_BankCard_CardNo = str3;
                        //masterSettings.OfflinePay_BankCard_BankName = str4;
                        break;
                    }
                    case "1":
                    {
                        string str5 = context.Request["mid"].ToString();
                        //masterSettings.OfflinePay_Alipay_id = str5;
                        break;
                    }
                    case "2":
                    {
                        string str6 = context.Request["content"].ToString();
                        //masterSettings.OffLinePayContent = str6;
                        website.OffLinePayContent = str6;
                        break;
                    }
                    case "3":
                    {
                        string str7 = context.Request["mid"].ToString();
                        string str8 = context.Request["key"].ToString();
                        //masterSettings.ShenPay_mid = str7;
                        //masterSettings.ShenPay_key = str8;
                        break;
                    }
                    case "4":
                    {
                        string str9 = context.Request["mid"].ToString();
                        string str10 = context.Request["name"].ToString();
                        string str11 = context.Request["pid"].ToString();
                        string str12 = context.Request["key"].ToString();
                        website.Alipay_mid = str9;
                        website.Alipay_mName = str10;
                        website.Alipay_Pid = str11;
                        website.Alipay_Key = str12;
                        break;
                    }
                    case "5":
                    {
                        string str13 = context.Request["appid"].ToString();
                        string str14 = context.Request["appsecret"].ToString();
                        string str15 = context.Request["mch_id"].ToString();
                        string str16 = context.Request["key"].ToString();
                        //masterSettings.WeixinAppId = str13;
                        //masterSettings.WeixinAppSecret = str14;
                        //masterSettings.WeixinPartnerID = str15;
                        //masterSettings.WeixinPartnerKey = str16;
                        website.appid = str13;
                        website.appsecret = str14;
                        website.weixin_pay_account = str15;
                        website.account_pay_key = str16;
                        break;
                    }
                    case "6":
                    {
                        string str17 = context.Request["mid"].ToString();
                        string str18 = context.Request["md5"].ToString();
                        string str19 = context.Request["des"].ToString();
                        //masterSettings.ChinaBank_mid = str17;
                        //masterSettings.ChinaBank_MD5 = str18;
                        //masterSettings.ChinaBank_DES = str19;
                        break;
                    }
                    case "7":
                    {
                        string str20 = context.Request["key"].ToString();
                        website.WeixinCertPassword = str20;
                        break;
                    }
                    case "-1":
                    {
                        bool flag = bool.Parse(context.Request["enable"].ToString());
                        //货到付款
                        website.EnablePodRequest = flag?"1":"0";
                        break;
                    }
                    case "-2":
                    {
                        bool flag2 = bool.Parse(context.Request["enable"].ToString());
                        website.EnableOffLineRequest = flag2 ? "1" : "0";
                        break;
                    }
                    case "-3":
                    {
                        bool flag3 = bool.Parse(context.Request["enable"].ToString());
                        //masterSettings.EnableWapShengPay = flag3;
                        break;
                    }
                    case "-4":
                    {
                        bool flag4 = bool.Parse(context.Request["enable"].ToString());
                        website.EnableAlipayRequest = flag4 ? "1" : "0";
                        break;
                    }
                    case "-5":
                    {
                        bool flag5 = bool.Parse(context.Request["enable"].ToString());
                        website.Enableweixinrequest = flag5 ? "1" : "0";
                        break;
                    }
                    case "-6":
                    {
                        bool flag6 = bool.Parse(context.Request["enable"].ToString());
                        //masterSettings.ChinaBank_Enable = flag6;
                        break;
                    }
                    case "-7":
                    {
                        bool flag7 = bool.Parse(context.Request["enable"].ToString());
                        website.EnableWeixinRed = flag7? "1" : "0";
                        break;
                    }
                }
                //SettingsManager.Save(masterSettings);
                bll.Update(website);
                context.Response.Write("保存成功");
            }
            catch (Exception exception)
            {
                context.Response.Write("保存失败！（" + exception.Message + ")");
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}

