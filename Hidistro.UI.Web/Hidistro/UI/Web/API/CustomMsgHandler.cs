namespace Hidistro.UI.Web.API
{
    using Hidistro.ControlPanel.WeiBo;
    using Hidistro.ControlPanel.WeiXin;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    //using Hidistro.Entities.Promotions;
    using Hidistro.Entities.VShop;
    //using Hidistro.Entities.Weibo;
    using Hishop.Weixin.MP;
    using Hishop.Weixin.MP.Domain;
    using Hishop.Weixin.MP.Handler;
    using Hishop.Weixin.MP.Request;
    using Hishop.Weixin.MP.Request.Event;
    using Hishop.Weixin.MP.Response;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Web;

    public class CustomMsgHandler : RequestHandler
    {
        public CustomMsgHandler(Stream inputStream) : base(inputStream)
        {
        }

        public override AbstractResponse DefaultResponse(AbstractRequest requestMessage)
        {
            WeiXinHelper.UpdateRencentOpenID(requestMessage.FromUserName,this.wid);
            ReplyInfo mismatchReply = ReplyHelper.GetMismatchReply(this.wid);
            if ((mismatchReply == null) || this.IsOpenManyService())
            {
                return this.GotoManyCustomerService(requestMessage);
            }
            AbstractResponse response = this.GetResponse(mismatchReply, requestMessage.FromUserName);
            if (response == null)
            {
                return this.GotoManyCustomerService(requestMessage);
            }
            response.ToUserName = requestMessage.FromUserName;
            response.FromUserName = requestMessage.ToUserName;
            return response;
        }

        private AbstractResponse GetKeyResponse(string key, AbstractRequest request)
        {
            IList<ReplyInfo> replies = ReplyHelper.GetReplies(ReplyType.Topic, this.wid);
            if ((replies != null) && (replies.Count > 0))
            {
                foreach (ReplyInfo info in replies)
                {
                    if (info.Keys == key)
                    {
                        TopicInfo topic = VShopHelper.Gettopic(info.ActivityId);
                        if (topic != null)
                        {
                            NewsResponse response = new NewsResponse {
                                CreateTime = DateTime.Now,
                                FromUserName = request.ToUserName,
                                ToUserName = request.FromUserName,
                                Articles = new List<Article>()
                            };
                            Article item = new Article {
                                Description = topic.Title,
                                PicUrl = string.Format("http://{0}{1}", HttpContext.Current.Request.Url.Host, topic.IconUrl),
                                Title = topic.Title,
                                Url = string.Format("http://{0}/vshop/Topics.aspx?TopicId={1}", HttpContext.Current.Request.Url.Host, topic.TopicId)
                            };
                            response.Articles.Add(item);
                            return response;
                        }
                    }
                }
            }
            IList<ReplyInfo> list2 = ReplyHelper.GetReplies(ReplyType.Vote, this.wid);
            if ((list2 != null) && (list2.Count > 0))
            {
                foreach (ReplyInfo info3 in list2)
                {
                    if (info3.Keys == key)
                    {
                        Hidistro.Entities.Promotions.VoteInfo voteById = StoreHelper.GetVoteById((long) info3.ActivityId);
                        if ((voteById != null) && voteById.IsBackup)
                        {
                            NewsResponse response2 = new NewsResponse {
                                CreateTime = DateTime.Now,
                                FromUserName = request.ToUserName,
                                ToUserName = request.FromUserName,
                                Articles = new List<Article>()
                            };
                            Article article2 = new Article {
                                Description = voteById.VoteName,
                                PicUrl = string.Format("http://{0}{1}", HttpContext.Current.Request.Url.Host, voteById.ImageUrl),
                                Title = voteById.VoteName,
                                Url = string.Format("http://{0}/vshop/Vote.aspx?voteId={1}", HttpContext.Current.Request.Url.Host, voteById.VoteId)
                            };
                            response2.Articles.Add(article2);
                            return response2;
                        }
                    }
                }
            }
            IList<ReplyInfo> list3 = ReplyHelper.GetReplies(ReplyType.Wheel, this.wid);
            if ((list3 != null) && (list3.Count > 0))
            {
                foreach (ReplyInfo info5 in list3)
                {
                    if (info5.Keys == key)
                    {
                        LotteryActivityInfo lotteryActivityInfo = VShopHelper.GetLotteryActivityInfo(info5.ActivityId);
                        if (lotteryActivityInfo != null)
                        {
                            NewsResponse response3 = new NewsResponse {
                                CreateTime = DateTime.Now,
                                FromUserName = request.ToUserName,
                                ToUserName = request.FromUserName,
                                Articles = new List<Article>()
                            };
                            Article article3 = new Article {
                                Description = lotteryActivityInfo.ActivityDesc,
                                PicUrl = string.Format("http://{0}{1}", HttpContext.Current.Request.Url.Host, lotteryActivityInfo.ActivityPic),
                                Title = lotteryActivityInfo.ActivityName,
                                Url = string.Format("http://{0}/vshop/BigWheel.aspx?activityId={1}", HttpContext.Current.Request.Url.Host, lotteryActivityInfo.ActivityId)
                            };
                            response3.Articles.Add(article3);
                            return response3;
                        }
                    }
                }
            }
            IList<ReplyInfo> list4 = ReplyHelper.GetReplies(ReplyType.Scratch, this.wid);
            if ((list4 != null) && (list4.Count > 0))
            {
                foreach (ReplyInfo info7 in list4)
                {
                    if (info7.Keys == key)
                    {
                        LotteryActivityInfo info8 = VShopHelper.GetLotteryActivityInfo(info7.ActivityId);
                        if (info8 != null)
                        {
                            NewsResponse response4 = new NewsResponse {
                                CreateTime = DateTime.Now,
                                FromUserName = request.ToUserName,
                                ToUserName = request.FromUserName,
                                Articles = new List<Article>()
                            };
                            Article article4 = new Article {
                                Description = info8.ActivityDesc,
                                PicUrl = string.Format("http://{0}{1}", HttpContext.Current.Request.Url.Host, info8.ActivityPic),
                                Title = info8.ActivityName,
                                Url = string.Format("http://{0}/vshop/Scratch.aspx?activityId={1}", HttpContext.Current.Request.Url.Host, info8.ActivityId)
                            };
                            response4.Articles.Add(article4);
                            return response4;
                        }
                    }
                }
            }
            IList<ReplyInfo> list5 = ReplyHelper.GetReplies(ReplyType.SmashEgg, this.wid);
            if ((list5 != null) && (list5.Count > 0))
            {
                foreach (ReplyInfo info9 in list5)
                {
                    if (info9.Keys == key)
                    {
                        LotteryActivityInfo info10 = VShopHelper.GetLotteryActivityInfo(info9.ActivityId);
                        if (info10 != null)
                        {
                            NewsResponse response5 = new NewsResponse {
                                CreateTime = DateTime.Now,
                                FromUserName = request.ToUserName,
                                ToUserName = request.FromUserName,
                                Articles = new List<Article>()
                            };
                            Article article5 = new Article {
                                Description = info10.ActivityDesc,
                                PicUrl = string.Format("http://{0}{1}", HttpContext.Current.Request.Url.Host, info10.ActivityPic),
                                Title = info10.ActivityName,
                                Url = string.Format("http://{0}/vshop/SmashEgg.aspx?activityId={1}", HttpContext.Current.Request.Url.Host, info10.ActivityId)
                            };
                            response5.Articles.Add(article5);
                            return response5;
                        }
                    }
                }
            }
            IList<ReplyInfo> list6 = ReplyHelper.GetReplies(ReplyType.SignUp,this.wid);
            if ((list6 != null) && (list6.Count > 0))
            {
                foreach (ReplyInfo info11 in list6)
                {
                    if (info11.Keys == key)
                    {
                        ActivityInfo activity = VShopHelper.GetActivity(info11.ActivityId);
                        if (activity != null)
                        {
                            NewsResponse response6 = new NewsResponse {
                                CreateTime = DateTime.Now,
                                FromUserName = request.ToUserName,
                                ToUserName = request.FromUserName,
                                Articles = new List<Article>()
                            };
                            Article article6 = new Article {
                                Description = activity.Description,
                                PicUrl = string.Format("http://{0}{1}", HttpContext.Current.Request.Url.Host, activity.PicUrl),
                                Title = activity.Name,
                                Url = string.Format("http://{0}/vshop/Activity.aspx?id={1}", HttpContext.Current.Request.Url.Host, activity.ActivityId)
                            };
                            response6.Articles.Add(article6);
                            return response6;
                        }
                    }
                }
            }
            IList<ReplyInfo> list7 = ReplyHelper.GetReplies(ReplyType.Ticket, this.wid);
            if ((list7 != null) && (list7.Count > 0))
            {
                foreach (ReplyInfo info13 in list7)
                {
                    if (info13.Keys == key)
                    {
                        LotteryTicketInfo lotteryTicket = VShopHelper.GetLotteryTicket(info13.ActivityId);
                        if (lotteryTicket != null)
                        {
                            NewsResponse response7 = new NewsResponse {
                                CreateTime = DateTime.Now,
                                FromUserName = request.ToUserName,
                                ToUserName = request.FromUserName,
                                Articles = new List<Article>()
                            };
                            Article article7 = new Article {
                                Description = lotteryTicket.ActivityDesc,
                                PicUrl = string.Format("http://{0}{1}", HttpContext.Current.Request.Url.Host, lotteryTicket.ActivityPic),
                                Title = lotteryTicket.ActivityName,
                                Url = string.Format("http://{0}/vshop/SignUp.aspx?id={1}", HttpContext.Current.Request.Url.Host, lotteryTicket.ActivityId)
                            };
                            response7.Articles.Add(article7);
                            return response7;
                        }
                    }
                }
            }
            return null;
        }

        public AbstractResponse GetResponse(ReplyInfo reply, string openId)
        {
            if (reply.MessageType == MessageType.Text)
            {
                TextReplyInfo info = reply as TextReplyInfo;
                TextResponse response = new TextResponse {
                    CreateTime = DateTime.Now,
                    Content = info.Text
                   
                };
                if (reply.Keys == "登录")
                {
                    string str = string.Format("http://{0}/Vshop/Login.aspx?SessionId={1}", HttpContext.Current.Request.Url.Host, openId);
                    response.Content = response.Content.Replace("$login$", string.Format("<a href=\"{0}\">一键登录</a>", str));
                }
                return response;
            }
            NewsResponse response2 = new NewsResponse {
                CreateTime = DateTime.Now,
                Articles = new List<Article>()
            };
            if (reply.ArticleID > 0)
            {
                Hidistro.Entities.Weibo.ArticleInfo articleInfo = ArticleHelper.GetArticleInfo(reply.ArticleID);
                if (articleInfo.ArticleType == Hidistro.Entities.Weibo.ArticleType.News)
                {
                    Article item = new Article {
                        Description = articleInfo.Memo,
                        PicUrl = string.Format("http://{0}{1}", HttpContext.Current.Request.Url.Host, articleInfo.ImageUrl),
                        Title = articleInfo.Title,
                        Url = string.IsNullOrEmpty(articleInfo.Url) ? string.Format("http://{0}/Vshop/ArticleDetail.aspx?sid={1}", HttpContext.Current.Request.Url.Host, articleInfo.ArticleId) : articleInfo.Url
                    };
                    response2.Articles.Add(item);
                    return response2;
                }
                if (articleInfo.ArticleType == Hidistro.Entities.Weibo.ArticleType.List)
                {
                    Article article3 = new Article {
                        Description = articleInfo.Memo,
                        PicUrl = string.Format("http://{0}{1}", HttpContext.Current.Request.Url.Host, articleInfo.ImageUrl),
                        Title = articleInfo.Title,
                        Url = string.IsNullOrEmpty(articleInfo.Url) ? string.Format("http://{0}/Vshop/ArticleDetail.aspx?sid={1}", HttpContext.Current.Request.Url.Host, articleInfo.ArticleId) : articleInfo.Url
                    };
                    response2.Articles.Add(article3);
                    foreach (Hidistro.Entities.Weibo.ArticleItemsInfo info3 in articleInfo.ItemsInfo)
                    {
                        article3 = new Article {
                            Description = "",
                            PicUrl = string.Format("http://{0}{1}", HttpContext.Current.Request.Url.Host, info3.ImageUrl),
                            Title = info3.Title,
                            Url = string.IsNullOrEmpty(info3.Url) ? string.Format("http://{0}/Vshop/ArticleDetail.aspx?iid={1}", HttpContext.Current.Request.Url.Host, info3.Id) : info3.Url
                        };
                        response2.Articles.Add(article3);
                    }
                }
                return response2;
            }
            foreach (NewsMsgInfo info4 in (reply as NewsReplyInfo).NewsMsg)
            {
                Article article6 = new Article {
                    Description = info4.Description,
                    PicUrl = string.Format("http://{0}{1}", HttpContext.Current.Request.Url.Host, info4.PicUrl),
                    Title = info4.Title,
                    Url = string.IsNullOrEmpty(info4.Url) ? string.Format("http://{0}/Vshop/ImageTextDetails.aspx?messageId={1}", HttpContext.Current.Request.Url.Host, info4.Id) : info4.Url
                };
                response2.Articles.Add(article6);
            }
            return response2;
        }

        public AbstractResponse GotoManyCustomerService(AbstractRequest requestMessage)
        {
            WeiXinHelper.UpdateRencentOpenID(requestMessage.FromUserName,this.wid);
            if (!this.IsOpenManyService())
            {
                return null;
            }
            return new AbstractResponse { FromUserName = requestMessage.ToUserName, ToUserName = requestMessage.FromUserName, MsgType = ResponseMsgType.transfer_customer_service };
        }

        public bool IsOpenManyService()
        {
            return SettingsManager.GetMasterSettings(false,wid).OpenManyService;
        }

        public override AbstractResponse OnEvent_ClickRequest(ClickEventRequest clickEventRequest)
        {
            WeiXinHelper.UpdateRencentOpenID(clickEventRequest.FromUserName,this.wid);
            MenuInfo menu = VShopHelper.GetMenu(Convert.ToInt32(clickEventRequest.EventKey),this.wid);
            if (menu == null)
            {
                return null;
            }
            ReplyInfo reply = ReplyHelper.GetReply(menu.ReplyId);
            if (reply == null)
            {
                return null;
            }
            AbstractResponse keyResponse = this.GetKeyResponse(reply.Keys, clickEventRequest);
            if (keyResponse != null)
            {
                return keyResponse;
            }
            AbstractResponse response = this.GetResponse(reply, clickEventRequest.FromUserName);
            if (response == null)
            {
                this.GotoManyCustomerService(clickEventRequest);
            }
            response.ToUserName = clickEventRequest.FromUserName;
            response.FromUserName = clickEventRequest.ToUserName;
            return response;
        }

        public override AbstractResponse OnEvent_MassendJobFinishEventRequest(MassendJobFinishEventRequest massendJobFinishEventRequest)
        {
            string returnjsondata = string.Concat(new object[] { "公众号的微信号(加密的):", massendJobFinishEventRequest.ToUserName, ",发送完成时间：", massendJobFinishEventRequest.CreateTime, "，过滤通过条数：", massendJobFinishEventRequest.FilterCount, "，发送失败的粉丝数：", massendJobFinishEventRequest.ErrorCount });
            switch (massendJobFinishEventRequest.Status)
            {
                case "send success":
                    returnjsondata = returnjsondata + "(发送成功)";
                    break;

                case "send fail":
                    returnjsondata = returnjsondata + "(发送失败)";
                    break;

                case "err(10001)":
                    returnjsondata = returnjsondata + "(涉嫌广告)";
                    break;

                case "err(20001)":
                    returnjsondata = returnjsondata + "(涉嫌政治)";
                    break;

                case "err(20004)":
                    returnjsondata = returnjsondata + "(涉嫌社会)";
                    break;

                case "err(20002)":
                    returnjsondata = returnjsondata + "(涉嫌色情)";
                    break;

                case "err(20006)":
                    returnjsondata = returnjsondata + "(涉嫌违法犯罪)";
                    break;

                case "err(20008)":
                    returnjsondata = returnjsondata + "(涉嫌欺诈)";
                    break;

                case "err(20013)":
                    returnjsondata = returnjsondata + "(涉嫌版权)";
                    break;

                case "err(22000)":
                    returnjsondata = returnjsondata + "(涉嫌互相宣传)";
                    break;

                case "err(21000)":
                    returnjsondata = returnjsondata + "(涉嫌其他)";
                    break;

                default:
                    returnjsondata = returnjsondata + "(" + massendJobFinishEventRequest.Status + ")";
                    break;
            }
            WeiXinHelper.UpdateMsgId(0, massendJobFinishEventRequest.MsgId.ToString(), (massendJobFinishEventRequest.Status == "send success") ? 1 : 2, Globals.ToNum(massendJobFinishEventRequest.SentCount), Globals.ToNum(massendJobFinishEventRequest.TotalCount), returnjsondata);
            return null;
        }

        public override AbstractResponse OnEvent_SubscribeRequest(SubscribeEventRequest subscribeEventRequest)
        {
            WeiXinHelper.UpdateRencentOpenID(subscribeEventRequest.FromUserName,this.wid);
            ReplyInfo subscribeReply = ReplyHelper.GetSubscribeReply(this.wid);
            if (subscribeReply == null)
            {
                return null;
            }
            subscribeReply.Keys = "登录";
            AbstractResponse response = this.GetResponse(subscribeReply, subscribeEventRequest.FromUserName);
            if (response == null)
            {
                this.GotoManyCustomerService(subscribeEventRequest);
            }
            response.ToUserName = subscribeEventRequest.FromUserName;
            response.FromUserName = subscribeEventRequest.ToUserName;
            return response;
        }

        public override AbstractResponse OnTextRequest(TextRequest textRequest)
        {
            WeiXinHelper.UpdateRencentOpenID(textRequest.FromUserName,this.wid);
            AbstractResponse keyResponse = this.GetKeyResponse(textRequest.Content, textRequest);
            if (keyResponse != null)
            {
                return keyResponse;
            }
            IList<ReplyInfo> replies = ReplyHelper.GetReplies(ReplyType.Keys, this.wid);
            if ((replies == null) || ((replies.Count == 0) && this.IsOpenManyService()))
            {
                this.GotoManyCustomerService(textRequest);
            }
            foreach (ReplyInfo info in replies)
            {
                if ((info.MatchType == MatchType.Equal) && (info.Keys == textRequest.Content))
                {
                    AbstractResponse response = this.GetResponse(info, textRequest.FromUserName);
                    response.ToUserName = textRequest.FromUserName;
                    response.FromUserName = textRequest.ToUserName;
                    return response;
                }
                if ((info.MatchType == MatchType.Like) && info.Keys.Contains(textRequest.Content))
                {
                    AbstractResponse response3 = this.GetResponse(info, textRequest.FromUserName);
                    response3.ToUserName = textRequest.FromUserName;
                    response3.FromUserName = textRequest.ToUserName;
                    return response3;
                }
            }
            return this.DefaultResponse(textRequest);
        }
    }
}

