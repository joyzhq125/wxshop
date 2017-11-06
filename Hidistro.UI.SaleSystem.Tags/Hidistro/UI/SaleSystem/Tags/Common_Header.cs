namespace Hidistro.UI.SaleSystem.Tags
{
    using Hidistro.UI.Common.Controls;
    using System;

    public class Common_Header : VshopTemplatedWebControl
    {
        //private PageTitle pageTitle;
        protected override void AttachChildControls()
        {
            //this.pageTitle = (PageTitle)this.FindControl("pagetitle");
            //PageTitle.wid = this.wid;
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "tags/skin-Common_Header.html";
            }
            base.OnInit(e);
        }
    }
}

