using System;
using System.Collections.Generic;
using System.Linq;
using Arsenal.Service;
using Arsenal.Service.Casino;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Arsenalcn.Core.Tests
{
    [TestClass]
    public class RepositoryTest
    {
        [TestMethod]
        public void Test_Single()
        {
            IRepository repo = new Repository();

            // correct value of argument
            var key1 = new Guid("FD32F77D-47A7-4D5F-B7CE-068E3E1A0833");

            var instance1 = repo.Single<League>(key1);

            Assert.IsNotNull(instance1);
            Assert.IsInstanceOfType(instance1, typeof(League));

            // wrong value of argument
            var key2 = new Guid();

            var instance2 = repo.Single<League>(key2);

            Assert.IsNull(instance2);
        }

        [Ignore]
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Test_Single_ArgumentNullException()
        {
            IRepository repo = new Repository();

            repo.Single<League>(null);

            IViewerFactory<MatchView> factory = new MatchViewFactory();

            factory.Single(null);
        }

        [Ignore]
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Test_Single_ArgumentException()
        {
            IRepository repo = new Repository();

            var key1 = 10000; // require guid, input int

            repo.Single<League>(key1);
            //repo.Single<MatchView>(key1);

            var key2 = new Guid(); // require int, input guid

            repo.Single<Bet>(key2);
            //repo.Single<BetView>(key2);
        }

        [TestMethod]
        public void Test_Single_Viewer()
        {
            var factory = new MatchViewFactory();

            // the match has not group
            var key1 = new Guid("12236a72-f35b-4a0f-90e6-67b11c3364bc");

            var instance1 = factory.Single(key1);

            Assert.IsNotNull(instance1);
            Assert.IsInstanceOfType(instance1, typeof(MatchView));
            Assert.IsNotNull(instance1.CasinoItem);
            Assert.IsNotNull(instance1.Home);
            Assert.IsNotNull(instance1.Away);
            Assert.IsNull(instance1.Group); // no relation group
            Assert.IsNotNull(instance1.League);
            Assert.IsNull(instance1.ChoiceOptions); // no relation choiceOptions Init

            // the match has group
            var key2 = new Guid("73c314c3-4e50-428d-b698-475fb854e4ea");

            var instance2 = factory.Single(key2);

            Assert.IsNotNull(instance2.Group); // has relation group
        }

        [TestMethod]
        public void Test_Single_Viewer_Many()
        {
            var factory = new MatchViewFactory();

            var key1 = new Guid("12236a72-f35b-4a0f-90e6-67b11c3364bc");

            var instance1 = factory.Single(key1);

            instance1.Many<ChoiceOption>(x => x.CasinoItemGuid == instance1.CasinoItem.ID);

            Assert.IsNotNull(instance1);
            Assert.IsInstanceOfType(instance1, typeof(MatchView));
            Assert.IsNotNull(instance1.ChoiceOptions);
            Assert.IsTrue(instance1.ChoiceOptions.Any());

            var instance2 = factory.Single(key1);

            instance2.Many<ChoiceOption>(x => x.CasinoItemGuid == instance1.ID);

            Assert.IsNotNull(instance2);
            Assert.IsInstanceOfType(instance2, typeof(MatchView));
            Assert.IsNull(instance2.ChoiceOptions);
        }

        [Ignore]
        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void Test_Single_Viewer_Many_FormatException()
        {
            var factory = new MatchViewFactory();

            var key = new Guid("12236a72-f35b-4a0f-90e6-67b11c3364bc");

            var instance = factory.Single(key);

            instance.Many<League>(x => x.ID == instance.CasinoItem.ID);
        }

        [TestMethod]
        public void Test_All()
        {
            IRepository repo = new Repository();

            var query = repo.All<League>();

            Assert.IsNotNull(query);
            Assert.IsInstanceOfType(query, typeof(List<League>));
            Assert.IsTrue(query.Any());
        }

        [TestMethod]
        public void Test_All_Viewer()
        {
            IViewerFactory<MatchView> factory = new MatchViewFactory();

            var query = factory.All();

            Assert.IsNotNull(query);
            Assert.IsInstanceOfType(query, typeof(List<MatchView>));
            Assert.IsTrue(query.Any());

            var instance = query.First();

            Assert.IsNotNull(instance);
            Assert.IsInstanceOfType(instance, typeof(MatchView));
            Assert.IsNotNull(instance.CasinoItem);
            Assert.IsNotNull(instance.Home);
            Assert.IsNotNull(instance.Away);
            Assert.IsNotNull(instance.League);
            Assert.IsNull(instance.ChoiceOptions); // no relation choiceOptions Init
        }

        [TestMethod]
        public void Test_All_Viewer_Many_Linq()
        {
            IViewerFactory<MatchView> factory = new MatchViewFactory();

            var query1 = factory.All().Take(5).ToList();

            query1.Many<MatchView, ChoiceOption>((tOne, tMany) => tOne.CasinoItem.ID.Equals(tMany.CasinoItemGuid));

            Assert.IsNotNull(query1);
            Assert.IsInstanceOfType(query1, typeof(List<MatchView>));
            Assert.IsTrue(query1.Any());

            var instance1 = query1.Last();

            Assert.IsNotNull(instance1);
            Assert.IsInstanceOfType(instance1, typeof(MatchView));
            Assert.IsNotNull(instance1.ChoiceOptions);
            Assert.IsTrue(instance1.ChoiceOptions.Any());

            var query2 = factory.All().Take(5).ToList();

            query2.Many<MatchView, ChoiceOption>((tOne, tMany) => tOne.ID.Equals(tMany.CasinoItemGuid));

            Assert.IsNotNull(query2);
            Assert.IsInstanceOfType(query2, typeof(List<MatchView>));
            Assert.IsTrue(query2.Any());

            var instance2 = query2.First();

            Assert.IsNotNull(instance2);
            Assert.IsInstanceOfType(instance2, typeof(MatchView));
            Assert.IsNull(instance2.ChoiceOptions);
        }

        [TestMethod]
        public void Test_All_Viewer_Many_Sql()
        {
            IViewerFactory<MatchView> factory = new MatchViewFactory();

            var query1 = factory.All().Take(5).ToList();

            query1.Many<MatchView, ChoiceOption, Guid>(t => t.CasinoItem.ID);

            Assert.IsNotNull(query1);
            Assert.IsInstanceOfType(query1, typeof(List<MatchView>));
            Assert.IsTrue(query1.Any());

            var instance1 = query1.Last();

            Assert.IsNotNull(instance1);
            Assert.IsInstanceOfType(instance1, typeof(MatchView));
            Assert.IsNotNull(instance1.ChoiceOptions);
            Assert.IsTrue(instance1.ChoiceOptions.Any());

            var query2 = factory.All().Take(5).ToList();

            query1.Many<MatchView, ChoiceOption, Guid>(t => t.ID);

            Assert.IsNotNull(query2);
            Assert.IsInstanceOfType(query2, typeof(List<MatchView>));
            Assert.IsTrue(query2.Any());

            var instance2 = query2.First();

            Assert.IsNotNull(instance2);
            Assert.IsInstanceOfType(instance2, typeof(MatchView));
            Assert.IsNull(instance2.ChoiceOptions);

        }

        [TestMethod]
        public void Test_All_Viewer_Many_Sql_Large()
        {
            var factory = new BetViewFactory();

            var query1 = factory.All(new Pager { PagingSize = 1000 }).ToList();

            query1.Many<BetView, BetDetail, int>(t => t.ID);

            Assert.IsNotNull(query1);
            Assert.IsInstanceOfType(query1, typeof(List<BetView>));
            Assert.IsTrue(query1.Last().BetDetails.Any());
        }

        [Ignore]
        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void Test_All_Viewer_Many_FormatException()
        {
            IViewerFactory<MatchView> factory = new MatchViewFactory();

            var query = factory.All().Take(2).ToList();

            query.Many<MatchView, League>((tOne, tMany) => tOne.CasinoItem.ID.Equals(tMany.ID));
            query.Many<MatchView, League, Guid>(t => t.ID);
        }

        [Ignore]
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Test_All_Viewer_Many_ArgumentException()
        {
            IViewerFactory<MatchView> factory = new MatchViewFactory();

            var query = factory.All().Take(2).ToList();

            query.Many<MatchView, ChoiceOption, DateTime>(t => t.PlayTime);
        }

        [TestMethod]
        public void Test_All_Pager()
        {
            IRepository repo = new Repository();

            // normal condition
            IPager pager1 = new Pager(1) { PagingSize = 10 };

            Assert.IsFalse(pager1.TotalCount > 0);

            var query1 = repo.All<League>(pager1, "LeagueOrder, LeagueOrgName");
            var queryVal = repo.All<League>();

            Assert.IsNotNull(query1);
            Assert.IsInstanceOfType(query1, typeof(List<League>));
            Assert.IsTrue(query1.Any());

            Assert.IsTrue(pager1.TotalCount > 0);
            Assert.AreEqual(pager1.TotalCount.ToString(), queryVal.Count.ToString());
            Assert.AreEqual(pager1.PagingSize.ToString(), query1.Count.ToString());

            Assert.IsTrue(query1[0].Equals(queryVal[10]));

            // large pagingSize
            IPager pager2 = new Pager(0) { PagingSize = 1000 };

            var query2 = repo.All<League>(pager2, "LeagueOrder, LeagueOrgName");

            Assert.IsNotNull(query2);
            Assert.IsInstanceOfType(query2, typeof(List<League>));
            Assert.IsTrue(query2.Any());

            Assert.IsTrue(pager2.TotalCount > 0);
            Assert.AreEqual(pager2.TotalCount.ToString(), queryVal.Count.ToString());
            Assert.AreEqual(query2.Count.ToString(), queryVal.Count.ToString());

            // max currentPage
            IPager pager3 = new Pager(1000) { PagingSize = 20 };

            var query3 = repo.All<League>(pager3, "LeagueOrder, LeagueOrgName");

            Assert.IsNotNull(query3);
            Assert.IsInstanceOfType(query3, typeof(List<League>));
            Assert.IsTrue(query3.Any());

            Assert.IsTrue(pager2.TotalCount > 0);
            Assert.AreEqual(pager2.TotalCount.ToString(), queryVal.Count.ToString());
            Assert.AreEqual(pager3.CurrentPage.ToString(), pager3.MaxPage.ToString());
            Assert.AreEqual(query3.Count.ToString(), (queryVal.Count % pager3.PagingSize).ToString());
        }

        [TestMethod]
        public void Test_All_Pager_Viewer()
        {
            var factory = new BetViewFactory();

            IPager pager = new Pager { PagingSize = 20 };

            Assert.IsFalse(pager.TotalCount > 0);

            var query = factory.All(pager, "BetTime DESC");

            Assert.IsNotNull(query);
            Assert.IsInstanceOfType(query, typeof(List<BetView>));
            Assert.IsTrue(query.Any());

            Assert.IsTrue(pager.TotalCount > 0);
            Assert.AreEqual(pager.PagingSize.ToString(), query.Count.ToString());
        }

        [TestMethod]
        public void Test_Query()
        {
            IRepository repo = new Repository();

            // correct value of argument
            // ReSharper disable once RedundantBoolCompare
            var query1 = repo.Query<League>(x => x.IsActive == true);

            Assert.IsNotNull(query1);
            Assert.IsInstanceOfType(query1, typeof(List<League>));
            Assert.IsTrue(query1.Any());

            // wrong value of argument
            var query2 = repo.Query<League>(x => x.LeagueOrder <= -2);

            Assert.IsNotNull(query2);
            Assert.IsInstanceOfType(query2, typeof(List<League>));
            Assert.IsFalse(query2.Any());
        }

        [TestMethod]
        public void Test_Query_Pager()
        {
            IRepository repo = new Repository();

            // normal condition
            IPager pager1 = new Pager(0) { PagingSize = 5 };

            Assert.IsFalse(pager1.TotalCount > 0);

            // ReSharper disable once RedundantBoolCompare
            var query1 = repo.Query<League>(pager1, x => x.IsActive == true, "LeagueOrder, LeagueOrgName");
            // ReSharper disable once RedundantBoolCompare
            var queryVal = repo.Query<League>(x => x.IsActive == true);

            Assert.IsNotNull(query1);
            Assert.IsInstanceOfType(query1, typeof(List<League>));
            Assert.IsTrue(query1.Any());

            Assert.IsTrue(pager1.TotalCount > 0);
            Assert.AreEqual(pager1.TotalCount.ToString(), queryVal.Count.ToString());
            Assert.AreEqual(pager1.PagingSize.ToString(), query1.Count.ToString());

            // large pagingSize
            IPager pager2 = new Pager(0) { PagingSize = 1000 };

            // ReSharper disable once RedundantBoolCompare
            var query2 = repo.Query<League>(pager2, x => x.IsActive == true, "LeagueOrder, LeagueOrgName");

            Assert.IsNotNull(query2);
            Assert.IsInstanceOfType(query2, typeof(List<League>));
            Assert.IsTrue(query2.Any());

            Assert.IsTrue(pager2.TotalCount > 0);
            Assert.AreEqual(pager2.TotalCount.ToString(), queryVal.Count.ToString());
            Assert.AreEqual(query2.Count.ToString(), queryVal.Count.ToString());

            // max currentPage
            IPager pager3 = new Pager(1000) { PagingSize = 20 };

            // ReSharper disable once RedundantBoolCompare
            var query3 = repo.Query<League>(pager3, x => x.IsActive == true, "LeagueOrder, LeagueOrgName");

            Assert.IsNotNull(query3);
            Assert.IsInstanceOfType(query3, typeof(List<League>));
            Assert.IsTrue(query3.Any());

            Assert.IsTrue(pager2.TotalCount > 0);
            Assert.AreEqual(pager2.TotalCount.ToString(), queryVal.Count.ToString());
            Assert.AreEqual(pager3.CurrentPage.ToString(), pager3.MaxPage.ToString());
            Assert.AreEqual(query3.Count.ToString(), (queryVal.Count % pager3.PagingSize).ToString());
        }

        [TestMethod]
        public void Test_Query_Criteria()
        {
            IRepository repo = new Repository();

            // normal condition
            var criteria1 = new Criteria();

            Assert.IsFalse(criteria1.TotalCount > 0);

            var query1 = repo.Query<League>(criteria1);
            var queryVal1 = repo.All<League>();

            Assert.IsNotNull(query1);
            Assert.IsInstanceOfType(query1, typeof(List<League>));
            Assert.IsTrue(query1.Any());

            Assert.IsTrue(criteria1.TotalCount > 0);
            Assert.AreEqual(criteria1.TotalCount.ToString(), queryVal1.Count.ToString());
            Assert.AreEqual(criteria1.PagingSize.ToString(), query1.Count.ToString());

            // large pagingSize
            var criteria2 = new Criteria(new { IsActive = true }, pagesize: 1000);

            var query2 = repo.Query<League>(criteria2);
            var queryVal2 = repo.Query<League>(x => x.IsActive == true);

            Assert.IsNotNull(query2);
            Assert.IsInstanceOfType(query2, typeof(List<League>));
            Assert.IsTrue(query2.Any());

            Assert.IsTrue(criteria2.TotalCount > 0);
            Assert.AreEqual(criteria2.TotalCount.ToString(), queryVal2.Count.ToString());
            Assert.AreEqual(query2.Count.ToString(), queryVal2.Count.ToString());

            // Customlize Criteria
            var criteria3 = new Criteria
            {
                WhereClause = "LeagueTime > '2014-01-01' AND LeagueOrder > 50",
                OrderClause = "LeagueName",
                Parameters = new { IsActive = true },
                PagingSize = 5
            };

            var query3 = repo.Query<League>(criteria3);
            var queryVal3 = repo.Query<League>(x => x.IsActive == true)
                .FindAll(x => x.LeagueTime > Convert.ToDateTime("2014-01-01") && x.LeagueOrder > 50);

            Assert.IsNotNull(query3);
            Assert.IsInstanceOfType(query3, typeof(List<League>));
            Assert.IsTrue(query3.Any());

            Assert.IsTrue(criteria3.TotalCount > 0);
            Assert.AreEqual(criteria3.TotalCount.ToString(), queryVal3.Count.ToString());
        }

        [TestMethod]
        public void Test_Query_Viewer()
        {
            IViewerFactory<MatchView> factory = new MatchViewFactory();

            var criteria = new Criteria
            {
                WhereClause = $"(PlayTime < '{DateTime.Now}') AND (ResultHome > 0) AND (ResultAway > 0) "
            };

            var query = factory.Query(criteria);

            Assert.IsNotNull(query);
            Assert.IsInstanceOfType(query, typeof(List<MatchView>));
            Assert.IsTrue(query.Any());

            var instance = query.First();

            Assert.IsNotNull(instance);
            Assert.IsInstanceOfType(instance, typeof(MatchView));
            Assert.IsNotNull(instance.CasinoItem);
            Assert.IsNotNull(instance.Home);
            Assert.IsNotNull(instance.Away);
            Assert.IsNotNull(instance.League);
            Assert.IsNull(instance.ChoiceOptions); // no relation choiceOptions Init
        }

        [TestMethod]
        public void Test_Query_Pager_Viewer()
        {
            var factory = new BetViewFactory();

            var criteria = new Criteria
            {
                PagingSize = 20,
                WhereClause = "(IsWin IS NOT NULL) AND (IsWin = 1)",
                OrderClause = "BetTime DESC"
            };

            Assert.IsFalse(criteria.TotalCount > 0);

            var query = factory.Query(criteria);

            Assert.IsNotNull(query);
            Assert.IsInstanceOfType(query, typeof(List<BetView>));
            Assert.IsTrue(query.Any());

            Assert.IsTrue(criteria.TotalCount > 0);
            Assert.AreEqual(criteria.PagingSize.ToString(), query.Count.ToString());
        }

        [TestMethod]
        public void Test_Query_Viewer_Many()
        {
            var factory = new BetViewFactory();

            var query = factory.Query(new Criteria(new { UserID = 443 }))
                .Many<BetView, BetDetail, int>(t => t.ID);

            Assert.IsNotNull(query);
        }

        [TestMethod]
        public void Test_Insert_Update_Delete()
        {
            var org = new League
            {
                LeagueName = "test",
                LeagueOrgName = "t",
                LeagueSeason = "2015",
                LeagueTime = DateTime.Now,
                LeagueLogo = string.Empty,
                LeagueOrder = 1000,
                IsActive = true
            };

            IRepository repo = new Repository();
            object key;

            repo.Insert(org, out key);

            Assert.IsNotNull(key);

            var res = repo.Single<League>(key);

            Assert.IsNotNull(res);
            Assert.IsInstanceOfType(res, typeof(League));

            res.IsActive = false;
            res.LeagueOrder++;

            repo.Update(res);

            var resUpdated = repo.Single<League>(key);

            Assert.IsNotNull(resUpdated);
            Assert.IsInstanceOfType(resUpdated, typeof(League));
            Assert.IsTrue(resUpdated.Equals(res));
            Assert.IsFalse(resUpdated.IsActive);

            repo.Delete(res);

            var resDeleted = repo.Single<League>(key);

            Assert.IsNull(resDeleted);

            repo.Insert(res);
            repo.Delete<League>(key);

            Assert.IsNull(repo.Single<League>(key));
        }

        [Ignore]
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Test_Insert_ArgumentNullException()
        {
            IRepository repo = new Repository();

            object key;

            repo.Insert<League>(null);
            repo.Insert<League>(null, out key);
        }

        [Ignore]
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Test_Update_ArgumentNullException()
        {
            IRepository repo = new Repository();

            repo.Update<League>(null);
        }

        [Ignore]
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Test_Update_ArgumentException()
        {
            IRepository repo = new Repository();

            var l = new League
            {
                LeagueName = "test",
                LeagueOrgName = "t",
                LeagueSeason = "2015",
                LeagueTime = DateTime.Now,
                LeagueLogo = string.Empty,
                LeagueOrder = 1000,
                IsActive = true
            };

            repo.Update(l);
        }

        [Ignore]
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Test_Delete_ArgumentException()
        {
            IRepository repo = new Repository();

            var l = new League
            {
                LeagueName = "test",
                LeagueOrgName = "t",
                LeagueSeason = "2015",
                LeagueTime = DateTime.Now,
                LeagueLogo = string.Empty,
                LeagueOrder = 1000,
                IsActive = true
            };

            repo.Delete(l);
        }
    }
}