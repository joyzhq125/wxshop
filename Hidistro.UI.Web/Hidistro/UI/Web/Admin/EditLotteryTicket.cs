namespace Hidistro.UI.Web.Admin
{
    using ASPNET.WebControls;
    using Hidistro.ControlPanel.Members;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Entities.VShop;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Collections.Generic;
    using System.Web.UI.WebControls;

    public class EditLotteryTicket : AdminPage
    {
        private int activityid;
        protected Button btnUpdateActivity;
        protected WebCalendar calendarEndDate;
        protected WebCalendar calendarOpenDate;
        protected WebCalendar calendarStartDate;
        protected CheckBoxList cbList;
        protected CheckBox ChkOpen;
        protected DropDownList ddlHours;
        protected TextBox txtActiveName;
        protected TextBox txtCode;
        protected TextBox txtdesc;
        protected TextBox txtKeyword;
        protected TextBox txtMinValue;
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
        protected UpImg uploader1;
        protected string wid;
        protected EditLotteryTicket() : base("", "")
        {
        }

        protected void btnUpdateActivity_Click(object sender, EventArgs e)
        {
            if (!this.calendarStartDate.SelectedDate.HasValue)
            {
                this.ShowMsg("请选择活动开始时间", false);
            }
            else if (!this.calendarOpenDate.SelectedDate.HasValue)
            {
                this.ShowMsg("请选择抽奖开始时间", false);
            }
            else if (!this.calendarEndDate.SelectedDate.HasValue)
            {
                this.ShowMsg("请选择抽奖结束时间", false);
            }
            else if (this.calendarStartDate.SelectedDate.Value.CompareTo(this.calendarOpenDate.SelectedDate.Value) > 0)
            {
                this.ShowMsg("报名开始日期不能晚于抽奖开始时间！", false);
            }
            else if (this.calendarOpenDate.SelectedDate.Value.CompareTo(this.calendarEndDate.SelectedDate.Value) >= 0)
            {
                this.ShowMsg("抽奖开始时间不能晚于活动结束时间！", false);
            }
            else
            {
                string uploadedImageUrl = this.uploader1.UploadedImageUrl;
                if (string.IsNullOrEmpty(uploadedImageUrl))
                {
                    this.ShowMsg("请上传封面图片", false);
                }
                else
                {
                    string str2 = string.Empty;
                    for (int i = 0; i < this.cbList.Items.Count; i++)
                    {
                        if (this.cbList.Items[i].Selected)
                        {
                            str2 = str2 + "," + this.cbList.Items[i].Value;
                        }
                    }
                    if (string.IsNullOrEmpty(str2))
                    {
                        this.ShowMsg("请选择活动会员等级", false);
                    }
                    else
                    {
                        LotteryTicketInfo lotteryTicket = VShopHelper.GetLotteryTicket(this.activityid);
                        if ((lotteryTicket.ActivityKey != this.txtKeyword.Text.Trim()) && ReplyHelper.HasReplyKey(this.txtKeyword.Text.Trim(),this.wid))
                        {
                            this.ShowMsg("关键字重复!", false);
                        }
                        else
                        {
                            int num2;
                            lotteryTicket.GradeIds = str2;
                            lotteryTicket.MinValue = Convert.ToInt32(this.txtMinValue.Text);
                            lotteryTicket.InvitationCode = this.txtCode.Text.Trim();
                            lotteryTicket.ActivityName = this.txtActiveName.Text;
                            lotteryTicket.ActivityKey = this.txtKeyword.Text;
                            lotteryTicket.ActivityDesc = this.txtdesc.Text;
                            lotteryTicket.ActivityPic = uploadedImageUrl;
                            lotteryTicket.StartTime = this.calendarStartDate.SelectedDate.Value;
                            lotteryTicket.OpenTime = this.calendarOpenDate.SelectedDate.Value.AddHours((double) this.ddlHours.SelectedIndex);
                            lotteryTicket.EndTime = this.calendarEndDate.SelectedDate.Value;
                            if ((int.TryParse(this.txtPrize1Num.Text, out num2) && int.TryParse(this.txtPrize2Num.Text, out num2)) && int.TryParse(this.txtPrize3Num.Text, out num2))
                            {
                                lotteryTicket.PrizeSettingList.Clear();
                                PrizeSetting item = new PrizeSetting {
                                    PrizeName = this.txtPrize1.Text,
                                    PrizeNum = Convert.ToInt32(this.txtPrize1Num.Text),
                                    PrizeLevel = "一等奖"
                                };
                                lotteryTicket.PrizeSettingList.Add(item);
                                PrizeSetting setting2 = new PrizeSetting {
                                    PrizeName = this.txtPrize2.Text,
                                    PrizeNum = Convert.ToInt32(this.txtPrize2Num.Text),
                                    PrizeLevel = "二等奖"
                                };
                                lotteryTicket.PrizeSettingList.Add(setting2);
                                PrizeSetting setting3 = new PrizeSetting {
                                    PrizeName = this.txtPrize3.Text,
                                    PrizeNum = Convert.ToInt32(this.txtPrize3Num.Text),
                                    PrizeLevel = "三等奖"
                                };
                                lotteryTicket.PrizeSettingList.Add(setting3);
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
                                    PrizeSetting setting4 = new PrizeSetting {
                                        PrizeName = this.txtPrize4.Text,
                                        PrizeNum = Convert.ToInt32(this.txtPrize4Num.Text),
                                        PrizeLevel = "四等奖"
                                    };
                                    lotteryTicket.PrizeSettingList.Add(setting4);
                                    PrizeSetting setting5 = new PrizeSetting {
                                        PrizeName = this.txtPrize5.Text,
                                        PrizeNum = Convert.ToInt32(this.txtPrize5Num.Text),
                                        PrizeLevel = "五等奖"
                                    };
                                    lotteryTicket.PrizeSettingList.Add(setting5);
                                    PrizeSetting setting6 = new PrizeSetting {
                                        PrizeName = this.txtPrize6.Text,
                                        PrizeNum = Convert.ToInt32(this.txtPrize6Num.Text),
                                        PrizeLevel = "六等奖"
                                    };
                                    lotteryTicket.PrizeSettingList.Add(setting6);
                                }
                                else
                                {
                                    this.ShowMsg("奖品数量必须为数字！", false);
                                    return;
                                }
                            }
                            if (VShopHelper.UpdateLotteryTicket(lotteryTicket))
                            {
                                base.Response.Redirect("ManageLotteryTicket.aspx");
                            }
                        }
                    }
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            wid = GetCurWebId();
            if (string.IsNullOrEmpty(wid)) return;
            this.activityid = base.GetUrlIntParam("id");
            if (!this.Page.IsPostBack)
            {
                this.cbList.DataSource = MemberHelper.GetMemberGrades(this.wid);
                this.cbList.DataTextField = "Name";
                this.cbList.DataValueField = "GradeId";
                this.cbList.DataBind();
                this.RestoreLottery();
            }
        }

        public void RestoreLottery()
        {
            LotteryTicketInfo lotteryTicket = VShopHelper.GetLotteryTicket(this.activityid);
            if (lotteryTicket != null)
            {
                this.txtMinValue.Text = lotteryTicket.MinValue.ToString();
                this.txtCode.Text = lotteryTicket.InvitationCode;
                this.txtActiveName.Text = lotteryTicket.ActivityName;
                this.txtKeyword.Text = lotteryTicket.ActivityKey;
                this.txtdesc.Text = lotteryTicket.ActivityDesc;
                string activityPic = lotteryTicket.ActivityPic;
                if (activityPic != "")
                {
                    this.uploader1.UploadedImageUrl = activityPic;
                }
                this.calendarStartDate.SelectedDate = new DateTime?(lotteryTicket.StartTime);
                this.calendarOpenDate.SelectedDate = new DateTime?(lotteryTicket.OpenTime);
                this.ddlHours.SelectedIndex = lotteryTicket.OpenTime.Hour;
                this.calendarEndDate.SelectedDate = new DateTime?(lotteryTicket.EndTime);
                this.txtPrize1.Text = lotteryTicket.PrizeSettingList[0].PrizeName;
                this.txtPrize1Num.Text = lotteryTicket.PrizeSettingList[0].PrizeNum.ToString();
                this.txtPrize2.Text = lotteryTicket.PrizeSettingList[1].PrizeName;
                this.txtPrize2Num.Text = lotteryTicket.PrizeSettingList[1].PrizeNum.ToString();
                this.txtPrize3.Text = lotteryTicket.PrizeSettingList[2].PrizeName;
                this.txtPrize3Num.Text = lotteryTicket.PrizeSettingList[2].PrizeNum.ToString();
                if (lotteryTicket.PrizeSettingList.Count > 3)
                {
                    this.ChkOpen.Checked = true;
                    this.txtPrize4.Text = lotteryTicket.PrizeSettingList[3].PrizeName;
                    this.txtPrize4Num.Text = lotteryTicket.PrizeSettingList[3].PrizeNum.ToString();
                    this.txtPrize5.Text = lotteryTicket.PrizeSettingList[4].PrizeName;
                    this.txtPrize5Num.Text = lotteryTicket.PrizeSettingList[4].PrizeNum.ToString();
                    this.txtPrize6.Text = lotteryTicket.PrizeSettingList[5].PrizeName;
                    this.txtPrize6Num.Text = lotteryTicket.PrizeSettingList[5].PrizeNum.ToString();
                }
                if (!string.IsNullOrEmpty(lotteryTicket.GradeIds) && (lotteryTicket.GradeIds.Length > 1))
                {
                    List<string> list = new List<string>(lotteryTicket.GradeIds.Split(new char[] { ',' }));
                    for (int i = 0; i < this.cbList.Items.Count; i++)
                    {
                        if (list.Contains(this.cbList.Items[i].Value))
                        {
                            this.cbList.Items[i].Selected = true;
                        }
                    }
                }
            }
        }
    }
}

