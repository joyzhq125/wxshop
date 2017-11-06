namespace Hidistro.ControlPanel.WeiXin
{
    using Hidistro.Core.Entities;
    using Hidistro.Entities.WeiXin;
    using Hidistro.SqlDal.Weibo;
    using System;
    using System.Data;

    public class WeiXinHelper
    {
        public static string ClearWeiXinMediaID(string wid)
        {
            return new SendAllDao().ClearWeiXinMediaID(wid);
        }

        public static DataTable GetRencentOpenID(int topnum,string wid)
        {
            return new SendAllDao().GetRencentOpenID(topnum,wid);
        }

        public static SendAllInfo GetSendAllInfo(int sendID)
        {
            return new SendAllDao().GetSendAllInfo(sendID);
        }

        public static DbQueryResult GetSendAllRequest(SendAllQuery query)
        {
            return new SendAllDao().GetSendAllRequest(query);
        }

        public static string SaveSendAllInfo(SendAllInfo sendAllInfo)
        {
            return new SendAllDao().SaveSendAllInfo(sendAllInfo);
        }

        public static bool UpdateMsgId(int id, string msgid, int sendstate, int sendcount, int totalcount, string returnjsondata)
        {
            return new SendAllDao().UpdateMsgId(id, msgid, sendstate, sendcount, totalcount, returnjsondata);
        }

        public static int UpdateRencentOpenID(string openid,string wid)
        {
            return new SendAllDao().UpdateRencentOpenID(openid,wid);
        }
    }
}

