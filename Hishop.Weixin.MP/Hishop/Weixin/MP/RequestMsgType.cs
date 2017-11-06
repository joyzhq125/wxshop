namespace Hishop.Weixin.MP
{
    using System;

    //public enum RequestMsgType
    //{
    //    Text,
    //    Image,
    //    Voice,
    //    Video,
    //    Location,
    //    Link,
    //    Event,
    //    transfer_customer_service
    //}


    public enum RequestMsgType
    {
        Text, //文本
        Location, //地理位置
        Image, //图片
        Voice, //语音
        Video, //视频
        Link, //连接信息
        ShortVideo,//小视频
        Event, //事件推送
    }



}

