using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

using Arsenalcn.Core;

namespace iArsenal.Service
{
    [DbSchema("iArsenal_Order", Sort = "ID DESC")]
    public class Order : Entity<int>
    {
        public Order() : base() { }

        public static void CreateMap()
        {
            var map = AutoMapper.Mapper.CreateMap<IDataReader, Order>();

            map.ForMember(d => d.UrlOrderView, opt => opt.UseValue(string.Empty));

            //map.ForMember(d => d.OrderType, opt => opt.MapFrom(s =>
            //    (OrderBaseType)Enum.Parse(typeof(OrderBaseType), s.GetValue("OrderType").ToString())));

            map.ForMember(d => d.PaymentInfo, opt => opt.ResolveUsing(s =>
            {
                #region Generate Order Payment Info
                var retValue = string.Empty;
                var payment = s.GetValue("Payment").ToString();

                if (!string.IsNullOrEmpty(payment))
                {
                    var _strPayment = payment.Substring(1, payment.Length - 2).Split('|');
                    if (_strPayment[0].Equals(OrderPaymentType.Alipay.ToString(), StringComparison.OrdinalIgnoreCase))
                        retValue = string.Format("【支付宝】{0}", _strPayment[1]);
                    else if (_strPayment[0].Equals(OrderPaymentType.Bank.ToString(), StringComparison.OrdinalIgnoreCase))
                    {
                        if (_strPayment.Length >= 3)
                            retValue = string.Format("【{0}】{1}", _strPayment[1], _strPayment[2]);
                        else
                            retValue = _strPayment[1];
                    }
                    else
                        retValue = string.Empty;
                }
                else
                {
                    retValue = string.Empty;
                }

                return retValue;
                #endregion
            }));

            map.ForMember(d => d.PriceInfo, opt => opt.ResolveUsing(s =>
            {
                #region Generate Order Price Info
                var sale = (double?)s.GetValue("Sale");
                var price = (double)s.GetValue("Price");

                return sale.HasValue ? sale.Value.ToString("f2") : price.ToString("f2");
                #endregion
            }));

            map.ForMember(d => d.StatusInfo, opt => opt.ResolveUsing(s =>
            {
                #region Generate Order Status Info
                var retValue = string.Empty;

                switch ((OrderStatusType)((int)s.GetValue("Status")))
                {
                    case OrderStatusType.Draft:
                        retValue = "未提交";
                        break;
                    case OrderStatusType.Submitted:
                        retValue = "审核中";
                        break;
                    case OrderStatusType.Confirmed:
                        retValue = "已确认";
                        break;
                    case OrderStatusType.Ordered:
                        retValue = "已下单";
                        break;
                    case OrderStatusType.Delivered:
                        retValue = "已发货";
                        break;
                    case OrderStatusType.Error:
                        retValue = "未知";
                        break;
                    case OrderStatusType.Approved:
                        retValue = "已审核";
                        break;
                    default:
                        retValue = string.Empty;
                        break;
                }

                return retValue;
                #endregion
            }));
        }

        public void CalcOrderPrice(SqlTransaction trans = null)
        {
            var price = default(double);

            IRepository repo = new Repository();

            var query = repo.Query<OrderItem>(x => x.OrderID == this.ID && x.IsActive == true);

            if (query != null && query.Count > 0)
            {
                foreach (var oi in query)
                {
                    price += oi.TotalPrice;
                }
            }

            this.Price = price + Postage;

            repo.Update(this, trans);
        }

        public static Order Select(int key)
        {
            IRepository repo = new Repository();

            var o = repo.Single<Order>(key);

            if (o != null)
            {
                switch (o.OrderType)
                {
                    case OrderBaseType.ReplicaKit:
                        AutoMapper.Mapper.CreateMap<Order, OrdrReplicaKit>()
                            .AfterMap((s, d) => d.Init());
                        return AutoMapper.Mapper.Map<OrdrReplicaKit>(o);
                    case OrderBaseType.Ticket:
                        AutoMapper.Mapper.CreateMap<Order, OrdrTicket>()
                            .AfterMap((s, d) => d.Init());
                        return AutoMapper.Mapper.Map<OrdrTicket>(o);
                    case OrderBaseType.Travel:
                        AutoMapper.Mapper.CreateMap<Order, OrdrTravel>()
                            .AfterMap((s, d) => d.Init());
                        return AutoMapper.Mapper.Map<OrdrTravel>(o);
                    case OrderBaseType.Wish:
                        AutoMapper.Mapper.CreateMap<Order, OrdrWish>()
                            .AfterMap((s, d) => d.Init());
                        return AutoMapper.Mapper.Map<OrdrWish>(o);
                    case OrderBaseType.MemberShip:
                        AutoMapper.Mapper.CreateMap<Order, OrdrMembership>()
                            .AfterMap((s, d) => d.Init());
                        return AutoMapper.Mapper.Map<OrdrMembership>(o);
                    default:
                        return o;
                }
            }
            else { return null; }
        }

