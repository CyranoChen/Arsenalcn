using System;
using System.Reflection;
using System.Threading;
using Arsenal.Service.Casino;
using Arsenalcn.Core;
using Arsenalcn.Core.Logger;
using Arsenalcn.Core.Scheduler;

namespace Arsenal.Scheduler
{
    internal class AutoUpdateActiveCasinoItemEarning : ISchedule
    {
        private readonly ILog log = new AppLog();

        public void Execute(object state)
        {
            var logInfo = new LogInfo
            {
                MethodInstance = MethodBase.GetCurrentMethod(),
                ThreadInstance = Thread.CurrentThread
            };

            try
            {
                log.Info("Scheduler Start: (AutoUpdateActiveCasinoItemEarning)", logInfo);

                IRepository repo = new Repository();

                var list = repo.All<CasinoItem>().FindAll(x =>
                    x.ItemType.Equals(CasinoType.SingleChoice) && x.Earning.HasValue);

                foreach (var c in list)
                {
                    c.Statistics();
                }

                log.Info("Scheduler End: (AutoUpdateActiveCasinoItemEarning)", logInfo);
            }
            catch (Exception ex)
            {
                log.Warn(ex, logInfo);
            }
        }
    }
}