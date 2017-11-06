namespace Hidistro.UI.Web.Admin.promotion
{
    using Hidistro.ControlPanel.Promotions;
    using Hidistro.Entities;
    using Hidistro.Entities.Promotions;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using System;
    using System.Collections.Generic;
    using System.Web;

    public class SaveActivityHandler : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            try
            {
                int id = int.Parse(context.Request["id"].ToString());
                string str = context.Request["name"].ToString();
                string val = context.Request["begin"].ToString();
                string str3 = context.Request["end"].ToString();
                string str4 = context.Request["memberlvl"].ToString();
                string str5 = context.Request["maxNum"].ToString();
                string str6 = context.Request["type"].ToString();
                string str7 = context.Request["attendType"].ToString();
                string str8 = context.Request["meetType"].ToString();
                int i = 0;
                int num3 = 0;
                DateTime now = DateTime.Now;
                DateTime time2 = DateTime.Now;
                int num4 = 0;
                int num5 = 0;
                if (str.Length > 30)
                {
                    context.Response.Write("{\"type\":\"error\",\"data\":\"活动名称不能超过30个字符\"}");
                }
                else if (!val.bDate(ref now))
                {
                    context.Response.Write("{\"type\":\"error\",\"data\":\"请输入正确的开始时间\"}");
                }
                else if (!str3.bDate(ref time2))
                {
                    context.Response.Write("{\"type\":\"error\",\"data\":\"请输入正确的结束时间\"}");
                }
                else if (time2 < now)
                {
                    context.Response.Write("{\"type\":\"error\",\"data\":\"结束时间不能早于开始时间\"}");
                }
                else if (string.IsNullOrEmpty(str4))
                {
                    context.Response.Write("{\"type\":\"error\",\"data\":\"请选择适用会员等级\"}");
                }
                else if (!str5.bInt(ref num4))
                {
                    context.Response.Write("{\"type\":\"error\",\"data\":\"请选择参与次数\"}");
                }
                else if (!str6.bInt(ref num5))
                {
                    context.Response.Write("{\"type\":\"error\",\"data\":\"请选择参与商品类型\"}");
                }
                else if (!str7.bInt(ref i))
                {
                    context.Response.Write("{\"type\":\"error\",\"data\":\"请选择优惠类型\"}");
                }
                else if (!str8.bInt(ref num3))
                {
                    context.Response.Write("{\"type\":\"error\",\"data\":\"请选择优惠条件\"}");
                }
                else
                {
                    List<ActivityDetailInfo> list = new List<ActivityDetailInfo>();
                    JArray array = (JArray) JsonConvert.DeserializeObject(context.Request.Form["stk"].ToString());
                    if (array.Count > 1)
                    {
                        for (int j = 0; j < (array.Count - 1); j++)
                        {
                            JToken token = array[j]["meet"];
                            for (int k = j + 1; k < array.Count; k++)
                            {
                                if (array[k]["meet"] == token)
                                {
                                    context.Response.Write("{\"type\":\"error\",\"data\":\"多级优惠的各级满足金额不能相同\"}");
                                    return;
                                }
                            }
                        }
                    }
                    if (array.Count > 0)
                    {
                        for (int m = 0; m < array.Count; m++)
                        {
                            ActivityDetailInfo item = new ActivityDetailInfo();
                            string str9 = array[m]["meet"].ToString();
                            string str10 = array[m]["meetNumber"].ToString();
                            string str11 = array[m]["redus"].ToString();
                            string str12 = array[m]["free"].ToString();
                            string str13 = array[m]["point"].ToString();
                            string str14 = array[m]["coupon"].ToString();
                            decimal num9 = 0M;
                            int num10 = 0;
                            decimal num11 = 0M;
                            bool flag = false;
                            int num12 = 0;
                            int num13 = 0;
                            int num14 = 0;
                            if (!str10.bInt(ref num10))
                            {
                                int num16 = m + 1;
                                context.Response.Write("{\"type\":\"error\",\"data\":\"第" + num16.ToString() + "级优惠满足次数输入错误！\"}");
                                return;
                            }
                            if (!str9.bDecimal(ref num9))
                            {
                                int num17 = m + 1;
                                context.Response.Write("{\"type\":\"error\",\"data\":\"第" + num17.ToString() + "级优惠满足金额输入错误！\"}");
                                return;
                            }
                            if (!str11.bDecimal(ref num11))
                            {
                                int num18 = m + 1;
                                context.Response.Write("{\"type\":\"error\",\"data\":\"第" + num18.ToString() + "级优惠减免金额输入错误！\"}");
                                return;
                            }
                            if (!str12.bInt(ref num14))
                            {
                                int num19 = m + 1;
                                context.Response.Write("{\"type\":\"error\",\"data\":\"第" + num19.ToString() + "级优惠免邮选择错误！\"}");
                                return;
                            }
                            flag = num14 != 0;
                            if (!str13.bInt(ref num12))
                            {
                                int num20 = m + 1;
                                context.Response.Write("{\"type\":\"error\",\"data\":\"第" + num20.ToString() + "级优惠积分输入错误！\"}");
                                return;
                            }
                            if (!str13.bInt(ref num12))
                            {
                                int num21 = m + 1;
                                context.Response.Write("{\"type\":\"error\",\"data\":\"第" + num21.ToString() + "级优惠积分输入错误！\"}");
                                return;
                            }
                            if (!str14.bInt(ref num13))
                            {
                                int num22 = m + 1;
                                context.Response.Write("{\"type\":\"error\",\"data\":\"第" + num22.ToString() + "级优惠优惠券选择错误！\"}");
                                return;
                            }
                            if ((num10 == 0) && (num11 > num9))
                            {
                                int num23 = m + 1;
                                context.Response.Write("{\"type\":\"error\",\"data\":\"第" + num23.ToString() + "级优惠减免金额不能大于满足金额！\"}");
                                return;
                            }
                            item.ActivitiesId = 0;
                            item.bFreeShipping = flag;
                            item.CouponId = num13;
                            item.MeetMoney = num9;
                            item.MeetNumber = num10;
                            item.ReductionMoney = num11;
                            item.Integral = num12;
                            list.Add(item);
                        }
                    }
                    ActivityInfo act = new ActivityInfo();
                    if (id != 0)
                    {
                        act = ActivityHelper.GetAct(id);
                        if (act == null)
                        {
                            context.Response.Write("{\"type\":\"error\",\"data\":\"没有找到这个活动\"}");
                            return;
                        }
                    }
                    act.ActivitiesName = str;
                    act.EndTime = time2.Date.AddDays(1.0).AddSeconds(-1.0);
                    act.StartTime = now.Date;
                    act.attendTime = num4;
                    act.attendType = i;
                    act.isAllProduct = num5 == 0;
                    act.MemberGrades = str4;
                    act.Type = 0;
                    act.MeetMoney = 0M;
                    act.ReductionMoney = 0M;
                    act.Details = list;
                    act.MeetType = num3;
                    string msg = "";
                    int activitiesId = 0;
                    if (id == 0)
                    {
                        activitiesId = ActivityHelper.Create(act, ref msg);
                    }
                    else
                    {
                        activitiesId = act.ActivitiesId;
                        if (!ActivityHelper.Update(act, ref msg))
                        {
                            activitiesId = 0;
                        }
                    }
                    if (activitiesId > 0)
                    {
                        context.Response.Write("{\"type\":\"success\",\"data\":\"" + activitiesId.ToString() + "\"}");
                    }
                    else
                    {
                        context.Response.Write("{\"type\":\"error\",\"data\":\"" + msg + "\"}");
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

