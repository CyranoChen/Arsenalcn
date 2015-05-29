using System;
using System.Reflection;
using System.Threading;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Arsenalcn.Core.Logger;

namespace Arsenalcn.Core.Tests
{
    [TestClass()]
    public class Logger_Tests
    {
        [TestMethod()]
        public void Logger_Test()
        {
            string sql = "INSERT XXX INTO XXX";

            ILog log = new DaoLog();

            log.Debug(sql, new LogInfo() { MethodInstance = MethodBase.GetCurrentMethod(), ThreadInstance = Thread.CurrentThread });

            Assert.IsTrue(true);
        }

        [TestMethod()]
        public void LoggerException_Test()
        {
            try
            {
                string _str = "0000-0000";
                Guid g = new Guid(_str);
            }
            catch (Exception ex)
            {
                ILog log = new DaoLog();

                log.Debug(ex, new LogInfo() { MethodInstance = MethodBase.GetCurrentMethod(), ThreadInstance = Thread.CurrentThread });
            }

            Assert.IsTrue(true);
        }
    }
}
