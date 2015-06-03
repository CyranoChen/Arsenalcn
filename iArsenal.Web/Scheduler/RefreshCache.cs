﻿using System;
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

            string _scheduleType = this.GetType().DeclaringType.FullName;

            try
            {
                log.Info(string.Format("Scheduler Start: {0}", _scheduleType), logInfo);

                Config.Cache.RefreshCache();

                Arsenal_Match.Cache.RefreshCache();
                Arsenal_Player.Cache.RefreshCache();
                Arsenal_Team.Cache.RefreshCache();

                MatchTicket.Cache.RefreshCache();
                Member.Cache.RefreshCache();
                Product.Cache.RefreshCache();

                log.Info(string.Format("Scheduler End: {0}", _scheduleType), logInfo);
            }
            catch (Exception ex)
            {
                log.Warn(ex, logInfo);
            }
        }
    }
}