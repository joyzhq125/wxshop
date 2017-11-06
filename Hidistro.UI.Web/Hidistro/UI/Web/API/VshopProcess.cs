namespace Hidistro.UI.Web.API
{
    using Hidistro.ControlPanel.Promotions;
    using Hidistro.ControlPanel.Settings;
    using Hidistro.ControlPanel.Store;
    using Hidistro.ControlPanel.VShop;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities;
    using Hidistro.Entities.Comments;
    using Hidistro.Entities.Commodities;
    using Hidistro.Entities.Members;
    using Hidistro.Entities.Orders;
    using Hidistro.Entities.Promotions;
    using Hidistro.Entities.Sales;
    using Hidistro.Entities.Settings;
    using Hidistro.Entities.StatisticsReport;
    using Hidistro.Entities.VShop;
    using Hidistro.Messages;
    using Hidistro.SaleSystem.Vshop;
    using Newtonsoft.Json.Linq;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Web;
    using System.Web.SessionState;

    public class VshopProcess : IHttpHandler, IRequiresSessionState
    {
        private int buyAmount;
        private UpdateStatistics myEvent = new UpdateStatistics();
        private StatisticNotifier myNotifier = new StatisticNotifier();
        private string productSku;
        protected string wid = string.Empty;
        private void AddCommissions(HttpContext context)
        {
            context.Response.ContentType = "text/json";
            string msg = "";
            if (!DistributorsBrower.IsExitsCommionsRequest())
            {
                if (this.CheckAddCommissions(context, ref msg))
                {
                    string str2 = context.Request["account"].Trim();
                    decimal num = decimal.Parse(context.Request["commissionmoney"].Trim());
                    int result = 0;
                    int.TryParse(context.Request["requesttype"].Trim(), out result);
                    string realName = context.Request["realname"].Trim();
                    string str4 = context.Request["bankname"].Trim();
                    BalanceDrawRequestInfo balancerequestinfo = new BalanceDrawRequestInfo
                    {
                        MerchantCode = str2,
                        Amount = num,
                        RequesType = result
                    };
                    MemberInfo currentMember = MemberProcessor.GetCurrentMember();
                    if (realName == "")
                    {
                        realName = currentMember.RealName;
                    }
                    else
                    {
                        currentMember.RealName = realName;
                    }
                    balancerequestinfo.AccountName = realName;
                    balancerequestinfo.BankName = str4;
                    if ((string.IsNullOrEmpty(currentMember.OpenId) || (currentMember.OpenId.Length < 0x1c)) && ((result == 3) || (result == 0)))
                    {
                        msg = "{\"success\":false,\"msg\":\"您的帐号未绑定，无法通过微信支付佣金！\"}";
                    }
                    else if (DistributorsBrower.AddBalanceDrawRequest(balancerequestinfo, currentMember))
                    {
                        try
                        {
                            this.myNotifier.updateAction = UpdateAction.OrderUpdate;
                            this.myNotifier.actionDesc = "申请提现";
                            this.myNotifier.RecDateUpdate = DateTime.Today;
                            this.myNotifier.DataUpdated += new StatisticNotifier.DataUpdatedEventHandler(this.myEvent.Update);
                            this.myNotifier.UpdateDB();
                        }
                        catch (Exception)
                        {
                        }
                        msg = "{\"success\":true,\"msg\":\"申请成功！\"}";
                    }
                    else
                    {
                        msg = "{\"success\":false,\"msg\":\"真实姓名或手机号未填写！\"}";
                    }
                }
            }
            else
            {
                msg = "{\"success\":false,\"msg\":\"您的申请正在审核中！\"}";
            }
            context.Response.Write(msg);
            context.Response.End();
        }

        public void AddDistributor(HttpContext context)
        {
            context.Response.ContentType = "text/json";
            StringBuilder sb = new StringBuilder();
            if (this.CheckRequestDistributors(context, sb))
            {
                DistributorsInfo distributors = new DistributorsInfo
                {
                    RequestAccount = context.Request["acctount"].Trim(),
                    StoreName = context.Request["stroename"].Trim(),
                    StoreDescription = context.Request["descriptions"].Trim(),
                    Logo = context.Request["logo"].Trim(),
                    BackImage = "",
                    CellPhone = context.Request["CellPhone"].Trim()
                };
                DistributorGradeInfo isDefaultDistributorGradeInfo = DistributorGradeBrower.GetIsDefaultDistributorGradeInfo();
                if (isDefaultDistributorGradeInfo == null)
                {
                    context.Response.Write("{\"success\":false,\"msg\":\"默认分销商等级未设置，请联系商家客服！\"}");
                }
                else
                {
                    distributors.DistriGradeId = isDefaultDistributorGradeInfo.GradeId;
                    if (!DistributorsBrower.AddDistributors(distributors))
                    {
                        context.Response.Write("{\"success\":false,\"msg\":\"店铺名称已存在，请重新输入店铺名称\"}");
                    }
                    else
                    {
                        if (HttpContext.Current.Request.Cookies["Vshop-Member"] != null)
                        {
                            string name = "Vshop-ReferralId";
                            HttpContext.Current.Response.Cookies[name].Expires = DateTime.Now.AddDays(-1.0);
                            HttpCookie cookie = new HttpCookie(name)
                            {
                                Value = Globals.GetCurrentMemberUserId().ToString(),
                                Expires = DateTime.Now.AddDays(10)
                            };
                            HttpContext.Current.Response.Cookies.Add(cookie);
                        }
                        this.AutoAddDistributorProducts(Globals.GetCurrentMemberUserId());
                        this.myNotifier.updateAction = UpdateAction.MemberUpdate;
                        this.myNotifier.actionDesc = "会员申请成为分销商";
                        this.myNotifier.RecDateUpdate = DateTime.Today;
                        this.myNotifier.DataUpdated += new StatisticNotifier.DataUpdatedEventHandler(this.myEvent.Update);
                        this.myNotifier.UpdateDB();
                        context.Response.Write("{\"success\":true}");
                        try
                        {
                            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
                            Messenger.DistributorRegister(distributors, MemberProcessor.GetCurrentMember(), isDefaultDistributorGradeInfo.Name, currentMember.wid);
                        }
                        catch (Exception)
                        {
                        }
                    }
                }
            }
            else
            {
                context.Response.Write("{\"success\":false,\"msg\":\"" + sb.ToString() + "\"}");
            }
        }

        private void AddDistributorProducts(HttpContext context)
        {
            if (!string.IsNullOrEmpty(context.Request["Params"]))
            {
                string str = context.Request["Params"];
                JObject obj2 = JObject.Parse(str);
                if (obj2.Count > 0)
                {
                    IEnumerable<JToken> sss=Extensions.Values(obj2);
                    DistributorsBrower.AddDistributorProductId((from s in sss select (int)s).ToList<int>());
                }
            }
            context.Response.Write("{\"success\":\"true\",\"msg\":\"保存成功\"}");
            context.Response.End();
        }

        private void AddFavorite(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            if (currentMember == null)
            {
                context.Response.Write("{\"success\":false, \"msg\":\"请先登录才可以收藏商品\"}");
            }
            else if (ProductBrowser.AddProductToFavorite(Convert.ToInt32(context.Request["ProductId"]), currentMember.UserId,this.wid))
            {
                context.Response.Write("{\"success\":true}");
            }
            else
            {
                context.Response.Write("{\"success\":false, \"msg\":\"提交失败\"}");
            }
        }

        private void AddProductConsultations(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            ProductConsultationInfo productConsultation = new ProductConsultationInfo
            {
                ConsultationDate = DateTime.Now,
                ConsultationText = context.Request["ConsultationText"],
                ProductId = Convert.ToInt32(context.Request["ProductId"]),
                UserEmail = currentMember.Email,
                UserId = currentMember.UserId,
                UserName = currentMember.UserName,
                wid = this.wid
            };
            if (ProductBrowser.InsertProductConsultation(productConsultation))
            {
                context.Response.Write("{\"success\":true}");
            }
            else
            {
                context.Response.Write("{\"success\":false, \"msg\":\"提交失败\"}");
            }
        }

        private void AddProductReview(HttpContext context)
        {
            int num2;
            int num3;
            context.Response.ContentType = "application/json";
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            int productId = Convert.ToInt32(context.Request["ProductId"]);
            ProductBrowser.LoadProductReview(productId, currentMember.UserId, out num2, out num3);
            if (num2 == 0)
            {
                context.Response.Write("{\"success\":false, \"msg\":\"您没有购买此商品(或此商品的订单尚未完成)，因此不能进行评论\"}");
            }
            else if (num3 >= num2)
            {
                context.Response.Write("{\"success\":false, \"msg\":\"您已经对此商品进行过评论(或此商品的订单尚未完成)，因此不能再次进行评论\"}");
            }
            else
            {
                ProductReviewInfo review = new ProductReviewInfo
                {
                    ReviewDate = DateTime.Now,
                    ReviewText = context.Request["ReviewText"],
                    ProductId = productId,
                    UserEmail = currentMember.Email,
                    UserId = currentMember.UserId,
                    UserName = currentMember.UserName,
                    wid= currentMember.wid
                };
                if (ProductBrowser.InsertProductReview(review))
                {
                    context.Response.Write("{\"success\":true}");
                }
                else
                {
                    context.Response.Write("{\"success\":false, \"msg\":\"提交失败\"}");
                }
            }
        }

        private void AddShippingAddress(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            if (currentMember == null)
            {
                context.Response.Write("{\"success\":false}");
            }
            else
            {
                ShippingAddressInfo shippingAddress = new ShippingAddressInfo
                {
                    Address = context.Request.Form["address"],
                    CellPhone = context.Request.Form["cellphone"],
                    ShipTo = context.Request.Form["shipTo"],
                    Zipcode = "12345",
                    IsDefault = true,
                    UserId = currentMember.UserId,
                    RegionId = Convert.ToInt32(context.Request.Form["regionSelectorValue"]),
                    wid = this.wid
                };
                if (MemberProcessor.AddShippingAddress(shippingAddress) > 0)
                {
                    context.Response.Write("{\"success\":true}");
                }
                else
                {
                    context.Response.Write("{\"success\":false}");
                }
            }
        }

        private void AddSignUp(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            int activityid = Convert.ToInt32(context.Request["id"]);
            string str = Convert.ToString(context.Request["code"]);
            LotteryTicketInfo lotteryTicket = VshopBrowser.GetLotteryTicket(activityid);
            if (!string.IsNullOrEmpty(lotteryTicket.InvitationCode) && (lotteryTicket.InvitationCode != str))
            {
                context.Response.Write("{\"success\":false, \"msg\":\"邀请码不正确\"}");
            }
            else if (lotteryTicket.EndTime < DateTime.Now)
            {
                context.Response.Write("{\"success\":false, \"msg\":\"活动已结束\"}");
            }
            else if (lotteryTicket.OpenTime < DateTime.Now)
            {
                context.Response.Write("{\"success\":false, \"msg\":\"报名已结束\"}");
            }
            else if (VshopBrowser.GetUserPrizeRecord(activityid) == null)
            {
                MemberInfo currentMember = MemberProcessor.GetCurrentMember();
                if ((currentMember != null) && !lotteryTicket.GradeIds.Contains(currentMember.GradeId.ToString()))
                {
                    context.Response.Write("{\"success\":false, \"msg\":\"您的会员等级不在此活动范内\"}");
                }
                else
                {
                    PrizeRecordInfo model = new PrizeRecordInfo
                    {
                        ActivityID = activityid,
                        UserID = currentMember.UserId,
                        UserName = currentMember.UserName,
                        IsPrize = true,
                        Prizelevel = "已报名",
                        PrizeTime = new DateTime?(DateTime.Now)
                    };
                    VshopBrowser.AddPrizeRecord(model);
                    context.Response.Write("{\"success\":true, \"msg\":\"报名成功\"}");
                }
            }
            else
            {
                context.Response.Write("{\"success\":false, \"msg\":\"你已经报名了，请不要重复报名！\"}");
            }
        }

        private void AddTicket(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            int activityid = Convert.ToInt32(context.Request["activityid"]);
            LotteryTicketInfo lotteryTicket = VshopBrowser.GetLotteryTicket(activityid);
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            if ((currentMember != null) && !lotteryTicket.GradeIds.Contains(currentMember.GradeId.ToString()))
            {
                context.Response.Write("{\"success\":false, \"msg\":\"您的会员等级不在此活动范内\"}");
            }
            else if (lotteryTicket.EndTime < DateTime.Now)
            {
                context.Response.Write("{\"success\":false, \"msg\":\"活动已结束\"}");
            }
            else if (DateTime.Now < lotteryTicket.OpenTime)
            {
                context.Response.Write("{\"success\":false, \"msg\":\"抽奖还未开始\"}");
            }
            else if (VshopBrowser.GetCountBySignUp(activityid) < lotteryTicket.MinValue)
            {
                context.Response.Write("{\"success\":false, \"msg\":\"还未达到人数下限\"}");
            }
            else
            {
                PrizeRecordInfo userPrizeRecord = VshopBrowser.GetUserPrizeRecord(activityid);
                try
                {
                    if (!lotteryTicket.IsOpened)
                    {
                        VshopBrowser.OpenTicket(activityid);
                        userPrizeRecord = VshopBrowser.GetUserPrizeRecord(activityid);
                    }
                    else if (!string.IsNullOrWhiteSpace(userPrizeRecord.RealName) && !string.IsNullOrWhiteSpace(userPrizeRecord.CellPhone))
                    {
                        context.Response.Write("{\"success\":false, \"msg\":\"您已经抽过奖了\"}");
                        return;
                    }
                    if ((userPrizeRecord == null) || string.IsNullOrEmpty(userPrizeRecord.PrizeName))
                    {
                        context.Response.Write("{\"success\":false, \"msg\":\"很可惜,你未中奖\"}");
                        return;
                    }
                    if (!userPrizeRecord.PrizeTime.HasValue)
                    {
                        userPrizeRecord.PrizeTime = new DateTime?(DateTime.Now);
                        VshopBrowser.UpdatePrizeRecord(userPrizeRecord);
                    }
                }
                catch (Exception exception)
                {
                    context.Response.Write("{\"success\":false, \"msg\":\"" + exception.Message + "\"}");
                    return;
                }
                context.Response.Write("{\"success\":true, \"msg\":\"恭喜你获得" + userPrizeRecord.Prizelevel + "\"}");
            }
        }

        private void AddUserPrize(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            int result = 1;
            int.TryParse(context.Request["activityid"], out result);
            string str = context.Request["prize"];
            LotteryActivityInfo lotteryActivity = VshopBrowser.GetLotteryActivity(result);
            PrizeRecordInfo model = new PrizeRecordInfo
            {
                PrizeTime = new DateTime?(DateTime.Now),
                UserID = Globals.GetCurrentMemberUserId(),
                ActivityName = lotteryActivity.ActivityName,
                ActivityID = result,
                Prizelevel = str
            };
            switch (str)
            {
                case "一等奖":
                    model.PrizeName = lotteryActivity.PrizeSettingList[0].PrizeName;
                    model.IsPrize = true;
                    break;

                case "二等奖":
                    model.PrizeName = model.PrizeName = lotteryActivity.PrizeSettingList[1].PrizeName;
                    model.IsPrize = true;
                    break;

                case "三等奖":
                    model.PrizeName = lotteryActivity.PrizeSettingList[2].PrizeName;
                    model.IsPrize = true;
                    break;

                case "四等奖":
                    model.PrizeName = lotteryActivity.PrizeSettingList[3].PrizeName;
                    model.IsPrize = true;
                    break;

                case "五等奖":
                    model.PrizeName = lotteryActivity.PrizeSettingList[4].PrizeName;
                    model.IsPrize = true;
                    break;

                case "六等奖":
                    model.PrizeName = lotteryActivity.PrizeSettingList[5].PrizeName;
                    model.IsPrize = true;
                    break;

                default:
                    model.IsPrize = false;
                    break;
            }
            VshopBrowser.AddPrizeRecord(model);
            StringBuilder builder = new StringBuilder();
            builder.Append("{");
            builder.Append("\"Status\":\"OK\"");
            builder.Append("}");
            context.Response.Write(builder);
        }

        private void AdjustCommissions(HttpContext context)
        {
            context.Response.ContentType = "text/json";
            string msg = "";
            if (this.CheckAjustCommissions(context, ref msg))
            {
                decimal result = 0M;
                decimal num2 = 0M;
                decimal.TryParse(context.Request["adjustprice"], out result);
                decimal.TryParse(context.Request["commssionprice"], out num2);
                if (ShoppingProcessor.UpdateAdjustCommssions(context.Request["orderId"], context.Request["skuId"], num2, result))
                {
                    msg = "{\"success\":true,\"msg\":\"修改金额成功！\"}";
                }
                else
                {
                    msg = "{\"success\":false,\"msg\":\"优惠金额修改失败！\"}";
                }
            }
            context.Response.Write(msg);
            context.Response.End();
        }

        private bool AutoAddDistributorProducts(int UserId)
        {
            int total = 0;
            List<int> productList = new List<int>();
            foreach (DataRow row in ProductBrowser.GetProducts(MemberProcessor.GetCurrentMember(), null, 0, "", 1, 0x2710, out total, "DisplaySequence", "desc", false).Rows)
            {
                int item = (int)row["ProductId"];
                productList.Add(item);
            }
            if (productList.Count > 0)
            {
                DistributorsBrower.AddDistributorProductId(productList);
                return true;
            }
            return false;
        }

        public void BindOldUserName(HttpContext context)
        {
            //string wid = string.Empty;
            context.Response.ContentType = "application/json";
            MemberInfo usernameMember = new MemberInfo();
            string str = context.Request["userName"];
            string sourceData = context.Request["password"];
            StringBuilder builder = new StringBuilder();
            builder.Append("{");
            if (!string.IsNullOrEmpty(str))
            {
                usernameMember = MemberProcessor.GetusernameMember(str, wid);
                if (usernameMember == null)
                {
                    builder.Append("\"Status\":\"-1\"");
                    builder.Append("}");
                    context.Response.Write(builder.ToString());
                    return;
                }
                if (usernameMember.Status != Convert.ToInt32(UserStatus.DEL))
                {
                    if (usernameMember.Password == HiCryptographer.Md5Encrypt(sourceData))
                    {
                        MemberInfo currentMember = MemberProcessor.GetCurrentMember();
                        if (currentMember.UserId != usernameMember.UserId)
                        {
                            if (currentMember.ReferralUserId == usernameMember.ReferralUserId)
                            {
                                DistributorsInfo distributorInfo = DistributorsBrower.GetDistributorInfo(currentMember.UserId);
                                DistributorsInfo info4 = DistributorsBrower.GetDistributorInfo(usernameMember.UserId);
                                int userOrders = ShoppingProcessor.GetUserOrders(currentMember.UserId);
                                ShoppingProcessor.GetUserOrders(usernameMember.UserId);
                                if ((info4 == null) && (distributorInfo == null))
                                {
                                    if (userOrders == 0)
                                    {
                                        if (MemberProcessor.DelUserMessage(currentMember.UserId, currentMember.OpenId, currentMember.UserHead, usernameMember.UserId))
                                        {
                                            HiCache.Remove(string.Format("DataCache-Member-{0}", currentMember.UserId));
                                            builder.Append(this.resultstring(usernameMember.UserId, context));
                                        }
                                        else
                                        {
                                            builder.Append("\"Status\":\"-4\",\"Msg\":\"删除会员信息失败！\"");
                                        }
                                    }
                                    else
                                    {
                                        builder.Append("\"Status\":\"-4\",\"Msg\":\"当前的登录帐号已产生订单，不能合并！\"");
                                    }
                                }
                                else if ((info4 != null) && (distributorInfo == null))
                                {
                                    if (userOrders == 0)
                                    {
                                        if (MemberProcessor.DelUserMessage(currentMember.UserId, currentMember.OpenId, currentMember.UserHead, usernameMember.UserId))
                                        {
                                            HiCache.Remove(string.Format("DataCache-Member-{0}", currentMember.UserId));
                                            builder.Append(this.resultstring(usernameMember.UserId, context));
                                        }
                                        else
                                        {
                                            builder.Append("\"Status\":\"-4\",\"Msg\":\"删除会员信息失败！\"");
                                        }
                                    }
                                    else
                                    {
                                        builder.Append("\"Status\":\"-4\",\"Msg\":\"会员帐号已产生订单，帐号不能合并！\"");
                                    }
                                }
                                else
                                {
                                    builder.Append("\"Status\":\"-4\",\"Msg\":\"当前分销商不能合并！\"");
                                }
                            }
                            else
                            {
                                builder.Append("\"Status\":\"-4\",\"Msg\":\"两个帐号不属于同一上级！\"");
                            }
                        }
                        else
                        {
                            builder.Append("\"Status\":\"-4\",\"Msg\":\"不能绑定相同帐号！\"");
                        }
                    }
                    else
                    {
                        builder.Append("\"Status\":\"-2\"");
                    }
                }
                else
                {
                    builder.Append("\"Status\":\"-4\",\"Msg\":\"您的帐号在系统中已删除，不能绑定！\"");
                }
            }
            else
            {
                builder.Append("\"Status\":\"-3\"");
            }
            builder.Append("}");
            context.Response.Write(builder.ToString());
        }

        public void BindUserName(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            string userName = context.Request["userName"];
            string password = context.Request["password"];
            string passagain = context.Request["passagain"];
            StringBuilder builder = new StringBuilder();
            builder.Append("{");
            string str4 = this.BindUserNameRegist(userName, password, passagain, context);
            builder.Append(str4);
            builder.Append("}");
            context.Response.Write(builder.ToString());
        }

        public string BindUserNameRegist(string userName, string password, string passagain, HttpContext context)
        {
            if (!(password == passagain))
            {
                return "\"Status\":\"-2\"";
            }
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            MemberInfo info2 = new MemberInfo();
            if (string.IsNullOrEmpty(userName))
            {
                return "\"Status\":\"-1\"";
            }
            if (MemberProcessor.GetBindusernameMember(userName, currentMember.wid) != null)
            {
                return "\"Status\":\"-1\"";
            }
            if (MemberProcessor.BindUserName(currentMember.UserId, userName, HiCryptographer.Md5Encrypt(password)))
            {
                return "\"Status\":\"OK\"";
            }
            return "\"Status\":\"-3\"";
        }

        private bool CheckAddCommissions(HttpContext context, ref string msg)
        {
            int result = 0;
            if (!int.TryParse(context.Request["requesttype"], out result))
            {
                result = 1;
            }
            string str = context.Request["bankname"].Trim();
            string str2 = context.Request["account"];
            if (((result == 1) && !Globals.CheckReg(str2, @"^1\d{10}$")) && !Globals.CheckReg(str2, @"^(\w-*\.*)+@(\w-?)+(\.\w{2,})+$"))
            {
                msg = "{\"success\":false,\"msg\":\"支付宝账号格式不正确！\"}";
                return false;
            }
            if ((result == 2) && !Globals.CheckReg(str2, @"^(\d{16}|\d{19})$"))
            {
                msg = "{\"success\":false,\"msg\":\"银行帐号格式不正确！\"}";
                return false;
            }
            if ((result == 2) && (str.Length < 2))
            {
                msg = "{\"success\":false,\"msg\":\"开户行不正确！\"}";
                return false;
            }
            if (string.IsNullOrEmpty(context.Request["commissionmoney"].Trim()))
            {
                msg = "{\"success\":false,\"msg\":\"提现金额不允许为空！\"}";
                return false;
            }
            if (decimal.Parse(context.Request["commissionmoney"].Trim()) <= 0M)
            {
                msg = "{\"success\":false,\"msg\":\"提现金额必须大于0的纯数字！\"}";
                return false;
            }
            Regex regex = new Regex("^[0-9]*[1-9][0-9]*$");
            if (!regex.IsMatch(context.Request["commissionmoney"].Trim()))
            {
                msg = "{\"success\":false,\"msg\":\"请输入正整数！\"}";
                return false;
            }
            decimal num2 = 0M;
            decimal.TryParse(SettingsManager.GetMasterSettings(false,this.wid).MentionNowMoney, out num2);
            if ((num2 > 0M) && (decimal.Parse(context.Request["commissionmoney"].Trim()) < 0M))
            {
                msg = "{\"success\":false,\"msg\":\"提现金额必须大于等于" + num2.ToString() + "元！\"}";
                return false;
            }
            DistributorsInfo currentDistributors = DistributorsBrower.GetCurrentDistributors();
            if (decimal.Parse(context.Request["commissionmoney"].Trim()) > currentDistributors.ReferralBlance)
            {
                msg = "{\"success\":false,\"msg\":\"提现金额必须为小于现有佣金余额！\"}";
                return false;
            }
            return true;
        }

        private bool CheckAjustCommissions(HttpContext context, ref string msg)
        {
            if (string.IsNullOrEmpty(context.Request["orderId"]))
            {
                msg = "{\"success\":false,\"msg\":\"订单号不允许为空！\"}";
                return false;
            }
            if (string.IsNullOrEmpty(context.Request["skuId"]))
            {
                msg = "{\"success\":false,\"msg\":\"商品规格不允许为空！\"}";
                return false;
            }
            if (string.IsNullOrEmpty(context.Request["adjustprice"]))
            {
                msg = "{\"success\":false,\"msg\":\"请输入要调整的价格！\"}";
                return false;
            }
            if (string.IsNullOrEmpty(context.Request["commssionprice"]))
            {
                msg = "{\"success\":false,\"msg\":\"佣金金额值不对！\"}";
                return false;
            }
            if ((Convert.ToDecimal(context.Request["adjustprice"]) >= 0M) && (Convert.ToDecimal(context.Request["ajustprice"]) <= Convert.ToDecimal(context.Request["commssionprice"])))
            {
                return true;
            }
            msg = "{\"success\":false,\"msg\":\"输入金额必须在0~" + context.Request["commssionprice"].ToString() + "之间！\"}";
            return false;
        }

        public void CheckCoupon(HttpContext context)
        {
            string str = context.Request["CouponId"];
            if (string.IsNullOrEmpty(str))
            {
                context.Response.Write("{\"status\":\"N\",\"tips\":\"参数错误1\"}");
            }
            else
            {
                int result = 0;
                if (int.TryParse(str, out result))
                {
                    context.Response.Write("{\"status\":\"Y\",\"tips\":\"" + CouponHelper.GetCouponsProductIds(result) + "\"}");
                }
                else
                {
                    context.Response.Write("{\"status\":\"N\",\"tips\":\"参数错误2\"}");
                }
            }
        }

        public void checkdistribution(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            ShoppingCartInfo shoppingCart = null;
            StringBuilder builder = new StringBuilder();
            if ((int.TryParse(context.Request["buyAmount"], out this.buyAmount) && !string.IsNullOrEmpty(context.Request["from"])) && (context.Request["from"] == "signBuy"))
            {
                this.productSku = context.Request["productSku"];
                shoppingCart = ShoppingCartProcessor.GetShoppingCart(this.productSku, this.buyAmount);
            }
            else
            {
                shoppingCart = ShoppingCartProcessor.GetShoppingCart();
            }
            string regionId = context.Request["city"];
            string str2 = "";
            foreach (ShoppingCartItemInfo info2 in shoppingCart.LineItems)
            {
                if (info2.FreightTemplateId > 0)
                {
                    str2 = str2 + info2.FreightTemplateId + ",";
                }
            }
            if (!string.IsNullOrEmpty(str2))
            {
                DataTable specifyRegionGroupsModeId = SettingsHelper.GetSpecifyRegionGroupsModeId(str2.Substring(0, str2.Length - 1), regionId);
                StringBuilder builder2 = new StringBuilder();
                builder2.AppendLine(" <button type=\"button\" class=\"btn btn-default dropdown-toggle\" data-toggle=\"dropdown\">请选择配送方式<span class=\"caret\"></span></button>");
                builder2.AppendLine("<ul id=\"shippingTypeUl\" class=\"dropdown-menu\" role=\"menu\">");
                if (specifyRegionGroupsModeId.Rows.Count > 0)
                {
                    for (int i = 0; i < specifyRegionGroupsModeId.Rows.Count; i++)
                    {
                        string str3 = this.getModelType(int.Parse(specifyRegionGroupsModeId.Rows[i]["ModeId"].ToString()));
                        builder2.AppendFormat(string.Concat(new object[] { "<li><a href=\"#\" name=\"", specifyRegionGroupsModeId.Rows[i]["ModeId"], "\" value=\"", specifyRegionGroupsModeId.Rows[i]["ModeId"], "\">", str3, "</a></li>" }), new object[0]);
                    }
                }
                else
                {
                    builder2.AppendFormat("<li><a href=\"#\" name=\"0\" value=\"0\">包邮</a></li>", new object[0]);
                }
                builder2.AppendLine("</ul>");
                builder.Append(builder2.ToString() ?? "");
            }
            else
            {
                StringBuilder builder3 = new StringBuilder();
                builder3.AppendLine(" <button type='button' class='btn btn-default dropdown-toggle' data-toggle='dropdown'>请选择配送方式<span class='caret'></span></button>");
                builder3.AppendLine("<ul id='shippingTypeUl' class='dropdown-menu' role='menu'>");
                builder3.AppendFormat("<li><a href='#' name='0' value='0'>包邮</a></li>", new object[0]);
                builder3.AppendLine("</ul>");
                builder.Append(builder3.ToString() ?? "");
            }
            context.Response.Write(builder.ToString());
        }

        private void CheckFavorite(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            if (currentMember == null)
            {
                context.Response.Write("{\"success\":false}");
            }
            else if (ProductBrowser.ExistsProduct(Convert.ToInt32(context.Request["ProductId"]), currentMember.UserId))
            {
                context.Response.Write("{\"success\":true}");
            }
            else
            {
                context.Response.Write("{\"success\":false}");
            }
        }

        private bool CheckRequestDistributors(HttpContext context, StringBuilder sb)
        {
            if (string.IsNullOrEmpty(context.Request["stroename"]))
            {
                sb.AppendFormat("请输入店铺名称", new object[0]);
                return false;
            }
            if (context.Request["stroename"].Length > 20)
            {
                sb.AppendFormat("请输入店铺名称字符不多于20个字符", new object[0]);
                return false;
            }
            if (!string.IsNullOrEmpty(context.Request["descriptions"]) && (context.Request["descriptions"].Trim().Length > 30))
            {
                sb.AppendFormat("店铺描述字不能多于30个字", new object[0]);
                return false;
            }
            return true;
        }

        private bool CheckUpdateDistributors(HttpContext context, StringBuilder sb)
        {
            if (string.IsNullOrEmpty(context.Request["stroename"]))
            {
                sb.AppendFormat("请输入店铺名称", new object[0]);
                return false;
            }
            if (context.Request["stroename"].Length > 20)
            {
                sb.AppendFormat("请输入店铺名称字符不多于20个字符", new object[0]);
                return false;
            }
            if (!string.IsNullOrEmpty(context.Request["descriptions"]) && (context.Request["descriptions"].Trim().Length > 30))
            {
                sb.AppendFormat("店铺描述字不能多于30个字", new object[0]);
                return false;
            }
            return true;
        }

        public void ConfirmPrizeAddr(HttpContext context)
        {
            string str = context.Request["selAddr"];
            string str2 = "";
            string str3 = "";
            string s = "";
            string str5 = context.Request["LogId"];
            string str6 = context.Request["pid"];
            if (string.IsNullOrEmpty(str) || (str.Length < 2))
            {
                context.Response.Write("收货人不能为空");
            }
            else
            {
                string[] strArray = str.Split(new char[] { '|' });
                if (strArray.Length < 4)
                {
                    context.Response.Write("地址信息不完整！");
                }
                else
                {
                    str = strArray[0];
                    str2 = strArray[1];
                    str3 = strArray[3];
                    s = strArray[2];
                    if (strArray.Length < 4)
                    {
                        context.Response.Write("地址信息不完整！");
                    }
                    else if (str2.Length < 8)
                    {
                        context.Response.Write("联系电话不正确");
                    }
                    else if (str3.Length < 6)
                    {
                        context.Response.Write("地址不够详细");
                    }
                    else
                    {
                        int result = 0;
                        if (!int.TryParse(s, out result))
                        {
                            context.Response.Write("省市信息不正确！");
                        }
                        else
                        {
                            s = RegionHelper.GetFullPath(result);
                            PrizesDeliveQuery query = new PrizesDeliveQuery
                            {
                                Status = 1,
                                ReggionPath = s,
                                Address = str3,
                                Tel = str2,
                                Receiver = str,
                                LogId = str5
                            };
                            int num2 = 0;
                            int.TryParse(str6.Trim(), out num2);
                            query.Id = num2;
                            if (GameHelper.UpdatePrizesDelivery(query))
                            {
                                context.Response.Write("success");
                            }
                            else
                            {
                                context.Response.Write("保存信息失败");
                            }
                        }
                    }
                }
            }
        }

        public void ConfirmPrizeArriver(HttpContext context)
        {
            string s = context.Request["pid"];
            string str2 = context.Request["LogId"];
            if (s == "")
            {
                context.Response.Write("pID为空，请检查！");
            }
            else if (str2 == "")
            {
                context.Response.Write("logID为空，请检查！");
            }
            else
            {
                int result = 0;
                if (!int.TryParse(s, out result))
                {
                    context.Response.Write("当前状态下不允许操作！");
                }
                else
                {
                    PrizesDeliveQuery query = new PrizesDeliveQuery
                    {
                        Status = 3,
                        ReceiveTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                        Id = result,
                        LogId = str2
                    };
                    if (GameHelper.UpdatePrizesDelivery(query))
                    {
                        context.Response.Write("success");
                    }
                    else
                    {
                        context.Response.Write("收货确认失败");
                    }
                }
            }
        }

        public void countfreight(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            string s = context.Request["id"];
            int result = 0;
            StringBuilder builder = new StringBuilder();
            builder.Append("{");
            string city = "";
            if (int.TryParse(s, out result))
            {
                city = RegionHelper.GetCity(result);
                if (city != "0")
                {
                    builder.Append("\"Status\":\"OK\",\"Msg\":\"" + city + "\"");
                }
            }
            builder.Append("}");
            context.Response.Write(builder.ToString());
        }

        public void countfreighttype(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            StringBuilder builder = new StringBuilder();
            builder.Append("{");
            ShoppingCartInfo shoppingCart = null;
            if ((int.TryParse(context.Request["buyAmount"], out this.buyAmount) && !string.IsNullOrEmpty(context.Request["from"])) && (context.Request["from"] == "signBuy"))
            {
                this.productSku = context.Request["productSku"];
                shoppingCart = ShoppingCartProcessor.GetShoppingCart(this.productSku, this.buyAmount);
            }
            else
            {
                int result = 0;
                if (!string.IsNullOrEmpty(context.Request["TemplateId"]) && int.TryParse(context.Request["TemplateId"], out result))
                {
                    shoppingCart = ShoppingCartProcessor.GetShoppingCart(result);
                }
            }
            DataView defaultView = new DataView();
            if (shoppingCart != null)
            {
                defaultView = SettingsHelper.GetAllFreightItems(wid).DefaultView;
            }
            float num2 = 0f;
            if (defaultView.Count > 0)
            {
                foreach (ShoppingCartItemInfo info2 in shoppingCart.LineItems)
                {
                    string str2;
                    if (!info2.IsfreeShipping)
                    {
                        bool flag = false;
                        FreightTemplate templateMessage = SettingsHelper.GetTemplateMessage(info2.FreightTemplateId);
                        if (((templateMessage != null) && (info2.FreightTemplateId > 0)) && !templateMessage.FreeShip)
                        {
                            if (templateMessage.HasFree)
                            {
                                flag = this.IsFreeTemplateShipping(context.Request["RegionId"], info2.FreightTemplateId, int.Parse(context.Request["ModeId"]), info2);
                            }
                            if (!flag)
                            {
                                defaultView.RowFilter = string.Concat(new object[] { " RegionId=", context.Request["RegionId"], " and ModeId=", context.Request["ModeId"], " and TemplateId=", info2.FreightTemplateId, " and IsDefault=0" });
                                if (defaultView.Count != 1)
                                {
                                    goto Label_0479;
                                }
                                string str = defaultView[0]["MUnit"].ToString();
                                if (str != null)
                                {
                                    if (!(str == "1"))
                                    {
                                        if (str == "2")
                                        {
                                            goto Label_02FD;
                                        }
                                        if (str == "3")
                                        {
                                            goto Label_03BB;
                                        }
                                    }
                                    else
                                    {
                                        num2 += this.setferight(float.Parse(info2.Quantity.ToString()), float.Parse(defaultView[0]["FristNumber"].ToString()), float.Parse(defaultView[0]["FristPrice"].ToString()), float.Parse(defaultView[0]["AddNumber"].ToString()), float.Parse(defaultView[0]["AddPrice"].ToString()));
                                    }
                                }
                            }
                        }
                    }
                    continue;
                Label_02FD:
                    if (info2.FreightWeight > 0M)
                    {
                        num2 += this.setferight(float.Parse(info2.FreightWeight.ToString()) * float.Parse(info2.Quantity.ToString()), float.Parse(defaultView[0]["FristNumber"].ToString()), float.Parse(defaultView[0]["FristPrice"].ToString()), float.Parse(defaultView[0]["AddNumber"].ToString()), float.Parse(defaultView[0]["AddPrice"].ToString()));
                    }
                    continue;
                Label_03BB:
                    if (info2.CubicMeter > 0M)
                    {
                        num2 += this.setferight(float.Parse(info2.CubicMeter.ToString()) * float.Parse(info2.Quantity.ToString()), float.Parse(defaultView[0]["FristNumber"].ToString()), float.Parse(defaultView[0]["FristPrice"].ToString()), float.Parse(defaultView[0]["AddNumber"].ToString()), float.Parse(defaultView[0]["AddPrice"].ToString()));
                    }
                    continue;
                Label_0479: ;
                    defaultView.RowFilter = string.Concat(new object[] { "  ModeId=", context.Request["ModeId"], " and TemplateId=", info2.FreightTemplateId, " and  IsDefault=1" });
                    if ((defaultView.Count == 1) && ((str2 = defaultView[0]["MUnit"].ToString()) != null))
                    {
                        if (!(str2 == "1"))
                        {
                            if (str2 == "2")
                            {
                                goto Label_05BD;
                            }
                            if (str2 == "3")
                            {
                                goto Label_067B;
                            }
                        }
                        else
                        {
                            num2 += this.setferight(float.Parse(info2.Quantity.ToString()), float.Parse(defaultView[0]["FristNumber"].ToString()), float.Parse(defaultView[0]["FristPrice"].ToString()), float.Parse(defaultView[0]["AddNumber"].ToString()), float.Parse(defaultView[0]["AddPrice"].ToString()));
                        }
                    }
                    continue;
                Label_05BD:
                    if (info2.FreightWeight > 0M)
                    {
                        num2 += this.setferight(float.Parse(info2.FreightWeight.ToString()) * float.Parse(info2.Quantity.ToString()), float.Parse(defaultView[0]["FristNumber"].ToString()), float.Parse(defaultView[0]["FristPrice"].ToString()), float.Parse(defaultView[0]["AddNumber"].ToString()), float.Parse(defaultView[0]["AddPrice"].ToString()));
                    }
                    continue;
                Label_067B:
                    if (info2.CubicMeter > 0M)
                    {
                        num2 += this.setferight(float.Parse(info2.CubicMeter.ToString()) * float.Parse(info2.Quantity.ToString()), float.Parse(defaultView[0]["FristNumber"].ToString()), float.Parse(defaultView[0]["FristPrice"].ToString()), float.Parse(defaultView[0]["AddNumber"].ToString()), float.Parse(defaultView[0]["AddPrice"].ToString()));
                    }
                }
            }
            builder.Append("\"Status\":\"OK\",\"CountFeright\":\"" + num2.ToString("F2") + "\"");
            builder.Append("}");
            context.Response.Write(builder.ToString());
        }

        private void DeleteDistributorProducts(HttpContext context)
        {
            if (!string.IsNullOrEmpty(context.Request["Params"]))
            {
                string text = context.Request["Params"];
                JObject jObject = JObject.Parse(text);
                if (jObject.Count > 0)
                {

                    IEnumerable<JToken> sss = Extensions.Values(jObject);
                    System.Collections.Generic.List<int> productList = (from s in sss select (int)s).ToList<int>();

                    DistributorsBrower.DeleteDistributorProductIds(productList);
                }
            }
            context.Response.Write("{\"success\":\"true\",\"msg\":\"保存成功\"}");
            context.Response.End();
        }

        private void DelFavorite(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            if (ProductBrowser.DeleteFavorite(Convert.ToInt32(context.Request["favoriteId"])) == 1)
            {
                context.Response.Write("{\"success\":true}");
            }
            else
            {
                context.Response.Write("{\"success\":false, \"msg\":\"取消失败\"}");
            }
        }

        private void DelShippingAddress(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            if (currentMember == null)
            {
                context.Response.Write("{\"success\":false}");
            }
            else
            {
                int userId = currentMember.UserId;
                if (MemberProcessor.DelShippingAddress(Convert.ToInt32(context.Request.Form["shippingid"]), userId))
                {
                    context.Response.Write("{\"success\":true}");
                }
                else
                {
                    context.Response.Write("{\"success\":false}");
                }
            }
        }

        public decimal DiscountMoney(IList<ShoppingCartItemInfo> LineItems, out string ActivitiesId, out string ActivitiesName, MemberInfo member)
        {
            decimal num = 0M;
            decimal num2 = 0M;
            decimal num3 = 0M;
            ActivitiesId = "";
            ActivitiesName = "";
            decimal num4 = 0M;
            int num5 = 0;
            foreach (ShoppingCartItemInfo info in LineItems)
            {
                if (info.Type == 0)
                {
                    num4 += info.SubTotal;
                    num5 += info.Quantity;
                }
            }
            DataTable activities = ActivityHelper.GetActivities();
            for (int i = 0; i < activities.Rows.Count; i++)
            {
                if ((int.Parse(activities.Rows[i]["attendTime"].ToString()) != 0) && (int.Parse(activities.Rows[i]["attendTime"].ToString()) <= ActivityHelper.GetActivitiesMember(member.UserId, int.Parse(activities.Rows[i]["ActivitiesId"].ToString()))))
                {
                    continue;
                }
                decimal num7 = 0M;
                int num8 = 0;
                DataTable table2 = ActivityHelper.GetActivities_Detail(int.Parse(activities.Rows[i]["ActivitiesId"].ToString()));
                foreach (ShoppingCartItemInfo info2 in LineItems)
                {
                    if ((info2.Type == 0) && (ActivityHelper.GetActivitiesProducts(int.Parse(activities.Rows[i]["ActivitiesId"].ToString()), info2.ProductId).Rows.Count > 0))
                    {
                        num7 += info2.SubTotal;
                        num8 += info2.Quantity;
                    }
                }
                bool flag = false;
                if (table2.Rows.Count > 0)
                {
                    for (int j = 0; j < table2.Rows.Count; j++)
                    {
                        if ((table2.Rows[j]["MemberGrades"].ToString() == "0") || (table2.Rows[j]["MemberGrades"].ToString() == member.GradeId.ToString()))
                        {
                            if (bool.Parse(activities.Rows[i]["isAllProduct"].ToString()))
                            {
                                if (decimal.Parse(table2.Rows[j]["MeetMoney"].ToString()) > 0M)
                                {
                                    if ((num4 != 0M) && (num4 >= decimal.Parse(table2.Rows[table2.Rows.Count - 1]["MeetMoney"].ToString())))
                                    {
                                        num2 = decimal.Parse(table2.Rows[table2.Rows.Count - 1]["MeetMoney"].ToString());
                                        num = decimal.Parse(table2.Rows[table2.Rows.Count - 1]["ReductionMoney"].ToString());
                                        ActivitiesId = table2.Rows[table2.Rows.Count - 1]["id"].ToString();
                                        ActivitiesName = table2.Rows[table2.Rows.Count - 1]["ActivitiesName"].ToString();
                                        break;
                                    }
                                    if ((num4 != 0M) && (num4 <= decimal.Parse(table2.Rows[0]["MeetMoney"].ToString())))
                                    {
                                        num2 = decimal.Parse(table2.Rows[0]["MeetMoney"].ToString());
                                        num = decimal.Parse(table2.Rows[0]["ReductionMoney"].ToString());
                                        ActivitiesId = table2.Rows[0]["id"].ToString();
                                        ActivitiesName = table2.Rows[0]["ActivitiesName"].ToString();
                                        break;
                                    }
                                    if ((num4 != 0M) && (num4 >= decimal.Parse(table2.Rows[j]["MeetMoney"].ToString())))
                                    {
                                        num2 = decimal.Parse(table2.Rows[j]["MeetMoney"].ToString());
                                        num = decimal.Parse(table2.Rows[j]["ReductionMoney"].ToString());
                                        ActivitiesId = table2.Rows[j]["id"].ToString();
                                        ActivitiesName = table2.Rows[j]["ActivitiesName"].ToString();
                                    }
                                }
                                else
                                {
                                    if ((num5 != 0) && (num5 >= int.Parse(table2.Rows[table2.Rows.Count - 1]["MeetNumber"].ToString())))
                                    {
                                        num2 = decimal.Parse(table2.Rows[table2.Rows.Count - 1]["MeetMoney"].ToString());
                                        num3 = decimal.Parse(table2.Rows[table2.Rows.Count - 1]["ReductionMoney"].ToString());
                                        ActivitiesId = table2.Rows[table2.Rows.Count - 1]["id"].ToString();
                                        ActivitiesName = table2.Rows[table2.Rows.Count - 1]["ActivitiesName"].ToString();
                                        flag = true;
                                        break;
                                    }
                                    if ((num5 != 0) && (num5 <= int.Parse(table2.Rows[0]["MeetNumber"].ToString())))
                                    {
                                        num2 = decimal.Parse(table2.Rows[0]["MeetMoney"].ToString());
                                        num3 = decimal.Parse(table2.Rows[0]["ReductionMoney"].ToString());
                                        ActivitiesId = table2.Rows[0]["id"].ToString();
                                        ActivitiesName = table2.Rows[0]["ActivitiesName"].ToString();
                                        flag = true;
                                        break;
                                    }
                                    if ((num5 != 0) && (num5 >= int.Parse(table2.Rows[j]["MeetNumber"].ToString())))
                                    {
                                        num2 = decimal.Parse(table2.Rows[j]["MeetMoney"].ToString());
                                        num3 = decimal.Parse(table2.Rows[j]["ReductionMoney"].ToString());
                                        ActivitiesId = table2.Rows[j]["id"].ToString();
                                        ActivitiesName = table2.Rows[j]["ActivitiesName"].ToString();
                                        flag = true;
                                    }
                                }
                            }
                            else
                            {
                                num4 = num7;
                                num5 = num8;
                                if (decimal.Parse(table2.Rows[j]["MeetMoney"].ToString()) > 0M)
                                {
                                    if ((num4 != 0M) && (num4 >= decimal.Parse(table2.Rows[table2.Rows.Count - 1]["MeetMoney"].ToString())))
                                    {
                                        num2 = decimal.Parse(table2.Rows[table2.Rows.Count - 1]["MeetMoney"].ToString());
                                        num = decimal.Parse(table2.Rows[table2.Rows.Count - 1]["ReductionMoney"].ToString());
                                        ActivitiesId = table2.Rows[table2.Rows.Count - 1]["id"].ToString();
                                        ActivitiesName = table2.Rows[table2.Rows.Count - 1]["ActivitiesName"].ToString();
                                        break;
                                    }
                                    if ((num4 != 0M) && (num4 <= decimal.Parse(table2.Rows[0]["MeetMoney"].ToString())))
                                    {
                                        num2 = decimal.Parse(table2.Rows[0]["MeetMoney"].ToString());
                                        num = decimal.Parse(table2.Rows[0]["ReductionMoney"].ToString());
                                        ActivitiesId = table2.Rows[0]["id"].ToString();
                                        ActivitiesName = table2.Rows[0]["ActivitiesName"].ToString();
                                        break;
                                    }
                                    if ((num4 != 0M) && (num4 >= decimal.Parse(table2.Rows[j]["MeetMoney"].ToString())))
                                    {
                                        num2 = decimal.Parse(table2.Rows[j]["MeetMoney"].ToString());
                                        num = decimal.Parse(table2.Rows[j]["ReductionMoney"].ToString());
                                        ActivitiesId = table2.Rows[j]["id"].ToString();
                                        ActivitiesName = table2.Rows[j]["ActivitiesName"].ToString();
                                    }
                                }
                                else
                                {
                                    if ((num5 != 0) && (num5 >= int.Parse(table2.Rows[table2.Rows.Count - 1]["MeetNumber"].ToString())))
                                    {
                                        num2 = decimal.Parse(table2.Rows[table2.Rows.Count - 1]["MeetMoney"].ToString());
                                        num = decimal.Parse(table2.Rows[table2.Rows.Count - 1]["ReductionMoney"].ToString());
                                        ActivitiesId = table2.Rows[table2.Rows.Count - 1]["id"].ToString();
                                        ActivitiesName = table2.Rows[table2.Rows.Count - 1]["ActivitiesName"].ToString();
                                        flag = true;
                                        break;
                                    }
                                    if ((num5 != 0) && (num5 <= int.Parse(table2.Rows[0]["MeetNumber"].ToString())))
                                    {
                                        num2 = decimal.Parse(table2.Rows[0]["MeetMoney"].ToString());
                                        num = decimal.Parse(table2.Rows[0]["ReductionMoney"].ToString());
                                        ActivitiesId = table2.Rows[0]["id"].ToString();
                                        ActivitiesName = table2.Rows[0]["ActivitiesName"].ToString();
                                        flag = true;
                                        break;
                                    }
                                    if ((num5 != 0) && (num5 >= int.Parse(table2.Rows[j]["MeetNumber"].ToString())))
                                    {
                                        num2 = decimal.Parse(table2.Rows[j]["MeetMoney"].ToString());
                                        num = decimal.Parse(table2.Rows[j]["ReductionMoney"].ToString());
                                        ActivitiesId = table2.Rows[j]["id"].ToString();
                                        ActivitiesName = table2.Rows[j]["ActivitiesName"].ToString();
                                        flag = true;
                                    }
                                }
                            }
                        }
                    }
                    if (flag)
                    {
                        if (num5 > 0)
                        {
                            num3 += num;
                        }
                    }
                    else if (((num4 != 0M) && (num2 != 0M)) && (num4 >= num2))
                    {
                        num3 += num;
                    }
                }
            }
            return num3;
        }

        public void EditPassword(HttpContext context)
        {
            int num;
            context.Response.ContentType = "application/json";
            string sourceData = context.Request["oldPwd"];
            string str2 = context.Request["password"];
            string str3 = context.Request["passagain"];
            MemberInfo member = new MemberInfo();
            HttpCookie cookie = HttpContext.Current.Request.Cookies["Vshop-Member"];
            if (cookie != null)
            {
                num = int.Parse(cookie.Value);
            }
            else
            {
                context.Response.Write("{\"Status\":\"-1\"}");
                return;
            }
            member = MemberProcessor.GetMember(num, false);
            StringBuilder builder = new StringBuilder();
            builder.Append("{");
            if (member.Password == HiCryptographer.Md5Encrypt(sourceData))
            {
                if (str2 == str3)
                {
                    if (MemberProcessor.SetPwd(num.ToString(), HiCryptographer.Md5Encrypt(str2)))
                    {
                        builder.Append("\"Status\":\"OK\"");
                    }
                    else
                    {
                        builder.Append("\"Status\":\"-3\"");
                    }
                }
                else
                {
                    builder.Append("\"Status\":\"-2\"");
                }
            }
            else
            {
                builder.Append("\"Status\":\"-4\"");
            }
            builder.Append("}");
            context.Response.Write(builder.ToString());
        }

        private void FinishOrder(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            bool flag = false;
            OrderInfo orderInfo = ShoppingProcessor.GetOrderInfo(Convert.ToString(context.Request["orderId"]));
            Dictionary<string, LineItemInfo> lineItems = orderInfo.LineItems;
            LineItemInfo info2 = new LineItemInfo();
            foreach (KeyValuePair<string, LineItemInfo> pair in lineItems)
            {
                info2 = pair.Value;
                if ((info2.OrderItemsStatus == OrderStatus.ApplyForRefund) || (info2.OrderItemsStatus == OrderStatus.ApplyForReturns))
                {
                    flag = true;
                }
            }
            if (!flag)
            {
                if ((orderInfo != null) && MemberProcessor.ConfirmOrderFinish(orderInfo))
                {
                    DistributorsBrower.UpdateCalculationCommission(orderInfo,wid);
                    MemberInfo currentMember = MemberProcessor.GetCurrentMember();
                    SiteSettings masterSettings = SettingsManager.GetMasterSettings(false, this.wid);
                    int num = 0;
                    if ((masterSettings.IsRequestDistributor && !string.IsNullOrEmpty(masterSettings.FinishedOrderMoney.ToString())) && (currentMember.Expenditure >= masterSettings.FinishedOrderMoney))
                    {
                        num = 1;
                    }
                    foreach (LineItemInfo info4 in orderInfo.LineItems.Values)
                    {
                        if (info4.OrderItemsStatus.ToString() == OrderStatus.SellerAlreadySent.ToString())
                        {
                            ShoppingProcessor.UpdateOrderGoodStatu(orderInfo.OrderId, info4.SkuId, 5);
                        }
                    }
                    DistributorsInfo userIdDistributors = new DistributorsInfo();
                    userIdDistributors = DistributorsBrower.GetUserIdDistributors(orderInfo.UserId);
                    if ((userIdDistributors != null) && (userIdDistributors.UserId > 0))
                    {
                        num = 0;
                    }
                    context.Response.Write("{\"success\":true,\"isapply\":" + num + "}");
                }
                else
                {
                    context.Response.Write("{\"success\":false, \"msg\":\"订单当前状态不允许完成\"}");
                }
            }
            else
            {
                context.Response.Write("{\"success\":false, \"msg\":\"订单中商品有退货(款)不允许完成\"}");
            }
        }

        private string GenerateOrderId()
        {
            string str = string.Empty;
            Random random = new Random();
            for (int i = 0; i < 7; i++)
            {
                int num = random.Next();
                str = str + ((char)(0x30 + ((ushort)(num % 10)))).ToString();
            }
            return (DateTime.Now.ToString("yyyyMMdd") + str);
        }

        public void GetDrawRemarks(HttpContext context)
        {
            int result = 0;
            if (int.TryParse(context.Request["SerialID"], out result))
            {
                BalanceDrawRequestInfo balanceDrawRequestById = DistributorsBrower.GetBalanceDrawRequestById(result.ToString());
                if (balanceDrawRequestById != null)
                {
                    context.Response.Write(balanceDrawRequestById.Remark);
                }
                else
                {
                    context.Response.Write("N");
                }
            }
            else
            {
                context.Response.Write("N");
            }
        }

        public string getModelType(int m)
        {
            switch (m)
            {
                case 1:
                    return "快递";

                case 2:
                    return "EMS";

                case 3:
                    return "顺丰";

                case 4:
                    return "平邮";
            }
            return "包邮";
        }

        public void GetOrderRedPager(HttpContext context)
        {
            int num3;
            context.Response.ContentType = "application/json";
            int num = 0;
            int.TryParse(context.Request["id"], out num);
            int num2 = 0;
            int.TryParse(context.Request["userid"], out num2);
            HttpCookie cookie = HttpContext.Current.Request.Cookies["Vshop-Member"];
            if (cookie != null)
            {
                num3 = int.Parse(cookie.Value);
            }
            else
            {
                context.Response.Write("{\"status\":\"-1\",\"tips\":\"用户未登录！\"}");
                return;
            }
            if (num2 == num3)
            {
                context.Response.Write("{\"status\":\"-2\",\"tips\":\"您不能参与该活动！\"}");
            }
            else
            {
                ShareActivityInfo act = ShareActHelper.GetAct(num);
                if (act == null)
                {
                    context.Response.Write("{\"status\":\"-2\",\"tips\":\"活动不存在！\"}");
                }
                else if (act.BeginDate > DateTime.Now)
                {
                    context.Response.Write("{\"status\":\"-2\",\"tips\":\"活动未开始！\"}");
                }
                else if (act.EndDate < DateTime.Now)
                {
                    context.Response.Write("{\"status\":\"-2\",\"tips\":\"活动已结束！\"}");
                }
                else if (ShareActHelper.GeTAttendCount(num, num2) > act.CouponNumber)
                {
                    context.Response.Write("{\"status\":\"-3\",\"tips\":\"您来晚了，领取机会已用完！\"}");
                }
                else if (ShareActHelper.HasAttend(num, num3))
                {
                    context.Response.Write("{\"status\":\"-5\",\"tips\":\"" + num3.ToString() + "\"}");
                }
                else
                {
                    int couponId = act.CouponId;
                    if (CouponHelper.GetCoupon(couponId) == null)
                    {
                        context.Response.Write("{\"status\":\"-2\",\"tips\":\"优惠券不存在！\"}");
                    }
                    else
                    {
                        SendCouponResult result = CouponHelper.IsCanSendCouponToMember(couponId, num3);
                        if (result.GetHashCode() == 1)
                        {
                            context.Response.Write("{\"status\":\"-2\",\"tips\":\"优惠劵已结束！\"}");
                        }
                        else if (result.GetHashCode() == 2)
                        {
                            context.Response.Write("{\"status\":\"-2\",\"tips\":\"会员等级不在此活动范内！\"}");
                        }
                        else if (result.GetHashCode() == 3)
                        {
                            context.Response.Write("{\"status\":\"-3\",\"tips\":\"优惠劵已领完！\"}");
                        }
                        else if (result.GetHashCode() == 4)
                        {
                            context.Response.Write("{\"status\":\"-2\",\"tips\":\"已到领取上限！\"}");
                        }
                        else if (result.GetHashCode() == 5)
                        {
                            context.Response.Write("{\"status\":\"-4\",\"tips\":\"领取优惠券失败！\"}");
                        }
                        else
                        {
                            ShareActivityRecordInfo record = new ShareActivityRecordInfo
                            {
                                shareId = num,
                                shareUser = num2,
                                attendUser = num3
                            };
                            if (ShareActHelper.AddRecord(record))
                            {
                                if (CouponHelper.SendCouponToMember(couponId, num3).GetHashCode() == 0)
                                {
                                    context.Response.Write("{\"status\":\"0\",\"tips\":\"" + num3.ToString() + "\"}");
                                }
                                else
                                {
                                    context.Response.Write("{\"status\":\"-4\",\"tips\":\"领取优惠券失败！\"}");
                                }
                            }
                            else
                            {
                                context.Response.Write("{\"status\":\"-4\",\"tips\":\"领取优惠券失败！\"}");
                            }
                        }
                    }
                }
            }
        }

        private void GetPrize(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            int result = 1;
            int.TryParse(context.Request["activityid"], out result);
            LotteryActivityInfo lotteryActivity = VshopBrowser.GetLotteryActivity(result);
            int userPrizeCount = VshopBrowser.GetUserPrizeCount(result);
            if (MemberProcessor.GetCurrentMember() == null)
            {
                MemberInfo member = new MemberInfo();
                string generateId = Globals.GetGenerateId();
                member.GradeId = MemberProcessor.GetDefaultMemberGrade(this.wid);
                member.UserName = "";
                member.OpenId = "";
                member.CreateDate = DateTime.Now;
                member.SessionId = generateId;
                member.SessionEndTime = DateTime.Now;
                member.wid = this.wid;
                MemberProcessor.CreateMember(member);
                member = MemberProcessor.GetMember(generateId);
                HttpCookie cookie = new HttpCookie("Vshop-Member")
                {
                    Value = member.UserId.ToString(),
                    Expires = DateTime.Now.AddDays(10)
                };
                HttpContext.Current.Response.Cookies.Add(cookie);
            }
            StringBuilder builder = new StringBuilder();
            builder.Append("{");
            if (userPrizeCount >= lotteryActivity.MaxNum)
            {
                builder.Append("\"No\":\"-1\"");
                builder.Append("}");
                context.Response.Write(builder.ToString());
            }
            else if ((DateTime.Now < lotteryActivity.StartTime) || (DateTime.Now > lotteryActivity.EndTime))
            {
                builder.Append("\"No\":\"-3\"");
                builder.Append("}");
                context.Response.Write(builder.ToString());
            }
            else
            {
                PrizeQuery page = new PrizeQuery
                {
                    ActivityId = result
                };
                List<PrizeRecordInfo> prizeList = VshopBrowser.GetPrizeList(page);
                int num3 = 0;
                int num4 = 0;
                int num5 = 0;
                int num6 = 0;
                int num7 = 0;
                int num8 = 0;
                if ((prizeList != null) && (prizeList.Count > 0))
                {
                    num3 = prizeList.Count<PrizeRecordInfo>(a => a.Prizelevel == "一等奖");
                    num4 = prizeList.Count<PrizeRecordInfo>(a => a.Prizelevel == "二等奖");
                    num5 = prizeList.Count<PrizeRecordInfo>(a => a.Prizelevel == "三等奖");
                }
                PrizeRecordInfo model = new PrizeRecordInfo
                {
                    PrizeTime = new DateTime?(DateTime.Now),
                    UserID = Globals.GetCurrentMemberUserId(),
                    ActivityName = lotteryActivity.ActivityName,
                    ActivityID = result,
                    IsPrize = true
                };
                List<PrizeSetting> prizeSettingList = lotteryActivity.PrizeSettingList;
                decimal num9 = prizeSettingList[0].Probability * 100M;
                decimal num10 = prizeSettingList[1].Probability * 100M;
                decimal num11 = prizeSettingList[2].Probability * 100M;
                int num15 = new Random(Guid.NewGuid().GetHashCode()).Next(1, 0x2711);
                if (prizeSettingList.Count > 3)
                {
                    decimal num12 = prizeSettingList[3].Probability * 100M;
                    decimal num13 = prizeSettingList[4].Probability * 100M;
                    decimal num14 = prizeSettingList[5].Probability * 100M;
                    num6 = prizeList.Count<PrizeRecordInfo>(a => a.Prizelevel == "四等奖");
                    num7 = prizeList.Count<PrizeRecordInfo>(a => a.Prizelevel == "五等奖");
                    num8 = prizeList.Count<PrizeRecordInfo>(a => a.Prizelevel == "六等奖");
                    if ((num15 < num9) && (prizeSettingList[0].PrizeNum > num3))
                    {
                        builder.Append("\"No\":\"9\"");
                        model.Prizelevel = "一等奖";
                        model.PrizeName = prizeSettingList[0].PrizeName;
                    }
                    else if ((num15 < num10) && (prizeSettingList[1].PrizeNum > num4))
                    {
                        builder.Append("\"No\":\"11\"");
                        model.Prizelevel = "二等奖";
                        model.PrizeName = prizeSettingList[1].PrizeName;
                    }
                    else if ((num15 < num11) && (prizeSettingList[2].PrizeNum > num5))
                    {
                        builder.Append("\"No\":\"1\"");
                        model.Prizelevel = "三等奖";
                        model.PrizeName = prizeSettingList[2].PrizeName;
                    }
                    else if ((num15 < num12) && (prizeSettingList[3].PrizeNum > num6))
                    {
                        builder.Append("\"No\":\"3\"");
                        model.Prizelevel = "四等奖";
                        model.PrizeName = prizeSettingList[3].PrizeName;
                    }
                    else if ((num15 < num13) && (prizeSettingList[4].PrizeNum > num7))
                    {
                        builder.Append("\"No\":\"5\"");
                        model.Prizelevel = "五等奖";
                        model.PrizeName = prizeSettingList[4].PrizeName;
                    }
                    else if ((num15 < num14) && (prizeSettingList[5].PrizeNum > num8))
                    {
                        builder.Append("\"No\":\"7\"");
                        model.Prizelevel = "六等奖";
                        model.PrizeName = prizeSettingList[5].PrizeName;
                    }
                    else
                    {
                        model.IsPrize = false;
                        builder.Append("\"No\":\"0\"");
                    }
                }
                else if ((num15 < num9) && (prizeSettingList[0].PrizeNum > num3))
                {
                    builder.Append("\"No\":\"9\"");
                    model.Prizelevel = "一等奖";
                    model.PrizeName = prizeSettingList[0].PrizeName;
                }
                else if ((num15 < num10) && (prizeSettingList[1].PrizeNum > num4))
                {
                    builder.Append("\"No\":\"11\"");
                    model.Prizelevel = "二等奖";
                    model.PrizeName = prizeSettingList[1].PrizeName;
                }
                else if ((num15 < num11) && (prizeSettingList[2].PrizeNum > num5))
                {
                    builder.Append("\"No\":\"1\"");
                    model.Prizelevel = "三等奖";
                    model.PrizeName = prizeSettingList[2].PrizeName;
                }
                else
                {
                    model.IsPrize = false;
                    builder.Append("\"No\":\"0\"");
                }
                builder.Append("}");
                if (context.Request["activitytype"] != "scratch")
                {
                    VshopBrowser.AddPrizeRecord(model);
                }
                context.Response.Write(builder.ToString());
            }
        }

        public void GetShippingTypes(HttpContext context)
        {
            ShoppingCartInfo shoppingCart = null;
            StringBuilder builder = new StringBuilder();
            if ((int.TryParse(context.Request["buyAmount"], out this.buyAmount) && !string.IsNullOrEmpty(context.Request["from"])) && (context.Request["from"] == "signBuy"))
            {
                this.productSku = context.Request["productSku"];
                shoppingCart = ShoppingCartProcessor.GetShoppingCart(this.productSku, this.buyAmount);
            }
            else
            {
                int result = 0;
                if (!string.IsNullOrEmpty(context.Request["TemplateId"]) && int.TryParse(context.Request["TemplateId"], out result))
                {
                    shoppingCart = ShoppingCartProcessor.GetShoppingCart(result);
                }
            }
            StringBuilder builder2 = new StringBuilder();
            string regionId = context.Request["city"];
            context.Response.ContentType = "application/json";
            string str2 = "";
            foreach (ShoppingCartItemInfo info2 in shoppingCart.LineItems)
            {
                if (info2.FreightTemplateId > 0)
                {
                    str2 = str2 + info2.FreightTemplateId + ",";
                }
            }
            if (!string.IsNullOrEmpty(str2))
            {
                DataTable specifyRegionGroupsModeId = SettingsHelper.GetSpecifyRegionGroupsModeId(str2.Substring(0, str2.Length - 1), regionId);
                if (specifyRegionGroupsModeId.Rows.Count > 0)
                {
                    for (int i = 0; i < specifyRegionGroupsModeId.Rows.Count; i++)
                    {
                        string str3 = this.getModelType(int.Parse(specifyRegionGroupsModeId.Rows[i]["ModeId"].ToString()));
                        builder2.Append(string.Concat(new object[] { ",{\"modelId\":\"", specifyRegionGroupsModeId.Rows[i]["ModeId"], "\",\"text\":\"", str3, "\"}" }));
                    }
                }
                else
                {
                    builder2.Append(",{\"modelId\":\"0\",\"text\":\"包邮\"}");
                }
                builder.Append(builder2.ToString() ?? "");
            }
            else
            {
                builder2.Append(",{\"modelId\":\"0\",\"text\":\"包邮\"}");
            }
            if (builder2.Length > 0)
            {
                builder2.Remove(0, 1);
            }
            builder2.Insert(0, "{\"data\":[").Append("]}");
            context.Response.ContentType = "application/json";
            context.Response.Write(builder2.ToString());
        }

        public bool IsFreeTemplateShipping(string RegionId, int FreightTemplateId, int ModeId, ShoppingCartItemInfo info)
        {
            bool flag = false;
            DataTable table = SettingsHelper.GetFreeTemplateShipping(RegionId, FreightTemplateId, ModeId);
            if (table.Rows.Count > 0)
            {
                string str2 = table.Rows[0]["ConditionType"].ToString();
                if (str2 == null)
                {
                    goto Label_0160;
                }
                if (!(str2 == "1"))
                {
                    if (str2 == "2")
                    {
                        if (info.SubTotal >= decimal.Parse(table.Rows[0]["ConditionNumber"].ToString()))
                        {
                            flag = true;
                        }
                        return flag;
                    }
                    if (str2 == "3")
                    {
                        if ((info.Quantity >= int.Parse(table.Rows[0]["ConditionNumber"].ToString().Split(new char[] { '$' })[0])) && (info.SubTotal >= decimal.Parse(table.Rows[0]["ConditionNumber"].ToString().Split(new char[] { '$' })[1])))
                        {
                            flag = true;
                        }
                        return flag;
                    }
                    goto Label_0160;
                }
                if (info.Quantity >= int.Parse(table.Rows[0]["ConditionNumber"].ToString()))
                {
                    flag = true;
                }
            }
            return flag;
        Label_0160:
            return false;
        }

        public string ordersummit(ShoppingCartInfo cart, HttpContext context, string remark, int shippingId, string couponCode, string selectCouponValue, string shippingTypeinfo, bool summittype, string OrderMarking, IList<ShoppingCartItemInfo> ItemInfo, int PointExchange, out string ActivitiesIds)
        {
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            ActivitiesIds = "";
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(false, currentMember.wid);
            StringBuilder builder = new StringBuilder();
            OrderInfo order = ShoppingProcessor.ConvertShoppingCartToOrder(cart, false, false, currentMember.wid);
            if (order == null)
            {
                builder.Append("\"Status\":\"None\"");
                goto Label_0CB4;
            }
            order.OrderId = this.GenerateOrderId();
            order.OrderDate = DateTime.Now;

            order.UserId = currentMember.UserId;
            order.Username = currentMember.UserName;
            order.EmailAddress = currentMember.Email;
            order.RealName = currentMember.RealName;
            order.QQ = currentMember.QQ;
            order.Remark = remark;
            //当前用户所在wid
            order.wid = this.wid;

            string activitiesId = "";
            string activitiesName = "";
            order.DiscountAmount = this.DiscountMoney(ItemInfo, out activitiesId, out activitiesName, currentMember);
            ActivitiesIds = activitiesId;
            order.OrderMarking = OrderMarking;
            if (order.DiscountAmount > 0M)
            {
                order.ActivitiesId = activitiesId;
                order.ActivitiesName = activitiesName;
            }
            order.OrderStatus = OrderStatus.WaitBuyerPay;
            int num = 0;
            foreach (KeyValuePair<string, LineItemInfo> pair in order.LineItems)
            {
                LineItemInfo info3 = new LineItemInfo();
                if (pair.Value.Type == 1)
                {
                    num++;
                }
            }
            if (order.LineItems.Count == num)
            {
                order.OrderStatus = OrderStatus.BuyerAlreadyPaid;
            }
            order.RefundStatus = RefundStatus.None;
            order.ShipToDate = context.Request["shiptoDate"];
            if (HttpContext.Current.Request.Cookies["Vshop-ReferralId"] != null)
            {
                order.ReferralUserId = int.Parse(HttpContext.Current.Request.Cookies.Get("Vshop-ReferralId").Value);
            }
            else
            {
                order.ReferralUserId = 0;
            }
            int result = 0;
            int num3 = 0;
            ShippingAddressInfo shippingAddress = MemberProcessor.GetShippingAddress(shippingId);
            if (shippingAddress != null)
            {
                order.ShippingRegion = RegionHelper.GetFullRegion(shippingAddress.RegionId, "，");
                order.RegionId = shippingAddress.RegionId;
                order.Address = shippingAddress.Address;
                order.ZipCode = shippingAddress.Zipcode;
                order.ShipTo = shippingAddress.ShipTo;
                order.TelPhone = shippingAddress.TelPhone;
                order.CellPhone = shippingAddress.CellPhone;
                MemberProcessor.SetDefaultShippingAddress(shippingId, MemberProcessor.GetCurrentMember().UserId);
            }
            if (int.TryParse(shippingTypeinfo, out result))
            {
                order.ShippingModeId = result;
                order.ModeName = this.getModelType(result);
                order.AdjustedFreight = 0M;
                if (result > 0)
                {
                    DataView defaultView = new DataView();
                    if (cart != null)
                    {
                        defaultView = SettingsHelper.GetAllFreightItems(wid).DefaultView;
                    }
                    float num4 = 0f;
                    if (defaultView.Count > 0)
                    {
                        foreach (ShoppingCartItemInfo info5 in cart.LineItems)
                        {
                            string str6;
                            if (!info5.IsfreeShipping)
                            {
                                bool flag = false;
                                FreightTemplate templateMessage = SettingsHelper.GetTemplateMessage(info5.FreightTemplateId);
                                if (((templateMessage != null) && (info5.FreightTemplateId > 0)) && !templateMessage.FreeShip)
                                {
                                    if (templateMessage.HasFree)
                                    {
                                        flag = this.IsFreeTemplateShipping(context.Request["Shippingcity"], info5.FreightTemplateId, result, info5);
                                    }
                                    if (!flag)
                                    {
                                        defaultView.RowFilter = string.Concat(new object[] { " RegionId=", context.Request["Shippingcity"], " and ModeId=", result, " and TemplateId=", info5.FreightTemplateId, " and IsDefault=0" });
                                        if (defaultView.Count != 1)
                                        {
                                            goto Label_061F;
                                        }
                                        string str5 = defaultView[0]["MUnit"].ToString();
                                        if (str5 != null)
                                        {
                                            if (!(str5 == "1"))
                                            {
                                                if (str5 == "2")
                                                {
                                                    goto Label_049B;
                                                }
                                                if (str5 == "3")
                                                {
                                                    goto Label_055D;
                                                }
                                            }
                                            else
                                            {
                                                num4 += this.setferight(float.Parse(info5.Quantity.ToString()), float.Parse(defaultView[0]["FristNumber"].ToString()), float.Parse(defaultView[0]["FristPrice"].ToString()), float.Parse(defaultView[0]["AddNumber"].ToString()), float.Parse(defaultView[0]["AddPrice"].ToString()));
                                            }
                                        }
                                    }
                                }
                            }
                            continue;
                        Label_049B:
                            if (info5.FreightWeight > 0M)
                            {
                                num4 += this.setferight(float.Parse(info5.FreightWeight.ToString()) * float.Parse(info5.Quantity.ToString()), float.Parse(defaultView[0]["FristNumber"].ToString()), float.Parse(defaultView[0]["FristPrice"].ToString()), float.Parse(defaultView[0]["AddNumber"].ToString()), float.Parse(defaultView[0]["AddPrice"].ToString()));
                            }
                            continue;
                        Label_055D:
                            if (info5.CubicMeter > 0M)
                            {
                                num4 += this.setferight(float.Parse(info5.CubicMeter.ToString()) * float.Parse(info5.Quantity.ToString()), float.Parse(defaultView[0]["FristNumber"].ToString()), float.Parse(defaultView[0]["FristPrice"].ToString()), float.Parse(defaultView[0]["AddNumber"].ToString()), float.Parse(defaultView[0]["AddPrice"].ToString()));
                            }
                            continue;
                        Label_061F: ;
                            defaultView.RowFilter = string.Concat(new object[] { "  ModeId=", result, " and TemplateId=", info5.FreightTemplateId, " and  IsDefault=1" });
                            if ((defaultView.Count == 1) && ((str6 = defaultView[0]["MUnit"].ToString()) != null))
                            {
                                if (!(str6 == "1"))
                                {
                                    if (str6 == "2")
                                    {
                                        goto Label_0761;
                                    }
                                    if (str6 == "3")
                                    {
                                        goto Label_0823;
                                    }
                                }
                                else
                                {
                                    num4 += this.setferight(float.Parse(info5.Quantity.ToString()), float.Parse(defaultView[0]["FristNumber"].ToString()), float.Parse(defaultView[0]["FristPrice"].ToString()), float.Parse(defaultView[0]["AddNumber"].ToString()), float.Parse(defaultView[0]["AddPrice"].ToString()));
                                }
                            }
                            continue;
                        Label_0761:
                            if (info5.FreightWeight > 0M)
                            {
                                num4 += this.setferight(float.Parse(info5.FreightWeight.ToString()) * float.Parse(info5.Quantity.ToString()), float.Parse(defaultView[0]["FristNumber"].ToString()), float.Parse(defaultView[0]["FristPrice"].ToString()), float.Parse(defaultView[0]["AddNumber"].ToString()), float.Parse(defaultView[0]["AddPrice"].ToString()));
                            }
                            continue;
                        Label_0823:
                            if (info5.CubicMeter > 0M)
                            {
                                num4 += this.setferight(float.Parse(info5.CubicMeter.ToString()) * float.Parse(info5.Quantity.ToString()), float.Parse(defaultView[0]["FristNumber"].ToString()), float.Parse(defaultView[0]["FristPrice"].ToString()), float.Parse(defaultView[0]["AddNumber"].ToString()), float.Parse(defaultView[0]["AddPrice"].ToString()));
                            }
                        }
                    }
                    string s = num4.ToString("F2");
                    order.AdjustedFreight = decimal.Parse(s);
                }
            }
            if (int.TryParse(context.Request["paymentType"], out num3))
            {
                order.PaymentTypeId = num3;
                switch (num3)
                {
                    case 0:
                    case -1:
                        order.PaymentType = "货到付款";
                        order.Gateway = "hishop.plugins.payment.podrequest";
                        goto Label_09CA;

                    case 0x58:
                        order.PaymentType = "微信支付";
                        order.Gateway = "hishop.plugins.payment.weixinrequest";
                        goto Label_09CA;

                    case 0x63:
                        order.PaymentType = "线下付款";
                        order.Gateway = "hishop.plugins.payment.offlinerequest";
                        goto Label_09CA;
                }
                PaymentModeInfo paymentMode = ShoppingProcessor.GetPaymentMode(num3);
                if (paymentMode != null)
                {
                    order.PaymentTypeId = paymentMode.ModeId;
                    order.PaymentType = paymentMode.Name;
                    order.Gateway = paymentMode.Gateway;
                }
            }
        Label_09CA:
            if (!string.IsNullOrEmpty(couponCode))
            {
                CouponInfo info7 = ShoppingProcessor.UseCoupon(cart.GetTotal(), couponCode);
                order.CouponName = info7.CouponName;
                if (info7.ConditionValue > 0M)
                {
                    order.CouponAmount = info7.ConditionValue;
                }
                order.CouponCode = couponCode;
                order.CouponValue = info7.CouponValue;
            }
            if (!string.IsNullOrEmpty(selectCouponValue))
            {
                order.RedPagerActivityName = selectCouponValue.Split(new char[] { '|' })[0];
                order.RedPagerID = new int?(int.Parse(selectCouponValue.Split(new char[] { '|' })[1]));
                order.RedPagerOrderAmountCanUse = decimal.Parse(selectCouponValue.Split(new char[] { '|' })[2]);
                order.RedPagerAmount = decimal.Parse(selectCouponValue.Split(new char[] { '|' })[3]);
            }
            order.PointToCash = 0M;
            order.PointExchange = 0;
            if (masterSettings.PonitToCash_Enable && (PointExchange > 0))
            {
                int pointToCashRate = masterSettings.PointToCashRate;
                decimal num6 = masterSettings.PonitToCash_MaxAmount;
                float num7 = float.Parse(PointExchange.ToString()) / float.Parse(pointToCashRate.ToString());
                decimal total = order.GetTotal();
                if ((decimal.Parse(num7.ToString()) > total) || (num6 < decimal.Parse(num7.ToString())))
                {
                    builder.Append("\"Status\":\"Error\"");
                    builder.AppendFormat(string.Concat(new object[] { ",\"ErrorMsg\":\"抵现金额不能大于应付总额", total, "元,最高抵现金额", num6, "元！\"" }), new object[0]);
                    return builder.ToString();
                }
                order.PointToCash = decimal.Parse(num7.ToString());
                order.PointExchange = PointExchange;
            }
            try
            {
                decimal goodDiscountAverage = 0M;
                goodDiscountAverage = (order.RedPagerAmount + order.PointToCash) + order.DiscountAmount;
                this.SetOrderItemStatus(order, goodDiscountAverage);
                order = ShoppingProcessor.GetCalculadtionCommission(order);
                if (ShoppingProcessor.CreatOrder(order))
                {
                    if (summittype)
                    {
                        ShoppingCartProcessor.ClearShoppingCart();
                    }
                    Messenger.OrderCreated(order, currentMember, currentMember.wid);
                    if (!string.IsNullOrEmpty(masterSettings.ManageOpenID))
                    {
                        MemberInfo openIdMember = MemberProcessor.GetOpenIdMember(masterSettings.ManageOpenID);
                        if (openIdMember != null)
                        {
                            Messenger.OrderCreatedSendManage(order, openIdMember, currentMember.wid);
                        }
                    }
                    builder.Append("\"Status\":\"OK\",\"OrderMarkingStatus\":\"" + order.OrderMarking + "\",");
                    builder.AppendFormat("\"OrderId\":\"{0}\"", order.OrderMarking);
                }
                else
                {
                    builder.Append("\"Status\":\"Error\"");
                    builder.AppendFormat(",\"ErrorMsg\":\"提交订单失败！\"", new object[0]);
                }
            }
            catch (OrderException exception)
            {
                builder.Append("\"Status\":\"Error\"");
                builder.AppendFormat(",\"ErrorMsg\":\"{0}\"", exception.Message);
            }
        Label_0CB4:
            return builder.ToString();
        }

        private void ProcessAddToCartBySkus(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            int quantity = int.Parse(context.Request["quantity"], NumberStyles.None);
            string skuId = context.Request["productSkuId"];
            int categoryid = int.Parse(context.Request["categoryid"], NumberStyles.None);
            int templateid = int.Parse(context.Request["Templateid"], NumberStyles.None);
            int result = 0;
            int.TryParse(context.Request["type"], out result);
            int num5 = 0;
            int.TryParse(context.Request["exchangeId"], out num5);
            if (MemberProcessor.GetCurrentMember() == null)
            {
                context.Response.Write("{\"Status\":\"2\"}");
            }
            else
            {
                ShoppingCartProcessor.AddLineItem(skuId, quantity, categoryid, templateid, result, num5);
                ShoppingCartInfo shoppingCart = ShoppingCartProcessor.GetShoppingCart();
                context.Response.Write("{\"Status\":\"OK\",\"TotalMoney\":\"" + shoppingCart.GetTotal().ToString(".00") + "\",\"Quantity\":\"" + shoppingCart.GetQuantity().ToString() + "\"}");
            }
        }

        private void ProcessChageQuantity(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            string skuId = context.Request["skuId"];
            int result = 1;
            int.TryParse(context.Request["quantity"], out result);
            int num2 = 0;
            int.TryParse(context.Request["type"], out num2);
            int num3 = 0;
            int.TryParse(context.Request["exchangeId"], out num3);
            StringBuilder builder = new StringBuilder();
            builder.Append("{");
            int num4 = ShoppingCartProcessor.GetSkuStock(skuId, num2, num3);
            if (result > num4)
            {
                builder.AppendFormat("\"Status\":\"{0}\"", num4);
                result = num4;
            }
            else
            {
                builder.Append("\"Status\":\"OK\",");
                ShoppingCartProcessor.UpdateLineItemQuantity(skuId, (result > 0) ? result : 1, num2);
                builder.AppendFormat("\"TotalPrice\":\"{0}\"", ShoppingCartProcessor.GetShoppingCart().GetAmount());
            }
            builder.Append("}");
            context.Response.ContentType = "application/json";
            context.Response.Write(builder.ToString());
        }

        private void ProcessDeleteCartProduct(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            string skuId = context.Request["skuId"];
            int result = 0;
            int.TryParse(context.Request["type"], out result);
            StringBuilder builder = new StringBuilder();
            ShoppingCartProcessor.RemoveLineItem(skuId, result);
            builder.Append("{");
            builder.Append("\"Status\":\"OK\"");
            builder.Append("}");
            context.Response.ContentType = "application/json";
            context.Response.Write(builder.ToString());
        }

        private void ProcessGetSkuByOptions(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            int productId = int.Parse(context.Request["productId"], NumberStyles.None);
            string str = context.Request["options"];
            if (string.IsNullOrEmpty(str))
            {
                context.Response.Write("{\"Status\":\"0\"}");
            }
            else
            {
                if (str.EndsWith(","))
                {
                    str = str.Substring(0, str.Length - 1);
                }
                SKUItem item = ShoppingProcessor.GetProductAndSku(MemberProcessor.GetCurrentMember(), productId, str);
                if (item == null)
                {
                    context.Response.Write("{\"Status\":\"1\"}");
                }
                else
                {
                    StringBuilder builder = new StringBuilder();
                    builder.Append("{");
                    builder.Append("\"Status\":\"OK\",");
                    builder.AppendFormat("\"SkuId\":\"{0}\",", item.SkuId);
                    builder.AppendFormat("\"SKU\":\"{0}\",", item.SKU);
                    builder.AppendFormat("\"Weight\":\"{0}\",", item.Weight);
                    builder.AppendFormat("\"Stock\":\"{0}\",", item.Stock);
                    builder.AppendFormat("\"SalePrice\":\"{0}\"", item.SalePrice.ToString("F2"));
                    builder.Append("}");
                    context.Response.ContentType = "application/json";
                    context.Response.Write(builder.ToString());
                }
            }
        }

        public void ProcessRequest(HttpContext context)
        {
            //wid = context.Session[DTKeys.SESSION_WEB_ID] as string;
            //if (string.IsNullOrEmpty(wid))
            //{
            //    return;
            //}
            this.wid = Globals.GetCurrentWid();
            switch (context.Request["action"])
            {
                case "GetDrawStatus":
                    this.GetDrawRemarks(context);
                    return;

                case "CheckCoupon":
                    this.CheckCoupon(context);
                    return;

                case "SignToday":
                    this.SignToday(context);
                    return;

                case "ConfirmPrizeArriver":
                    this.ConfirmPrizeArriver(context);
                    return;

                case "ConfirmPrizeAddr":
                    this.ConfirmPrizeAddr(context);
                    return;

                case "AddToCartBySkus":
                    this.ProcessAddToCartBySkus(context);
                    return;

                case "GetSkuByOptions":
                    this.ProcessGetSkuByOptions(context);
                    return;

                case "DeleteCartProduct":
                    this.ProcessDeleteCartProduct(context);
                    return;

                case "ChageQuantity":
                    this.ProcessChageQuantity(context);
                    return;

                case "Submmitorder":
                    this.ProcessSubmmitorder(context);
                    return;

                case "SubmitMemberCard":
                    this.ProcessSubmitMemberCard(context);
                    return;

                case "AddShippingAddress":
                    this.AddShippingAddress(context);
                    return;

                case "DelShippingAddress":
                    this.DelShippingAddress(context);
                    return;

                case "SetDefaultShippingAddress":
                    this.SetDefaultShippingAddress(context);
                    return;

                case "UpdateShippingAddress":
                    this.UpdateShippingAddress(context);
                    return;

                case "GetPrize":
                    this.GetPrize(context);
                    return;

                case "Vote":
                    this.Vote(context);
                    return;

                case "SubmitActivity":
                    this.SubmitActivity(context);
                    return;

                case "AddSignUp":
                    this.AddSignUp(context);
                    return;

                case "AddTicket":
                    this.AddTicket(context);
                    return;

                case "FinishOrder":
                    this.FinishOrder(context);
                    return;

                case "AddUserPrize":
                    this.AddUserPrize(context);
                    return;

                case "SubmitWinnerInfo":
                    this.SubmitWinnerInfo(context);
                    return;

                case "SetUserName":
                    this.SetUserName(context);
                    return;

                case "RequestReturn":
                    this.RequestReturn(context);
                    return;

                case "AddProductConsultations":
                    this.AddProductConsultations(context);
                    return;

                case "AddProductReview":
                    this.AddProductReview(context);
                    return;

                case "AddFavorite":
                    this.AddFavorite(context);
                    return;

                case "DelFavorite":
                    this.DelFavorite(context);
                    return;

                case "CheckFavorite":
                    this.CheckFavorite(context);
                    return;

                case "Logistic":
                    this.SearchExpressData(context);
                    return;

                case "GetShippingTypes":
                    this.GetShippingTypes(context);
                    return;

                case "UserLogin":
                    this.UserLogin(context);
                    return;

                case "RegisterUser":
                    this.RegisterUser(context);
                    return;

                case "BindUserName":
                    this.BindUserName(context);
                    return;

                case "BindOldUserName":
                    this.BindOldUserName(context);
                    return;

                case "AddDistributor":
                    this.AddDistributor(context);
                    return;

                case "SetDistributorMsg":
                    this.SetDistributorMsg(context);
                    return;

                case "DeleteProducts":
                    this.DeleteDistributorProducts(context);
                    return;

                case "AddDistributorProducts":
                    this.AddDistributorProducts(context);
                    return;

                case "UpdateDistributor":
                    this.UpdateDistributor(context);
                    return;

                case "AddCommissions":
                    this.AddCommissions(context);
                    return;

                case "AdjustCommissions":
                    this.AdjustCommissions(context);
                    return;

                case "EditPassword":
                    this.EditPassword(context);
                    return;

                case "GetOrderRedPager":
                    this.GetOrderRedPager(context);
                    return;

                case "countfreight":
                    this.countfreight(context);
                    return;

                case "checkdistribution":
                    this.checkdistribution(context);
                    return;

                case "countfreighttype":
                    this.countfreighttype(context);
                    return;
            }
        }

        private void ProcessSubmitMemberCard(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            if (currentMember == null)
            {
                context.Response.Write("{\"success\":false}");
            }
            else
            {
                currentMember.Address = context.Request.Form.Get("address");
                currentMember.RealName = context.Request.Form.Get("name");
                currentMember.CellPhone = context.Request.Form.Get("phone");
                currentMember.QQ = context.Request.Form.Get("qq");
                if (!string.IsNullOrEmpty(currentMember.QQ))
                {
                    currentMember.Email = currentMember.QQ + "@qq.com";
                }
                currentMember.VipCardNumber = SettingsManager.GetMasterSettings(false,currentMember.wid).VipCardPrefix + currentMember.UserId.ToString();
                currentMember.VipCardDate = new DateTime?(DateTime.Now);
                string s = MemberProcessor.UpdateMember(currentMember) ? "{\"success\":true}" : "{\"success\":false}";
                context.Response.Write(s);
            }
        }

        private void ProcessSubmmitorder(HttpContext context)
        {
            int num4;
            int num5;
            context.Response.ContentType = "application/json";
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            StringBuilder builder = new StringBuilder();
            builder.Append("{");
            int shippingId = 0;
            string couponCode = context.Request["couponCode"];
            string[] strArray = context.Request["selectCouponValue"].Split(new char[] { ',' });
            string[] strArray2 = context.Request["PointNumber"].Split(new char[] { ',' });
            if (strArray2.Length > 0)
            {
                int num2 = 0;
                int result = 0;
                foreach (string str2 in strArray2)
                {
                    if (int.TryParse(str2, out result))
                    {
                        num2 += result;
                    }
                    else
                    {
                        builder.Append("\"Status\":\"Eror\",\"ErrorMsg\":\"输入参数不正确！\"");
                        builder.Append("}");
                        context.Response.ContentType = "application/json";
                        context.Response.Write(builder.ToString());
                        return;
                    }
                }
                if ((num2 != 0) && (num2 > currentMember.Points))
                {
                    builder.Append("\"Status\":\"Eror\",\"ErrorMsg\":\"您当前积分不足！\"");
                    builder.Append("}");
                    context.Response.ContentType = "application/json";
                    context.Response.Write(builder.ToString());
                    return;
                }
            }
            string str3 = "";
            string activitiesIds = "";
            shippingId = int.Parse(context.Request["shippingId"]);
            int.TryParse(context.Request["groupbuyId"], out num4);
            string remark = context.Request["remark"];
            string orderMarking = this.GenerateOrderId();
            if (((int.TryParse(context.Request["buyAmount"], out num5) && !string.IsNullOrEmpty(context.Request["productSku"])) && !string.IsNullOrEmpty(context.Request["from"])) && ((context.Request["from"] == "signBuy") || (context.Request["from"] == "groupBuy")))
            {
                string productSkuId = context.Request["productSku"];
                if (context.Request["from"] == "signBuy")
                {
                    foreach (ShoppingCartInfo info2 in ShoppingCartProcessor.GetListShoppingCart(productSkuId, num5))
                    {
                        builder.Append(this.ordersummit(info2, context, remark, shippingId, couponCode, strArray[0], context.Request["shippingType"], false, orderMarking, info2.LineItems, int.Parse(strArray2[0]), out activitiesIds));
                        str3 = activitiesIds + ",";
                    }
                }
            }
            else
            {
                List<ShoppingCartInfo> orderSummitCart = null;
                orderSummitCart = ShoppingCartProcessor.GetOrderSummitCart();
                string[] strArray3 = context.Request["shippingType"].Split(new char[] { ',' });
                string[] strArray4 = context.Request["remark"].Split(new char[] { ',' });
                int index = 0;
                int num7 = 0;
                foreach (ShoppingCartInfo info3 in orderSummitCart)
                {
                    foreach (ShoppingCartItemInfo info4 in info3.LineItems)
                    {
                        if (info4.Type == 1)
                        {
                            num7 += info4.PointNumber;
                        }
                    }
                }
                if (num7 > currentMember.Points)
                {
                    builder.Append("\"Status\":\"Eror\",\"ErrorMsg\":\"您当前积分不足！\"");
                }
                else
                {
                    foreach (ShoppingCartInfo info5 in orderSummitCart)
                    {
                        this.ordersummit(info5, context, strArray4[index], shippingId, couponCode, strArray[index], strArray3[index], true, orderMarking, info5.LineItems, int.Parse(strArray2[index]), out activitiesIds);
                        str3 = str3 + activitiesIds + ",";
                        index++;
                    }
                    builder.Append("\"Status\":\"OK\",\"OrderMarkingStatus\":\"1\",");
                    builder.AppendFormat("\"OrderId\":\"{0}\"", orderMarking);
                }
                if (!string.IsNullOrEmpty(str3))
                {
                    foreach (string str8 in str3.Substring(0, str3.Length - 1).Split(new char[] { ',' }))
                    {
                        if (!string.IsNullOrEmpty(str8) && (int.Parse(str8) > 0))
                        {
                            int activitiesId = ActivityHelper.GetHishop_Activities(int.Parse(str8));
                            int userId = currentMember.UserId;
                            ActivityHelper.AddActivitiesMember(activitiesId, userId);
                        }
                    }
                }
            }
            builder.Append("}");
            context.Response.ContentType = "application/json";
            context.Response.Write(builder.ToString());
        }
        /// <summary>
        /// 注册到当前网站
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="passagain"></param>
        /// <param name="openId"></param>
        /// <param name="headimgurl"></param>
        /// <param name="referralUserId"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public string regist(string userName, string password, string passagain, string openId, string headimgurl, string referralUserId, HttpContext context)
        {
            if (!(password == passagain))
            {
                return "\"Status\":\"-2\"";
            }
            MemberInfo info = new MemberInfo();
            if (MemberProcessor.GetusernameMember(userName,this.wid) != null)
            {
                return "\"Status\":\"-1\"";
            }
            MemberInfo member = new MemberInfo();
            string generateId = Globals.GetGenerateId();
            member.GradeId = MemberProcessor.GetDefaultMemberGrade(this.wid);
            member.OpenId = openId;
            member.UserHead = headimgurl;
            member.UserName = userName;
            member.ReferralUserId = string.IsNullOrEmpty(referralUserId) ? 0 : Convert.ToInt32(referralUserId);
            member.CreateDate = DateTime.Now;
            member.SessionId = generateId;
            member.SessionEndTime = DateTime.Now.AddDays(10);
            member.Password = HiCryptographer.Md5Encrypt(password);
            member.UserBindName = userName;

            //注册到当前网站
            member.wid = this.wid;
            if (MemberProcessor.CreateMember(member))
            {
                this.myNotifier.updateAction = UpdateAction.MemberUpdate;
                this.myNotifier.actionDesc = "会员注册";
                this.myNotifier.RecDateUpdate = DateTime.Today;
                this.myNotifier.DataUpdated += new StatisticNotifier.DataUpdatedEventHandler(this.myEvent.Update);
                this.myNotifier.UpdateDB();
            }
            MemberInfo info3 = MemberProcessor.GetMember(generateId);
            if (HttpContext.Current.Request.Cookies["Vshop-Member"] != null)
            {
                HttpContext.Current.Response.Cookies["Vshop-Member"].Expires = DateTime.Now.AddDays(-1.0);
                HttpCookie cookie = new HttpCookie("Vshop-Member")
                {
                    Value = info3.UserId.ToString(),
                    Expires = DateTime.Now.AddDays(10)
                };
                HttpContext.Current.Response.Cookies.Add(cookie);
            }
            else
            {
                HttpCookie cookie3 = new HttpCookie("Vshop-Member")
                {
                    Value = info3.UserId.ToString(),
                    Expires = DateTime.Now.AddDays(10)
                };
                HttpContext.Current.Response.Cookies.Add(cookie3);
            }
            context.Session["userid"] = info3.UserId.ToString();
            return ("\"Status\":\"OK\",\"referralUserId\":" + member.ReferralUserId);
        }

        public void RegisterUser(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            string userName = context.Request["userName"];
            string password = context.Request["password"];
            string passagain = context.Request["passagain"];
            string str4 = context.Request["openId"];
            string headimgurl = context.Request["headimgurl"];
            string referralUserId = "";
            HttpCookie cookie = HttpContext.Current.Request.Cookies["Vshop-ReferralId"];
            if (cookie != null)
            {
                referralUserId = cookie.Value;
            }
            else
            {
                referralUserId = "0";
            }
            StringBuilder builder = new StringBuilder();
            builder.Append("{");
            if (!string.IsNullOrEmpty(str4))
            {
                if (MemberProcessor.GetOpenIdMember(str4) == null)
                {
                    string str7 = this.regist(userName, password, passagain, str4, headimgurl, referralUserId, context);
                    builder.Append(str7);
                }
                else
                {
                    builder.Append("\"Status\":\"-3\"");
                }
            }
            else
            {
                string str8 = this.regist(userName, password, passagain, str4, headimgurl, referralUserId, context);
                builder.Append(str8);
            }
            builder.Append("}");
            context.Response.Write(builder.ToString());
        }

        public void RequestReturn(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            decimal num = decimal.Parse(context.Request["Money"]);
            RefundInfo refundInfo = new RefundInfo
            {
                OrderId = context.Request["orderid"],
                ApplyForTime = DateTime.Now,
                RefundRemark = context.Request["Reason"],
                HandleStatus = RefundInfo.Handlestatus.NoneAudit,
                Account = context.Request["Account"],
                RefundMoney = num,
                ProductId = int.Parse(context.Request["productid"])
            };
            StringBuilder builder = new StringBuilder();
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            refundInfo.UserId = currentMember.UserId;
            int orderItemsStatus = 7;
            refundInfo.RefundType = 1;
            if (int.Parse(context.Request["OrderStatus"].ToString()) == 2)
            {
                orderItemsStatus = 6;
                refundInfo.HandleStatus = RefundInfo.Handlestatus.NoRefund;
                refundInfo.RefundType = 2;
                refundInfo.AuditTime = DateTime.Now.ToString();
            }
            builder.Append("{");
            if (!string.IsNullOrEmpty(refundInfo.Account.Trim()))
            {
                if (!ShoppingProcessor.GetReturnInfo(refundInfo.UserId, refundInfo.OrderId, refundInfo.ProductId))
                {
                    if (ShoppingProcessor.InsertOrderRefund(refundInfo))
                    {
                        if (ShoppingProcessor.UpdateOrderGoodStatu(refundInfo.OrderId, context.Request["skuid"], orderItemsStatus))
                        {
                            try
                            {
                                this.myNotifier.updateAction = UpdateAction.OrderUpdate;
                                this.myNotifier.actionDesc = "申请退货或退款";
                                this.myNotifier.RecDateUpdate = DateTime.Today;
                                this.myNotifier.DataUpdated += new StatisticNotifier.DataUpdatedEventHandler(this.myEvent.Update);
                                this.myNotifier.UpdateDB();
                            }
                            catch (Exception)
                            {
                            }
                            builder.Append("\"Status\":\"OK\"");
                        }
                        else
                        {
                            builder.Append("\"Status\":\"Error\"");
                        }
                    }
                    else
                    {
                        builder.Append("\"Status\":\"Error\"");
                    }
                }
                else
                {
                    builder.Append("\"Status\":\"Repeat\"");
                }
            }
            else
            {
                builder.Append("\"Status\":\"Mesg\"");
            }
            builder.Append("}");
            context.Response.Write(builder.ToString());
        }

        public string resultstring(int userid, HttpContext context)
        {
            DistributorsInfo userIdDistributors = new DistributorsInfo();
            userIdDistributors = DistributorsBrower.GetUserIdDistributors(userid);
            if ((userIdDistributors != null) && (userIdDistributors.UserId > 0))
            {
                HttpContext.Current.Response.Cookies["Vshop-ReferralId"].Expires = DateTime.Now.AddDays(-1.0);
                HttpCookie cookie = new HttpCookie("Vshop-ReferralId")
                {
                    Value = Globals.UrlEncode(userIdDistributors.UserId.ToString()),
                    Expires = DateTime.Now.AddDays(1)
                };
                HttpContext.Current.Response.Cookies.Add(cookie);
            }
            HttpCookie cookie2 = HttpContext.Current.Request.Cookies["Vshop-Member"];
            if (cookie2 != null)
            {
                cookie2.Value = userid.ToString();
                HttpContext.Current.Response.Cookies.Set(cookie2);
                cookie2.Expires = DateTime.Now.AddDays(10);
                HttpContext.Current.Response.Cookies.Add(cookie2);
            }
            else
            {
                HttpCookie cookie3 = new HttpCookie("Vshop-Member")
                {
                    Value = Globals.UrlEncode(userid.ToString()),
                    Expires = DateTime.Now.AddDays(1)
                };
                HttpContext.Current.Response.Cookies.Add(cookie3);
            }
            context.Session["userid"] = userid.ToString();
            return "\"Status\":\"OK\"";
        }

        private void SearchExpressData(HttpContext context)
        {
            string s = string.Empty;
            if (!string.IsNullOrEmpty(context.Request["OrderId"]))
            {
                string orderId = context.Request["OrderId"];
                OrderInfo orderInfo = ShoppingProcessor.GetOrderInfo(orderId);
                if (((orderInfo != null) && ((orderInfo.OrderStatus == OrderStatus.SellerAlreadySent) || (orderInfo.OrderStatus == OrderStatus.Finished))) && !string.IsNullOrEmpty(orderInfo.ExpressCompanyAbb))
                {
                    s = Express.GetExpressData(orderInfo.ExpressCompanyAbb, orderInfo.ShipOrderNumber, 0);
                }
            }
            context.Response.ContentType = "application/json";
            context.Response.Write(s);
            context.Response.End();
        }

        private void SetDefaultShippingAddress(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            if (currentMember == null)
            {
                context.Response.Write("{\"success\":false}");
            }
            else
            {
                int userId = currentMember.UserId;
                if (MemberProcessor.SetDefaultShippingAddress(Convert.ToInt32(context.Request.Form["shippingid"]), userId))
                {
                    context.Response.Write("{\"success\":true}");
                }
                else
                {
                    context.Response.Write("{\"success\":false}");
                }
            }
        }

        public void SetDistributorMsg(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            currentMember.VipCardDate = new DateTime?(DateTime.Now);
            currentMember.CellPhone = context.Request["CellPhone"];
            currentMember.MicroSignal = context.Request["MicroSignal"];
            currentMember.RealName = context.Request["RealName"];
            StringBuilder builder = new StringBuilder();
            builder.Append("{");
            if (MemberProcessor.UpdateMember(currentMember))
            {
                builder.Append("\"Status\":\"OK\"");
            }
            else
            {
                builder.Append("\"Status\":\"Error\"");
            }
            builder.Append("}");
            context.Response.Write(builder.ToString());
        }

        public float setferight(float counttype, float FristNumber, float FristPrice, float AddNumber, float AddPrice)
        {
            float num = 0f;
            float num2 = counttype - FristNumber;
            if (num2 <= 0f)
            {
                return (num + FristPrice);
            }
            num += FristPrice;
            float num3 = num2 / AddNumber;
            float num4 = AddPrice / AddNumber;
            float num5 = num2 % AddNumber;
            return (num + ((num3 * AddPrice) + (num5 * num4)));
        }

        public void SetOrderItemStatus(OrderInfo order, decimal GoodDiscountAverage)
        {
            decimal num = 0M;
            Dictionary<string, LineItemInfo> lineItems = order.LineItems;
            LineItemInfo info = new LineItemInfo();
            foreach (KeyValuePair<string, LineItemInfo> pair in lineItems)
            {
                info = pair.Value;
                info.OrderItemsStatus = OrderStatus.WaitBuyerPay;
                if (info.Type == 0)
                {
                    num += info.GetSubTotal();
                }
            }
            if (lineItems.Count > 1)
            {
                if (GoodDiscountAverage > 0M)
                {
                    foreach (KeyValuePair<string, LineItemInfo> pair2 in lineItems)
                    {
                        info = pair2.Value;
                        if (info.Type == 0)
                        {
                            float num2 = 0f;
                            num2 = (float.Parse(info.GetSubTotal().ToString()) / float.Parse(num.ToString())) * float.Parse(GoodDiscountAverage.ToString());
                            info.DiscountAverage = Convert.ToDecimal(num2);
                        }
                        else
                        {
                            info.DiscountAverage = 0M;
                        }
                    }
                }
            }
            else if (lineItems.Count == 1)
            {
                using (Dictionary<string, LineItemInfo>.Enumerator enumerator3 = lineItems.GetEnumerator())
                {
                    while (enumerator3.MoveNext())
                    {
                        KeyValuePair<string, LineItemInfo> current = enumerator3.Current;
                        if (info.Type == 0)
                        {
                            info.DiscountAverage = GoodDiscountAverage;
                        }
                        else
                        {
                            info.DiscountAverage = 0M;
                        }
                    }
                }
            }
        }

        public void SetUserName(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            currentMember.UserName = context.Request["userName"];
            currentMember.VipCardDate = new DateTime?(DateTime.Now);
            currentMember.CellPhone = context.Request["CellPhone"];
            currentMember.QQ = context.Request["QQ"];
            if (!string.IsNullOrEmpty(currentMember.QQ))
            {
                currentMember.Email = currentMember.QQ + "@qq.com";
            }
            currentMember.RealName = context.Request["RealName"];
            new DistributorsInfo();
            StringBuilder builder = new StringBuilder();
            builder.Append("{");
            if (MemberProcessor.UpdateMember(currentMember))
            {
                builder.Append("\"Status\":\"OK\"");
            }
            else
            {
                builder.Append("\"Status\":\"Error\"");
            }
            builder.Append("}");
            context.Response.Write(builder.ToString());
        }

        public void SignToday(HttpContext context)
        {
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            if (currentMember == null)
            {
                context.Response.Write("未找到会员信息");
            }
            else if (UserSignHelper.IsSign(currentMember.UserId))
            {
                int num = UserSignHelper.USign(currentMember.UserId,currentMember.wid);
                context.Response.Write("suss" + num.ToString());
            }
            else
            {
                context.Response.Write("已签到");
            }
        }

        private void SubmitActivity(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            if (currentMember == null)
            {
                context.Response.Write("{\"success\":false}");
            }
            else
            {
                Hidistro.Entities.VShop.ActivityInfo activity = VshopBrowser.GetActivity(Convert.ToInt32(context.Request.Form.Get("id")));
                if ((DateTime.Now < activity.StartDate) || (DateTime.Now > activity.EndDate))
                {
                    context.Response.Write("{\"success\":false, \"msg\":\"报名还未开始或已结束\"}");
                }
                else
                {
                    ActivitySignUpInfo info = new ActivitySignUpInfo
                    {
                        ActivityId = Convert.ToInt32(context.Request.Form.Get("id")),
                        Item1 = context.Request.Form.Get("item1"),
                        Item2 = context.Request.Form.Get("item2"),
                        Item3 = context.Request.Form.Get("item3"),
                        Item4 = context.Request.Form.Get("item4"),
                        Item5 = context.Request.Form.Get("item5"),
                        RealName = currentMember.RealName,
                        SignUpDate = DateTime.Now,
                        UserId = currentMember.UserId,
                        UserName = currentMember.UserName
                    };
                    string str = VshopBrowser.SaveActivitySignUp(info);
                    string s = "{\"success\":true}";
                    if (str != "1")
                    {
                        s = "{\"success\":false, \"msg\":\"" + str + "\"}";
                    }
                    context.Response.Write(s);
                }
            }
        }

        private void SubmitWinnerInfo(HttpContext context)
        {
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            if (currentMember == null)
            {
                context.Response.Write("{\"success\":false}");
            }
            else
            {
                int activityId = Convert.ToInt32(context.Request.Form.Get("id"));
                string realName = context.Request.Form.Get("name");
                string cellPhone = context.Request.Form.Get("phone");
                string s = VshopBrowser.UpdatePrizeRecord(activityId, currentMember.UserId, realName, cellPhone) ? "{\"success\":true}" : "{\"success\":false}";
                context.Response.ContentType = "application/json";
                context.Response.Write(s);
            }
        }

        private void UpdateDistributor(HttpContext context)
        {
            context.Response.ContentType = "text/json";
            StringBuilder sb = new StringBuilder();
            if (this.CheckUpdateDistributors(context, sb))
            {
                DistributorsInfo currentDistributors = DistributorsBrower.GetCurrentDistributors(Globals.GetCurrentMemberUserId());
                currentDistributors.StoreName = context.Request["stroename"].Trim();
                currentDistributors.StoreDescription = context.Request["descriptions"].Trim();
                currentDistributors.RequestAccount = context.Request["accountname"].Trim();
                currentDistributors.Logo = context.Request["logo"].Trim();
                currentDistributors.CellPhone = context.Request["CellPhone"].Trim();
                if (DistributorsBrower.UpdateDistributorMessage(currentDistributors))
                {
                    context.Response.Write("{\"success\":true}");
                }
                else
                {
                    context.Response.Write("{\"success\":false,\"msg\":\"店铺名称已存在，请重新命名!\"}");
                }
            }
            else
            {
                context.Response.Write("{\"success\":false,\"msg\":\"" + sb.ToString() + "\"}");
            }
        }

        private void UpdateShippingAddress(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            if (currentMember == null)
            {
                context.Response.Write("{\"success\":false}");
            }
            else
            {
                ShippingAddressInfo shippingAddress = new ShippingAddressInfo
                {
                    Address = context.Request.Form["address"],
                    CellPhone = context.Request.Form["cellphone"],
                    ShipTo = context.Request.Form["shipTo"],
                    Zipcode = "12345",
                    UserId = currentMember.UserId,
                    ShippingId = Convert.ToInt32(context.Request.Form["shippingid"]),
                    RegionId = Convert.ToInt32(context.Request.Form["regionSelectorValue"])
                };
                if (MemberProcessor.UpdateShippingAddress(shippingAddress))
                {
                    context.Response.Write("{\"success\":true}");
                }
                else
                {
                    context.Response.Write("{\"success\":false}");
                }
            }
        }

        private string UploadFileImages(HttpContext context, HttpPostedFile file)
        {
            string virtualPath = string.Empty;
            if ((file != null) && !string.IsNullOrEmpty(file.FileName))
            {
                string str2 = Globals.GetStoragePath() + "/Logo";
                string str3 = Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + Path.GetExtension(file.FileName);
                virtualPath = str2 + "/" + str3;
                string str4 = Path.GetExtension(file.FileName).ToLower();
                if ((!str4.Equals(".gif") && !str4.Equals(".jpg")) && (!str4.Equals(".png") && !str4.Equals(".bmp")))
                {
                    context.Response.Write("你上传的文件格式不正确！上传格式有(.gif、.jpg、.png、.bmp)");
                    context.Response.End();
                }
                if (file.ContentLength > 0x100000)
                {
                    context.Response.Write("你上传的文件不能大于1048576KB!请重新上传！");
                    context.Response.End();
                }
                file.SaveAs(context.Request.MapPath(virtualPath));
                return virtualPath;
            }
            context.Response.Write("图片上传失败!");
            context.Response.End();
            return virtualPath;
        }

        public void UserLogin(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            MemberInfo member = new MemberInfo();
            string str = context.Request["userName"];
            string sourceData = context.Request["password"];
            string str3 = context.Request["openId"];
            string str4 = context.Request["headimgurl"];
            StringBuilder builder = new StringBuilder();
            builder.Append("{");
            if (!string.IsNullOrEmpty(str))
            {
                //登录到当前网站
                member = MemberProcessor.GetusernameMember(str,this.wid);
                if (member == null)
                {
                    builder.Append("\"Status\":\"-1\"");
                    builder.Append("}");
                    context.Response.Write(builder.ToString());
                    return;
                }
                if (member.Status == Convert.ToInt32(UserStatus.DEL))
                {
                    builder.Append("\"Status\":\"-4\"");
                    builder.Append("}");
                    context.Response.Write(builder.ToString());
                    return;
                }
                if (member.Password == HiCryptographer.Md5Encrypt(sourceData))
                {
                    if (!string.IsNullOrEmpty(str3))
                    {
                        member.OpenId = str3;
                        member.UserHead = str4;
                        MemberProcessor.UpdateMember(member);
                    }
                    //如果是分销商
                    DistributorsInfo userIdDistributors = new DistributorsInfo();
                    userIdDistributors = DistributorsBrower.GetUserIdDistributors(member.UserId);
                    if ((userIdDistributors != null) && (userIdDistributors.UserId > 0))
                    {
                        HttpContext.Current.Response.Cookies["Vshop-ReferralId"].Expires = DateTime.Now.AddDays(-1.0);
                        HttpCookie cookie = new HttpCookie("Vshop-ReferralId")
                        {
                            Value = Globals.UrlEncode(userIdDistributors.UserId.ToString()),
                            Expires = DateTime.Now.AddDays(1)
                        };
                        HttpContext.Current.Response.Cookies.Add(cookie);
                    }
                    if (HttpContext.Current.Request.Cookies["Vshop-Member"] != null)
                    {
                        HttpContext.Current.Response.Cookies["Vshop-Member"].Expires = DateTime.Now.AddDays(-1.0);
                    }
                    HttpCookie cookie3 = new HttpCookie("Vshop-Member")
                    {
                        Value = Globals.UrlEncode(member.UserId.ToString()),
                        Expires = DateTime.Now.AddDays(1)
                    };
                    HttpContext.Current.Response.Cookies.Add(cookie3);
                    context.Session["userid"] = member.UserId.ToString();
                    builder.Append("\"Status\":\"OK\"");
                }
                else
                {
                    builder.Append("\"Status\":\"-2\"");
                }
            }
            else
            {
                builder.Append("\"Status\":\"-3\"");
            }
            builder.Append("}");
            context.Response.Write(builder.ToString());
        }

        private void Vote(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            int result = 1;
            int.TryParse(context.Request["voteId"], out result);
            string itemIds = context.Request["itemIds"];
            itemIds = itemIds.Remove(itemIds.Length - 1);
            if (MemberProcessor.GetCurrentMember() == null)
            {
                MemberInfo member = new MemberInfo();
                string generateId = Globals.GetGenerateId();
                member.GradeId = MemberProcessor.GetDefaultMemberGrade(this.wid);
                member.UserName = "";
                member.OpenId = "";
                member.CreateDate = DateTime.Now;
                member.SessionId = generateId;
                member.SessionEndTime = DateTime.Now;
                member.wid = this.wid;
                MemberProcessor.CreateMember(member);
                member = MemberProcessor.GetMember(generateId);
                HttpCookie cookie = new HttpCookie("Vshop-Member")
                {
                    Value = member.UserId.ToString(),
                    Expires = DateTime.Now.AddDays(10)
                };
                HttpContext.Current.Response.Cookies.Add(cookie);
            }
            StringBuilder builder = new StringBuilder();
            builder.Append("{");
            if (VshopBrowser.Vote(result, itemIds))
            {
                builder.Append("\"Status\":\"OK\"");
            }
            else
            {
                builder.Append("\"Status\":\"Error\"");
            }
            builder.Append("}");
            context.Response.Write(builder.ToString());
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

