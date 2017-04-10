using System;
using System.Reflection;
using System.Threading;
using Arsenalcn.Core;
using Arsenalcn.Core.Dapper;
using Arsenalcn.Core.Logger;
using Arsenalcn.Core.Scheduler;
using ArsenalMatch = Arsenal.Service.Match;
using CasinoMatch = Arsenal.Service.Casino.Match;

namespace Arsenal.Scheduler
{
    internal class AutoUpdateArsenalMatchResult : ISchedule
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
                log.Info("Scheduler Start: (AutoUpdateArsenalMatchResult)", logInfo);

                IRepository repo = new Repository();

                var cmList = repo.All<CasinoMatch>();
                var mList = repo.All<ArsenalMatch>();

                if (mList != null & mList.Count > 0 & cmList != null & cmList.Count > 0)
                {
                    foreach (var m in mList)
                    {
                        var cm = new CasinoMatch();

                        if (m.CasinoMatchGuid.HasValue)
                        {
                            //Casino MatchGuid Bound
                            cm = cmList.Find(x => x.ID.Equals(m.CasinoMatchGuid.Value));
                        }
                        else
                        {
                            //new Arsenal Match
                            cm = cmList.Find(x =>
                            {
                                if (m.IsHome)
                                {
                                    return m.TeamGuid.Equals(x.Away) && m.PlayTime.Equals(x.PlayTime);
                                }
                                return m.TeamGuid.Equals(x.Home) && m.PlayTime.Equals(x.PlayTime);
                            });
                        }

                        if (cm != null && cm.ResultHome.HasValue && cm.ResultAway.HasValue)
                        {
                            if (m.ResultHome.Equals(cm.ResultHome) && m.ResultAway.Equals(cm.ResultAway)
                                && m.PlayTime.Equals(cm.PlayTime) && m.CasinoMatchGuid.Equals(cm.ID))
                            {
                                continue;
                            }

                            m.ResultHome = cm.ResultHome;
                            m.ResultAway = cm.ResultAway;
                            m.PlayTime = cm.PlayTime;
                            m.CasinoMatchGuid = cm.ID;

                            repo.Update(m);
                        }
                        else
                        {
                        }
                    }
                }

                ArsenalMatch.Cache.RefreshCache();

                log.Info("Scheduler End: (AutoUpdateArsenalMatchResult)", logInfo);
            }
            catch (Exception ex)
            {
                log.Warn(ex, logInfo);
            }
        }
    }
}