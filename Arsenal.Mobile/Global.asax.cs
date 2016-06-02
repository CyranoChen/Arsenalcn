using System;
using System.Reflection;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Arsenal.Service;
using Arsenalcn.Core.Logger;
using Arsenalcn.Core.Scheduler;

namespace Arsenal.Mobile
{
    public class MvcApplication : HttpApplication
    {
        private static Timer _eventTimer;

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            if (_eventTimer == null && ConfigGlobal.SchedulerActive)
            {
                _eventTimer = new Timer(SchedulerCallback, null, 60 * 1000, ScheduleManager.TimerMinutesInterval * 60 * 1000);
            }
        }

        private void SchedulerCallback(object sender)
        {
            try
            {
                if (ConfigGlobal.SchedulerActive)
                {
                    var declaringType = MethodBase.GetCurrentMethod().DeclaringType;

                    if (declaringType != null)
                    {
                        var assembly = declaringType.Assembly.GetName().Name;

                        ScheduleManager.Execute(assembly);
                    }
                }
            }
            catch (Exception ex)
            {
                ILog log = new AppLog();

                log.Warn(ex, new LogInfo
                {
                    MethodInstance = MethodBase.GetCurrentMethod(),
                    ThreadInstance = Thread.CurrentThread
                });
            }
        }
    }
}