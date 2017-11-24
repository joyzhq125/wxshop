using com.force.json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using Hidistro.Core;

public partial class API_app : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    [WebMethod(EnableSession = true)]
    public static string login(string userName,string password)
    {
        JSONObject j = new JSONObject();
        LogHelper.Info(typeof(API_app),"username:"+ userName+" psw:"+ password);
        j.Put("statusCode", 1);
        j.Put("content", "");
        return j.ToString();

    }
}