using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Arsenalcn.Core;

namespace iArsenal.Service
{
    public class OrdrWish : Order
    {
        public void Init(IDbTransaction trans)
        {
            IRepository repo = new Repository();

            var list = repo.Query<OrderItem>(x => x.OrderID == ID, trans).FindAll(x => x.IsActive);

            if (list.Any())
            {
                WishList_Existent = list.FindAll(x => !x.ProductGuid.Equals(Guid.Empty) &&
                    Product.Cache.Load(x.ProductGuid).ProductType.Equals(ProductType.Other));

                WishList_Nonexistent = list.FindAll(x => x.ProductGuid.Equals(Guid.Empty));

                if (WishList_Existent.Count > 0 || WishList_Nonexistent.Count > 0)
                {
                    UrlOrderView = "iArsenalOrderView_ArsenalDirect.aspx";
                }
                else
                {
                    throw new Exception("Unable to init Order_Wish.");
                }
            }

            #region Order Status Workflow Info

            var strWorkflow = "{{ \"StatusType\": \"{0}\", \"StatusInfo\": \"{1}\" }}";

            string[] workflowInfo =
            {
                string.Format(strWorkflow, ((int) OrderStatusType.Draft), "未提交"),
                string.Format(strWorkflow, ((int) OrderStatusType.Submitted), "后台审核"),
                string.Format(strWorkflow, ((int) OrderStatusType.Approved), "待会员确认"),
                string.Format(strWorkflow, ((int) OrderStatusType.Confirmed), "已确认"),
                string.Format(strWorkflow, ((int) OrderStatusType.Ordered), "已下单"),
                string.Format(strWorkflow, ((int) OrderStatusType.Delivered), "已发货")
            };

            StatusWorkflowInfo = workflowInfo;

            #endregion
        }

        #region Members and Properties

        public List<OrderItem> WishList_Existent { get; set; }

        public List<OrderItem> WishList_Nonexistent { get; set; }

        #endregion
    }
}