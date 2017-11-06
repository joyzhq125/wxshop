namespace Hidistro.ControlPanel.Promotions
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Promotions;
    using Hidistro.SqlDal.Promotions;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Runtime.InteropServices;
    using System.Text;

    public static class CouponHelper
    {
        public static bool AddCouponProducts(int couponId, int productID)
        {
            return new CouponDao().AddCouponProducts(couponId, productID);
        }

        public static bool AddCouponProducts(int couponId, bool IsAllProduct, IList<string> productIDs)
        {
            return new CouponDao().AddCouponProducts(couponId, IsAllProduct, productIDs);
        }

        public static CouponActionStatus CreateCoupon(CouponInfo coupon)
        {
            Globals.EntityCoding(coupon, true);
            return new CouponDao().CreateCoupon(coupon);
        }

        public static CouponActionStatus CreateCoupon(CouponInfo coupon, int count, out string lotNumber)
        {
            Globals.EntityCoding(coupon, true);
            lotNumber = "";
            return new CouponDao().CreateCoupon(coupon);
        }

        public static bool DeleteCoupon(int couponId)
        {
            return new CouponDao().DeleteCoupon(couponId);
        }

        public static bool DeleteProducts(int couponId, string productIds)
        {
            return new CouponDao().DeleteProducts(couponId, productIds);
        }

        public static CouponInfo GetCoupon(int couponId)
        {
            return new CouponDao().GetCouponDetails(couponId);
        }

        public static CouponInfo GetCoupon(string couponName)
        {
            return new CouponDao().GetCouponDetails(couponName);
        }

        public static DbQueryResult GetCouponInfos(CouponsSearch search)
        {
            return new CouponDao().GetCouponInfos(search);
        }

        public static IList<CouponItemInfo> GetCouponItemInfos(string lotNumber)
        {
            return null;
        }

        public static string GetCouponProductIds(int couponId)
        {
            DataTable couponProducts = GetCouponProducts(couponId);
            StringBuilder builder = new StringBuilder();
            if (couponProducts != null)
            {
                int count = couponProducts.Rows.Count;
                for (int i = 0; i < count; i++)
                {
                    builder.Append(couponProducts.Rows[i]["ProductId"].ToString());
                    if (i != (count - 1))
                    {
                        builder.Append("_");
                    }
                }
            }
            return builder.ToString();
        }

        public static DataTable GetCouponProducts(int couponId)
        {
            return new CouponDao().GetCouponProducts(couponId);
        }

        public static DbQueryResult GetCouponsList(CouponItemInfoQuery query)
        {
            return new CouponDao().GetCouponsList(query);
        }

        public static string GetCouponsProductIds(int CouponId)
        {
            return new CouponDao().GetCouponsProductIds(CouponId);
        }

        public static DataTable GetMemberCoupons(MemberCouponsSearch search, ref int total)
        {
            return new CouponDao().GetMemberCoupons(search, ref total);
        }

        public static int GetMemberCouponsNumbyUserId(int UserId)
        {
            return new CouponDao().GetMemberCouponsNumbyUserId(UserId);
        }

        public static DataTable GetMemberWaitCoupons(int UserId, int GradeId)
        {
            return new CouponDao().GetMemberWaitCoupons(UserId, GradeId);
        }

        public static int GetMemberWaitCouponsNum(int UserId, int GradeId)
        {
            return new CouponDao().GetMemberWaitCouponsNum(UserId, GradeId);
        }

        public static DbQueryResult GetNewCoupons(Pagination page)
        {
            return new CouponDao().GetNewCoupons(page);
        }

        public static DataTable GetUnFinishedCoupon(DateTime end)
        {
            return new CouponDao().GetUnFinishedCoupon(end);
        }

        public static SendCouponResult IsCanSendCouponToMember(int couponId, int userId)
        {
            return new CouponDao().IsCanSendCouponToMember(couponId, userId);
        }

        public static void SendClaimCodes(int couponId, IList<CouponItemInfo> listCouponItem)
        {
            foreach (CouponItemInfo info in listCouponItem)
            {
                new CouponDao().SendClaimCodes(couponId, info);
            }
        }

        public static SendCouponResult SendCouponToMember(int couponId, int userId)
        {
            return new CouponDao().SendCouponToMember(couponId, userId);
        }

        public static bool setCouponFinished(int couponId, bool flag)
        {
            return new CouponDao().setCouponFinished(couponId, flag);
        }

        public static bool SetProductsStatus(int couponId, int status, string productIds)
        {
            return new CouponDao().SetProductsStatus(couponId, status, productIds);
        }

        public static CouponActionStatus UpdateCoupon(CouponInfo coupon)
        {
            return CouponActionStatus.UnknowError;
        }

        public static bool UpdateCoupon(int couponId, CouponEdit coupon, ref string msg)
        {
            Globals.EntityCoding(coupon, true);
            return new CouponDao().UpdateCoupon(couponId, coupon, ref msg);
        }
    }
}

