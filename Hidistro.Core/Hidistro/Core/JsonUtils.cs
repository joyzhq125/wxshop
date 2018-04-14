using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
namespace Hidistro.Core
{
    public class JsonUtils
    {
        // 从一个对象信息生成Json串  
        public static string ObjectToJson<T>(T obj)
        {
            return JsonConvert.SerializeObject(obj);
        }
        // 从一个Json串生成对象信息  
        public static object JsonToObject(string jsonString, object obj)
        {
            return JsonConvert.DeserializeObject(jsonString, obj.GetType());
        }
        // List转换成json字符串  
        public static string ListToJson<T>(List<T> list)
        {

            return JsonConvert.SerializeObject(list);
        }
        // json字符串转换成List  
        public static T JsonToList<T>(string json)
        {

            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}

