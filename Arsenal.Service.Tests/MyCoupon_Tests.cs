using System.Collections.Generic;
using System.Linq;
using System.Web.Script.Serialization;
using Arsenal.Mobile.Models.Casino;
using Arsenal.Service.Casino;
using Arsenalcn.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Arsenal.Mobile.Tests
{
    [TestClass]
    public class MyCouponTests
    {
        [TestMethod]
        public void MyCouponJsonSerializeAndDeserialize_Test()
        {
            var factory = new CouponViewFactory();

            var coupons = factory.Query(new Criteria(new { UserID = 443 }));

            if (coupons != null && coupons.Count > 0)
            {
                var couponDtos = coupons.MapToList<CouponView, CouponDto>().ToList();

                var jsonSerializer = new JavaScriptSerializer();

                var json = jsonSerializer.Serialize(couponDtos);

                if (!string.IsNullOrEmpty(json))
                {
                    couponDtos = jsonSerializer.Deserialize<List<CouponDto>>(json);

                    Assert.IsInstanceOfType(couponDtos[0], typeof(CouponDto));
                }

                var myCouponDto = new MyCouponDto
                {
                    Coupons = couponDtos
                };

                json = jsonSerializer.Serialize(myCouponDto);

                Assert.IsTrue(!string.IsNullOrEmpty(json));
            }
        }
    }
}