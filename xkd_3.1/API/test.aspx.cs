using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using com.force.json;
using Hidistro.Core;

public partial class API_webinf1 : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    [WebMethod(EnableSession = true)]
    public static string login()
    {

        return "ok123";

    }
    [WebMethod(EnableSession = true)]
    public static string test(string str, string str2)
    {

        return str + str2;

    }
    [WebMethod(EnableSession = true)]
    public static string testjson(string str, string str2)
    {
        JSONObject j1 = new JSONObject();
        j1.Put("num",20);

        JSONObject j2 = new JSONObject();
        j2.Put("name", "joy");
        j2.Put("age", 35);

        JSONArray ja = new JSONArray();
        ja.Put(j2);

        j1.Put("rows", ja);

        LogHelper.Info(typeof(API_webinf1), "testjson:"+str +str2);
        return j1.ToString();
    }
}