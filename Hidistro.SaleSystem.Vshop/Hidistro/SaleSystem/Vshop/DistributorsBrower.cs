namespace Hidistro.SaleSystem.Vshop
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities.Members;
    using Hidistro.Entities.Orders;
    using Hidistro.Entities.Promotions;
    using Hidistro.Entities.VShop;
    using Hidistro.SqlDal.Members;
    using Hidistro.SqlDal.Orders;
    using Hidistro.SqlDal.Promotions;
    using Hidistro.SqlDal.VShop;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Runtime.InteropServices;
    using System.Web.Caching;
    using System.Linq;

    public class DistributorsBrower
    {
        public static bool AddBalanceDrawRequest(BalanceDrawRequestInfo balancerequestinfo, MemberInfo memberinfo)
        {
            DistributorsInfo currentDistributors = GetCurrentDistributors();
            if ((((memberinfo == null) || string.IsNullOrEmpty(memberinfo.RealName)) || ((currentDistributors == null) || (currentDistributors.UserId <= 0))) || string.IsNullOrEmpty(memberinfo.CellPhone))
            {
                return false;
            }
            if (!string.IsNullOrEmpty(balancerequestinfo.MerchantCode) && (currentDistributors.RequestAccount != balancerequestinfo.MerchantCode))
            {
                new DistributorsDao().UpdateDistributorById(balancerequestinfo.MerchantCode, memberinfo.UserId);
            }
            balancerequestinfo.UserId = memberinfo.UserId;
            balancerequestinfo.UserName = memberinfo.UserName;
            if ((balancerequestinfo.RequesType == 0) || (balancerequestinfo.RequesType == 3))
            {
                balancerequestinfo.MerchantCode = memberinfo.OpenId;
            }
            else if (balancerequestinfo.MerchantCode.Length < 1)
            {
                balancerequestinfo.MerchantCode = currentDistributors.RequestAccount;
            }
            balancerequestinfo.CellPhone = memberinfo.CellPhone;
            return new DistributorsDao().AddBalanceDrawRequest(balancerequestinfo);
        }

        public static void AddDistributorProductId(List<int> productList)
        {
            int currentMemberUserId = Globals.GetCurrentMemberUserId();
            if ((currentMemberUserId > 0) && (productList.Count > 0))
            {
                new DistributorsDao().RemoveDistributorProducts(productList, currentMemberUserId);
                foreach (int num2 in productList)
                {
                    new DistributorsDao().AddDistributorProducts(num2, currentMemberUserId);
                }
            }
        }

        public static bool AddDistributors(DistributorsInfo distributors)
        {
            if (IsExiteDistributorsByStoreName(distributors.StoreName) > 0)
            {
                return false;
            }
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            distributors.DistributorGradeId = DistributorGrade.OneDistributor;
            distributors.ParentUserId = new int?(currentMember.UserId);
            distributors.UserId = currentMember.UserId;
            DistributorsInfo currentDistributors = GetCurrentDistributors();
            if (currentDistributors != null)
            {
                if (!string.IsNullOrEmpty(currentDistributors.ReferralPath) && !currentDistributors.ReferralPath.Contains("|"))
                {
                    distributors.ReferralPath = currentDistributors.ReferralPath + "|" + currentDistributors.UserId.ToString();
                }
                else if (!string.IsNullOrEmpty(currentDistributors.ReferralPath) && currentDistributors.ReferralPath.Contains("|"))
                {
                    distributors.ReferralPath = currentDistributors.ReferralPath.Split(new char[] { '|' })[1] + "|" + currentDistributors.UserId.ToString();
                }
                else
                {
                    distributors.ReferralPath = currentDistributors.UserId.ToString();
                }
                distributors.ParentUserId = new int?(currentDistributors.UserId);
                if (currentDistributors.DistributorGradeId == DistributorGrade.OneDistributor)
                {
                    distributors.DistributorGradeId = DistributorGrade.TowDistributor;
                }
                else if (currentDistributors.DistributorGradeId == DistributorGrade.TowDistributor)
                {
                    distributors.DistributorGradeId = DistributorGrade.ThreeDistributor;
                }
                else
                {
                    distributors.DistributorGradeId = DistributorGrade.ThreeDistributor;
                }
            }
            return new DistributorsDao().CreateDistributor(distributors);
        }

        public static void DeleteDistributorProductIds(List<int> productList)
        {
            int userId = GetCurrentDistributors().UserId;
            if ((userId > 0) && (productList.Count > 0))
            {
                new DistributorsDao().RemoveDistributorProducts(productList, userId);
            }
        }

        public static int EditCommisionsGrade(string userids, string Grade)
        {
            return new DistributorsDao().EditCommisionsGrade(userids, Grade);
        }

        public static bool EditDisbutosInfos(string userid, string QQNum, string CellPhone, string RealName, string Password)
        {
            return new DistributorsDao().EditDisbutosInfos(userid, QQNum, CellPhone, RealName, Password);
        }

        public static bool FrozenCommision(int userid, string ReferralStatus)
        {
            return new DistributorsDao().FrozenCommision(userid, ReferralStatus);
        }

        public static int FrozenCommisionChecks(string userids, string ReferralStatus)
        {
            return new DistributorsDao().FrozenCommisionChecks(userids, ReferralStatus);
        }

        public static DbQueryResult GetBalanceDrawRequest(BalanceDrawRequestQuery query, string[] extendChecks = null)
        {
            return new DistributorsDao().GetBalanceDrawRequest(query, extendChecks);
        }

        public static BalanceDrawRequestInfo GetBalanceDrawRequestById(string serialids)
        {
            return new DistributorsDao().GetBalanceDrawRequestById(serialids);
        }

        public static bool GetBalanceDrawRequestIsCheck(int serialid)
        {
            return new DistributorsDao().GetBalanceDrawRequestIsCheck(serialid);
        }

        public static int GetBalanceDrawRequestIsCheckStatus(int serialid)
        {
            return new DistributorsDao().GetBalanceDrawRequestIsCheckStatus(serialid);
        }

        public static string GetBalanceDrawRequestStatus(int status)
        {
            switch (status)
            {
                case -1:
                    return "已驳回";

                case 0:
                    return "待审核";

                case 1:
                    return "已审核";

                case 2:
                    return "已发放";

                case 3:
                    return "付款异常";
            }
            return "未知";
        }

        public static DbQueryResult GetCommissions(CommissionsQuery query)
        {
            return new DistributorsDao().GetCommissions(query);
        }

        public static DistributorsInfo GetCurrentDistributors()
        {
            return GetCurrentDistributors(Globals.GetCurrentDistributorId());
        }

        public static DistributorsInfo GetCurrentDistributors(int userId)
        {
            DistributorsInfo distributorInfo = HiCache.Get(string.Format("DataCache-Distributor-{0}", userId)) as DistributorsInfo;
            if ((distributorInfo == null) || (distributorInfo.UserId == 0))
            {
                distributorInfo = new DistributorsDao().GetDistributorInfo(userId);
                HiCache.Insert(string.Format("DataCache-Distributor-{0}", userId), distributorInfo, 360, CacheItemPriority.Normal);
            }
            return distributorInfo;
        }

        public static DataTable GetCurrentDistributorsCommosion()
        {
            return new DistributorsDao().GetDistributorsCommosion(Globals.GetCurrentDistributorId());
        }

        public static DataTable GetCurrentDistributorsCommosion(int userId)
        {
            return new DistributorsDao().GetCurrentDistributorsCommosion(userId);
        }

        public static int GetDistributorGrades(string ReferralUserId)
        {
            DistributorsInfo userIdDistributors = GetUserIdDistributors(int.Parse(ReferralUserId));
            List<DistributorGradeInfo> distributorGrades = new DistributorsDao().GetDistributorGrades() as List<DistributorGradeInfo>;
            foreach (DistributorGradeInfo info2 in from item in distributorGrades
                orderby item.CommissionsLimit descending
                select item)
            {
                if (userIdDistributors.DistriGradeId == info2.GradeId)
                {
                    return 0;
                }
                if (info2.CommissionsLimit <= (userIdDistributors.ReferralBlance + userIdDistributors.ReferralRequestBalance))
                {
                    userIdDistributors.DistriGradeId = info2.GradeId;
                    return info2.GradeId;
                }
            }
            return 0;
        }

        public static DistributorsInfo GetDistributorInfo(int distributorid)
        {
            return new DistributorsDao().GetDistributorInfo(distributorid);
        }

        public static int GetDistributorNum(DistributorGrade grade)
        {
            return new DistributorsDao().GetDistributorNum(grade);
        }

        public static DataSet GetDistributorOrder(OrderQuery query)
        {
            return new OrderDao().GetDistributorOrder(query);
        }

        public static int GetDistributorOrderCount(OrderQuery query)
        {
            return new OrderDao().GetDistributorOrderCount(query);
        }

        public static DbQueryResult GetDistributors(DistributorsQuery query)
        {
            return new DistributorsDao().GetDistributors(query, null, null);
        }

        public static DataTable GetDistributorsCommission(DistributorsQuery query)
        {
            return new DistributorsDao().GetDistributorsCommission(query);
        }

        public static DataTable GetDistributorsCommosion(int userId, DistributorGrade distributorgrade)
        {
            return new DistributorsDao().GetDistributorsCommosion(userId, distributorgrade);
        }

        public static int GetDownDistributorNum(string userid)
        {
            return new DistributorsDao().GetDownDistributorNum(userid);
        }

        public static DataTable GetDownDistributors(DistributorsQuery query)
        {
            return new DistributorsDao().GetDownDistributors(query);
        }

        public static DataTable GetDrawRequestNum(int[] CheckValues)
        {
            return new DistributorsDao().GetDrawRequestNum(CheckValues);
        }

        public static Dictionary<int, int> GetMulBalanceDrawRequestIsCheckStatus(int[] serialids)
        {
            return new DistributorsDao().GetMulBalanceDrawRequestIsCheckStatus(serialids);
        }

        public static int GetNotDescDistributorGrades(string ReferralUserId)
        {
            DistributorsInfo userIdDistributors = GetUserIdDistributors(int.Parse(ReferralUserId));
            decimal num2 = userIdDistributors.ReferralBlance + userIdDistributors.ReferralRequestBalance;
            DistributorGradeInfo distributorGradeInfo = DistributorGradeBrower.GetDistributorGradeInfo(userIdDistributors.DistriGradeId);
            if ((distributorGradeInfo != null) && (num2 < distributorGradeInfo.CommissionsLimit))
            {
                return userIdDistributors.DistriGradeId;
            }
            List<DistributorGradeInfo> distributorGrades = new DistributorsDao().GetDistributorGrades() as List<DistributorGradeInfo>;
            foreach (DistributorGradeInfo info3 in from item in distributorGrades
                orderby item.CommissionsLimit descending
                select item)
            {
                if (userIdDistributors.DistriGradeId == info3.GradeId)
                {
                    return 0;
                }
                if (info3.CommissionsLimit <= (userIdDistributors.ReferralBlance + userIdDistributors.ReferralRequestBalance))
                {
                    return info3.GradeId;
                }
            }
            return 0;
        }

        public static DataTable GetNotSendRedpackRecord(int balancedrawrequestid)
        {
            return new SendRedpackRecordDao().GetNotSendRedpackRecord(balancedrawrequestid);
        }

        public static int GetRedPackTotalAmount(int balancedrawrequestid, int userid)
        {
            return new SendRedpackRecordDao().GetRedPackTotalAmount(balancedrawrequestid, userid);
        }

        public static SendRedpackRecordInfo GetSendRedpackRecordByID(string id = null, string sid = null)
        {
            return new SendRedpackRecordDao().GetSendRedpackRecordByID(id, sid);
        }

        public static DbQueryResult GetSendRedpackRecordRequest(SendRedpackRecordQuery query)
        {
            return new SendRedpackRecordDao().GetSendRedpackRecordRequest(query);
        }

        public static decimal GetUserCommissions(int userid, DateTime fromdatetime, string endtime = null, string storeName = null, string OrderNum = null)
        {
            return new DistributorsDao().GetUserCommissions(userid, fromdatetime, endtime, storeName, OrderNum);
        }

        public static DistributorsInfo GetUserIdDistributors(int userid)
        {
            return new DistributorsDao().GetDistributorInfo(userid);
        }

        public static DataSet GetUserRanking(int userid)
        {
            return new DistributorsDao().GetUserRanking(userid);
        }

        public static bool HasDrawRequest(int serialid)
        {
            return new SendRedpackRecordDao().HasDrawRequest(serialid);
        }

        private static int IsExiteDistributorsByStoreName(string stroname)
        {
            return new DistributorsDao().IsExiteDistributorsByStoreName(stroname);
        }

        public static bool IsExitsCommionsRequest()
        {
            return new DistributorsDao().IsExitsCommionsRequest(Globals.GetCurrentDistributorId());
        }

        public static DataTable OrderIDGetCommosion(string orderid)
        {
            return new DistributorsDao().OrderIDGetCommosion(orderid);
        }

        public static void RemoveDistributorCache(int userId)
        {
            HiCache.Remove(string.Format("DataCache-Distributor-{0}", userId));
        }

        public static DataTable SelectDistributors(DistributorsQuery query)
        {
            return new DistributorsDao().SelectDistributors(query, null, null);
        }

        public static string SendRedPackToBalanceDrawRequest(int serialid,string wid)
        {
            return new DistributorsDao().SendRedPackToBalanceDrawRequest(serialid,wid);
        }

        public static bool SetBalanceDrawRequestIsCheckStatus(int[] serialids, int checkValue, string Remark = null, string Amount = null)
        {
            return new DistributorsDao().SetBalanceDrawRequestIsCheckStatus(serialids, checkValue, Remark, Amount);
        }

        public static bool setCommission(OrderInfo order, DistributorsInfo DisInfo)
        {
            bool flag = false;
            decimal num = 0M;
            decimal num2 = 0M;
            decimal resultCommTatal = 0M;
            string userId = order.ReferralUserId.ToString();
            string orderId = order.OrderId;
            decimal orderTotal = 0M;
            ArrayList gradeIdList = new ArrayList();
            ArrayList referralUserIdList = new ArrayList();
            foreach (LineItemInfo info in order.LineItems.Values)
            {
                if (info.OrderItemsStatus.ToString() == OrderStatus.SellerAlreadySent.ToString())
                {
                    num2 += info.ItemsCommission;
                    if ((!string.IsNullOrEmpty(info.ItemAdjustedCommssion.ToString()) && (info.ItemAdjustedCommssion > 0M)) && !info.IsAdminModify)
                    {
                        num += info.ItemAdjustedCommssion;
                    }
                    orderTotal += (info.GetSubTotal() - info.DiscountAverage) - info.ItemAdjustedCommssion;
                }
            }
            orderTotal -= order.Freight;
            if (false)
            {
                resultCommTatal = num2;
            }
            else
            {
                resultCommTatal = num2 - num;
                if (resultCommTatal < 0M)
                {
                    resultCommTatal = 0M;
                }
            }
            flag = new DistributorsDao().UpdateCalculationCommission(userId, userId, orderId, orderTotal, resultCommTatal);
            int notDescDistributorGrades = GetNotDescDistributorGrades(userId);
            if (notDescDistributorGrades > 0)
            {
                gradeIdList.Add(notDescDistributorGrades);
                referralUserIdList.Add(userId);
                flag = new DistributorsDao().UpdateGradeId(gradeIdList, referralUserIdList);
            }
            return flag;
        }

        public static bool SetRedpackRecordIsUsed(int id, bool issend)
        {
            return new SendRedpackRecordDao().SetRedpackRecordIsUsed(id, issend);
        }

        public static bool UpdateCalculationCommission(OrderInfo order,string wid)
        {
            DistributorsInfo userIdDistributors = GetUserIdDistributors(order.ReferralUserId);
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(false,wid);
            bool flag = false;
            if (userIdDistributors != null)
            {
                if (!masterSettings.EnableCommission)
                {
                    if (userIdDistributors.ReferralStatus == 0)
                    {
                        flag = setCommission(order, userIdDistributors);
                    }
                }
                else
                {
                    if (userIdDistributors.ReferralStatus == 0)
                    {
                        flag = setCommission(order, userIdDistributors);
                    }
                    if (!string.IsNullOrEmpty(userIdDistributors.ReferralPath))
                    {
                        ArrayList commTatalList = new ArrayList();
                        decimal num = 0M;
                        ArrayList userIdList = new ArrayList();
                        string referralUserId = order.ReferralUserId.ToString();
                        string orderId = order.OrderId;
                        ArrayList orderTotalList = new ArrayList();
                        decimal num2 = 0M;
                        ArrayList gradeIdList = new ArrayList();
                        string[] strArray = userIdDistributors.ReferralPath.Split(new char[] { '|' });
                        if (strArray.Length == 1)
                        {
                            DistributorsInfo info2 = GetUserIdDistributors(int.Parse(strArray[0]));
                            if (info2.ReferralStatus == 0)
                            {
                                foreach (LineItemInfo info3 in order.LineItems.Values)
                                {
                                    if (info3.OrderItemsStatus.ToString() == OrderStatus.SellerAlreadySent.ToString())
                                    {
                                        num += info3.SecondItemsCommission;
                                        num2 += info3.GetSubTotal();
                                    }
                                }
                                commTatalList.Add(num);
                                orderTotalList.Add(num2);
                                userIdList.Add(info2.UserId);
                            }
                        }
                        if (strArray.Length == 2)
                        {
                            DistributorsInfo info4 = GetUserIdDistributors(int.Parse(strArray[0]));
                            if (info4.ReferralStatus == 0)
                            {
                                foreach (LineItemInfo info5 in order.LineItems.Values)
                                {
                                    if (info5.OrderItemsStatus.ToString() == OrderStatus.SellerAlreadySent.ToString())
                                    {
                                        num += info5.ThirdItemsCommission;
                                        num2 += info5.GetSubTotal();
                                    }
                                }
                                commTatalList.Add(num);
                                orderTotalList.Add(num2);
                                userIdList.Add(info4.UserId);
                            }
                            DistributorsInfo info6 = GetUserIdDistributors(int.Parse(strArray[1]));
                            num = 0M;
                            num2 = 0M;
                            if (info6.ReferralStatus == 0)
                            {
                                foreach (LineItemInfo info7 in order.LineItems.Values)
                                {
                                    if (info7.OrderItemsStatus.ToString() == OrderStatus.SellerAlreadySent.ToString())
                                    {
                                        num += info7.SecondItemsCommission;
                                        num2 += info7.GetSubTotal();
                                    }
                                }
                                commTatalList.Add(num);
                                orderTotalList.Add(num2);
                                userIdList.Add(info6.UserId);
                            }
                        }
                        flag = new DistributorsDao().UpdateTwoCalculationCommission(userIdList, referralUserId, orderId, orderTotalList, commTatalList);
                        for (int i = 0; i < userIdList.Count; i++)
                        {
                            int notDescDistributorGrades = GetNotDescDistributorGrades(userIdList[i].ToString());
                            gradeIdList.Add(notDescDistributorGrades);
                        }
                        flag = new DistributorsDao().UpdateGradeId(gradeIdList, userIdList);
                    }
                }
                RemoveDistributorCache(userIdDistributors.UserId);
            }
            OrderRedPagerBrower.CreateOrderRedPager(order.OrderId, order.GetTotal(), order.UserId);
            int id = Globals.IsNumeric(order.ActivitiesId) ? Globals.ToNum(order.ActivitiesId) : 0;
            if (id > 0)
            {
                ActivityDetailInfo activityDetailInfo = new Hidistro.SqlDal.VShop.ActivityDao().GetActivityDetailInfo(id);
                if (activityDetailInfo != null)
                {
                    int couponId = activityDetailInfo.CouponId;
                    int integral = activityDetailInfo.Integral;
                    if ((couponId > 0) && (ShoppingProcessor.GetCoupon(couponId.ToString()) != null))
                    {
                        new CouponDao().SendCouponToMember(couponId, order.UserId);
                    }
                    if (integral > 0)
                    {
                        new OrderDao().AddMemberPointNumber(integral, order, null);
                    }
                }
            }
            MemberProcessor.UpdateUserAccount(order,wid);
            return flag;
        }

        public static bool UpdateDistributor(DistributorsInfo query)
        {
            int num = IsExiteDistributorsByStoreName(query.StoreName);
            if ((num != 0) && (num != query.UserId))
            {
                return false;
            }
            return new DistributorsDao().UpdateDistributor(query);
        }

        public static bool UpdateDistributorMessage(DistributorsInfo query)
        {
            int num = IsExiteDistributorsByStoreName(query.StoreName);
            if ((num != 0) && (num != query.UserId))
            {
                return false;
            }
            return new DistributorsDao().UpdateDistributorMessage(query);
        }
    }
}

