using System;
using System.Reflection;
using System.Threading;
using Arsenal.Service;
using Arsenal.Service.Casino;
using Arsenalcn.Core;
using Arsenalcn.Core.Logger;
using Arsenalcn.Core.Scheduler;
using Match = Arsenal.Service.Match;

namespace Arsenal.Scheduler
{
    internal class RefreshCache : ISchedule
    {
        private readonly ILog log = new AppLog();

        public void Execute(object state)
        {
            var logInfo = new LogInfo
            {
                MethodInstance = MethodBase.GetCurrentMethod(),
                ThreadInstance = Thread.CurrentThread
            };

            //string _scheduleType = this.GetType().DeclaringType.FullName;

            try
            {
                log.Info("Scheduler Start: (RefreshCache)", logInfo);

                Config.UpdateAssemblyInfo(Assembly.GetExecutingAssembly(), ConfigSystem.Arsenal);

                ConfigGlobal.Refresh();

                RelationLeagueTeam.Clean();
                RelationLeagueTeam.Cache.RefreshCache();

                League.Cache.RefreshCache();
                Match.Cache.RefreshCache();
                Player.Cache.RefreshCache();
                Team.Cache.RefreshCache();
                Video.Cache.RefreshCache();

                //AcnCasino
                CasinoItem.Clean();
                ChoiceOption.Clean();
                Bet.Clean();
                BetDetail.Clean();

                // Clean Log
                Log.Clean();

                log.Info("Scheduler End: (RefreshCache)", logInfo);
            }
            catch (Exception ex)
            {
                log.Warn(ex, logInfo);
            }
        }
    }
}