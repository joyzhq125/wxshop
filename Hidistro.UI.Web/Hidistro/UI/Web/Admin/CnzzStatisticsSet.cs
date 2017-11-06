namespace Hidistro.UI.Web.Admin
{
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.IO;
    using System.Net;
    using System.Web.Security;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    [AdministerCheck(true)]
    public class CnzzStatisticsSet : AdminPage
    {
        protected HtmlGenericControl div_pan1;
        protected HtmlGenericControl div_pan2;
        protected LinkButton hlinkCreate;
        protected LinkButton hplinkSet;
        protected Literal litThemeName;

        protected CnzzStatisticsSet() : base("", "")
        {
        }

        protected void hlinkCreate_Click(object sender, EventArgs e)
        {
            string host = this.Page.Request.Url.Host;
            string str2 = FormsAuthentication.HashPasswordForStoringInConfigFile(host + "A9jkLUxm", "MD5").ToLower();
            HttpWebRequest request = (HttpWebRequest) WebRequest.Create("http://wss.cnzz.com/user/companion/92hi.php?domain=" + host + "&key=" + str2);
            Stream responseStream = ((HttpWebResponse) request.GetResponse()).GetResponseStream();
            responseStream.ReadTimeout = 100;
            StreamReader reader = new StreamReader(responseStream);
            string str4 = reader.ReadToEnd().Trim();
            reader.Close();
            if (str4.IndexOf("@") == -1)
            {
                this.ShowMsg("创建账号失败", false);
            }
            else
            {
                SiteSettings masterSettings = SettingsManager.GetMasterSettings(true,wid);
                string[] strArray = str4.Split(new char[] { '@' });
                masterSettings.CnzzUsername = strArray[0];
                masterSettings.CnzzPassword = strArray[1];
                masterSettings.EnabledCnzz = false;
                this.div_pan1.Visible = false;
                this.div_pan2.Visible = true;
                this.hplinkSet.Text = "开启统计功能";
                SettingsManager.Save(masterSettings);
                this.ShowMsg("创建账号成功", true);
            }
        }

        protected void hplinkSet_Click(object sender, EventArgs e)
        {
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(true,wid);
            this.div_pan1.Visible = false;
            this.div_pan2.Visible = true;
            if (masterSettings.EnabledCnzz)
            {
                masterSettings.EnabledCnzz = false;
                this.hplinkSet.Text = "开启统计功能";
                SettingsManager.Save(masterSettings);
                this.ShowMsg("关闭统计功能成功", true);
            }
            else
            {
                masterSettings.EnabledCnzz = true;
                this.hplinkSet.Text = "关闭统计功能";
                SettingsManager.Save(masterSettings);
                this.ShowMsg("开启统计功能成功", true);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.hlinkCreate.Click += new EventHandler(this.hlinkCreate_Click);
            this.hplinkSet.Click += new EventHandler(this.hplinkSet_Click);
            wid = GetCurWebId();
            if (string.IsNullOrEmpty(wid)) return;
            if (!base.IsPostBack)
            {
                SiteSettings masterSettings = SettingsManager.GetMasterSettings(true,wid);
                if (string.IsNullOrEmpty(masterSettings.CnzzPassword) || string.IsNullOrEmpty(masterSettings.CnzzUsername))
                {
                    this.div_pan1.Visible = true;
                    this.div_pan2.Visible = false;
                }
                else
                {
                    this.div_pan1.Visible = false;
                    this.div_pan2.Visible = true;
                    if (masterSettings.EnabledCnzz)
                    {
                        this.hplinkSet.Text = "关闭统计功能";
                    }
                    else
                    {
                        this.hplinkSet.Text = "开启统计功能";
                    }
                }
            }
        }
    }
}

