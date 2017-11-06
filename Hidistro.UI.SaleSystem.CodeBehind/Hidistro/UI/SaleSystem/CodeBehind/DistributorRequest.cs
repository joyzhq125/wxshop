namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.Entities.Members;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;

    [ParseChildren(true)]
    public class DistributorRequest : VMemberTemplatedWebControl
    {
        private HtmlInputHidden litIsEnable;

        protected override void AttachChildControls()
        {
            PageTitle.AddSiteNameTitle("申请分销");
            this.Page.Session["stylestatus"] = "2";
            DistributorsInfo userIdDistributors = DistributorsBrower.GetUserIdDistributors(MemberProcessor.GetCurrentMember().UserId);
            if ((userIdDistributors != null) && (userIdDistributors.ReferralStatus == 0))
            {
                this.Page.Response.Redirect("DistributorCenter.aspx", true);
            }
            if ((userIdDistributors != null) && (userIdDistributors.ReferralStatus != 0))
            {
                this.litIsEnable = (HtmlInputHidden) this.FindControl("litIsEnable");
                this.litIsEnable.Value = userIdDistributors.ReferralStatus.ToString();
            }
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-VDistributorRequest.html";
            }
            base.OnInit(e);
        }
    }
}

