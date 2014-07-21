using System;
using System.Collections.Generic;

namespace iArsenal.Entity
{
    public class Order_MemberShip : OrderBase
    {
        public Order_MemberShip() { }

        public Order_MemberShip(int id)
            : base(id)
        {
            List<OrderItemBase> oiList = OrderItemBase.GetOrderItems(id).FindAll(oi => oi.IsActive && Product.Cache.Load(oi.ProductGuid) != null);

            if (oiList != null && oiList.Count > 0)
            {
                OrderItemBase oiBase = null;

                oiBase = oiList.Find(oi => Product.Cache.Load(oi.ProductGuid).ProductType.Equals(ProductType.MemberShipCore));
                if (oiBase != null) { OIMemberShipCore = new OrderItem_Core(oiBase.OrderItemID); }

                oiBase = oiList.Find(oi => Product.Cache.Load(oi.ProductGuid).ProductType.Equals(ProductType.MemberShipPremier));
                if (oiBase != null) { OIMemberShipPremier = new OrderItem_Premier(oiBase.OrderItemID); }

                if (OIMemberShipCore != null || OIMemberShipPremier != null)
                {
                    base.URLOrderView = "iArsenalOrderView_MemberShip.aspx";
                }
                else
                {
                    throw new Exception("Unable to init Order_MemberShip.");
                }
            }

            #region Order Status Workflow Info

            string _strWorkflow = "{{ \"StatusType\": \"{0}\", \"StatusInfo\": \"{1}\" }}";

            string[] _workflowInfo = {
                                      string.Format(_strWorkflow, ((int)OrderStatusType.Draft).ToString(), "未提交"),
                                      string.Format(_strWorkflow, ((int)OrderStatusType.Submitted).ToString(), "审核中"), 
                                      string.Format(_strWorkflow, ((int)OrderStatusType.Confirmed).ToString(), "已确认")
                                  };

            base.StatusWorkflowInfo = _workflowInfo;

            #endregion

        }

        #region Members and Properties

        public OrderItem_Core OIMemberShipCore { get; set; }

        public OrderItem_Premier OIMemberShipPremier { get; set; }

        #endregion

    }
}
