using System;
using Arsenalcn.Core;

namespace iArsenal.Service
{
    public class OrdrTicket : Order
    {
        public OrdrTicket() { }

        public void Init()
        {
            IRepository repo = new Repository();

            var list = repo.Query<OrderItem>(x => x.OrderID == ID && x.IsActive == true)
                .FindAll(x => Product.Cache.Load(x.ProductGuid) != null);

            if (list != null && list.Count > 0)
            {
                OrderItem oiBase = null;

                oiBase = list.Find(x => Product.Cache.Load(x.ProductGuid).ProductType.Equals(ProductType.MatchTicket));
                if (oiBase != null)
                {
                    AutoMapper.Mapper.CreateMap<OrderItem, OrdrItmMatchTicket>().AfterMap((s, d) => d.Init());
                    OIMatchTicket = AutoMapper.Mapper.Map<OrdrItmMatchTicket>(oiBase);
                }

                oiBase = list.Find(x => Product.Cache.Load(x.ProductGuid).ProductType.Equals(ProductType.TicketBeijing));
                if (oiBase != null)
                {
                    AutoMapper.Mapper.CreateMap<OrderItem, OrdrItm2012TicketBeijing>().AfterMap((s, d) => d.Init());
                    OITicketBeijing = AutoMapper.Mapper.Map<OrdrItm2012TicketBeijing>(oiBase);
                }

                // Set the value of URLOrderView;

                if (OIMatchTicket != null)
                {
                    base.UrlOrderView = "iArsenalOrderView_MatchTicket.aspx";
                }
                else if (OITicketBeijing != null)
                {
                    base.UrlOrderView = "iArsenalOrderView_TicketBeijing.aspx";
                }
                else
                {
                    throw new Exception("Unable to init Order_Ticket.");
                }
            }

            #region Order Status Workflow Info

            var _strWorkflow = "{{ \"StatusType\": \"{0}\", \"StatusInfo\": \"{1}\" }}";

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