        public void RefreshOrderType()
        {
            IRepository repo = new Repository();
            var query = repo.Query<OrderItem>(x => x.OrderID == ID)
                .FindAll(x => Product.Cache.Load(x.ProductGuid) != null);

            if (query.Count() > 0)
            {
                OrderType = SetOrderType(query.ToList());
            }
        }

        private static OrderBaseType SetOrderType(List<OrderItem> list)
        {
            if (list.Any(delegate (OrderItem x)
            {
                var _type = Product.Cache.Load(x.ProductGuid).ProductType;
                return _type.Equals(ProductType.ReplicaKitHome) || _type.Equals(ProductType.ReplicaKitAway) || _type.Equals(ProductType.ReplicaKitCup);
            }))
            {
                return OrderBaseType.ReplicaKit;
            }
            else if (list.Any(delegate (OrderItem x)
            {
                var _type = Product.Cache.Load(x.ProductGuid).ProductType;
                return _type.Equals(ProductType.MatchTicket) || _type.Equals(ProductType.TicketBeijing);
            }))
            {
                return OrderBaseType.Ticket;
            }
            else if (list.Any(x => Product.Cache.Load(x.ProductGuid).ProductType.Equals(ProductType.TravelPlan)))
            {
                return OrderBaseType.Travel;
            }
            else if (list.Any(delegate (OrderItem x)
            {
                if (!x.ProductGuid.Equals(Guid.Empty))
                {
                    return Product.Cache.Load(x.ProductGuid).ProductType.Equals(ProductType.Other);
                }
                else
                {
                    return true;
                }
            }))
            {
                return OrderBaseType.Wish;
            }
            else if (list.Any(delegate (OrderItem x)
            {
                var _type = Product.Cache.Load(x.ProductGuid).ProductType;
                return _type.Equals(ProductType.MemberShipCore) || _type.Equals(ProductType.MemberShipPremier);
            }))
            {
                return OrderBaseType.MemberShip;
            }
            else
            {
                return OrderBaseType.None;
            }
        }

        // Don't place LINQ to Foreach, first ToList(), then use list.FindAll to improve performance
        public static void RefreshOrderBaseType()
        {
            IRepository repo = new Repository();
            var oList = repo.All<Order>();
            var oiList = repo.All<OrderItem>().FindAll(x => Product.Cache.Load(x.ProductGuid) != null);

            if (oList.Count > 0 && oiList.Count > 0)
            {
                foreach (var o in oList)
                {
                    var _type = o.OrderType;
                    var list = oiList.FindAll(x => x.OrderID.Equals(o.ID));

                    // Refresh the OrderType of instance
                    if (list.Count > 0)
                    {
                        o.OrderType = SetOrderType(list);

                        if (!_type.Equals(o.OrderType))
                        {
                            repo.Update(o);
                        }
                    }
                    else
                    { continue; }
                }
            }
        }

        #region Members and Properties

        [DbColumn("MemberID")]
        public int MemberID
        { get; set; }

        [DbColumn("MemberName")]
        public string MemberName
        { get; set; }

        [DbColumn("Mobile")]
        public string Mobile
        { get; set; }

        [DbColumn("Address")]
        public string Address
        { get; set; }

        [DbColumn("Payment")]
        public string Payment
        { get; set; }

        [DbColumn("Price")]
        public double Price
        { get; set; }

        [DbColumn("Sale")]
        public double? Sale
        { get; set; }

        [DbColumn("Deposit")]
        public double? Deposit
        { get; set; }

        [DbColumn("Postage")]
        public float Postage
        { get; set; }

        [DbColumn("Status")]
        public OrderStatusType Status
        { get; set; }

        [DbColumn("Rate")]
        public short Rate
        { get; set; }

        [DbColumn("CreateTime")]
        public DateTime CreateTime
        { get; set; }

        [DbColumn("UpdateTime")]
        public DateTime UpdateTime
        { get; set; }

        [DbColumn("IsActive")]
        public Boolean IsActive
        { get; set; }

        [DbColumn("Description")]
        public string Description
        { get; set; }

        [DbColumn("Remark")]
        public string Remark
        { get; set; }

        public string PriceInfo
        { get; set; }

        public string PaymentInfo
        { get; set; }

        public string StatusInfo
        { get; set; }

        public string[] StatusWorkflowInfo
        { get; set; }

        [DbColumn("OrderType")]
        public OrderBaseType OrderType
        { get; set; }

        public string UrlOrderView
        { get; set; }

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
        MemberShip = 5
    }
}
