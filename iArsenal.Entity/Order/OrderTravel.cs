using System;
using System.Collections.Generic;

namespace iArsenal.Entity
{
    public class OrdrTravel : Order
    {
        public OrdrTravel() { }

        public OrdrTravel(int id) : base(id) { this.Init(); }

        private void Init()
        {
            List<OrderItem> oiList = OrderItem.GetOrderItems(OrderID).FindAll(oi =>
                oi.IsActive && Product.Cache.Load(oi.ProductGuid) != null);

            if (oiList != null && oiList.Count > 0)
            {
                OrderItem oiBase = null;

                oiBase = oiList.Find(oi => Product.Cache.Load(oi.ProductGuid).ProductType.Equals(ProductType.TravelPlan));
                if (oiBase != null) { OITravelPlan = new OrdrItmTravelPlan(oiBase.OrderItemID); }

                if (OITravelPlan != null)
                {
                    OITravelPartnerList = oiList.FindAll(oi => Product.Cache.Load(oi.ProductGuid).ProductType.Equals(ProductType.TravelPartner));

                    base.URLOrderView = "iArsenalOrderView_Travel.aspx";
                }
                else
                {
                    throw new Exception("Unable to init Order_Travel.");
                }
            }

            #region Order Status Workflow Info

            string _strWorkflow = "{{ \"StatusType\": \"{0}\", \"StatusInfo\": \"{1}\" }}";

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

        public List<OrderItem> OITravelPartnerList { get; set; }

        #endregion

    }
}
