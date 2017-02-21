using System;
using System.Configuration;
using System.Web;
using System.Web.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace iArsenal.Tests
{
    [TestClass]
    public class CommonTest
    {
        [TestMethod]
        public void Test_GetWebConfigSection()
        {
            var compilationSection = (CompilationSection)ConfigurationManager.GetSection(@"system.web/compilation");

            Assert.IsFalse(compilationSection != null && compilationSection.Debug);
        }
    }
}
