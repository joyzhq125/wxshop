namespace Hidistro.UI.Web.Admin.promotion
{
    using ASPNET.WebControls;
    using Hidistro.ControlPanel.Promotions;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities;
    using Hidistro.Entities.Promotions;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Data;
    using System.Linq;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class ShareActList : AdminPage
    {
        protected Button btnSeach;
        protected Button DelBtn;
        protected Repeater grdDate;
        protected Label lblAll;
        protected Label lblEnd;
        protected Label lblIn;
        protected Label lblUnBegin;
        protected Pager pager1;
        protected PageSize PageSize1;
        protected ShareActivityStatus status;
        protected HtmlForm thisForm;
        protected TextBox txt_Ids;
        protected TextBox txt_name;

        protected ShareActList() : base("m08", "yxp04")
        {
        }

        private void BindData()
        {
            string name = this.txt_name.Text.Trim();
            ShareActivitySearch query = new ShareActivitySearch {
                status = this.status,
                IsCount = true,
                PageIndex = this.pager1.PageIndex,
                PageSize = this.pager1.PageSize,
                SortBy = "Id",
                SortOrder = SortAction.Desc,
                CouponName = name
            };
            DbQueryResult result = ShareActHelper.Query(query);
            DataTable data = (DataTable) result.Data;
            this.grdDate.DataSource = data;
            this.grdDate.DataBind();
            this.pager1.TotalRecords = result.TotalRecords;
            this.CountTotal(name);
        }

        protected void btnSeach_Click(object sender, EventArgs e)
        {
            this.BindData();
        }

        private void CountTotal(string name)
        {
            ShareActivitySearch query = new ShareActivitySearch {
                status = ShareActivityStatus.All,
                IsCount = true,
                PageIndex = this.pager1.PageIndex,
                PageSize = this.pager1.PageSize,
                SortBy = "Id",
                SortOrder = SortAction.Desc,
                CouponName = name
            };
            DataTable data = (DataTable) ShareActHelper.Query(query).Data;
            this.lblAll.Text = (data != null) ? data.Rows.Count.ToString() : "0";
            query.status = ShareActivityStatus.In;
            data = (DataTable) ShareActHelper.Query(query).Data;
            this.lblIn.Text = (data != null) ? data.Rows.Count.ToString() : "0";
            query.status = ShareActivityStatus.End;
            data = (DataTable) ShareActHelper.Query(query).Data;
            this.lblEnd.Text = (data != null) ? data.Rows.Count.ToString() : "0";
            query.status = ShareActivityStatus.unBegin;
            data = (DataTable) ShareActHelper.Query(query).Data;
            this.lblUnBegin.Text = (data != null) ? data.Rows.Count.ToString() : "0";
        }

        protected void DelBtn_Click(object sender, EventArgs e)
        {
            string text = this.txt_Ids.Text;
            this.txt_Ids.Text = "";
            if (text.Length > 1)
            {
                text = text.TrimStart(new char[] { ',' }).TrimEnd(new char[] { ',' });
            }
            foreach (string str2 in text.Split(new char[] { ',' }))
            {
                ShareActHelper.Delete(int.Parse(str2));
            }
            this.BindData();
            this.ShowMsg("批量删除成功！", true);
        }

        protected void lkDelete_Click(object sender, EventArgs e)
        {
            string text = this.txt_Ids.Text;
            this.txt_Ids.Text = "";
            int i = 0;
            if (text.bInt(ref i))
            {
                ShareActHelper.Delete(i);
                this.BindData();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.btnSeach.Click += new EventHandler(this.btnSeach_Click);
            this.DelBtn.Click += new EventHandler(this.DelBtn_Click);
            if (base.Request.Params.AllKeys.Contains<string>("status"))
            {
                int i = 0;
                if (base.Request["status"].ToString().bInt(ref i))
                {
                    this.status = (ShareActivityStatus) i;
                }
            }
            if (!base.IsPostBack)
            {
                this.BindData();
            }
        }
    }
}

