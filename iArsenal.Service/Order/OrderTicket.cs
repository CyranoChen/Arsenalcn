using System;
using System.Data;
using System.Linq;
using Arsenalcn.Core;
using AutoMapper;

namespace iArsenal.Service
{
    public class OrdrTicket : Order
    {
        public void Init(IDbTransaction trans)
        {
            IRepository repo = new Repository();

            var list = repo.Query<OrderItem>(x => x.OrderID == ID, trans)
                .FindAll(x => x.IsActive && Product.Cache.Load(x.ProductGuid) != null);

            if (list.Any())
            {
                var oiBase = list.Find(x => Product.Cache.Load(x.ProductGuid).ProductType.Equals(ProductType.MatchTicket));
                if (oiBase != null)
                {
                    var mapperMatchTicket = new MapperConfiguration(cfg =>
                        cfg.CreateMap<OrderItem, OrdrItmMatchTicket>().AfterMap((s, d) => d.Init()))
                        .CreateMapper();

                    OIMatchTicket = mapperMatchTicket.Map<OrdrItmMatchTicket>(oiBase);
                }

                oiBase = list.Find(x => Product.Cache.Load(x.ProductGuid).ProductType.Equals(ProductType.TicketFriendly) && 
                    Product.Cache.Load(x.ProductGuid).Code.StartsWith("20170722"));
                if (oiBase != null)
                {
                    var mapperTicketBeijing = new MapperConfiguration(cfg =>
                        cfg.CreateMap<OrderItem, OrdrItm2017TicketBeijing>().AfterMap((s, d) => d.Init()))
                        .CreateMapper();

                    OI2017TicketBeijing = mapperTicketBeijing.Map<OrdrItm2017TicketBeijing>(oiBase);
                }

                oiBase = list.Find(x => Product.Cache.Load(x.ProductGuid).ProductType.Equals(ProductType.TicketFriendly) && 
                    Product.Cache.Load(x.ProductGuid).Code.StartsWith("20120727"));
                if (oiBase != null)
                {
                    var mapperTicketBeijing = new MapperConfiguration(cfg =>
                        cfg.CreateMap<OrderItem, OrdrItm2012TicketBeijing>().AfterMap((s, d) => d.Init()))
                        .CreateMapper();

                    OI2012TicketBeijing = mapperTicketBeijing.Map<OrdrItm2012TicketBeijing>(oiBase);
                }

                // Set the value of URLOrderView;

                if (OIMatchTicket != null)
                {
                    UrlOrderView = "iArsenalOrderView_MatchTicket.aspx";
                }
                else if (OI2017TicketBeijing != null || OI2012TicketBeijing != null)
                {
                    UrlOrderView = "iArsenalOrderView_TicketFriendly.aspx";
                }
                else
                {
                    throw new Exception("Unable to init Order_Ticket.");
                }
            }

            #region Order Status Workflow Info

            var strWorkflow = "{{ \"StatusType\": \"{0}\", \"StatusInfo\": \"{1}\" }}";

            string[] workflowInfo =
            {
                string.Format(strWorkflow, ((int) OrderStatusType.Draft), "未提交"),
                string.Format(strWorkflow, ((int) OrderStatusType.Submitted), "后台审核"),
                string.Format(strWorkflow, ((int) OrderStatusType.Confirmed), "已付款"),
                string.Format(strWorkflow, ((int) OrderStatusType.Ordered), "预定成功"),
                string.Format(strWorkflow, ((int) OrderStatusType.Delivered), "已出票")
            };

            StatusWorkflowInfo = workflowInfo;

            #endregion
        }

        #region Members and Properties

        public OrdrItmMatchTicket OIMatchTicket { get; set; }

        public OrdrItm2017TicketBeijing OI2017TicketBeijing { get; set; }

        public OrdrItm2012TicketBeijing OI2012TicketBeijing { get; set; }

        #endregion
    }
}