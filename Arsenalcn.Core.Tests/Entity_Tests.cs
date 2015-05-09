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
        public void Single_Test()
        {
            League l = new League();

            var guid = "fd32f77d-47a7-4d5f-b7ce-068e3e1a0833";

            var instance = l.Single<League>(guid);

            Console.Write(instance.LeagueNameInfo.ToString());

            Assert.IsNotNull(instance);
        }
    }
}
