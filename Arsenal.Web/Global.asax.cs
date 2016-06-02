using System;
using System.Reflection;
using System.Threading;
using System.Web;
using Arsenal.Service;
using Arsenalcn.Core.Logger;
using Arsenalcn.Core.Scheduler;

namespace Arsenal.Web
{
    public class Global : HttpApplication
    {
        private static Timer _eventTimer;

        protected void Application_Start(object sender, EventArgs e)
        {
            if (_eventTimer == null && ConfigGlobal_Arsenal.SchedulerActive)
            {
                _eventTimer = new Timer(SchedulerCallback, null, 60 * 1000, ScheduleManager.TimerMinutesInterval * 60 * 1000);
            }
        }

        private void SchedulerCallback(object sender)
        {
            try
            {
                if (ConfigGlobal_Arsenal.SchedulerActive)
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