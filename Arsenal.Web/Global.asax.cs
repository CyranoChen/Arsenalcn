using System;
using System.Threading;

using Arsenal.Service;
using Arsenalcn.Core.Scheduler;

namespace Arsenal.Web
{
    public class Global : System.Web.HttpApplication
    {
        static Timer eventTimer;
        protected void Application_Start(object sender, EventArgs e)
        {
            //if (eventTimer == null && ScheduleConfigs.GetConfig().Enabled)
            if (eventTimer == null && ConfigGlobal.SchedulerActive)
            {
                //EventLogs.LogFileName = Utils.GetMapPath(string.Format("{0}cache/scheduleeventfaildlog.config", BaseConfigs.GetForumPath));
                //EventManager.RootPath = Utils.GetMapPath(BaseConfigs.GetForumPath);
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
            catch
            {
                //EventLogs.WriteFailedLog("Failed ScheduledEventCallBack");
            }
        }
    }
}