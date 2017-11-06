namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.Core;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.Common.Controls;
    using Newtonsoft.Json.Linq;
    using System;
    using System.Linq;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    [ParseChildren(true)]
    public class VRequestDistributorFinish : VMemberTemplatedWebControl
    {
        private Literal litDescirption;

        protected override void AttachChildControls()
        {
            PageTitle.AddSiteNameTitle("去看店铺");
            this.litDescirption = (Literal) this.FindControl("litDescirption");
            this.litDescirption.Text = SettingsManager.GetMasterSettings(false,wid).DistributorDescription;
            if (!this.Page.IsPostBack)
            {
                HttpCookie cookie = HttpContext.Current.Request.Cookies["SelectProcutId"];
                if ((cookie != null) && !string.IsNullOrEmpty(cookie.Value))
                {
                    Newtonsoft.Json.Linq.IJEnumerable<JToken> sss = Extensions.Values(JObject.Parse(Globals.UrlDecode(cookie.Value)));
                    DistributorsBrower.AddDistributorProductId((from s in sss select (int)s).ToList<int>());
                    HttpContext.Current.Response.Cookies["SelectProcutId"].Value = null;
                }
            }
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-RequestDistributorFinish.html";
            }
            base.OnInit(e);
        }
    }
}

