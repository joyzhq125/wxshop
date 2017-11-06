namespace Hidistro.UI.Web.Admin
{
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Web;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;
    using System.Xml;

    [AdministerCheck(true)]
    public class SetVThemes : AdminPage
    {
        protected Button btnOK;
        protected HtmlGenericControl customerbg;
        protected HtmlAnchor delpic;
        protected HtmlInputHidden fmSrc;
        protected HiImage imgPic;
        protected Literal litThemeName;
        protected HtmlImage littlepic;
        private string path;
        protected RadioButton RadCustom;
        protected RadioButton RadDefault;
        protected TextBox txtMarketPrice;
        protected TextBox txtNavigate;
        protected TextBox txtPhone;
        protected TextBox txtSalePrice;
        protected TextBox txtTopicProductMaxNum;
        protected HtmlGenericControl txtTopicProductMaxNumTip;
        private string vTheme;

        protected SetVThemes() : base("", "")
        {
        }

        protected void btnOK_Click(object sender, EventArgs e)
        {
            XmlDocument xmlNode = this.GetXmlNode();
            if (this.RadDefault.Checked || string.IsNullOrWhiteSpace(this.fmSrc.Value))
            {
                xmlNode.SelectSingleNode("root/DefaultBg").InnerText = Globals.GetVshopSkinPath(this.vTheme) + "/images/ad/imgDefaultBg.jpg";
            }
            else
            {
                xmlNode.SelectSingleNode("root/DefaultBg").InnerText = this.fmSrc.Value;
            }
            xmlNode.SelectSingleNode("root/TopicProductMaxNum").InnerText = this.txtTopicProductMaxNum.Text;
            xmlNode.SelectSingleNode("root/MarketPrice").InnerText = this.txtMarketPrice.Text;
            xmlNode.SelectSingleNode("root/SalePrice").InnerText = this.txtSalePrice.Text;
            xmlNode.SelectSingleNode("root/Phone").InnerText = this.txtPhone.Text;
            xmlNode.SelectSingleNode("root/Navigate").InnerText = this.txtNavigate.Text;
            xmlNode.Save(this.path);
            HiCache.Remove("TemplateFileCache");
            base.Response.Redirect("ManageVthemes.aspx");
        }

        private XmlDocument GetXmlNode()
        {
            XmlDocument document = new XmlDocument();
            document.Load(this.path);
            return document;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            wid = GetCurWebId();
            if (string.IsNullOrEmpty(wid)) return;
            this.vTheme = base.Request.QueryString["themeName"];
            if (string.IsNullOrWhiteSpace(this.vTheme))
            {
                this.vTheme = SettingsManager.GetMasterSettings(true,wid).VTheme;
            }
            this.path = HttpContext.Current.Request.MapPath(Globals.ApplicationPath + "/Templates/vshop/" + this.vTheme + "/template.xml");
            this.litThemeName.Text = this.vTheme;
            if (!this.Page.IsPostBack)
            {
                XmlDocument xmlNode = this.GetXmlNode();
                this.txtTopicProductMaxNum.Text = xmlNode.SelectSingleNode("root/TopicProductMaxNum").InnerText;
                this.txtMarketPrice.Text = xmlNode.SelectSingleNode("root/MarketPrice").InnerText;
                this.txtSalePrice.Text = xmlNode.SelectSingleNode("root/SalePrice").InnerText;
                this.txtPhone.Text = xmlNode.SelectSingleNode("root/Phone").InnerText;
                this.txtNavigate.Text = xmlNode.SelectSingleNode("root/Navigate").InnerText;
                this.imgPic.ImageUrl = Globals.GetVshopSkinPath(this.vTheme) + "/images/ad/imgDefaultBg.jpg";
                if (xmlNode.SelectSingleNode("root/DefaultBg").InnerText.EndsWith("imgDefaultBg.jpg"))
                {
                    this.RadDefault.Checked = true;
                    this.delpic.Attributes.CssStyle.Add("display", "none");
                    this.customerbg.Attributes.CssStyle.Add("display", "none");
                    this.littlepic.Attributes.CssStyle.Add("display", "none");
                }
                else
                {
                    this.littlepic.Src = xmlNode.SelectSingleNode("root/DefaultBg").InnerText;
                    this.fmSrc.Value = xmlNode.SelectSingleNode("root/DefaultBg").InnerText;
                    this.RadCustom.Checked = true;
                    this.customerbg.Attributes.CssStyle.Remove("display");
                    this.delpic.Attributes.CssStyle.Remove("display");
                    this.littlepic.Attributes.CssStyle.Remove("display");
                }
            }
            this.btnOK.Click += new EventHandler(this.btnOK_Click);
        }
    }
}

