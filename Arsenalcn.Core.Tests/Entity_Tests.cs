using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Arsenalcn.Core;

namespace Arsenalcn.Core.Tests
{
    [TestClass()]
    public class Entity_Tests
    {
        [TestMethod()]
        public void All_Test()
        {
            IEntity instance = new League();

            var query = instance.All<League>();

            Assert.IsNotNull(query);
        }

        [TestMethod()]
        public void Query_Test()
        {
            IEntity instance = new League();

            var query = instance.Query<League>(x => x.IsActive);

            Assert.IsNotNull(query);
        }

        [TestMethod()]
        public void Crud_Test()
        {
            League l = new League();

            l.LeagueGuid = Guid.NewGuid();
            l.LeagueName = "test";
            l.LeagueOrgName = "t";
            l.LeagueSeason = "2015";
            l.LeagueTime = DateTime.Now;
            l.LeagueLogo = string.Empty;
            l.LeagueOrder = 1000;
            l.IsActive = true;

            IEntity instance = new League();

            instance.Create<League>(l);

            l.IsActive = false;

            instance.Update<League>(l);

            l = instance.Single<League>(l.LeagueGuid);

            instance.Delete<League>(l);

            Assert.IsNotNull(l);
        }

    }
}
