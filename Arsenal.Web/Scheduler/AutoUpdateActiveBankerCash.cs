using System;
using System.Reflection;
using System.Threading;
using Arsenal.Service.Casino;
using Arsenalcn.Core;
using Arsenalcn.Core.Dapper;
using Arsenalcn.Core.Logger;
using Arsenalcn.Core.Scheduler;

namespace Arsenal.Scheduler
{
    internal class AutoUpdateActiveBankerCash : ISchedule
    {
        private readonly ILog _log = new AppLog();

        public void Execute(object state)
        {
            var logInfo = new LogInfo
            {
                MethodInstance = MethodBase.GetCurrentMethod(),
                ThreadInstance = Thread.CurrentThread
            };

            try
            {
                _log.Info("Scheduler Start: (AutoUpdateActiveBankerCash)", logInfo);

                IRepository repo = new Repository();

                var list = repo.All<Banker>().FindAll(x => x.IsActive);

                foreach (var b in list)
                {
                    b.Statistic();
                }

                _log.Info("Scheduler End: (AutoUpdateActiveBankerCash)", logInfo);
            }
            catch (Exception ex)
            {
                _log.Warn(ex, logInfo);
            }
        }
    }
}