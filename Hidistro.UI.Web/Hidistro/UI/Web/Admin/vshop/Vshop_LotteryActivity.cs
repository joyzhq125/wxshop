namespace Hidistro.UI.Web.Admin.vshop
{
    using ASPNET.WebControls;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Entities.VShop;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Collections.Generic;
    using System.Web.UI.WebControls;

    public class Vshop_LotteryActivity : AdminPage
    {
        protected Button btnAddActivity;
        protected WebCalendar calendarEndDate;
        protected WebCalendar calendarStartDate;
        protected CheckBox ChkOpen;
        protected FileUpload fileUpload;
        protected Literal Litdesc;
        protected Literal LitTitle;
        protected TextBox txtActiveName;
        protected TextBox txtdesc;
        protected TextBox txtKeyword;
        protected TextBox txtMaxNum;
        protected TextBox txtPrize1;
        protected TextBox txtPrize1Num;
        protected TextBox txtPrize2;
        protected TextBox txtPrize2Num;
        protected TextBox txtPrize3;
        protected TextBox txtPrize3Num;
        protected TextBox txtPrize4;
        protected TextBox txtPrize4Num;
        protected TextBox txtPrize5;
        protected TextBox txtPrize5Num;
        protected TextBox txtPrize6;
        protected TextBox txtPrize6Num;
        protected TextBox txtProbability1;
        protected TextBox txtProbability2;
        protected TextBox txtProbability3;
        protected TextBox txtProbability4;
        protected TextBox txtProbability5;
        protected TextBox txtProbability6;
        private int type;
        protected string wid;
        protected Vshop_LotteryActivity() : base("", "")
        {
        }

        protected void btnAddActivity_Click(object sender, EventArgs e)
        {
            if (ReplyHelper.HasReplyKey(this.txtKeyword.Text.Trim(),this.wid))
            {
                this.ShowMsg("关键字重复!", false);
            }
            else if (!this.calendarStartDate.SelectedDate.HasValue)
            {
                this.ShowMsg("请选择活动开始时间", false);
            }
            else if (!this.calendarEndDate.SelectedDate.HasValue)
            {
                this.ShowMsg("请选择活动结束时间", false);
            }
            else if (this.calendarStartDate.SelectedDate.Value.CompareTo(this.calendarEndDate.SelectedDate.Value) >= 0)
            {
                this.ShowMsg("开始时间不能晚于结束时间！", false);
            }
            else
            {
                int num;
                if (!int.TryParse(this.txtMaxNum.Text, out num) || (num.ToString() != this.txtMaxNum.Text))
                {
                    this.ShowMsg("可抽奖次数必须是整数", false);
                }
                else
                {
                    string str = string.Empty;
                    if (this.fileUpload.HasFile)
                    {
                        try
                        {
                            int num2;
                            str = VShopHelper.UploadTopicImage(this.fileUpload.PostedFile);
                            LotteryActivityInfo info = new LotteryActivityInfo {
                                ActivityName = this.txtActiveName.Text,
                                ActivityKey = this.txtKeyword.Text,
                                ActivityDesc = this.txtdesc.Text,
                                ActivityPic = str,
                                ActivityType = this.type,
                                StartTime = this.calendarStartDate.SelectedDate.Value,
                                EndTime = this.calendarEndDate.SelectedDate.Value,
                                MaxNum = Convert.ToInt32(this.txtMaxNum.Text)
                            };
                            List<PrizeSetting> list = new List<PrizeSetting>();
                            if ((int.TryParse(this.txtPrize1Num.Text, out num2) && int.TryParse(this.txtPrize2Num.Text, out num2)) && int.TryParse(this.txtPrize3Num.Text, out num2))
                            {
                                decimal num3 = Convert.ToDecimal(this.txtProbability1.Text);
                                decimal num4 = Convert.ToDecimal(this.txtProbability2.Text);
                                decimal num5 = Convert.ToDecimal(this.txtProbability3.Text);
                                List<PrizeSetting> collection = new List<PrizeSetting>();
                                PrizeSetting item = new PrizeSetting {
                                    PrizeName = this.txtPrize1.Text,
                                    PrizeNum = Convert.ToInt32(this.txtPrize1Num.Text),
                                    PrizeLevel = "一等奖",
                                    Probability = num3
                                };
                                collection.Add(item);
                                PrizeSetting setting2 = new PrizeSetting {
                                    PrizeName = this.txtPrize2.Text,
                                    PrizeNum = Convert.ToInt32(this.txtPrize2Num.Text),
                                    PrizeLevel = "二等奖",
                                    Probability = num4
                                };
                                collection.Add(setting2);
                                PrizeSetting setting3 = new PrizeSetting {
                                    PrizeName = this.txtPrize3.Text,
                                    PrizeNum = Convert.ToInt32(this.txtPrize3Num.Text),
                                    PrizeLevel = "三等奖",
                                    Probability = num5
                                };
                                collection.Add(setting3);
                                list.AddRange(collection);
                            }
                            else
                            {
                                this.ShowMsg("奖品数量必须为数字！", false);
                                return;
                            }
                            if (this.ChkOpen.Checked)
                            {
                                if ((string.IsNullOrEmpty(this.txtPrize4.Text) || string.IsNullOrEmpty(this.txtPrize5.Text)) || string.IsNullOrEmpty(this.txtPrize6.Text))
                                {
                                    this.ShowMsg("开启四五六名必须填写", false);
                                    return;
                                }
                                if ((int.TryParse(this.txtPrize4Num.Text, out num2) && int.TryParse(this.txtPrize5Num.Text, out num2)) && int.TryParse(this.txtPrize6Num.Text, out num2))
                                {
                                    decimal num6 = Convert.ToDecimal(this.txtProbability4.Text);
                                    decimal num7 = Convert.ToDecimal(this.txtProbability5.Text);
                                    decimal num8 = Convert.ToDecimal(this.txtProbability6.Text);
                                    List<PrizeSetting> list3 = new List<PrizeSetting>();
                                    PrizeSetting setting4 = new PrizeSetting {
                                        PrizeName = this.txtPrize4.Text,
                                        PrizeNum = Convert.ToInt32(this.txtPrize4Num.Text),
                                        PrizeLevel = "四等奖",
                                        Probability = num6
                                    };
                                    list3.Add(setting4);
                                    PrizeSetting setting5 = new PrizeSetting {
                                        PrizeName = this.txtPrize5.Text,
                                        PrizeNum = Convert.ToInt32(this.txtPrize5Num.Text),
                                        PrizeLevel = "五等奖",
                                        Probability = num7
                                    };
                                    list3.Add(setting5);
                                    PrizeSetting setting6 = new PrizeSetting {
                                        PrizeName = this.txtPrize6.Text,
                                        PrizeNum = Convert.ToInt32(this.txtPrize6Num.Text),
                                        PrizeLevel = "六等奖",
                                        Probability = num8
                                    };
                                    list3.Add(setting6);
                                    list.AddRange(list3);
                                }
                                else
                                {
                                    this.ShowMsg("奖品数量必须为数字！", false);
                                    return;
                                }
                            }
                            info.PrizeSettingList = list;
                            int num9 = VShopHelper.InsertLotteryActivity(info);
                            if (num9 > 0)
                            {
                                ReplyInfo reply = new TextReplyInfo {
                                    Keys = info.ActivityKey,
                                    MatchType = MatchType.Equal,
                                    ActivityId = num9
                                };
                                string str2 = ((LotteryActivityType) info.ActivityType).ToString();
                                object obj2 = Enum.Parse(typeof(ReplyType), str2);
                                reply.ReplyType = (ReplyType) obj2;
                                ReplyHelper.SaveReply(reply);
                                base.Response.Redirect("ManageLotteryActivity.aspx?type=" + this.type, true);
                                this.ShowMsg("添加成功！", true);
                            }
                            return;
                        }
                        catch
                        {
                            this.ShowMsg("图片上传失败，您选择的不是图片类型的文件，或者网站的虚拟目录没有写入文件的权限", false);
                            return;
                        }
                    }
                    this.ShowMsg("您没有选择上传的图片文件！", false);
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            wid = GetCurWebId();
            if (string.IsNullOrEmpty(wid)) return;
            if (!int.TryParse(base.Request.QueryString["type"], out this.type))
            {
                this.ShowMsg("参数错误", false);
            }
        }
    }
}

