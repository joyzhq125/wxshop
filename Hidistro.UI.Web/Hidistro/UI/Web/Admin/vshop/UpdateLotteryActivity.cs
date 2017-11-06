namespace Hidistro.UI.Web.Admin.vshop
{
    using ASPNET.WebControls;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Entities.VShop;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Collections.Generic;
    using System.Web.UI.WebControls;

    public class UpdateLotteryActivity : AdminPage
    {
        private int activityid;
        protected ImageLinkButton btnPicDelete;
        protected Button btnUpdateActivity;
        protected WebCalendar calendarEndDate;
        protected WebCalendar calendarStartDate;
        protected CheckBox ChkOpen;
        protected FileUpload fileUpload;
        protected HiImage imgPic;
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
        protected UpdateLotteryActivity() : base("", "")
        {
        }

        protected void btnPicDelete_Click(object sender, EventArgs e)
        {
            try
            {
                ResourcesHelper.DeleteImage(this.imgPic.ImageUrl);
                this.imgPic.ImageUrl = "";
                this.btnPicDelete.Visible = !string.IsNullOrEmpty(this.imgPic.ImageUrl);
                this.imgPic.Visible = !string.IsNullOrEmpty(this.imgPic.ImageUrl);
            }
            catch
            {
            }
        }

        protected void btnUpdateActivity_Click(object sender, EventArgs e)
        {
            if ((VShopHelper.GetLotteryActivityInfo(this.activityid).ActivityKey != this.txtKeyword.Text.Trim()) && ReplyHelper.HasReplyKey(this.txtKeyword.Text.Trim(),this.wid))
            {
                this.ShowMsg("关键字重复!", false);
                return;
            }
            string imageUrl = string.Empty;
            if (this.fileUpload.HasFile)
            {
                try
                {
                    imageUrl = VShopHelper.UploadTopicImage(this.fileUpload.PostedFile);
                    goto Label_00AB;
                }
                catch
                {
                    this.ShowMsg("图片上传失败，您选择的不是图片类型的文件，或者网站的虚拟目录没有写入文件的权限", false);
                    return;
                }
            }
            if (!string.IsNullOrEmpty(this.imgPic.ImageUrl))
            {
                imageUrl = this.imgPic.ImageUrl;
            }
            else
            {
                this.ShowMsg("您没有选择上传的图片文件！", false);
                return;
            }
        Label_00AB:
            if (!this.calendarStartDate.SelectedDate.HasValue)
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
                    int num3;
                    LotteryActivityInfo info = new LotteryActivityInfo {
                        ActivityName = this.txtActiveName.Text,
                        ActivityId = this.activityid,
                        ActivityType = this.type,
                        ActivityKey = this.txtKeyword.Text,
                        ActivityDesc = this.txtdesc.Text,
                        ActivityPic = imageUrl,
                        StartTime = this.calendarStartDate.SelectedDate.Value,
                        EndTime = this.calendarEndDate.SelectedDate.Value
                    };
                    int num2 = Convert.ToInt32(this.txtMaxNum.Text);
                    if (num2 < 1)
                    {
                        num2 = 1;
                    }
                    info.MaxNum = num2;
                    List<PrizeSetting> list = new List<PrizeSetting>();
                    if ((int.TryParse(this.txtPrize1Num.Text, out num3) && int.TryParse(this.txtPrize2Num.Text, out num3)) && int.TryParse(this.txtPrize3Num.Text, out num3))
                    {
                        decimal num4 = Convert.ToDecimal(this.txtProbability1.Text);
                        decimal num5 = Convert.ToDecimal(this.txtProbability2.Text);
                        decimal num6 = Convert.ToDecimal(this.txtProbability3.Text);
                        List<PrizeSetting> collection = new List<PrizeSetting>();
                        PrizeSetting item = new PrizeSetting {
                            PrizeName = this.txtPrize1.Text,
                            PrizeNum = Convert.ToInt32(this.txtPrize1Num.Text),
                            PrizeLevel = "一等奖",
                            Probability = num4
                        };
                        collection.Add(item);
                        PrizeSetting setting2 = new PrizeSetting {
                            PrizeName = this.txtPrize2.Text,
                            PrizeNum = Convert.ToInt32(this.txtPrize2Num.Text),
                            PrizeLevel = "二等奖",
                            Probability = num5
                        };
                        collection.Add(setting2);
                        PrizeSetting setting3 = new PrizeSetting {
                            PrizeName = this.txtPrize3.Text,
                            PrizeNum = Convert.ToInt32(this.txtPrize3Num.Text),
                            PrizeLevel = "三等奖",
                            Probability = num6
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
                        if ((int.TryParse(this.txtPrize4Num.Text, out num3) && int.TryParse(this.txtPrize5Num.Text, out num3)) && int.TryParse(this.txtPrize6Num.Text, out num3))
                        {
                            decimal num7 = Convert.ToDecimal(this.txtProbability4.Text);
                            decimal num8 = Convert.ToDecimal(this.txtProbability5.Text);
                            decimal num9 = Convert.ToDecimal(this.txtProbability6.Text);
                            List<PrizeSetting> list3 = new List<PrizeSetting>();
                            PrizeSetting setting4 = new PrizeSetting {
                                PrizeName = this.txtPrize4.Text,
                                PrizeNum = Convert.ToInt32(this.txtPrize4Num.Text),
                                PrizeLevel = "四等奖",
                                Probability = num7
                            };
                            list3.Add(setting4);
                            PrizeSetting setting5 = new PrizeSetting {
                                PrizeName = this.txtPrize5.Text,
                                PrizeNum = Convert.ToInt32(this.txtPrize5Num.Text),
                                PrizeLevel = "五等奖",
                                Probability = num8
                            };
                            list3.Add(setting5);
                            PrizeSetting setting6 = new PrizeSetting {
                                PrizeName = this.txtPrize6.Text,
                                PrizeNum = Convert.ToInt32(this.txtPrize6Num.Text),
                                PrizeLevel = "六等奖",
                                Probability = num9
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
                    if (VShopHelper.UpdateLotteryActivity(info))
                    {
                        this.imgPic.ImageUrl = imageUrl;
                        base.Response.Redirect("ManageLotteryActivity.aspx?type=" + this.type, true);
                    }
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            wid = GetCurWebId();
            if (string.IsNullOrEmpty(wid)) return;
            if (int.TryParse(base.Request.QueryString["typeid"], out this.type) && int.TryParse(base.Request.QueryString["id"], out this.activityid))
            {
                if (!this.Page.IsPostBack)
                {
                    this.RestoreLottery();
                }
            }
            else
            {
                this.ShowMsg("参数错误", false);
            }
        }

        public void RestoreLottery()
        {
            LotteryActivityInfo lotteryActivityInfo = VShopHelper.GetLotteryActivityInfo(this.activityid);
            this.txtActiveName.Text = lotteryActivityInfo.ActivityName;
            this.txtKeyword.Text = lotteryActivityInfo.ActivityKey;
            this.txtdesc.Text = lotteryActivityInfo.ActivityDesc;
            this.imgPic.ImageUrl = lotteryActivityInfo.ActivityPic;
            this.btnPicDelete.Visible = !string.IsNullOrEmpty(this.imgPic.ImageUrl);
            this.calendarStartDate.SelectedDate = new DateTime?(lotteryActivityInfo.StartTime);
            this.calendarEndDate.SelectedDate = new DateTime?(lotteryActivityInfo.EndTime);
            this.txtMaxNum.Text = lotteryActivityInfo.MaxNum.ToString();
            this.txtPrize1.Text = lotteryActivityInfo.PrizeSettingList[0].PrizeName;
            this.txtPrize1Num.Text = lotteryActivityInfo.PrizeSettingList[0].PrizeNum.ToString();
            this.txtProbability1.Text = lotteryActivityInfo.PrizeSettingList[0].Probability.ToString();
            this.txtPrize2.Text = lotteryActivityInfo.PrizeSettingList[1].PrizeName;
            this.txtPrize2Num.Text = lotteryActivityInfo.PrizeSettingList[1].PrizeNum.ToString();
            this.txtProbability2.Text = lotteryActivityInfo.PrizeSettingList[1].Probability.ToString();
            this.txtPrize3.Text = lotteryActivityInfo.PrizeSettingList[2].PrizeName;
            this.txtPrize3Num.Text = lotteryActivityInfo.PrizeSettingList[2].PrizeNum.ToString();
            this.txtProbability3.Text = lotteryActivityInfo.PrizeSettingList[2].Probability.ToString();
            if (lotteryActivityInfo.PrizeSettingList.Count > 3)
            {
                this.ChkOpen.Checked = true;
                this.txtPrize4.Text = lotteryActivityInfo.PrizeSettingList[3].PrizeName;
                this.txtPrize4Num.Text = lotteryActivityInfo.PrizeSettingList[3].PrizeNum.ToString();
                this.txtProbability4.Text = lotteryActivityInfo.PrizeSettingList[3].Probability.ToString();
                this.txtPrize5.Text = lotteryActivityInfo.PrizeSettingList[4].PrizeName;
                this.txtPrize5Num.Text = lotteryActivityInfo.PrizeSettingList[4].PrizeNum.ToString();
                this.txtProbability5.Text = lotteryActivityInfo.PrizeSettingList[4].Probability.ToString();
                this.txtPrize6.Text = lotteryActivityInfo.PrizeSettingList[5].PrizeName;
                this.txtPrize6Num.Text = lotteryActivityInfo.PrizeSettingList[5].PrizeNum.ToString();
                this.txtProbability6.Text = lotteryActivityInfo.PrizeSettingList[5].Probability.ToString();
            }
        }
    }
}

