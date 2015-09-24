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
        private readonly ILog _log = new AppLog();

        public void Execute(object state)
        {
            var logInfo = new LogInfo()
            {
                MethodInstance = MethodBase.GetCurrentMethod(),
                ThreadInstance = Thread.CurrentThread
            };

            //string _scheduleType = this.GetType().DeclaringType.FullName;

            try
            {
                _log.Info("Scheduler Start: (RefreshCache)", logInfo);

                Config.Cache.RefreshCache();

                RelationLeagueTeam.Clean();
                RelationLeagueTeam.Cache.RefreshCache();

                League.Cache.RefreshCache();
                Match.Cache.RefreshCache();
                Player.Cache.RefreshCache();
                Team.Cache.RefreshCache();
                Video.Cache.RefreshCache();

                _log.Info("Scheduler End: (RefreshCache)", logInfo);
            }
            catch (Exception ex)
            {
                _log.Warn(ex, logInfo);
            }
        }
    }
}