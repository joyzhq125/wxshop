namespace Hidistro.Core.Entities
{
    using System;
    using System.Data;
    using System.Data.SqlClient;
    using System.Globalization;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Xml;

    public class SiteSettings
    {
        public SiteSettings(string siteUrl)
        {
            this.SiteUrl = siteUrl;
            this.Theme = "default";
            this.VTheme = "default";
            this.Disabled = false;
            this.SiteName = "辰多";
            this.LogoUrl = "/utility/pics/logo.jpg";
            this.ShopTel = "";
            this.DefaultProductImage = "/utility/pics/none.gif";
            this.DefaultProductThumbnail1 = "/utility/pics/none.gif";
            this.DefaultProductThumbnail2 = "/utility/pics/none.gif";
            this.DefaultProductThumbnail3 = "/utility/pics/none.gif";
            this.DefaultProductThumbnail4 = "/utility/pics/none.gif";
            this.DefaultProductThumbnail5 = "/utility/pics/none.gif";
            this.DefaultProductThumbnail6 = "/utility/pics/none.gif";
            this.DefaultProductThumbnail7 = "/utility/pics/none.gif";
            this.DefaultProductThumbnail8 = "/utility/pics/none.gif";
            this.WeiXinCodeImageUrl = "/Storage/master/WeiXinCodeImageUrl.jpg";
            this.VipCardBG = "/Storage/master/Vipcard/vipbg.png";
            this.VipCardQR = "/Storage/master/Vipcard/vipqr.jpg";
            this.VipCardPrefix = "100000";
            this.VipRequireName = true;
            this.VipRequireMobile = true;
            this.EnablePodRequest = true;
            this.CustomReply = true;
            this.SubscribeReply = true;
            this.ByRemind = true;
            this.DecimalLength = 2;
            this.PointsRate = 1M;
            this.OrderShowDays = 7;
            this.CloseOrderDays = 3;
            this.FinishOrderDays = 7;
            this.MaxReturnedDays = 15;
            this.OpenManyService = false;
            this.BatchAliPay = false;
            this.BatchWeixinPay = false;
            this.BatchWeixinPayCheckRealName = 2;
            this.DrawPayType = "";
        }

        public static SiteSettings FromXml(XmlDocument doc,string wid)
        {
            XmlNode node = doc.SelectSingleNode("Settings");

            SiteSettings SiteSettings = new SiteSettings(node.SelectSingleNode("SiteUrl").InnerText);

            SiteSettings.Theme = node.SelectSingleNode("Theme").InnerText;
            SiteSettings.VTheme = node.SelectSingleNode("VTheme").InnerText;
            SiteSettings.ServiceMeiQia = node.SelectSingleNode("ServiceMeiQia").InnerText;
            SiteSettings.DecimalLength = int.Parse(node.SelectSingleNode("DecimalLength").InnerText);
            SiteSettings.DefaultProductImage = node.SelectSingleNode("DefaultProductImage").InnerText;
            SiteSettings.DefaultProductThumbnail1 = node.SelectSingleNode("DefaultProductThumbnail1").InnerText;
            SiteSettings.DefaultProductThumbnail2 = node.SelectSingleNode("DefaultProductThumbnail2").InnerText;
            SiteSettings.DefaultProductThumbnail3 = node.SelectSingleNode("DefaultProductThumbnail3").InnerText;
            SiteSettings.DefaultProductThumbnail4 = node.SelectSingleNode("DefaultProductThumbnail4").InnerText;
            SiteSettings.DefaultProductThumbnail5 = node.SelectSingleNode("DefaultProductThumbnail5").InnerText;
            SiteSettings.DefaultProductThumbnail6 = node.SelectSingleNode("DefaultProductThumbnail6").InnerText;
            SiteSettings.DefaultProductThumbnail7 = node.SelectSingleNode("DefaultProductThumbnail7").InnerText;
            SiteSettings.DefaultProductThumbnail8 = node.SelectSingleNode("DefaultProductThumbnail8").InnerText;
            SiteSettings.CheckCode = node.SelectSingleNode("CheckCode").InnerText;
            SiteSettings.App_Secret = node.SelectSingleNode("App_Secret").InnerText;
            SiteSettings.Access_Token = node.SelectSingleNode("Access_Token").InnerText;
            SiteSettings.Disabled = bool.Parse(node.SelectSingleNode("Disabled").InnerText);
            SiteSettings.Footer = node.SelectSingleNode("Footer").InnerText;
            SiteSettings.RegisterAgreement = node.SelectSingleNode("RegisterAgreement").InnerText;
            SiteSettings.LogoUrl = node.SelectSingleNode("LogoUrl").InnerText;
            SiteSettings.ShopTel = node.SelectSingleNode("ShopTel").InnerText;
            SiteSettings.OrderShowDays = int.Parse(node.SelectSingleNode("OrderShowDays").InnerText);
            SiteSettings.CloseOrderDays = int.Parse(node.SelectSingleNode("CloseOrderDays").InnerText);
            SiteSettings.FinishOrderDays = int.Parse(node.SelectSingleNode("FinishOrderDays").InnerText);
            SiteSettings.MaxReturnedDays = int.Parse(node.SelectSingleNode("MaxReturnedDays").InnerText);
            SiteSettings.TaxRate = decimal.Parse(node.SelectSingleNode("TaxRate").InnerText);
            SiteSettings.PointsRate = decimal.Parse(node.SelectSingleNode("PointsRate").InnerText);
            SiteSettings.SiteName = node.SelectSingleNode("SiteName").InnerText;
            SiteSettings.SiteUrl = node.SelectSingleNode("SiteUrl").InnerText;
            SiteSettings.YourPriceName = node.SelectSingleNode("YourPriceName").InnerText;
            SiteSettings.EmailSender = node.SelectSingleNode("EmailSender").InnerText;
            SiteSettings.EmailSettings = node.SelectSingleNode("EmailSettings").InnerText;
            SiteSettings.SMSSender = node.SelectSingleNode("SMSSender").InnerText;
            SiteSettings.SMSSettings = node.SelectSingleNode("SMSSettings").InnerText;
            SiteSettings.EnabledCnzz = bool.Parse(node.SelectSingleNode("EnabledCnzz").InnerText);
            SiteSettings.CnzzUsername = node.SelectSingleNode("CnzzUsername").InnerText;
            SiteSettings.CnzzPassword = node.SelectSingleNode("CnzzPassword").InnerText;
            SiteSettings.WeixinAppId = node.SelectSingleNode("WeixinAppId").InnerText;
            SiteSettings.WeixinAppSecret = node.SelectSingleNode("WeixinAppSecret").InnerText;
            SiteSettings.WeixinPaySignKey = node.SelectSingleNode("WeixinPaySignKey").InnerText;
            SiteSettings.WeixinPartnerID = node.SelectSingleNode("WeixinPartnerID").InnerText;
            SiteSettings.WeixinPartnerKey = node.SelectSingleNode("WeixinPartnerKey").InnerText;
            SiteSettings.IsValidationService = bool.Parse(node.SelectSingleNode("IsValidationService").InnerText);
            SiteSettings.WeixinToken = node.SelectSingleNode("WeixinToken").InnerText;
            SiteSettings.WeixinNumber = node.SelectSingleNode("WeixinNumber").InnerText;
            SiteSettings.WeixinLoginUrl = node.SelectSingleNode("WeixinLoginUrl").InnerText;
            SiteSettings.WeiXinCodeImageUrl = node.SelectSingleNode("WeiXinCodeImageUrl").InnerText;
            SiteSettings.VipCardLogo = node.SelectSingleNode("VipCardLogo").InnerText;
            SiteSettings.VipCardBG = node.SelectSingleNode("VipCardBG").InnerText;
            SiteSettings.VipCardQR = node.SelectSingleNode("VipCardQR").InnerText;
            SiteSettings.VipCardName = node.SelectSingleNode("VipCardName").InnerText;
            SiteSettings.VipCardPrefix = node.SelectSingleNode("VipCardPrefix").InnerText;
            SiteSettings.VipRequireName = bool.Parse(node.SelectSingleNode("VipRequireName").InnerText);
            SiteSettings.VipRequireMobile = bool.Parse(node.SelectSingleNode("VipRequireMobile").InnerText);
            SiteSettings.CustomReply = bool.Parse(node.SelectSingleNode("CustomReply").InnerText);
            SiteSettings.EnableSaleService = bool.Parse(node.SelectSingleNode("EnableSaleService").InnerText);
            SiteSettings.ByRemind = bool.Parse(node.SelectSingleNode("ByRemind").InnerText);
            SiteSettings.EnableShopMenu = bool.Parse(node.SelectSingleNode("EnableShopMenu").InnerText);
            SiteSettings.ShopDefault = bool.Parse(node.SelectSingleNode("ShopDefault").InnerText);
            SiteSettings.ActivityMenu = bool.Parse(node.SelectSingleNode("ActivityMenu").InnerText);
            SiteSettings.DistributorsMenu = bool.Parse(node.SelectSingleNode("DistributorsMenu").InnerText);
            SiteSettings.GoodsListMenu = bool.Parse(node.SelectSingleNode("GoodsListMenu").InnerText);
            SiteSettings.BrandMenu = bool.Parse(node.SelectSingleNode("BrandMenu").InnerText);
            SiteSettings.MemberDefault = bool.Parse(node.SelectSingleNode("MemberDefault").InnerText);
            SiteSettings.GoodsType = bool.Parse(node.SelectSingleNode("GoodsType").InnerText);
            SiteSettings.GoodsCheck = bool.Parse(node.SelectSingleNode("GoodsCheck").InnerText);
            SiteSettings.ShopMenuStyle = node.SelectSingleNode("ShopMenuStyle").InnerText;
            SiteSettings.SubscribeReply = bool.Parse(node.SelectSingleNode("SubscribeReply").InnerText);
            SiteSettings.VipRequireAdress = bool.Parse(node.SelectSingleNode("VipRequireAdress").InnerText);
            SiteSettings.VipRequireQQ = bool.Parse(node.SelectSingleNode("VipRequireQQ").InnerText);
            SiteSettings.VipEnableCoupon = bool.Parse(node.SelectSingleNode("VipEnableCoupon").InnerText);
            SiteSettings.VipRemark = node.SelectSingleNode("VipRemark").InnerText;
            SiteSettings.EnablePodRequest = bool.Parse(node.SelectSingleNode("EnablePodRequest").InnerText);
            SiteSettings.EnableCommission = bool.Parse(node.SelectSingleNode("EnableCommission").InnerText);
            SiteSettings.EnableAlipayRequest = bool.Parse(node.SelectSingleNode("EnableAlipayRequest").InnerText);
            SiteSettings.EnableWeiXinRequest = bool.Parse(node.SelectSingleNode("EnableWeiXinRequest").InnerText);
            SiteSettings.EnableOffLineRequest = bool.Parse(node.SelectSingleNode("EnableOffLineRequest").InnerText);
            SiteSettings.EnableWapShengPay = bool.Parse(node.SelectSingleNode("EnableWapShengPay").InnerText);
            SiteSettings.OffLinePayContent = node.SelectSingleNode("OffLinePayContent").InnerText;
            SiteSettings.DistributorDescription = node.SelectSingleNode("DistributorDescription").InnerText;
            SiteSettings.DistributorBackgroundPic = node.SelectSingleNode("DistributorBackgroundPic").InnerText;
            SiteSettings.DistributorLogoPic = node.SelectSingleNode("DistributorLogoPic").InnerText;
            SiteSettings.SaleService = node.SelectSingleNode("SaleService").InnerText;
            SiteSettings.MentionNowMoney = node.SelectSingleNode("MentionNowMoney").InnerText;
            SiteSettings.ShopIntroduction = node.SelectSingleNode("ShopIntroduction").InnerText;
            SiteSettings.ApplicationDescription = node.SelectSingleNode("ApplicationDescription").InnerText;
            SiteSettings.EnableGuidePageSet = bool.Parse(node.SelectSingleNode("EnableGuidePageSet").InnerText);
            SiteSettings.GuidePageSet = node.SelectSingleNode("GuidePageSet").InnerText;
            SiteSettings.ManageOpenID = node.SelectSingleNode("ManageOpenID").InnerText;
            SiteSettings.WeixinCertPath = node.SelectSingleNode("WeixinCertPath").InnerText;
            SiteSettings.WeixinCertPassword = node.SelectSingleNode("WeixinCertPassword").InnerText;
            SiteSettings.GoodsPic = node.SelectSingleNode("GoodsPic").InnerText;
            SiteSettings.GoodsName = node.SelectSingleNode("GoodsName").InnerText;
            SiteSettings.GoodsDescription = node.SelectSingleNode("GoodsDescription").InnerText;
            SiteSettings.ShopHomePic = node.SelectSingleNode("ShopHomePic").InnerText;
            SiteSettings.ShopHomeName = node.SelectSingleNode("ShopHomeName").InnerText;
            SiteSettings.ShopHomeDescription = node.SelectSingleNode("ShopHomeDescription").InnerText;
            SiteSettings.ShopSpreadingCodePic = node.SelectSingleNode("ShopSpreadingCodePic").InnerText;
            SiteSettings.ShopSpreadingCodeName = node.SelectSingleNode("ShopSpreadingCodeName").InnerText;
            SiteSettings.ShopSpreadingCodeDescription = node.SelectSingleNode("ShopSpreadingCodeDescription").InnerText;
            SiteSettings.OpenManyService = bool.Parse(node.SelectSingleNode("OpenManyService").InnerText);
            SiteSettings.IsRequestDistributor = bool.Parse(node.SelectSingleNode("IsRequestDistributor").InnerText);
            SiteSettings.FinishedOrderMoney = int.Parse(node.SelectSingleNode("FinishedOrderMoney").InnerText);
            SiteSettings.RegisterDistributorsPoints = int.Parse(node.SelectSingleNode("RegisterDistributorsPoints").InnerText);
            SiteSettings.OrdersPoints = int.Parse(node.SelectSingleNode("OrdersPoints").InnerText);
            SiteSettings.ChinaBank_DES = node.SelectSingleNode("ChinaBank_DES").InnerText;
            SiteSettings.ChinaBank_Enable = bool.Parse(node.SelectSingleNode("ChinaBank_Enable").InnerText);
            SiteSettings.ChinaBank_MD5 = node.SelectSingleNode("ChinaBank_MD5").InnerText;
            SiteSettings.ChinaBank_mid = node.SelectSingleNode("ChinaBank_mid").InnerText;
            SiteSettings.Alipay_Key = node.SelectSingleNode("Alipay_Key").InnerText;
            SiteSettings.Alipay_mid = node.SelectSingleNode("Alipay_mid").InnerText;
            SiteSettings.Alipay_mName = node.SelectSingleNode("Alipay_mName").InnerText;
            SiteSettings.Alipay_Pid = node.SelectSingleNode("Alipay_Pid").InnerText;
            SiteSettings.OfflinePay_BankCard_Name = node.SelectSingleNode("OfflinePay_BankCard_Name").InnerText;
            SiteSettings.OfflinePay_BankCard_BankName = node.SelectSingleNode("OfflinePay_BankCard_BankName").InnerText;
            SiteSettings.OfflinePay_BankCard_CardNo = node.SelectSingleNode("OfflinePay_BankCard_CardNo").InnerText;
            SiteSettings.OfflinePay_Alipay_id = node.SelectSingleNode("OfflinePay_Alipay_id").InnerText;
            SiteSettings.ShenPay_mid = node.SelectSingleNode("ShenPay_mid").InnerText;
            SiteSettings.ShenPay_key = node.SelectSingleNode("ShenPay_key").InnerText;
            SiteSettings.EnableWeixinRed = bool.Parse(node.SelectSingleNode("EnableWeixinRed").InnerText);
            SiteSettings.MemberRoleContent = node.SelectSingleNode("MemberRoleContent").InnerText;
            SiteSettings.sign_EverDayScore = int.Parse(node.SelectSingleNode("sign_EverDayScore").InnerText);
            SiteSettings.sign_StraightDay = int.Parse(node.SelectSingleNode("sign_StraightDay").InnerText);
            SiteSettings.sign_RewardScore = int.Parse(node.SelectSingleNode("sign_RewardScore").InnerText);
            SiteSettings.sign_score_Enable = bool.Parse(node.SelectSingleNode("sign_score_Enable").InnerText);
            SiteSettings.open_signContinuity = bool.Parse(node.SelectSingleNode("open_signContinuity").InnerText);
            SiteSettings.shopping_reward_Enable = bool.Parse(node.SelectSingleNode("shopping_reward_Enable").InnerText);
            SiteSettings.shopping_score_Enable = bool.Parse(node.SelectSingleNode("shopping_score_Enable").InnerText);
            SiteSettings.shopping_Score = int.Parse(node.SelectSingleNode("shopping_Score").InnerText);
            SiteSettings.shopping_reward_Score = int.Parse(node.SelectSingleNode("shopping_reward_Score").InnerText);
            SiteSettings.shopping_reward_OrderValue = double.Parse(node.SelectSingleNode("shopping_reward_OrderValue").InnerText);
            SiteSettings.share_score_Enable = bool.Parse(node.SelectSingleNode("share_score_Enable").InnerText);
            SiteSettings.share_Score = int.Parse(node.SelectSingleNode("share_Score").InnerText);
            SiteSettings.PonitToCash_Enable = bool.Parse(node.SelectSingleNode("PonitToCash_Enable").InnerText);
            SiteSettings.PointToCashRate = int.Parse(node.SelectSingleNode("PointToCashRate").InnerText);
            SiteSettings.PonitToCash_MaxAmount = decimal.Parse(node.SelectSingleNode("PonitToCash_MaxAmount").InnerText);
            SiteSettings.BatchAliPay = bool.Parse(node.SelectSingleNode("BatchAliPay").InnerText);
            SiteSettings.BatchWeixinPay = bool.Parse(node.SelectSingleNode("BatchWeixinPay").InnerText);
            SiteSettings.DrawPayType = node.SelectSingleNode("DrawPayType").InnerText;
            SiteSettings.BatchWeixinPayCheckRealName = int.Parse(node.SelectSingleNode("BatchWeixinPayCheckRealName").InnerText);
            SiteSettings.ShareAct_Enable = bool.Parse(node.SelectSingleNode("ShareAct_Enable").InnerText);
            SiteSettings.SignWhere = int.Parse(node.SelectSingleNode("SignWhere").InnerText);
            SiteSettings.SignWherePoint = int.Parse(node.SelectSingleNode("SignWherePoint").InnerText);
            SiteSettings.SignPoint = int.Parse(node.SelectSingleNode("SignPoint").InnerText);
            SiteSettings.ActiveDay = int.Parse(node.SelectSingleNode("ActiveDay").InnerText);
            SiteSettings.entId = "0";

            Chenduo.Model.sf_website website = Globals.GetModelByWid(wid);
            if (website != null)
            {
                SiteSettings.VTheme = website.templatesNum;//node.SelectSingleNode("VTheme").InnerText;
                SiteSettings.LogoUrl = website.logo;//node.SelectSingleNode("LogoUrl").InnerText;
                SiteSettings.ShopTel = website.tel;//node.SelectSingleNode("ShopTel").InnerText;
                SiteSettings.SiteName = website.sitename;//node.SelectSingleNode("SiteName").InnerText;
                SiteSettings.WeixinAppId = website.appid;//node.SelectSingleNode("WeixinAppId").InnerText;
                SiteSettings.WeixinAppSecret = website.appsecret;//node.SelectSingleNode("WeixinAppSecret").InnerText;
                SiteSettings.WeixinPartnerID = website.weixin_pay_account;//node.SelectSingleNode("WeixinPartnerID").InnerText;
                SiteSettings.WeixinPartnerKey = website.account_pay_key;//node.SelectSingleNode("WeixinPartnerKey").InnerText;
                SiteSettings.WeixinToken = website.token_value;//node.SelectSingleNode("WeixinToken").InnerText;
                SiteSettings.EnablePodRequest = website.EnablePodRequest == "1" ? true : false;//bool.Parse(node.SelectSingleNode("EnablePodRequest").InnerText);
                SiteSettings.EnableAlipayRequest = website.EnableAlipayRequest == "1" ? true : false;//bool.Parse(node.SelectSingleNode("EnableAlipayRequest").InnerText);
                SiteSettings.EnableWeiXinRequest = website.Enableweixinrequest == "1" ? true : false;//bool.Parse(node.SelectSingleNode("EnableWeiXinRequest").InnerText);
                SiteSettings.EnableOffLineRequest = website.EnableOffLineRequest == "1" ? true : false;//bool.Parse(node.SelectSingleNode("EnableOffLineRequest").InnerText);
                SiteSettings.OffLinePayContent = website.OffLinePayContent;//node.SelectSingleNode("OffLinePayContent").InnerText;
                SiteSettings.WeixinCertPath = website.WeixinCertPath;//node.SelectSingleNode("WeixinCertPath").InnerText;
                SiteSettings.WeixinCertPassword = website.WeixinCertPassword;//node.SelectSingleNode("WeixinCertPassword").InnerText;
                SiteSettings.Alipay_Key = website.Alipay_Key;//node.SelectSingleNode("Alipay_Key").InnerText;
                SiteSettings.Alipay_mid = website.Alipay_mid;//node.SelectSingleNode("Alipay_mid").InnerText;
                SiteSettings.Alipay_mName = website.Alipay_mName;//node.SelectSingleNode("Alipay_mName").InnerText;
                SiteSettings.Alipay_Pid = website.Alipay_Pid;//node.SelectSingleNode("Alipay_Pid").InnerText;
                SiteSettings.EnableWeixinRed = website.EnableWeixinRed == "1" ? true : false; ;//bool.Parse(node.SelectSingleNode("EnableWeixinRed").InnerText);

                SiteSettings.OpenManyService = website.OpenManyService == "1" ? true : false;
                SiteSettings.ManageOpenID = website.ManageOpenID;
                SiteSettings.GuidePageSet = website.GuidePageSet;
                SiteSettings.EnableGuidePageSet = website.EnableGuidePageSet == "1" ? true : false;

                SiteSettings.IsValidationService = website.IsValidationService == "1" ? true : false;
                SiteSettings.EnableShopMenu = website.EnableShopMenu;
                SiteSettings.EnableSaleService = website.EnableSaleService;
                SiteSettings.entId = website.entId;
            }
            return SiteSettings;
        }

        private static void SetNodeValue(XmlDocument doc, XmlNode root, string nodeName, string nodeValue)
        {
            XmlNode newChild = root.SelectSingleNode(nodeName);
            if (newChild == null)
            {
                newChild = doc.CreateElement(nodeName);
                root.AppendChild(newChild);
            }
            newChild.InnerText = nodeValue;
        }

        public void WriteToXml(XmlDocument doc)
        {
            XmlNode root = doc.SelectSingleNode("Settings");
            SetNodeValue(doc, root, "SiteUrl", this.SiteUrl);
            SetNodeValue(doc, root, "Theme", this.Theme);
            SetNodeValue(doc, root, "VTheme", this.VTheme);
            SetNodeValue(doc, root, "ServiceMeiQia", this.ServiceMeiQia);
            SetNodeValue(doc, root, "DecimalLength", this.DecimalLength.ToString(CultureInfo.InvariantCulture));
            SetNodeValue(doc, root, "DefaultProductImage", this.DefaultProductImage);
            SetNodeValue(doc, root, "DefaultProductThumbnail1", this.DefaultProductThumbnail1);
            SetNodeValue(doc, root, "DefaultProductThumbnail2", this.DefaultProductThumbnail2);
            SetNodeValue(doc, root, "DefaultProductThumbnail3", this.DefaultProductThumbnail3);
            SetNodeValue(doc, root, "DefaultProductThumbnail4", this.DefaultProductThumbnail4);
            SetNodeValue(doc, root, "DefaultProductThumbnail5", this.DefaultProductThumbnail5);
            SetNodeValue(doc, root, "DefaultProductThumbnail6", this.DefaultProductThumbnail6);
            SetNodeValue(doc, root, "DefaultProductThumbnail7", this.DefaultProductThumbnail7);
            SetNodeValue(doc, root, "DefaultProductThumbnail8", this.DefaultProductThumbnail8);
            SetNodeValue(doc, root, "App_Secret", this.App_Secret);
            SetNodeValue(doc, root, "CheckCode", this.CheckCode);
            SetNodeValue(doc, root, "Access_Token", this.Access_Token);
            SetNodeValue(doc, root, "Disabled", this.Disabled ? "true" : "false");
            SetNodeValue(doc, root, "Footer", this.Footer);
            SetNodeValue(doc, root, "RegisterAgreement", this.RegisterAgreement);
            SetNodeValue(doc, root, "ShopTel", this.ShopTel);
            SetNodeValue(doc, root, "LogoUrl", this.LogoUrl);
            SetNodeValue(doc, root, "OrderShowDays", this.OrderShowDays.ToString(CultureInfo.InvariantCulture));
            SetNodeValue(doc, root, "CloseOrderDays", this.CloseOrderDays.ToString(CultureInfo.InvariantCulture));
            SetNodeValue(doc, root, "FinishOrderDays", this.FinishOrderDays.ToString(CultureInfo.InvariantCulture));
            SetNodeValue(doc, root, "MaxReturnedDays", this.MaxReturnedDays.ToString(CultureInfo.InvariantCulture));
            SetNodeValue(doc, root, "TaxRate", this.TaxRate.ToString(CultureInfo.InvariantCulture));
            SetNodeValue(doc, root, "PointsRate", this.PointsRate.ToString("F"));
            SetNodeValue(doc, root, "SiteName", this.SiteName);
            SetNodeValue(doc, root, "YourPriceName", this.YourPriceName);
            SetNodeValue(doc, root, "EmailSender", this.EmailSender);
            SetNodeValue(doc, root, "EmailSettings", this.EmailSettings);
            SetNodeValue(doc, root, "SMSSender", this.SMSSender);
            SetNodeValue(doc, root, "SMSSettings", this.SMSSettings);
            SetNodeValue(doc, root, "EnabledCnzz", this.EnabledCnzz ? "true" : "false");
            SetNodeValue(doc, root, "CnzzUsername", this.CnzzUsername);
            SetNodeValue(doc, root, "CnzzPassword", this.CnzzPassword);
            SetNodeValue(doc, root, "WeixinAppId", this.WeixinAppId);
            SetNodeValue(doc, root, "WeixinAppSecret", this.WeixinAppSecret);
            SetNodeValue(doc, root, "WeixinPaySignKey", this.WeixinPaySignKey);
            SetNodeValue(doc, root, "WeixinPartnerID", this.WeixinPartnerID);
            SetNodeValue(doc, root, "WeixinPartnerKey", this.WeixinPartnerKey);
            SetNodeValue(doc, root, "IsValidationService", this.IsValidationService ? "true" : "false");
            SetNodeValue(doc, root, "WeixinToken", this.WeixinToken);
            SetNodeValue(doc, root, "WeixinNumber", this.WeixinNumber);
            SetNodeValue(doc, root, "WeixinLoginUrl", this.WeixinLoginUrl);
            SetNodeValue(doc, root, "WeiXinCodeImageUrl", this.WeiXinCodeImageUrl);
            SetNodeValue(doc, root, "VipCardBG", this.VipCardBG);
            SetNodeValue(doc, root, "VipCardLogo", this.VipCardLogo);
            SetNodeValue(doc, root, "VipCardQR", this.VipCardQR);
            SetNodeValue(doc, root, "VipCardPrefix", this.VipCardPrefix);
            SetNodeValue(doc, root, "VipCardName", this.VipCardName);
            SetNodeValue(doc, root, "VipRequireName", this.VipRequireName ? "true" : "false");
            SetNodeValue(doc, root, "VipRequireMobile", this.VipRequireMobile ? "true" : "false");
            SetNodeValue(doc, root, "CustomReply", this.CustomReply ? "true" : "false");
            SetNodeValue(doc, root, "EnableSaleService", this.EnableSaleService ? "true" : "false");
            SetNodeValue(doc, root, "ByRemind", this.ByRemind ? "true" : "false");
            SetNodeValue(doc, root, "ShopMenuStyle", this.ShopMenuStyle);
            SetNodeValue(doc, root, "EnableShopMenu", this.EnableShopMenu ? "true" : "false");
            SetNodeValue(doc, root, "ShopDefault", this.ShopDefault ? "true" : "false");
            SetNodeValue(doc, root, "MemberDefault", this.MemberDefault ? "true" : "false");
            SetNodeValue(doc, root, "GoodsType", this.GoodsType ? "true" : "false");
            SetNodeValue(doc, root, "GoodsCheck", this.GoodsCheck ? "true" : "false");
            SetNodeValue(doc, root, "ActivityMenu", this.ActivityMenu ? "true" : "false");
            SetNodeValue(doc, root, "DistributorsMenu", this.DistributorsMenu ? "true" : "false");
            SetNodeValue(doc, root, "GoodsListMenu", this.GoodsListMenu ? "true" : "false");
            SetNodeValue(doc, root, "BrandMenu", this.BrandMenu ? "true" : "false");
            SetNodeValue(doc, root, "SubscribeReply", this.SubscribeReply ? "true" : "false");
            SetNodeValue(doc, root, "VipRequireQQ", this.VipRequireQQ ? "true" : "false");
            SetNodeValue(doc, root, "VipRequireAdress", this.VipRequireAdress ? "true" : "false");
            SetNodeValue(doc, root, "VipEnableCoupon", this.VipEnableCoupon ? "true" : "false");
            SetNodeValue(doc, root, "VipRemark", this.VipRemark);
            SetNodeValue(doc, root, "EnablePodRequest", this.EnablePodRequest ? "true" : "false");
            SetNodeValue(doc, root, "EnableCommission", this.EnableCommission ? "true" : "false");
            SetNodeValue(doc, root, "EnableAlipayRequest", this.EnableAlipayRequest ? "true" : "false");
            SetNodeValue(doc, root, "EnableWeiXinRequest", this.EnableWeiXinRequest ? "true" : "false");
            SetNodeValue(doc, root, "EnableOffLineRequest", this.EnableOffLineRequest ? "true" : "false");
            SetNodeValue(doc, root, "EnableWapShengPay", this.EnableWapShengPay ? "true" : "false");
            SetNodeValue(doc, root, "OffLinePayContent", this.OffLinePayContent);
            SetNodeValue(doc, root, "DistributorDescription", this.DistributorDescription);
            SetNodeValue(doc, root, "DistributorBackgroundPic", this.DistributorBackgroundPic);
            SetNodeValue(doc, root, "DistributorLogoPic", this.DistributorLogoPic);
            SetNodeValue(doc, root, "SaleService", this.SaleService);
            SetNodeValue(doc, root, "MentionNowMoney", this.MentionNowMoney);
            SetNodeValue(doc, root, "ShopIntroduction", this.ShopIntroduction);
            SetNodeValue(doc, root, "ApplicationDescription", this.ApplicationDescription);
            SetNodeValue(doc, root, "GuidePageSet", this.GuidePageSet);
            SetNodeValue(doc, root, "EnableGuidePageSet", this.EnableGuidePageSet ? "true" : "false");
            SetNodeValue(doc, root, "ManageOpenID", this.ManageOpenID);
            SetNodeValue(doc, root, "WeixinCertPath", this.WeixinCertPath);
            SetNodeValue(doc, root, "WeixinCertPassword", this.WeixinCertPassword);
            SetNodeValue(doc, root, "GoodsPic", this.GoodsPic);
            SetNodeValue(doc, root, "GoodsName", this.GoodsName);
            SetNodeValue(doc, root, "GoodsDescription", this.GoodsDescription);
            SetNodeValue(doc, root, "ShopHomePic", this.ShopHomePic);
            SetNodeValue(doc, root, "ShopHomeName", this.ShopHomeName);
            SetNodeValue(doc, root, "ShopHomeDescription", this.ShopHomeDescription);
            SetNodeValue(doc, root, "ShopSpreadingCodePic", this.ShopSpreadingCodePic);
            SetNodeValue(doc, root, "ShopSpreadingCodeName", this.ShopSpreadingCodeName);
            SetNodeValue(doc, root, "ShopSpreadingCodeDescription", this.ShopSpreadingCodeDescription);
            SetNodeValue(doc, root, "OpenManyService", this.OpenManyService ? "true" : "false");
            SetNodeValue(doc, root, "IsRequestDistributor", this.IsRequestDistributor ? "true" : "false");
            SetNodeValue(doc, root, "FinishedOrderMoney", this.FinishedOrderMoney.ToString());
            SetNodeValue(doc, root, "RegisterDistributorsPoints", this.RegisterDistributorsPoints.ToString());
            SetNodeValue(doc, root, "OrdersPoints", this.OrdersPoints.ToString());
            SetNodeValue(doc, root, "ChinaBank_Enable", this.ChinaBank_Enable ? "true" : "false");
            SetNodeValue(doc, root, "ChinaBank_DES", this.ChinaBank_DES);
            SetNodeValue(doc, root, "ChinaBank_MD5", this.ChinaBank_MD5);
            SetNodeValue(doc, root, "ChinaBank_mid", this.ChinaBank_mid);
            SetNodeValue(doc, root, "Alipay_Key", this.Alipay_Key);
            SetNodeValue(doc, root, "Alipay_mid", this.Alipay_mid);
            SetNodeValue(doc, root, "Alipay_mName", this.Alipay_mName);
            SetNodeValue(doc, root, "Alipay_Pid", this.Alipay_Pid);
            SetNodeValue(doc, root, "OfflinePay_Alipay_id", this.OfflinePay_Alipay_id);
            SetNodeValue(doc, root, "OfflinePay_BankCard_Name", this.OfflinePay_BankCard_Name);
            SetNodeValue(doc, root, "OfflinePay_BankCard_BankName", this.OfflinePay_BankCard_BankName);
            SetNodeValue(doc, root, "OfflinePay_BankCard_CardNo", this.OfflinePay_BankCard_CardNo);
            SetNodeValue(doc, root, "ShenPay_mid", this.ShenPay_mid);
            SetNodeValue(doc, root, "ShenPay_key", this.ShenPay_key);
            SetNodeValue(doc, root, "EnableWeixinRed", this.EnableWeixinRed ? "true" : "false");
            SetNodeValue(doc, root, "MemberRoleContent", this.MemberRoleContent);
            SetNodeValue(doc, root, "sign_EverDayScore", this.sign_EverDayScore.ToString());
            SetNodeValue(doc, root, "sign_StraightDay", this.sign_StraightDay.ToString());
            SetNodeValue(doc, root, "sign_RewardScore", this.sign_RewardScore.ToString());
            SetNodeValue(doc, root, "sign_score_Enable", this.sign_score_Enable ? "true" : "false");
            SetNodeValue(doc, root, "open_signContinuity", this.open_signContinuity ? "true" : "false");
            SetNodeValue(doc, root, "shopping_score_Enable", this.shopping_score_Enable ? "true" : "false");
            SetNodeValue(doc, root, "shopping_reward_Enable", this.shopping_reward_Enable ? "true" : "false");
            SetNodeValue(doc, root, "shopping_Score", this.shopping_Score.ToString());
            SetNodeValue(doc, root, "shopping_reward_OrderValue", this.shopping_reward_OrderValue.ToString("F2"));
            SetNodeValue(doc, root, "shopping_reward_Score", this.shopping_reward_Score.ToString());
            SetNodeValue(doc, root, "share_score_Enable", this.share_score_Enable ? "true" : "false");
            SetNodeValue(doc, root, "share_Score", this.share_Score.ToString());
            SetNodeValue(doc, root, "PonitToCash_Enable", this.PonitToCash_Enable ? "true" : "false");
            SetNodeValue(doc, root, "PointToCashRate", this.PointToCashRate.ToString());
            SetNodeValue(doc, root, "PonitToCash_MaxAmount", this.PonitToCash_MaxAmount.ToString("F2"));
            SetNodeValue(doc, root, "DrawPayType", this.DrawPayType.ToString());
            SetNodeValue(doc, root, "BatchAliPay", this.BatchAliPay ? "true" : "false");
            SetNodeValue(doc, root, "BatchWeixinPay", this.BatchWeixinPay ? "true" : "false");
            SetNodeValue(doc, root, "BatchWeixinPayCheckRealName", this.BatchWeixinPayCheckRealName.ToString());
            SetNodeValue(doc, root, "ShareAct_Enable", this.ShareAct_Enable ? "true" : "false");
            SetNodeValue(doc, root, "SignWhere", this.SignWhere.ToString());
            SetNodeValue(doc, root, "SignWherePoint", this.SignWherePoint.ToString());
            SetNodeValue(doc, root, "SignPoint", this.SignPoint.ToString());
            SetNodeValue(doc, root, "ActiveDay", this.ActiveDay.ToString());
        }
        /*
        public static SF.Model.sf_website GetModelByWid(string wid)
        {

            StringBuilder strSql = new StringBuilder();
            //strSql.Append("select  top 1 id,businessNum,mid,templatesNum,appid_name,appid_origin_id,weixin_account,avatar,interface_url,token_value,encodingaeskey,appid,appsecret,create_user,create_time,payment_name,state,weixin_pay_account,account_pay_key,send_type,logo,description,wid,sitename,tel,Enableweixinrequest,WeixinCertPassword,Alipay_mid,Alipay_mName,Alipay_Pid ,Alipay_Key,OffLinePayContent,EnableWeixinRed,EnableAlipayRequest,EnablePodRequest,EnableOffLineRequest from sf_website ");
            strSql.Append("select  top 1 * from sf_website ");
            strSql.Append(" where wid=@wid");
            SqlParameter[] parameters = {
                    new SqlParameter("@wid", SqlDbType.NVarChar)
            };
            parameters[0].Value = wid;

            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                return DataRowToModel(ds.Tables[0].Rows[0]);
            }
            else
            {
                return null;
            }
        }


        public static SF.Model.sf_website DataRowToModel(DataRow row)
        {
            SF.Model.sf_website model = new SF.Model.sf_website();
            if (row != null)
            {
                if (row["id"] != null && row["id"].ToString() != "")
                {
                    model.id = long.Parse(row["id"].ToString());
                }
                if (row["businessNum"] != null)
                {
                    model.businessNum = row["businessNum"].ToString();
                }
                if (row["mid"] != null)
                {
                    model.mid = long.Parse(row["mid"].ToString());
                }
                if (row["templatesNum"] != null)
                {
                    model.templatesNum = row["templatesNum"].ToString();
                }
                if (row["appid_name"] != null)
                {
                    model.appid_name = row["appid_name"].ToString();
                }
                if (row["appid_origin_id"] != null)
                {
                    model.appid_origin_id = row["appid_origin_id"].ToString();
                }
                if (row["weixin_account"] != null)
                {
                    model.weixin_account = row["weixin_account"].ToString();
                }
                if (row["avatar"] != null)
                {
                    model.avatar = row["avatar"].ToString();
                }
                if (row["interface_url"] != null)
                {
                    model.interface_url = row["interface_url"].ToString();
                }
                if (row["token_value"] != null)
                {
                    model.token_value = row["token_value"].ToString();
                }
                if (row["encodingaeskey"] != null)
                {
                    model.encodingaeskey = row["encodingaeskey"].ToString();
                }
                if (row["appid"] != null)
                {
                    model.appid = row["appid"].ToString();
                }
                if (row["appsecret"] != null)
                {
                    model.appsecret = row["appsecret"].ToString();
                }
                if (row["create_user"] != null)
                {
                    model.create_user = row["create_user"].ToString();
                }
                if (row["create_time"] != null)
                {
                    model.create_time = row["create_time"].ToString();
                }
                if (row["payment_name"] != null)
                {
                    model.payment_name = row["payment_name"].ToString();
                }
                if (row["state"] != null && row["state"].ToString() != "")
                {
                    model.state = int.Parse(row["state"].ToString());
                }
                if (row["weixin_pay_account"] != null)
                {
                    model.weixin_pay_account = row["weixin_pay_account"].ToString();
                }
                if (row["account_pay_key"] != null)
                {
                    model.account_pay_key = row["account_pay_key"].ToString();
                }
                if (row["send_type"] != null && row["send_type"].ToString() != "")
                {
                    model.send_type = int.Parse(row["send_type"].ToString());
                }
                if (row["logo"] != null)
                {
                    model.logo = row["logo"].ToString();
                }
                if (row["description"] != null)
                {
                    model.description = row["description"].ToString();
                }
                if (row["wid"] != null)
                {
                    model.wid = row["wid"].ToString();
                }
                if (row["sitename"] != null)
                {
                    model.sitename = row["sitename"].ToString();
                }
                if (row["tel"] != null)
                {
                    model.tel = row["tel"].ToString();
                }
                if (row["Enableweixinrequest"] != null)
                {
                    model.Enableweixinrequest = row["Enableweixinrequest"].ToString();
                }

                if (row["WeixinCertPassword"] != null)
                {
                    model.WeixinCertPassword = row["WeixinCertPassword"].ToString();
                }
                if (row["Alipay_mid"] != null)
                {
                    model.Alipay_mid = row["Alipay_mid"].ToString();
                }
                if (row["Alipay_mName"] != null)
                {
                    model.Alipay_mName = row["Alipay_mName"].ToString();
                }
                if (row["Alipay_Pid"] != null)
                {
                    model.Alipay_Pid = row["Alipay_Pid"].ToString();
                }
                if (row["Alipay_Key"] != null)
                {
                    model.Alipay_Key = row["Alipay_Key"].ToString();
                }
                if (row["OffLinePayContent"] != null)
                {
                    model.OffLinePayContent = row["OffLinePayContent"].ToString();
                }
                if (row["EnableWeixinRed"] != null)
                {
                    model.EnableWeixinRed = row["EnableWeixinRed"].ToString();
                }
                if (row["EnableAlipayRequest"] != null)
                {
                    model.EnableAlipayRequest = row["EnableAlipayRequest"].ToString();
                }
                if (row["EnablePodRequest"] != null)
                {
                    model.EnablePodRequest = row["EnablePodRequest"].ToString();
                }
                if (row["EnableOffLineRequest"] != null)
                {
                    model.EnableOffLineRequest = row["EnableOffLineRequest"].ToString();

                }


                if (row["WeixinCertPath"] != null)
                {
                    model.WeixinCertPath = row["WeixinCertPath"].ToString();

                }
                if (row["OpenManyService"] != null)
                {
                    model.OpenManyService = row["OpenManyService"].ToString();

                }
                if (row["GuidePageSet"] != null)
                {
                    model.GuidePageSet = row["GuidePageSet"].ToString();

                }
                if (row["EnableGuidePageSet"] != null)
                {
                    model.EnableGuidePageSet = row["EnableGuidePageSet"].ToString();

                }
                if (row["ManageOpenID"] != null)
                {
                    model.ManageOpenID = row["ManageOpenID"].ToString();

                }
                if (row["IsValidationService"] != null)
                {
                    model.IsValidationService = row["IsValidationService"].ToString();

                }

            }
            return model;
        }
        */
        public string Access_Token { get; set; }

        public int ActiveDay { get; set; }

        public bool ActivityMenu { get; set; }

        public string Alipay_Key { get; set; }

        public string Alipay_mid { get; set; }

        public string Alipay_mName { get; set; }

        public string Alipay_Pid { get; set; }

        public string App_Secret { get; set; }

        public string ApplicationDescription { get; set; }

        public bool BatchAliPay { get; set; }

        public bool BatchWeixinPay { get; set; }

        public int BatchWeixinPayCheckRealName { get; set; }

        public bool BrandMenu { get; set; }

        public bool ByRemind { get; set; }

        public string CheckCode { get; set; }

        public string ChinaBank_DES { get; set; }

        public bool ChinaBank_Enable { get; set; }

        public string ChinaBank_MD5 { get; set; }

        public string ChinaBank_mid { get; set; }

        public int CloseOrderDays { get; set; }

        public string CnzzPassword { get; set; }

        public string CnzzUsername { get; set; }

        public bool CustomReply { get; set; }

        public int DecimalLength { get; set; }

        public string DefaultProductImage { get; set; }

        public string DefaultProductThumbnail1 { get; set; }

        public string DefaultProductThumbnail2 { get; set; }

        public string DefaultProductThumbnail3 { get; set; }

        public string DefaultProductThumbnail4 { get; set; }

        public string DefaultProductThumbnail5 { get; set; }

        public string DefaultProductThumbnail6 { get; set; }

        public string DefaultProductThumbnail7 { get; set; }

        public string DefaultProductThumbnail8 { get; set; }

        public bool Disabled { get; set; }

        public string DistributorBackgroundPic { get; set; }

        public string DistributorDescription { get; set; }

        public string DistributorLogoPic { get; set; }

        public bool DistributorsMenu { get; set; }

        public string DrawPayType { get; set; }

        public bool EmailEnabled
        {
            get
            {
                return (((!string.IsNullOrEmpty(this.EmailSender) && !string.IsNullOrEmpty(this.EmailSettings)) && (this.EmailSender.Trim().Length > 0)) && (this.EmailSettings.Trim().Length > 0));
            }
        }

        public string EmailSender { get; set; }

        public string EmailSettings { get; set; }

        public bool EnableAlipayRequest { get; set; }

        public bool EnableCommission { get; set; }

        public bool EnabledCnzz { get; set; }

        public bool EnableGuidePageSet { get; set; }

        public bool EnableOffLineRequest { get; set; }

        public bool EnablePodRequest { get; set; }

        public bool EnableSaleService { get; set; }

        public bool EnableShopMenu { get; set; }

        public bool EnableWapShengPay { get; set; }

        public bool EnableWeixinRed { get; set; }

        public bool EnableWeiXinRequest { get; set; }

        public int FinishedOrderMoney { get; set; }

        public int FinishOrderDays { get; set; }

        public string Footer { get; set; }

        public bool GoodsCheck { get; set; }

        public string GoodsDescription { get; set; }

        public bool GoodsListMenu { get; set; }

        public string GoodsName { get; set; }

        public string GoodsPic { get; set; }

        public bool GoodsType { get; set; }

        public string GuidePageSet { get; set; }

        public bool IsRequestDistributor { get; set; }

        public bool IsValidationService { get; set; }

        public string LogoUrl { get; set; }

        public string ManageOpenID { get; set; }

        public int MaxReturnedDays { get; set; }

        public bool MemberDefault { get; set; }

        public string MemberRoleContent { get; set; }

        public string MentionNowMoney { get; set; }

        public string OfflinePay_Alipay_id { get; set; }

        public string OfflinePay_BankCard_BankName { get; set; }

        public string OfflinePay_BankCard_CardNo { get; set; }

        public string OfflinePay_BankCard_Name { get; set; }

        public string OffLinePayContent { get; set; }

        public bool open_signContinuity { get; set; }

        public bool OpenManyService { get; set; }

        public int OrderShowDays { get; set; }

        public int OrdersPoints { get; set; }

        public decimal PointsRate { get; set; }

        public int PointToCashRate { get; set; }

        public bool PonitToCash_Enable { get; set; }

        public decimal PonitToCash_MaxAmount { get; set; }

        public string RegisterAgreement { get; set; }

        public int RegisterDistributorsPoints { get; set; }

        public string SaleService { get; set; }

        public string ServiceMeiQia { get; set; }

        public int share_Score { get; set; }

        public bool share_score_Enable { get; set; }

        public bool ShareAct_Enable { get; set; }

        public string ShenPay_key { get; set; }

        public string ShenPay_mid { get; set; }

        public bool ShopDefault { get; set; }

        public string ShopHomeDescription { get; set; }

        public string ShopHomeName { get; set; }

        public string ShopHomePic { get; set; }

        public string ShopIntroduction { get; set; }

        public string ShopMenuStyle { get; set; }

        public bool shopping_reward_Enable { get; set; }

        public double shopping_reward_OrderValue { get; set; }

        public int shopping_reward_Score { get; set; }

        public int shopping_Score { get; set; }

        public bool shopping_score_Enable { get; set; }

        public string ShopSpreadingCodeDescription { get; set; }

        public string ShopSpreadingCodeName { get; set; }

        public string ShopSpreadingCodePic { get; set; }

        public string ShopTel { get; set; }

        public int sign_EverDayScore { get; set; }

        public int sign_RewardScore { get; set; }

        public bool sign_score_Enable { get; set; }

        public int sign_StraightDay { get; set; }

        public int SignPoint { get; set; }

        public int SignWhere { get; set; }

        public int SignWherePoint { get; set; }

        public string SiteName { get; set; }

        public string SiteUrl { get; set; }

        public bool SMSEnabled
        {
            get
            {
                return (((!string.IsNullOrEmpty(this.SMSSender) && !string.IsNullOrEmpty(this.SMSSettings)) && (this.SMSSender.Trim().Length > 0)) && (this.SMSSettings.Trim().Length > 0));
            }
        }

        public string SMSSender { get; set; }

        public string SMSSettings { get; set; }

        public bool SubscribeReply { get; set; }

        public decimal TaxRate { get; set; }

        public string Theme { get; set; }

        public string VipCardBG { get; set; }

        public string VipCardLogo { get; set; }

        public string VipCardName { get; set; }

        public string VipCardPrefix { get; set; }

        public string VipCardQR { get; set; }

        public bool VipEnableCoupon { get; set; }

        public string VipRemark { get; set; }

        public bool VipRequireAdress { get; set; }

        public bool VipRequireMobile { get; set; }

        public bool VipRequireName { get; set; }

        public bool VipRequireQQ { get; set; }

        public string VTheme { get; set; }

        public string WeixinAppId { get; set; }

        public string WeixinAppSecret { get; set; }

        public string WeixinCertPassword { get; set; }

        public string WeixinCertPath { get; set; }

        public string WeiXinCodeImageUrl { get; set; }

        public string WeixinLoginUrl { get; set; }

        public string WeixinNumber { get; set; }

        public string WeixinPartnerID { get; set; }

        public string WeixinPartnerKey { get; set; }

        public string WeixinPaySignKey { get; set; }

        public string WeixinToken { get; set; }

        public string YourPriceName { get; set; }

        /// <summary>
        /// 美恰ID
        /// </summary>
        public string  entId { get; set; }
    }
}

