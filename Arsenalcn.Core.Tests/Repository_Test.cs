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
        public void Select_Test()
        {
            IRepository repo = new Repository();

            DataTable dt = repo.Select<League>();

            Console.WriteLine(dt.Rows.Count.ToString());

            Assert.IsNotNull(dt);
        }

        [TestMethod()]
        public void Repo_Test()
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

            IRepository repo = new Repository();

            repo.Insert<League>(l);

            l.IsActive = false;

            repo.Update<League>(l);

            DataRow dr = repo.Select<League>(l.LeagueGuid);

            repo.Delete<League>(l.LeagueGuid);

            Assert.IsNotNull(dr);
        }
    }
}
