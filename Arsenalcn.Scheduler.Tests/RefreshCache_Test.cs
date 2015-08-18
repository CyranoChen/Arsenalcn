using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Arsenalcn.Core;
using Arsenal.Service;

namespace Arsenalcn.Scheduler.Tests
{
    [TestClass]
    public class RefreshCache
    {
        [TestMethod]
        public void ArsenalServiceRefreshCache_Test()
        {
            try
            {
                Config.Cache.RefreshCache();

                RelationLeagueTeam.Clean();
                RelationLeagueTeam.Cache.RefreshCache();

                League.Cache.RefreshCache();
                Match.Cache.RefreshCache();
                Player.Cache.RefreshCache();
                Team.Cache.RefreshCache();
                Video.Cache.RefreshCache();

                //AcnCasino
                Arsenal.Service.Casino.CasinoItem.Clean();
                Arsenal.Service.Casino.ChoiceOption.Clean();
                Arsenal.Service.Casino.Bet.Clean();
                Arsenal.Service.Casino.BetDetail.Clean();

                // Clean Log
                Arsenalcn.Core.Logger.Log.Clean();
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }
    }
}
