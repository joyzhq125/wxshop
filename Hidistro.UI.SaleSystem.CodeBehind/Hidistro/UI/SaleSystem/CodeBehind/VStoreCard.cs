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
    public class VStoreCard : VshopTemplatedWebControl
    {
        private int userId;

        protected override void AttachChildControls()
        {
            if (!int.TryParse(this.Page.Request.QueryString["ReferralId"], out this.userId))
            {
                base.GotoResourceNotFound("");
            }
            MemberInfo member = MemberProcessor.GetMember(this.userId, true);
            if (member != null)
            {
                DistributorsInfo userIdDistributors = DistributorsBrower.GetUserIdDistributors(member.UserId);
                if (userIdDistributors != null)
                {
                    Literal literal = (Literal) this.FindControl("RealName");
                    literal.Text = member.RealName;
                    if (string.IsNullOrEmpty(member.RealName))
                    {
                        literal.Text = member.UserName;
                    }
                    Literal literal2 = (Literal) this.FindControl("StroeDesc");
                    Literal literal3 = (Literal) this.FindControl("StoreName");
                    literal2.Text = userIdDistributors.StoreDescription;
                    literal3.Text = userIdDistributors.StoreName;
                    HiImage image = (HiImage) this.FindControl("imgHead");
                    HiImage image2 = (HiImage) this.FindControl("imgCode");
                    Literal literal1 = (Literal) this.FindControl("litStoreUrl");
                    if (!string.IsNullOrEmpty(member.UserHead))
                    {
                        image.ImageUrl = member.UserHead;
                    }
                    else
                    {
                        MemberInfo currentMember = MemberProcessor.GetCurrentMember();
                        if (((currentMember != null) && (currentMember.UserId == this.userId)) && this.Page.Request.UserAgent.ToLower().Contains("micromessenger"))
                        {
                            HttpContext.Current.Response.Redirect("ReGetWeiXinUserInfos.aspx?returnUrl=StoreCard.aspx%3fUserId%3d" + this.userId);
                        }
                    }
                    string str2 = Globals.FullPath("/Default.aspx?ReferralId=" + userIdDistributors.UserId);
                    image2.ImageUrl = "http://s.jiathis.com/qrcode.php?url=" + str2;
                }
                else
                {
                    base.GotoResourceNotFound("");
                }
            }
            else
            {
                base.GotoResourceNotFound("");
            }
            PageTitle.AddSiteNameTitle("掌柜名片");
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "skin-VStoreCard.html";
            }
            base.OnInit(e);
        }
    }
}

