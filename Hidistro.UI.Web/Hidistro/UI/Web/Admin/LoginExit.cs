namespace Hidistro.UI.Web.Admin
{
    using Core;
    using System;
    using System.Web;
    using System.Web.UI;

    public class LoginExit : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            HttpCookie cookie = new HttpCookie("Vshop-Manager") {
                Expires = DateTime.Now
            };
            HttpContext.Current.Response.Cookies.Add(cookie);
            base.Response.Redirect("Login.aspx", true);

            Session[DTKeys.SESSION_WEB_ID] = null;
            Session[DTKeys.SESSION_ADMIN_INFO] = null;
        }
    }
}

