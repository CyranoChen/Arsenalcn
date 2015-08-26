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
            var sql = "INSERT XXX INTO XXX";

            ILog log1 = new DaoLog();

            log1.Debug(sql, new LogInfo() { MethodInstance = MethodBase.GetCurrentMethod(), ThreadInstance = Thread.CurrentThread });

            //ILog log2 = new UserLog();

            //log2.Debug(sql, new LogInfo() { UserClient = new UserClientInfo { UserIP = IPLocation.GetIP(), UserID = 0, UserBrowser = BrowserInfo.GetBrowser() } });

            Assert.IsTrue(true);
        }

        [TestMethod()]
        public void LoggerException_Test()
        {
            try
            {
                var _str = "0000-0000";
                var g = new Guid(_str);
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
