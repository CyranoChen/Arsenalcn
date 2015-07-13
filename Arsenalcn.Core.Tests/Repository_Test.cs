using System;
using System.Collections;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Arsenalcn.Core.Tests
{
    [TestClass()]
    public class Repository_Test
    {
        [TestMethod()]
        public void Single_Test()
        {
            IRepository repo = new Repository();

            var key = new Guid("066edf53-f823-4020-b740-f4c9fff98ec8");

            var instance = repo.Single<League>(key);

            Assert.IsNotNull(instance);
        }

        [TestMethod()]
        public void All_Test()
        {
            IRepository repo = new Repository();

            var query = repo.All<League>();

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
