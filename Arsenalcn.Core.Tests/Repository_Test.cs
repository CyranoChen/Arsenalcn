using System;
using System.Collections;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Arsenal.Service.Casino;
using Arsenal.Service;
using System.Linq.Expressions;

namespace Arsenalcn.Core.Tests
{
    [TestClass()]
    public class Repository_Test
    {
        [TestMethod()]
        public void Single_Test()
        {
            IRepository repo = new Repository();

            var key = new Guid("FD32F77D-47A7-4D5F-B7CE-068E3E1A0833");

            var instance = repo.Single<League>(key);

            Assert.IsNotNull(instance);
        }

        [TestMethod()]
        public void Single_Viewer_Test()
        {
            IRepository repo = new Repository();

            var key = new Guid("12236a72-f35b-4a0f-90e6-67b11c3364bc");

            var instance = repo.Single<MatchView>(key);

            Assert.IsNotNull(instance);
        }

        [TestMethod()]
        public void Single_Viewer_Many_Test()
        {
            IRepository repo = new Repository();

            var key = new Guid("12236a72-f35b-4a0f-90e6-67b11c3364bc");

            var instance = repo.Single<MatchView>(key);

            instance.Many<ChoiceOption>(x => x.CasinoItemGuid == instance.CasinoItem.ID);

            Assert.IsNotNull(instance);
        }

        [TestMethod()]
        public void All_Test()
        {
            IRepository repo = new Repository();

            var query = repo.All<MatchView>();

            Assert.IsNotNull(query);
        }

        [TestMethod()]
        public void All_Viewer_Test()
        {
            IRepository repo = new Repository();

            var query = repo.All<MatchView>();

            Assert.IsNotNull(query);
        }

        [TestMethod()]
        public void All_Viewer_Many_Test()
        {
            IRepository repo = new Repository();

            var query = repo.All<MatchView>().Take(10);

            query.Many<MatchView, ChoiceOption>(
                (tSource, t) => tSource.CasinoItem.ID.Equals(t.CasinoItemGuid));

            Assert.IsNotNull(query);
        }

        [TestMethod()]
        public void AllByPager_Test()
        {
            IRepository repo = new Repository();

            var query = repo.All<League>(new Pager(2) { PagingSize = 20 }, "LeagueTime DESC");

            Assert.IsNotNull(query);
        }

        [TestMethod()]
        public void AllByPager_Viewer_Test()
        {
            IRepository repo = new Repository();

            var query = repo.All<BetView>(new Pager(2) { PagingSize = 20 }, "BetTime DESC");

            Assert.IsNotNull(query);
        }

        [TestMethod()]
        public void Query_Test()
        {
            IRepository repo = new Repository();

            var query = repo.Query<League>(x => x.IsActive == true);

            Assert.IsNotNull(query);
        }

        [TestMethod]
        public void Query_Viewer_Many_Test()
        {
            IRepository repo = new Repository();

            var query = repo.Query<BetView>(x => x.UserID == 443)
                .Many<BetView, BetDetail>((tOne, tMany) => tOne.ID.Equals(tMany.BetID));

            Assert.IsNotNull(query);
        }

        [TestMethod()]
        public void QueryByPager_Test()
        {
            IRepository repo = new Repository();

            var orderBy = new Hashtable();

            orderBy.Add("LeagueTime", "DESC");

            var whereBy = new Hashtable();

            whereBy.Add("IsActive", true);

            var query = repo.Query<League>(new Pager(2) { PagingSize = 5 },
                x => x.IsActive == true, "LeagueTime DESC");

            Assert.IsNotNull(query);
        }

        [TestMethod]
        public void Query_Condition_Test()
        {
            IRepository repo = new Repository();

            var query = repo.Query<MatchView>(x => x.ResultHome.HasValue && x.ResultAway.HasValue &&
                x.ResultHome >= 0 && x.ResultAway >= 0 && x.PlayTime < DateTime.Now);

            Assert.IsNotNull(query);
        }

        [TestMethod()]
        public void Crud_Test()
        {
            League l = new League();

            l.LeagueName = "test";
            l.LeagueOrgName = "t";
            l.LeagueSeason = "2015";
            l.LeagueTime = DateTime.Now;
            l.LeagueLogo = string.Empty;
            l.LeagueOrder = 1000;
            l.IsActive = true;

            IRepository repo = new Repository();

            repo.Insert(l);

            l.IsActive = false;

            repo.Update(l);

            repo.Delete<League>(l.ID);

            repo.Insert(l);

            repo.Delete(l);

            Assert.IsNotNull(l);
        }
    }
}
