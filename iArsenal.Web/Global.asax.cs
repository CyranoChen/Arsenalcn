using System;
using System.Reflection;
using System.Threading;
using System.Web;
using Arsenalcn.Core.Logger;
using Arsenalcn.Core.Scheduler;
using iArsenal.Service;

namespace iArsenal.Web
{
    public class Global : HttpApplication
    {
        private static Timer eventTimer;

        protected void Application_Start(object sender, EventArgs e)
        {
            if (eventTimer == null && ConfigGlobal.SchedulerActive)
            {
                eventTimer = new Timer(SchedulerCallback, null, 60*1000, ScheduleManager.TimerMinutesInterval*60*1000);
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

                log.Warn(ex, new LogInfo
                {
                    MethodInstance = MethodBase.GetCurrentMethod(),
                    ThreadInstance = Thread.CurrentThread
                });
            }
        }
    }
}