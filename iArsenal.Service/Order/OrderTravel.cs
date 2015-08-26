using System;
using System.Collections.Generic;
using Arsenalcn.Core;

namespace iArsenal.Service
{
    public class OrdrTravel : Order
    {
        public OrdrTravel() { }

        public void Init()
        {
            IRepository repo = new Repository();

            var list = repo.Query<OrderItem>(x => x.OrderID == ID && x.IsActive == true)
                .FindAll(x => Product.Cache.Load(x.ProductGuid) != null);

            if (list != null && list.Count > 0)
            {
                OrderItem oiBase = null;

                oiBase = list.Find(x => Product.Cache.Load(x.ProductGuid).ProductType.Equals(ProductType.TravelPlan));
                if (oiBase != null)
                {
                    AutoMapper.Mapper.CreateMap<OrderItem, OrdrItmTravelPlan>().AfterMap((s, d) => d.Init());
                    OITravelPlan = AutoMapper.Mapper.Map<OrdrItmTravelPlan>(oiBase);
                }

                if (OITravelPlan != null)
                {
                    var oiPartnerList = list.FindAll(x => Product.Cache.Load(x.ProductGuid).ProductType.Equals(ProductType.TravelPartner));
                    AutoMapper.Mapper.CreateMap<OrderItem, OrdrItmTravelPartner>().AfterMap((s, d) => d.Init());
                    OITravelPartnerList = AutoMapper.Mapper.Map<List<OrdrItmTravelPartner>>(oiPartnerList);

                    #region Generate UrlOrderView by Product Code
                    var p = Product.Cache.Load(OITravelPlan.ProductGuid);

                    if (p != null && p.ProductType.Equals(ProductType.TravelPlan))
                    {
                        if (p.Code.Equals("iETPL", StringComparison.OrdinalIgnoreCase))
                        {
                            base.UrlOrderView = "iArsenalOrderView_LondonTravel.aspx";
                        }
                        else if (p.Code.Equals("2015ATPL", StringComparison.OrdinalIgnoreCase))
                        {
                            base.UrlOrderView = "iArsenalOrderView_AsiaTrophy2015.aspx";
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

            var _strWorkflow = "{{ \"StatusType\": \"{0}\", \"StatusInfo\": \"{1}\" }}";

            string[] _workflowInfo = {
                                      string.Format(_strWorkflow, ((int)OrderStatusType.Draft).ToString(), "未提交"),
                                      string.Format(_strWorkflow, ((int)OrderStatusType.Submitted).ToString(), "审核中"),
                                      string.Format(_strWorkflow, ((int)OrderStatusType.Confirmed).ToString(), "已确认"),
                                      string.Format(_strWorkflow, ((int)OrderStatusType.Delivered).ToString(), "已完成")
                                  };

            base.StatusWorkflowInfo = _workflowInfo;

            #endregion

        }

        #region Members and Properties

        public OrdrItmTravelPlan OITravelPlan { get; set; }

        public List<OrdrItmTravelPartner> OITravelPartnerList { get; set; }

        #endregion

    }
}
