namespace Hishop.Weixin.MP.Request.Event
{
    using Hishop.Weixin.MP;
    using Hishop.Weixin.MP.Request;
    using System;
    using System.Runtime.CompilerServices;

    public class TemplateJobFinishEventRequest : EventRequest
    {
        public override RequestEventType Event
        {
            get
            {
                return RequestEventType.TEMPLATESENDJOBFINISH;
            }
            set
            {
            }
        }

        public string Status { get; set; }

        /// <summary>
        /// 消息id
        /// </summary>
        public long MsgID { get; set; }

        [Obsolete("请使用MsgID")]
        public new long MsgId { get; set; }
    }
}

