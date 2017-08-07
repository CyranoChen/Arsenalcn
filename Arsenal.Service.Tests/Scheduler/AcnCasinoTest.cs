using System;
using System.Linq;
using Arsenal.Service.Casino;
using Arsenalcn.Core.Dapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArsenalMatch = Arsenal.Service.Match;
using CasinoMatch = Arsenal.Service.Casino.Match;

namespace Arsenal.Service.Tests.Scheduler
{
    [TestClass]
    public class AcnCasinoTest
    {
        [TestMethod]
        public void AutoUpdateArsenalMatchResult_Test()
        {
            try
            {
                IRepository repo = new Repository();

                var cmList = repo.All<CasinoMatch>();
                var mList = repo.All<ArsenalMatch>();

                if (mList != null && mList.Count > 0 && cmList != null && cmList.Count > 0)
                {
                    foreach (var m in mList)
                    {
                        CasinoMatch cm;

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

                        if (cm?.ResultHome != null && cm.ResultAway.HasValue)
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
                    }
                }
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod]
        public void AutoUpdateActiveCasinoItemEarning_Test()
        {
            try
            {
                using (IRepository repo = new Repository())
                {

                    var list = repo.All<CasinoItem>().FindAll(x =>
                        x.ItemType.Equals(CasinoType.SingleChoice) && x.Earning.HasValue);

                    foreach (var c in list.Take(10))
                    {
                        c.Statistics();
                    }
                }
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod]
        public void AutoUpdateActiveBankerCash_Test()
        {
            try
            {
                using (IRepository repo = new Repository())
                {
                    var list = repo.All<Banker>().FindAll(x => x.IsActive);

                    foreach (var b in list)
                    {
                        b.Statistic();
                    }
                }
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod]
        public void GroupTableStatistics_Test()
        {
            try
            {
                IRepository repo = new Repository();

                var list = repo.All<Group>().FindAll(x => League.Cache.Load(x.LeagueGuid).IsActive);

                if (list.Count > 0)
                {
                    foreach (var g in list)
                    {
                        g.Statistic();
                    }
                }
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod]
        public void AutoUpdateMonthlyRank_Test()
        {
            try
            {
                using (IRepository repo = new Repository())
                {
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
                                rank.Init(winner, loser, rper);

                                repo.Update(rank);
                            }
                            else
                            {
                                //insert
                                var instance = new Rank { RankYear = day.Year, RankMonth = day.Month };
                                instance.Init(winner, loser, rper);

                                repo.Insert(instance);
                            }
                        }
                        iDay = iDay.AddMonths(-1);
                    }
                }
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }
    }
}