namespace Hidistro.UI.Web.API
{
    using Hidistro.ControlPanel.Settings;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Settings;
    using Hidistro.SaleSystem.Vshop;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Web;
    using System.Web.SessionState;
    public class Hi_Ajax_NavMenu : IHttpHandler, IRequiresSessionState
    {
        protected string wid;
        public IList<MenuInfo> GetAllMenu()
        {
            IList<MenuInfo> list = new List<MenuInfo>();
            return MenuHelper.GetMenus(wid);
        }

        public string GetPhone()
        {
            int currentDistributorId = Globals.GetCurrentDistributorId();
            if (currentDistributorId == 0)
            {
                return SettingsManager.GetMasterSettings(true,wid).ShopTel;
            }
            return MemberProcessor.GetMember(currentDistributorId, true).CellPhone;
        }

        public void ProcessRequest(HttpContext context)
        {
            //wid = context.Session[DTKeys.SESSION_WEB_ID] as string;
            this.wid = Globals.GetCurrentWid();
            if (string.IsNullOrEmpty(wid))
            {            
                return;
            }
            context.Response.ContentType = "text/plain";
            IList<MenuInfo> allMenu = this.GetAllMenu();
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(false,wid);
            context.Response.Write(JsonConvert.SerializeObject(new { status = 1, msg = "", Phone = this.GetPhone(), GuidePage = SettingsManager.GetMasterSettings(true,wid).GuidePageSet, ShopDefault = masterSettings.ShopDefault, EnableShopMenu = masterSettings.EnableShopMenu, MemberDefault = masterSettings.MemberDefault, GoodsType = masterSettings.GoodsType, GoodsCheck = masterSettings.GoodsCheck, ActivityMenu = masterSettings.ActivityMenu, DistributorsMenu = masterSettings.DistributorsMenu, GoodsListMenu = masterSettings.GoodsListMenu, BrandMenu = masterSettings.BrandMenu, ShopMenuStyle = masterSettings.ShopMenuStyle, menuList = allMenu }));
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}

