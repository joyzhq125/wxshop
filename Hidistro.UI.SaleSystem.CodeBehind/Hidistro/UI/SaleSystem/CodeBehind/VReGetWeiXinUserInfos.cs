namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Members;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.Common.Controls;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using System;
    using System.IO;
    using System.Net;
    using System.Text;
    using System.Web;
    using System.Web.UI;

    [ParseChildren(true)]
    public class VReGetWeiXinUserInfos : VshopTemplatedWebControl
    {
        protected override void AttachChildControls()
        {
            string userAgent = this.Page.Request.UserAgent;
            string str2 = Globals.RequestQueryStr("returnUrl");
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(true,wid);
            try
            {
                if ((currentMember != null) && (currentMember.Status == 1))
                {
                    if (userAgent.ToLower().Contains("micromessenger"))
                    {
                        if (masterSettings.IsValidationService)
                        {
                            string str3 = this.Page.Request.QueryString["code"];
                            if (!string.IsNullOrEmpty(str3))
                            {
                                string responseResult = this.GetResponseResult("https://api.weixin.qq.com/sns/oauth2/access_token?appid=" + masterSettings.WeixinAppId + "&secret=" + masterSettings.WeixinAppSecret + "&code=" + str3 + "&grant_type=authorization_code");
                                if (responseResult.Contains("access_token"))
                                {
                                    JObject obj2 = JsonConvert.DeserializeObject(responseResult) as JObject;
                                    string str5 = this.GetResponseResult("https://api.weixin.qq.com/sns/userinfo?access_token=" + obj2["access_token"].ToString() + "&openid=" + obj2["openid"].ToString() + "&lang=zh_CN");
                                    if (str5.Contains("nickname"))
                                    {
                                        JObject obj3 = JsonConvert.DeserializeObject(str5) as JObject;
                                        MemberProcessor.ReSetUserHead(currentMember.UserId.ToString(), Globals.UrlDecode(obj3["nickname"].ToString()), obj3["headimgurl"].ToString(), obj3["openid"].ToString());
                                        if (string.IsNullOrEmpty(str2))
                                        {
                                            this.Page.Response.Redirect(str2);
                                        }
                                        else
                                        {
                                            this.Page.Response.Redirect("MemberCenter.aspx");
                                        }
                                    }
                                    else
                                    {
                                        this.Page.Response.Write("获取微信用户信息失败");
                                    }
                                }
                                else
                                {
                                    this.Page.Response.Write("获取微信用户信息失败");
                                }
                            }
                            else if (!string.IsNullOrEmpty(this.Page.Request.QueryString["state"]))
                            {
                                this.Page.Response.Redirect(Globals.ApplicationPath + "/Default.aspx");
                            }
                            else
                            {
                                string url = "https://open.weixin.qq.com/connect/oauth2/authorize?appid=" + masterSettings.WeixinAppId + "&redirect_uri=" + Globals.UrlEncode(HttpContext.Current.Request.Url.ToString()) + "&response_type=code&scope=snsapi_userinfo&state=STATE#wechat_redirect";
                                this.Page.Response.Redirect(url);
                            }
                        }
                        else
                        {
                            this.Page.Response.Write("公众号未绑定!");
                        }
                    }
                    else
                    {
                        this.Page.Response.Write("请使用微信客户端打开!");
                    }
                }
                else
                {
                    HttpContext.Current.Response.Write("非正常访问，服务器拒绝服务！");
                }
            }
            catch (Exception)
            {
                this.Page.Response.Write("重新获取用户微信信息失败！");
            }
        }

        private string GetResponseResult(string url)
        {
            using (HttpWebResponse response = (HttpWebResponse) WebRequest.Create(url).GetResponse())
            {
                using (Stream stream = response.GetResponseStream())
                {
                    using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                    {
                        return reader.ReadToEnd();
                    }
                }
            }
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "skin-VReGetWeiXinUserInfos.html";
            }
            base.OnInit(e);
        }
    }
}

