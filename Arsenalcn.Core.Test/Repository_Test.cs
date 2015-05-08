using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Arsenalcn.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Arsenalcn.Core.Test
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
        public void Insert_Test()
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

            DataRow dr = repo.Select<League>(l.LeagueGuid);

            Assert.IsNotNull(dr);
        }
    }

    [AttrDbTable("Arsenal_League", Key = "LeagueGuid")]
    public class League
    {
        public League() { }

        #region Members and Properties

        [AttrDbColumn("LeagueGuid", IsKey = true)]
        public Guid LeagueGuid
        { get; set; }

        [AttrDbColumn("LeagueName")]
        public string LeagueName
        { get; set; }

        [AttrDbColumn("LeagueOrgName")]
        public string LeagueOrgName
        { get; set; }

        [AttrDbColumn("LeagueSeason")]
        public string LeagueSeason
        { get; set; }

        [AttrDbColumn("LeagueTime")]
        public DateTime LeagueTime
        { get; set; }

        [AttrDbColumn("LeagueLogo")]
        public string LeagueLogo
        { get; set; }

        [AttrDbColumn("LeagueOrder")]
        public int LeagueOrder
        { get; set; }

        [AttrDbColumn("IsActive")]
        public bool IsActive
        { get; set; }

        #endregion
    }
}
