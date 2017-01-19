using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Arsenalcn.Core;
using AutoMapper;

namespace iArsenal.Service
{
    [DbSchema("iArsenal_Order", Sort = "ID DESC")]
    public class Order : Entity<int>
    {
        //public static void CreateMap()
        //{
        //    var map = Mapper.CreateMap<IDataReader, Order>();

        //    map.ForMember(d => d.UrlOrderView, opt => opt.UseValue(string.Empty));

        //    //map.ForMember(d => d.OrderType, opt => opt.MapFrom(s =>
        //    //    (OrderBaseType)Enum.Parse(typeof(OrderBaseType), s.GetValue("OrderType").ToString())));

        //    map.ForMember(d => d.PaymentInfo, opt => opt.ResolveUsing(s =>
        //    {
        //        #region Generate Order Payment Info

        //        string retValue;
        //        var payment = s.GetValue("Payment").ToString();

        //        if (!string.IsNullOrEmpty(payment))
        //        {
        //            var strPayment = payment.Substring(1, payment.Length - 2).Split('|');
        //            if (strPayment[0].Equals(OrderPaymentType.Alipay.ToString(), StringComparison.OrdinalIgnoreCase))
        //                retValue = $"【支付宝】{strPayment[1]}";
        //            else if (strPayment[0].Equals(OrderPaymentType.Bank.ToString(), StringComparison.OrdinalIgnoreCase))
        //            {
        //                retValue = strPayment.Length >= 3 ? $"【{strPayment[1]}】{strPayment[2]}" : strPayment[1];
        //            }
        //            else
        //                retValue = string.Empty;
        //        }
        //        else
        //        {
        //            retValue = string.Empty;
        //        }

        //        return retValue;

        //        #endregion
        //    }));

        //    map.ForMember(d => d.PriceInfo, opt => opt.ResolveUsing(s =>
        //    {
        //        #region Generate Order Price Info

        //        var sale = (double?)s.GetValue("Sale");
        //        var price = (double)s.GetValue("Price");

        //        return sale?.ToString("f2") ?? price.ToString("f2");

        //        #endregion
        //    }));

        //    map.ForMember(d => d.StatusInfo, opt => opt.ResolveUsing(s =>
        //    {
        //        #region Generate Order Status Info

        //        string retValue;

        //        switch ((OrderStatusType)((int)s.GetValue("Status")))
        //        {
        //            case OrderStatusType.Draft:
        //                retValue = "未提交";
        //                break;
        //            case OrderStatusType.Submitted:
        //                retValue = "审核中";
        //                break;
        //            case OrderStatusType.Confirmed:
        //                retValue = "已确认";
        //                break;
        //            case OrderStatusType.Ordered:
        //                retValue = "已下单";
        //                break;
        //            case OrderStatusType.Delivered:
        //                retValue = "已发货";
        //                break;
        //            case OrderStatusType.Error:
        //                retValue = "未知";
        //                break;
        //            case OrderStatusType.Approved:
        //                retValue = "已审核";
        //                break;
        //            default:
        //                retValue = string.Empty;
        //                break;
        //        }

        //        return retValue;

        //        #endregion
        //    }));
        //}

