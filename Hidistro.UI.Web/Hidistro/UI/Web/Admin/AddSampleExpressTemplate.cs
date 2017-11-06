namespace Hidistro.UI.Web.Admin
{
    using Hidistro.ControlPanel.Store;
    using Hidistro.Entities;
    using Hidistro.Entities.Store;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Data;
    using System.Text;

    [PrivilegeCheck(Privilege.ExpressTemplates)]
    public class AddSampleExpressTemplate : AdminPage
    {
        protected string ems;

        protected AddSampleExpressTemplate() : base("", "")
        {
            this.ems = "";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!base.IsPostBack)
            {
                DataTable expressTable = ExpressHelper.GetExpressTable();
                StringBuilder builder = new StringBuilder();
                foreach (DataRow row in expressTable.Rows)
                {
                    builder.AppendFormat("<option value='{0}'>{0}</option>", row["Name"]);
                }
                this.ems = builder.ToString();
            }
            string str = this.Page.Request.QueryString["ExpressName"];
            string str2 = this.Page.Request.QueryString["XmlFile"];
            if ((string.IsNullOrEmpty(str) || string.IsNullOrEmpty(str2)) || !str2.EndsWith(".xml"))
            {
                base.GotoResourceNotFound();
            }
        }
    }
}

