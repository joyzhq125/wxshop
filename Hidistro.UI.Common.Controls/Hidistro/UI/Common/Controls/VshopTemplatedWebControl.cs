namespace Hidistro.UI.Common.Controls
{
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using System;
    using System.Collections.Specialized;
    using System.Globalization;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Web;
    using System.Web.UI;

    [ParseChildren(true), PersistChildren(false)]
    public abstract class VshopTemplatedWebControl : TemplatedWebControl
    {
        protected int referralId;
        private string RefrrealKey = "Vshop-ReferralId";
        private string skinName;
        protected VshopTemplatedWebControl()
        {
            HiAffiliation.LoadPage();
            this.wid = Globals.GetCurrentWid();
        }

        private string ControlText()
        {
            if (!this.SkinFileExists)
            {
                return null;
            }
            StringBuilder builder = new StringBuilder(File.ReadAllText(this.Page.Request.MapPath(this.SkinPath), Encoding.UTF8));
            if (builder.Length == 0)
            {
                return null;
            }
            builder.Replace("<%", "").Replace("%>", "");
            string vshopSkinPath = Globals.GetVshopSkinPath(null);
            builder.Replace("/images/", vshopSkinPath + "/images/");
            builder.Replace("/script/", vshopSkinPath + "/script/");
            builder.Replace("/style/", vshopSkinPath + "/style/");
            builder.Replace("/utility/", Globals.ApplicationPath + "/utility/");
            builder.Insert(0, "<%@ Register TagPrefix=\"UI\" Namespace=\"ASPNET.WebControls\" Assembly=\"ASPNET.WebControls\" %>" + Environment.NewLine);
            builder.Insert(0, "<%@ Register TagPrefix=\"Kindeditor\" Namespace=\"kindeditor.Net\" Assembly=\"kindeditor.Net\" %>" + Environment.NewLine);
            builder.Insert(0, "<%@ Register TagPrefix=\"Hi\" Namespace=\"Hidistro.UI.Common.Controls\" Assembly=\"Hidistro.UI.Common.Controls\" %>" + Environment.NewLine);
            builder.Insert(0, "<%@ Register TagPrefix=\"Hi\" Namespace=\"Hidistro.UI.SaleSystem.Tags\" Assembly=\"Hidistro.UI.SaleSystem.Tags\" %>" + Environment.NewLine);
            builder.Insert(0, "<%@ Control Language=\"C#\" %>" + Environment.NewLine);
            MatchCollection matchs = Regex.Matches(builder.ToString(), "href(\\s+)?=(\\s+)?\"url:(?<UrlName>.*?)(\\((?<Param>.*?)\\))?\"", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            for (int i = matchs.Count - 1; i >= 0; i--)
            {
                int startIndex = matchs[i].Groups["UrlName"].Index - 4;
                int length = matchs[i].Groups["UrlName"].Length + 4;
                if (matchs[i].Groups["Param"].Length > 0)
                {
                    length += matchs[i].Groups["Param"].Length + 2;
                }
                builder.Remove(startIndex, length);
                builder.Insert(startIndex, Globals.GetSiteUrls().UrlData.FormatUrl(matchs[i].Groups["UrlName"].Value.Trim(), new object[] { matchs[i].Groups["Param"].Value }));
            }
            return builder.ToString();
        }

        protected override void CreateChildControls()
        {
            this.Controls.Clear();
            //HiAffiliation.LoadPage();
            //this.wid = Globals.GetCurrentWid();
            if (!this.LoadHtmlThemedControl())
            {
                throw new SkinNotFoundException(this.SkinPath);
            }
            PageTitle.wid = this.wid;
            this.AttachChildControls();
        }

        private string GenericReloadUrl(NameValueCollection queryStrings)
        {
            if ((queryStrings == null) || (queryStrings.Count == 0))
            {
                return this.Page.Request.Url.AbsolutePath;
            }
            StringBuilder builder = new StringBuilder();
            builder.Append(this.Page.Request.Url.AbsolutePath).Append("?");
            foreach (string str2 in queryStrings.Keys)
            {
                if (queryStrings[str2] != null)
                {
                    string str = queryStrings[str2].Trim();
                    if (!string.IsNullOrEmpty(str) && (str.Length > 0))
                    {
                        builder.Append(str2).Append("=").Append(this.Page.Server.UrlEncode(str)).Append("&");
                    }
                }
            }
            queryStrings.Clear();
            builder.Remove(builder.Length - 1, 1);
            return builder.ToString();
        }

        public string getUrl()
        {
            string str = HttpContext.Current.Request.Url.PathAndQuery.ToString();
            int startIndex = str.LastIndexOf("/") + 1;
            int length = (str.IndexOf(".aspx") - str.LastIndexOf("/")) - 1;
            return str.Substring(startIndex, length);
        }

        protected void GotoResourceNotFound(string errorMsg = "")
        {
            this.Page.Response.Redirect(Globals.ApplicationPath + "/ResourceNotFound.aspx?errorMsg=" + errorMsg);
        }

        protected bool LoadHtmlThemedControl()
        {
            string str = this.ControlText();
            if (!string.IsNullOrEmpty(str))
            {
                Control child = this.Page.ParseControl(str);
                child.ID = "_";
                this.Controls.Add(child);
                return true;
            }
            return false;
        }

        public void RegisterShareScript(string ImgUrl, string lineLink, string descContent, string shareTitle)
        {
            string weixinAppId = SettingsManager.GetMasterSettings(true,wid).WeixinAppId;
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("<script language=\"javascript\" type=\"text/javascript\">");
            builder.AppendLine("var wxShareImgUrl = '" + ImgUrl + "';");
            builder.AppendLine("var wxSharelineLink = '" + lineLink + "';");
            builder.AppendLine("var wxSharedescContent = '" + (string.IsNullOrEmpty(descContent) ? "" : descContent.Replace("'", "")) + "';");
            builder.AppendLine("var wxShareshareTitle = '" + shareTitle + "';");
            builder.AppendLine("var wxappid = '" + weixinAppId + "';");
            builder.AppendLine("var wxShareImgWidth = '220';");
            builder.AppendLine("var wxShareImgHeight = '220';");
            builder.AppendLine("function weixinShareTimeline(){");
            builder.AppendLine("WeixinJSBridge.invoke('shareTimeline',{");
            builder.AppendLine("\"img_url\":wxShareImgUrl,");
            builder.AppendLine("\"img_width\":wxShareImgWidth,");
            builder.AppendLine("\"img_height\": wxShareImgHeight,");
            builder.AppendLine("\"link\": wxSharelineLink,");
            builder.AppendLine("\"desc\": wxSharedescContent,");
            builder.AppendLine("\"title\": wxShareshareTitle");
            builder.AppendLine("});");
            builder.AppendLine("}");
            builder.AppendLine("function wxshareFriend() {");
            builder.AppendLine("WeixinJSBridge.invoke('sendAppMessage', {");
            builder.AppendLine("\"appid\": wxappid,");
            builder.AppendLine("\"img_url\": wxShareImgUrl,");
            builder.AppendLine("\"img_width\": wxShareImgWidth,");
            builder.AppendLine("\"img_height\": wxShareImgHeight,");
            builder.AppendLine("\"link\": wxSharelineLink,");
            builder.AppendLine("\"desc\": wxSharedescContent,");
            builder.AppendLine("\"title\": wxShareshareTitle");
            builder.AppendLine("}, function (res) {");
            builder.AppendLine("})");
            builder.AppendLine("}");
            builder.AppendLine("document.addEventListener('WeixinJSBridgeReady', function onBridgeReady() {");
            builder.AppendLine("WeixinJSBridge.on('menu:share:appmessage', function (argv) {");
            builder.AppendLine("wxshareFriend();");
            builder.AppendLine("});");
            builder.AppendLine("WeixinJSBridge.on('menu:share:timeline', function (argv) {");
            builder.AppendLine("weixinShareTimeline();");
            builder.AppendLine("});");
            builder.AppendLine("},false);");
            builder.AppendLine("</script>");
            HttpContext.Current.Response.Write(builder.ToString());
        }

        public void ReloadPage(NameValueCollection queryStrings)
        {
            this.Page.Response.Redirect(this.GenericReloadUrl(queryStrings));
        }

        public void ReloadPage(NameValueCollection queryStrings, bool endResponse)
        {
            this.Page.Response.Redirect(this.GenericReloadUrl(queryStrings), endResponse);
        }

        private bool SkinFileExists
        {
            get
            {
                return !string.IsNullOrEmpty(this.SkinName);
            }
        }

        public virtual string SkinName
        {
            get
            {
                return this.skinName;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    value = value.ToLower(CultureInfo.InvariantCulture);
                    if (value.EndsWith(".html"))
                    {
                        this.skinName = value;
                    }
                }
            }
        }

        protected virtual string SkinPath
        {
            get
            {
                /*
                //string vTheme = SettingsManager.GetMasterSettings(true,wid).VTheme;
                //SF.Model.sf_website website = new SF.BLL.sf_website().GetModelByWid(wid);
                this.wid = Globals.GetCurrentWid();
                if (string.IsNullOrEmpty(this.wid))
                {
                    //HttpContext.Current.Response.Redirect(str);
                    //如果未传入WID
                    return "";

                }
                else
                {
                    SF.Model.sf_website website = Globals.GetModelByWid(this.wid);
                    if (website == null) return "";
                    string vTheme = website.templatesNum;
                    if (this.SkinName.StartsWith(vTheme))
                    {
                        return this.SkinName;
                    }
                    if (this.SkinName.StartsWith("/"))
                    {
                        return (vTheme + this.SkinName);
                    }
                    return (Globals.ApplicationPath + "/Templates/vshop/" + this.wid + "/" + vTheme + "/" + this.SkinName);
                    */
                    return (Globals.ApplicationPath + "/Templates/common/" + this.skinName);
                //}
            }
        }
    }
}

