using System;
using System.Reflection;
using System.Threading;

using Arsenalcn.Core;
using Arsenalcn.Core.Logger;
using Arsenalcn.Core.Scheduler;
using iArsenal.Service;

namespace iArsenal.Scheduler
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

            try
            {
                log.Info("Scheduler Start (RefreshCache)", logInfo);

                Config.Cache.RefreshCache();

                Arsenal_Match.Cache.RefreshCache();
                Arsenal_Player.Cache.RefreshCache();
                Arsenal_Team.Cache.RefreshCache();

                MatchTicket.Cache.RefreshCache();
                Member.Cache.RefreshCache();
                Product.Cache.RefreshCache();
            }
            catch (Exception ex)
            {
                log.Warn(ex, logInfo);
            }
        }
    }
}