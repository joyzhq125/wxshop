namespace Hidistro.ControlPanel.VShop
{
    using Hidistro.Core.Entities;
    using Hidistro.Entities.StatisticsReport;
    using Hidistro.SqlDal.VShop;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Runtime.InteropServices;

    public static class ShopStatisticHelper
    {
        public static bool AutoStatisticsOrders(out string RetInfo)
        {
            return new ShopStatisticDao().AutoStatisticsOrders(out RetInfo);
        }

        public static DataRow Distributor_GetGlobal(DateTime RecDate)
        {
            return new ShopStatisticDao().Distributor_GetGlobal(RecDate);
        }

        public static DataRow Distributor_GetGlobalTotal(DateTime dYesterday)
        {
            return new ShopStatisticDao().Distributor_GetGlobalTotal(dYesterday);
        }

        public static DataRow GetOrder_Member_CountInfo(DateTime BeginDate, DateTime EndDate,string wid)
        {
            return new ShopStatisticDao().GetOrder_Member_CountInfo(BeginDate, EndDate,wid);
        }

        public static DataSet GetOrder_Member_Rebuy(DateTime BeginDate, DateTime EndDate,string wid)
        {
            return new ShopStatisticDao().GetOrder_Member_Rebuy(BeginDate, EndDate,wid);
        }

        public static DbQueryResult GetOrderStatisticReport(OrderStatisticsQuery Query)
        {
            return new ShopStatisticDao().GetOrderStatisticReport(Query);
        }

        public static DbQueryResult GetOrderStatisticReport_UnderShop(OrderStatisticsQuery_UnderShop Query)
        {
            return new ShopStatisticDao().GetOrderStatisticReport_UnderShop(Query);
        }

        public static DataRow GetOrderStatisticReportGlobalByAgentID(OrderStatisticsQuery_UnderShop Query)
        {
            return new ShopStatisticDao().GetOrderStatisticReportGlobalByAgentID(Query);
        }

        public static DataTable GetSaleReport(DateTime BeginDate, DateTime EndDate,string wid)
        {
            return new ShopStatisticDao().GetSaleReport(BeginDate, EndDate,wid);
        }

        public static DataTable GetTrendDataList_FX(DateTime BeginDate, int Days)
        {
            return new ShopStatisticDao().GetTrendDataList_FX(BeginDate, Days);
        }

        public static DataTable Member_GetInCreateReport(OrderStatisticsQuery query)
        {
            return new ShopStatisticDao().Member_GetInCreateReport(query);
        }

        public static DbQueryResult Member_GetRegionReport(OrderStatisticsQuery query)
        {
            return new ShopStatisticDao().Member_GetRegionReport(query);
        }

        public static DbQueryResult Member_GetStatisticReport(OrderStatisticsQuery Query)
        {
            return new ShopStatisticDao().Member_GetStatisticReport(Query);
        }

        public static DataTable Member_GetStatisticReport_NoPage(OrderStatisticsQuery Query, IList<string> fields)
        {
            return new ShopStatisticDao().Member_GetStatisticReport_NoPage(Query, fields);
        }

        public static DataRow MemberGlobal_GetCountInfo(string wid)
        {
            return new ShopStatisticDao().MemberGlobal_GetCountInfo(wid);
        }

        public static DataTable MemberGlobal_GetStatisticList(int FuncID,string wid)
        {
            return new ShopStatisticDao().MemberGlobal_GetStatisticList(FuncID,wid);
        }

        public static DbQueryResult Product_GetStatisticReport(OrderStatisticsQuery Query)
        {
            return new ShopStatisticDao().Product_GetStatisticReport(Query);
        }

        public static DataTable Product_GetStatisticReport_NoPage(OrderStatisticsQuery query, IList<string> fields)
        {
            return new ShopStatisticDao().Product_GetStatisticReport_NoPage(query, fields);
        }

        public static DataRow ShopGlobal_GetMemberCount(string wid)
        {
            return new ShopStatisticDao().ShopGlobal_GetMemberCount(wid);
        }

        public static DataRow ShopGlobal_GetOrderCountByDate(DateTime dDate,string wid)
        {
            return new ShopStatisticDao().ShopGlobal_GetOrderCountByDate(dDate,wid);
        }

        public static DataTable ShopGlobal_GetSortList_Distributor(DateTime BeginDate, int TopCount,string wid)
        {
            return new ShopStatisticDao().ShopGlobal_GetSortList_Distributor(BeginDate, TopCount,wid);
        }

        public static DataTable ShopGlobal_GetSortList_Member(DateTime BeginDate, int TopCount, string wid)
        {
            return new ShopStatisticDao().ShopGlobal_GetSortList_Member(BeginDate, TopCount,wid);
        }

        public static DataTable ShopGlobal_GetTrendDataList(DateTime BeginDate, int Days,string wid)
        {
            return new ShopStatisticDao().ShopGlobal_GetTrendDataList(BeginDate, Days,wid);
        }

        public static bool StatisticsOrdersByNotify(DateTime RecDate, UpdateAction FuncAction, string ActionDesc, out string RetInfo)
        {
            return new ShopStatisticDao().StatisticsOrdersByNotify(RecDate, FuncAction, ActionDesc, out RetInfo);
        }

        public static bool StatisticsOrdersByRecDate(DateTime RecDate, UpdateAction FuncAction, out string RetInfo)
        {
            return new ShopStatisticDao().StatisticsOrdersByRecDate(RecDate, FuncAction, out RetInfo);
        }
    }
}

