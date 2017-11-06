namespace Hidistro.UI.Web.umeditor
{
    using Hidistro.UI.Web.umeditor.controls;
    using System;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class test : Page
    {
        protected Button btnSubmit;
        protected HtmlForm form1;
        protected Label lblTest;
        protected ucEditor ucEditor1;

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string text = this.ucEditor1.Text;
            this.lblTest.Text = text;
            this.ucEditor1.Text = text;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.ucEditor1.Text = "测试1";
        }
    }
}

