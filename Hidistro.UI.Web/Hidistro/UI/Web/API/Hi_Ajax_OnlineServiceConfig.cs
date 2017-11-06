namespace Hidistro.UI.Web.API
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using System;
    using System.Text;
    using System.Web;
    using System.Web.SessionState;
    public class Hi_Ajax_OnlineServiceConfig : IHttpHandler, IRequiresSessionState
    {
        protected string wid;
        public void ProcessRequest(HttpContext context)
        {
            //wid = context.Session[DTKeys.SESSION_WEB_ID] as string;
            //if (string.IsNullOrEmpty(wid))
            //{
            //    return;
            //}
            this.wid = Globals.GetCurrentWid();
            context.Response.ContentType = "text/plain";
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(false, wid);
            string entId = masterSettings.entId;
            if (!masterSettings.EnableSaleService && string.IsNullOrEmpty(entId) )
            {
                context.Response.Write("");
            }
            else
            {
                //CustomerServiceSettings masterSettings = CustomerServiceManager.GetMasterSettings(false);
                //context.Response.Write(string.Format("<script src='//meiqia.com/js/mechat.js?unitid={0}' charset='UTF-8' async='async'></script>", masterSettings.unitid));

                //string content = string.Format("<script type='text/javascript'>(function(m, ei, q, i, a, j, s) {m[i] = m[i] || function() {(m[i].a = m[i].a || []).push(arguments)};j = ei.createElement(q),s = ei.getElementsByTagName(q)[0];j.async = true;j.charset = 'UTF-8';j.src = '//static.meiqia.com/dist/meiqia.js?_=t';s.parentNode.insertBefore(j, s);})(window, document, 'script', '_MEIQIA');_MEIQIA('entId', {0});</script> ", uid);
                //string content = @"<script type='text/javascript'>(function(m, ei, q, i, a, j, s) {m[i] = m[i] || function() {(m[i].a = m[i].a || []).push(arguments)};j = ei.createElement(q),s = ei.getElementsByTagName(q)[0];j.async = true;j.charset = 'UTF-8';j.src = '//static.meiqia.com/dist/meiqia.js?_=t';s.parentNode.insertBefore(j, s);})(window, document, 'script', '_MEIQIA');_MEIQIA('entId', 64612);</script> ";
                StringBuilder content = new StringBuilder();
                content.Append("<script type='text/javascript'>");
                content.Append("(function(m, ei, q, i, a, j, s) {");
                content.Append("m[i] = m[i] || function() {");
                content.Append("(m[i].a = m[i].a || []).push(arguments)");
                content.Append("};");
                content.Append("j = ei.createElement(q),");
                content.Append("s = ei.getElementsByTagName(q)[0];");
                content.Append("j.async = true;");
                content.Append("j.charset = 'UTF-8';");
                content.Append("j.src = '//static.meiqia.com/dist/meiqia.js?_=t';");
                content.Append("s.parentNode.insertBefore(j, s);");
                content.Append("})(window, document, 'script', '_MEIQIA');");
                content.AppendFormat("_MEIQIA('entId', {0});", entId);
                content.Append("</script>");
                context.Response.Write(content.ToString());
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}

