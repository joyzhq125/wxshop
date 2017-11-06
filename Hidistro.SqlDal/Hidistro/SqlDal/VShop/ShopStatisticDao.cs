namespace Hidistro.SqlDal.VShop
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.StatisticsReport;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Runtime.InteropServices;
    using System.Text;

    public class ShopStatisticDao
    {
        private Database database = DatabaseFactory.CreateDatabase();

        public bool AutoStatisticsOrders(out string RetInfo)
        {
            DbCommand storedProcCommand = this.database.GetStoredProcCommand("sp_vshop_Statistics_Auto");
            this.database.AddInParameter(storedProcCommand, "@RecDate", DbType.Date, DateTime.Today);
            this.database.AddOutParameter(storedProcCommand, "@RetCode", DbType.Int32, 0);
            this.database.AddOutParameter(storedProcCommand, "@RetInfo", DbType.String, 250);
            try
            {
                this.database.ExecuteNonQuery(storedProcCommand);
                RetInfo = storedProcCommand.Parameters["@RetInfo"].Value.ToString();
                return (storedProcCommand.Parameters["@RetCode"].Value.ToString() == "1");
            }
            catch (Exception exception)
            {
                RetInfo = exception.Message;
                return false;
            }
        }

        public DataRow Distributor_GetGlobal(DateTime dDate)
        {
            DataTable table = new DataTable();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("\r\n  select \r\n\t                (\r\n\t\t                select   sum(ValidOrderTotal) as ValidOrderTotal  \r\n\t\t                from vw_VShop_FinishOrder_Main \r\n\t\t                where  CONVERT( varchar(10), PayDate, 120) =  CONVERT( varchar(10), @RecDate, 120)\r\n\t                ) as ValidOrderTotal,\r\n                * from\r\n                (\r\n\t                select   sum(ValidOrderTotal) as FXValidOrderTotal,   sum( SumCommission) as FXSumCommission  , COUNT(*) as FXOrderNumber\r\n\t                from vw_VShop_FinishOrder_Main_Payed \r\n\t                where  ReferralUserId>0 and  CONVERT( varchar(10), PayDate, 120) =  CONVERT( varchar(10), @RecDate, 120)\r\n                ) T1\r\n                ");
            this.database.AddInParameter(sqlStringCommand, "RecDate", DbType.String, dDate.ToString("yyyy-MM-dd"));
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                table = DataHelper.ConverDataReaderToDataTable(reader);
            }
            return table.Rows[0];
        }

        public DataRow Distributor_GetGlobalTotal(DateTime dYesterday)
        {
            DataTable table = new DataTable();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("\r\n\t            select \r\n\t            (select COUNT(*) from aspnet_Distributors where ReferralStatus<=1) as DistributorNumber,\r\n\t            (\r\n\t            select  COUNT(*)\r\n\t\t            from aspnet_Distributors\r\n\t\t            where ReferralStatus<=1 and  CONVERT(varchar(10), CreateTime , 120 ) =  CONVERT(varchar(10), @RecDate  , 120 ) \r\n\t            ) as NewAgentNumber,\t\r\n\t            (\r\n\t            SELECT  ISNULL(SUM(Amount),0) from Hishop_BalanceDrawRequest   where isnull(IsCheck,0)=1  \r\n\t            ) as FinishedDrawCommissionFee,\r\n\t\r\n\t            (\r\n\t            SELECT ISNULL( SUM(isnull(Amount,0)),0) from Hishop_BalanceDrawRequest   where isnull(IsCheck,0)<>1 \r\n\t            ) as WaitDrawCommissionFee\r\n                ");
            this.database.AddInParameter(sqlStringCommand, "RecDate", DbType.String, dYesterday.ToString("yyyy-MM-dd"));
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                table = DataHelper.ConverDataReaderToDataTable(reader);
            }
            return table.Rows[0];
        }

        public DataRow GetOrder_Member_CountInfo(DateTime BeginDate, DateTime EndDate,string wid)
        {
            DataTable table = new DataTable();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("\r\n  select a.*, b.* from \r\n  (\r\n\r\n  select 1 as RecNO1,\r\n  sum(OrderNumber) as OrderNumber, sum(SaleAmountFee) as SaleAmountFee, sum(BuyerNumber) as BuyerNumber, \r\n   --AVG(BuyerAvgPrice) as BuyerAvgPrice , \r\n   sum(CommissionAmountFee)  as CommissionAmountFee\r\n   from dbo.vshop_Statistics_Distributors a\r\n   where 1=1\r\n    and CONVERT( varchar(10), RecDate, 120) >=  CONVERT( varchar(10), @BeginDate, 120)\r\n   and CONVERT( varchar(10), RecDate, 120) <=  CONVERT( varchar(10), @EndDate, 120) and wid=@wid \r\n\t ) a \r\n  left join\r\n     (\r\n      select  \r\n    1 as RecNO2, \r\n  SUM(NewAgentNumber) as NewAgentNumber, SUM(NewMemberNumber) as NewMemberNumber ,\r\n    sum(isnull(FXOrderNumber,0)) as FXOrderNumber, SUM(FXSaleAmountFee) as FXSaleAmountFee,\r\n                    --AVG(FXResultPercent) as FXResultPercent,\r\n                     SUM ( isnull(CommissionFee,0)) as FXCommissionFee\r\n  from vshop_Statistics_Globals\r\n    where 1=1\r\n   and CONVERT( varchar(10), RecDate, 120) >=  CONVERT( varchar(10), @BeginDate, 120)\r\n    and CONVERT( varchar(10), RecDate, 120) <=  CONVERT(varchar(10), @EndDate, 120) \r\n  and wid=@wid ) b on a.RecNO1=b.RecNO2\r\n ");
            this.database.AddInParameter(sqlStringCommand, "BeginDate", DbType.Date, BeginDate);
            this.database.AddInParameter(sqlStringCommand, "EndDate", DbType.Date, EndDate);
            this.database.AddInParameter(sqlStringCommand, "wid", DbType.String, wid);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                table = DataHelper.ConverDataReaderToDataTable(reader);
            }
            if ((table != null) && (table.Rows.Count > 0))
            {
                return table.Rows[0];
            }
            return null;
        }

        public DataSet GetOrder_Member_Rebuy(DateTime BeginDate, DateTime EndDate,string wid)
        {
            DataSet set = new DataSet();
            DataTable table = null;
            DataTable table2 = null;
            DbCommand sqlStringCommand = null;
            sqlStringCommand = this.database.GetSqlStringCommand("\r\n                select IsOldMember ,  count(*) as TotalDiffMemberNumber, sum(ValidOrderTotal) as  ValidOrderTotal\r\n    from \r\n   (\r\n\t  select \r\n\t  IsOldMember , UserId, count(*) as TotalDiffMemberNumber, sum(ValidOrderTotal) as  ValidOrderTotal\r\n\t                from\r\n\t                (\t\r\n\t\t                select \r\n\t\t\t                case \r\n\t\t\t\t                when X.UserId is null then 1\r\n\t\t\t\t                else -1 \r\n\t\t\t                end IsOldMember\t ,\r\n\t\t\t                a.*  \r\n\t\t\t                from vw_VShop_FinishOrder_Main a  \r\n\t\t\t                left join\r\n\t\t\t                (\r\n\t\t\t\t                select  UserId from  vw_VShop_FinishOrder_Main\r\n\t\t\t\t                where PayDate<= @EndDate and wid=@wid  \r\n\t\t\t\t                group by UserId\r\n\t\t\t\t                having COUNT(*) > 1\r\n\t\t\t                ) X on a.UserId= X.UserId\r\n                            where 1=1\r\n                            and CONVERT( varchar(10), PayDate, 120) >=  CONVERT( varchar(10), @BeginDate, 120)\r\n                            and CONVERT( varchar(10), PayDate, 120) <=  CONVERT( varchar(10), @EndDate, 120) and wid=@wid \r\n\r\n\t                ) T\r\n\t                group by IsOldMember, UserId\r\n                ) T2\r\n                group by IsOldMember \r\n                ");
            this.database.AddInParameter(sqlStringCommand, "BeginDate", DbType.Date, BeginDate);
            this.database.AddInParameter(sqlStringCommand, "EndDate", DbType.Date, EndDate);
            this.database.AddInParameter(sqlStringCommand, "wid", DbType.String, wid);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                table = DataHelper.ConverDataReaderToDataTable(reader);
            }
            sqlStringCommand = this.database.GetSqlStringCommand("\r\n                select *\r\n                from\r\n                (  \r\n\t                select IsOldMember, CONVERT(varchar(10), PayDate, 120) as PayDate , COUNT(*) as  Number , UserId\r\n\t                from\r\n\t                (\r\n\t\t                select \r\n\t\t\t                case \r\n\t\t\t\t                when X.UserId is null then 1\r\n\t\t\t\t                else -1\r\n\t\t\t                end IsOldMember\t ,\r\n\t\t\t                a.*  \r\n\t\t\t\t\r\n\t\t\t                from vw_VShop_FinishOrder_Main a\r\n\t\t\t                left join\r\n\t\t\t                (\r\n\t\t\t\t                select  UserId from  vw_VShop_FinishOrder_Main\r\n\t\t\t\t                where PayDate<= @EndDate\r\n\t\t\t\t                group by UserId\r\n\t\t\t\t                having COUNT(*) > 1\r\n\t\t\t                ) X on a.UserId= X.UserId\r\n\t\t\t                where PayDate>= @BeginDate and PayDate<=@EndDate\r\n\t                ) T\r\n\t                where 1=1\r\n\t                group by  CONVERT(varchar(10), PayDate, 120)  , IsOldMember   , UserID\r\n                ) TA\r\n                order by TA.PayDate , TA.IsOldMember\r\n                ");
            this.database.AddInParameter(sqlStringCommand, "BeginDate", DbType.Date, BeginDate);
            this.database.AddInParameter(sqlStringCommand, "EndDate", DbType.Date, EndDate);
            using (IDataReader reader2 = this.database.ExecuteReader(sqlStringCommand))
            {
                table2 = DataHelper.ConverDataReaderToDataTable(reader2);
            }
            set.Tables.Add(table);
            set.Tables.Add(table2);
            return set;
        }

        public DataTable GetOrderCountInfo(DateTime BeginDate, DateTime EndDate)
        {
            DataTable table = new DataTable();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("\r\n            select * from \r\n                (\r\n                   select 1 as RecNO,\r\n                                count(*) as OrderNumber,  sum(   ValidOrderTotal) as SaleAmountFee\r\n                                from vw_VShop_FinishOrder_Main\r\n                                where 1=1\r\n                                 and CONVERT( varchar(10), PayDate, 120) >=  CONVERT( varchar(10), @BeginDate, 120)\r\n                                 and CONVERT( varchar(10), PayDate, 120) <=  CONVERT( varchar(10), @EndDate, 120)                   \r\n                ) T1\r\n                left join\r\n                (\r\n                   select 1 as FXRecNO,\r\n                                count(*) as FXOrderNumber,  sum(   ValidOrderTotal) as FXSaleAmountFee\r\n                                from vw_VShop_FinishOrder_Main\r\n                                where  ReferralUserId>0\r\n                                 and CONVERT( varchar(10), PayDate, 120) >=  CONVERT( varchar(10), @BeginDate, 120)\r\n                                 and CONVERT( varchar(10), PayDate, 120) <=  CONVERT( varchar(10), @EndDate, 120)                   \r\n                ) T2 on T1.RecNO= T2.FXRecNO\r\n                ");
            this.database.AddInParameter(sqlStringCommand, "BeginDate", DbType.Date, BeginDate);
            this.database.AddInParameter(sqlStringCommand, "EndDate", DbType.Date, EndDate);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        public DbQueryResult GetOrderStatisticReport(OrderStatisticsQuery query)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(" 1=1 ");
            if (!query.BeginDate.HasValue)
            {
                query.BeginDate = new DateTime?(DateTime.Today.AddDays(-7.0));
            }
            if (!query.EndDate.HasValue)
            {
                query.EndDate = new DateTime?(DateTime.Today);
            }
            //builder.AppendFormat(" and wid='{0}'", query.wid);
            string table = string.Format("\r\n  (\r\n  select T1.*, b.RealName, b.CellPhone, b.UserName,  b.UserHead , b.StoreName\r\n    from \r\n  (\r\n\t  select AgentId,\r\n\t   sum(OrderNumber) as OrderNumber, sum(SaleAmountFee) as SaleAmountFee, sum(BuyerNumber) as BuyerNumber, \r\n\t   AVG(BuyerAvgPrice) as BuyerAvgPrice , sum(CommissionAmountFee)  as CommissionAmountFee\r\n\t   from dbo.vshop_Statistics_Distributors a\r\n\t    where 1=1\r\n\t   and AgentID>0 \r\n\t  and CONVERT( varchar(10), RecDate, 120) >=  CONVERT( varchar(10), '{0}', 120)\r\n\t  and CONVERT( varchar(10), RecDate, 120) <=  CONVERT( varchar(10), '{1}', 120) \r\n\t   group by \t AgentID  \r\n   ) T1\r\n   left join vw_Hishop_DistributorsMembers b on T1.AgentId= b.UserId and b.wid='{2}' \r\n    -- where b.ReferralStatus<=1\r\n ) P  \r\n ", query.BeginDate.Value.ToString("yyyy-MM-dd"), query.EndDate.Value.ToString("yyyy-MM-dd"),query.wid);
            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, table, "AgentId", (builder.Length > 0) ? builder.ToString() : null, "*");
        }

        public DbQueryResult GetOrderStatisticReport_UnderShop(OrderStatisticsQuery_UnderShop query)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(" 1=1 ");
            if (!query.BeginDate.HasValue)
            {
                query.BeginDate = new DateTime?(DateTime.Today.AddDays(-7.0));
            }
            if (!query.EndDate.HasValue)
            {
                query.EndDate = new DateTime?(DateTime.Today);
            }
            string table = "";
            if (query.ShopLevel == 1)
            {
                table = string.Format("\r\n                    (\r\n                        select T1.*, b.RealName, b.CellPhone, b.UserName,  b.UserHead , b.StoreName\r\n                        from \r\n                        (\r\n                            select  AgentID, SUM(OrderNumber) OrderNumber, SUM(SaleAmountFee) SaleAmountFee, SUM(BuyerNumber) BuyerNumber, AVG(BuyerAvgPrice) BuyerAvgPrice, SUM(CommissionAmountFee) CommissionAmountFee\r\n                            from dbo.vshop_Statistics_Distributors\r\n                            where AgentID >0\r\n\t                             and CONVERT( varchar(10), RecDate, 120) >=  CONVERT( varchar(10), '{0}', 120)\r\n\t                             and CONVERT( varchar(10), RecDate, 120) <=  CONVERT( varchar(10), '{1}', 120)\r\n                                 and AgentId in\r\n                                 (\r\n                                select UserId \r\n                                from aspnet_Distributors\r\n                                where ReferralUserId= {2} and UserId <> ReferralUserId\r\n                                 ) \r\n                            group by AgentID\r\n                         ) T1\r\n                        left join vw_Hishop_DistributorsMembers b on T1.AgentId= b.UserId \r\n                    ) P  ", query.BeginDate.Value.ToString("yyyy-MM-dd"), query.EndDate.Value.ToString("yyyy-MM-dd"), query.AgentId);
            }
            else if (query.ShopLevel == 2)
            {
                table = string.Format("\r\n                    (\r\n                        select T1.*, b.RealName, b.CellPhone, b.UserName,  b.UserHead , b.StoreName\r\n                        from \r\n                        (\r\n                            select  AgentID, SUM(OrderNumber) OrderNumber, SUM(SaleAmountFee) SaleAmountFee, SUM(BuyerNumber) BuyerNumber, AVG(BuyerAvgPrice) BuyerAvgPrice, SUM(CommissionAmountFee) CommissionAmountFee\r\n                            from dbo.vshop_Statistics_Distributors\r\n                            where AgentID >0\r\n\t                             and CONVERT( varchar(10), RecDate, 120) >=  CONVERT( varchar(10), '{0}', 120)\r\n\t                             and CONVERT( varchar(10), RecDate, 120) <=  CONVERT( varchar(10), '{1}', 120)\r\n                                 and AgentId in\r\n                                 (\r\n                                    select UserId \r\n                                    from aspnet_Distributors\r\n                                    where ReferralUserId in\r\n                                    (\r\n                                    select UserId \r\n                                    from aspnet_Distributors\r\n                                    where ReferralUserId= {2} and UserId <> ReferralUserId\r\n                                    )\r\n                                    and UserId <> ReferralUserId\r\n                                 ) \r\n                            group by AgentID\r\n                         ) T1\r\n                        left join vw_Hishop_DistributorsMembers b on T1.AgentId= b.UserId \r\n                    ) P  ", query.BeginDate.Value.ToString("yyyy-MM-dd"), query.EndDate.Value.ToString("yyyy-MM-dd"), query.AgentId);
            }
            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, table, "AgentId", (builder.Length > 0) ? builder.ToString() : null, "*");
        }

        private DataRow GetOrderStatisticReportGlobal_UnderShop_BAD(OrderStatisticsQuery_UnderShop query)
        {
            if (!query.BeginDate.HasValue)
            {
                query.BeginDate = new DateTime?(DateTime.Today.AddDays(-7.0));
            }
            if (!query.EndDate.HasValue)
            {
                query.EndDate = new DateTime?(DateTime.Today);
            }
            DataTable table = new DataTable();
            DbCommand sqlStringCommand = null;
            if (query.ShopLevel == 1)
            {
                sqlStringCommand = this.database.GetSqlStringCommand("\r\n                            select  SUM(OrderNumber) OrderNumber, SUM(SaleAmountFee) SaleAmountFee, SUM(BuyerNumber) BuyerNumber, AVG(BuyerAvgPrice) BuyerAvgPrice, SUM(CommissionAmountFee) CommissionAmountFee\r\n                            from dbo.vshop_Statistics_Distributors\r\n                            where AgentID >0\r\n\t                             and CONVERT( varchar(10), RecDate, 120) >=  CONVERT( varchar(10), @BeginDate, 120)\r\n\t                             and CONVERT( varchar(10), RecDate, 120) <=  CONVERT( varchar(10), @EndDate, 120)\r\n                                 and AgentId in\r\n                                 (\r\n                                select UserId \r\n                                from aspnet_Distributors\r\n                                where ReferralUserId= @AgentId   and UserId <> ReferralUserId \r\n                                 ) \r\n\r\n                     ");
            }
            else if (query.ShopLevel == 2)
            {
                sqlStringCommand = this.database.GetSqlStringCommand("\r\n                            select  SUM(OrderNumber) OrderNumber, SUM(SaleAmountFee) SaleAmountFee, SUM(BuyerNumber) BuyerNumber, AVG(BuyerAvgPrice) BuyerAvgPrice, SUM(CommissionAmountFee) CommissionAmountFee\r\n                            from dbo.vshop_Statistics_Distributors\r\n                            where AgentID >0\r\n\t                             and CONVERT( varchar(10), RecDate, 120) >=  CONVERT( varchar(10),  @BeginDate, 120)\r\n\t                             and CONVERT( varchar(10), RecDate, 120) <=  CONVERT( varchar(10),  @EndDate, 120)\r\n                                 and AgentId in\r\n                                 (\r\n                                    select UserId \r\n                                    from aspnet_Distributors\r\n                                    where ReferralUserId in\r\n                                    (\r\n                                    select UserId \r\n                                    from aspnet_Distributors\r\n                                    where ReferralUserId=  @AgentId and UserId <> ReferralUserId \r\n                                    )\r\n                                    and UserId <> ReferralUserId\r\n                                 ) \r\n                    ");
            }
            this.database.AddInParameter(sqlStringCommand, "BeginDate", DbType.Date, query.BeginDate.Value);
            this.database.AddInParameter(sqlStringCommand, "EndDate", DbType.Date, query.EndDate.Value);
            this.database.AddInParameter(sqlStringCommand, "AgentId", DbType.Int32, query.AgentId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                table = DataHelper.ConverDataReaderToDataTable(reader);
            }
            if ((table != null) && (table.Rows.Count > 0))
            {
                return table.Rows[0];
            }
            return null;
        }

        public DataRow GetOrderStatisticReportGlobalByAgentID(OrderStatisticsQuery_UnderShop query)
        {
            if (!query.BeginDate.HasValue)
            {
                query.BeginDate = new DateTime?(DateTime.Today.AddDays(-7.0));
            }
            if (!query.EndDate.HasValue)
            {
                query.EndDate = new DateTime?(DateTime.Today);
            }
            DataTable table = new DataTable();
            DbCommand sqlStringCommand = null;
            sqlStringCommand = this.database.GetSqlStringCommand("\r\n                        select  SUM(OrderNumber) OrderNumber, SUM(SaleAmountFee) SaleAmountFee, SUM(BuyerNumber) BuyerNumber, AVG(BuyerAvgPrice) BuyerAvgPrice, SUM(CommissionAmountFee) CommissionAmountFee\r\n                        from dbo.vshop_Statistics_Distributors\r\n                        where AgentID >0\r\n\t                            and CONVERT( varchar(10), RecDate, 120) >=  CONVERT( varchar(10), @BeginDate, 120)\r\n\t                            and CONVERT( varchar(10), RecDate, 120) <=  CONVERT( varchar(10), @EndDate, 120)\r\n                                and AgentId = @AgentId \r\n                                \r\n\r\n                    ");
            this.database.AddInParameter(sqlStringCommand, "BeginDate", DbType.Date, query.BeginDate.Value);
            this.database.AddInParameter(sqlStringCommand, "EndDate", DbType.Date, query.EndDate.Value);
            this.database.AddInParameter(sqlStringCommand, "AgentId", DbType.Int32, query.AgentId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                table = DataHelper.ConverDataReaderToDataTable(reader);
            }
            if ((table != null) && (table.Rows.Count > 0))
            {
                return table.Rows[0];
            }
            return null;
        }

        public DataTable GetSaleReport(DateTime BeginDate, DateTime EndDate,string wid)
        {
            DataSet set = new DataSet();
            DbCommand sqlStringCommand = null;
            sqlStringCommand = this.database.GetSqlStringCommand("\r\n   select  * from vshop_Statistics_Globals\r\n   where 1=1\r\n   and CONVERT( varchar(10), RecDate, 120) >=  CONVERT( varchar(10), @BeginDate, 120)\r\n  and CONVERT( varchar(10), RecDate, 120) <=  CONVERT( varchar(10), @EndDate, 120)   and wid=@wid    \r\n\r\n                ");
            this.database.AddInParameter(sqlStringCommand, "BeginDate", DbType.Date, BeginDate);
            this.database.AddInParameter(sqlStringCommand, "EndDate", DbType.Date, EndDate);
            this.database.AddInParameter(sqlStringCommand, "wid", DbType.String, wid);
            DataTable table = null;
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                table = DataHelper.ConverDataReaderToDataTable(reader);
            }
            set.Tables.Add(table);
            return set.Tables[0];
        }

        public DataTable GetTrendDataList_FX(DateTime BeginDate, int Days)
        {
            DataTable table = new DataTable();
            table.Columns.Add(new DataColumn("ID", typeof(int)));
            table.Columns.Add(new DataColumn("RecDate", typeof(DateTime)));
            table.Columns.Add(new DataColumn("NewAgentCount", typeof(decimal)));
            table.Columns.Add(new DataColumn("FXAmountFee", typeof(decimal)));
            table.Columns.Add(new DataColumn("FXCommisionFee", typeof(decimal)));
            for (int i = 0; i < Days; i++)
            {
                DataRow row = table.NewRow();
                DateTime time = BeginDate.AddDays((double) i);
                DbCommand sqlStringCommand = this.database.GetSqlStringCommand("\r\n                    select\r\n                    (select  COUNT(*) from aspnet_Distributors where ReferralStatus<=1 and  CONVERT(varchar(10),  CreateTime , 120)= CONVERT(varchar(10),  @RecDate , 120 ) ) as NewAgentCount ,\r\n                    isnull(sum(ValidOrderTotal),0) as ValidOrderTotal, isnull(sum(SumCommission),0) as SumCommission \r\n                        from vw_VShop_FinishOrder_Main where  ReferralUserId>0 and  CONVERT(varchar(10),  PayDate, 120 )= CONVERT(varchar(10),  @RecDate ,120)\r\n                    ");
                this.database.AddInParameter(sqlStringCommand, "RecDate", DbType.Date, time);
                DataTable table2 = new DataTable();
                using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
                {
                    table2 = DataHelper.ConverDataReaderToDataTable(reader);
                }
                if (table2.Rows.Count > 0)
                {
                    row["NewAgentCount"] = table2.Rows[0]["NewAgentCount"];
                    row["FXAmountFee"] = table2.Rows[0]["ValidOrderTotal"];
                    row["FXCommisionFee"] = table2.Rows[0]["SumCommission"];
                }
                else
                {
                    row["NewAgentCount"] = 0;
                    row["FXAmountFee"] = 0;
                    row["FXCommisionFee"] = 0;
                }
                row["RecDate"] = time;
                table.Rows.Add(row);
            }
            return table;
        }

        public DataTable Member_GetInCreateReport(OrderStatisticsQuery query)
        {
            new StringBuilder().Append(" 1=1 ");
            if (!query.BeginDate.HasValue)
            {
                query.BeginDate = new DateTime?(DateTime.Today.AddDays(-7.0));
            }
            if (!query.EndDate.HasValue)
            {
                query.EndDate = new DateTime?(DateTime.Today);
            }
            string str = string.Format("\r\n                    select * from   vshop_Statistics_Globals\r\n                    where CONVERT(varchar(10), RecDate, 120) >= CONVERT(varchar(10), '{0}', 120)  \r\n                          and  CONVERT(varchar(10), RecDate, 120) <=CONVERT(varchar(10), '{1}', 120)  and wid='{2}' \r\n                    order by RecDate \r\n                    ", query.BeginDate.Value.ToString("yyyy-MM-dd"), query.EndDate.Value.ToString("yyyy-MM-dd"),query.wid);
            DataTable table = new DataTable();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(str);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        public DbQueryResult Member_GetRegionReport(OrderStatisticsQuery query)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(" 1=1 ");
            string table = string.Format("\r\n                    (\r\n                        select  ROW_NUMBER() over(order by a.RegionId) as RowIndex  , a.*, isnull(b.TotalRec, 0) as TotalRec\r\n                        from VShop_Region a\r\n                        left join \r\n                         (\r\n                            select COUNT(*) TotalRec,  RegionId\r\n                            from aspnet_Members\r\n                            where Status=1  and wid='{0}' \r\n                            group by RegionId\r\n                        ) b on a.RegionId = b.RegionId \r\n                        \r\n                    ) P  \r\n                    ", query.wid/*query.BeginDate, query.EndDate*/);
            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, table, "RowIndex", (builder.Length > 0) ? builder.ToString() : null, "*");
        }

        public DbQueryResult Member_GetStatisticReport(OrderStatisticsQuery query)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(" 1=1 ");
            if (!query.BeginDate.HasValue)
            {
                query.BeginDate = new DateTime?(DateTime.Today.AddDays(-7.0));
            }
            if (!query.EndDate.HasValue)
            {
                query.EndDate = new DateTime?(DateTime.Today);
            }
            string table = string.Format("\r\n                    (\r\n\t                select T1.*, b.RealName, b.CellPhone, b.UserName, b.CreateDate, b.UserHead\r\n\t                from \r\n\t                (\r\n\t                select  UserId ,COUNT(*) as OrderNumber, SUM(ValidOrderTotal) as OrderTotal, \r\n\t                case \r\n\t\t                when  SUM(ValidOrderTotal)>0 then SUM(ValidOrderTotal) * 1.0 / COUNT(*) \r\n\t\t                else 0\r\n\t                end as AvgPrice\r\n\t                from  dbo.vw_VShop_FinishOrder_Main a\r\n                    where CONVERT(varchar(10), PayDate, 120) >= CONVERT(varchar(10), '{0}', 120)  \r\n                          and  CONVERT(varchar(10), PayDate, 120) <=CONVERT(varchar(10), '{1}', 120) and wid='{2}' \r\n \t                group by UserId\r\n\t                ) T1\r\n\t                left join aspnet_Members b on T1.UserID= b.UserId \r\n                    where b.Status=1 and b.wid='{2}' \r\n                    ) P  \r\n                    ", query.BeginDate.Value.ToString("yyyy-MM-dd"), query.EndDate.Value.ToString("yyyy-MM-dd"),query.wid);
            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, table, "UserId", (builder.Length > 0) ? builder.ToString() : null, "*");
        }

        public DataTable Member_GetStatisticReport_NoPage(OrderStatisticsQuery query, IList<string> fields)
        {
            if (fields.Count == 0)
            {
                return null;
            }
            string str = string.Empty;
            foreach (string str2 in fields)
            {
                str = str + str2 + ",";
            }
            str = str.Substring(0, str.Length - 1);
            new StringBuilder().Append(" 1=1 ");
            if (!query.BeginDate.HasValue)
            {
                query.BeginDate = new DateTime?(DateTime.Today.AddDays(-7.0));
            }
            if (!query.EndDate.HasValue)
            {
                query.EndDate = new DateTime?(DateTime.Today);
            }
            string str3 = string.Format("\r\n  (\r\n\t  select   ROW_NUMBER() over(order by OrderTotal desc) as RankIndex  , \r\n                        T1.*, b.RealName, b.CellPhone, b.UserName, b.CreateDate, b.UserHead\r\n\t                from \r\n\t                (\r\n\t                select  UserId ,COUNT(*) as OrderNumber, SUM(ValidOrderTotal) as OrderTotal, \r\n\t                case \r\n\t\t                when  SUM(ValidOrderTotal)>0 then SUM(ValidOrderTotal) * 1.0 / COUNT(*) \r\n\t\t                else 0\r\n\t                end as AvgPrice\r\n\t                from  dbo.vw_VShop_FinishOrder_Main a\r\n                    where CONVERT(varchar(10), PayDate, 120) >= CONVERT(varchar(10), '{0}', 120)  \r\n                          and  CONVERT(varchar(10), PayDate, 120) <=CONVERT(varchar(10), '{1}', 120)  \r\n \t                group by UserId\r\n\t                ) T1\r\n\t                left join aspnet_Members b on T1.UserID= b.UserId \r\n                    ) P  \r\n                    ", query.BeginDate.Value.ToString("yyyy-MM-dd"), query.EndDate.Value.ToString("yyyy-MM-dd"));
            DataTable table = new DataTable();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select " + str + " from " + str3);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        public DataRow MemberGlobal_GetCountInfo(string wid)
        {
            DataTable table = new DataTable();
            DbCommand storedProcCommand = this.database.GetStoredProcCommand("sp_Statistics_Member");
            this.database.AddInParameter(storedProcCommand, "wid", DbType.String, wid);
            using (IDataReader reader = this.database.ExecuteReader(storedProcCommand))
            {
                table = DataHelper.ConverDataReaderToDataTable(reader);
            }
            return table.Rows[0];
        }

        public DataTable MemberGlobal_GetStatisticList(int FuncID,string wid)
        {
            DataTable table = new DataTable();
            DbCommand sqlStringCommand = null;
            if (FuncID == 1)
            {
                sqlStringCommand = this.database.GetSqlStringCommand("select isnull(T1.Total ,0) as Total, isnull(T2.Name,'其它') as Name from  (select  COUNT(*) as Total, a.GradeId from aspnet_Members a where  1=1 and a.Status=1 and wid=@wid  group by a.GradeId) T1 left join aspnet_MemberGrades T2 on T1.GradeId= T2.GradeId");
            }
            else if (FuncID == 2)
            {
                sqlStringCommand = this.database.GetSqlStringCommand("select v.*, ISNULL( T1.Total,0) as Total from VShop_Region v left join(select  COUNT(*) as Total, a.TopRegionId from aspnet_Members a where  1=1 and a.Status=1 and wid=@wid group by a.TopRegionId) T1 on v.RegionID = T1.TopRegionId");
            }
            else
            {
                return null;
            }
            this.database.AddInParameter(sqlStringCommand, "wid", DbType.String, wid);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        public DbQueryResult Product_GetStatisticReport(OrderStatisticsQuery query)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(" 1=1 ");
            if (!query.BeginDate.HasValue)
            {
                query.BeginDate = new DateTime?(DateTime.Today.AddDays(-7.0));
            }
            if (!query.EndDate.HasValue)
            {
                query.EndDate = new DateTime?(DateTime.Today);
            }
            string table = string.Format("\r\n   (\r\n   select  ROW_NUMBER() over(order by a.SaleAmountFee desc) as RankIndex  , \r\n  a.* , b.ProductName,b.ThumbnailUrl60\r\n                    from vshop_Statistics_Products a\r\n                    inner join Hishop_Products b on a.ProductID  = b.ProductId\r\n                    where CONVERT(varchar(10), RecDate, 120) >= CONVERT(varchar(10), '{0}', 120)  \r\n                          and  CONVERT(varchar(10), RecDate, 120) <=CONVERT(varchar(10), '{1}', 120) and a.wid='{2}' \r\n                    ) P  \r\n                    ", query.BeginDate.Value.ToString("yyyy-MM-dd"), query.EndDate.Value.ToString("yyyy-MM-dd"), query.wid);
            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, table, "ProductId", (builder.Length > 0) ? builder.ToString() : null, "*");
        }

        public DataTable Product_GetStatisticReport_NoPage(OrderStatisticsQuery query, IList<string> fields)
        {
            if (fields.Count == 0)
            {
                return null;
            }
            string str = string.Empty;
            foreach (string str2 in fields)
            {
                str = str + str2 + ",";
            }
            str = str.Substring(0, str.Length - 1);
            new StringBuilder().Append(" 1=1 ");
            if (!query.BeginDate.HasValue)
            {
                query.BeginDate = new DateTime?(DateTime.Today.AddDays(-7.0));
            }
            if (!query.EndDate.HasValue)
            {
                query.EndDate = new DateTime?(DateTime.Today);
            }
            string str3 = string.Format("\r\n                    (\r\n                    select  ROW_NUMBER() over(order by a.SaleAmountFee desc) as RankIndex  , \r\n                    a.* , b.ProductName,b.ThumbnailUrl60\r\n                    from vshop_Statistics_Products a\r\n                    inner join Hishop_Products b on a.ProductID  = b.ProductId\r\n                    where CONVERT(varchar(10), RecDate, 120) >= CONVERT(varchar(10), '{0}', 120)  \r\n                          and  CONVERT(varchar(10), RecDate, 120) <=CONVERT(varchar(10), '{1}', 120) and wid='{2}' \r\n                    ) P  \r\n                    ", query.BeginDate.Value.ToString("yyyy-MM-dd"), query.EndDate.Value.ToString("yyyy-MM-dd"),query.wid);
            DataTable table = new DataTable();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select " + str + " from " + str3);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        public DataRow ShopGlobal_GetMemberCount(string wid)
        {
            DataTable table = new DataTable();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("\r\n      select  \r\n (select count(*)   from Hishop_Orders where (OrderStatus=2   or ( OrderStatus=1 and  Gateway='hishop.plugins.payment.podrequest' )) and wid=@wid ) as 'WaitSendOrderQty',\r\n    (select count(*)  from Hishop_Products  where SaleStatus=1 and wid=@wid) as GoodsQty ,\r\n  (select COUNT(*)  from aspnet_Members where Status=1 and wid=@wid ) as  MemberQty,\r\n  (select  COUNT(*) from aspnet_Distributors where  ReferralStatus<=1 ) as DistributorQty,\r\n   (\r\n  select COUNT(*) from\r\n (\r\n    select COUNT(*) SumRec from  Hishop_OrderItems a,Hishop_Orders b where a.orderid=b.orderid and OrderItemsStatus>=6 and  OrderItemsStatus<=8  \r\n   group by a.OrderId\r\n    ) T1\r\n   ) as ServiceOrderQty\r\n     \r\n ");
            this.database.AddInParameter(sqlStringCommand, "wid", DbType.String, wid);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                table = DataHelper.ConverDataReaderToDataTable(reader);
            }
            return table.Rows[0];
        }

        public DataRow ShopGlobal_GetOrderCountByDate(DateTime dDate,string wid)
        {
            DataTable table = new DataTable();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("\r\n  select count(*) as OrderQty , isnull( SUM( ValidOrderTotal),0)  as OrderAmountFee from vw_VShop_FinishOrder_Main \r\n   where  CONVERT(varchar(10),  PayDate  , 120 ) = @RecDate and wid=@wid \r\n ");
            this.database.AddInParameter(sqlStringCommand, "RecDate", DbType.String, dDate.ToString("yyyy-MM-dd"));
            this.database.AddInParameter(sqlStringCommand, "wid", DbType.String,wid);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                table = DataHelper.ConverDataReaderToDataTable(reader);
            }
            return table.Rows[0];
        }

        public DataTable ShopGlobal_GetSortList_Distributor(DateTime BeginDate, int TopCount,string wid)
        {
            DataTable table = new DataTable();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("\r\n     select   top( @TopCount )  T1.*, T1.ReferralUserId as UserID, b.UserName, b.StoreName from \r\n      (\r\n  SELECT a.ReferralUserId, COUNT(*) as OrderQty, SUM(ValidOrderTotal) as ValidOrderTotal , sum(isnull(SumCommission,0)) as SumCommission,\r\n     RANK() OVER  ( ORDER BY SUM(ValidOrderTotal) desc) AS Rank\r\n      FROM vw_VShop_FinishOrder_Main a\r\n                    where ReferralUserId>0\r\n    and  CONVERT(varchar(10),  PayDate ) >= CONVERT(varchar(10),  @BeginDate ) and wid=@wid \r\n   group by a.ReferralUserId\r\n     ) T1\r\n     left join vw_Hishop_DistributorsMembers b on T1.ReferralUserId= b.UserId\r\n    where b.ReferralStatus<=1\r\n                    order by T1.ValidOrderTotal desc\r\n                ");
            this.database.AddInParameter(sqlStringCommand, "TopCount", DbType.Int32, TopCount);
            this.database.AddInParameter(sqlStringCommand, "BeginDate", DbType.Date, BeginDate);
            this.database.AddInParameter(sqlStringCommand, "wid", DbType.String, wid);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        public DataTable ShopGlobal_GetSortList_Member(DateTime BeginDate, int TopCount,string wid)
        {
            DataTable table = new DataTable();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("\r\n     select top( @TopCount )  T1.*, b.UserName from \r\n  (\r\n  SELECT a.UserId, COUNT(*) as OrderQty, SUM(ValidOrderTotal) as ValidOrderTotal ,\r\n   RANK() OVER (ORDER BY SUM(ValidOrderTotal) desc) AS Rank\r\n   FROM vw_VShop_FinishOrder_Main a\r\n    where 1=1\r\n   and  CONVERT(varchar(10),  PayDate ) >= CONVERT(varchar(10),  @BeginDate ) and wid=@wid \r\n                group by a.UserId\r\n\r\n                ) T1\r\n                left join aspnet_Members b on T1.UserId= b.UserId\r\n                where b.Status=1\r\n                order by T1.ValidOrderTotal desc\r\n                ");
            this.database.AddInParameter(sqlStringCommand, "TopCount", DbType.Int32, TopCount);
            this.database.AddInParameter(sqlStringCommand, "BeginDate", DbType.Date, BeginDate);
            this.database.AddInParameter(sqlStringCommand, "wid", DbType.String, wid);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        public DataTable ShopGlobal_GetTrendDataList(DateTime BeginDate, int Days,string wid)
        {
            DataTable table = new DataTable();
            table.Columns.Add(new DataColumn("ID", typeof(int)));
            table.Columns.Add(new DataColumn("RecDate", typeof(DateTime)));
            table.Columns.Add(new DataColumn("OrderCount", typeof(int)));
            table.Columns.Add(new DataColumn("NewMemberCount", typeof(int)));
            table.Columns.Add(new DataColumn("NewDistributorCount", typeof(int)));
            for (int i = 0; i < Days; i++)
            {
                DataRow row = table.NewRow();
                DateTime time = BeginDate.AddDays((double) i);
                DbCommand sqlStringCommand = this.database.GetSqlStringCommand("\r\n     select\r\n     (select count(*) from vw_VShop_FinishOrder_Main where CONVERT(varchar(10),  PayDate )= CONVERT(varchar(10),  @RecDate0 ) and wid=@wid) as OrderCount,\r\n      ( select    COUNT(*)  from aspnet_Members where Status=1 and CONVERT(varchar(10),  CreateDate )= CONVERT(varchar(10),  @RecDate1 ) and wid=@wid ) as MemberQty,\r\n     ( select  COUNT(*) from aspnet_Distributors where ReferralStatus<=1 and  CONVERT(varchar(10),  CreateTime )= CONVERT(varchar(10),  @RecDate2 ) ) as DistributorQty \r\n   ");
                this.database.AddInParameter(sqlStringCommand, "RecDate0", DbType.Date, time);
                this.database.AddInParameter(sqlStringCommand, "RecDate1", DbType.Date, time);
                this.database.AddInParameter(sqlStringCommand, "RecDate2", DbType.Date, time);
                this.database.AddInParameter(sqlStringCommand, "wid", DbType.String, wid);
                DataTable table2 = new DataTable();
                using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
                {
                    table2 = DataHelper.ConverDataReaderToDataTable(reader);
                }
                if (table2.Rows.Count > 0)
                {
                    row["OrderCount"] = table2.Rows[0]["OrderCount"];
                    row["NewMemberCount"] = table2.Rows[0]["MemberQty"];
                    row["NewDistributorCount"] = table2.Rows[0]["DistributorQty"];
                }
                else
                {
                    row["OrderCount"] = 0;
                    row["NewMemberCount"] = 0;
                    row["NewDistributorCount"] = 0;
                }
                row["RecDate"] = time;
                table.Rows.Add(row);
            }
            return table;
        }

        public bool StatisticsOrdersByNotify(DateTime RecDate, UpdateAction FuncAction, string ActionDesc, out string RetInfo)
        {
            DbCommand storedProcCommand = this.database.GetStoredProcCommand("sp_vshop_Statistics_Notify");
            this.database.AddInParameter(storedProcCommand, "@CalDate", DbType.Date, RecDate);
            this.database.AddInParameter(storedProcCommand, "@FuncAction", DbType.Int32, FuncAction);
            this.database.AddInParameter(storedProcCommand, "@ActionDesc", DbType.String, ActionDesc);
            this.database.AddOutParameter(storedProcCommand, "@RetCode", DbType.Int32, 0);
            this.database.AddOutParameter(storedProcCommand, "@RetInfo", DbType.String, 250);
            try
            {
                this.database.ExecuteNonQuery(storedProcCommand);
                RetInfo = storedProcCommand.Parameters["@RetInfo"].Value.ToString();
                return (storedProcCommand.Parameters["@RetCode"].Value.ToString() == "1");
            }
            catch (Exception exception)
            {
                RetInfo = exception.Message;
                return false;
            }
        }

        public bool StatisticsOrdersByRecDate(DateTime RecDate, UpdateAction FuncAction, out string RetInfo)
        {
            DbCommand storedProcCommand = this.database.GetStoredProcCommand("sp_vshop_Statistics_Daily");
            this.database.AddInParameter(storedProcCommand, "@RecDate", DbType.Date, RecDate);
            this.database.AddInParameter(storedProcCommand, "@FuncAction", DbType.Int32, FuncAction);
            this.database.AddOutParameter(storedProcCommand, "@RetCode", DbType.Int32, 0);
            this.database.AddOutParameter(storedProcCommand, "@RetInfo", DbType.String, 250);
            try
            {
                this.database.ExecuteNonQuery(storedProcCommand);
                RetInfo = storedProcCommand.Parameters["@RetInfo"].Value.ToString();
                return (storedProcCommand.Parameters["@RetCode"].Value.ToString() == "1");
            }
            catch (Exception exception)
            {
                RetInfo = exception.Message;
                return false;
            }
        }
    }
}

