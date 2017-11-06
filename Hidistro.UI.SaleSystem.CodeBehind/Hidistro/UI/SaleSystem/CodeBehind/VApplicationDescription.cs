namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    [ParseChildren(true)]
    public class VApplicationDescription : VMemberTemplatedWebControl
    {
        private Literal litApplicationDescription;

        protected override void AttachChildControls()
        {
            this.litApplicationDescription = (Literal) this.FindControl("litApplicationDescription");
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(false,wid);
            this.litApplicationDescription.Text = masterSettings.ApplicationDescription;
            PageTitle.AddSiteNameTitle("分销商描述");
            this.Page.Session["stylestatus"] = "2";
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-VApplicationDescription.html";
            }
            base.OnInit(e);
        }
    }
}

