using System;
using System.Collections.Generic;

namespace iArsenal.Entity
{
    public class Order_Wish : OrderBase
    {
        public Order_Wish() { }

        public Order_Wish(int id)
            : base(id)
        {
            List<OrderItemBase> oiList = OrderItemBase.GetOrderItems(id).FindAll(oi => oi.IsActive);

            if (oiList != null && oiList.Count > 0)
            {
                WishList_Existent = oiList.FindAll(oi => !oi.ProductGuid.Equals(Guid.Empty) && Product.Cache.Load(oi.ProductGuid).ProductType.Equals(ProductType.Other));

                WishList_Nonexistent = oiList.FindAll(oi => oi.ProductGuid.Equals(Guid.Empty));

                if (WishList_Existent.Count > 0 || WishList_Nonexistent.Count > 0)
                {
                    base.URLOrderView = "iArsenalOrderView_ArsenalDirect.aspx";
                }
                else
                {
                    throw new Exception("Unable to init Order_Wish.");
                }
            }

            #region Order Status Workflow Info

            string _strWorkflow = "{{ \"StatusType\": \"{0}\", \"StatusInfo\": \"{1}\" }}";

            string[] _workflowInfo = {
                                      string.Format(_strWorkflow, ((int)OrderStatusType.Draft).ToString(), "未提交"),
                                      string.Format(_strWorkflow, ((int)OrderStatusType.Submitted).ToString(), "后台审核"), 
                                      string.Format(_strWorkflow, ((int)OrderStatusType.Approved).ToString(), "待会员确认"), 
                                      string.Format(_strWorkflow, ((int)OrderStatusType.Confirmed).ToString(), "已确认"), 
                                      string.Format(_strWorkflow, ((int)OrderStatusType.Ordered).ToString(), "已下单"), 
                                      string.Format(_strWorkflow, ((int)OrderStatusType.Delivered).ToString(), "已发货")
                                  };

            base.StatusWorkflowInfo = _workflowInfo;

            #endregion

        }

        #region Members and Properties

        public List<OrderItemBase> WishList_Existent { get; set; }

        public List<OrderItemBase> WishList_Nonexistent { get; set; }

        #endregion

    }
}
