namespace Hidistro.UI.Web.API
{
    using Core.Configuration;
    using Hidistro.Core;
    using Hishop.Weixin.MP.Util;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Web;
    using System.Xml;

    public class wx : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            HttpRequest request = context.Request;
            //string weixinToken = SettingsManager.GetMasterSettings(false).WeixinToken;
            SF.Model.sf_website website;
            string wid = request["wid"];
            if(string.IsNullOrEmpty(wid))
            {
                context.Response.Write("参数非法");
                return;
            }

            website = new SF.BLL.sf_website().GetModelByWid(wid);
            if(string.IsNullOrEmpty(website.token_value))
            {
                context.Response.Write("不存在该微信号");
                return;
            }

            string signature = request["signature"];
            string nonce = request["nonce"];
            string timestamp = request["timestamp"];
            string s = request["echostr"];
            
            if (request.HttpMethod == "GET")
            {
                if (CheckSignature.Check(signature, timestamp, nonce, website.token_value))
                {
                    context.Response.Write(s);//返回随机字符串则表示验证通过
                }
                else
                {
                    context.Response.Write("failed:" + signature + ",token:" + website.token_value + " " + CheckSignature.GetSignature(timestamp, nonce, website.token_value) +
                                "。如果你在浏览器中看到这句话，说明此地址可以被作为微信公众账号后台的Url，请注意保持Token一致。");
                }
                context.Response.End();
            }
            else
            {
                try
                {
                    CustomMsgHandler handler = new CustomMsgHandler(request.InputStream);
                    handler.wid = wid;

                    //测试时可开启此记录，帮助跟踪数据
                    if (HiConfiguration.LOG_LEVENL >= 3)
                    {
                        handler.RequestDocument.Save(context.Server.MapPath("~/logs/" + DateTime.Now.Ticks + "_Request_" +
                                       handler.RequestMessage.FromUserName + ".txt"));
                    }
                    handler.Execute();

                    if (handler.ResponseDocument != null && HiConfiguration.LOG_LEVENL >= 3)
                    {
                        handler.ResponseDocument.Save(context.Server.MapPath("~/logs/" + DateTime.Now.Ticks + "_Response_" +
                                       handler.ResponseMessage.ToUserName + ".txt"));
                    }
                    context.Response.Write(handler.ResponseDocument == null ? string.Empty : handler.ResponseDocument.ToString());
                }
                catch (Exception exception)
                {
                    StreamWriter writer = File.AppendText(context.Server.MapPath("error.txt"));
                    writer.WriteLine(exception.Message);
                    writer.WriteLine(exception.StackTrace);
                    writer.WriteLine(DateTime.Now);
                    writer.Flush();
                    writer.Close();
                }
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

