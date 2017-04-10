using System;
using System.Reflection;
using System.Threading;
using Arsenal.Service;
using Arsenal.Service.Casino;
using Arsenalcn.Core;
using Arsenalcn.Core.Dapper;
using Arsenalcn.Core.Logger;
using Arsenalcn.Core.Scheduler;

namespace Arsenal.Scheduler
{
    public class GroupTableStatistics : ISchedule
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
                _log.Info("Scheduler Start: (GroupTableStatistics)", logInfo);

                IRepository repo = new Repository();

                var list = repo.All<Group>().FindAll(x => League.Cache.Load(x.LeagueGuid).IsActive);

                if (list.Count > 0)
                {
                    foreach (var g in list)
                    {
                        g.Statistic();
                    }
                }

                _log.Info("Scheduler End: (GroupTableStatistics)", logInfo);
            }
            catch (Exception ex)
            {
                _log.Warn(ex, logInfo);
            }
        }
    }
}