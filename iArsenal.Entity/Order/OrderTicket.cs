using System;
using System.Collections.Generic;

namespace iArsenal.Entity
{
    public class OrdrTicket : Order
    {
        public OrdrTicket() { }

        public OrdrTicket(int id) : base(id) { this.Init(); }

        private void Init()
        {
            List<OrderItem> oiList = OrderItem.GetOrderItems(OrderID).FindAll(oi => oi.IsActive && Product.Cache.Load(oi.ProductGuid) != null);

            if (oiList != null && oiList.Count > 0)
            {
                OrderItem oiBase = null;

                oiBase = oiList.Find(oi => Product.Cache.Load(oi.ProductGuid).ProductType.Equals(ProductType.MatchTicket));
                if (oiBase != null) { OIMatchTicket = new OrdrItmMatchTicket(oiBase.OrderItemID); }

                oiBase = oiList.Find(oi => Product.Cache.Load(oi.ProductGuid).ProductType.Equals(ProductType.TicketBeijing));
                if (oiBase != null) { OITicketBeijing = new OrdrItm2012TicketBeijing(oiBase.OrderItemID); }

                if (OIMatchTicket != null)
                {
                    base.URLOrderView = "iArsenalOrderView_MatchTicket.aspx";
                }
                else if (OITicketBeijing != null)
                {
                    base.URLOrderView = "iArsenalOrderView_TicketBeijing.aspx";
                }
                else
                {
                    throw new Exception("Unable to init Order_Ticket.");
                }
            }

            #region Order Status Workflow Info

            string _strWorkflow = "{{ \"StatusType\": \"{0}\", \"StatusInfo\": \"{1}\" }}";

            string[] _workflowInfo = {
                                      string.Format(_strWorkflow, ((int)OrderStatusType.Draft).ToString(), "未提交"),
                                      string.Format(_strWorkflow, ((int)OrderStatusType.Submitted).ToString(), "审核中"), 
                                      string.Format(_strWorkflow, ((int)OrderStatusType.Confirmed).ToString(), "已付款"), 
                                      string.Format(_strWorkflow, ((int)OrderStatusType.Ordered).ToString(), "已下单"), 
                                      string.Format(_strWorkflow, ((int)OrderStatusType.Delivered).ToString(), "已出票")
                                  };

            base.StatusWorkflowInfo = _workflowInfo;

            #endregion

        }

        #region Members and Properties

        public OrdrItmMatchTicket OIMatchTicket { get; set; }

        public OrdrItm2012TicketBeijing OITicketBeijing { get; set; }

        #endregion

    }
}
