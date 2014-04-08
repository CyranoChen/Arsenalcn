using System;
using System.Collections.Generic;

namespace iArsenal.Entity
{
    public class Order_Travel : OrderBase
    {
        public Order_Travel() { }

        public Order_Travel(int id)
            : base(id)
        {
            List<OrderItemBase> oiList = OrderItemBase.GetOrderItems(id).FindAll(oi => oi.IsActive && Product.Cache.Load(oi.ProductGuid) != null);

            if (oiList != null && oiList.Count > 0)
            {
                OrderItemBase oiBase = null;

                oiBase = oiList.Find(oi => Product.Cache.Load(oi.ProductGuid).ProductType.Equals(ProductType.TravelPlan));
                if (oiBase != null) { OITravelPlan = new OrderItem_TravelPlan(oiBase.OrderItemID); }

                if (OITravelPlan != null)
                {
                    oiBase = oiList.Find(oi => Product.Cache.Load(oi.ProductGuid).ProductType.Equals(ProductType.TravelPartner));
                    if (oiBase != null) { OITravelPartner = new OrderItem_TravelPartner(oiBase.OrderItemID); }

                    base.URLOrderView = "iArsenalOrderView_LondonTravel.aspx";
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

        public OrderItem_TravelPlan OITravelPlan { get; set; }

        public OrderItem_TravelPartner OITravelPartner { get; set; }

        #endregion

    }
}
