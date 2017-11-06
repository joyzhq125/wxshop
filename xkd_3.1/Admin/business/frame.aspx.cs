using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Hidistro.Core;
using Hidistro.ControlPanel.Store;
using Hidistro.Entities.Store;

namespace XKD.Web.Admin
{
    public partial class frame : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            int userId = Globals.GetCurrentManagerUserId();
            if (userId == 0)
            {
                Response.Redirect("/admin/login.aspx");
                return;
            }

            ManagerInfo mgr = Session[DTKeys.SESSION_ADMIN_INFO] as ManagerInfo;
            if (mgr != null)
            {
                //Session[DTKeys.SESSION_BUSNIESE_NUM] = mgr.businessNum;
                //Session[DTKeys.SESSION_ADMIN_ID] = mgr.UserId;
                //Session["UserName"] = mgr.realname;
            }
            else
            {
                Response.Redirect("/admin/login.aspx");
            }

        }
        protected void lbtnExit_Click(object sender, EventArgs e)
        {
            if (HttpContext.Current.Request.Cookies.Get("Vshop-Member") != null)
            {
                HiAffiliation.ClearUserCookie("",false);
            }
            if (HttpContext.Current.Request.Cookies.Get("Vshop-ReferralId") != null)
            {
                HiAffiliation.ClearReferralIdCookie();
            }
            if (HttpContext.Current.Request.Cookies.Get("Vshop-Wid") != null)
            {
                HiAffiliation.ClearWidCookie("", false);
            }
            Response.Redirect("/admin/login.aspx");

            Session[DTKeys.SESSION_WEB_ID] = null;
            Session[DTKeys.SESSION_ADMIN_INFO] = null;
            //SFUtils.WriteCookie("BusnieseName", "Busniese", -14400);
            //Utils.WriteCookie("BusniesePwd", "Busniese", -14400);
        }
    }
}