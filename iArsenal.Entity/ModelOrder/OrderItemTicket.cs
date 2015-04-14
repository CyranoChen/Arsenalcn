using System;

namespace iArsenal.Entity
{
    public class OrderItem_MatchTicket : OrderItem
    {
        public OrderItem_MatchTicket() { }

        public OrderItem_MatchTicket(int id)
            : base(id)
        {
            if (ProductGuid == null)
                throw new Exception("Loading OrderItem failed.");

            Product p = Product.Cache.Load(ProductGuid);

            if (!p.ProductType.Equals(ProductType.MatchTicket))
                throw new Exception("The OrderItem is not the type of MatchTicket.");
        }

        #region Members and Properties

        public Guid MatchGuid
        {
            get
            {
                if (!string.IsNullOrEmpty(Remark))
                {
                    try { return new Guid(Remark); }
                    catch { throw new Exception("Can't get the Partner of OrderItem_MatchTicket.Remark"); }
                }
                else
                {
                    return Guid.Empty;
                }
            }
            set
            {
                Remark = value.ToString();
            }
        }

        public DateTime? TravelDate
        {
            get
            {
                DateTime _date = DateTime.MinValue;
                if (!string.IsNullOrEmpty(Size) && DateTime.TryParse(Size, out _date))
                {
                    return _date;
                }
                else
                {
                    return null;
                }
            }
            set
            {
                if (!value.HasValue)
                {
                    Size = value.Value.ToString();
                }
                else
                {
                    Size = string.Empty;
                }
            }
        }

        #endregion
    }

    public class OrderItem_TicketBeijing : OrderItem
    {
        public OrderItem_TicketBeijing() { }

        public OrderItem_TicketBeijing(int id)
            : base(id)
        {
            if (ProductGuid == null)
                throw new Exception("Loading OrderItem failed.");

            Product p = Product.Cache.Load(ProductGuid);

            if (!p.ProductType.Equals(ProductType.TicketBeijing))
                throw new Exception("The OrderItem is not the type of TicketBeijing.");
        }

        #region Members and Properties

        public string SeatLevel
        {
            get { return Size; }
            set { Size = value; }
        }

        #endregion
    }
}
