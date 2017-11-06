namespace ASPNET.WebControls
{
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class PageSize : WebControl
    {
        private int defaultPageSize = 10;
        private string urlFormat;

        public PageSize()
        {
            this.SelectedSizeCss = "selectthis";
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.urlFormat = this.Context.Request.RawUrl;
            if (this.Context.Request.QueryString.Count > 0)
            {
                this.urlFormat = this.urlFormat.Replace(this.Context.Request.Url.Query, "?");
                foreach (string str in this.Context.Request.QueryString.Keys)
                {
                    if ((string.Compare(str, "pagesize", true) != 0) && (string.Compare(str, "pageindex", true) != 0))
                    {
                        string urlFormat = this.urlFormat;
                        this.urlFormat = urlFormat + str + "=" + this.Page.Server.UrlEncode(this.Context.Request.QueryString[str]) + "&";
                    }
                }
            }
            this.urlFormat = this.urlFormat + (this.urlFormat.Contains("?") ? "pagesize=" : "?pagesize=");
        }

        protected override void Render(HtmlTextWriter writer)
        {
            this.RenderButton(writer);
        }

        private void RenderButton(HtmlTextWriter writer)
        {
            WebControl control = new WebControl(HtmlTextWriterTag.A);
            control.Controls.Add(new LiteralControl("10"));
            control.Attributes.Add("href", this.urlFormat + "10");
            if (this.SelectedSize == 10)
            {
                control.Attributes.Add("class", this.SelectedSizeCss);
            }
            control.RenderControl(writer);
            WebControl control2 = new WebControl(HtmlTextWriterTag.A);
            control2.Controls.Add(new LiteralControl("20"));
            control2.Attributes.Add("href", this.urlFormat + "20");
            if (this.SelectedSize == 20)
            {
                control2.Attributes.Add("class", this.SelectedSizeCss);
            }
            control2.RenderControl(writer);
            WebControl control3 = new WebControl(HtmlTextWriterTag.A);
            control3.Controls.Add(new LiteralControl("40"));
            control3.Attributes.Add("href", this.urlFormat + "40");
            if (this.SelectedSize == 40)
            {
                control3.Attributes.Add("class", this.SelectedSizeCss);
            }
            control3.RenderControl(writer);
            WebControl control4 = new WebControl(HtmlTextWriterTag.A);
            control4.Controls.Add(new LiteralControl("100"));
            control4.Attributes.Add("href", this.urlFormat + "100");
            if (this.SelectedSize == 100)
            {
                control4.Attributes.Add("class", this.SelectedSizeCss);
            }
            control4.RenderControl(writer);
        }

        public int DefaultPageSize
        {
            get
            {
                return this.defaultPageSize;
            }
            set
            {
                this.defaultPageSize = value;
            }
        }

        [Browsable(false)]
        public int SelectedSize
        {
            get
            {
                int defaultPageSize = this.defaultPageSize;
                if (!string.IsNullOrEmpty(this.Context.Request.QueryString["pagesize"]))
                {
                    int.TryParse(this.Context.Request.QueryString["pagesize"], out defaultPageSize);
                }
                if (defaultPageSize <= 0)
                {
                    return this.defaultPageSize;
                }
                return defaultPageSize;
            }
        }

        public string SelectedSizeCss { get; set; }
    }
}

