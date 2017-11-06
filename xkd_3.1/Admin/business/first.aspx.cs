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
    public partial class first : System.Web.UI.Page
    {
        protected string avatar_url = "imgs/avatar.png";
        protected string phone_verify = "verify-no";
        protected string email_verify = "verify-no";

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
            if (Session[DTKeys.SESSION_APP_INFO] == null)
            {
                Response.Write("<script>parent.location.href='business/frame.aspx';</script>");
                return;
            }


            if (Session[DTKeys.SESSION_ADMIN_INFO] != null)
            {
                ManagerInfo model = (ManagerInfo)Session[DTKeys.SESSION_ADMIN_INFO];
                string businessNum = model.businessNum;
                string appNum = Session[DTKeys.SESSION_APP_INFO].ToString();
                //businessNum = Session[DTKeys.SESSION_BUSNIESE_NUM].ToString();


                phone_verify = string.IsNullOrEmpty(model.telephone) == true ? "verify-no" : "verify-pass";
                email_verify = string.IsNullOrEmpty(model.Email) == true ? "verify-no" : "verify-pass";
                business_name = model.realname;
                SF.Model.sf_website appModel = new SF.BLL.sf_website().GetModel(appNum);
                avatar_url = string.IsNullOrEmpty(appModel.avatar) == false ? appModel.avatar : avatar_url;

                /*
                SF.BLL.statistics bll = new SF.BLL.statistics();
                total_money = bll.GetAppNumTotalMoney(appNum);
                total_orders_count = bll.GetAppNumOrdersCount(appNum);
                total_customers_count = bll.GetAppNumCustomersCount(appNum);
                total_appid_count = bll.GetBusinessAppIdCount(businessNum);

                today_notices_count = bll.GetAppNumTodayNoticesCount(model.user_name);
                today_orders_count = bll.GetAppNumTodayOrdersCount(appNum);
                today_customers_count = bll.GetAppNumTodayCustomersCount(appNum);


                //获取最近10天的每日订单数据
                orders_Tendency = GetOrdersTendency(10, appNum);

                open_appList = GetOpenAppList(businessNum);
                */
            }
        }

        /*
        private string[] GetOrdersTendency(int nday, string appNum)
        {
            //获取最近n天的统计数据
            DateTime startday = DateTime.Now.AddDays(-nday);
            DateTime endDay = DateTime.Now.AddDays(-1);
            string strStartTime = startday.ToString("yyyy-MM-dd") + " 00:00:00.000";
            string strEndTime = endDay.ToString("yyyy-MM-dd") + " 23:59:59.999";


            BLL.sf_goods_order bll = new BLL.sf_goods_order();
            System.Data.DataSet dsOrders = bll.GetList(" isPay = 2 and appNum = '" + appNum + "' and addTime >= '" + strStartTime + "' and addTime <= '" + strEndTime + "' ");

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
        private string GetOpenAppList(int mid)
        {
            SF.BLL.sf_website bll = new SF.BLL.sf_website();
            System.Data.DataSet dsAppList = bll.GetAppNumListCount(mid);

            System.Text.StringBuilder sbAppList = new System.Text.StringBuilder();
            sbAppList.AppendLine("<div class=\"product-row\">");
            for (int i = 0; i < dsAppList.Tables[0].Rows.Count; i++)
            {
                System.Data.DataRow row = dsAppList.Tables[0].Rows[i];

                //if (i == 0 || i == 3)
                //{
                //}

                sbAppList.AppendLine("<a class=\"product\" href=\"javascript:showAppDetails('" + row["id"] + "')\">");
                sbAppList.AppendLine("    <div class=\"product-name\">");
                sbAppList.AppendLine("        <i class=\"iconfont icon-bos\"></i>");
                sbAppList.AppendLine("        <b>" + row["appid_name"].ToString() + "</b>");
                sbAppList.AppendLine("    </div>");
                sbAppList.AppendLine("    <div class=\"product-spec bos-spec\">");
                sbAppList.AppendLine("        <div class=\"spec-row\">");
                sbAppList.AppendLine("            <span class=\"spec-key\">");
                sbAppList.AppendLine("                <i class=\"iconfont icon-pie\"></i>流量引入");
                sbAppList.AppendLine("            </span>");
                sbAppList.AppendLine("            <span class=\"spec-value bos-value\">" + row["count"].ToString() + "</span>");
                sbAppList.AppendLine("        </div>");
                sbAppList.AppendLine("    </div>");
                sbAppList.AppendLine("</a>");

                //if (i == 2 || i == 4)
                //{
                //}
            }
            sbAppList.AppendLine("</div>");

            return sbAppList.ToString();
        }
    }
}