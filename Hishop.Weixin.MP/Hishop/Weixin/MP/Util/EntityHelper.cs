namespace Hishop.Weixin.MP.Util
{
    using Hishop.Weixin.MP.Domain;
    using Response;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Xml.Linq;

    public static class EntityHelper
    {
        public static XDocument ConvertEntityToXml<T>(T entity) where T: class, new()
        {
            //if (entity == null)
            //{
            //}
            //entity = Activator.CreateInstance<T>();
            entity = entity ?? new T();
            XDocument document = new XDocument();
            document.Add(new XElement("xml"));
            XElement root = document.Root;

            //List<string> list = new string[] { "ToUserName", "FromUserName", "CreateTime", "MsgType", "Content", "ArticleCount", "Articles", "FuncFlag", "Title ", "Description ", "PicUrl", "Url" }.ToList<string>();
            /* 注意！
             * 经过测试，微信对字段排序有严格要求，这里对排序进行强制约束
            */
            var propNameOrder = new List<string>() { "ToUserName", "FromUserName", "CreateTime", "MsgType" };
            //不同返回类型需要对应不同特殊格式的排序
            if (entity is NewsResponse)
            {
                propNameOrder.AddRange(new[] { "ArticleCount", "Articles", "FuncFlag",/*以下是Atricle属性*/ "Title ", "Description ", "PicUrl", "Url" });
            }
            //else if (entity is ResponseMessageTransfer_Customer_Service)
            //{
            //    propNameOrder.AddRange(new[] { "TransInfo", "KfAccount", "FuncFlag" });
            //}
            else if (entity is MusicResponse)
            {
                propNameOrder.AddRange(new[] { "Music", "FuncFlag", "ThumbMediaId",/*以下是Music属性*/ "Title ", "Description ", "MusicUrl", "HQMusicUrl" });
            }
            else if (entity is ImageResponse)
            {
                propNameOrder.AddRange(new[] { "Image",/*以下是Image属性*/ "MediaId " });
            }
            else if (entity is VoiceResponse)
            {
                propNameOrder.AddRange(new[] { "Voice",/*以下是Voice属性*/ "MediaId " });
            }
            else if (entity is VideoResponse)
            {
                propNameOrder.AddRange(new[] { "Video",/*以下是Video属性*/ "MediaId ", "Title", "Description" });
            }
            else
            {
                //如Text类型
                propNameOrder.AddRange(new[] { "Content", "FuncFlag" });
            }


            Func<string, int> orderByPropName = new Func<string, int>(propNameOrder.IndexOf);
            foreach (PropertyInfo info in (from p in entity.GetType().GetProperties()
                orderby orderByPropName(p.Name)
                select p).ToList<PropertyInfo>())
            {
                DateTime time;
                string name = info.Name;
                if (name == "Articles")
                {
                    XElement content = new XElement("Articles");
                    List<Hishop.Weixin.MP.Domain.Article> list3 = info.GetValue(entity, null) as List<Hishop.Weixin.MP.Domain.Article>;
                    foreach (Hishop.Weixin.MP.Domain.Article article in list3)
                    {
                        IEnumerable<XElement> enumerable = ConvertEntityToXml<Hishop.Weixin.MP.Domain.Article>(article).Root.Elements();
                        content.Add(new XElement("item", enumerable));
                    }
                    root.Add(content);
                }
                else
                {
                    string str2 = info.PropertyType.Name;
                    if (str2 == null)
                    {
                        goto Label_0328;
                    }
                    if (!(str2 == "String"))
                    {
                        if (str2 == "DateTime")
                        {
                            goto Label_0252;
                        }
                        if (str2 == "Boolean")
                        {
                            goto Label_028A;
                        }
                        if (str2 == "ResponseMsgType")
                        {
                            goto Label_02D0;
                        }
                        if (str2 == "Article")
                        {
                            goto Label_02FC;
                        }
                        goto Label_0328;
                    }
                    root.Add(new XElement(name, new XCData((info.GetValue(entity, null) as string) ?? "")));
                }
                continue;
            Label_0252:
                time = (DateTime) info.GetValue(entity, null);
                root.Add(new XElement(name, time.Ticks));
                continue;
            Label_028A:
                if (!(name == "FuncFlag"))
                {
                    goto Label_0328;
                }
                root.Add(new XElement(name, ((bool) info.GetValue(entity, null)) ? "1" : "0"));
                continue;
            Label_02D0:
                root.Add(new XElement(name, info.GetValue(entity, null).ToString().ToLower()));
                continue;
            Label_02FC:
                root.Add(new XElement(name, info.GetValue(entity, null).ToString().ToLower()));
                continue;
            Label_0328:
                root.Add(new XElement(name, info.GetValue(entity, null)));
            }
            return document;
        }

        public static void FillEntityWithXml<T>(T entity, XDocument doc) where T : /*AbstractRequest*/ class, new()
        {
            //if (entity == null)
            //{
            //}
            //修改无法解析完整数据 2017.7.4
            entity = entity ?? new T();
            //entity = Activator.CreateInstance<T>();
            XElement root = doc.Root;
            foreach (PropertyInfo info in entity.GetType().GetProperties())
            {
                string name = info.Name;
                if (root.Element(name) != null)
                {
                    switch (info.PropertyType.Name)
                    {
                        case "DateTime":
                            {
                                info.SetValue(entity, new DateTime(long.Parse(root.Element(name).Value)), null);
                                continue;
                            }
                        case "Boolean":
                            {
                                if (!(name == "FuncFlag"))
                                {
                                    break;
                                }
                                info.SetValue(entity, root.Element(name).Value == "1", null);
                                continue;
                            }
                        case "Int64":
                            {
                                info.SetValue(entity, long.Parse(root.Element(name).Value), null);
                                continue;
                            }
                        case "Int32":
                            {
                                info.SetValue(entity, int.Parse(root.Element(name).Value), null);
                                continue;
                            }
                        case "RequestEventType":
                            {
                                info.SetValue(entity, EventTypeHelper.GetEventType(root.Element(name).Value), null);
                                continue;
                            }
                        case "RequestMsgType":
                            {
                                info.SetValue(entity, MsgTypeHelper.GetMsgType(root.Element(name).Value), null);
                                continue;
                            }


                    }
                    info.SetValue(entity, root.Element(name).Value, null);
                }
            }
        }


        /// <summary>
        /// 根据XML信息填充实实体
        /// </summary>
        /// <typeparam name="T">MessageBase为基类的类型，Response和Request都可以</typeparam>
        /// <param name="entity">实体</param>
        /// <param name="doc">XML</param>
        //public static void FillEntityWithXml<T>(this T entity, XDocument doc) where T : /*MessageBase*/ class, new()
        //{
        //    entity = entity ?? new T();
        //    var root = doc.Root;

        //    var props = entity.GetType().GetProperties();
        //    foreach (var prop in props)
        //    {
        //        var propName = prop.Name;
        //        if (root.Element(propName) != null)
        //        {
        //            switch (prop.PropertyType.Name)
        //            {
        //                //case "String":
        //                //    goto default;
        //                case "DateTime":
        //                    prop.SetValue(entity, DateTimeHelper.GetDateTimeFromXml(root.Element(propName).Value), null);
        //                    break;
        //                case "Boolean":
        //                    if (propName == "FuncFlag")
        //                    {
        //                        prop.SetValue(entity, root.Element(propName).Value == "1", null);
        //                    }
        //                    else
        //                    {
        //                        goto default;
        //                    }
        //                    break;
        //                case "Int32":
        //                    prop.SetValue(entity, int.Parse(root.Element(propName).Value), null);
        //                    break;
        //                case "Int64":
        //                    prop.SetValue(entity, long.Parse(root.Element(propName).Value), null);
        //                    break;
        //                case "Double":
        //                    prop.SetValue(entity, double.Parse(root.Element(propName).Value), null);
        //                    break;
        //                //以下为枚举类型
        //                case "RequestMsgType":
        //                    //已设为只读
        //                    //prop.SetValue(entity, MsgTypeHelper.GetRequestMsgType(root.Element(propName).Value), null);
        //                    break;
        //                case "ResponseMsgType"://Response适用
        //                    //已设为只读
        //                    //prop.SetValue(entity, MsgTypeHelper.GetResponseMsgType(root.Element(propName).Value), null);
        //                    break;
        //                case "Event":
        //                    //已设为只读
        //                    //prop.SetValue(entity, EventHelper.GetEventType(root.Element(propName).Value), null);
        //                    break;
        //                //以下为实体类型
        //                case "List`1"://List<T>类型，ResponseMessageNews适用
        //                    var genericArguments = prop.PropertyType.GetGenericArguments();
        //                    if (genericArguments[0].Name == "Article")//ResponseMessageNews适用
        //                    {
        //                        //文章下属节点item
        //                        List<Hishop.Weixin.MP.Domain.Article> articles = new List<Hishop.Weixin.MP.Domain.Article>();
        //                        foreach (var item in root.Element(propName).Elements("item"))
        //                        {
        //                            var article = new Hishop.Weixin.MP.Domain.Article();
        //                            FillEntityWithXml(article, new XDocument(item));
        //                            articles.Add(article);
        //                        }
        //                        prop.SetValue(entity, articles, null);
        //                    }
        //                    else if (genericArguments[0].Name == "Account")
        //                    {
        //                        List<CustomerServiceAccount> accounts = new List<CustomerServiceAccount>();
        //                        foreach (var item in root.Elements(propName))
        //                        {
        //                            var account = new CustomerServiceAccount();
        //                            FillEntityWithXml(account, new XDocument(item));
        //                            accounts.Add(account);
        //                        }
        //                        prop.SetValue(entity, accounts, null);
        //                    }
        //                    else if (genericArguments[0].Name == "PicItem")
        //                    {
        //                        List<PicItem> picItems = new List<PicItem>();
        //                        foreach (var item in root.Elements(propName).Elements("item"))
        //                        {
        //                            var picItem = new PicItem();
        //                            var picMd5Sum = item.Element("PicMd5Sum").Value;
        //                            Md5Sum md5Sum = new Md5Sum() { PicMd5Sum = picMd5Sum };
        //                            picItem.item = md5Sum;
        //                            picItems.Add(picItem);
        //                        }
        //                        prop.SetValue(entity, picItems, null);
        //                    }
        //                    else if (genericArguments[0].Name == "AroundBeacon")
        //                    {
        //                        List<AroundBeacon> aroundBeacons = new List<AroundBeacon>();
        //                        foreach (var item in root.Elements(propName).Elements("AroundBeacon"))
        //                        {
        //                            var aroundBeaconItem = new AroundBeacon();
        //                            FillEntityWithXml(aroundBeaconItem, new XDocument(item));
        //                            aroundBeacons.Add(aroundBeaconItem);
        //                        }
        //                        prop.SetValue(entity, aroundBeacons, null);
        //                    }
        //                    break;

        //                case "Music"://ResponseMessageMusic适用
        //                    Hishop.Weixin.MP.Domain.Music music = new Hishop.Weixin.MP.Domain.Music();
        //                    FillEntityWithXml(music, new XDocument(root.Element(propName)));
        //                    prop.SetValue(entity, music, null);
        //                    break;
        //                case "Image"://ResponseMessageImage适用
        //                    Hishop.Weixin.MP.Domain.Image image = new Hishop.Weixin.MP.Domain.Image();
        //                    FillEntityWithXml(image, new XDocument(root.Element(propName)));
        //                    prop.SetValue(entity, image, null);
        //                    break;
        //                case "Voice"://ResponseMessageVoice适用
        //                    Hishop.Weixin.MP.Domain.Voice voice = new Hishop.Weixin.MP.Domain.Voice();
        //                    FillEntityWithXml(voice, new XDocument(root.Element(propName)));
        //                    prop.SetValue(entity, voice, null);
        //                    break;
        //                case "Video"://ResponseMessageVideo适用
        //                    Hishop.Weixin.MP.Domain.Video video = new Hishop.Weixin.MP.Domain.Video();
        //                    FillEntityWithXml(video, new XDocument(root.Element(propName)));
        //                    prop.SetValue(entity, video, null);
        //                    break;
        //                case "ScanCodeInfo"://扫码事件中的ScanCodeInfo适用
        //                    ScanCodeInfo scanCodeInfo = new ScanCodeInfo();
        //                    FillEntityWithXml(scanCodeInfo, new XDocument(root.Element(propName)));
        //                    prop.SetValue(entity, scanCodeInfo, null);
        //                    break;
        //                case "SendLocationInfo"://弹出地理位置选择器的事件推送中的SendLocationInfo适用
        //                    SendLocationInfo sendLocationInfo = new SendLocationInfo();
        //                    FillEntityWithXml(sendLocationInfo, new XDocument(root.Element(propName)));
        //                    prop.SetValue(entity, sendLocationInfo, null);
        //                    break;
        //                case "SendPicsInfo"://系统拍照发图中的SendPicsInfo适用
        //                    SendPicsInfo sendPicsInfo = new SendPicsInfo();
        //                    FillEntityWithXml(sendPicsInfo, new XDocument(root.Element(propName)));
        //                    prop.SetValue(entity, sendPicsInfo, null);
        //                    break;
        //                case "ChosenBeacon"://摇一摇事件通知
        //                    ChosenBeacon chosenBeacon = new ChosenBeacon();
        //                    FillEntityWithXml(chosenBeacon, new XDocument(root.Element(propName)));
        //                    prop.SetValue(entity, chosenBeacon, null);
        //                    break;
        //                case "AroundBeacon"://摇一摇事件通知
        //                    AroundBeacon aroundBeacon = new AroundBeacon();
        //                    FillEntityWithXml(aroundBeacon, new XDocument(root.Element(propName)));
        //                    prop.SetValue(entity, aroundBeacon, null);
        //                    break;
        //                default:
        //                    prop.SetValue(entity, root.Element(propName).Value, null);
        //                    break;
        //            }
        //        }
        //    }
        //}
    }
}

