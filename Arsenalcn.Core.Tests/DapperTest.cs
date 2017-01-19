using System;
using Arsenal.Service;
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
    }
}