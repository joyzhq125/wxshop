namespace Hishop.Weixin.MP.Util
{
    using Hishop.Weixin.MP;
    using Hishop.Weixin.MP.Request;
    using Hishop.Weixin.MP.Request.Event;
    using System;
    using System.Text.RegularExpressions;
    using System.Xml.Linq;

    public static class RequestMessageFactory
    {
        public static AbstractRequest GetRequestEntity(XDocument doc)
        {
            RequestMsgType msgType = MsgTypeHelper.GetMsgType(doc);
            AbstractRequest entity = null;
            switch (msgType)
            {
                case RequestMsgType.Text:
                    entity = new TextRequest();
                    break;

                case RequestMsgType.Image:
                    entity = new ImageRequest();
                    break;

                case RequestMsgType.Voice:
                    entity = new VoiceRequest();
                    break;

                case RequestMsgType.Video:
                    entity = new VideoRequest();
                    break;

                case RequestMsgType.Location:
                    entity = new LocationRequest();
                    break;

                case RequestMsgType.Link:
                    entity = new LinkRequest();
                    break;

                case RequestMsgType.Event:
                    //switch (EventTypeHelper.GetEventType(doc))
                    switch (doc.Root.Element("Event").Value.ToUpper())
                    {
                        //case RequestEventType.Subscribe:
                        case "SUBSCRIBE"://订阅（关注）
                            entity = new SubscribeEventRequest();
                            goto Label_00D1;

                        //case RequestEventType.UnSubscribe:
                        case "UNSUBSCRIBE"://取消订阅（关注）
                            entity = new UnSubscribeEventRequest();
                            goto Label_00D1;

                        //case RequestEventType.Scan:
                        case "SCAN"://二维码扫描
                            entity = new ScanEventRequest();
                            goto Label_00D1;

                        //case RequestEventType.Location:
                        case "LOCATION"://地理位置
                            entity = new LocationEventRequest();
                            goto Label_00D1;

                        //case RequestEventType.Click:
                        case "CLICK"://菜单点击
                            entity = new ClickEventRequest();
                            goto Label_00D1;

                        //case RequestEventType.MASSSENDJOBFINISH:
                        case "MASSSENDJOBFINISH":
                            entity = new MassendJobFinishEventRequest();
                            goto Label_00D1;

                        case "VIEW":
                            entity = new ViewEventRequest();
                            goto Label_00D1;

                        //default://其他意外类型（也可以选择抛出异常）
                        //    entity = new AbstractRequest();
                        //    goto Label_00D1;

                    }
                    throw new ArgumentOutOfRangeException();

                default:
                    throw new ArgumentOutOfRangeException();
            }
        Label_00D1:
            //EntityHelper.FillEntityWithXml<AbstractRequest>(entity, doc);
            //修改无法解析完整数据 2017.7.4
            EntityHelper.FillEntityWithXml(entity, doc);
            new Regex(@"<MsgID>(?<url>\d+)</MsgID>");
            if (entity.MsgId == 0L)
            {
                Match match = Regex.Match(doc.Root.ToString(), @"<MsgID>(?<msgid>\d+)</MsgID>", RegexOptions.IgnoreCase);
                if (match.Success)
                {
                    entity.MsgId = long.Parse(match.Groups["msgid"].Value);
                    entity.CreateTime = DateTime.Now;
                }
            }
            return entity;
        }
    }
}

