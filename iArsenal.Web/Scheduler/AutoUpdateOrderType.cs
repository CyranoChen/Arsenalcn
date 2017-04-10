using System;
using System.Reflection;
using System.Threading;
using Arsenalcn.Core;
using Arsenalcn.Core.Dapper;
using Arsenalcn.Core.Logger;
using Arsenalcn.Core.Scheduler;
using iArsenal.Service;

namespace iArsenal.Scheduler
{
    internal class AutoUpdateOrderType : ISchedule
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
                _log.Info("Scheduler Start: (AutoUpdateOrderType)", logInfo);

                IRepository repo = new Repository();

                var oList = repo.All<Order>();
                var oiList = repo.All<OrderItem>().FindAll(x => Product.Cache.Load(x.ProductGuid) != null);

                if (oList.Count > 0 && oiList.Count > 0)
                {
                    // Don't place LINQ to Foreach, first ToList(), then use list.FindAll to improve performance
                    foreach (var o in oList)
                    {
                        var list = oiList.FindAll(x => x.OrderID.Equals(o.ID));

                        // Refresh the OrderType of instance
                        if (list.Count > 0)
                        {
                            var type = Order.GetOrderTypeByOrderItems(list);

                            if (!o.OrderType.Equals(type))
                            {
                                o.OrderType = type;

                                repo.Update(o);
                            }
                        }
                    }
                }

                _log.Info("Scheduler End: (AutoUpdateOrderType)", logInfo);
            }
            catch (Exception ex)
            {
                _log.Warn(ex, logInfo);
            }
        }
    }
}