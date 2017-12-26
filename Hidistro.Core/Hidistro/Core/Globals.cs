namespace Hidistro.Core
{
    using Hidistro.Core.Configuration;
    using Hidistro.Core.Enums;
    using Hidistro.Core.Urls;
    using System;
    using System.Collections;
    using System.Data;
    using System.Data.SqlClient;
    using System.Globalization;
    using System.IO;
    using System.Management;
    using System.Net.Mail;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Web;

    public static class Globals
    {
        public static string AppendQuerystring(string url, string querystring)
        {
            return AppendQuerystring(url, querystring, false);
        }

        public static string AppendQuerystring(string url, string querystring, bool urlEncoded)
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentNullException("url");
            }
            string str = "?";
            if (url.IndexOf('?') > -1)
            {
                if (!urlEncoded)
                {
                    str = "&";
                }
                else
                {
                    str = "&amp;";
                }
            }
            return (url + str + querystring);
        }

        public static int[] BubbleSort(int[] r)
        {
            for (int i = 0; i < r.Length; i++)
            {
                bool flag = false;
                for (int j = r.Length - 2; j >= i; j--)
                {
                    if (r[j + 1] > r[j])
                    {
                        int num3 = r[j + 1];
                        r[j + 1] = r[j];
                        r[j] = num3;
                        flag = true;
                    }
                }
                if (!flag)
                {
                    return r;
                }
            }
            return r;
        }

        public static bool CheckReg(string str, string reg)
        {
            return Regex.IsMatch(str, reg);
        }

        public static bool CheckVerifyCode(string verifyCode)
        {
            if (HttpContext.Current.Request.Cookies["VerifyCode"] == null)
            {
                RemoveVerifyCookie();
                return false;
            }
            bool flag = string.Compare(HiCryptographer.Decrypt(HttpContext.Current.Request.Cookies["VerifyCode"].Value), verifyCode, true, CultureInfo.InvariantCulture) == 0;
            RemoveVerifyCookie();
            return flag;
        }

        public static string CreateVerifyCode(int length)
        {
            string text = string.Empty;
            Random random = new Random();
            while (text.Length < length)
            {
                char ch;
                int num = random.Next();
                if ((num % 3) == 0)
                {
                    ch = (char) (0x61 + ((ushort) (num % 0x1a)));
                }
                else if ((num % 4) == 0)
                {
                    ch = (char) (0x41 + ((ushort) (num % 0x1a)));
                }
                else
                {
                    ch = (char) (0x30 + ((ushort) (num % 10)));
                }
                if ((((ch != '0') && (ch != 'o')) && ((ch != '1') && (ch != '7'))) && (((ch != 'l') && (ch != '9')) && ((ch != 'g') && (ch != 'I'))))
                {
                    text = text + ch.ToString();
                }
            }
            RemoveVerifyCookie();
            HttpCookie cookie = new HttpCookie("VerifyCode") {
                Value = HiCryptographer.Encrypt(text)
            };
            HttpContext.Current.Response.Cookies.Add(cookie);
            return text;
        }

        public static void EntityCoding(object entity, bool encode)
        {
            if (entity != null)
            {
                foreach (PropertyInfo info in entity.GetType().GetProperties())
                {
                    if (info.GetCustomAttributes(typeof(HtmlCodingAttribute), true).Length != 0)
                    {
                        if (!info.CanWrite || !info.CanRead)
                        {
                            throw new Exception("使用HtmlEncodeAttribute修饰的属性必须是可读可写的");
                        }
                        if (!info.PropertyType.Equals(typeof(string)))
                        {
                            throw new Exception("非字符串类型的属性不能使用HtmlEncodeAttribute修饰");
                        }
                        string str = info.GetValue(entity, null) as string;
                        if (!string.IsNullOrEmpty(str))
                        {
                            if (encode)
                            {
                                info.SetValue(entity, HtmlEncode(str), null);
                            }
                            else
                            {
                                info.SetValue(entity, HtmlDecode(str), null);
                            }
                        }
                    }
                }
            }
        }

        public static string FormatMoney(decimal money)
        {
            return string.Format(CultureInfo.InvariantCulture, "{0}", new object[] { money.ToString("F", CultureInfo.InvariantCulture) });
        }

        public static string FullPath(string local)
        {
            if (string.IsNullOrEmpty(local))
            {
                return local;
            }
            if (local.ToLower(CultureInfo.InvariantCulture).StartsWith("http://"))
            {
                return local;
            }
            if (HttpContext.Current == null)
            {
                return local;
            }
            return FullPath(HostPath(HttpContext.Current.Request.Url), local);
        }

        public static string FullPath(string hostPath, string local)
        {
            return (hostPath + local);
        }

        public static string GetAdminAbsolutePath(string path)
        {
            if (path.StartsWith("/"))
            {
                return (ApplicationPath + "/" + HiConfiguration.GetConfig().AdminFolder + path);
            }
            return (ApplicationPath + "/" + HiConfiguration.GetConfig().AdminFolder + "/" + path);
        }

        public static int GetCurrentDistributorId()
        {
            int result = 0;
            HttpCookie cookie = HttpContext.Current.Request.Cookies.Get("Vshop-ReferralId");
            if ((cookie == null) || string.IsNullOrEmpty(cookie.Value))
            {
                return 0;
            }
            int.TryParse(cookie.Value, out result);
            return result;
        }

        public static int GetCurrentManagerUserId()
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies.Get("Vshop-Manager");
            if ((cookie == null) || string.IsNullOrEmpty(cookie.Value))
            {
                return 0;
            }
            int result = 0;
            int.TryParse(cookie.Value, out result);
            return result;
        }

        public static int GetCurrentMemberUserId()
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies.Get("Vshop-Member");
            if ((cookie == null) || string.IsNullOrEmpty(cookie.Value))
            {
                return 0;
            }
            int result = 0;
            int.TryParse(cookie.Value, out result);
            return result;
        }

        //获取当前WID
        public static string GetCurrentWid()
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies.Get("Vshop-Wid");
            if ((cookie == null) || string.IsNullOrEmpty(cookie.Value))
            {
                //string wid = Globals.RequestQueryStr("wid");
                //cookie = new HttpCookie("Vshop-Wid");
                //cookie.Value = wid;
                //cookie.Expires = DateTime.Now.AddYears(1);
                //HttpContext.Current.Response.Cookies.Set(cookie);
                return "";
            }
            return cookie.Value;
        }

        public static string GetFavorites()
        {
            string str = "";
            int result = 0;
            HttpCookie cookie = HttpContext.Current.Request.Cookies.Get("Vshop-ReferralId");
            if ((cookie == null) || string.IsNullOrEmpty(cookie.Value))
            {
                return "";
            }
            int.TryParse(cookie.Value, out result);
            if (result > 0)
            {
                str = "&ReferralId=" + result;
            }
            return str;
        }

        public static string GetGenerateId()
        {
            string str = string.Empty;
            Random random = new Random();
            for (int i = 0; i < 7; i++)
            {
                int num = random.Next();
                str = str + ((char) (0x30 + ((ushort) (num % 10)))).ToString();
            }
            return (DateTime.Now.ToString("yyyyMMdd") + str);
        }

        public static SiteUrls GetSiteUrls()
        {
            return SiteUrls.Instance();
        }

        public static string GetStoragePath()
        {
            return "/Storage/master";
        }

        public static int GetStrLen(string strData)
        {
            return Encoding.GetEncoding("GB2312").GetByteCount(strData);
        }

        public static string GetVshopSkinPath(string themeName = null)
        {
            return (ApplicationPath + "/Templates/common").ToLower(CultureInfo.InvariantCulture);
        }

        public static string HostPath(Uri uri)
        {
            if (uri == null)
            {
                return string.Empty;
            }
            string str = (uri.Port == 80) ? string.Empty : (":" + uri.Port.ToString(CultureInfo.InvariantCulture));
            return string.Format(CultureInfo.InvariantCulture, "{0}://{1}{2}", new object[] { uri.Scheme, uri.Host, str });
        }

        public static string HtmlDecode(string textToFormat)
        {
            if (string.IsNullOrEmpty(textToFormat))
            {
                return textToFormat;
            }
            return HttpUtility.HtmlDecode(textToFormat);
        }

        public static string HtmlEncode(string textToFormat)
        {
            if (string.IsNullOrEmpty(textToFormat))
            {
                return textToFormat;
            }
            return HttpUtility.HtmlEncode(textToFormat);
        }

        public static bool IsNumeric(string lstr)
        {
            return (!string.IsNullOrEmpty(lstr) && Regex.IsMatch(lstr, @"^\d+(\.)?\d*$"));
        }

        public static string LinkUrl(string url)
        {
            string str = url;
            if (str.Contains("?"))
            {
                return (str + "&ReferralId=" + RequestQueryStr("UserId"));
            }
            return (str + "?ReferralId=" + RequestQueryStr("UserId"));
        }

        public static string MapPath(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return string.Empty;
            }
            HttpContext current = HttpContext.Current;
            if (current != null)
            {
                return current.Request.MapPath(path);
            }
            return PhysicalPath(path.Replace("/", Path.DirectorySeparatorChar.ToString()).Replace("~", ""));
        }

        public static string PhysicalPath(string path)
        {
            if (path == null)
            {
                return string.Empty;
            }
            return (RootPath().TrimEnd(new char[] { Path.DirectorySeparatorChar }) + Path.DirectorySeparatorChar.ToString() + path.TrimStart(new char[] { Path.DirectorySeparatorChar }));
        }

        public static void RedirectToSSL(HttpContext context)
        {
            if ((context != null) && !context.Request.IsSecureConnection)
            {
                Uri url = context.Request.Url;
                context.Response.Redirect("https://" + url.ToString().Substring(7));
            }
        }

        private static void RemoveVerifyCookie()
        {
            HttpContext.Current.Response.Cookies["VerifyCode"].Value = null;
            HttpContext.Current.Response.Cookies["VerifyCode"].Expires = new DateTime(0x777, 10, 12);
        }

        public static int RequestFormNum(string sTemp)
        {
            string s = HttpContext.Current.Request.Form[sTemp];
            return ToNum(s);
        }

        public static string RequestFormStr(string sTemp)
        {
            string str = HttpContext.Current.Request.Form[sTemp];
            if (string.IsNullOrEmpty(str))
            {
                return "";
            }
            return str.Trim();
        }

        public static int RequestQueryNum(string sTemp)
        {
            string s = HttpContext.Current.Request.QueryString[sTemp];
            return ToNum(s);
        }

        public static string RequestQueryStr(string sTemp)
        {
            string str = HttpContext.Current.Request.QueryString[sTemp];
            if (string.IsNullOrEmpty(str))
            {
                return "";
            }
            return str.Trim();
        }

        private static string RootPath()
        {
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string newValue = Path.DirectorySeparatorChar.ToString();
            baseDirectory = baseDirectory.Replace("/", newValue);
            string filesPath = HiConfiguration.GetConfig().FilesPath;
            if (filesPath == null)
            {
                return baseDirectory;
            }
            filesPath = filesPath.Replace("/", newValue);
            if (((filesPath.Length > 0) && filesPath.StartsWith(newValue)) && baseDirectory.EndsWith(newValue))
            {
                return (baseDirectory + filesPath.Substring(1));
            }
            return (baseDirectory + filesPath);
        }

        public static string ServerIP()
        {
            string str = HttpContext.Current.Request.ServerVariables.Get("Local_Addr").ToString();
            if (str.Length < 7)
            {
                ManagementClass class2 = new ManagementClass("Win32_NetworkAdapterConfiguration");
                foreach (ManagementObject obj2 in class2.GetInstances())
                {
                    if ((bool) obj2["IPEnabled"])
                    {
                        string[] strArray = (string[]) obj2["IPAddress"];
                        if (strArray.Length > 0)
                        {
                            str = strArray[0];
                        }
                        return str;
                    }
                }
            }
            return str;
        }

        public static string String2Json(string s)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < s.Length; i++)
            {
                char ch = s.ToCharArray()[i];
                switch (ch)
                {
                    case '/':
                    {
                        builder.Append(@"\/");
                        continue;
                    }
                    case '\\':
                    {
                        builder.Append(@"\\");
                        continue;
                    }
                    case '\b':
                    {
                        builder.Append(@"\b");
                        continue;
                    }
                    case '\t':
                    {
                        builder.Append(@"\t");
                        continue;
                    }
                    case '\n':
                    {
                        builder.Append(@"\n");
                        continue;
                    }
                    case '\f':
                    {
                        builder.Append(@"\f");
                        continue;
                    }
                    case '\r':
                    {
                        builder.Append(@"\r");
                        continue;
                    }
                    case '"':
                    {
                        builder.Append("\\\"");
                        continue;
                    }
                }
                builder.Append(ch);
            }
            return builder.ToString();
        }

        public static string StripAllTags(string strToStrip)
        {
            strToStrip = Regex.Replace(strToStrip, @"</p(?:\s*)>(?:\s*)<p(?:\s*)>", "\n\n", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            strToStrip = Regex.Replace(strToStrip, @"<br(?:\s*)/>", "\n", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            strToStrip = Regex.Replace(strToStrip, "\"", "''", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            strToStrip = StripHtmlXmlTags(strToStrip);
            return strToStrip;
        }

        public static string StripForPreview(string content)
        {
            content = Regex.Replace(content, "<br>", "\n", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            content = Regex.Replace(content, "<br/>", "\n", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            content = Regex.Replace(content, "<br />", "\n", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            content = Regex.Replace(content, "<p>", "\n", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            content = content.Replace("'", "&#39;");
            return StripHtmlXmlTags(content);
        }

        public static string StripHtmlXmlTags(string content)
        {
            return Regex.Replace(content, "<[^>]+>", "", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        }

        public static string StripScriptTags(string content)
        {
            content = Regex.Replace(content, "<script((.|\n)*?)</script>", "", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            content = Regex.Replace(content, "'javascript:", "", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            return Regex.Replace(content, "\"javascript:", "", RegexOptions.Multiline | RegexOptions.IgnoreCase);
        }

        public static string SubStr(string s, int i, string smore)
        {
            int num = 0;
            int num2 = 0;
            string str = s;
            if (GetStrLen(s) <= i)
            {
                return str;
            }
            foreach (char ch in s)
            {
                if (num >= i)
                {
                    break;
                }
                num2++;
                if (ch > '\x007f')
                {
                    num += 2;
                }
                else
                {
                    num++;
                }
            }
            str = s.Substring(0, num2 - GetStrLen(smore));
            return (str + smore);
        }

        public static string ToDelimitedString(ICollection collection, string delimiter)
        {
            if (collection == null)
            {
                return string.Empty;
            }
            StringBuilder builder = new StringBuilder();
            if (collection is Hashtable)
            {
                foreach (object obj2 in ((Hashtable) collection).Keys)
                {
                    builder.Append(obj2.ToString() + delimiter);
                }
            }
            if (collection is ArrayList)
            {
                foreach (object obj3 in (ArrayList) collection)
                {
                    builder.Append(obj3.ToString() + delimiter);
                }
            }
            if (collection is string[])
            {
                foreach (string str in (string[]) collection)
                {
                    builder.Append(str + delimiter);
                }
            }
            if (collection is MailAddressCollection)
            {
                foreach (MailAddress address in (MailAddressCollection) collection)
                {
                    builder.Append(address.Address + delimiter);
                }
            }
            return builder.ToString().TrimEnd(new char[] { Convert.ToChar(delimiter, CultureInfo.InvariantCulture) });
        }

        public static int ToNum(object s)
        {
            string str = (s == null) ? "0" : s.ToString();
            if (!string.IsNullOrEmpty(str) && IsNumeric(str))
            {
                try
                {
                    return Convert.ToInt32(str);
                }
                catch
                {
                    return 0;
                }
            }
            return 0;
        }

        public static string UnHtmlEncode(string formattedPost)
        {
            RegexOptions options = RegexOptions.Compiled | RegexOptions.IgnoreCase;
            formattedPost = Regex.Replace(formattedPost, "&quot;", "\"", options);
            formattedPost = Regex.Replace(formattedPost, "&lt;", "<", options);
            formattedPost = Regex.Replace(formattedPost, "&gt;", ">", options);
            return formattedPost;
        }

        public static string UrlDecode(string urlToDecode)
        {
            if (string.IsNullOrEmpty(urlToDecode))
            {
                return urlToDecode;
            }
            return HttpUtility.UrlDecode(urlToDecode, Encoding.UTF8);
        }

        public static string UrlEncode(string urlToEncode)
        {
            if (string.IsNullOrEmpty(urlToEncode))
            {
                return urlToEncode;
            }
            return HttpUtility.UrlEncode(urlToEncode, Encoding.UTF8);
        }

        public static void ValidateSecureConnection(SSLSettings ssl, HttpContext context)
        {
            if (HiConfiguration.GetConfig().SSL == ssl)
            {
                RedirectToSSL(context);
            }
        }

        public static string ApplicationPath
        {
            get
            {
                string applicationPath = "/";
                if (HttpContext.Current != null)
                {
                    try
                    {
                        applicationPath = HttpContext.Current.Request.ApplicationPath;
                    }
                    catch
                    {
                        applicationPath = AppDomain.CurrentDomain.BaseDirectory;
                    }
                }
                if (applicationPath == "/")
                {
                    return string.Empty;
                }
                return applicationPath.ToLower(CultureInfo.InvariantCulture);
            }
        }

        public static string DomainName
        {
            get
            {
                if (HttpContext.Current == null)
                {
                    return string.Empty;
                }
                return HttpContext.Current.Request.Url.Host;
            }
        }

        public static string IPAddress
        {
            get
            {
                string userHostAddress;
                HttpRequest request = HttpContext.Current.Request;
                if (string.IsNullOrEmpty(request.ServerVariables["HTTP_X_FORWARDED_FOR"]))
                {
                    userHostAddress = request.ServerVariables["REMOTE_ADDR"];
                }
                else
                {
                    userHostAddress = request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                }
                if (string.IsNullOrEmpty(userHostAddress))
                {
                    userHostAddress = request.UserHostAddress;
                }
                return userHostAddress;
            }
        }

        public static Chenduo.Model.sf_website GetModelByWid(string wid)
        {

            StringBuilder strSql = new StringBuilder();
            //strSql.Append("select  top 1 id,businessNum,mid,templatesNum,appid_name,appid_origin_id,weixin_account,avatar,interface_url,token_value,encodingaeskey,appid,appsecret,create_user,create_time,payment_name,state,weixin_pay_account,account_pay_key,send_type,logo,description,wid,sitename,tel,Enableweixinrequest,WeixinCertPassword,Alipay_mid,Alipay_mName,Alipay_Pid ,Alipay_Key,OffLinePayContent,EnableWeixinRed,EnableAlipayRequest,EnablePodRequest,EnableOffLineRequest from sf_website ");
            strSql.Append("select  top 1 * from sf_website ");
            strSql.Append(" where wid=@wid");
            SqlParameter[] parameters = {
                    new SqlParameter("@wid", SqlDbType.NVarChar)
            };
            parameters[0].Value = wid;

            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                return DataRowToModel(ds.Tables[0].Rows[0]);
            }
            else
            {
                return null;
            }
        }

        public static Chenduo.Model.sf_website DataRowToModel(DataRow row)
        {
            Chenduo.Model.sf_website model = new Chenduo.Model.sf_website();
            if (row != null)
            {
                if (row["id"] != null && row["id"].ToString() != "")
                {
                    model.id = long.Parse(row["id"].ToString());
                }
                if (row["businessNum"] != null)
                {
                    model.businessNum = row["businessNum"].ToString();
                }
                if (row["mid"] != null)
                {
                    model.mid = long.Parse(row["mid"].ToString());
                }
                if (row["templatesNum"] != null)
                {
                    model.templatesNum = row["templatesNum"].ToString();
                }
                if (row["appid_name"] != null)
                {
                    model.appid_name = row["appid_name"].ToString();
                }
                if (row["appid_origin_id"] != null)
                {
                    model.appid_origin_id = row["appid_origin_id"].ToString();
                }
                if (row["weixin_account"] != null)
                {
                    model.weixin_account = row["weixin_account"].ToString();
                }
                if (row["avatar"] != null)
                {
                    model.avatar = row["avatar"].ToString();
                }
                if (row["interface_url"] != null)
                {
                    model.interface_url = row["interface_url"].ToString();
                }
                if (row["token_value"] != null)
                {
                    model.token_value = row["token_value"].ToString();
                }
                if (row["encodingaeskey"] != null)
                {
                    model.encodingaeskey = row["encodingaeskey"].ToString();
                }
                if (row["appid"] != null)
                {
                    model.appid = row["appid"].ToString();
                }
                if (row["appsecret"] != null)
                {
                    model.appsecret = row["appsecret"].ToString();
                }
                if (row["create_user"] != null)
                {
                    model.create_user = row["create_user"].ToString();
                }
                if (row["create_time"] != null)
                {
                    model.create_time = row["create_time"].ToString();
                }
                if (row["payment_name"] != null)
                {
                    model.payment_name = row["payment_name"].ToString();
                }
                if (row["state"] != null && row["state"].ToString() != "")
                {
                    model.state = int.Parse(row["state"].ToString());
                }
                if (row["weixin_pay_account"] != null)
                {
                    model.weixin_pay_account = row["weixin_pay_account"].ToString();
                }
                if (row["account_pay_key"] != null)
                {
                    model.account_pay_key = row["account_pay_key"].ToString();
                }
                if (row["send_type"] != null && row["send_type"].ToString() != "")
                {
                    model.send_type = int.Parse(row["send_type"].ToString());
                }
                if (row["logo"] != null)
                {
                    model.logo = row["logo"].ToString();
                }
                if (row["description"] != null)
                {
                    model.description = row["description"].ToString();
                }
                if (row["wid"] != null)
                {
                    model.wid = row["wid"].ToString();
                }
                if (row["sitename"] != null)
                {
                    model.sitename = row["sitename"].ToString();
                }
                if (row["tel"] != null)
                {
                    model.tel = row["tel"].ToString();
                }
                if (row["Enableweixinrequest"] != null)
                {
                    model.Enableweixinrequest = row["Enableweixinrequest"].ToString();
                }

                if (row["WeixinCertPassword"] != null)
                {
                    model.WeixinCertPassword = row["WeixinCertPassword"].ToString();
                }
                if (row["Alipay_mid"] != null)
                {
                    model.Alipay_mid = row["Alipay_mid"].ToString();
                }
                if (row["Alipay_mName"] != null)
                {
                    model.Alipay_mName = row["Alipay_mName"].ToString();
                }
                if (row["Alipay_Pid"] != null)
                {
                    model.Alipay_Pid = row["Alipay_Pid"].ToString();
                }
                if (row["Alipay_Key"] != null)
                {
                    model.Alipay_Key = row["Alipay_Key"].ToString();
                }
                if (row["OffLinePayContent"] != null)
                {
                    model.OffLinePayContent = row["OffLinePayContent"].ToString();
                }
                if (row["EnableWeixinRed"] != null)
                {
                    model.EnableWeixinRed = row["EnableWeixinRed"].ToString();
                }
                if (row["EnableAlipayRequest"] != null)
                {
                    model.EnableAlipayRequest = row["EnableAlipayRequest"].ToString();
                }
                if (row["EnablePodRequest"] != null)
                {
                    model.EnablePodRequest = row["EnablePodRequest"].ToString();
                }
                if (row["EnableOffLineRequest"] != null)
                {
                    model.EnableOffLineRequest = row["EnableOffLineRequest"].ToString();

                }


                if (row["WeixinCertPath"] != null)
                {
                    model.WeixinCertPath = row["WeixinCertPath"].ToString();

                }
                if (row["OpenManyService"] != null)
                {
                    model.OpenManyService = row["OpenManyService"].ToString();

                }
                if (row["GuidePageSet"] != null)
                {
                    model.GuidePageSet = row["GuidePageSet"].ToString();

                }
                if (row["EnableGuidePageSet"] != null)
                {
                    model.EnableGuidePageSet = row["EnableGuidePageSet"].ToString();

                }
                if (row["ManageOpenID"] != null)
                {
                    model.ManageOpenID = row["ManageOpenID"].ToString();

                }
                if (row["IsValidationService"] != null)
                {
                    model.IsValidationService = row["IsValidationService"].ToString();
                }
                if (row["EnableShopMenu"] != null && row["EnableShopMenu"].ToString() != "")
                {
                    model.EnableShopMenu = Boolean.Parse(row["EnableShopMenu"].ToString());
                }

                if (row["EnableSaleService"] != null && row["EnableSaleService"].ToString() != "")
                {
                    model.EnableSaleService = Boolean.Parse(row["EnableSaleService"].ToString());
                }
                if (row["entId"] != null)
                {
                    model.entId=row["entId"].ToString();
                }

            }
            return model;
        }
    }
}

