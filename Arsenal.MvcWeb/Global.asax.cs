using System;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Reflection;
using System.Threading;

using Arsenalcn.Core.Logger;
using Arsenalcn.Core.Scheduler;
using Arsenal.Service;

namespace Arsenal.MvcWeb
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        static Timer eventTimer;
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            if (eventTimer == null && ConfigGlobal.SchedulerActive)
            {
                eventTimer = new Timer(new TimerCallback(SchedulerCallback), null, 60 * 1000, ScheduleManager.TimerMinutesInterval * 60 * 1000);
            }
        }

        private void SchedulerCallback(object sender)
        {
            try
            {
                if (ConfigGlobal.SchedulerActive)
                {
                    ScheduleManager.Execute();
                }
            }
            catch (Exception ex)
            {
                ILog log = new AppLog();

                log.Warn(ex, new LogInfo()
                {
                    MethodInstance = MethodBase.GetCurrentMethod(),
                    ThreadInstance = Thread.CurrentThread
                });
            }
        }

    }
}