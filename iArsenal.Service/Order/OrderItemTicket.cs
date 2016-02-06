using System;
using System.Data.SqlClient;

namespace iArsenal.Service
{
    public class OrdrItmMatchTicket : OrderItem
    {
        public void Init()
        {
            try
            {
                MatchGuid = !string.IsNullOrEmpty(Remark) ? new Guid(Remark) : Guid.Empty;
            }
            catch
            {
                throw new Exception("Can't get the Partner of OrderItem_MatchTicket.Remark.");
            }

            var _date = DateTime.MinValue;
            if (!string.IsNullOrEmpty(Size) && DateTime.TryParse(Size, out _date))
            {
                TravelDate = _date;
            }
            else
            {
                throw new Exception("Can't get the TravelDate of OrderItem_MatchTicket.Size.");
            }

            if (ProductGuid == null)
                throw new Exception("Loading OrderItem failed.");

            var p = Product.Cache.Load(ProductGuid);

            if (!p.ProductType.Equals(ProductType.MatchTicket))
                throw new Exception("The OrderItem is not the type of MatchTicket.");
        }

        public override void Place(Member m, Product p, SqlTransaction trans = null)
        {
            Remark = MatchGuid.ToString();
            Size = TravelDate.ToString("yyyy-MM-dd");

            //Product product = Product.Cache.ProductList.Find(p =>
            //    p.ProductType.Equals(ProductType.MatchTicket));

            base.Place(m, p, trans);
        }

        #region Members and Properties

        public Guid MatchGuid { get; set; }

        public DateTime TravelDate { get; set; }

        #endregion
    }

    public class OrdrItm2012TicketBeijing : OrderItem
    {
        #region Members and Properties

        public string SeatLevel { get; set; }

        #endregion

        public void Init()
        {
            SeatLevel = Size;

            if (ProductGuid == null)
                throw new Exception("Loading OrderItem failed.");

            var p = Product.Cache.Load(ProductGuid);

            if (!p.ProductType.Equals(ProductType.TicketBeijing))
                throw new Exception("The OrderItem is not the type of TicketBeijing.");
        }

        public void Place(Member m, SqlTransaction trans = null)
        {
            Size = SeatLevel;

            var product = Product.Cache.ProductList.Find(p =>
                p.ProductType.Equals(ProductType.TicketBeijing));

            base.Place(m, product, trans);
        }
    }
}