using Hidistro.Core;
using Hidistro.Entities.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace XKD.Web.Admin
{
    public partial class center : System.Web.UI.Page
    {
        protected string business_name = string.Empty;
        protected decimal total_money = 0;
        protected int total_orders_count = 0;
        protected int total_customers_count = 0;
        protected int total_appid_count = 0;

        protected int today_notices_count = 0;
        protected int today_orders_count = 0;
        protected int today_customers_count = 0;

        protected string[] orders_Tendency = new string[] { "", "", "0.00" };

        protected string open_appList = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session[DTKeys.SESSION_ADMIN_INFO] == null)
            {
                Response.Write("<script>parent.location.href='login.aspx';</script>");
                return;
            }

            ManagerInfo model = (ManagerInfo)Session[DTKeys.SESSION_ADMIN_INFO];
            business_name = model.realname;

            decimal moneys = 0, tomoney = 0;
            int tocount = 0;
            /*
            BLL.sf_goods_order bll = new BLL.sf_goods_order();
            DataTable dt = bll.GetList("isPay>1").Tables[0];
            foreach (DataRow r in dt.Rows)
            {
                moneys += Convert.ToDecimal(r["totalMoney"]);
                if (r["payTime"] != null && Convert.ToDateTime(r["payTime"]).ToShortDateString().Equals(DateTime.Now.ToShortDateString()))
                {
                    tomoney += Convert.ToDecimal(r["totalMoney"]);
                    tocount++;
                }
            }
            total_money = moneys;
            total_orders_count = dt.Rows.Count;
            //客户数，商户数
            int tousers = 0;
            int tocustom = 0;
            BLL.sf_user_info userBll = new BLL.sf_user_info();
            DataTable du = userBll.GetList("").Tables[0];
            foreach (DataRow r in du.Rows)
            {
                if (r["addTime"] != null && Convert.ToDateTime(r["addTime"]).ToShortDateString().Equals(DateTime.Now.ToShortDateString()))
                {
                    tousers++;
                }
            }
            BLL.manager mgrBll = new BLL.manager();
            DataTable dm = mgrBll.GetList("type=1").Tables[0];
            foreach (DataRow r in dm.Rows)
            {
                if (r["add_time"] != null && Convert.ToDateTime(r["add_time"]).ToShortDateString().Equals(DateTime.Now.ToShortDateString()))
                {
                    tocustom++;
                }
            }
            
            total_customers_count = du.Rows.Count;
            total_appid_count = dm.Rows.Count;

            today_notices_count = tocount;
            today_orders_count = tousers;
            today_customers_count = tocustom;



            //获取最近10天的每日订单数据
            orders_Tendency = GetOrdersTendency(10);
            */
        }

        /*
        private string[] GetOrdersTendency(int nday)
        {
            //获取最近n天的统计数据
            DateTime startday = DateTime.Now.AddDays(-nday);
            DateTime endDay = DateTime.Now.AddDays(-1);
            string strStartTime = startday.ToString("yyyy-MM-dd") + " 00:00:00.000";
            string strEndTime = endDay.ToString("yyyy-MM-dd") + " 23:59:59.999";


            BLL.sf_goods_order bll = new BLL.sf_goods_order();
            System.Data.DataSet dsOrders = bll.GetList(" isPay = 2 and addTime >= '" + strStartTime + "' and addTime <= '" + strEndTime + "' ");

            string xAxis = "";
            string yAxis = "";
            for (int i = 0; i < nday; i++)
            {
                string strStartTime1 = startday.AddDays(i).ToString("yyyy-MM-dd") + " 00:00:00.000";
                string strEndTime1 = startday.AddDays(i).ToString("yyyy-MM-dd") + " 23:59:59.999";
                System.Data.DataRow[] rows = dsOrders.Tables[0].Select(" addTime >= '" + strStartTime1 + "' and addTime <= '" + strEndTime1 + "'");
                xAxis += "'" + startday.AddDays(i).ToString("MM-dd") + "',";
                yAxis += rows.Length + ",";
            }

            if (xAxis.Length > 0)
            {
                xAxis = xAxis.Remove(xAxis.Length - 1, 1);
                yAxis = yAxis.Remove(yAxis.Length - 1, 1);
            }


            decimal total_money = 0;
            foreach (System.Data.DataRow r in dsOrders.Tables[0].Rows)
            {
                total_money += decimal.Parse(r["totalMoney"].ToString());
            }
            string strTotal_money = total_money == 0 ? "0.00" : total_money.ToString();


            return new[] { xAxis, yAxis, strTotal_money };
        }
        */
    }
}