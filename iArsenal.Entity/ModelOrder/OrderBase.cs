using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace iArsenal.Entity
{
    public class OrderBase
    {
        public OrderBase() { }

        protected OrderBase(int id)
        {
            this.OrderID = id;
            this.Select();
        }

        private OrderBase(DataRow dr)
        {
            InitOrder(dr);
        }

        private void InitOrder(DataRow dr)
        {
            if (dr != null)
            {
                OrderID = Convert.ToInt32(dr["ID"]);
                MemberID = Convert.ToInt32(dr["MemberID"]);
                MemberName = dr["MemberName"].ToString();

                if (!string.IsNullOrEmpty((dr["OrderType"].ToString())))
                    OrderType = (OrderBaseType)Enum.Parse(typeof(OrderBaseType), dr["OrderType"].ToString());
                else
                    OrderType = null;

                Mobile = dr["Mobile"].ToString();
                Address = dr["Address"].ToString();
                Payment = dr["Payment"].ToString();
                Price = Convert.ToSingle(dr["Price"]);

                if (!Convert.IsDBNull(dr["Sale"]))
                    Sale = Convert.ToSingle(dr["Sale"]);
                else
                    Sale = null;

                if (!Convert.IsDBNull(dr["Deposit"]))
                    Deposit = Convert.ToSingle(dr["Deposit"]);
                else
                    Deposit = null;

                Postage = Convert.ToSingle(dr["Postage"]);
                Status = (OrderStatusType)Enum.Parse(typeof(OrderStatusType), dr["Status"].ToString());
                Rate = Convert.ToInt16(dr["Rate"]);
                CreateTime = (DateTime)dr["CreateTime"];
                UpdateTime = (DateTime)dr["UpdateTime"];
                IsActive = Convert.ToBoolean(dr["IsActive"]);
                Description = dr["Description"].ToString();
                Remark = dr["Remark"].ToString();

                #region Generate Order URLOrderView

                URLOrderView = string.Empty;

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
            else
                throw new Exception("Unable to init Order.");
        }

        public void Select()
        {
            DataRow dr = DataAccess.Order.GetOrderByID(OrderID);

            if (dr != null)
                InitOrder(dr);
        }

        public static OrderBase Select(int id)
        {
            DataRow dr = DataAccess.Order.GetOrderByID(id);

            if (dr != null)
            {
                if (string.IsNullOrEmpty(dr["OrderType"].ToString()))
                {
                    return new OrderBase(id);
                }

                OrderBaseType oType = (OrderBaseType)Enum.Parse(typeof(OrderBaseType), dr["OrderType"].ToString());

                switch (oType)
                {
                    case OrderBaseType.ReplicaKit:
                        return new Order_ReplicaKit(id);
                    case OrderBaseType.Ticket:
                        return new Order_Ticket(id);
                    case OrderBaseType.Travel:
                        return new Order_Travel(id);
                    case OrderBaseType.Wish:
                        return new Order_Wish(id);
                    default:
                        return new OrderBase(id);
                }
            }
            else
            {
                return null;
            }
        }

        public void Update(SqlTransaction trans = null)
        {
            this.GetOrderBaseType();

            string _oType = OrderType.HasValue ? OrderType.Value.ToString() : string.Empty;

            DataAccess.Order.UpdateOrder(OrderID, MemberID, MemberName, _oType, Mobile, Address, Payment,
                Price, Sale, Deposit, Postage, (int)Status, Rate, CreateTime, UpdateTime, IsActive, Description, Remark, trans);
        }

        public void Insert(SqlTransaction trans = null)
        {
            this.GetOrderBaseType();

            string _oType = OrderType.HasValue ? OrderType.Value.ToString() : string.Empty;

            OrderID = DataAccess.Order.InsertOrder(OrderID, MemberID, MemberName, _oType, Mobile, Address, Payment,
                Price, Sale, Deposit, Postage, (int)Status, Rate, CreateTime, UpdateTime, IsActive, Description, Remark, trans);
        }

        public void Delete(SqlTransaction trans = null)
        {
            DataAccess.Order.DeleteOrder(OrderID, trans);
        }

        public void CalcOrderPrice(SqlTransaction trans = null)
        {
            float price = 0f;

            List<OrderItemBase> list = OrderItemBase.GetOrderItems(this.OrderID).FindAll(oi => oi.IsActive);

            if (list != null && list.Count > 0)
            {
                foreach (OrderItemBase oi in list)
                {
                    price += oi.TotalPrice;
                }
            }

            this.Price = price + Postage;

            this.Update(trans);
        }


        public static List<OrderBase> GetOrders()
        {
            DataTable dt = DataAccess.Order.GetOrders();
            List<OrderBase> list = new List<OrderBase>();

            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    list.Add(new OrderBase(dr));
                }
            }

            return list;
        }

        public static List<OrderBase> GetOrders(int memberID)
        {
            DataTable dt = DataAccess.Order.GetOrders(memberID);
            List<OrderBase> list = new List<OrderBase>();

            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    list.Add(new OrderBase(dr));
                }
            }

            return list;
        }

        public void GetOrderBaseType()
        {
            List<OrderItemBase> oiList = OrderItemBase.GetOrderItems(OrderID).FindAll(oi => Product.Cache.Load(oi.ProductGuid) != null);

            if (oiList != null && oiList.Count > 0)
            {
                if (oiList.Exists(oi => Product.Cache.Load(oi.ProductGuid).ProductType.Equals(ProductType.ReplicaKitHome)
                    || Product.Cache.Load(oi.ProductGuid).ProductType.Equals(ProductType.ReplicaKitAway)))
                {
                    OrderType = OrderBaseType.ReplicaKit;
                }
                else if (oiList.Exists(oi => Product.Cache.Load(oi.ProductGuid).ProductType.Equals(ProductType.MatchTicket))
                    || oiList.Exists(oi => Product.Cache.Load(oi.ProductGuid).ProductType.Equals(ProductType.TicketBeijing)))
                {
                    OrderType = OrderBaseType.Ticket;
                }
                else if (oiList.Exists(oi => Product.Cache.Load(oi.ProductGuid).ProductType.Equals(ProductType.TravelPlan)))
                {
                    OrderType = OrderBaseType.Travel;
                }
                else if (oiList.Exists(oi => !oi.ProductGuid.Equals(Guid.Empty) && Product.Cache.Load(oi.ProductGuid).ProductType.Equals(ProductType.Other))
                    || oiList.Exists(oi => oi.ProductGuid.Equals(Guid.Empty)))
                {
                    OrderType = OrderBaseType.Wish;
                }
                else
                {
                    OrderType = null;
                }
            }
            else
            {
                OrderType = null;
            }
        }

        public static void RefreshOrderBaseType()
        {
            List<OrderBase> list = OrderBase.GetOrders();

            if (list != null && list.Count > 0)
            {
                foreach (OrderBase o in list)
                {
                    o.GetOrderBaseType();
                    o.Update();
                }
            }
        }

        #region Members and Properties

        public int OrderID
        { get; set; }

        public int MemberID
        { get; set; }

        public string MemberName
        { get; set; }

        public string Mobile
        { get; set; }

        public string Address
        { get; set; }

        public string Payment
        { get; set; }

        public float Price
        { get; set; }

        public float? Sale
        { get; set; }

        public float? Deposit
        { get; set; }

        public float Postage
        { get; set; }

        public OrderStatusType Status
        { get; set; }

        public int Rate
        { get; set; }

        public DateTime CreateTime
        { get; set; }

        public DateTime UpdateTime
        { get; set; }

        public Boolean IsActive
        { get; set; }

        public string Description
        { get; set; }

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

        public OrderBaseType? OrderType
        { get; set; }

        public string URLOrderView
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
        Wish
    }
}
