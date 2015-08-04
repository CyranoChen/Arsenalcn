using System;
using System.Reflection;
using System.Threading;

using Arsenalcn.Core;
using Arsenalcn.Core.Logger;
using Arsenalcn.Core.Scheduler;

using Arsenal.Service.Casino;

namespace Arsenal.Scheduler
{
    class AutoUpdateActiveBankerCash : ISchedule
    {
        private readonly ILog log = new AppLog();

        public void Execute(object state)
        {
            var logInfo = new LogInfo()
            {
                MethodInstance = MethodBase.GetCurrentMethod(),
                ThreadInstance = Thread.CurrentThread
            };

            try
            {
                log.Info("Scheduler Start: (AutoUpdateActiveBankerCash)", logInfo);

                IRepository repo = new Repository();

                var list = repo.All<Banker>().FindAll(x => x.IsActive);

                foreach (var b in list)
                {
                    b.Statistics();
                }

                log.Info("Scheduler End: (AutoUpdateActiveBankerCash)", logInfo);
            }
            catch (Exception ex)
            {
                log.Warn(ex, logInfo);
            }
        }
    }
}