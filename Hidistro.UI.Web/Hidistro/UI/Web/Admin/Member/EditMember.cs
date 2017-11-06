namespace Hidistro.UI.Web.Admin.Member
{
    using Hidistro.ControlPanel.Members;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Entities;
    using Hidistro.Entities.Members;
    using Hidistro.Entities.Store;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using Hishop.Components.Validation;
    using System;
    using System.Collections.Generic;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    [PrivilegeCheck(Privilege.EditMember)]
    public class EditMember : AdminPage
    {
        protected Button btnEditUser;
        private int currentUserId;
        protected MemberGradeDropDownList drpMemberRankList;
        protected Literal lblLoginNameValue;
        protected FormatedTimeLabel lblRegsTimeValue;
        protected Literal lblTotalAmountValue;
        protected RegionSelector rsddlRegion;
        protected HtmlForm thisForm;
        protected TextBox txtAddress;
        protected TextBox txtCellPhone;
        protected TextBox txtprivateEmail;
        protected TextBox txtQQ;
        protected TextBox txtRealName;

        protected EditMember() : base("m04", "hyp02")
        {
        }

        protected void btnEditUser_Click(object sender, EventArgs e)
        {
            MemberInfo member = MemberHelper.GetMember(this.currentUserId);
            member.GradeId = this.drpMemberRankList.SelectedValue.Value;
            member.RealName = this.txtRealName.Text.Trim();
            if (this.rsddlRegion.GetSelectedRegionId().HasValue)
            {
                member.RegionId = this.rsddlRegion.GetSelectedRegionId().Value;
                member.TopRegionId = RegionHelper.GetTopRegionId(member.RegionId);
            }
            member.Address = Globals.HtmlEncode(this.txtAddress.Text);
            member.QQ = this.txtQQ.Text;
            member.Email = member.QQ + "@qq.com";
            member.CellPhone = this.txtCellPhone.Text;
            member.Email = this.txtprivateEmail.Text;
            if (this.ValidationMember(member))
            {
                if (MemberHelper.Update(member))
                {
                    this.ShowMsgAndReUrl("成功修改了当前会员的个人资料", true, "/Admin/member/managemembers.aspx");
                }
                else
                {
                    this.ShowMsg("当前会员的个人信息修改失败", false);
                }
            }
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
                this.drpMemberRankList.SelectedValue = new int?(member.GradeId);
                this.lblLoginNameValue.Text = member.UserName;
                this.lblRegsTimeValue.Time = member.CreateDate;
                this.lblTotalAmountValue.Text = Globals.FormatMoney(member.Expenditure);
                this.txtRealName.Text = member.RealName;
                this.txtAddress.Text = Globals.HtmlDecode(member.Address);
                this.rsddlRegion.SetSelectedRegionId(new int?(member.RegionId));
                this.txtQQ.Text = member.QQ;
                this.txtCellPhone.Text = member.CellPhone;
                this.txtprivateEmail.Text = member.Email;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!int.TryParse(this.Page.Request.QueryString["userId"], out this.currentUserId))
            {
                base.GotoResourceNotFound();
            }
            else
            {
                this.btnEditUser.Click += new EventHandler(this.btnEditUser_Click);
                if (!this.Page.IsPostBack)
                {
                    this.drpMemberRankList.AllowNull = false;
                    this.drpMemberRankList.DataBind();
                    this.LoadMemberInfo();
                }
            }
        }

        private bool ValidationMember(MemberInfo member)
        {
            ValidationResults results = Hishop.Components.Validation.Validation.Validate<MemberInfo>(member, new string[] { "ValMember" });
            string msg = string.Empty;
            if (!results.IsValid)
            {
                foreach (ValidationResult result in (IEnumerable<ValidationResult>) results)
                {
                    msg = msg + Formatter.FormatErrorMessage(result.Message);
                }
                this.ShowMsg(msg, false);
            }
            return results.IsValid;
        }
    }
}

