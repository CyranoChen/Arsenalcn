using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

using Arsenalcn.Core;

namespace iArsenal.Service
{
    public class OrdrWish : Order
    {
        public OrdrWish() { }

        public void Init()
        {
            IRepository repo = new Repository();

            var list = repo.Query<OrderItem>(x => x.OrderID == ID && x.IsActive == true);

            if (list != null && list.Count > 0)
            {
                WishList_Existent = list.FindAll(x => !x.ProductGuid.Equals(Guid.Empty) &&
                    Product.Cache.Load(x.ProductGuid).ProductType.Equals(ProductType.Other));

                WishList_Nonexistent = list.FindAll(x => x.ProductGuid.Equals(Guid.Empty));

                if (WishList_Existent.Count > 0 || WishList_Nonexistent.Count > 0)
                {
                    base.UrlOrderView = "iArsenalOrderView_ArsenalDirect.aspx";
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

        public List<OrderItem> WishList_Existent { get; set; }

        public List<OrderItem> WishList_Nonexistent { get; set; }

        #endregion

    }
}
