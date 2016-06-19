using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using Arsenal.Service.Casino;
using Arsenalcn.Core;
using Arsenalcn.Core.Logger;
using Arsenalcn.Core.Scheduler;

namespace Arsenal.Scheduler
{
    internal class AutoUpdateMonthlyRank : ISchedule
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
                _log.Info("Scheduler Start: (AutoUpdateMonthlyRank)", logInfo);

                IRepository repo = new Repository();

                var iDay = DateTime.Today;

                var firstBetDate = repo.Single<Bet>(1).BetTime;

                while (!(iDay.Year <= firstBetDate.Year && iDay.Month < firstBetDate.Month))
                {
                    var winner = GamblerDW.GetTopGamblerMonthly(iDay, RankType.Winner);
                    var loser = GamblerDW.GetTopGamblerMonthly(iDay, RankType.Loser);
                    var rper = GamblerDW.GetTopGamblerMonthly(iDay, RankType.RP);

                    if (winner != null && loser != null)
                    {
                        var day = iDay;
                        var rank = repo.Query<Rank>(x => x.RankYear == day.Year && x.RankMonth == day.Month).FirstOrDefault();

                        if (rank != null)
                        {
                            //update
                            rank.Initial(winner, loser, rper);

                            repo.Update(rank);
                        }
                        else
                        {
                            //insert
                            var instance = new Rank { RankYear = day.Year, RankMonth = day.Month };
                            instance.Initial(winner, loser, rper);

                            repo.Insert(instance);
                        }
                    }
                    iDay = iDay.AddMonths(-1);
                }

                _log.Info("Scheduler End: (AutoUpdateMonthlyRank)", logInfo);
            }
            catch (Exception ex)
            {
                _log.Warn(ex, logInfo);
            }
        }
    }
}