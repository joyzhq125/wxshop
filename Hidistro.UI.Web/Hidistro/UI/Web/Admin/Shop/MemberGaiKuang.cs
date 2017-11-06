﻿namespace Hidistro.UI.Web.Admin.Shop
{
    using Hidistro.ControlPanel.VShop;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Data;

    public class MemberGaiKuang : AdminPage
    {
        public string ActiveUserQty;
        public string CartUserQty;
        public string CollectUserQty;
        public string MemberGradeList;
        public string MemberQty;
        public string PotentialUserQty;
        public string PotentialUserQty_Yesterday;
        public string QtyList_Grade;
        public string RegionNameList;
        public string RegionQtyList;
        public string SleepUserQty;
        public string SuccessTradeUserQty;
        public string SuccessTradeUserQty_Yesterday;
        protected string wid;
        protected MemberGaiKuang() : base("m04", "hyp01")
        {
            this.ActiveUserQty = "0";
            this.SleepUserQty = "0";
            this.SuccessTradeUserQty = "0";
            this.SuccessTradeUserQty_Yesterday = "0";
            this.PotentialUserQty = "0";
            this.PotentialUserQty_Yesterday = "0";
            this.CollectUserQty = "0";
            this.CartUserQty = "0";
            this.MemberQty = "0";
            this.MemberGradeList = "";
            this.QtyList_Grade = "";
            this.RegionNameList = "";
            this.RegionQtyList = "";
        }

        private void LoadData()
        {
            DataRow drOne = ShopStatisticHelper.MemberGlobal_GetCountInfo(this.wid);
            if (drOne != null)
            {
                this.ActiveUserQty = base.GetFieldValue(drOne, "ActiveUserQty");
                this.SleepUserQty = base.GetFieldValue(drOne, "SleepUserQty");
                this.SuccessTradeUserQty = base.GetFieldValue(drOne, "SuccessTradeUserQty");
                this.SuccessTradeUserQty_Yesterday = base.GetFieldValue(drOne, "SuccessTradeUserQty_Yesterday");
                this.PotentialUserQty = base.GetFieldValue(drOne, "PotentialUserQty");
                this.PotentialUserQty_Yesterday = base.GetFieldValue(drOne, "PotentialUserQty_Yesterday");
                this.CollectUserQty = base.GetFieldValue(drOne, "CollectUserQty");
                this.CartUserQty = base.GetFieldValue(drOne, "CartUserQty");
                this.MemberQty = base.GetFieldValue(drOne, "MemberQty");
            }
            DataTable table = ShopStatisticHelper.MemberGlobal_GetStatisticList(1,this.wid);
            DataTable table2 = ShopStatisticHelper.MemberGlobal_GetStatisticList(2,this.wid);
            this.MemberGradeList = "";
            int num = 0;
            int count = table.Rows.Count;
            foreach (DataRow row2 in table.Rows)
            {
                this.MemberGradeList = this.MemberGradeList + "'" + base.GetFieldValue(row2, "Name") + "'";
                this.QtyList_Grade = this.QtyList_Grade + "{" + string.Format("value:{0}, name:'{1}'", base.GetFieldValue(row2, "Total"), base.GetFieldValue(row2, "Name")) + "}";
                if (num < (count - 1))
                {
                    this.MemberGradeList = this.MemberGradeList + ",";
                    this.QtyList_Grade = this.QtyList_Grade + ",";
                }
                this.QtyList_Grade = this.QtyList_Grade + "\n";
                num++;
            }
            DataView defaultView = table2.DefaultView;
            defaultView.Sort = "Total Desc";
            table2 = defaultView.ToTable();
            this.RegionNameList = "";
            this.RegionQtyList = "";
            num = 0;
            count = table2.Rows.Count;
            if (count > 9)
            {
                count = 9;
            }
            foreach (DataRow row3 in table2.Rows)
            {
                if (num >= 9)
                {
                    break;
                }
                this.RegionNameList = this.RegionNameList + "'" + base.GetFieldValue(row3, "RegionName") + "'";
                this.RegionQtyList = this.RegionQtyList + base.GetFieldValue(row3, "Total");
                if (num < (count - 1))
                {
                    this.RegionNameList = this.RegionNameList + ",";
                    this.RegionQtyList = this.RegionQtyList + ",";
                }
                num++;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            wid = GetCurWebId();
            if (string.IsNullOrEmpty(wid)) return;

            if (!base.IsPostBack)
            {
                this.LoadData();
            }
        }
    }
}

