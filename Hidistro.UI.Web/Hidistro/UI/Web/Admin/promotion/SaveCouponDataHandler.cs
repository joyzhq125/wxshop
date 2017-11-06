namespace Hidistro.UI.Web.Admin.promotion
{
    using Hidistro.ControlPanel.Promotions;
    using Hidistro.Entities.Promotions;
    using System;
    using System.Web;

    public class SaveCouponDataHandler : IHttpHandler
    {
        private bool bDate(string val, bool flag, ref DateTime i)
        {
            if (string.IsNullOrEmpty(val))
            {
                return false;
            }
            if (!DateTime.TryParse(val, out i))
            {
                return false;
            }
            if (flag)
            {
                i = i.Date;
            }
            else
            {
                i = i.Date.AddDays(1.0).AddSeconds(-1.0);
            }
            return true;
        }

        private bool bDecimal(string val, ref decimal i)
        {
            if (string.IsNullOrEmpty(val))
            {
                return false;
            }
            i = 0M;
            if (val.Contains("-"))
            {
                return false;
            }
            return decimal.TryParse(val, out i);
        }

        private bool bInt(string val, ref int i)
        {
            if (string.IsNullOrEmpty(val))
            {
                return false;
            }
            i = 0;
            if (val.Contains(".") || val.Contains("-"))
            {
                return false;
            }
            return int.TryParse(val, out i);
        }

        private string GetErrMsg(string msg)
        {
            string str2;
            if (((str2 = msg) != null) && (str2 == "DuplicateName"))
            {
                return "优惠券名称重复";
            }
            return "写数据库异常";
        }

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            try
            {
                string str = context.Request["name"].ToString();
                string val = context.Request["val"].ToString();
                string str3 = context.Request["condition"].ToString();
                string str4 = context.Request["begin"].ToString();
                string str5 = context.Request["end"].ToString();
                string str6 = context.Request["total"].ToString();
                string str7 = context.Request["isAllMember"].ToString();
                string str8 = context.Request["memberlvl"].ToString();
                string str9 = context.Request["maxNum"].ToString();
                string str10 = context.Request["type"].ToString();
                string couponName = "";
                decimal i = 0M;
                decimal num2 = 0M;
                DateTime now = DateTime.Now;
                DateTime time2 = DateTime.Now;
                int num3 = 0;
                bool flag = false;
                string str12 = "";
                int num4 = 1;
                int num5 = 0;
                if (string.IsNullOrEmpty(str) || (str.Length > 20))
                {
                    context.Response.Write("{\"type\":\"error\",\"data\":\"请输入正确的优惠券名称\"}");
                }
                else
                {
                    couponName = str;
                    if (!this.bDecimal(val, ref i))
                    {
                        context.Response.Write("{\"type\":\"error\",\"data\":\"请输入正确的优惠券面值\"}");
                    }
                    else if (!this.bDecimal(str3, ref num2))
                    {
                        context.Response.Write("{\"type\":\"error\",\"data\":\"请输入正确的优惠券适用最大满足额\"}");
                    }
                    else if (!this.bDate(str4, true, ref now))
                    {
                        context.Response.Write("{\"type\":\"error\",\"data\":\"请输入正确的生效日期\"}");
                    }
                    else if (!this.bDate(str5, false, ref time2))
                    {
                        context.Response.Write("{\"type\":\"error\",\"data\":\"请输入正确的失效日期\"}");
                    }
                    else if (!this.bInt(str6, ref num3))
                    {
                        context.Response.Write("{\"type\":\"error\",\"data\":\"请输入正确的优惠券发放量\"}");
                    }
                    else
                    {
                        flag = bool.Parse(str7);
                        if (!flag)
                        {
                            if (string.IsNullOrEmpty(str8))
                            {
                                context.Response.Write("{\"type\":\"error\",\"data\":\"请选择正确的会员等级\"}");
                                return;
                            }
                            str12 = str8;
                        }
                        if (!this.bInt(str9, ref num4))
                        {
                            context.Response.Write("{\"type\":\"error\",\"data\":\"请选择正确的优惠券最大领取张数\"}");
                        }
                        else
                        {
                            this.bInt(str10, ref num5);
                            if ((num2 < i) && (num2 != 0M))
                            {
                                context.Response.Write("{\"type\":\"error\",\"data\":\"优惠券面值不能大于满足金额\"}");
                            }
                            else if (time2 < now)
                            {
                                context.Response.Write("{\"type\":\"error\",\"data\":\"优惠券失效日期不能早于生效日期\"}");
                            }
                            else
                            {
                                CouponInfo coupon = new CouponInfo {
                                    CouponName = couponName,
                                    CouponValue = i,
                                    ConditionValue = num2,
                                    BeginDate = now,
                                    EndDate = time2,
                                    StockNum = num3,
                                    IsAllProduct = (num5 == 0) ? true : false
                                };
                                if (flag)
                                {
                                    coupon.MemberGrades = "0";
                                }
                                else
                                {
                                    coupon.MemberGrades = str12;
                                }
                                coupon.maxReceivNum = num4;
                                CouponActionStatus status = CouponHelper.CreateCoupon(coupon);
                                if (status == CouponActionStatus.Success)
                                {
                                    coupon = CouponHelper.GetCoupon(couponName);
                                    if (coupon != null)
                                    {
                                        context.Response.Write("{\"type\":\"success\",\"data\":\"" + coupon.CouponId.ToString() + "\"}");
                                    }
                                    else
                                    {
                                        context.Response.Write("{\"type\":\"error\",\"data\":\"写数据库异常\"}");
                                    }
                                }
                                else
                                {
                                    context.Response.Write("{\"type\":\"error\",\"data\":\"" + this.GetErrMsg(status.ToString()) + "\"}");
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                context.Response.Write("{\"type\":\"error\",\"data\":\"" + exception.Message + "\"}");
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}

