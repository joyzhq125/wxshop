namespace Hidistro.UI.Web
{
    using Hidistro.Core.Jobs;
    using Hidistro.UI.Web.API;
    using Quartz;
    using Quartz.Impl;
    using Quartz.Spi;
    using System;
    using System.IO;
    using System.Web;

    public class Global : HttpApplication
    {
        private readonly TriggerFiredBundle firedTriggerBundle;
        private IScheduler sched;
        private ISchedulerFactory sf;

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            try
            {
                string str = "ASPSESSID";
                string str2 = "ASP.NET_SESSIONID";
                if (HttpContext.Current.Request.Form[str] != null)
                {
                    this.UpdateCookie(str2, HttpContext.Current.Request.Form[str]);
                }
                else if (HttpContext.Current.Request.QueryString[str] != null)
                {
                    this.UpdateCookie(str2, HttpContext.Current.Request.QueryString[str]);
                }
            }
            catch (Exception)
            {
                base.Response.StatusCode = 500;
                base.Response.Write("Error Initializing Session");
            }
        }

        protected void Application_End(object sender, EventArgs e)
        {
            Hidistro.Core.Jobs.Jobs.Instance().Stop();
            if (this.sched != null)
            {
                this.sched.Shutdown();
            }
        }

        protected void Application_Error(object sender, EventArgs e)
        {
        }

        protected void Application_Start(object sender, EventArgs e)
        {
            Hidistro.Core.Jobs.Jobs.Instance().Start();
            try
            {
                if (!Directory.Exists(base.Server.MapPath("/log")))
                {
                    Directory.CreateDirectory(base.Server.MapPath("/log"));
                }
                TLog.LogFile = base.Server.MapPath("/log/") + "JobLog" + DateTime.Now.ToString("yyyyMM") + ".log";
                this.StartPlan();
                TLog.SaveLog("Application_Start：已启动定时计划。");
            }
            catch (Exception exception)
            {
                TLog.SaveLog("Application_Start：启动定时计划出错：原因：" + exception.Message);
            }
        }

        protected void Session_End(object sender, EventArgs e)
        {
        }

        protected void Session_Start(object sender, EventArgs e)
        {
        }

        private void StartPlan()
        {
            string xmlFile = base.Server.MapPath("/JobConfig.xml");
            string nodeValue = XmlHelper.GetNodeValue(xmlFile, "JobName");
            string str3 = XmlHelper.GetNodeValue(xmlFile, "JobGoup");
            string str4 = XmlHelper.GetNodeValue(xmlFile, "CronSchedule");
            this.sched = new StdSchedulerFactory().GetScheduler();
            IJobDetail detail = JobBuilder.Create<Hidistro.UI.Web.Jobs.ShiftNotify>().WithIdentity(nodeValue, str3).Build();
            ITrigger trigger = CronScheduleTriggerBuilderExtensions.WithCronSchedule(TriggerBuilder.Create().WithIdentity(nodeValue, str3), str4).Build();
            this.sched.ScheduleJob(detail, trigger);
            this.sched.Start();
        }

        private void UpdateCookie(string cookie_name, string cookie_value)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies.Get(cookie_name);
            if (cookie == null)
            {
                cookie = new HttpCookie(cookie_name);
                HttpContext.Current.Request.Cookies.Add(cookie);
            }
            cookie.Value = cookie_value;
            HttpContext.Current.Request.Cookies.Set(cookie);
        }
    }
}

