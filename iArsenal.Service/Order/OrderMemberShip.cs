using System;
using System.Linq;
using Arsenalcn.Core;
using AutoMapper;

namespace iArsenal.Service
{
    public class OrdrMembership : Order
    {
        public void Init()
        {
            IRepository repo = new Repository();

            var list = repo.Query<OrderItem>(x => x.OrderID == ID)
                .FindAll(x => x.IsActive && Product.Cache.Load(x.ProductGuid) != null);

            if (list.Any())
            {
                var oiBase = list.Find(x => Product.Cache.Load(x.ProductGuid).ProductType.Equals(ProductType.MembershipCore));
                if (oiBase != null)
                {
                    var mapperMemShipCore = new MapperConfiguration(cfg =>
                        cfg.CreateMap<OrderItem, OrdrItmMemShipCore>().AfterMap((s, d) => d.Init()))
                        .CreateMapper();

                    OIMembershipCore = mapperMemShipCore.Map<OrdrItmMemShipCore>(oiBase);
                }

                oiBase =
                    list.Find(x => Product.Cache.Load(x.ProductGuid).ProductType.Equals(ProductType.MembershipPremier));
                if (oiBase != null)
                {
                    var mapperMemShipPremier = new MapperConfiguration(cfg =>
                        cfg.CreateMap<OrderItem, OrdrItmMemShipPremier>().AfterMap((s, d) => d.Init()))
                        .CreateMapper();

                    OIMembershipPremier = mapperMemShipPremier.Map<OrdrItmMemShipPremier>(oiBase);
                }

                if (OIMembershipCore != null || OIMembershipPremier != null)
                {
                    UrlOrderView = "iArsenalOrderView_Membership.aspx";
                }
                else
                {
                    throw new Exception("Unable to init Order_Membership.");
                }
            }

            #region Order Status Workflow Info

            var strWorkflow = "{{ \"StatusType\": \"{0}\", \"StatusInfo\": \"{1}\" }}";

            string[] workflowInfo =
            {
                string.Format(strWorkflow, ((int) OrderStatusType.Draft), "未提交"),
                string.Format(strWorkflow, ((int) OrderStatusType.Submitted), "审核中"),
                string.Format(strWorkflow, ((int) OrderStatusType.Confirmed), "已确认")
            };

            StatusWorkflowInfo = workflowInfo;

            #endregion
        }

        #region Members and Properties

        public OrdrItmMemShipCore OIMembershipCore { get; set; }

        public OrdrItmMemShipPremier OIMembershipPremier { get; set; }

        #endregion
    }
}