using System;
using System.IO;
using System.Reflection;
using System.Threading;
using Arsenalcn.Core;
using Arsenalcn.Core.Logger;
using Arsenalcn.Core.Scheduler;
using iArsenal.Service;

namespace iArsenal.Scheduler
{
    internal class RefreshCache : ISchedule
    {
        private readonly ILog _log = new AppLog();

        public void Execute(object state)
        {
            var logInfo = new LogInfo
            {
                MethodInstance = MethodBase.GetCurrentMethod(),
                ThreadInstance = Thread.CurrentThread
            };

            //string _scheduleType = this.GetType().DeclaringType.FullName;

            try
            {
                _log.Info("Scheduler Start: (RefreshCache)", logInfo);

                Config.UpdateAssemblyInfo(Assembly.GetExecutingAssembly(), ConfigSystem.iArsenal);

                ConfigGlobal.Refresh();

                Arsenal_Match.Cache.RefreshCache();
                Arsenal_Player.Cache.RefreshCache();
                Arsenal_Team.Cache.RefreshCache();

                MatchTicket.Cache.RefreshCache();
                Member.Cache.RefreshCache();
                Product.Cache.RefreshCache();

                // Clean Log
                Log.Clean();

                // Clean QrCode Files
                CleanQrCodeFiles();

                _log.Info("Scheduler End: (RefreshCache)", logInfo);
            }
            catch (Exception ex)
            {
                _log.Warn(ex, logInfo);
            }
        }

        private static void CleanQrCodeFiles()
        {
            // TODO
            const string fileUrl = "C:\\websoft\\wwwroot\\www.iarsenal.com\\UploadFiles\\QrCode";

            // 判断文件夹是否存在，存在就删除目录与文件
            if (Directory.Exists(fileUrl))
            {
                try
                {
                    Directory.Delete(fileUrl, true);
                }
                catch
                {
                    // ignored
                }
            }
        }
    }
}