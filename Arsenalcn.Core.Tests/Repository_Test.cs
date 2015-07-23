using System;
using System.Collections;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Arsenal.Service.Casino;
using Arsenal.Service;

namespace Arsenalcn.Core.Tests
{
    [TestClass()]
    public class Repository_Test
    {
        [TestMethod()]
        public void Single_Test()
        {
            IRepository repo = new Repository();

            var key = new Guid("12236a72-f35b-4a0f-90e6-67b11c3364bc");

            var instance = repo.Single<Arsenal.Service.Casino.Match>(key);

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

            instance.Many<ChoiceOption>(instance.CasinoItem.ID);

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

            var ht = new Hashtable();

            ht.Add("LeagueTime", "DESC");

            var query = repo.All<League>(new Pager(2) { PagingSize = 20 }, ht);

            Assert.IsNotNull(query);
        }

        [TestMethod()]
        public void AllByPager_Viewer_Test()
        {
            IRepository repo = new Repository();

            var ht = new Hashtable();

            ht.Add("BetTime", "DESC");

            var query = repo.All<BetView>(new Pager(2) { PagingSize = 20 }, ht);

            Assert.IsNotNull(query);
        }

        [TestMethod()]
        public void Query_Test()
        {
            IRepository repo = new Repository();

            var query1 = repo.Query<League>(x => x.IsActive);

            var ht = new Hashtable();

            ht.Add("IsActive", true);

            var query2 = repo.Query<League>(ht);

            Assert.IsNotNull(query1);
            Assert.IsNotNull(query2);
        }

        [TestMethod()]
        public void QueryByPager_Test()
        {
            IRepository repo = new Repository();

            var orderBy = new Hashtable();

            orderBy.Add("LeagueTime", "DESC");

            var whereBy = new Hashtable();

            whereBy.Add("IsActive", true);

            var query = repo.Query<League>(new Pager(2) { PagingSize = 5 }, whereBy, orderBy);

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
