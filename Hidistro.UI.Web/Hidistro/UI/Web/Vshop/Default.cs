namespace Hidistro.UI.Web.Vshop
{
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Members;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.SaleSystem.CodeBehind;
    using System;
    using System.Web;
    using System.Web.UI;

    public class Default : Page
    {
        public string cssSrc = "/Templates/vshop/";
        public string Desc = "";
        protected HomePage H_Page;
        public string imgUrl = "";
        public string memberID = "";
        public bool showMenu;
        public string siteName = "";
        public SiteSettings siteSettings;
        protected WeixinSet weixin;
        public string wid = string.Empty;
        public void BindUser()
        {
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            if (currentMember != null)
            {
                this.memberID = currentMember.UserId.ToString();
            }
        }

        public void BindWXInfo()
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies["Vshop-ReferralId"];
            if ((cookie == null) || string.IsNullOrEmpty(cookie.Value))
            {
                this.siteName = this.siteSettings.SiteName;
                this.imgUrl = "http://" + HttpContext.Current.Request.Url.Host + this.siteSettings.DistributorLogoPic;
                this.Desc = this.siteSettings.ShopIntroduction;
            }
            else
            {
                DistributorsInfo distributorInfo = DistributorsBrower.GetDistributorInfo(Convert.ToInt32(cookie.Value));
                if (distributorInfo != null)
                {
                    this.siteName = distributorInfo.StoreName;
                    this.imgUrl = "http://" + HttpContext.Current.Request.Url.Host + distributorInfo.Logo;
                    this.Desc = distributorInfo.StoreDescription;
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!base.IsPostBack)
            {
                HiAffiliation.LoadPage();
                wid = Globals.GetCurrentWid();
                //wid = Globals.RequestQueryStr("wid");
                if (string.IsNullOrEmpty(this.wid)) return;
                this.siteSettings = SettingsManager.GetMasterSettings(false,wid);
                this.showMenu = this.siteSettings.EnableShopMenu;
                this.cssSrc = this.cssSrc +this.wid+"/" +this.siteSettings.VTheme + "/css/head.css";
                this.BindWXInfo();
                this.BindUser();
            }
        }
    }
}

