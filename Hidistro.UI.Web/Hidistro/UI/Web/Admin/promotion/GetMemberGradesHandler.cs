namespace Hidistro.UI.Web.Admin.promotion
{
    using Core;
    using Hidistro.ControlPanel.Members;
    using Hidistro.Entities.Members;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Web;
    using System.Web.SessionState;
    public class GetMemberGradesHandler : IHttpHandler,IRequiresSessionState
    {
        protected string wid;
        public void ProcessRequest(HttpContext context)
        {
            wid = context.Session[DTKeys.SESSION_WEB_ID] as string;
            if (string.IsNullOrEmpty(wid))
            {
                return;
            }
            context.Response.ContentType = "text/plain";
            try
            {
                new StringBuilder();
                IList<MemberGradeInfo> memberGrades = MemberHelper.GetMemberGrades(this.wid);
                List<SimpleGradeClass> list2 = new List<SimpleGradeClass>();
                if (memberGrades.Count > 0)
                {
                    foreach (MemberGradeInfo info in memberGrades)
                    {
                        SimpleGradeClass item = new SimpleGradeClass {
                            GradeId = info.GradeId,
                            Name = info.Name
                        };
                        list2.Add(item);
                    }
                }
                var type = new {
                    type = "success",
                    data = list2
                };
                string s = JsonConvert.SerializeObject(type);
                context.Response.Write(s);
            }
            catch (Exception exception)
            {
                context.Response.Write("{\"type\":\"error\",data:\"" + exception.Message + "\"}");
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

