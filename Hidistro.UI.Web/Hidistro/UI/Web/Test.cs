namespace Hidistro.UI.Web
{
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Members;
    using Hidistro.SaleSystem.Vshop;
    using System;
    using System.Collections.Generic;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class Test : Page
    {
        protected Button Button1;
        protected HtmlForm form1;
        public string wid = string.Empty;
        protected void Button1_Click(object sender, EventArgs e)
        {
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            if (currentMember != null)
            {
                if (UserSignHelper.IsSign(currentMember.UserId))
                {
                    int num = UserSignHelper.USign(currentMember.UserId,wid);
                    base.Response.Write("增加:" + num + "分");
                }
                else
                {
                    base.Response.Write("已签到");
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(true,wid);
            base.Response.Write(masterSettings.WeixinCertPath);
            base.Response.Write(masterSettings.WeixinPartnerID);
            base.Response.End();
            Guid.NewGuid().ToString();
            List<int> values = new List<int> { 1, 3, 4 };
            base.Response.Write(string.Join<int>(",", values));
            base.Response.End();
        }
    }
}

