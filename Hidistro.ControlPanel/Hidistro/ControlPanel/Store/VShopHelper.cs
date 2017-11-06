namespace Hidistro.ControlPanel.Store
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Members;
    using Hidistro.Entities.Orders;
    using Hidistro.Entities.VShop;
    using Hidistro.SqlDal.Commodities;
    using Hidistro.SqlDal.Members;
    using Hidistro.SqlDal.VShop;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Web;

    public static class VShopHelper
    {
        private const string CacheKey = "Message-{0}";

        public static int AddActivities(ActivitiesInfo activity)
        {
            return new ActivitiesDao().AddActivities(activity);
        }

        public static bool AddHomeProdcut(int productId)
        {
            return new HomeProductDao().AddHomeProdcut(productId);
        }

        public static bool AddHomeTopic(int topicId)
        {
            return new HomeTopicDao().AddHomeTopic(topicId);
        }

        public static bool AddReleatesProdcutBytopicid(int topicid, int productId)
        {
            return new TopicDao().AddReleatesProdcutBytopicid(topicid, productId);
        }

        public static bool CanAddMenu(int parentId,string wid)
        {
            IList<MenuInfo> menusByParentId = new MenuDao().GetMenusByParentId(parentId,wid);
            if ((menusByParentId == null) || (menusByParentId.Count == 0))
            {
                return true;
            }
            if (parentId == 0)
            {
                return (menusByParentId.Count < 3);
            }
            return (menusByParentId.Count < 5);
        }

        public static bool Createtopic(TopicInfo topic, out int id)
        {
            id = 0;
            if (topic == null)
            {
                return false;
            }
            Globals.EntityCoding(topic, true);
            id = new TopicDao().AddTopic(topic);
            ReplyInfo reply = new TextReplyInfo {
                Keys = topic.Keys,
                MatchType = MatchType.Equal,
                MessageType = MessageType.Text,
                ReplyType = ReplyType.Topic,
                ActivityId = id
            };
            return new ReplyDao().SaveReply(reply);
        }

        public static bool DeleteActivities(int ActivitiesId)
        {
            return new ActivitiesDao().DeleteActivities(ActivitiesId);
        }

        public static bool DeleteActivity(int activityId)
        {
            return new ActivityDao().DeleteActivity(activityId);
        }

        public static bool DeleteAlarm(int id)
        {
            return new AlarmDao().Delete(id);
        }

        public static bool DeleteFeedBack(int id)
        {
            return new FeedBackDao().Delete(id);
        }

        public static bool DeleteLotteryActivity(int activityid, string type = "")
        {
            return new LotteryActivityDao().DelteLotteryActivity(activityid, type);
        }

        public static bool DeleteMenu(int menuId)
        {
            return new MenuDao().DeleteMenu(menuId);
        }

        public static bool Deletetopic(int topicId)
        {
            return new TopicDao().DeleteTopic(topicId);
        }

        public static int Deletetopics(IList<int> topics)
        {
            if ((topics != null) && (topics.Count != 0))
            {
                return new TopicDao().DeleteTopics(topics);
            }
            return 0;
        }

        public static bool DelteLotteryTicket(int activityId)
        {
            return new LotteryActivityDao().DelteLotteryTicket(activityId);
        }

        public static bool DelTplCfg(int id)
        {
            return new BannerDao().DelTplCfg(id);
        }

        public static IList<ActivitiesInfo> GetActivitiesInfo(string ActivitiesId)
        {
            return new ActivitiesDao().GetActivitiesInfo(ActivitiesId);
        }

        public static DbQueryResult GetActivitiesList(ActivitiesQuery query)
        {
            return new ActivitiesDao().GetActivitiesList(query);
        }

        public static ActivityInfo GetActivity(int activityId)
        {
            return new ActivityDao().GetActivity(activityId);
        }

        public static IList<ActivitySignUpInfo> GetActivitySignUpById(int activityId)
        {
            return new ActivitySignUpDao().GetActivitySignUpById(activityId);
        }

        public static DbQueryResult GetAlarms(int pageIndex, int pageSize)
        {
            return new AlarmDao().List(pageIndex, pageSize);
        }

        public static IList<ActivityInfo> GetAllActivity()
        {
            return new ActivityDao().GetAllActivity();
        }

        public static IList<BannerInfo> GetAllBanners()
        {
            return new BannerDao().GetAllBanners();
        }

        public static IList<NavigateInfo> GetAllNavigate()
        {
            return new BannerDao().GetAllNavigate();
        }

        public static DbQueryResult GetBalanceDrawRequest(BalanceDrawRequestQuery query)
        {
            return new DistributorsDao().GetBalanceDrawRequest(query, null);
        }

        public static string GetCommissionPayStatus(string ischeck)
        {
            string str = "未定义";
            string str2 = ischeck;
            if (str2 == null)
            {
                return str;
            }
            if (!(str2 == "0"))
            {
                if (str2 != "1")
                {
                    if (str2 != "2")
                    {
                        return str;
                    }
                    return "已支付";
                }
            }
            else
            {
                return "未审核";
            }
            return "已审核";
        }

        public static string GetCommissionPayType(string payType)
        {
            string str = "未定义";
            string str2 = payType;
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
                        return "网银支付";
                    }
                    if (str2 != "3")
                    {
                        return str;
                    }
                    return "微信红包";
                }
            }
            else
            {
                return "微信钱包";
            }
            return "支付宝";
        }

        public static DbQueryResult GetCommissions(CommissionsQuery query)
        {
            return new DistributorsDao().GetCommissions(query);
        }

        public static DbQueryResult GetCommissionsWithStoreName(CommissionsQuery query)
        {
            return new DistributorsDao().GetCommissionsWithStoreName(query);
        }

        public static int GetCountBanner()
        {
            return new BannerDao().GetCountBanner();
        }

        public static IList<DistributorGradeInfo> GetDistributorGradeInfos()
        {
            return new DistributorGradeDao().GetDistributorGradeInfos();
        }

        public static DbQueryResult GetDistributors(DistributorsQuery query, string topUserId = null, string level = null)
        {
            return new DistributorsDao().GetDistributors(query, topUserId, level);
        }

        public static DataTable GetDistributorSaleinfo(string startTime, string endTime, int[] UserIds)
        {
            return new DistributorsDao().GetDistributorSaleinfo(startTime, endTime, UserIds);
        }

        public static DataTable GetDistributorsNum()
        {
            return new DistributorsDao().GetDistributorsNum();
        }

        public static DbQueryResult GetDistributorsRankings(string startTime, string endTime, int pgSize, int CurrPage)
        {
            return new DistributorsDao().GetDistributorsRankings(startTime, endTime, pgSize, CurrPage);
        }

        public static DataTable GetDistributorsSubStoreNum(int topUserId)
        {
            return new DistributorsDao().GetDistributorsSubStoreNum(topUserId);
        }

        public static int GetDistributorsSubStoreNumN(int topUserId, int grade, string startTime, string endTime)
        {
            return new DistributorsDao().GetDistributorsSubStoreNumN(topUserId, grade, startTime, endTime);
        }

        public static int GetDownDistributorNum(string userid)
        {
            return new DistributorsDao().GetDownDistributorNum(userid);
        }

        public static int GetDownDistributorNumReferralOrders(string userid)
        {
            return new DistributorsDao().GetDownDistributorNumReferralOrders(userid);
        }

        public static FeedBackInfo GetFeedBack(int id)
        {
            return new FeedBackDao().Get(id);
        }

        public static FeedBackInfo GetFeedBack(string feedBackID)
        {
            return new FeedBackDao().Get(feedBackID);
        }

        public static DbQueryResult GetFeedBacks(int pageIndex, int pageSize, string msgType)
        {
            return new FeedBackDao().List(pageIndex, pageSize, msgType);
        }

        public static DataTable GetHomeProducts()
        {
            return new HomeProductDao().GetHomeProducts();
        }

        public static DataTable GetHomeTopics()
        {
            return new HomeTopicDao().GetHomeTopics();
        }

        public static IList<MenuInfo> GetInitMenus(string wid)
        {
            MenuDao dao = new MenuDao();
            IList<MenuInfo> topMenus = dao.GetTopMenus(wid);
            foreach (MenuInfo info in topMenus)
            {
                info.Chilren = dao.GetMenusByParentId(info.MenuId,wid);
                if (info.Chilren == null)
                {
                    info.Chilren = new List<MenuInfo>();
                }
            }
            return topMenus;
        }

        public static IList<LotteryActivityInfo> GetLotteryActivityByType(LotteryActivityType type)
        {
            return new LotteryActivityDao().GetLotteryActivityByType(type);
        }

        public static LotteryActivityInfo GetLotteryActivityInfo(int activityid)
        {
            LotteryActivityInfo lotteryActivityInfo = new LotteryActivityDao().GetLotteryActivityInfo(activityid);
            lotteryActivityInfo.PrizeSettingList = JsonConvert.DeserializeObject<List<PrizeSetting>>(lotteryActivityInfo.PrizeSetting);
            return lotteryActivityInfo;
        }

        public static DbQueryResult GetLotteryActivityList(LotteryActivityQuery page)
        {
            return new LotteryActivityDao().GetLotteryActivityList(page);
        }

        public static LotteryTicketInfo GetLotteryTicket(int activityid)
        {
            LotteryTicketInfo lotteryTicket = new LotteryActivityDao().GetLotteryTicket(activityid);
            lotteryTicket.PrizeSettingList = JsonConvert.DeserializeObject<List<PrizeSetting>>(lotteryTicket.PrizeSetting);
            return lotteryTicket;
        }

        public static DbQueryResult GetLotteryTicketList(LotteryActivityQuery page)
        {
            return new LotteryActivityDao().GetLotteryTicketList(page);
        }

        public static MenuInfo GetMenu(int menuId,string wid)
        {
            return new MenuDao().GetMenu(menuId,wid);
        }

        public static IList<MenuInfo> GetMenus(string wid)
        {
            IList<MenuInfo> list = new List<MenuInfo>();
            MenuDao dao = new MenuDao();
            IList<MenuInfo> topMenus = dao.GetTopMenus(wid);
            if (topMenus != null)
            {
                foreach (MenuInfo info in topMenus)
                {
                    list.Add(info);
                    IList<MenuInfo> menusByParentId = dao.GetMenusByParentId(info.MenuId,wid);
                    if (menusByParentId != null)
                    {
                        foreach (MenuInfo info2 in menusByParentId)
                        {
                            list.Add(info2);
                        }
                    }
                }
            }
            return list;
        }

        public static IList<MenuInfo> GetMenusByParentId(int parentId,string wid)
        {
            return new MenuDao().GetMenusByParentId(parentId,wid);
        }

        public static int IsExistMessageTemplate(string wid)
        {
            return new MessageTemplateHelperDao().IsExistSettings(wid);
        }

        /// <summary>
        /// 增加默认wid模板
        /// </summary>
        /// <param name="wid"></param>
        /// <returns></returns>
        public static bool AddMessageTemplate(string wid)
        {
            return new MessageTemplateHelperDao().AddSettings(wid);
        }

        public static MessageTemplate GetMessageTemplate(string messageType,string wid)
        {
            if (string.IsNullOrEmpty(messageType))
            {
                return null;
            }
            return new MessageTemplateHelperDao().GetMessageTemplate(messageType,wid);
        }

        public static IList<MessageTemplate> GetMessageTemplates(string wid)
        {
            return new MessageTemplateHelperDao().GetMessageTemplates(wid);
        }

        public static List<PrizeRecordInfo> GetPrizeList(PrizeQuery page)
        {
            return new LotteryActivityDao().GetPrizeList(page);
        }

        public static DataTable GetRelatedTopicProducts(int topicid)
        {
            return new TopicDao().GetRelatedTopicProducts(topicid);
        }

        public static DbQueryResult GetSubDistributorsRankings(string startTime, string endTime, int pgSize, int CurrPage, int belongUserId, int grade)
        {
            return new DistributorsDao().GetSubDistributorsRankings(startTime, endTime, pgSize, CurrPage, belongUserId, grade);
        }

        public static DbQueryResult GetSubDistributorsRankingsN(string startTime, string endTime, int pgSize, int CurrPage, int belongUserId, int grade)
        {
            return new DistributorsDao().GetSubDistributorsRankingsN(startTime, endTime, pgSize, CurrPage, belongUserId, grade);
        }

        public static TopicInfo Gettopic(int topicId)
        {
            return new TopicDao().GetTopic(topicId);
        }

        public static DbQueryResult GettopicList(TopicQuery page)
        {
            return new TopicDao().GetTopicList(page);
        }

        public static IList<TopicInfo> Gettopics()
        {
            return new TopicDao().GetTopics();
        }

        public static IList<MenuInfo> GetTopMenus(string wid)
        {
            return new MenuDao().GetTopMenus(wid);
        }

        public static TplCfgInfo GetTplCfgById(int id)
        {
            return new BannerDao().GetTplCfgById(id);
        }

        public static DataTable GetType(int Types)
        {
            return new ActivitiesDao().GetType(Types);
        }

        public static DistributorsInfo GetUserIdDistributors(int userid)
        {
            return new DistributorsDao().GetDistributorInfo(userid);
        }

        public static int InsertLotteryActivity(LotteryActivityInfo info)
        {
            string str = JsonConvert.SerializeObject(info.PrizeSettingList);
            info.PrizeSetting = str;
            return new LotteryActivityDao().InsertLotteryActivity(info);
        }

        public static int IsExistUsers(string userIds)
        {
            return new DistributorsDao().IsExistUsers(userIds);
        }

        public static bool RemoveAllHomeProduct()
        {
            return new HomeProductDao().RemoveAllHomeProduct();
        }

        public static bool RemoveAllHomeTopics()
        {
            return new HomeTopicDao().RemoveAllHomeTopics();
        }

        public static bool RemoveHomeProduct(int productId)
        {
            return new HomeProductDao().RemoveHomeProduct(productId);
        }

        public static bool RemoveHomeTopic(int TopicId)
        {
            return new HomeTopicDao().RemoveHomeTopic(TopicId);
        }

        public static bool RemoveReleatesProductBytopicid(int topicid)
        {
            return new TopicDao().RemoveReleatesProductBytopicid(topicid);
        }

        public static bool RemoveReleatesProductBytopicid(int topicid, int productId)
        {
            return new TopicDao().RemoveReleatesProductBytopicid(topicid, productId);
        }

        public static bool SaveActivity(ActivityInfo activity)
        {
            int num = new ActivityDao().SaveActivity(activity);
            ReplyInfo reply = new TextReplyInfo {
                Keys = activity.Keys,
                MatchType = MatchType.Equal,
                MessageType = MessageType.Text,
                ReplyType = ReplyType.SignUp,
                ActivityId = num
            };
            return new ReplyDao().SaveReply(reply);
        }

        public static bool SaveAlarm(AlarmInfo info)
        {
            return new AlarmDao().Save(info);
        }

        public static bool SaveFeedBack(FeedBackInfo info)
        {
            return new FeedBackDao().Save(info);
        }

        public static int SaveLotteryTicket(LotteryTicketInfo info)
        {
            string str = JsonConvert.SerializeObject(info.PrizeSettingList);
            info.PrizeSetting = str;
            return new LotteryActivityDao().SaveLotteryTicket(info);
        }

        public static bool SaveMenu(MenuInfo menu)
        {
            return new MenuDao().SaveMenu(menu);
        }

        public static bool SaveTplCfg(TplCfgInfo info)
        {
            return new BannerDao().SaveTplCfg(info);
        }

        public static void SwapMenuSequence(int menuId, bool isUp,string wid)
        {
            new MenuDao().SwapMenuSequence(menuId, isUp,wid);
        }

        public static bool SwapTopicSequence(int topicid, int displaysequence)
        {
            return new TopicDao().SwapTopicSequence(topicid, displaysequence);
        }

        public static void SwapTplCfgSequence(int bannerId, int replaceBannerId)
        {
            BannerDao dao = new BannerDao();
            TplCfgInfo tplCfgById = dao.GetTplCfgById(bannerId);
            TplCfgInfo info = dao.GetTplCfgById(replaceBannerId);
            if ((tplCfgById != null) && (info != null))
            {
                int displaySequence = tplCfgById.DisplaySequence;
                tplCfgById.DisplaySequence = info.DisplaySequence;
                info.DisplaySequence = displaySequence;
                dao.UpdateTplCfg(tplCfgById);
                dao.UpdateTplCfg(info);
            }
        }

        public static bool UpdateActivities(ActivitiesInfo activity)
        {
            return new ActivitiesDao().UpdateActivities(activity);
        }

        public static bool UpdateActivity(ActivityInfo activity)
        {
            return new ActivityDao().UpdateActivity(activity);
        }

        public static bool UpdateBalanceDistributors(int UserId, decimal ReferralRequestBalance)
        {
            return new DistributorsDao().UpdateBalanceDistributors(UserId, ReferralRequestBalance);
        }

        public static bool UpdateBalanceDrawRequest(int Id, string Remark)
        {
            HiCache.Remove(string.Format("DataCache-Distributor-{0}", Id));
            return new DistributorsDao().UpdateBalanceDrawRequest(Id, Remark, null);
        }

        public static bool UpdateFeedBackMsgType(string feedBackId, string msgType)
        {
            return new FeedBackDao().UpdateMsgType(feedBackId, msgType);
        }

        public static bool UpdateHomeProductSequence(int ProductId, int displaysequence)
        {
            return new HomeProductDao().UpdateHomeProductSequence(ProductId, displaysequence);
        }

        public static bool UpdateHomeTopicSequence(int TopicId, int displaysequence)
        {
            return new HomeTopicDao().UpdateHomeTopicSequence(TopicId, displaysequence);
        }

        public static bool UpdateLotteryActivity(LotteryActivityInfo info)
        {
            string str = JsonConvert.SerializeObject(info.PrizeSettingList);
            info.PrizeSetting = str;
            return new LotteryActivityDao().UpdateLotteryActivity(info);
        }

        public static bool UpdateLotteryTicket(LotteryTicketInfo info)
        {
            string str = JsonConvert.SerializeObject(info.PrizeSettingList);
            info.PrizeSetting = str;
            return new LotteryActivityDao().UpdateLotteryTicket(info);
        }

        public static bool UpdateMenu(MenuInfo menu)
        {
            return new MenuDao().UpdateMenu(menu);
        }

        public static bool UpdateRelateProductSequence(int TopicId, int RelatedProductId, int displaysequence)
        {
            return new TopicDao().UpdateRelateProductSequence(TopicId, RelatedProductId, displaysequence);
        }

        public static void UpdateSettings(IList<MessageTemplate> templates,string wid)
        {
            if ((templates != null) && (templates.Count != 0))
            {
                new MessageTemplateHelperDao().UpdateSettings(templates,wid);
                foreach (MessageTemplate template in templates)
                {
                    HiCache.Remove(string.Format("Message-{0}", template.MessageType.ToLower()));
                }
            }
        }

        public static void UpdateTemplate(MessageTemplate template,string wid)
        {
            if (template != null)
            {
                new MessageTemplateHelperDao().UpdateTemplate(template,wid);
                HiCache.Remove(string.Format("Message-{0}", template.MessageType.ToLower()));
            }
        }

        public static bool Updatetopic(TopicInfo topic)
        {
            if (topic == null)
            {
                return false;
            }
            Globals.EntityCoding(topic, true);
            return new TopicDao().UpdateTopic(topic);
        }

        public static bool UpdateTplCfg(TplCfgInfo info)
        {
            return new BannerDao().UpdateTplCfg(info);
        }

        public static string UploadDefautBg(HttpPostedFile postedFile)
        {
            if (!ResourcesHelper.CheckPostedFile(postedFile, "image"))
            {
                return string.Empty;
            }
            string str = Globals.GetVshopSkinPath(null) + "/images/ad/DefautPageBg" + Path.GetExtension(postedFile.FileName);
            postedFile.SaveAs(HttpContext.Current.Request.MapPath(Globals.ApplicationPath + str));
            return str;
        }

        public static string UploadTopicImage(HttpPostedFile postedFile)
        {
            if (!ResourcesHelper.CheckPostedFile(postedFile, "image"))
            {
                return string.Empty;
            }
            string str = Globals.GetStoragePath() + "/topic/" + ResourcesHelper.GenerateFilename(Path.GetExtension(postedFile.FileName));
            postedFile.SaveAs(HttpContext.Current.Request.MapPath(Globals.ApplicationPath + str));
            return str;
        }

        public static string UploadVipBGImage(HttpPostedFile postedFile)
        {
            if (!ResourcesHelper.CheckPostedFile(postedFile, "image"))
            {
                return string.Empty;
            }
            string str = Globals.GetStoragePath() + "/Vipcard/vipbg" + Path.GetExtension(postedFile.FileName);
            postedFile.SaveAs(HttpContext.Current.Request.MapPath(Globals.ApplicationPath + str));
            return str;
        }

        public static string UploadVipQRImage(HttpPostedFile postedFile)
        {
            if (!ResourcesHelper.CheckPostedFile(postedFile, "image"))
            {
                return string.Empty;
            }
            string str = Globals.GetStoragePath() + "/Vipcard/vipqr" + Path.GetExtension(postedFile.FileName);
            postedFile.SaveAs(HttpContext.Current.Request.MapPath(Globals.ApplicationPath + str));
            return str;
        }

        public static string UploadWeiXinCodeImage(HttpPostedFile postedFile)
        {
            if (!ResourcesHelper.CheckPostedFile(postedFile, "image"))
            {
                return string.Empty;
            }
            string str = Globals.GetStoragePath() + "/WeiXinCodeImageUrl" + Path.GetExtension(postedFile.FileName);
            postedFile.SaveAs(HttpContext.Current.Request.MapPath(Globals.ApplicationPath + str));
            return str;
        }
    }
}

