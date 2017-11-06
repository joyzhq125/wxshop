namespace Hidistro.UI.Web.Jobs
{
    using Hidistro.ControlPanel.VShop;
    using Hidistro.UI.Web.API;
    using Quartz;
    using System;

    public class ShiftNotify : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            TLog.SaveLog("定时器正执行指定任务...");
            string retInfo = "";
            ShopStatisticHelper.AutoStatisticsOrders(out retInfo);
            TLog.SaveLog("任务执行完毕。结果：" + retInfo);
        }
    }
}

