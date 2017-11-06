namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Entities.Members;
    using Hidistro.Entities.Orders;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    [ParseChildren(true)]
    public class VMemberCenter : VMemberTemplatedWebControl
    {
        private Image image;
        private HtmlInputHidden IsSign;
        private Literal litBindUser;
        private Literal litExpenditure;
        private Literal litMemberGrade;
        private Literal litPoints;
        private Literal litrGradeName;
        private Literal litSaleService;
        private Literal litUserName;
        private HtmlInputHidden txtShowDis;
        private HtmlInputHidden txtWaitForstr;

        protected override void AttachChildControls()
        {
            PageTitle.AddSiteNameTitle("会员中心");
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            if (currentMember == null)
            {
                base.GotoResourceNotFound("用户信息获取错误!");
            }
            this.litUserName = (Literal) this.FindControl("litUserName");
            this.litPoints = (Literal) this.FindControl("litPoints");
            this.litPoints.Text = currentMember.Points.ToString();
            this.image = (Image) this.FindControl("image");
            this.litBindUser = (Literal) this.FindControl("litBindUser");
            this.litExpenditure = (Literal) this.FindControl("litExpenditure");
            this.litExpenditure.SetWhenIsNotNull("￥" + currentMember.Expenditure.ToString("F2"));
            if (!string.IsNullOrEmpty(currentMember.UserBindName))
            {
                this.litBindUser.Text = " style=\"display:none\"";
            }
            MemberGradeInfo memberGrade = MemberProcessor.GetMemberGrade(currentMember.GradeId);
            this.litrGradeName = (Literal) this.FindControl("litrGradeName");
            if (memberGrade != null)
            {
                this.litrGradeName.Text = memberGrade.Name;
            }
            else
            {
                this.litrGradeName.Text = "普通会员";
            }
            this.litUserName.Text = string.IsNullOrEmpty(currentMember.RealName) ? currentMember.UserName : currentMember.RealName;
            if (!UserSignHelper.IsSign(currentMember.UserId))
            {
                this.IsSign = (HtmlInputHidden) this.FindControl("IsSign");
                this.IsSign.Value = "1";
            }
            if (!string.IsNullOrEmpty(currentMember.UserHead))
            {
                this.image.ImageUrl = currentMember.UserHead;
            }
            this.txtWaitForstr = (HtmlInputHidden) this.FindControl("txtWaitForstr");
            OrderQuery query = new OrderQuery {
                Status = OrderStatus.WaitBuyerPay
            };
            int userOrderCount = MemberProcessor.GetUserOrderCount(Globals.GetCurrentMemberUserId(), query);
            query.Status = OrderStatus.SellerAlreadySent;
            int num2 = MemberProcessor.GetUserOrderCount(Globals.GetCurrentMemberUserId(), query);
            int userOrderReturnCount = MemberProcessor.GetUserOrderReturnCount(Globals.GetCurrentMemberUserId());
            this.txtWaitForstr.Value = userOrderCount.ToString() + "|" + num2.ToString() + "|" + userOrderReturnCount.ToString();
            DistributorsInfo userIdDistributors = DistributorsBrower.GetUserIdDistributors(Globals.GetCurrentMemberUserId());
            this.txtShowDis = (HtmlInputHidden) this.FindControl("txtShowDis");
            if ((userIdDistributors == null) || (userIdDistributors.ReferralStatus != 0))
            {
                this.txtShowDis.Value = "false";
            }
            else
            {
                this.txtShowDis.Value = "true";
            }
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-VMemberCenter.html";
            }
            base.OnInit(e);
        }
    }
}

