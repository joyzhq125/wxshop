namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.Core;
    using Hidistro.Entities.Members;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    [ParseChildren(true)]
    public class VChirldrenDistributors : VMemberTemplatedWebControl
    {
        private Panel onedistributor;
        private VshopTemplatedRepeater rpdistributor;
        private Panel twodistributor;

        protected override void AttachChildControls()
        {
            PageTitle.AddSiteNameTitle("下级分销商");
            this.onedistributor = (Panel) this.FindControl("onedistributor");
            this.twodistributor = (Panel) this.FindControl("twodistributor");
            this.rpdistributor = (VshopTemplatedRepeater) this.FindControl("rpdistributor");
            DistributorsQuery query = new DistributorsQuery {
                PageIndex = 1,
                PageSize = 0x2710
            };
            DistributorsInfo currentDistributors = DistributorsBrower.GetCurrentDistributors(Globals.GetCurrentMemberUserId());
            if (currentDistributors.ReferralStatus != 0)
            {
                HttpContext.Current.Response.Redirect("MemberCenter.aspx");
            }
            else
            {
                query.GradeId = 2;
                this.twodistributor.Visible = false;
                int result = 0;
                if (int.TryParse(this.Page.Request.QueryString["gradeId"], out result))
                {
                    query.GradeId = result;
                }
                query.ReferralPath = currentDistributors.UserId.ToString();
                query.UserId = currentDistributors.UserId;
                this.rpdistributor.DataSource = DistributorsBrower.GetDownDistributors(query);
                this.rpdistributor.DataBind();
            }
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-VChirldrenDistributors.html";
            }
            base.OnInit(e);
        }
    }
}

