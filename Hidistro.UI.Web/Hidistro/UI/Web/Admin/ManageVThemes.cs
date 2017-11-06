namespace Hidistro.UI.Web.Admin
{
    using Hidistro.Core;
    using Hidistro.Core.Configuration;
    using Hidistro.Core.Entities;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Web;
    using System.Web.UI.WebControls;
    using System.Xml;

    public class ManageVThemes : AdminPage
    {
        protected DataList dtManageThemes;
        protected Literal lblThemeCount;
        protected Literal litThemeName;

        protected ManageVThemes() : base("", "")
        {
        }

        private void BindData(SiteSettings siteSettings)
        {
            IList<ManageThemeInfo> list = this.LoadThemes(siteSettings.VTheme);
            this.dtManageThemes.DataSource = list;
            this.dtManageThemes.DataBind();
            this.lblThemeCount.Text = list.Count.ToString();
        }

        private void dtManageThemes_ItemCommand(object sender, DataListCommandEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                string name = this.dtManageThemes.DataKeys[e.Item.ItemIndex].ToString();
                if (e.CommandName == "btnUse")
                {
                    this.UserThems(name);
                    this.ShowMsg("成功修改了店铺模板", true);
                }
            }
        }

        protected IList<ManageThemeInfo> LoadThemes(string currentThemeName)
        {
            XmlDocument document = new XmlDocument();
            IList<ManageThemeInfo> list = new List<ManageThemeInfo>();
            string path = HttpContext.Current.Request.PhysicalApplicationPath + HiConfiguration.GetConfig().FilesPath + @"\Templates\vshop";
            string[] strArray = Directory.Exists(path) ? Directory.GetDirectories(path) : null;
            ManageThemeInfo item = null;
            foreach (string str3 in strArray)
            {
                DirectoryInfo info2 = new DirectoryInfo(str3);
                string str2 = info2.Name.ToLower(CultureInfo.InvariantCulture);
                if ((str2.Length > 0) && !str2.StartsWith("_"))
                {
                    foreach (FileInfo info3 in info2.GetFiles("template.xml"))
                    {
                        item = new ManageThemeInfo();
                        FileStream inStream = info3.OpenRead();
                        document.Load(inStream);
                        inStream.Close();
                        item.Name = document.SelectSingleNode("root/Name").InnerText;
                        item.ThemeImgUrl = Globals.ApplicationPath + "/Templates/vshop/" + str2 + "/" + document.SelectSingleNode("root/ImageUrl").InnerText;
                        item.ThemeName = str2;
                        if (string.Compare(item.ThemeName, currentThemeName) == 0)
                        {
                            this.litThemeName.Text = item.ThemeName;
                        }
                        list.Add(item);
                    }
                }
            }
            return list;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            wid = GetCurWebId();
            if (string.IsNullOrEmpty(wid)) return;
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(true,wid);
            this.litThemeName.Text = masterSettings.VTheme;
            this.dtManageThemes.ItemCommand += new DataListCommandEventHandler(this.dtManageThemes_ItemCommand);
            if (!this.Page.IsPostBack)
            {
                this.BindData(masterSettings);
            }
        }

        protected void UserThems(string name)
        {
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(true,wid);
            masterSettings.VTheme = name;
            SettingsManager.Save(masterSettings);
            HiCache.Remove("TemplateFileCache");
            this.BindData(masterSettings);
        }
    }
}

