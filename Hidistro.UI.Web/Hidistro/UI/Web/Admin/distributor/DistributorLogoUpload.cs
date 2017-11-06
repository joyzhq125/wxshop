namespace Hidistro.UI.Web.Admin.distributor
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using System;
    using System.Globalization;
    using System.IO;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;

    public class DistributorLogoUpload : Page
    {
        protected HtmlForm form1;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (base.Request.QueryString["delimg"] != null)
            {
                string path = base.Server.HtmlEncode(base.Request.QueryString["delimg"]);
                path = base.Server.MapPath(path);
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
                base.Response.Write("0");
                base.Response.End();
            }
            int num = int.Parse(base.Request.QueryString["imgurl"]);
            //string wid = base.Request.QueryString["webid"];
            string wid = Session[DTKeys.SESSION_WEB_ID] as string;
            try
            {
                if (num < 1)
                {
                    if (string.IsNullOrEmpty(wid)) return;
                    SF.BLL.sf_website bll = new SF.BLL.sf_website();
                    SF.Model.sf_website website = bll.GetModelByWid(wid);
                    if (website == null) return;

                    HttpPostedFile file = base.Request.Files["Filedata"];
                    string str2 = DateTime.Now.ToString("yyyyMMddHHmmss_ffff", DateTimeFormatInfo.InvariantInfo);
                    string str3 = "/Storage/data/DistributorLogoPic/";
                    string str4 = str2 + Path.GetExtension(file.FileName);
                    file.SaveAs(Globals.MapPath(str3 + str4));
                    base.Response.StatusCode = 200;
                    base.Response.Write(str2 + "|/Storage/data/DistributorLogoPic/" + str4);
                    //SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
                    string distributorLogoPic = website.logo;//masterSettings.DistributorLogoPic;
                    distributorLogoPic = base.Server.MapPath(distributorLogoPic);
                    if (File.Exists(distributorLogoPic))
                    {
                        File.Delete(distributorLogoPic);
                    }
                    /*masterSettings.DistributorLogoPic*/
                    website.logo = "/Storage/data/DistributorLogoPic/" + str4;
                    //SettingsManager.Save(masterSettings);
                    bll.Update(website);
                }
                else
                {
                    base.Response.Write("0");
                }
            }
            catch (Exception)
            {
                base.Response.StatusCode = 500;
                base.Response.Write("服务器错误");
                base.Response.End();
            }
            finally
            {
                base.Response.End();
            }
        }
    }
}

