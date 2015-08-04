using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Arsenalcn.Core;

using ArsenalMatch = Arsenal.Service.Match;
using CasinoMatch = Arsenal.Service.Casino.Match;
using Arsenal.Service.Casino;

namespace Arsenalcn.Scheduler.Tests
{
    [TestClass]
    public class Casino_Test
    {
        [TestMethod]
        public void AutoUpdateArsenalMatchResult_Test()
        {
            try
            {
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
                                else
                                {
                                    return m.TeamGuid.Equals(x.Home) && m.PlayTime.Equals(x.PlayTime);
                                }
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

                            repo.Update<ArsenalMatch>(m);
                        }
                        else
                        {
                            continue;
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
                IRepository repo = new Repository();

                var list = repo.All<CasinoItem>().FindAll(x =>
                    x.ItemType.Equals(CasinoType.SingleChoice) && x.Earning.HasValue);

                foreach (var c in list)
                {
                    c.Statistics();
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
                IRepository repo = new Repository();

                var list = repo.All<Banker>().FindAll(x => x.IsActive);

                foreach (var b in list)
                {
                    b.Statistics();
                }
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }

        }
    }
}
