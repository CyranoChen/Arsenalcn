using System;
using System.Reflection;
using System.Threading;

using Arsenal.Service;
using Arsenalcn.Core;
using Arsenalcn.Core.Logger;
using Arsenalcn.Core.Scheduler;

namespace Arsenal.Scheduler
{
    class RefreshCache : ISchedule
    {
        private readonly ILog log = new AppLog();

        public void Execute(object state)
        {
            var logInfo = new LogInfo()
            {
                MethodInstance = MethodBase.GetCurrentMethod(),
                ThreadInstance = Thread.CurrentThread
            };

            string _scheduleType = this.GetType().DeclaringType.FullName;

            try
            {
                log.Info(string.Format("Scheduler Start: {0}", _scheduleType), logInfo);

                Config.Cache.RefreshCache();

                RelationLeagueTeam.Clean();
                RelationLeagueTeam.Cache.RefreshCache();

                League.Cache.RefreshCache();
                Match.Cache.RefreshCache();
                Player.Cache.RefreshCache();
                Team.Cache.RefreshCache();
                Video.Cache.RefreshCache();

                log.Info(string.Format("Scheduler End: {0}", _scheduleType), logInfo);
            }
            catch (Exception ex)
            {
                log.Warn(ex, logInfo);
            }
        }
    }
}