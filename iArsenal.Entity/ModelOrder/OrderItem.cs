using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace iArsenal.Entity
{
    public class OrderItem
    {
        public OrderItem() { }

        protected OrderItem(int id)
        {
            this.OrderItemID = id;
            this.Select();
        }

        private OrderItem(DataRow dr)
        {
            InitOrderItem(dr);
        }

        private void InitOrderItem(DataRow dr)
        {
            if (dr != null)
            {
                OrderItemID = Convert.ToInt32(dr["ID"]);
                MemberID = Convert.ToInt32(dr["MemberID"]);
                MemberName = dr["MemberName"].ToString();
                OrderID = Convert.ToInt32(dr["OrderID"]); ;
                ProductGuid = (Guid)dr["ProductGuid"];
                Code = dr["Code"].ToString();
                ProductName = dr["ProductName"].ToString();
                Size = dr["Size"].ToString();
                UnitPrice = Convert.ToSingle(dr["UnitPrice"]);
                Quantity = Convert.ToInt32(dr["Quantity"]);

                if (!Convert.IsDBNull(dr["Sale"]))
                    Sale = Convert.ToSingle(dr["Sale"]);
                else
                    Sale = null;

                CreateTime = (DateTime)dr["CreateTime"];
                IsActive = Convert.ToBoolean(dr["IsActive"]);
                Remark = dr["Remark"].ToString();

                #region Generate OrderItem TotalPrice

                if (Sale.HasValue)
                    TotalPrice = Sale.Value;
                else
                    TotalPrice = UnitPrice * Quantity;

                #endregion

            }
            else
                throw new Exception("Unable to init OrderItemBase.");
        }

        public virtual void Mapper(Object obj)
        {
            foreach (var properInfo in this.GetType()
                .GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance))
            {
                var properInfoOrgin = obj.GetType().GetProperty(properInfo.Name);
                if (properInfoOrgin != null)
                {
                    properInfo.SetValue(this, properInfoOrgin.GetValue(obj, null), null);
                }
            }
        }

        public void Select()
        {
            DataRow dr = DataAccess.OrderItem.GetOrderItemByID(OrderItemID);

            if (dr != null)
                InitOrderItem(dr);
        }

        public virtual void Update(SqlTransaction trans = null)
        {
            DataAccess.OrderItem.UpdateOrderItem(OrderItemID, MemberID, MemberName, OrderID, ProductGuid, Code, ProductName, Size, UnitPrice, Quantity, Sale, CreateTime, IsActive, Remark, trans);
        }

        public virtual void Insert(SqlTransaction trans = null)
        {
            DataAccess.OrderItem.InsertOrderItem(OrderItemID, MemberID, MemberName, OrderID, ProductGuid, Code, ProductName, Size, UnitPrice, Quantity, Sale, CreateTime, IsActive, Remark, trans);
        }

        public void Delete(SqlTransaction trans = null)
        {
            DataAccess.OrderItem.DeleteOrderItem(OrderItemID, trans);
        }

        public static List<OrderItem> GetOrderItems()
        {
            DataTable dt = DataAccess.OrderItem.GetOrderItems();
            List<OrderItem> list = new List<OrderItem>();

            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    list.Add(new OrderItem(dr));
                }
            }

            return list;
        }

        public static List<OrderItem> GetOrderItems(int orderID)
        {
            DataTable dt = DataAccess.OrderItem.GetOrderItems(orderID);
            List<OrderItem> list = new List<OrderItem>();

            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    list.Add(new OrderItem(dr));
                }
            }

            return list;
        }

        public static int RemoveOrderItemByOrderID(int orderID, SqlTransaction trans = null)
        {
            int count = 0;

            List<OrderItem> list = OrderItem.GetOrderItems(orderID);

            if (list != null && list.Count > 0)
            {
                count = list.Count;

                DataAccess.OrderItem.DeleteOrderItemByOrderID(orderID, trans);
            }

            return count;
        }

        public virtual void Insert(Member m, Product p, Order o, string size, int quantity, float? sale, string remark, SqlTransaction trans = null)
        {
            OrderItem oi = new OrderItem();
            oi.MemberID = m.MemberID;
            oi.MemberName = m.Name;
            oi.OrderID = o.OrderID;
            oi.ProductGuid = p.ProductGuid;
            oi.Code = p.Code;
            oi.ProductName = string.Format("{0} ({1})", p.DisplayName, p.Name);
            oi.Size = size;
            oi.UnitPrice = p.PriceCNY;
            oi.Quantity = quantity;
            oi.Sale = sale;
            oi.CreateTime = DateTime.Now;
            oi.IsActive = true;
            oi.Remark = remark;

            oi.Insert(trans);
        }

        #region Members and Properties

        public int OrderItemID
        { get; set; }

        public int MemberID
        { get; set; }

        public string MemberName
        { get; set; }

        public int OrderID
        { get; set; }

        public Guid ProductGuid
        { get; set; }

        public string Code
        { get; set; }

        public string ProductName
        { get; set; }

        public string Size
        { get; set; }

        public float UnitPrice
        { get; set; }

        public int Quantity
        { get; set; }

        public float? Sale
        { get; set; }

        public DateTime CreateTime
        { get; set; }

        public Boolean IsActive
        { get; set; }

        public string Remark
        { get; set; }

        public float TotalPrice
        { get; set; }

        #endregion
    }
}
