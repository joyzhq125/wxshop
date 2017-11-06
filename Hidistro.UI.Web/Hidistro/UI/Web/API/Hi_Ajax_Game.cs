namespace Hidistro.UI.Web.API
{
    using Hidistro.ControlPanel.Promotions;
    using Hidistro.ControlPanel.Promotions;
    using Hidistro.Entities.Promotions;
    using Hidistro.SaleSystem.Vshop;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Web;

    public class Hi_Ajax_Game : IHttpHandler
    {
        private void CheckCanVote(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            StringBuilder builder = new StringBuilder("{");
            int voteId = 0;
            try
            {
                voteId = int.Parse(context.Request["voteId"]);
            }
            catch (Exception)
            {
                builder.Append("\"status\":\"0\",\"Desciption\":\"参数错误!\"}");
                context.Response.Write(builder.ToString());
                return;
            }
            if (VoteHelper.IsVote(voteId))
            {
                builder.Append("\"status\":\"2\",\"Desciption\":\"已投过票!\"}");
                context.Response.Write(builder.ToString());
            }
            else
            {
                builder.Append("\"status\":\"1\",\"Desciption\":\"可以投票!\"}");
                context.Response.Write(builder.ToString());
            }
        }

        private void CheckUserCanPlay(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            StringBuilder builder = new StringBuilder("{");
            string str = context.Request["gameid"];
            if (string.IsNullOrEmpty(str))
            {
                builder.Append("\"status\":\"0\",\"Desciption\":\"参数错误!\"}");
                context.Response.Write(builder.ToString());
            }
            else
            {
                GameInfo modelByGameId = GameHelper.GetModelByGameId(str);
                if (modelByGameId == null)
                {
                    builder.Append("\"status\":\"0\",\"Desciption\":\"游戏不存在!\"}");
                    context.Response.Write(builder.ToString());
                }
                else
                {
                    int userid = 0;
                    try
                    {
                        userid = MemberProcessor.GetCurrentMember().UserId;
                    }
                    catch (Exception)
                    {
                        userid = 1;
                    }
                    try
                    {
                        if (!GameHelper.IsCanPrize(modelByGameId.GameId, userid))
                        {
                            throw new Exception("不能再玩！");
                        }
                        builder.Append("\"status\":\"1\",\"Desciption\":\"可以正常玩!\"}");
                        context.Response.Write(builder.ToString());
                    }
                    catch (Exception exception)
                    {
                        builder.Append("\"status\":\"0\",\"Desciption\":\"" + exception.Message + "!\"}");
                        context.Response.Write(builder.ToString());
                    }
                }
            }
        }

        private void GetCouponToMember(HttpContext context)
        {
            StringBuilder builder = new StringBuilder("{");
            int couponId = 0;
            try
            {
                couponId = int.Parse(context.Request["couponId"]);
            }
            catch (Exception)
            {
                builder.Append("\"status\":\"0\",\"Desciption\":\"参数错误!\"}");
                context.Response.Write(builder.ToString());
                return;
            }
            int userId = 0;
            try
            {
                userId = MemberProcessor.GetCurrentMember().UserId;
            }
            catch (Exception)
            {
                userId = 1;
            }
            try
            {
                SendCouponResult result = CouponHelper.SendCouponToMember(couponId, userId);
                switch (result)
                {
                    case SendCouponResult.正常领取:
                        builder.Append("\"status\":\"1\",\"Desciption\":\"领取成功!\"}");
                        context.Response.Write(builder.ToString());
                        return;

                    case SendCouponResult.其它错误:
                        throw new Exception();
                }
                builder.Append("\"status\":\"2\",\"Desciption\":\"" + result.ToString() + "!\"}");
                context.Response.Write(builder.ToString());
            }
            catch (Exception)
            {
                builder.Append("\"status\":\"3\",\"Desciption\":\"领取失败!\"}");
                context.Response.Write(builder.ToString());
            }
        }

        private void GetPrize(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            StringBuilder builder = new StringBuilder("{");
            string str = context.Request["gameid"];
            if (string.IsNullOrEmpty(str))
            {
                builder.Append("\"status\":\"0\",\"Desciption\":\"参数错误!\"}");
                context.Response.Write(builder.ToString());
            }
            else
            {
                GameInfo modelByGameId = GameHelper.GetModelByGameId(str);
                if (DateTime.Now < modelByGameId.BeginTime)
                {
                    builder.Append("\"status\":\"0\",\"Desciption\":\"活动还没开始!\"}");
                    context.Response.Write(builder.ToString());
                }
                else if ((DateTime.Now > modelByGameId.EndTime) || (modelByGameId.Status == GameStatus.结束))
                {
                    builder.Append("\"status\":\"0\",\"Desciption\":\"活动已结束!\"}");
                    context.Response.Write(builder.ToString());
                }
                else
                {
                    int userid = 0;
                    try
                    {
                        userid = MemberProcessor.GetCurrentMember().UserId;
                    }
                    catch (Exception)
                    {
                        userid = 1;
                    }
                    try
                    {
                        GameHelper.IsCanPrize(modelByGameId.GameId, userid);
                    }
                    catch (Exception exception)
                    {
                        builder.Append("\"status\":\"0\",\"Desciption\":\"" + exception.Message + "!\"}");
                        context.Response.Write(builder.ToString());
                        return;
                    }
                    GamePrizeInfo info3 = GameHelper.UserPrize(modelByGameId.GameId, userid);
                    builder.Append(string.Concat(new object[] { "\"status\":\"1\",\"Desciption\":\"\",\"prizeType\":\"", info3.PrizeGrade, "\",\"prizeIndex\":\"", this.GetPrizeIndex(info3.PrizeGrade), "\"}" }));
                    context.Response.Write(builder.ToString());
                }
            }
        }

        private int GetPrizeIndex(PrizeGrade prizeGrade)
        {
            int num = (int) prizeGrade;
            if (prizeGrade != PrizeGrade.未中奖)
            {
                Random random = new Random();
                if ((random.Next(1, 10) % 2) == 0)
                {
                    return (num + 5);
                }
            }
            return num;
        }

        private void GetPrizeLists(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            StringBuilder builder = new StringBuilder("{");
            string str = context.Request["gameid"];
            if (string.IsNullOrEmpty(str))
            {
                builder.Append("\"status\":\"0\",\"Desciption\":\"参数错误!\"}");
                context.Response.Write(builder.ToString());
            }
            else
            {
                GameInfo modelByGameId = GameHelper.GetModelByGameId(str);
                if (modelByGameId == null)
                {
                    builder.Append("\"status\":\"0\",\"Desciption\":\"游戏不存在!\"}");
                    context.Response.Write(builder.ToString());
                }
                else
                {
                    GameData data = new GameData {
                        status = 1,
                        Description = modelByGameId.Description,
                        BeginDate = modelByGameId.BeginTime,
                        EndDate = modelByGameId.EndTime
                    };
                    IList<GamePrizeInfo> gamePrizeListsByGameId = GameHelper.GetGamePrizeListsByGameId(modelByGameId.GameId);
                    List<PrizeData> list2 = new List<PrizeData>();
                    GamePrizeInfo info2 = gamePrizeListsByGameId.FirstOrDefault<GamePrizeInfo>(p => p.PrizeGrade == PrizeGrade.一等奖);
                    PrizeData item = new PrizeData {
                        prizeType = info2.PrizeGrade.ToString()
                    };
                    PrizeResultViewInfo info6 = new PrizeResultViewInfo {
                        PrizeType = info2.PrizeType,
                        GivePoint = info2.GivePoint,
                        GiveCouponId = info2.GiveCouponId,
                        GiveShopBookId = info2.GiveShopBookId
                    };
                    item.prizeName = GameHelper.GetPrizeName(info6);
                    list2.Add(item);
                    GamePrizeInfo info3 = gamePrizeListsByGameId.FirstOrDefault<GamePrizeInfo>(p => p.PrizeGrade == PrizeGrade.二等奖);
                    PrizeData data4 = new PrizeData {
                        prizeType = info3.PrizeGrade.ToString()
                    };
                    PrizeResultViewInfo info7 = new PrizeResultViewInfo {
                        PrizeType = info3.PrizeType,
                        GivePoint = info3.GivePoint,
                        GiveCouponId = info3.GiveCouponId,
                        GiveShopBookId = info3.GiveShopBookId
                    };
                    data4.prizeName = GameHelper.GetPrizeName(info7);
                    list2.Add(data4);
                    GamePrizeInfo info4 = gamePrizeListsByGameId.FirstOrDefault<GamePrizeInfo>(p => p.PrizeGrade == PrizeGrade.三等奖);
                    PrizeData data5 = new PrizeData {
                        prizeType = info4.PrizeGrade.ToString()
                    };
                    PrizeResultViewInfo info8 = new PrizeResultViewInfo {
                        PrizeType = info4.PrizeType,
                        GivePoint = info4.GivePoint,
                        GiveCouponId = info4.GiveCouponId,
                        GiveShopBookId = info4.GiveShopBookId
                    };
                    data5.prizeName = GameHelper.GetPrizeName(info8);
                    list2.Add(data5);
                    GamePrizeInfo info5 = gamePrizeListsByGameId.FirstOrDefault<GamePrizeInfo>(p => p.PrizeGrade == PrizeGrade.普通奖);
                    PrizeData data6 = new PrizeData {
                        prizeType = info5.PrizeGrade.ToString()
                    };
                    PrizeResultViewInfo info9 = new PrizeResultViewInfo {
                        PrizeType = info5.PrizeType,
                        GivePoint = info5.GivePoint,
                        GiveCouponId = info5.GiveCouponId,
                        GiveShopBookId = info5.GiveShopBookId
                    };
                    data6.prizeName = GameHelper.GetPrizeName(info9);
                    list2.Add(data6);
                    PrizeData data7 = new PrizeData {
                        prizeType = PrizeGrade.未中奖.ToString(),
                        prizeName = modelByGameId.NotPrzeDescription
                    };
                    list2.Add(data7);
                    data.prizeLists = list2;
                    IsoDateTimeConverter converter = new IsoDateTimeConverter();
                    converter.DateTimeFormat="yyyy-MM-dd HH:mm:ss";
                    context.Response.Write(JsonConvert.SerializeObject(data,Formatting.Indented, new JsonConverter[] { converter }));
                }
            }
        }

        private string GetUserName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return "";
            }
            int length = name.Length;
            string str = name.Substring(0, 1) + "**";
            if (length <= 3)
            {
                return str;
            }
            return (str + name.Substring(length - 1));
        }

        private void GetUserPrizeLists(HttpContext context)
        {
            StringBuilder builder = new StringBuilder("{");
            string str = context.Request["gameid"];
            if (string.IsNullOrEmpty(str))
            {
                builder.Append("\"status\":\"0\",\"Desciption\":\"参数错误!\"}");
                context.Response.Write(builder.ToString());
            }
            else
            {
                int pageIndex = 1;
                int pageSize = 7;
                try
                {
                    pageIndex = int.Parse(context.Request["pageIndex"]);
                }
                catch (Exception)
                {
                    builder.Append("\"status\":\"0\",\"Desciption\":\"参数错误!\"}");
                    context.Response.Write(builder.ToString());
                    return;
                }
                try
                {
                    pageSize = int.Parse(context.Request["pageSize"]);
                }
                catch (Exception)
                {
                    builder.Append("\"status\":\"0\",\"Desciption\":\"参数错误!\"}");
                    context.Response.Write(builder.ToString());
                    return;
                }
                IList<PrizeResultViewInfo> list = GameHelper.GetPrizeLogLists(GameHelper.GetModelByGameId(str).GameId, pageIndex, pageSize);
                builder.Append("\"lists\":[");
                int count = list.Count;
                if (count > 0)
                {
                    for (int i = 0; i < count; i++)
                    {
                        PrizeResultViewInfo item = list[i];
                        builder.Append("{\"PrizeGrade\":\"" + item.PrizeGrade.ToString() + "\",\"UserName\":\"" + this.GetUserName(item.UserName) + "\",\"PrizeName\":\"" + GameHelper.GetPrizeName(item) + "\",\"DateTime\":\"" + item.PlayTime.ToString("yyyy-MM-dd") + "\"}");
                        if (i != (count - 1))
                        {
                            builder.Append(",");
                        }
                    }
                }
                builder.Append("]}");
                context.Response.Write(builder);
            }
        }

        public void ProcessRequest(HttpContext context)
        {
            string str = context.Request["action"];
            if (string.IsNullOrEmpty(str))
            {
                str = str.ToLower();
            }
            switch (str)
            {
                case "getprizelists":
                    this.GetPrizeLists(context);
                    return;

                case "checkusercanplay":
                    this.CheckUserCanPlay(context);
                    return;

                case "getprizeinfo":
                    this.GetPrize(context);
                    return;

                case "getuserprizelists":
                    this.GetUserPrizeLists(context);
                    return;

                case "checkcanvote":
                    this.CheckCanVote(context);
                    return;

                case "uservote":
                    this.UserVote(context);
                    return;

                case "getcoupon":
                    this.GetCouponToMember(context);
                    return;
            }
        }

        private void UserVote(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            StringBuilder builder = new StringBuilder("{");
            int voteId = 0;
            try
            {
                voteId = int.Parse(context.Request["voteId"]);
            }
            catch (Exception)
            {
                builder.Append("\"status\":\"0\",\"Desciption\":\"参数错误!\"}");
                context.Response.Write(builder.ToString());
                return;
            }
            string str = context.Request["voteItem"];
            if (!string.IsNullOrEmpty(str))
            {
                try
                {
                    if (!VoteHelper.Vote(voteId, str))
                    {
                        throw new Exception("投票失败！");
                    }
                    builder.Append("\"status\":\"1\",\"Desciption\":\"成功!\"}");
                    context.Response.Write(builder.ToString());
                }
                catch (Exception exception)
                {
                    builder.Append("\"status\":\"2\",\"Desciption\":\"" + exception.Message + "\"}");
                    context.Response.Write(builder.ToString());
                }
            }
            else
            {
                builder.Append("\"status\":\"0\",\"Desciption\":\"参数错误!\"}");
                context.Response.Write(builder.ToString());
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        public class GameData
        {
            public DateTime BeginDate { get; set; }

            public string Description { get; set; }

            public DateTime EndDate { get; set; }

            public IList<Hi_Ajax_Game.PrizeData> prizeLists { get; set; }

            public int status { get; set; }
        }

        public class PrizeData
        {
            public string prizeName { get; set; }

            public string prizeType { get; set; }
        }
    }
}

