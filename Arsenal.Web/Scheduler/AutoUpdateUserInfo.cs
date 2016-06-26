using System;
using System.Reflection;
using System.Threading;
using Arsenal.Service;
using Arsenalcn.Core;
using Arsenalcn.Core.Logger;
using Arsenalcn.Core.Scheduler;

namespace Arsenal.Scheduler
{
    internal class AutoUpdateUserInfo : ISchedule
    {
        private readonly ILog _log = new AppLog();

        public void Execute(object state)
        {
            var logInfo = new LogInfo
            {
                MethodInstance = MethodBase.GetCurrentMethod(),
                ThreadInstance = Thread.CurrentThread
            };

            try
            {
                _log.Info("Scheduler Start: (AutoUpdateUserInfo)", logInfo);

                IRepository repo = new Repository();

                // 同步所有未取得实名信息的用户资料
                var users = repo.Query<User>(x => x.AcnID.HasValue).FindAll(x => !x.MemberID.HasValue);

                if (users.Count > 0)
                {
                    foreach (var u in users)
                    {
                        try
                        {
                            u.SyncUserByMember();
                        }
                        catch
                        {
                            // ignored
                        }
                    }
                }

                _log.Info("Scheduler End: (AutoUpdateUserInfo)", logInfo);
            }
            catch (Exception ex)
            {
                _log.Warn(ex, logInfo);
            }
        }
    }
}