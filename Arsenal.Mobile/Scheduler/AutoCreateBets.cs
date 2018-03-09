using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using Arsenal.Mobile.Models.Casino;
using Arsenal.Service;
using Arsenal.Service.Casino;
using Arsenalcn.Core;
using Arsenalcn.Core.Dapper;
using Arsenalcn.Core.Extension;
using Arsenalcn.Core.Logger;
using Arsenalcn.Core.Scheduler;

namespace Arsenal.Mobile.Scheduler
{
    internal class AutoCreateBets : ISchedule
    {
        private readonly ILog _log = new AppLog();

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
                _log.Info("Scheduler Start: (AutoCreateBets)", logInfo);

                // Get all open matches which could be bet
                // no deadline set
                IViewerFactory<MatchView> factory = new MatchViewFactory();

                var query = factory.All()
                    .FindAll(x => x.League.ID.Equals(ConfigGlobal_AcnCasino.DefaultLeagueID))
                    .FindAll(x => x.PlayTime > DateTime.Now)
                    .FindAll(x => !x.ResultHome.HasValue && !x.ResultAway.HasValue)
                    .OrderBy(x => x.PlayTime)
                    .Many<MatchView, ChoiceOption, Guid>(t => t.CasinoItem.ID);

                var mapper = MatchDto.ConfigMapper().CreateMapper();

                var list = mapper.Map<IEnumerable<MatchDto>>(query.AsEnumerable()).ToList();

                // Exist open bet match
                if (list.Count > 0)
                {
                    foreach (var uid in ConfigGlobal_AcnCasino.AutoBetUsers)
                    {
                        IRepository repo = new Repository();
                        var g = repo.Single<Gambler>(x => x.UserID == uid);

                        // Get all bet records by users
                        var bets = GetBetsByUserId(uid);

                        foreach (var m in list)
                        {
                            // no bet of MatchResult
                            if (!bets.Exists(x => x.ItemType.Equals(CasinoType.MatchResult) && x.MatchGuid.Equals(m.ID)))
                            {
                                // Bet it
                                var bet = new Bet
                                {
                                    UserID = uid,
                                    UserName = g.UserName
                                };

                                if (m.HomeRate < m.AwayRate)
                                {
                                    bet.Place(m.ID, resultHome: 1, resultAway: 0);
                                }
                                else if (m.HomeRate > m.AwayRate)
                                {
                                    bet.Place(m.ID, resultHome: 0, resultAway: 1);
                                }
                                else if (Math.Abs(m.HomeRate - m.AwayRate) < 0.001)
                                {
                                    bet.Place(m.ID, resultHome: 1, resultAway: 1);
                                }
                            }

                            // no bet of SingleChoice
                            if (!bets.Exists(x => x.ItemType.Equals(CasinoType.SingleChoice) && x.MatchGuid.Equals(m.ID)))
                            {
                                var option = m.HomeRate < m.AwayRate ? "home" : "away";
                                var amount = Math.Min(m.HomeRate, m.AwayRate) < 2.0f ? 5000 : 2000;

                                // tackle with rate equal condition
                                if (Math.Abs(m.HomeRate - m.AwayRate) < 0.001)
                                {
                                    option = "draw";
                                    amount = m.DrawRate < 2.0f ? 5000 : 2000;
                                }

                                // Bet it
                                var bet = new Bet
                                {
                                    UserID = uid,
                                    UserName = g.UserName,
                                    BetAmount = amount
                                };

                                bet.Place(m.ID, selectedOption: option);
                            }
                        }
                    }
                }

                _log.Info("Scheduler End: (AutoCreateBets)", logInfo);
            }
            catch (Exception ex)
            {
                _log.Warn(ex, logInfo);
            }
        }

        private List<BetDto> GetBetsByUserId(int userId)
        {
            var criteria = new Criteria(new { UserID = userId });
            var queryBets = new BetViewFactory().Query(criteria).Many<BetView, BetDetail, int>(t => t.ID);
            var mapper = BetDto.ConfigMapper().CreateMapper();

            return mapper.Map<IEnumerable<BetDto>>(queryBets.AsEnumerable()).ToList();
        }
    }
}