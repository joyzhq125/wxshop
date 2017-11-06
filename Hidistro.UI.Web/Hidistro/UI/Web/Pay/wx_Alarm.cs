﻿namespace Hidistro.UI.Web.Pay
{
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.VShop;
    using Hishop.Weixin.Pay;
    using Hishop.Weixin.Pay.Notify;
    using System;
    using System.Web.UI;

    public class wx_Alarm : Page
    {
        public string wid = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            base.Response.Write("success");
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(false,wid);
            AlarmNotify alarmNotify = new NotifyClient(masterSettings.WeixinAppId, masterSettings.WeixinAppSecret, masterSettings.WeixinPartnerID, masterSettings.WeixinPartnerKey, masterSettings.WeixinPaySignKey).GetAlarmNotify(base.Request.InputStream);
            if (alarmNotify != null)
            {
                AlarmInfo info = new AlarmInfo {
                    AlarmContent = alarmNotify.AlarmContent,
                    AppId = alarmNotify.AppId,
                    Description = alarmNotify.Description
                };
                VShopHelper.SaveAlarm(info);
            }
        }
    }
}

