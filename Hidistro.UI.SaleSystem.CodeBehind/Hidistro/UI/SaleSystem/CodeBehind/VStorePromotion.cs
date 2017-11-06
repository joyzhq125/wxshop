namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.Core;
    using Hidistro.Entities.Members;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Web.UI.WebControls;

    public class VStorePromotion : VshopTemplatedWebControl
    {
        private Literal litLinkurl;
        private Literal litStoreurl;
        private Literal litStroeDesc;
        private Literal litStroeName;
        private Literal liturl;
        private Image Logoimage;
        private int userId;

        protected override void AttachChildControls()
        {
            PageTitle.AddSiteNameTitle("店铺推广");
            if (!int.TryParse(this.Page.Request.QueryString["UserId"], out this.userId))
            {
                base.GotoResourceNotFound("");
            }
            DistributorsInfo userIdDistributors = DistributorsBrower.GetUserIdDistributors(this.userId);
            if (userIdDistributors == null)
            {
                base.GotoResourceNotFound("");
            }
            this.litStroeDesc = (Literal) this.FindControl("litStroeDesc");
            this.litLinkurl = (Literal) this.FindControl("litLinkurl");
            this.litStoreurl = (Literal) this.FindControl("litStoreurl");
            string str = Globals.FullPath("/Default.aspx?ReferralId=" + this.userId);
            this.litLinkurl.Text = str;
            this.litStoreurl.Text = str;
            this.Logoimage = (Image) this.FindControl("Logoimage");
            if (!string.IsNullOrEmpty(userIdDistributors.Logo))
            {
                this.Logoimage.ImageUrl = Globals.HostPath(this.Page.Request.Url) + userIdDistributors.Logo;
            }
            this.litStroeName = (Literal) this.FindControl("litStroeName");
            this.litStroeName.Text = userIdDistributors.StoreName;
            this.litStroeDesc.Text = userIdDistributors.StoreDescription;
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-VStorePromotion.html";
            }
            base.OnInit(e);
        }
    }
}

