namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.Core;
    using Hidistro.Entities.Members;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Web.UI.HtmlControls;

    public class VDistributorRegCheck : VshopTemplatedWebControl
    {
        protected string IsEnable = "0";
        private HtmlInputHidden litExpenditure;
        private HtmlInputHidden litIsEnable;
        private HtmlInputHidden litIsMember;
        private HtmlInputHidden litminMoney;

        protected override void AttachChildControls()
        {
            PageTitle.AddSiteNameTitle("申请分销商");
            this.litIsEnable = (HtmlInputHidden) this.FindControl("litIsEnable");
            this.litIsMember = (HtmlInputHidden) this.FindControl("litIsMember");
            this.litExpenditure = (HtmlInputHidden) this.FindControl("litExpenditure");
            this.litminMoney = (HtmlInputHidden) this.FindControl("litminMoney");
            int finishedOrderMoney = SettingsManager.GetMasterSettings(true,wid).FinishedOrderMoney;
            this.litminMoney.Value = finishedOrderMoney.ToString();
            int currentMemberUserId = Globals.GetCurrentMemberUserId();
            if (currentMemberUserId > 0)
            {
                this.litIsMember.Value = "1";
                DistributorsInfo userIdDistributors = DistributorsBrower.GetUserIdDistributors(currentMemberUserId);
                MemberInfo currentMember = MemberProcessor.GetCurrentMember();
                this.litExpenditure.Value = currentMember.Expenditure.ToString("F2");
                if (userIdDistributors != null)
                {
                    if (userIdDistributors.ReferralStatus == 0)
                    {
                        this.IsEnable = "1";
                    }
                    else if (userIdDistributors.ReferralStatus == 1)
                    {
                        this.IsEnable = "3";
                    }
                    else if (userIdDistributors.ReferralStatus == 9)
                    {
                        this.IsEnable = "9";
                    }
                }
                else if (currentMember.Expenditure >= finishedOrderMoney)
                {
                    this.IsEnable = "2";
                }
            }
            else
            {
                this.litIsMember.Value = "0";
            }
            this.litIsEnable.Value = this.IsEnable;
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-VDistributorRegCheck.html";
            }
            base.OnInit(e);
        }
    }
}

