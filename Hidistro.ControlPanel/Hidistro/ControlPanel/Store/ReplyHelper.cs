namespace Hidistro.ControlPanel.Store
{
    using Hidistro.Entities.VShop;
    using Hidistro.SqlDal.VShop;
    using System;
    using System.Collections.Generic;

    public class ReplyHelper
    {
        public static void DeleteNewsMsg(int id)
        {
            new ReplyDao().DeleteNewsMsg(id);
        }

        public static bool DeleteReply(int id)
        {
            return new ReplyDao().DeleteReply(id);
        }

        public static IList<ReplyInfo> GetAllReply(string wid)
        {
            return new ReplyDao().GetAllReply(wid);
        }

        public static int GetArticleIDByOldArticle(int replyid, MessageType msgtype)
        {
            return new ReplyDao().GetArticleIDByOldArticle(replyid, msgtype);
        }

        public static ReplyInfo GetMismatchReply(string wid)
        {
            IList<ReplyInfo> replies = new ReplyDao().GetReplies(ReplyType.NoMatch,wid);
            if ((replies != null) && (replies.Count > 0))
            {
                return replies[0];
            }
            return null;
        }

        public static int GetNoMatchReplyID(int compareid,string wid)
        {
            return new ReplyDao().GetNoMatchReplyID(compareid,wid);
        }

        public static IList<ReplyInfo> GetReplies(ReplyType type,string wid)
        {
            return new ReplyDao().GetReplies(type,wid);
        }

        public static ReplyInfo GetReply(int id)
        {
            return new ReplyDao().GetReply(id);
        }

        public static int GetSubscribeID(int compareid,string wid)
        {
            return new ReplyDao().GetSubscribeID(compareid,wid);
        }

        public static ReplyInfo GetSubscribeReply(string wid)
        {
            IList<ReplyInfo> replies = new ReplyDao().GetReplies(ReplyType.Subscribe,wid);
            if ((replies != null) && (replies.Count > 0))
            {
                return replies[0];
            }
            return null;
        }

        public static bool HasReplyKey(string key,string wid)
        {
            return new ReplyDao().HasReplyKey(key,wid);
        }

        public static bool HasReplyKey(string key, int replyid,string wid)
        {
            return new ReplyDao().HasReplyKey(key, replyid,wid);
        }

        public static bool SaveReply(ReplyInfo reply)
        {
            reply.LastEditDate = DateTime.Now;
            reply.LastEditor = ManagerHelper.GetCurrentManager().UserName;
            return new ReplyDao().SaveReply(reply);
        }

        public static bool UpdateReply(ReplyInfo reply)
        {
            reply.LastEditDate = DateTime.Now;
            reply.LastEditor = ManagerHelper.GetCurrentManager().UserName;
            return new ReplyDao().UpdateReply(reply);
        }

        public static bool UpdateReplyRelease(int id)
        {
            return new ReplyDao().UpdateReplyRelease(id);
        }
    }
}

