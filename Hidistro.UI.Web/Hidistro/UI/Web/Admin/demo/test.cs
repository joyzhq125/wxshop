namespace Hidistro.UI.Web.Admin.demo
{
    using Hidistro.UI.ControlPanel.Utility;
    using Hidistro.UI.Web.Admin.Ascx;
    using System;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class test : AdminPage
    {
        protected Button btnSave;
        protected HtmlForm form1;
        protected Label lblTest;
        protected ucDateTimePicker ucDateTimePicker1;

        protected test() : base("m02", "spp06")
        {
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string str = string.Empty;
            if (this.ucDateTimePicker1.SelectedDate.HasValue)
            {
                str = this.ucDateTimePicker1.SelectedDate.ToString();
                this.lblTest.Text = str;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }
    }
}

