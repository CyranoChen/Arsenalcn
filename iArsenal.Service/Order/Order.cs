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

        public Order(DataRow dr)
            : base(dr)
        {
            #region Generate Order URLOrderView

            UrlOrderView = string.Empty;

            #endregion

            #region Generate Order TotalPrice

            if (Sale.HasValue)
                PriceInfo = Sale.Value.ToString("f2");
            else
                PriceInfo = Price.ToString("f2");

            #endregion

            #region Generate Order Payment Info

            if (!string.IsNullOrEmpty(Payment))
            {
                string[] _strPayment = Payment.Substring(1, Payment.Length - 2).Split('|');
                if (_strPayment[0].Equals(OrderPaymentType.Alipay.ToString(), StringComparison.OrdinalIgnoreCase))
                    PaymentInfo = string.Format("【支付宝】{0}", _strPayment[1]);
                else if (_strPayment[0].Equals(OrderPaymentType.Bank.ToString(), StringComparison.OrdinalIgnoreCase))
                {
                    if (_strPayment.Length >= 3)
                        PaymentInfo = string.Format("【{0}】{1}", _strPayment[1], _strPayment[2]);
                    else
                        PaymentInfo = _strPayment[1];
                }
                else
                    PaymentInfo = string.Empty;
            }
            else
            {
                PaymentInfo = string.Empty;
            }

            #endregion

            #region Generate Order Status Info

            string _strStatus = string.Empty;

            switch (Status)
            {
                case OrderStatusType.Draft:
                    _strStatus = "未提交";
                    break;
                case OrderStatusType.Submitted:
                    _strStatus = "审核中";
                    break;
                case OrderStatusType.Confirmed:
                    _strStatus = "已确认";
                    break;
                case OrderStatusType.Ordered:
                    _strStatus = "已下单";
                    break;
                case OrderStatusType.Delivered:
                    _strStatus = "已发货";
                    break;
                case OrderStatusType.Error:
                    _strStatus = "未知";
                    break;
                case OrderStatusType.Approved:
                    _strStatus = "已审核";
                    break;
                default:
                    _strStatus = string.Empty;
                    break;
            }

            StatusInfo = _strStatus;

            #endregion
        }

        public void CalcOrderPrice(SqlTransaction trans = null)
        {
            float price = 0f;

            IRepository repo = new Repository();

            var query = repo.Query<OrderItem>(x => x.OrderID.Equals(this.ID) && x.IsActive);

            if (query != null && query.Count() > 0)
            {
                foreach (var oi in query)
                {
                    price += oi.TotalPrice;
                }
            }

            this.Price = price + Postage;

            repo.Update(this, trans);
        }

        public static Order SelectByID(int id)
        {
            IRepository repo = new Repository();

            Order o = repo.Single<Order>(id);

            if (o != null)
            {
                if (!o.OrderType.HasValue)
                {
                    return o;
                }
                else
                {
                    switch (o.OrderType.Value)
                    {
                        case OrderBaseType.ReplicaKit:
                            return repo.Single<OrdrReplicaKit>(id);
                        case OrderBaseType.Ticket:
                            return repo.Single<OrdrTicket>(id);
                        case OrderBaseType.Travel:
                            return repo.Single<OrdrTravel>(id);
                        case OrderBaseType.Wish:
                            return repo.Single<OrdrWish>(id);
                        case OrderBaseType.MemberShip:
                            return repo.Single<Order_MemberShip>(id);
                        default:
                            return o;
                    }
                }
            }
            else { return null; }
        }

        public void RefreshOrderType()
        {
            IRepository repo = new Repository();
            var query = repo.Query<OrderItem>(x => Product.Cache.Load(x.ProductGuid) != null && x.OrderID.Equals(ID));

            if (query.Count() > 0)
            {
                OrderType = SetOrderType(query.ToList());
            }
        }

        private static OrderBaseType? SetOrderType(List<OrderItem> list)
        {
            if (list.Any(delegate(OrderItem x)
            {
                var _type = Product.Cache.Load(x.ProductGuid).ProductType;
                return _type.Equals(ProductType.ReplicaKitHome) || _type.Equals(ProductType.ReplicaKitAway) || _type.Equals(ProductType.ReplicaKitCup);
            }))
            {
                return OrderBaseType.ReplicaKit;
            }
            else if (list.Any(delegate(OrderItem x)
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
            else if (list.Any(delegate(OrderItem x)
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
            else if (list.Any(delegate(OrderItem x)
            {
                var _type = Product.Cache.Load(x.ProductGuid).ProductType;
                return _type.Equals(ProductType.MemberShipCore) || _type.Equals(ProductType.MemberShipPremier);
            }))
            {
                return OrderBaseType.MemberShip;
            }
            else
            {
                return null;
            }
        }

        // Don't place LINQ to Foreach, first ToList(), then use list.FindAll to improve performance
        public static void RefreshOrderBaseType()
        {
            IRepository repo = new Repository();
            var oList = repo.All<Order>();
            var oiList = repo.Query<OrderItem>(x => Product.Cache.Load(x.ProductGuid) != null).ToList();

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
        public float Price
        { get; set; }

        [DbColumn("Sale")]
        public float? Sale
        { get; set; }

        [DbColumn("Deposit")]
        public float? Deposit
        { get; set; }

        [DbColumn("Postage")]
        public float Postage
        { get; set; }

        [DbColumn("Status")]
        public OrderStatusType Status
        { get; set; }

        [DbColumn("Rate")]
        public int Rate
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
        public OrderBaseType? OrderType
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
        ReplicaKit,
        Ticket,
        Travel,
        Wish,
        MemberShip
    }
}
