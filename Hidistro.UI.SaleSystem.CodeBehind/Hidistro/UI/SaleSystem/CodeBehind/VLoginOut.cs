namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.IO;
    using System.Net;
    using System.Text;
    using System.Web;
    using System.Web.UI;

    [ParseChildren(true)]
    public class VLoginOut : VshopTemplatedWebControl
    {
        protected override void AttachChildControls()
        {
            if (HttpContext.Current.Request.Cookies.Get("Vshop-Member") != null)
            {
                HiAffiliation.ClearUserCookie("", false);
            }
            if (HttpContext.Current.Request.Cookies.Get("Vshop-ReferralId") != null)
            {
                HiAffiliation.ClearReferralIdCookie();
            }
            if (HttpContext.Current.Request.Cookies.Get("Vshop-Wid") != null)
            {
                HiAffiliation.ClearWidCookie("", false);
            }
            this.Page.Response.Redirect(Globals.ApplicationPath + "/Default.aspx");
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
                this.SkinName = "Skin-VLogout.html";
            }
            base.OnInit(e);
        }
    }
}

