using System;
using System.Data;
using System.Linq;
using Arsenal.Service;
using Arsenal.Service.Casino;
using Arsenalcn.Core.Dapper;
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
            using (IRepository repo = new Repository())
            {
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
        }

        [TestMethod]
        public void Test_Count()
        {
            using (IRepository repo = new Repository())
            {
                Assert.IsTrue(repo.Count<League>(x => x.IsActive == true) > 0);
                Assert.IsTrue(repo.Count<League>(x => x.LeagueName == "AA") == 0);
            }
        }

        [TestMethod]
        public void Test_All()
        {
            using (IRepository repo = new Repository())
            {
                var list1 = repo.All<League>();

                Assert.IsTrue(list1.Count > 0);

                var list2 = repo.All<Config>();

                Assert.IsTrue(list2.Count > 0);
            }
        }

        [TestMethod]
        public void Test_Query()
        {
            using (IRepository repo = new Repository())
            {
                var list1 = repo.Query<League>(x => x.LeagueName == "");

                Assert.IsNotNull(list1);
                Assert.IsTrue(list1.Count == 0);

                var list2 = repo.Query<Config>(x => x.ConfigSystem == ConfigSystem.Arsenal.ToString());

                Assert.IsNotNull(list2);
                Assert.IsTrue(list2.Count > 0);
            }
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

            using (var dapper = DapperHelper.GetInstance())
            {
                var list = dapper.Query<MatchView, CasinoItem, HomeTeam, AwayTeam, Group, League, MatchView>(sql,
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

        [TestMethod]
        public void Test_DapperPara()
        {
            var para1 = new { UserID = 100, UserName = "Cyrano", ID = 0 };

            var para2 = new DynamicParameters();

            para2.Add("UserID", 100);
            para2.Add("UserName", "Cyrano");
            para2.Add("ID", 0, DbType.Int32, ParameterDirection.Output);

            var json1 = para1.ToJson();
            var json2 = para2.ParameterNames
                .ToDictionary(p => p, p => para2.Get<dynamic>(p)).ToJson();

            Assert.AreEqual(json1, json2);
        }
    }
}
