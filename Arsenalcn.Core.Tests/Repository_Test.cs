using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Arsenalcn.Core;

namespace Arsenalcn.Core.Tests
{
    [TestClass()]
    public class Repository_Test
    {
        [TestMethod()]
        public void Single_Test()
        {
            IRepository repo = new Repository();

            var instance = repo.Single<League>(new Guid("066edf53-f823-4020-b740-f4c9fff98ec8"));

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
        public void Query_Test()
        {
            IRepository repo = new Repository();

            var query = repo.Query<League>(x => x.IsActive);

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
