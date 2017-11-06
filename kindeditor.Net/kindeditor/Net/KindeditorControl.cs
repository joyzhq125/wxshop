namespace kindeditor.Net
{
    using System;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Configuration;
    using System.Web.Security;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    [ToolboxData("<{0}:KindeditorControl runat=server></{0}:KindeditorControl>"), DefaultProperty("Text")]
    public class KindeditorControl : WebControl, IPostBackDataHandler
    {
        public bool LoadPostData(string postDataKey, NameValueCollection postCollection)
        {
            string str = postCollection[postDataKey];
            if (this.HtmlEncodeOutput)
            {
                str = str.Replace("&lt;", "<").Replace("&gt;", ">").Replace("&amp;", "&");
            }
            if (str != this.Text)
            {
                this.Text = str;
                return true;
            }
            return false;
        }

        public void RaisePostDataChangedEvent()
        {
        }

        protected override void RenderContents(HtmlTextWriter output)
        {
            if (this.ImportLib)
            {
                output.Write(string.Format("<link rel=\"stylesheet\" src=\"{0}themes/default/default.css\" />", this.BasePath));
                output.Write(string.Format("<script charset=\"utf-8\" src=\"{0}kindeditor.js\"></script>", this.BasePath));
                output.Write(string.Format("<script charset=\"utf-8\" src=\"{0}lang/zh_CN.js\"></script>", this.BasePath));
            }
            output.Write("<script type=\"text/javascript\">var auth = \"" + ((this.Page.Request.Cookies[FormsAuthentication.FormsCookieName] == null) ? string.Empty : this.Page.Request.Cookies[FormsAuthentication.FormsCookieName].Value) + "\";</script>");
            output.Write("<script type=\"text/javascript\">var editor;KindEditor.ready(function(K) {editor = K.create('#content_" + this.ID + "', {resizeType : 2,allowFileManager: true,allowFlashUpload:false,allowMediaUpload:false,IsAdvPositions:" + (this.IsAdvPositions ? "true" : "false") + ", fileManagerJson : '" + this.FileManagerJson + "',uploadJson:'" + this.UploadFileJson + "',fileCategoryJson :'" + this.FileCategoryJson + "'});});</script>");
            output.Write(string.Format("<textarea id=\"content_{0}\" name=\"{1}\" style=\"width:{2};height:{3};visibility:hidden;\">{4}</textarea>", new object[] { this.ID, this.UniqueID, this.Width, this.Height, this.Text }));
        }

        [Bindable(true), DefaultValue(""), Localizable(true), Category("Appearance")]
        public string BasePath
        {
            get
            {
                string str;
                string applicationPath;
                object obj2 = this.ViewState["BasePath"];
                if (obj2 == null)
                {
                    obj2 = ConfigurationSettings.AppSettings["KindEditor:BasePath"];
                    if (obj2 != null)
                    {
                        str = obj2.ToString();
                        applicationPath = this.Page.Request.ApplicationPath;
                        if (!applicationPath.EndsWith("/"))
                        {
                            applicationPath = applicationPath + "/";
                        }
                        str = str.Replace("~/", applicationPath);
                        if (!str.EndsWith("/"))
                        {
                            str = str + "/";
                        }
                    }
                    else
                    {
                        str = "/kindeditor/";
                    }
                    this.ViewState["BasePath"] = str;
                    return str;
                }
                str = obj2.ToString();
                if (str.StartsWith("~/"))
                {
                    applicationPath = this.Page.Request.ApplicationPath;
                    if (!applicationPath.EndsWith("/"))
                    {
                        applicationPath = applicationPath + "/";
                    }
                    str = str.Replace("~/", applicationPath);
                    this.ViewState["BasePath"] = str;
                }
                return str;
            }
            set
            {
                string str = value;
                if (!str.EndsWith("/"))
                {
                    str = str + "/";
                }
                this.ViewState["BasePath"] = str;
            }
        }

        [Localizable(true), Category("Appearance"), DefaultValue(""), Bindable(true)]
        public string FileCategoryJson
        {
            get
            {
                string str;
                string applicationPath;
                object obj2 = this.ViewState["FileCategoryJson"];
                if (obj2 == null)
                {
                    obj2 = ConfigurationSettings.AppSettings["KindEditor:FileCategoryJson"];
                    if (obj2 != null)
                    {
                        str = obj2.ToString();
                        applicationPath = this.Page.Request.ApplicationPath;
                        if (!applicationPath.EndsWith("/"))
                        {
                            applicationPath = applicationPath + "/";
                        }
                        str = str.Replace("~/", applicationPath);
                    }
                    else
                    {
                        str = "";
                    }
                    this.ViewState["FileCategoryJson"] = str;
                    return str;
                }
                str = obj2.ToString();
                if (str.StartsWith("~/"))
                {
                    applicationPath = this.Page.Request.ApplicationPath;
                    if (!applicationPath.EndsWith("/"))
                    {
                        applicationPath = applicationPath + "/";
                    }
                    str = str.Replace("~/", applicationPath);
                    this.ViewState["FileCategoryJson"] = str;
                }
                return str;
            }
            set
            {
                this.ViewState["FileCategoryJson"] = value;
            }
        }

        [Bindable(true), Category("Appearance"), Localizable(true), DefaultValue("")]
        public string FileManagerJson
        {
            get
            {
                string str;
                string applicationPath;
                object obj2 = this.ViewState["FileManagerJson"];
                if (obj2 == null)
                {
                    obj2 = ConfigurationSettings.AppSettings["KindEditor:FileManagerJson"];
                    if (obj2 != null)
                    {
                        str = obj2.ToString();
                        applicationPath = this.Page.Request.ApplicationPath;
                        if (!applicationPath.EndsWith("/"))
                        {
                            applicationPath = applicationPath + "/";
                        }
                        str = str.Replace("~/", applicationPath);
                    }
                    else
                    {
                        str = "";
                    }
                    this.ViewState["FileManagerJson"] = str;
                    return str;
                }
                str = obj2.ToString();
                if (str.StartsWith("~/"))
                {
                    applicationPath = this.Page.Request.ApplicationPath;
                    if (!applicationPath.EndsWith("/"))
                    {
                        applicationPath = applicationPath + "/";
                    }
                    str = str.Replace("~/", applicationPath);
                    this.ViewState["FileManagerJson"] = str;
                }
                return str;
            }
            set
            {
                this.ViewState["FileManagerJson"] = value;
            }
        }

        [Localizable(true), Bindable(true), Category("Appearance"), DefaultValue(300)]
        public override Unit Height
        {
            get
            {
                object obj2 = this.ViewState["Height"];
                return ((obj2 == null) ? 300 : ((Unit) obj2));
            }
            set
            {
                this.ViewState["Height"] = value;
            }
        }

        [Localizable(true), Bindable(true), Category("Appearance"), DefaultValue(false)]
        public bool HtmlEncodeOutput
        {
            get
            {
                object obj2 = this.ViewState["HtmlEncodeOutput"];
                return ((obj2 != null) && ((bool) obj2));
            }
            set
            {
                this.ViewState["HtmlEncodeOutput"] = value;
            }
        }

        [Localizable(true), DefaultValue(true), Bindable(true), Category("Appearance")]
        public bool ImportLib
        {
            get
            {
                object obj2 = this.ViewState["ImportLib"];
                return ((obj2 == null) || ((bool) obj2));
            }
            set
            {
                this.ViewState["ImportLib"] = value;
            }
        }

        [Category("Appearance"), DefaultValue(false), Localizable(true), Bindable(true)]
        public bool IsAdvPositions
        {
            get
            {
                object obj2 = this.ViewState["IsAdvPositions"];
                return ((obj2 != null) && ((bool) obj2));
            }
            set
            {
                this.ViewState["IsAdvPositions"] = value;
            }
        }

        [Bindable(true), Localizable(true), Category("Appearance"), DefaultValue(false)]
        public bool SimpleMode
        {
            get
            {
                object obj2 = this.ViewState["SimpleMode"];
                return ((obj2 != null) && ((bool) obj2));
            }
            set
            {
                this.ViewState["SimpleMode"] = value;
            }
        }

        [Localizable(true), Bindable(true), Category("Appearance"), DefaultValue("")]
        public string Text
        {
            get
            {
                string str = (string) this.ViewState["Text"];
                return ((str == null) ? "" : str);
            }
            set
            {
                this.ViewState["Text"] = value;
            }
        }

        [Category("Appearance"), Localizable(true), DefaultValue(""), Bindable(true)]
        public string UploadFileJson
        {
            get
            {
                string str;
                string applicationPath;
                object obj2 = this.ViewState["UploadFileJson"];
                if (obj2 == null)
                {
                    obj2 = ConfigurationSettings.AppSettings["KindEditor:UploadFileJson"];
                    if (obj2 != null)
                    {
                        str = obj2.ToString();
                        applicationPath = this.Page.Request.ApplicationPath;
                        if (!applicationPath.EndsWith("/"))
                        {
                            applicationPath = applicationPath + "/";
                        }
                        str = str.Replace("~/", applicationPath);
                    }
                    else
                    {
                        str = "";
                    }
                    this.ViewState["UploadFileJson"] = str;
                    return str;
                }
                str = obj2.ToString();
                if (str.StartsWith("~/"))
                {
                    applicationPath = this.Page.Request.ApplicationPath;
                    if (!applicationPath.EndsWith("/"))
                    {
                        applicationPath = applicationPath + "/";
                    }
                    str = str.Replace("~/", applicationPath);
                    this.ViewState["UploadFileJson"] = str;
                }
                return str;
            }
            set
            {
                this.ViewState["UploadFileJson"] = value;
            }
        }

        [Category("Appearance"), Localizable(true), Bindable(true), DefaultValue(700)]
        public override Unit Width
        {
            get
            {
                object obj2 = this.ViewState["Width"];
                return ((obj2 == null) ? 700 : ((Unit) obj2));
            }
            set
            {
                this.ViewState["Width"] = value;
            }
        }
    }
}

