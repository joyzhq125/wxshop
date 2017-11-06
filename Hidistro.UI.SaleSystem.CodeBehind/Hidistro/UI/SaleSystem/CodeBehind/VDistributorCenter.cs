namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Entities.Members;
    using Hidistro.Entities.Orders;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Data;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    [ParseChildren(true)]
    public class VDistributorCenter : VMemberTemplatedWebControl
    {
        private Image imgGrade;
        private Image imglogo;
        private Literal litdistirbutors;
        private Literal litMysubFirst;
        private Literal litMysubSecond;
        private Literal litMysubStore;
        private Literal litOrders;
        private Literal litProtuctNum;
        private Literal litReferralBlance;
        private Literal litrGradeName;
        private Literal litStroeName;
        private Literal litTodayOrdersNum;
        private Literal litUserId;
        private Literal litUserId1;
        private Literal litUserId2;
        private FormatedMoneyLabel refrraltotal;
        private FormatedMoneyLabel saletotal;

        protected override void AttachChildControls()
        {
            PageTitle.AddSiteNameTitle("店铺中心");
            int currentMemberUserId = Globals.GetCurrentMemberUserId();
            DistributorsInfo userIdDistributors = DistributorsBrower.GetUserIdDistributors(currentMemberUserId);
            if (userIdDistributors == null)
            {
                HttpContext.Current.Response.Redirect("DistributorRegCheck.aspx");
            }
            else if (userIdDistributors.ReferralStatus != 0)
            {
                HttpContext.Current.Response.Redirect("MemberCenter.aspx");
            }
            else
            {
                this.imglogo = (Image) this.FindControl("image");
                if (!string.IsNullOrEmpty(userIdDistributors.Logo))
                {
                    this.imglogo.ImageUrl = userIdDistributors.Logo;
                }
                this.litStroeName = (Literal) this.FindControl("litStroeName");
                this.litStroeName.Text = userIdDistributors.StoreName;
                this.litrGradeName = (Literal) this.FindControl("litrGradeName");
                DistributorGradeInfo distributorGradeInfo = DistributorGradeBrower.GetDistributorGradeInfo(userIdDistributors.DistriGradeId);
                this.litrGradeName.Text = distributorGradeInfo.Name;
                this.litReferralBlance = (Literal) this.FindControl("litReferralBlance");
                this.litReferralBlance.Text = userIdDistributors.ReferralBlance.ToString("F2");
                this.litUserId = (Literal) this.FindControl("litUserId");
                this.litUserId1 = (Literal) this.FindControl("litUserId1");
                this.litUserId2 = (Literal) this.FindControl("litUserId2");
                this.litUserId.Text = userIdDistributors.UserId.ToString();
                this.litUserId1.Text = userIdDistributors.UserId.ToString();
                this.litUserId2.Text = userIdDistributors.UserId.ToString();
                this.litTodayOrdersNum = (Literal) this.FindControl("litTodayOrdersNum");
                OrderQuery query = new OrderQuery {
                    UserId = new int?(currentMemberUserId),
                    Status = OrderStatus.Today
                };
                this.litTodayOrdersNum.Text = DistributorsBrower.GetDistributorOrderCount(query).ToString();
                this.refrraltotal = (FormatedMoneyLabel) this.FindControl("refrraltotal");
                this.refrraltotal.Money = DistributorsBrower.GetUserCommissions(userIdDistributors.UserId, DateTime.Now, null, null, null);
                this.saletotal = (FormatedMoneyLabel) this.FindControl("saletotal");
                this.saletotal.Money = userIdDistributors.OrdersTotal;
                this.saletotal = (FormatedMoneyLabel) this.FindControl("saletotal");
                this.saletotal.Money = userIdDistributors.OrdersTotal;
                this.litMysubStore = (Literal) this.FindControl("litMysubStore");
                this.litMysubFirst = (Literal) this.FindControl("litMysubFirst");
                this.litMysubSecond = (Literal) this.FindControl("litMysubSecond");
                DataTable distributorsSubStoreNum = VShopHelper.GetDistributorsSubStoreNum(userIdDistributors.UserId);
                if ((distributorsSubStoreNum != null) || (distributorsSubStoreNum.Rows.Count > 0))
                {
                    this.litMysubFirst.Text = distributorsSubStoreNum.Rows[0]["firstV"].ToString();
                    this.litMysubSecond.Text = distributorsSubStoreNum.Rows[0]["secondV"].ToString();
                    this.litMysubStore.Text = (int.Parse(this.litMysubFirst.Text) + int.Parse(this.litMysubSecond.Text)).ToString();
                }
                else
                {
                    this.litMysubFirst.Text = "0";
                    this.litMysubSecond.Text = "0";
                    this.litMysubStore.Text = "0";
                }
                this.litProtuctNum = (Literal) this.FindControl("litProtuctNum");
                this.litProtuctNum.Text = ProductBrowser.GetProductsNumber().ToString();
                query.Status = OrderStatus.All;
                this.litOrders = (Literal) this.FindControl("litOrders");
                this.litOrders.Text = DistributorsBrower.GetDistributorOrderCount(query).ToString();
            }
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-DistributorCenter.html";
            }
            base.OnInit(e);
        }
    }
}

