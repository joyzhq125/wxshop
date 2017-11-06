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
    using System.Data;
    using System.IO;
    using System.Net;
    using System.Text;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;

    [ParseChildren(true)]
    public class VUserLogining : VshopTemplatedWebControl
    {
        private HtmlInputHidden hidurl;

        protected override void AttachChildControls()
        {
            this.hidurl = (HtmlInputHidden) this.FindControl("hidurl");
            string userAgent = this.Page.Request.UserAgent;
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(true,wid);
            if (((currentMember == null) || (this.Page.Session["userid"] == null)) || (this.Page.Session["userid"].ToString() != currentMember.UserId.ToString()))
            {
                if (userAgent.ToLower().Contains("micromessenger"))
                {
                    if (masterSettings.IsValidationService)
                    {
                        string msg = this.Page.Request.QueryString["code"];
                        this.WriteError(msg, "code值");
                        if (!string.IsNullOrEmpty(msg))
                        {
                            string responseResult = this.GetResponseResult("https://api.weixin.qq.com/sns/oauth2/access_token?appid=" + masterSettings.WeixinAppId + "&secret=" + masterSettings.WeixinAppSecret + "&code=" + msg + "&grant_type=authorization_code");
                            if (responseResult.Contains("access_token"))
                            {
                                this.WriteError(responseResult, "access_token");
                                JObject obj2 = JsonConvert.DeserializeObject(responseResult) as JObject;
                                if (!this.HasLogin(obj2["openid"].ToString()))
                                {
                                    string str4 = this.GetResponseResult("https://api.weixin.qq.com/sns/userinfo?access_token=" + obj2["access_token"].ToString() + "&openid=" + obj2["openid"].ToString() + "&lang=zh_CN");
                                    if (str4.Contains("nickname"))
                                    {
                                        JObject obj3 = JsonConvert.DeserializeObject(str4) as JObject;
                                        string generateId = Globals.GetGenerateId();
                                        MemberInfo member = new MemberInfo {
                                            GradeId = MemberProcessor.GetDefaultMemberGrade(this.wid),
                                            UserName = Globals.UrlDecode(obj3["nickname"].ToString()),
                                            OpenId = obj3["openid"].ToString(),
                                            CreateDate = DateTime.Now,
                                            SessionId = generateId,
                                            SessionEndTime = DateTime.Now.AddDays(10),
                                            UserHead = obj3["headimgurl"].ToString(),
                                            Password = HiCryptographer.Md5Encrypt("888888")
                                        };
                                        MemberProcessor.CreateMember(member);
                                        MemberInfo info3 = MemberProcessor.GetMember(generateId);
                                        HttpCookie cookie = new HttpCookie("Vshop-Member") {
                                            Value = info3.UserId.ToString(),
                                            Expires = DateTime.Now.AddDays(10)
                                        };
                                        HttpContext.Current.Response.Cookies.Add(cookie);
                                        this.Page.Session["userid"] = info3.UserId.ToString();
                                        DistributorsInfo userIdDistributors = new DistributorsInfo();
                                        userIdDistributors = DistributorsBrower.GetUserIdDistributors(info3.UserId);
                                        if ((userIdDistributors != null) && (userIdDistributors.UserId > 0))
                                        {
                                            HttpCookie cookie2 = new HttpCookie("Vshop-ReferralId") {
                                                Value = userIdDistributors.UserId.ToString(),
                                                Expires = DateTime.Now.AddDays(1)
                                            };
                                            HttpContext.Current.Response.Cookies.Add(cookie2);
                                        }
                                        this.hidurl.Value = this.Page.Request.QueryString["returnUrl"];
                                    }
                                    else
                                    {
                                        this.Page.Response.Redirect(Globals.ApplicationPath + "/Default.aspx");
                                    }
                                }
                                else
                                {
                                    MemberInfo openIdMember = MemberProcessor.GetOpenIdMember(obj2["openid"].ToString());
                                    HttpCookie cookie3 = new HttpCookie("Vshop-Member") {
                                        Value = openIdMember.UserId.ToString(),
                                        Expires = DateTime.Now.AddDays(10)
                                    };
                                    HttpContext.Current.Response.Cookies.Add(cookie3);
                                    this.Page.Session["userid"] = openIdMember.UserId.ToString();
                                    DistributorsInfo info6 = new DistributorsInfo();
                                    info6 = DistributorsBrower.GetUserIdDistributors(openIdMember.UserId);
                                    if ((info6 != null) && (info6.UserId > 0))
                                    {
                                        HttpCookie cookie4 = new HttpCookie("Vshop-ReferralId") {
                                            Value = info6.UserId.ToString(),
                                            Expires = DateTime.Now.AddDays(1)
                                        };
                                        HttpContext.Current.Response.Cookies.Add(cookie4);
                                    }
                                    this.WriteError("会员OpenId已绑定过会员帐号已自动登陆！", obj2["openid"].ToString());
                                    this.hidurl.Value = this.Page.Request.QueryString["returnUrl"];
                                }
                            }
                            else
                            {
                                this.Page.Response.Redirect(Globals.ApplicationPath + "/Default.aspx");
                            }
                        }
                        else if (!string.IsNullOrEmpty(this.Page.Request.QueryString["state"]))
                        {
                            this.Page.Response.Redirect(Globals.ApplicationPath + "/Default.aspx");
                        }
                        else
                        {
                            string str6 = "https://open.weixin.qq.com/connect/oauth2/authorize?appid=" + masterSettings.WeixinAppId + "&redirect_uri=" + Globals.UrlEncode(HttpContext.Current.Request.Url.ToString()) + "&response_type=code&scope=snsapi_userinfo&state=STATE#wechat_redirect";
                            this.WriteError(str6, "用户授权的路径");
                            this.Page.Response.Redirect(str6);
                        }
                    }
                    else
                    {
                        this.Page.Response.Redirect(Globals.ApplicationPath + "/UserLogin.aspx?returnUrl=" + this.Page.Request.QueryString["returnUrl"]);
                    }
                }
                else if (this.Page.Request.Cookies["Vshop-Member"] == null)
                {
                    this.Page.Response.Redirect(Globals.ApplicationPath + "/UserLogin.aspx?returnUrl=" + this.Page.Request.QueryString["returnUrl"]);
                }
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

        public bool HasLogin(string OpenId)
        {
            MemberInfo openIdMember = MemberProcessor.GetOpenIdMember(OpenId);
            if (openIdMember != null)
            {
                HttpCookie cookie = new HttpCookie("Vshop-Member") {
                    Value = openIdMember.UserId.ToString(),
                    Expires = DateTime.Now.AddDays(10)
                };
                HttpContext.Current.Response.Cookies.Add(cookie);
                this.Page.Session["userid"] = openIdMember.UserId.ToString();
                return true;
            }
            return false;
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "skin-VUserLogining.html";
            }
            base.OnInit(e);
        }

        public void WriteError(string msg, string OpenId)
        {
            DataTable table = new DataTable {
                TableName = "wxlogin"
            };
            table.Columns.Add("OperTime");
            table.Columns.Add("ErrorMsg");
            table.Columns.Add("OpenId");
            table.Columns.Add("PageUrl");
            DataRow row = table.NewRow();
            row["OperTime"] = DateTime.Now;
            row["ErrorMsg"] = msg;
            row["OpenId"] = OpenId;
            row["PageUrl"] = HttpContext.Current.Request.Url;
            table.Rows.Add(row);
            table.WriteXml(HttpContext.Current.Request.MapPath("/wxlogin.xml"));
        }
    }
}

