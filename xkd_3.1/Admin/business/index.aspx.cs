using Hidistro.Core;
using Hidistro.Entities.Store;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace XKD.Web.Admin
{
    public partial class index : AdminPage
    {
        protected ManagerInfo admin_info; //管理员信息
        protected string str = "注销登录";
        protected string uri = "center.aspx";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                admin_info = GetAdminInfo();
                if(admin_info == null)
                {
                    Response.Redirect("/admin/login.aspx");
                }
                else if (admin_info.RoleId != 1)
                {
                    //str = "公众号列表";
                    //uri = "first.aspx";
                    Response.Redirect("/admin/default.aspx");
                }
            }
        }
        //安全退出
        protected void lbtnExit_Click(object sender, EventArgs e)
        {
            admin_info = GetAdminInfo();
            if (admin_info.RoleId == 1)
            {
                //Session[DTKeys.SESSION_TEMPLATES_INFO] = null;
                Session[DTKeys.SESSION_APP_INFO] = null;
                Response.Redirect("/admin/business/frame.aspx");
            }
            else
            {
                Session[DTKeys.SESSION_ADMIN_INFO] = null;
                //Utils.WriteCookie("AdminName", "DTcms", -14400);
                //Utils.WriteCookie("AdminPwd", "DTcms", -14400);
                Response.Redirect("login.aspx");
            }
        }
    }
}