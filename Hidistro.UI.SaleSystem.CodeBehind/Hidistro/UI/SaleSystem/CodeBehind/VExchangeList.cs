namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.ControlPanel.Promotions;
    using Hidistro.Entities.Members;
    using Hidistro.Entities.Promotions;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    [ParseChildren(true)]
    public class VExchangeList : VshopTemplatedWebControl
    {
        private int gradeId;
        private int id;
        private Image imgCover;
        private Literal litPoints;

        protected override void AttachChildControls()
        {
            if (!int.TryParse(this.Page.Request.QueryString["id"], out this.id))
            {
                base.GotoResourceNotFound("");
            }
            PointExChangeInfo info = PointExChangeHelper.Get(this.id);
            if (info != null)
            {
                PageTitle.AddSiteNameTitle("积分兑换");
                MemberInfo currentMember = MemberProcessor.GetCurrentMember();
                if (currentMember != null)
                {
                    this.imgCover = (Image) this.FindControl("imgCover");
                    this.litPoints = (Literal) this.FindControl("litPoints");
                    this.litPoints.Text = currentMember.Points.ToString();
                    this.gradeId = currentMember.GradeId;
                    this.imgCover.ImageUrl = !string.IsNullOrEmpty(info.ImgUrl) ? info.ImgUrl : "http://fpoimg.com/640x220";
                }
            }
            else
            {
                HttpContext.Current.Response.Redirect("/vshop/");
                HttpContext.Current.Response.End();
            }
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-VExchangeList.html";
            }
            base.OnInit(e);
        }
    }
}

