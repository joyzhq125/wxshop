namespace Hidistro.UI.Web.Admin.promotion
{
    using Hidistro.ControlPanel.Promotions;
    using Hidistro.Entities;
    using Hidistro.Entities.Promotions;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using Hidistro.UI.Web.Admin.Ascx;
    using System;
    using System.Linq;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class PointExchange : AdminPage
    {
        protected bool bFinished;
        protected ucDateTimePicker calendarEndDate;
        protected ucDateTimePicker calendarStartDate;
        protected int eId;
        protected HiddenField hidpic;
        protected HiddenField hidpicdel;
        protected Button saveBtn;
        protected Script Script4;
        protected HtmlForm thisForm;
        protected HiddenField txt_Grades;
        protected TextBox txt_img;
        protected TextBox txt_name;

        protected PointExchange() : base("m08", "yxp02")
        {
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.saveBtn.Click += new EventHandler(this.saveBtn_Click);
            if ((base.Request.Params.AllKeys.Contains<string>("id") && base.Request["id"].ToString().bInt(ref this.eId)) && !this.Page.IsPostBack)
            {
                PointExChangeInfo info = PointExChangeHelper.Get(this.eId);
                this.txt_name.Text = info.Name;
                this.txt_Grades.Value = info.MemberGrades;
                this.calendarStartDate.SelectedDate = new DateTime?(info.BeginDate);
                this.calendarEndDate.SelectedDate = new DateTime?(info.EndDate);
                this.txt_img.Text = info.ImgUrl;
                this.hidpic.Value = info.ImgUrl;
                if (info.EndDate < DateTime.Now)
                {
                    this.bFinished = true;
                }
                else
                {
                    this.bFinished = false;
                }
            }
        }

        protected void saveBtn_Click(object sender, EventArgs e)
        {
            string text = this.txt_name.Text;
            string str2 = this.txt_Grades.Value;
            DateTime date = this.calendarStartDate.SelectedDate.Value.Date;
            DateTime time2 = this.calendarEndDate.SelectedDate.Value.Date.AddDays(1.0).AddSeconds(-1.0);
            string str3 = this.txt_img.Text;
            if (string.IsNullOrEmpty(text) || (text.Length > 30))
            {
                this.ShowMsg("请输入活动名称，长度不能超过30个字符！", false);
            }
            else if (string.IsNullOrEmpty(str2))
            {
                this.ShowMsg("请选择会员等级！", false);
            }
            else if (time2 < date)
            {
                this.ShowMsg("结束时间不能早于开始时间！", false);
            }
            else if (string.IsNullOrEmpty(str3))
            {
                this.ShowMsg("请上传封面图片！", false);
            }
            else
            {
                PointExChangeInfo exchange = new PointExChangeInfo();
                if (this.eId != 0)
                {
                    exchange = PointExChangeHelper.Get(this.eId);
                }
                exchange.BeginDate = date;
                exchange.EndDate = time2;
                exchange.Name = text;
                exchange.MemberGrades = str2;
                exchange.ImgUrl = str3;
                int eId = this.eId;
                string msg = "";
                if (this.eId == 0)
                {
                    exchange.ProductNumber = 0;
                    int num2 = PointExChangeHelper.Create(exchange, ref msg);
                    if (num2 == 0)
                    {
                        this.ShowMsg("保存失败(" + msg + ")", false);
                        return;
                    }
                    eId = num2;
                    this.ShowMsg("保存成功！", true);
                }
                else if (PointExChangeHelper.Update(exchange, ref msg))
                {
                    this.ShowMsg("保存成功！", true);
                }
                else
                {
                    this.ShowMsg("保存失败(" + msg + ")", false);
                    return;
                }
                base.Response.Redirect("AddProductToPointExchange.aspx?id=" + eId.ToString());
            }
        }
    }
}

