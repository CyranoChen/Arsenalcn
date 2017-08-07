using System;
using Arsenalcn.Core.Dapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Arsenal.Service.Tests
{
    [TestClass]
    public class ArsenalPlayerTest
    {
        [TestMethod]
        public void ArsenalPlayer_Test()
        {
            using (IRepository repo = new Repository())
            {
                var player = repo.Single<Player>(new Guid("4a43c418-9140-4c1b-a04f-d3789336d690"));

                Assert.IsNotNull(player);
                Assert.AreEqual("Defender", player.PlayerPosition.ToString());
            }
        }
    }
}

