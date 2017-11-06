namespace HiTemplate
{
    using Hidistro.Core;
    using HiTemplate.Model;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using System;
    using System.ComponentModel;
    using System.IO;
    using System.Net;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Web.UI;

    public class GoodsListModule : RazorModuleWebControl
    {
        public string wid = string.Empty;
        public string GetDataJson()
        {
            Exception exception;
            string str = "";
            try
            {
                //传递wid参数
                this.wid = Globals.GetCurrentWid();
                string s = string.Format("GroupID={0}&GoodListSize={1}&FirstPriority={2}&SecondPriority={3}&ShowPrice={4}&Layout={5}&ShowIco={6}&ShowName={7}&wid={8}", new object[] { this.GroupID, this.GoodListSize, this.FirstPriority, this.SecondPriority, base.ShowPrice, base.Layout, base.ShowIco, base.ShowName,this.wid });
                byte[] bytes = Encoding.UTF8.GetBytes(s);
                HttpWebRequest request = (HttpWebRequest) WebRequest.Create(Urls.ApplicationPath + base.DataUrl);
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = bytes.Length;
                try
                {
                    request.GetRequestStream().Write(bytes, 0, bytes.Length);
                    HttpWebResponse response = (HttpWebResponse) request.GetResponse();
                    StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                    StringBuilder builder = new StringBuilder();
                    while (-1 != reader.Peek())
                    {
                        builder.Append(reader.ReadLine());
                    }
                    str = builder.ToString();
                }
                catch (Exception exception1)
                {
                    exception = exception1;
                    str = "错误：" + exception.Message;
                }
            }
            catch (Exception exception2)
            {
                exception = exception2;
                str = "错误：" + exception.Message;
            }
            return str;
        }

        protected override void Render(HtmlTextWriter writer)
        {
            Hi_Json_GoodGourpContent jsonData = ((JObject) JsonConvert.DeserializeObject(this.GetDataJson())).ToObject<Hi_Json_GoodGourpContent>();
            base.RenderModule(writer, jsonData);
        }

        [Bindable(true)]
        public string FirstPriority { get; set; }

        [Bindable(true)]
        public string GoodListSize { get; set; }

        [Bindable(true)]
        public string GroupID { get; set; }

        [Bindable(true)]
        public string SecondPriority { get; set; }

        [Bindable(true)]
        public string ShowOrder { get; set; }

        [Bindable(true)]
        public string ThirdPriority { get; set; }
    }
}

