namespace Hidistro.ControlPanel.Store
{
    using Hidistro.Entities.Members;
    using Hidistro.SaleSystem.Vshop;
    using System;
    using System.Runtime.InteropServices;
    using System.Web;
    using System.Linq;
    using Core;
    using Commodities;
    using Entities.Commodities;

    public static class HiAffiliation
    {
        public static void ClearReferralIdCookie()
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies["Vshop-ReferralId"];
            if (((cookie != null) && !string.IsNullOrEmpty(cookie.Value)) && (int.Parse(cookie.Value) == 0))
            {
                cookie.Value = null;
                cookie.Expires = DateTime.Now.AddDays(-1);
                HttpContext.Current.Response.Cookies.Set(cookie);
            }
        }

        public static void ClearUserCookie(string url = "", bool isRedirect = false)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies["Vshop-Member"];
            if ((cookie != null) && !string.IsNullOrEmpty(cookie.Value))
            {
                cookie.Value = null;
                cookie.Expires = DateTime.Now.AddDays(-1);
                HttpContext.Current.Response.Cookies.Set(cookie);
                if (isRedirect && !string.IsNullOrEmpty(url))
                {
                    HttpContext.Current.Response.Redirect(url);
                }
            }
        }

        public static string GetReturnUrl(string returnUrl)
        {
            if (returnUrl.IndexOf("?") > -1)
            {
                returnUrl = returnUrl.Substring(returnUrl.IndexOf("?"));
            }
            return returnUrl;
        }

        public static void LoadPage()
        {
            string str = ReturnUrl();
            if (!string.IsNullOrEmpty(str))
            {
                HttpContext.Current.Response.Redirect(str);
            }
            HttpCookie cookie = HttpContext.Current.Request.Cookies["Vshop-ReferralId"];
            if (((cookie != null) && !string.IsNullOrEmpty(cookie.Value)) && ((int.Parse(cookie.Value) != 0) && (DistributorsBrower.GetCurrentDistributors(Convert.ToInt16(cookie.Value)) == null)))
            {
                ClearReferralIdCookie();
                ClearWidCookie("", false);
                ClearUserCookie("/UserLogin.aspx", true);
            }
        }

        public static string ReturnUrl()
        {
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            if (currentMember != null)
            {
                //return ReturnUrlByUser(currentMember);
            }
            return ReturnUrlByQueryString();
        }

        public static string ReturnUrlByQueryString()
        {
            int result = 0;
            string str = HttpContext.Current.Request.Url.PathAndQuery.ToString();
            //url中没有返回url
            if (!HttpContext.Current.Request.QueryString.AllKeys.Contains("returnUrl"))
            {
                if (HttpContext.Current.Request.Url.AbsolutePath == "/logout.aspx")
                {
                    return string.Empty;
                }
                //save wid              
                if (!string.IsNullOrEmpty(HttpContext.Current.Request.QueryString["wid"]))
                {
                    ClearWidCookie("",false);
                    SetWidCookie(Globals.RequestQueryStr("wid"));
                }
                //商品详情
                //if(HttpContext.Current.Request.QueryString.AllKeys.Contains("ProductDetails") && !string.IsNullOrEmpty(HttpContext.Current.Request.QueryString["ProductId"]))
                //{
                //    string productid = HttpContext.Current.Request.QueryString["ProductId"];
                //    //获取该商品所在wid
                //    ProductInfo productInfo = ProductHelper.GetProduct(int.Parse(productid));
                //    if(!string.IsNullOrEmpty(productInfo.wid))
                //    {
                //        SetWidCookie(productInfo.wid);
                //    }
                //}
                if (!string.IsNullOrEmpty(HttpContext.Current.Request.QueryString["ReferralId"]))
                {
                    if (int.TryParse(HttpContext.Current.Request.QueryString["ReferralId"], out result) && (result != 0))
                    {
                        HttpCookie cookie = HttpContext.Current.Request.Cookies["Vshop-ReferralId"];
                        if ((cookie != null) && (cookie.Value != result.ToString()))
                        {
                            SetReferralIdCookie(result.ToString(), "", false);
                        }
                    }
                }
                else  if (HttpContext.Current.Request.Url.AbsolutePath.ToLower() != "/default.aspx")
                {
                    HttpCookie cookie2 = HttpContext.Current.Request.Cookies["Vshop-ReferralId"];
                    if ((cookie2 != null) && !string.IsNullOrEmpty(cookie2.Value))
                    {
                        if (HttpContext.Current.Request.QueryString.Count > 0)
                        {
                            if (!HttpContext.Current.Request.QueryString.AllKeys.Contains("ReferralId"))
                            {
                                return (str + "&ReferralId=" + cookie2.Value);
                            }
                            return string.Empty;
                        }
                        if (!HttpContext.Current.Request.QueryString.AllKeys.Contains("ReferralId"))
                        {
                            return (str + "?ReferralId=" + cookie2.Value);
                        }
                        return string.Empty;
                    }
                }
                else
                {
                    SetReferralIdCookie("0", "", false);
                }
                if (!HttpContext.Current.Request.QueryString.AllKeys.Contains("returnUrl") && (HttpContext.Current.Request.Url.AbsolutePath != "/logout.aspx"))
                {
                    if (!HttpContext.Current.Request.QueryString.AllKeys.Contains("ReferralId") && (HttpContext.Current.Request.QueryString.Count > 0))
                    {
                        return (str + "&ReferralId=" + result.ToString());
                    }
                    if (!HttpContext.Current.Request.QueryString.AllKeys.Contains("ReferralId"))
                    {
                        return (str + "?ReferralId=" + result.ToString());
                    }
                }
            }
            else
            {
                string returnUrl = HttpContext.Current.Request.QueryString["returnUrl"];
                //productDetails
            }
            return string.Empty;
        }

        public static string ReturnUrlByUser(MemberInfo mInfo)
        {
            string str = HttpContext.Current.Request.Url.PathAndQuery.ToString();
            DistributorsInfo currentDistributors = DistributorsBrower.GetCurrentDistributors(Convert.ToInt32(mInfo.UserId));
            if (currentDistributors != null)
            {
                SetReferralIdCookie(currentDistributors.UserId.ToString(), "", false);
            }
            else
            {
                if (string.IsNullOrEmpty(mInfo.wid))
                {
                    ClearReferralIdCookie();
                    ClearUserCookie("", false);
                    ClearWidCookie("",false);
                    ReturnUrlByQueryString();
                }
                else
                { 
                    SetReferralIdCookie(mInfo.ReferralUserId.ToString(), "", false);
                    SetWidCookie(mInfo.wid);
                }
            }
            HttpCookie cookie = HttpContext.Current.Request.Cookies["Vshop-ReferralId"];
            if ((cookie != null) && !string.IsNullOrEmpty(cookie.Value))
            {
                if (!string.IsNullOrEmpty(HttpContext.Current.Request.QueryString["ReferralId"]))
                {
                    HiUriHelp help = new HiUriHelp(HttpContext.Current.Request.Url.Query);
                    string queryString = help.GetQueryString("ReferralId");
                    if (!string.IsNullOrEmpty(queryString))
                    {
                        if (queryString == cookie.Value)
                        {
                            return string.Empty;
                        }
                        help.SetQueryString("ReferralId", cookie.Value);
                        return (HttpContext.Current.Request.Url.AbsolutePath + help.GetNewQuery());
                    }
                }
                if (!HttpContext.Current.Request.QueryString.AllKeys.Contains("returnUrl"))
                {
                    if (HttpContext.Current.Request.Url.AbsolutePath == "/logout.aspx")
                    {
                        return string.Empty;
                    }
                    if (!HttpContext.Current.Request.QueryString.AllKeys.Contains("ReferralId") && (HttpContext.Current.Request.QueryString.Count > 0))
                    {
                        return (str + "&ReferralId=" + cookie.Value);
                    }
                    if (!HttpContext.Current.Request.QueryString.AllKeys.Contains("ReferralId"))
                    {
                        return (str + "?ReferralId=" + cookie.Value);
                    }
                }
            }
            return string.Empty;
        }

        public static void SetReferralIdCookie(string referralId, string url = "", bool isRedirect = false)
        {
            ClearReferralIdCookie();
            HttpCookie cookie = HttpContext.Current.Request.Cookies["Vshop-ReferralId"];
            if (cookie == null)
            {
                cookie = new HttpCookie("Vshop-ReferralId");
            }
            cookie.Value = referralId;
            cookie.Expires = DateTime.Now.AddDays(1);
            HttpContext.Current.Response.Cookies.Set(cookie);
            if (isRedirect && !string.IsNullOrEmpty(url))
            {
                //重定向
                HttpContext.Current.Response.Redirect(url);
            }
        }

        public static void SetUserCookie(string userID)
        {
            ClearUserCookie("", false);
            HttpCookie cookie = HttpContext.Current.Request.Cookies["Vshop-Member"];
            if (cookie == null)
            {
                cookie = new HttpCookie("Vshop-Member");
            }
            cookie.Value = userID;
            cookie.Expires = DateTime.Now.AddDays(1);
            HttpContext.Current.Response.Cookies.Set(cookie);
        }
        public static void SetWidCookie(string wid)
        {
            ClearWidCookie("", false);
            HttpCookie cookie = HttpContext.Current.Request.Cookies["Vshop-Wid"];
            if (cookie == null)
            {
                cookie = new HttpCookie("Vshop-Wid");
            }
            cookie.Value = wid;
            cookie.Expires = DateTime.Now.AddDays(1);
            HttpContext.Current.Response.Cookies.Set(cookie);
        }
        public static void ClearWidCookie(string url = "", bool isRedirect = false)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies["Vshop-Wid"];
            if ((cookie != null) && !string.IsNullOrEmpty(cookie.Value))
            {
                cookie.Value = null;
                cookie.Expires = DateTime.Now.AddDays(-1);
                HttpContext.Current.Response.Cookies.Set(cookie);
                if (isRedirect && !string.IsNullOrEmpty(url))
                {
                    HttpContext.Current.Response.Redirect(url);
                }
            }
        }
    }
}

