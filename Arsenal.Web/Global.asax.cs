using System;
using System.Threading;

using Arsenalcn.Core.Scheduler;

namespace Arsenal.Web
{
    public class Global : System.Web.HttpApplication
    {
        static Timer eventTimer;
        protected void Application_Start(object sender, EventArgs e)
        {
            //if (eventTimer == null && ScheduleConfigs.GetConfig().Enabled)
            if (eventTimer == null)
            {
                //EventLogs.LogFileName = Utils.GetMapPath(string.Format("{0}cache/scheduleeventfaildlog.config", BaseConfigs.GetForumPath));
                //EventManager.RootPath = Utils.GetMapPath(BaseConfigs.GetForumPath);
                eventTimer = new Timer(new TimerCallback(SchedulerCallback), null, 60000, ScheduleManager.TimerMinutesInterval * 60000);
            }
        }

        private void SchedulerCallback(object sender)
        {
            try
            {
                //if (ScheduleConfigs.GetConfig().Enabled)
                //{
                //    EventManager.Execute();
                //}

                ScheduleManager.Execute();
            }
            catch
            {
                //EventLogs.WriteFailedLog("Failed ScheduledEventCallBack");
            }
        }
    }
}