        public override void Inital()
        {
            UrlOrderView = string.Empty;

            #region Generate Order Payment Info

            if (!string.IsNullOrEmpty(Payment))
            {
                var strPayment = Payment.Substring(1, Payment.Length - 2).Split('|');
                if (strPayment[0].Equals(OrderPaymentType.Alipay.ToString(), StringComparison.OrdinalIgnoreCase))
                    PaymentInfo = $"【支付宝】{strPayment[1]}";
                else if (strPayment[0].Equals(OrderPaymentType.Bank.ToString(), StringComparison.OrdinalIgnoreCase))
                {
                    PaymentInfo = strPayment.Length >= 3 ? $"【{strPayment[1]}】{strPayment[2]}" : strPayment[1];
                }
                else
                    PaymentInfo = string.Empty;
            }
            else
            {
                PaymentInfo = string.Empty;
            }

            #endregion

            #region Generate Order Price Info

            PriceInfo = Sale?.ToString("f2") ?? Price.ToString("f2");

            #endregion

            #region Generate Order Status Info

            switch (Status)
            {
                case OrderStatusType.Draft:
                    StatusInfo = "未提交";
                    break;
                case OrderStatusType.Submitted:
                    StatusInfo = "审核中";
                    break;
                case OrderStatusType.Confirmed:
                    StatusInfo = "已确认";
                    break;
                case OrderStatusType.Ordered:
                    StatusInfo = "已下单";
                    break;
                case OrderStatusType.Delivered:
                    StatusInfo = "已发货";
                    break;
                case OrderStatusType.Error:
                    StatusInfo = "未知";
                    break;
                case OrderStatusType.Approved:
                    StatusInfo = "已审核";
                    break;
                default:
                    StatusInfo = string.Empty;
                    break;
            }

            #endregion
        }

        public void CalcOrderPrice(SqlTransaction trans = null)
        {
            var price = default(double);

            IRepository repo = new Repository();

            var query = repo.Query<OrderItem>(x => x.OrderID == ID).FindAll(x => x.IsActive);

            if (query.Any())
            {
                price += query.Sum(oi => oi.TotalPrice);
            }

            Price = price + Postage;

            repo.Update(this, trans);
        }

        public static Order Select(int key)
        {
            IRepository repo = new Repository();

            var o = repo.Single<Order>(key);

            if (o == null) return null;

            switch (o.OrderType)
            {
                case OrderBaseType.ReplicaKit:
                    var mapperReplicaKit = new MapperConfiguration(cfg =>
                        cfg.CreateMap<Order, OrdrReplicaKit>().AfterMap((s, d) => d.Init())).CreateMapper();
                    return mapperReplicaKit.Map<OrdrReplicaKit>(o);
                case OrderBaseType.Printing:
                    var mapperPrinting = new MapperConfiguration(cfg =>
                        cfg.CreateMap<Order, OrdrPrinting>().AfterMap((s, d) => d.Init())).CreateMapper();
                    return mapperPrinting.Map<OrdrPrinting>(o);
                case OrderBaseType.Ticket:
                    var mapperTicket = new MapperConfiguration(cfg =>
                        cfg.CreateMap<Order, OrdrTicket>().AfterMap((s, d) => d.Init())).CreateMapper();
                    return mapperTicket.Map<OrdrTicket>(o);
                case OrderBaseType.Travel:
                    var mapperTravel = new MapperConfiguration(cfg =>
                        cfg.CreateMap<Order, OrdrTravel>().AfterMap((s, d) => d.Init())).CreateMapper();
                    return mapperTravel.Map<OrdrTravel>(o);
                case OrderBaseType.Wish:
                    var mapperWish = new MapperConfiguration(cfg =>
                        cfg.CreateMap<Order, OrdrWish>().AfterMap((s, d) => d.Init())).CreateMapper();
                    return mapperWish.Map<OrdrWish>(o);
                case OrderBaseType.Membership:
                    var mapperMembership = new MapperConfiguration(cfg =>
                        cfg.CreateMap<Order, OrdrMembership>().AfterMap((s, d) => d.Init())).CreateMapper();
                    return mapperMembership.Map<OrdrMembership>(o);
                default:
                    return o;
            }
        }

        public void RefreshOrderType()
        {
            IRepository repo = new Repository();
            var query = repo.Query<OrderItem>(x => x.OrderID == ID)
                .FindAll(x => Product.Cache.Load(x.ProductGuid) != null);

            if (query.Count > 0)
            {
                OrderType = GetOrderTypeByOrderItems(query.ToList());
            }
        }

