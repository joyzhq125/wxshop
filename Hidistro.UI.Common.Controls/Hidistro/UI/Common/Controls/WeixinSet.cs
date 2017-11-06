namespace Hidistro.UI.Common.Controls
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hishop.Weixin.MP.Api;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Net.Security;
    using System.Runtime.InteropServices;
    using System.Security.Cryptography.X509Certificates;
    using System.Text;
    using System.Web.Security;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class WeixinSet : Literal
    {
        public string htmlAppID = string.Empty;
        public string htmlNonceStr = "QoN4FvGbxdTi7mnffL";
        public string htmlSignature = string.Empty;
        public string htmlstring1 = string.Empty;
        public string htmlTicket = string.Empty;
        public string htmlTimeStamp = string.Empty;
        public string htmlToken = string.Empty;
        public string wid = string.Empty;

        public bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true;
        }

        public static int ConvertDateTimeInt(DateTime time)
        {
            DateTime time2 = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(0x7b2, 1, 1));
            TimeSpan span = (TimeSpan) (time - time2);
            return (int) span.TotalSeconds;
        }

        public string DoGet(string url)
        {
            HttpWebRequest webRequest = this.GetWebRequest(url, "GET");
            webRequest.ContentType = "application/x-www-form-urlencoded;charset=utf-8";
            HttpWebResponse rsp = (HttpWebResponse) webRequest.GetResponse();
            Encoding encoding = Encoding.UTF8;
            return this.GetResponseAsString(rsp, encoding);
        }

        public string GetJsApi_ticket(string token)
        {
            string url = string.Format("https://api.weixin.qq.com/cgi-bin/ticket/getticket?access_token={0}&type=jsapi", token);
            string str2 = this.DoGet(url);
            if (!string.IsNullOrEmpty(str2))
            {
                Dictionary<string, string> dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(str2);
                if ((dictionary != null) && dictionary.ContainsKey("ticket"))
                {
                    return dictionary["ticket"];
                }
            }
            return string.Empty;
        }

        public string GetResponseAsString(HttpWebResponse rsp, Encoding encoding)
        {
            Stream responseStream = null;
            StreamReader reader = null;
            string str;
            try
            {
                responseStream = rsp.GetResponseStream();
                reader = new StreamReader(responseStream, encoding);
                str = reader.ReadToEnd();
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
                if (responseStream != null)
                {
                    responseStream.Close();
                }
                if (rsp != null)
                {
                    rsp.Close();
                }
            }
            return str;
        }

        public string GetSignature(string token, string timestamp, string nonce, out string str)
        {
            string str2 = this.Page.Request.Url.ToString();
            string str3 = this.GetJsApi_ticket(token);
            this.htmlTicket = str3;
            string str4 = "jsapi_ticket=" + str3;
            string str5 = "noncestr=" + nonce;
            string str6 = "timestamp=" + timestamp;
            string str7 = "url=" + str2;
            string[] strArray = new string[] { str4, str5, str6, str7 };
            str = string.Join("&", strArray);
            string str8 = str;
            return FormsAuthentication.HashPasswordForStoringInConfigFile(str, "SHA1").ToLower();
        }

        public static DateTime GetTime(string timeStamp)
        {
            DateTime time = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(0x7b2, 1, 1));
            long ticks = long.Parse(timeStamp + "0000000");
            TimeSpan span = new TimeSpan(ticks);
            return time.Add(span);
        }

        public string GetToken(string appid, string secret)
        {
            string url = string.Format("https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}", appid, secret);
            string str2 = this.DoGet(url);
            if (!string.IsNullOrEmpty(str2))
            {
                Dictionary<string, string> dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(str2);
                if ((dictionary != null) && dictionary.ContainsKey("access_token"))
                {
                    return dictionary["access_token"];
                }
            }
            return string.Empty;
        }

        public HttpWebRequest GetWebRequest(string url, string method)
        {
            int num = 0x186a0;
            HttpWebRequest request = null;
            if (url.Contains("https"))
            {
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(this.CheckValidationResult);
                request = (HttpWebRequest) WebRequest.CreateDefault(new Uri(url));
            }
            else
            {
                request = (HttpWebRequest) WebRequest.Create(url);
            }
            request.ServicePoint.Expect100Continue = false;
            request.Method = method;
            request.KeepAlive = true;
            request.UserAgent = "Hishop";
            request.Timeout = num;
            return request;
        }

        protected override void Render(HtmlTextWriter writer)
        {
            base.Text = "";
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(false,wid);
            this.htmlAppID = masterSettings.WeixinAppId;
            string weixinAppSecret = masterSettings.WeixinAppSecret;
            try
            {
                this.htmlToken = this.GetToken(this.htmlAppID, weixinAppSecret);
            }
            catch (Exception)
            {
            }
            this.htmlTimeStamp = ConvertDateTimeInt(DateTime.Now).ToString();
            this.htmlSignature = this.GetSignature(this.htmlToken, this.htmlTimeStamp, this.htmlNonceStr, out this.htmlstring1);
            MenuApi.GetMenus(TokenApi.GetToken(masterSettings.WeixinAppId, masterSettings.WeixinAppSecret));
            base.Text = "<script>wx.config({ debug: false,appId: '" + this.htmlAppID + "',timestamp: '" + this.htmlTimeStamp + "', nonceStr: '" + this.htmlNonceStr + "',signature: '" + this.htmlSignature + "',jsApiList: ['checkJsApi','onMenuShareTimeline','onMenuShareAppMessage','onMenuShareQQ','onMenuShareWeibo']});</script>";
            base.Render(writer);
        }
    }
}

