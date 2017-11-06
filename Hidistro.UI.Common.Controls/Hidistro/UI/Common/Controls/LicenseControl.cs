namespace Hidistro.UI.Common.Controls
{
    using Hidistro.Core;
    using System;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class LicenseControl : WebControl
    {
        public string wid = string.Empty;
        private readonly string renderFormat = "<a href=\"http://www.chenduo.com\">辰多技术支持</a>";

        protected override void Render(HtmlTextWriter writer)
        {
            //if (!CopyrightLicenser.CheckCopyright(wid))
            //{
            //    writer.Write(string.Format(this.renderFormat, SettingsManager.GetMasterSettings(false, wid).SiteUrl));
            //}
        }
    }
}

