using System;
using System.Data;
using System.Data.SqlClient;
using Arsenalcn.Core;
using DataReaderMapper;

namespace iArsenal.Service
{
    [DbSchema("iArsenal_OrderItem", Sort = "ID DESC")]
    public class OrderItem : Entity<int>
    {
        public static void CreateMap()
        {
            var map = Mapper.CreateMap<IDataReader, OrderItem>();

            map.ForMember(d => d.TotalPrice, opt => opt.ResolveUsing(s =>
            {
                #region Generate OrderItem TotalPrice

                var sale = (double?) s.GetValue("Sale");

                if (sale.HasValue)
                    return sale.Value;
                return (double) s.GetValue("UnitPrice")*(int) s.GetValue("Quantity");

                #endregion
            }));
        }

        public virtual void Place(Member m, Product p, SqlTransaction trans = null)
        {
            MemberID = m.ID;
            MemberName = m.Name;
            //oi.OrderID = o.OrderID;

            ProductGuid = p.ID;
            Code = p.Code;
            ProductName = $"{p.DisplayName} ({p.Name})";
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

        [DbColumn("MemberID")]
        public int MemberID { get; set; }

        [DbColumn("MemberName")]
        public string MemberName { get; set; }

        [DbColumn("OrderID")]
        public int OrderID { get; set; }

        [DbColumn("ProductGuid")]
        public Guid ProductGuid { get; set; }

        [DbColumn("Code")]
        public string Code { get; set; }

        [DbColumn("ProductName")]
        public string ProductName { get; set; }

        [DbColumn("Size")]
        public string Size { get; set; }

        [DbColumn("UnitPrice")]
        public double UnitPrice { get; set; }

        [DbColumn("Quantity")]
        public int Quantity { get; set; }

        [DbColumn("Sale")]
        public double? Sale { get; set; }

        [DbColumn("CreateTime")]
        public DateTime CreateTime { get; set; }

        [DbColumn("IsActive")]
        public bool IsActive { get; set; }

        [DbColumn("Remark")]
        public string Remark { get; set; }

        public double TotalPrice { get; set; }

        #endregion
    }
}