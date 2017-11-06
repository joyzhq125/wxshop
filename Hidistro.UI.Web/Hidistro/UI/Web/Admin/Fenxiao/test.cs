namespace Hidistro.UI.Web.Admin.Fenxiao
{
    using Hidistro.Core;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using Newtonsoft.Json;
    using System;
    using System.Data;
    using System.Data.Common;
    using System.Web.UI;

    public class test : Page
    {
        private Database database;

        protected void Page_Load(object sender, EventArgs e)
        {
            string str = base.Request.QueryString["tb"];
            if (!string.IsNullOrEmpty(str))
            {
                this.database = DatabaseFactory.CreateDatabase();
                string str2 = "select top 1 * from " + str;
                DataTable table = new DataTable();
                DbCommand sqlStringCommand = this.database.GetSqlStringCommand(str2);
                using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
                {
                    table = DataHelper.ConverDataReaderToDataTable(reader);
                }
                if (table != null)
                {
                    base.Response.Write(JsonConvert.SerializeObject(table));
                }
                else
                {
                    base.Response.Write("test1");
                }
            }
            else
            {
                base.Response.Write("test");
            }
        }
    }
}

