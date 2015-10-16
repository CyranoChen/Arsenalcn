using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

using Arsenalcn.Core.Logger;
using Arsenalcn.Core.Scheduler;
using Arsenal.Service;

namespace Arsenal.Mobile
{
    public class MvcApplication : System.Web.HttpApplication
    {
        static Timer _eventTimer;
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            if (_eventTimer == null && ConfigGlobal.SchedulerActive)
            {
                _eventTimer = new Timer(new TimerCallback(SchedulerCallback), null, 60 * 1000, ScheduleManager.TimerMinutesInterval * 60 * 1000);
            }
        }

        private void SchedulerCallback(object sender)
        {
            try
            {
                if (ConfigGlobal.SchedulerActive)
                {
                    var assembly = MethodBase.GetCurrentMethod().DeclaringType.Assembly.GetName().Name;

                    ScheduleManager.Execute(assembly);
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