        public static OrderBaseType GetOrderTypeByOrderItems(List<OrderItem> list)
        {
            if (list.Any(delegate (OrderItem x)
            {
                var type = Product.Cache.Load(x.ProductGuid).ProductType;
                return type.Equals(ProductType.ReplicaKitHome) || type.Equals(ProductType.ReplicaKitAway) ||
                       type.Equals(ProductType.ReplicaKitCup);
            }))
            {
                return OrderBaseType.ReplicaKit;
            }

            if (list.Any(x =>
            {
                var type = Product.Cache.Load(x.ProductGuid).ProductType;
                return type.Equals(ProductType.PlayerName);
            }) && list.Any(x =>
            {
                var type = Product.Cache.Load(x.ProductGuid).ProductType;
                return type.Equals(ProductType.PlayerNumber);
            }))
            {
                return OrderBaseType.Printing;
            }

            if (list.Any(delegate (OrderItem x)
            {
                var type = Product.Cache.Load(x.ProductGuid).ProductType;
                return type.Equals(ProductType.MatchTicket) || type.Equals(ProductType.TicketBeijing);
            }))
            {
                return OrderBaseType.Ticket;
            }

            if (list.Any(x => Product.Cache.Load(x.ProductGuid).ProductType.Equals(ProductType.TravelPlan)))
            {
                return OrderBaseType.Travel;
            }

            if (list.Any(
                x =>
                    x.ProductGuid.Equals(Guid.Empty) ||
                    Product.Cache.Load(x.ProductGuid).ProductType.Equals(ProductType.Other)))
            {
                return OrderBaseType.Wish;
            }

            if (list.Any(delegate (OrderItem x)
            {
                var type = Product.Cache.Load(x.ProductGuid).ProductType;
                return type.Equals(ProductType.MembershipCore) || type.Equals(ProductType.MembershipPremier);
            }))
            {
                return OrderBaseType.Membership;
            }

            return OrderBaseType.None;
        }

        #region Members and Properties

        [DbColumn("MemberID")]
        public int MemberID { get; set; }

        [DbColumn("MemberName")]
        public string MemberName { get; set; }

        [DbColumn("Mobile")]
        public string Mobile { get; set; }

        [DbColumn("Address")]
        public string Address { get; set; }

        [DbColumn("Payment")]
        public string Payment { get; set; }

        [DbColumn("Price")]
        public double Price { get; set; }

        [DbColumn("Sale")]
        public double? Sale { get; set; }

        [DbColumn("Deposit")]
        public double? Deposit { get; set; }

        [DbColumn("Postage")]
        public double Postage { get; set; }

        [DbColumn("Status")]
        public OrderStatusType Status { get; set; }

        [DbColumn("Rate")]
        public short Rate { get; set; }

        [DbColumn("CreateTime")]
        public DateTime CreateTime { get; set; }

        [DbColumn("UpdateTime")]
        public DateTime UpdateTime { get; set; }

        [DbColumn("IsActive")]
        public bool IsActive { get; set; }

        [DbColumn("Description")]
        public string Description { get; set; }

        [DbColumn("Remark")]
        public string Remark { get; set; }

        public string PriceInfo { get; set; }

        public string PaymentInfo { get; set; }

        public string StatusInfo { get; set; }

        public string[] StatusWorkflowInfo { get; set; }

        [DbColumn("OrderType")]
        public OrderBaseType OrderType { get; set; }

        public string UrlOrderView { get; set; }

        #endregion
    }

    public enum OrderStatusType
    {
        Draft = 1,
        Submitted = 2,
        Confirmed = 3,
        Ordered = 4,
        Delivered = 5,
        Error = 0,
        // for WishList to be confirmed
        Approved = 21
    }

    public enum OrderPaymentType
    {
        Alipay,
        Bank
    }

    public enum OrderBaseType
    {
        None = 0,
        ReplicaKit = 1,
        Ticket = 2,
        Travel = 3,
        Wish = 4,
        Membership = 5,
        Printing = 6
    }
}