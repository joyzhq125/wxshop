﻿namespace Hidistro.UI.Web.Admin.promotion
{
    using Hidistro.ControlPanel.Promotions;
    using Hidistro.Entities;
    using Hidistro.Entities.Promotions;
    using Hidistro.UI.ControlPanel.Utility;
    using Hidistro.UI.Web.Admin.Ascx;
    using Newtonsoft.Json;
    using System;
    using System.Data;
    using System.Linq;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class AddActivity : AdminPage
    {
        protected ActivityInfo _act;
        protected int _id;
        protected string _json;
        protected ucDateTimePicker calendarEndDate;
        protected ucDateTimePicker calendarStartDate;
        protected DropDownList ddl_maxNum;
        protected bool hasPartProductAct;
        protected bool isAllProduct;
        protected HtmlGenericControl productCount;
        protected HtmlForm thisForm;
        protected TextBox txt_name;

        protected AddActivity() : base("m08", "yxp05")
        {
            this._act = new ActivityInfo();
            this._json = "";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!base.IsPostBack)
            {
                for (int i = 1; i <= 10; i++)
                {
                    string text = "每人参与" + i.ToString() + "次";
                    this.ddl_maxNum.Items.Add(new ListItem(text, i.ToString()));
                }
                this.ddl_maxNum.Items.Add(new ListItem("不限", "0"));
                this.hasPartProductAct = ActivityHelper.HasPartProductAct();
            }
            if (base.Request.Params.AllKeys.Contains<string>("id") && base.Request["id"].ToString().bInt(ref this._id))
            {
                this._act = ActivityHelper.GetAct(this._id);
                if (this._act == null)
                {
                    this.ShowMsg("没有这个满减活动~", false);
                }
                this.txt_name.Text = this._act.ActivitiesName;
                this.calendarStartDate.SelectedDate = new DateTime?(this._act.StartTime);
                this.calendarEndDate.SelectedDate = new DateTime?(this._act.EndTime);
                this.ddl_maxNum.SelectedValue = this._act.attendTime.ToString();
                this._json = JsonConvert.SerializeObject(this._act.Details);
                this.isAllProduct = this._act.isAllProduct;
                DataTable table = ActivityHelper.QueryProducts(this._id);
                this.productCount.InnerText = (table != null) ? table.Rows.Count.ToString() : "0";
            }
        }
    }
}

