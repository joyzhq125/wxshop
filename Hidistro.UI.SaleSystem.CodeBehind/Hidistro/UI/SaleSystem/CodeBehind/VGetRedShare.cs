namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.ControlPanel.Promotions;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Members;
    using Hidistro.Entities.Promotions;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Data;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    [ParseChildren(true)]
    public class VGetRedShare : VshopTemplatedWebControl
    {
        private Literal litItemParams;

        protected override void AttachChildControls()
        {
            this.litItemParams = (Literal) this.FindControl("litItemParams");
            string str = HttpContext.Current.Request.QueryString.Get("shareid");
            int result = 0;
            if (!string.IsNullOrEmpty(str))
            {
                if (int.TryParse(str, out result))
                {
                    DataView defaultView = ShareActHelper.GetShareActivity().DefaultView;
                    defaultView.RowFilter = " id=" + result;
                    if (defaultView.Count > 0)
                    {
                        ShareActivityInfo act = ShareActHelper.GetAct(result);
                        if (act != null)
                        {
                            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
                            if (act.ShareTitle.Contains("{{微信昵称}}"))
                            {
                                act.ShareTitle = act.ShareTitle.Replace("{{微信昵称}}", currentMember.UserName);
                            }
                            if (act.Description.Contains("{{店铺名称}}"))
                            {
                                HttpCookie cookie = new HttpCookie("Vshop-ReferralId");
                                if ((cookie != null) && (cookie.Value != null))
                                {
                                    DistributorsInfo userIdDistributors = new DistributorsInfo();
                                    userIdDistributors = DistributorsBrower.GetUserIdDistributors(int.Parse(cookie.Value));
                                    act.Description = act.Description.Replace("{{店铺名称}}", userIdDistributors.StoreName);
                                }
                                else
                                {
                                    SiteSettings masterSettings = SettingsManager.GetMasterSettings(true,wid);
                                    act.Description = act.Description.Replace("{{店铺名称}}", masterSettings.SiteName);
                                }
                            }
                            if (currentMember == null)
                            {
                                base.GotoResourceNotFound("用户信息获取错误!");
                            }
                            this.litItemParams.Text = string.Concat(new object[] { Globals.HostPath(HttpContext.Current.Request.Url), act.ImgUrl, "|", act.ShareTitle, "|", Globals.HostPath(HttpContext.Current.Request.Url), "/Vshop/getredpager.aspx?id=", result, "&userid=", currentMember.UserId, "|", act.Description });
                        }
                    }
                    else
                    {
                        HttpContext.Current.Response.Redirect("/vshop/MemberCenter.aspx");
                    }
                }
                else
                {
                    base.GotoResourceNotFound("输入的参数不正确!");
                }
            }
            PageTitle.AddSiteNameTitle("分享助力");
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "skin-VGetRedShare.html";
            }
            base.OnInit(e);
        }
    }
}

