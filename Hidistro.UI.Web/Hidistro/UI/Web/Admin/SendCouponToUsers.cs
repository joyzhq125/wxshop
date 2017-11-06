namespace Hidistro.UI.Web.Admin
{
    using Hidistro.ControlPanel.Members;
    using Hidistro.ControlPanel.Promotions;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Entities.Members;
    using Hidistro.Entities.Promotions;
    using Hidistro.Entities.Store;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Collections.Generic;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    [PrivilegeCheck(Privilege.Coupons)]
    public class SendCouponToUsers : AdminPage
    {
        protected Button btnSend;
        private int couponId;
        protected MemberGradeDropDownList rankList;
        protected HtmlInputRadioButton rdoName;
        protected HtmlInputRadioButton rdoRank;
        protected TextBox txtMemberNames;

        protected SendCouponToUsers() : base("", "")
        {
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            CouponItemInfo item = new CouponItemInfo();
            IList<CouponItemInfo> listCouponItem = new List<CouponItemInfo>();
            IList<MemberInfo> memdersByOpenIds = new List<MemberInfo>();
            if (this.rdoName.Checked)
            {
                if (!string.IsNullOrEmpty(this.txtMemberNames.Text.Trim()))
                {
                    memdersByOpenIds = MemberHelper.GetMemdersByOpenIds("'" + this.txtMemberNames.Text.Trim().Replace("\r\n", "\n").Replace("\n", "','") + "'");
                }
                string claimCode = string.Empty;
                foreach (MemberInfo info2 in memdersByOpenIds)
                {
                    claimCode = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 15);
                    item = new CouponItemInfo(this.couponId, claimCode, new int?(info2.UserId), info2.UserName, info2.Email, DateTime.Now);
                    listCouponItem.Add(item);
                }
                if (listCouponItem.Count <= 0)
                {
                    this.ShowMsg("你输入的OpenId中没有一个正确的，请输入正确的OpenId", false);
                    return;
                }
                CouponHelper.SendClaimCodes(this.couponId, listCouponItem);
                this.txtMemberNames.Text = string.Empty;
                this.ShowMsg(string.Format("此次发送操作已成功，优惠券发送数量：{0}", listCouponItem.Count), true);
            }
            if (this.rdoRank.Checked)
            {
                memdersByOpenIds = MemberHelper.GetMembersByRank(this.rankList.SelectedValue);
                string str3 = string.Empty;
                foreach (MemberInfo info3 in memdersByOpenIds)
                {
                    str3 = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 15);
                    item = new CouponItemInfo(this.couponId, str3, new int?(info3.UserId), info3.UserName, info3.Email, DateTime.Now);
                    listCouponItem.Add(item);
                }
                if (listCouponItem.Count <= 0)
                {
                    this.ShowMsg("您选择的会员等级下面没有会员", false);
                }
                else
                {
                    CouponHelper.SendClaimCodes(this.couponId, listCouponItem);
                    this.txtMemberNames.Text = string.Empty;
                    this.ShowMsg(string.Format("此次发送操作已成功，优惠券发送数量：{0}", listCouponItem.Count), true);
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!int.TryParse(this.Page.Request.QueryString["couponId"], out this.couponId))
            {
                base.GotoResourceNotFound();
            }
            else
            {
                this.btnSend.Click += new EventHandler(this.btnSend_Click);
                if (!this.Page.IsPostBack)
                {
                    this.rankList.DataBind();
                }
            }
        }
    }
}

