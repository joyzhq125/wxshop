namespace Hidistro.UI.Common.Controls
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using System;
    using System.Web.UI;

    public class CnzzShow : LiteralControl
    {
        public string wid = string.Empty;
        protected override void OnLoad(EventArgs e)
        {
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(true,wid);
            if ((masterSettings.EnabledCnzz && !string.IsNullOrEmpty(masterSettings.CnzzPassword)) && !string.IsNullOrEmpty(masterSettings.CnzzUsername))
            {
                base.Text = "<script src='http://pw.cnzz.com/c.php?id=" + masterSettings.CnzzUsername + "&l=2' language='JavaScript' charset='gb2312'></script>";
            }
            base.OnLoad(e);
        }
    }
}

