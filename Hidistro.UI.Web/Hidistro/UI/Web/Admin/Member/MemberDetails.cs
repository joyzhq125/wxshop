namespace Hidistro.UI.Web.Admin.Member
{
    using Hidistro.ControlPanel.Members;
    using Hidistro.Core;
    using Hidistro.Entities;
    using Hidistro.Entities.Members;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Globalization;
    using System.Web;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class MemberDetails : AdminPage
    {
        protected Button btnEdit;
        private int currentUserId;
        protected TextBox lblUserLink;
        protected TextBox litAddress;
        protected TextBox litCellPhone;
        protected TextBox litCreateDate;
        protected TextBox litEmail;
        protected TextBox litGrade;
        protected TextBox litOpenId;
        protected TextBox litQQ;
        protected TextBox litRealName;
        protected Literal litUserName;
        protected HtmlForm thisForm;

        protected MemberDetails() : base("m04", "hyp02")
        {
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            base.Response.Redirect(Globals.GetAdminAbsolutePath("/member/EditMember.aspx?userId=" + this.Page.Request.QueryString["userId"]), true);
        }

        private void LoadMemberInfo()
        {
            MemberInfo member = MemberHelper.GetMember(this.currentUserId);
            if (member == null)
            {
                base.GotoResourceNotFound();
            }
            else
            {
                Uri url = HttpContext.Current.Request.Url;
                string str = (url.Port == 80) ? string.Empty : (":" + url.Port.ToString(CultureInfo.InvariantCulture));
                this.lblUserLink.Text = string.Concat(new object[] { string.Format(CultureInfo.InvariantCulture, "{0}://{1}{2}", new object[] { url.Scheme, SettingsManager.GetMasterSettings(true,wid).SiteUrl, str }), Globals.ApplicationPath, "/?ReferralUserId=", member.UserId });
                this.litUserName.Text = member.UserName;
                this.litGrade.Text = MemberHelper.GetMemberGrade(member.GradeId).Name;
                this.litCreateDate.Text = member.CreateDate.ToString();
                this.litRealName.Text = member.RealName;
                this.litAddress.Text = RegionHelper.GetFullRegion(member.RegionId, "") + member.Address;
                this.litQQ.Text = member.QQ;
                this.litCellPhone.Text = member.CellPhone;
                this.litEmail.Text = member.Email;
                this.litOpenId.Text = member.OpenId;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            wid = GetCurWebId();
            if (string.IsNullOrEmpty(wid)) return;
            if (!int.TryParse(this.Page.Request.QueryString["userId"], out this.currentUserId))
            {
                base.GotoResourceNotFound();
            }
            else
            {
                this.btnEdit.Click += new EventHandler(this.btnEdit_Click);
                if (!this.Page.IsPostBack)
                {
                    this.LoadMemberInfo();
                }
            }
        }
    }
}

