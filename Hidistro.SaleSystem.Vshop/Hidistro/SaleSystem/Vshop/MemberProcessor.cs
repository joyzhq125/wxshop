namespace Hidistro.SaleSystem.Vshop
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Commodities;
    using Hidistro.Entities.Members;
    using Hidistro.Entities.Orders;
    using Hidistro.Entities.Sales;
    using Hidistro.Messages;
    using Hidistro.SqlDal.Commodities;
    using Hidistro.SqlDal.Members;
    using Hidistro.SqlDal.Orders;
    using Hidistro.SqlDal.Promotions;
    using Hidistro.SqlDal.Sales;
    using Hidistro.SqlDal.VShop;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Web.Caching;

    public static class MemberProcessor
    {
        public static bool AddIntegralDetail(IntegralDetailInfo point)
        {
            return new IntegralDetailDao().AddIntegralDetail(point, null);
        }

        public static int AddShippingAddress(ShippingAddressInfo shippingAddress)
        {
            ShippingAddressDao dao = new ShippingAddressDao();
            int shippingId = dao.AddShippingAddress(shippingAddress);
            if (dao.SetDefaultShippingAddress(shippingId, Globals.GetCurrentMemberUserId()))
            {
                return 1;
            }
            return 0;
        }

        public static bool BindUserName(int UserId, string UserBindName, string Password)
        {
            MemberDao dao = new MemberDao();
            return dao.BindUserName(UserId, UserBindName, Password);
        }

        public static bool ConfirmOrderFinish(OrderInfo order)
        {
            bool flag = false;
            if (order.CheckAction(OrderActions.BUYER_CONFIRM_GOODS))
            {
                order.OrderStatus = OrderStatus.Finished;
                order.FinishDate = new DateTime?(DateTime.Now);
                //货到付款 付款时间即确认收货时间
                if(order.Gateway== "hishop.plugins.payment.podrequest")
                {
                    order.PayDate = new DateTime?(DateTime.Now);
                }
                flag = new OrderDao().UpdateOrder(order, null);
                HiCache.Remove(string.Format("DataCache-Member-{0}", order.UserId));
            }
            return flag;
        }

        public static bool CreateMember(MemberInfo member)
        {
            MemberDao dao = new MemberDao();
            return dao.CreateMember(member);
        }

        public static bool Delete(int userId)
        {
            bool flag = new MemberDao().Delete(userId);
            if (flag)
            {
                HiCache.Remove(string.Format("DataCache-Member-{0}", userId));
            }
            return flag;
        }

        public static bool DelShippingAddress(int shippingid, int userid)
        {
            return new ShippingAddressDao().DelShippingAddress(shippingid, userid);
        }

        public static bool DelUserMessage(int userid, string openid, string userhead, int olduserid)
        {
            return new MemberDao().DelUserMessage(userid, openid, userhead, olduserid);
        }

        public static MemberInfo GetBindusernameMember(string UserBindName,string wid)
        {
            MemberDao dao = new MemberDao();
            return dao.GetBindusernameMember(UserBindName,wid);
        }

        public static DataTable GetCouponByProducts(int couponId, int ProductId)
        {
            return new CouponDao().GetCouponByProducts(couponId, ProductId);
        }

        public static MemberInfo GetCurrentMember()
        {
            return new MemberDao().GetMember(Globals.GetCurrentMemberUserId());
        }

        public static int GetDefaultMemberGrade(string wid)
        {
            return new MemberGradeDao().GetDefaultMemberGrade(wid);
        }

        public static ShippingAddressInfo GetDefaultShippingAddress()
        {
            foreach (ShippingAddressInfo info in new ShippingAddressDao().GetShippingAddresses(Globals.GetCurrentMemberUserId()))
            {
                if (info.IsDefault)
                {
                    return info;
                }
            }
            return null;
        }

        public static decimal GetIntegral(int userId)
        {
            return new PointDetailDao().GetIntegral(userId);
        }

        public static DbQueryResult GetIntegralDetail(IntegralDetailQuery query)
        {
            return new IntegralDetailDao().GetIntegralDetail(query);
        }

        public static MemberInfo GetMember()
        {
            return GetMember(Globals.GetCurrentMemberUserId(), true);
        }

        public static MemberInfo GetMember(string sessionId)
        {
            return new MemberDao().GetMember(sessionId);
        }

        public static MemberInfo GetMember(int userId, bool isUserCache = true)
        {
            MemberInfo member = null;
            if (isUserCache)
            {
                member = HiCache.Get(string.Format("DataCache-Member-{0}", userId)) as MemberInfo;
                if (member == null)
                {
                    member = new MemberDao().GetMember(userId);
                    HiCache.Insert(string.Format("DataCache-Member-{0}", userId), member, 360, CacheItemPriority.Normal);
                }
                return member;
            }
            return new MemberDao().GetMember(userId);
        }

        public static MemberGradeInfo GetMemberGrade(int gradeId)
        {
            return new MemberGradeDao().GetMemberGrade(gradeId);
        }

        public static MemberInfo GetOpenIdMember(string OpenId)
        {
            MemberDao dao = new MemberDao();
            return dao.GetOpenIdMember(OpenId);
        }

        public static int GetPoint(decimal money,string wid)
        {
            if (money == 0M)
            {
                return 0;
            }
            int num = 0;
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(false,wid);
            if (!masterSettings.shopping_score_Enable)
            {
                return 0;
            }
            if (masterSettings.PointsRate != 0M)
            {
                num = (int) Math.Round((decimal) (money / masterSettings.PointsRate), 0);
                if (num > 0x7fffffff)
                {
                    return 0x7fffffff;
                }
            }
            if (masterSettings.shopping_reward_Enable && (money >= ((decimal) masterSettings.shopping_reward_OrderValue)))
            {
                num += masterSettings.shopping_reward_Score;
                if (num > 0x7fffffff)
                {
                    num = 0x7fffffff;
                }
            }
            return num;
        }

        public static ShippingAddressInfo GetShippingAddress(int shippingId)
        {
            return new ShippingAddressDao().GetShippingAddress(shippingId, Globals.GetCurrentMemberUserId());
        }

        public static int GetShippingAddressCount()
        {
            return new ShippingAddressDao().GetShippingAddresses(Globals.GetCurrentMemberUserId()).Count;
        }

        public static IList<ShippingAddressInfo> GetShippingAddresses()
        {
            return new ShippingAddressDao().GetShippingAddresses(Globals.GetCurrentMemberUserId());
        }

        public static DataTable GetUserCoupons()
        {
            return new CouponDao().GetUserCoupons();
        }

        public static DataTable GetUserCoupons(int userId, int useType = 0)
        {
            return new CouponDao().GetUserCoupons(userId, useType);
        }

        public static int GetUserHistoryPoint(int userId)
        {
            return new PointDetailDao().GetHistoryPoint(userId);
        }

        public static MemberInfo GetusernameMember(string username,string wid)
        {
            return new MemberDao().GetusernameMember(username,wid);
        }

        public static DataSet GetUserOrder(int userId, OrderQuery query)
        {
            return new OrderDao().GetUserOrder(userId, query);
        }

        public static int GetUserOrderCount(int userId, OrderQuery query)
        {
            return new OrderDao().GetUserOrderCount(userId, query);
        }

        public static DataSet GetUserOrderReturn(int userId, OrderQuery query)
        {
            return new OrderDao().GetUserOrderReturn(userId, query);
        }

        public static int GetUserOrderReturnCount(int userId)
        {
            return new OrderDao().GetUserOrderReturnCount(userId);
        }

        public static bool ReSetUserHead(string userid, string wxName, string wxHead, string Openid = "")
        {
            return new MemberDao().ReSetUserHead(userid, wxName, wxHead, Openid);
        }

        public static bool SetDefaultShippingAddress(int shippingId, int UserId)
        {
            return new ShippingAddressDao().SetDefaultShippingAddress(shippingId, UserId);
        }

        public static bool SetMemberSessionId(MemberInfo member)
        {
            MemberDao dao = new MemberDao();
            return dao.SetMemberSessionId(member.SessionId, member.SessionEndTime, member.OpenId);
        }

        public static bool SetMemberSessionId(string sessionId, DateTime sessionEndTime, string openId)
        {
            return new MemberDao().SetMemberSessionId(sessionId, sessionEndTime, openId);
        }

        public static int SetMultiplePwd(string userids, string pwd)
        {
            return new MemberDao().SetMultiplePwd(userids, pwd);
        }

        public static bool SetPwd(string userid, string pwd)
        {
            return new MemberDao().SetPwd(userid, pwd);
        }

        public static bool UpdateMember(MemberInfo member)
        {
            HiCache.Remove(string.Format("DataCache-Member-{0}", member.UserId));
            return new MemberDao().Update(member);
        }

        public static bool UpdateOpenid(MemberInfo member)
        {
            return new MemberDao().UpdateOpenid(member);
        }

        public static bool UpdateShippingAddress(ShippingAddressInfo shippingAddress)
        {
            return new ShippingAddressDao().UpdateShippingAddress(shippingAddress);
        }

        public static void UpdateUserAccount(OrderInfo order,string wid)
        {
            Func<MemberGradeInfo, bool> predicate = null;
            Func<MemberGradeInfo, bool> func2 = null;
            MemberDao dao = new MemberDao();
            decimal money = order.GetTotal() - order.Freight;
            if (GetPoint(money,wid) > 0)
            {
                IntegralDetailInfo point = new IntegralDetailInfo {
                    IntegralChange = order.Points,
                    IntegralSource = "购物送积分",
                    IntegralSourceType = 1,
                    IntegralStatus = 1,
                    Userid = order.UserId
                };
                new IntegralDetailDao().AddIntegralDetail(point, null);
            }
            MemberInfo member = new MemberDao().GetMember(order.UserId);
            member.Expenditure += order.GetTotal();
            member.OrderNumber++;
            dao.Update(member);
            MemberGradeInfo memberGrade = GetMemberGrade(member.GradeId);
            if (memberGrade != null)
            {
                bool flag = false;
                if (memberGrade.TranVol.HasValue)
                {
                    flag = memberGrade.TranVol.Value < double.Parse(member.Expenditure.ToString());
                }
                bool flag2 = false;
                if (memberGrade.TranTimes.HasValue)
                {
                    int? tranTimes = memberGrade.TranTimes;
                    int orderNumber = member.OrderNumber;
                    flag2 = (tranTimes.GetValueOrDefault() < orderNumber) && tranTimes.HasValue;
                }
                if (flag || flag2)
                {
                    List<MemberGradeInfo> memberGrades = new MemberGradeDao().GetMemberGrades(order.wid) as List<MemberGradeInfo>;
                    MemberGradeInfo info3 = null;
                    if (flag)
                    {
                        if (predicate == null)
                        {
                            predicate = m => ((decimal) m.TranVol.Value) <= member.Expenditure;
                        }
                        info3 = (from m in memberGrades
                            where m.TranVol.HasValue
                            orderby m.TranVol descending
                            select m).FirstOrDefault<MemberGradeInfo>(predicate);
                    }
                    MemberGradeInfo info4 = null;
                    if (flag2)
                    {
                        if (func2 == null)
                        {
                            func2 = m => m.TranTimes.Value <= member.OrderNumber;
                        }
                        info4 = (from m in memberGrades
                            where m.TranTimes.HasValue
                            orderby m.TranTimes descending
                            select m).FirstOrDefault<MemberGradeInfo>(func2);
                    }
                    else
                    {
                        info4 = info3;
                    }
                    MemberGradeInfo info5 = null;
                    if (info3 == null)
                    {
                        info3 = info4;
                    }
                    if (info3 != null)
                    {
                        double? tranVol = info3.TranVol;
                        double? nullable6 = info4.TranVol;
                        if ((tranVol.GetValueOrDefault() > nullable6.GetValueOrDefault()) && (tranVol.HasValue & nullable6.HasValue))
                        {
                            info5 = info3;
                        }
                        else
                        {
                            info5 = info4;
                        }
                        if (memberGrade.GradeId != info5.GradeId)
                        {
                            double? nullable7 = memberGrade.TranVol;
                            double? nullable8 = info5.TranVol;
                            if ((nullable7.GetValueOrDefault() <= nullable8.GetValueOrDefault()) || !(nullable7.HasValue & nullable8.HasValue))
                            {
                                member.GradeId = info5.GradeId;
                                dao.Update(member);
                            }
                        }
                    }
                }
            }
        }

        public static bool UserPayOrder(OrderInfo order,string wid)
        {
            OrderDao dao = new OrderDao();
            order.OrderStatus = OrderStatus.BuyerAlreadyPaid;
            order.PayDate = new DateTime?(DateTime.Now);
            bool flag = dao.UpdateOrder(order, null);
            string str = "";
            if (flag)
            {
                dao.UpdatePayOrderStock(order.OrderId);
                foreach (LineItemInfo info in order.LineItems.Values)
                {
                    ProductDao dao2 = new ProductDao();
                    str = str + "'" + info.SkuId + "',";
                    ProductInfo productDetails = dao2.GetProductDetails(info.ProductId);
                    productDetails.SaleCounts += info.Quantity;
                    productDetails.ShowSaleCounts += info.Quantity;
                    dao2.UpdateProduct(productDetails, null);
                }
                if (!string.IsNullOrEmpty(str))
                {
                    dao.UpdateItemsStatus(order.OrderId, 2, str.Substring(0, str.Length - 1));
                }
                if (!string.IsNullOrEmpty(order.ActivitiesId))
                {
                    new ActivitiesDao().UpdateActivitiesTakeEffect(order.ActivitiesId);
                }
                MemberInfo member = GetMember(order.UserId, true);
                if (member != null)
                {
                    Messenger.OrderPayment(member, order.OrderId, order.GetTotal(),wid);
                }
            }
            return flag;
        }
    }
}

