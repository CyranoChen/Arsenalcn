using System;
using Arsenalcn.Core;
using Arsenalcn.Core.Dapper;
using iArsenal.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace iArsenal.Tests.Scheduler
{
    [TestClass]
    public class AutoUpdateOrderTypeTest
    {
        [TestMethod]
        public void AutoUpdateOrderType_Test()
        {
            try
            {
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
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }
    }
}
