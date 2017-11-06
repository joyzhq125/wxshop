namespace Hidistro.ControlPanel.Promotions
{
    using Hidistro.ControlPanel.Commodities;
    using Hidistro.ControlPanel.Promotions;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Promotions;
    using Hidistro.SqlDal.Promotions;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Runtime.InteropServices;

    public static class GameHelper
    {
        private static Random Rnd = new Random();

        public static bool AddPrizeLog(PrizeResultInfo model)
        {
            if (model == null)
            {
                throw new ArgumentNullException("参数model不能不null");
            }
            return new PrizeResultDao().AddPrizeLog(model);
        }

        private static GamePrizeInfo ChouJiang(IList<GamePrizeInfo> prizeLists)
        {
            return (from x in Enumerable.Range(0, 0x2710)
                let prizeInfo0 = prizeLists[Rnd.Next(prizeLists.Count<GamePrizeInfo>())]
                let rnd = Rnd.Next(0, 100)
                where rnd < prizeInfo0.PrizeRate
                select prizeInfo0).First<GamePrizeInfo>();
        }

        public static bool Create(GameInfo model, out int gameId)
        {
            if (model == null)
            {
                throw new ArgumentNullException("model参数不能为null");
            }
            Globals.EntityCoding(model, true);
            return new GameDao().Create(model, out gameId);
        }

        public static bool CreatePrize(GamePrizeInfo model)
        {
            if (model == null)
            {
                throw new ArgumentNullException("model参数不能为null");
            }
            return new GamePrizeDao().Create(model);
        }

        public static bool Delete(params int[] gameIds)
        {
            if ((gameIds == null) || (gameIds.Count<int>() <= 0))
            {
                throw new ArgumentNullException("参数gameIds不能为空！");
            }
            return new GameDao().Delete(gameIds);
        }

        public static bool DeletePrizesDelivery(int[] ids)
        {
            return new GameDao().DeletePrizesDelivery(ids);
        }

        public static DbQueryResult GetGameList(GameSearch search)
        {
            return new GameDao().GetGameList(search);
        }

        public static DbQueryResult GetGameListByView(GameSearch search)
        {
            return new GameDao().GetGameListByView(search);
        }

        public static IList<GamePrizeInfo> GetGamePrizeListsByGameId(int gameId)
        {
            if (gameId <= 0)
            {
                return null;
            }
            return new GamePrizeDao().GetGamePrizeListsByGameId(gameId);
        }

        public static string GetGameTypeName(string GameType)
        {
            return Enum.GetName(typeof(GameType), int.Parse(GameType));
        }

        public static IEnumerable<GameInfo> GetLists(GameSearch search)
        {
            return new GameDao().GetLists(search);
        }

        public static GameInfo GetModelByGameId(int gameId)
        {
            GameInfo modelByGameId = new GameDao().GetModelByGameId(gameId);
            Globals.EntityCoding(modelByGameId, false);
            return modelByGameId;
        }

        public static GameInfo GetModelByGameId(string keyWord)
        {
            GameInfo modelByGameId = new GameDao().GetModelByGameId(keyWord);
            Globals.EntityCoding(modelByGameId, false);
            return modelByGameId;
        }

        public static GamePrizeInfo GetModelByPrizeGradeAndGameId(PrizeGrade grade, int gameId)
        {
            return new GamePrizeDao().GetModelByPrizeGradeAndGameId(grade, gameId);
        }

        public static string GetPrizeGradeName(string PrizeGrade)
        {
            return Enum.GetName(typeof(PrizeGrade), int.Parse(PrizeGrade));
        }

        public static DbQueryResult GetPrizeLogLists(PrizesDeliveQuery query)
        {
            return new PrizeResultDao().GetPrizeLogLists(query);
        }

        public static IList<PrizeResultViewInfo> GetPrizeLogLists(int gameId, int pageIndex, int pageSize)
        {
            return new PrizeResultDao().GetPrizeLogLists(gameId, pageIndex, pageSize);
        }

        public static string GetPrizeName(PrizeResultViewInfo item)
        {
            switch (item.PrizeType)
            {
                case PrizeType.赠送积分:
                    return string.Format("{0} 积分", item.GivePoint);

                case PrizeType.赠送优惠劵:
                    return Globals.SubStr(CouponHelper.GetCoupon(int.Parse(item.GiveCouponId)).CouponName, 12, "..");

                case PrizeType.赠送商品:
                    return Globals.SubStr(ProductHelper.GetProductBaseInfo(int.Parse(item.GiveShopBookId)).ProductName, 12, "..");
            }
            return "";
        }

        public static DbQueryResult GetPrizesDeliveryList(PrizesDeliveQuery query, string ExtendLimits = "", string selectFields = "*")
        {
            return new GameDao().GetPrizesDeliveryList(query, ExtendLimits, selectFields);
        }

        public static DataTable GetPrizesDeliveryNum()
        {
            return new GameDao().GetPrizesDeliveryNum();
        }

        public static string GetPrizesDeliveStatus(string status)
        {
            string str = "未定义";
            string str2 = status;
            if (str2 == null)
            {
                return str;
            }
            if (!(str2 == "0"))
            {
                if (str2 != "1")
                {
                    if (str2 == "2")
                    {
                        return "已发货";
                    }
                    if (str2 != "3")
                    {
                        return str;
                    }
                    return "已收货";
                }
            }
            else
            {
                return "待填写收货地址";
            }
            return "待发货";
        }

        public static bool IsCanPrize(int gameId, int userid)
        {
            return new PrizeResultDao().IsCanPrize(gameId, userid);
        }

        public static bool Update(GameInfo model)
        {
            if (model == null)
            {
                throw new ArgumentNullException("model参数不能为null");
            }
            Globals.EntityCoding(model, true);
            return new GameDao().Update(model);
        }

        public static bool UpdatePrize(GamePrizeInfo model)
        {
            if (model == null)
            {
                throw new ArgumentNullException("model参数不能为null");
            }
            return new GamePrizeDao().Update(model);
        }

        public static bool UpdatePrizeLogStatus(int logId)
        {
            return new PrizeResultDao().UpdatePrizeLogStatus(logId);
        }

        public static bool UpdatePrizesDelivery(PrizesDeliveQuery query)
        {
            return new GameDao().UpdatePrizesDelivery(query);
        }

        public static bool UpdateStatus(int gameId, GameStatus status)
        {
            return new GameDao().UpdateStatus(gameId, status);
        }

        public static GamePrizeInfo UserPrize(int gameId, int useId)
        {
            IList<GamePrizeInfo> gamePrizeListsByGameId = GetGamePrizeListsByGameId(gameId);
            int num = gamePrizeListsByGameId.Max<GamePrizeInfo>((Func<GamePrizeInfo, int>) (p => p.PrizeRate));
            GamePrizeInfo item = new GamePrizeInfo {
                PrizeId = 0,
                PrizeRate = (num >= 100) ? 0 : 100,
                PrizeGrade = PrizeGrade.未中奖
            };
            gamePrizeListsByGameId.Add(item);
            GamePrizeInfo info2 = ChouJiang(gamePrizeListsByGameId);
            if ((info2.PrizeId != 0) && (info2.PrizeCount <= 0))
            {
                info2 = item;
            }
            if (((info2.PrizeId != 0) && (info2.PrizeType == PrizeType.赠送优惠劵)) && (CouponHelper.IsCanSendCouponToMember(int.Parse(info2.GiveCouponId), useId) != SendCouponResult.正常领取))
            {
                info2 = item;
            }
            PrizeResultInfo model = new PrizeResultInfo {
                GameId = gameId,
                PrizeId = info2.PrizeId,
                UserId = useId
            };
            new PrizeResultDao().AddPrizeLog(model);
            return info2;
        }
    }
}

