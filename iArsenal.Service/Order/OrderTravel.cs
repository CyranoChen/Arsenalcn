using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Arsenalcn.Core;
using AutoMapper;

namespace iArsenal.Service
{
    public class OrdrTravel : Order
    {
        public void Init(IDbTransaction trans)
        {
            IRepository repo = new Repository();

            var list = repo.Query<OrderItem>(x => x.OrderID == ID, trans)
                .FindAll(x => x.IsActive && Product.Cache.Load(x.ProductGuid) != null);

            if (list.Any())
            {
                var oiBase = list.Find(x => Product.Cache.Load(x.ProductGuid).ProductType.Equals(ProductType.TravelPlan));
                if (oiBase != null)
                {
                    var mapperTravelPlan = new MapperConfiguration(cfg =>
                        cfg.CreateMap<OrderItem, OrdrItmTravelPlan>().AfterMap((s, d) => d.Init()))
                        .CreateMapper();

                    OITravelPlan = mapperTravelPlan.Map<OrdrItmTravelPlan>(oiBase);
                }

                if (OITravelPlan != null)
                {
                    var oiPartnerList =
                        list.FindAll(
                            x => Product.Cache.Load(x.ProductGuid).ProductType.Equals(ProductType.TravelPartner));

                    var mapperTravelPartner = new MapperConfiguration(cfg =>
                        cfg.CreateMap<OrderItem, OrdrItmTravelPartner>().AfterMap((s, d) => d.Init()))
                        .CreateMapper();

                    OITravelPartnerList = mapperTravelPartner.Map<List<OrdrItmTravelPartner>>(oiPartnerList);

                    #region Generate UrlOrderView by Product Code

                    var p = Product.Cache.Load(OITravelPlan.ProductGuid);

                    if (p != null && p.ProductType.Equals(ProductType.TravelPlan))
                    {
                        if (p.Code.Equals("iETPL", StringComparison.OrdinalIgnoreCase))
                        {
                            UrlOrderView = "iArsenalOrderView_LondonTravel.aspx";
                        }
                        else if (p.Code.Equals("2015ATPL", StringComparison.OrdinalIgnoreCase))
                        {
                            UrlOrderView = "iArsenalOrderView_AsiaTrophy2015.aspx";
                        }
                        else
                        {
                            throw new Exception("Unable to init Order_Travel.");
                        }
                    }
                    else
                    {
                        throw new Exception("Unable to init Order_Travel.");
                    }

                    #endregion
                }
                else
                {
                    throw new Exception("Unable to init Order_Travel.");
                }
            }

            #region Order Status Workflow Info

            var strWorkflow = "{{ \"StatusType\": \"{0}\", \"StatusInfo\": \"{1}\" }}";

            string[] workflowInfo =
            {
                string.Format(strWorkflow, ((int) OrderStatusType.Draft), "未提交"),
                string.Format(strWorkflow, ((int) OrderStatusType.Submitted), "审核中"),
                string.Format(strWorkflow, ((int) OrderStatusType.Confirmed), "已确认"),
                string.Format(strWorkflow, ((int) OrderStatusType.Delivered), "已完成")
            };

            StatusWorkflowInfo = workflowInfo;

            #endregion
        }

        #region Members and Properties

        public OrdrItmTravelPlan OITravelPlan { get; set; }

        public List<OrdrItmTravelPartner> OITravelPartnerList { get; set; }

        #endregion
    }
}