using System;
using System.Linq;
using Arsenal.Service;
using Arsenal.Service.Casino;
using Dapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Arsenalcn.Core.Tests
{
    [TestClass]
    public class DapperTest
    {
        [TestMethod]
        public void Test_Single()
        {
            IRepository repo = new Repository();

            var key1 = new Guid("FD32F77D-47A7-4D5F-B7CE-068E3E1A0833");

            var instance1 = repo.Single<League>(key1);

            Assert.IsNotNull(instance1);
            Assert.IsInstanceOfType(instance1, typeof(League));
            Assert.IsNotNull(instance1.LeagueNameInfo);
            Assert.AreEqual(key1, instance1.ID);

            // wrong value of argument
            var key2 = new Guid();

            var instance2 = repo.Single<League>(key2);

            Assert.IsNull(instance2);
        }

        [TestMethod]
        public void Test_Count()
        {
            IRepository repo = new Repository();

            Assert.IsTrue(repo.Count<League>(x => x.IsActive == true) > 0);
            Assert.IsTrue(repo.Count<League>(x => x.LeagueName == "AA") == 0);
        }

        [TestMethod]
        public void Test_All()
        {
            IRepository repo = new Repository();

            var list1 = repo.All<League>();

            Assert.IsTrue(list1.Count > 0);

            var list2 = repo.All<Config>();

            Assert.IsTrue(list2.Count > 0);
        }

        [TestMethod]
        public void Test_Query()
        {
            IRepository repo = new Repository();

            var list1 = repo.Query<League>(x => x.LeagueName == "");

            Assert.IsNotNull(list1);
            Assert.IsTrue(list1.Count == 0);

            var list2 = repo.Query<Config>(x => x.ConfigSystem.ToString() == ConfigSystem.Arsenal.ToString());

            Assert.IsNotNull(list2);
            Assert.IsTrue(list2.Count > 0);
        }

        [TestMethod]
        public void Test_Viewer()
        {
            var sql = @"SELECT m.MatchGuid AS ID, m.ResultHome, m.ResultAway, m.PlayTime, m.LeagueName, m.Round, 
                  c.CasinoItemGuid, c.ItemType, c.MatchGuid, c.CloseTime, c.Earning, 
                  h.TeamGuid AS HomeTeamGuid, h.TeamEnglishName AS HomeEnglishName, h.TeamDisplayName AS HomeDisplayName, h.TeamLogo AS HomeLogo, 
                  a.TeamGuid AS AwayTeamGuid, a.TeamEnglishName AS AwayEnglishName, a.TeamDisplayName AS AwayDisplayName, a.TeamLogo AS AwayLogo, 
                  g.GroupGuid, g.GroupName, g.IsTable, 
                  l.LeagueGuid, l.LeagueOrgName, l.LeagueSeason, l.LeagueLogo
                  FROM     dbo.AcnCasino_Match AS m LEFT OUTER JOIN
                      dbo.AcnCasino_CasinoItem AS c ON m.MatchGuid = c.MatchGuid LEFT OUTER JOIN
                      dbo.Arsenal_League AS l ON m.LeagueGuid = l.LeagueGuid LEFT OUTER JOIN
                      dbo.Arsenal_Team AS h ON m.Home = h.TeamGuid LEFT OUTER JOIN
                      dbo.Arsenal_Team AS a ON m.Away = a.TeamGuid LEFT OUTER JOIN
                      dbo.Arsenal_Group AS g ON m.GroupGuid = g.GroupGuid
                  WHERE  (c.ItemType = 2)";

            var list = DapperHelper.Connection.Query<MatchView, CasinoItem, HomeTeam, AwayTeam, Group, League, MatchView>(sql,
                            (x, c, h, a, g, l) =>
                            {
                                x.CasinoItem = c;
                                x.Home = h;
                                x.Away = a;
                                x.Group = g;
                                x.League = l;

                                return x;
                            }, splitOn: "ID, CasinoItemGuid, HomeTeamGuid, AwayTeamGuid, GroupGuid, LeagueGuid").ToList<IViewer>();


            Assert.IsTrue(list.Count > 0);

            IViewerFactory<MatchView> factory = new MatchViewFactory();

            var result = factory.All();

            Assert.IsInstanceOfType(result[0], typeof(MatchView));
        }
    }
}
