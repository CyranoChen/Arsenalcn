using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

using Arsenalcn.Core;

namespace iArsenal.Service
{
    [AttrDbTable("iArsenal_OrderItem", Sort = "ID DESC")]
    public class OrderItem : Entity<int>
    {
        public OrderItem() : base() { }

        public OrderItem(DataRow dr)
            : base(dr)
        {
            #region Generate OrderItem TotalPrice

            if (Sale.HasValue)
                TotalPrice = Sale.Value;
            else
                TotalPrice = UnitPrice * Quantity;

            #endregion
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

        //public static List<OrderItem> GetOrderItems(int orderID)
        //{
        //    DataTable dt = DataAccess.OrderItem.GetOrderItems(orderID);
        //    List<OrderItem> list = new List<OrderItem>();

        //    if (dt != null)
        //    {
        //        foreach (DataRow dr in dt.Rows)
        //        {
        //            list.Add(new OrderItem(dr));
        //        }
        //    }

        //    return list;
        //}

        //public static int RemoveOrderItemByOrderID(int orderID, SqlTransaction trans = null)
        //{
        //    int count = 0;

        //    List<OrderItem> list = OrderItem.GetOrderItems(orderID);

        //    if (list != null && list.Count > 0)
        //    {
        //        count = list.Count;

        //        DataAccess.OrderItem.DeleteOrderItemByOrderID(orderID, trans);
        //    }

        //    return count;
        //}

        public virtual void Place(Member m, Product p, SqlTransaction trans = null)
        {
            MemberID = m.ID;
            MemberName = m.Name;
            //oi.OrderID = o.OrderID;

            ProductGuid = p.ID;
            Code = p.Code;
            ProductName = string.Format("{0} ({1})", p.DisplayName, p.Name);
            //oi.Size = size;
            UnitPrice = p.PriceCNY;

            //oi.Quantity = quantity;
            //oi.Sale = sale;
            CreateTime = DateTime.Now;
            IsActive = true;
            //oi.Remark = remark;

            IRepository repo = new Repository();
            repo.Insert(this, trans);
        }

        #region Members and Properties

        [AttrDbColumn("MemberID")]
        public int MemberID
        { get; set; }
        
        [AttrDbColumn("MemberName")]
        public string MemberName
        { get; set; }

        [AttrDbColumn("OrderID")]
        public int OrderID
        { get; set; }

        [AttrDbColumn("ProductGuid")]
        public Guid ProductGuid
        { get; set; }

        [AttrDbColumn("Code")]
        public string Code
        { get; set; }

        [AttrDbColumn("ProductName")]
        public string ProductName
        { get; set; }

        [AttrDbColumn("Size")]
        public string Size
        { get; set; }

        [AttrDbColumn("UnitPrice")]
        public float UnitPrice
        { get; set; }

        [AttrDbColumn("Quantity")]
        public int Quantity
        { get; set; }

        [AttrDbColumn("Sale")]
        public float? Sale
        { get; set; }

        [AttrDbColumn("CreateTime")]
        public DateTime CreateTime
        { get; set; }

        [AttrDbColumn("IsActive")]
        public Boolean IsActive
        { get; set; }

        [AttrDbColumn("Remark")]
        public string Remark
        { get; set; }

        public float TotalPrice
        { get; set; }

        #endregion
    }
}
