<%@ WebHandler Language="C#" Class="AppAPI" %>

using System;
using System.Web;
using System.Web.SessionState;
using Hidistro.Core;

public class AppAPI : IHttpHandler ,IRequiresSessionState{
    protected string wid = string.Empty;
    public void ProcessRequest (HttpContext context) {
        //context.Response.ContentType = "text/plain";
        //context.Response.Write("Hello World");
        this.wid = Globals.GetCurrentWid();
        switch (context.Request["action"])
        {
            case "countfreighttype":
                return;
            default:
                return;
        }

    }

    public bool IsReusable {
        get {
            return false;
        }
    }

}