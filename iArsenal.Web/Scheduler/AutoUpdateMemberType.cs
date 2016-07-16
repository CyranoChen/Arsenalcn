using System;
using System.Reflection;
using System.Threading;
using Arsenalcn.Core;
using Arsenalcn.Core.Logger;
using Arsenalcn.Core.Scheduler;
using iArsenal.Service;

namespace iArsenal.Scheduler
{
    internal class AutoUpdateMemberType : ISchedule
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
                _log.Info("Scheduler Start: (AutoUpdateMemberType)", logInfo);

                IRepository repo = new Repository();

                var mlist = repo.All<Member>().FindAll(x => (int)x.MemberType <= 2);
                var olist = repo.All<Order>().FindAll(x => x.IsActive);

                if (mlist.Count > 0 && olist.Count > 0)
                {
                    // Don't place LINQ to Foreach, first ToList(), then use list.FindAll to improve performance
                    foreach (var m in mlist)
                    {
                        MemberType type;
                        var list = olist.FindAll(x => x.MemberID == m.ID);

                        // Refresh the MemberType of instance
                        if (list.Count > 0)
                        {
                            if (list.Exists(x => x.OrderType == OrderBaseType.Ticket || x.OrderType == OrderBaseType.Travel))
                            {
                                type = MemberType.Match;
                            }
                            else if (list.Exists(x => x.OrderType == OrderBaseType.ReplicaKit ||
                                x.OrderType == OrderBaseType.Printing || x.OrderType == OrderBaseType.Wish))
                            {
                                type = MemberType.Buyer;
                            }
                            else
                            {
                                type = MemberType.None;
                            }
                        }
                        else
                        {
                            type = MemberType.None;
                        }

                        if (!m.MemberType.Equals(type))
                        {
                            m.MemberType = type;

                            repo.Update(m);
                        }
                    }
                }

                _log.Info("Scheduler End: (AutoUpdateMemberType)", logInfo);
            }
            catch (Exception ex)
            {
                _log.Warn(ex, logInfo);
            }
        }
    }
}