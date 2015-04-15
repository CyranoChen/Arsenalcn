using System;

namespace iArsenal.Entity
{
    public class OrdrItmMatchTicket : OrderItem
    {
        public OrdrItmMatchTicket() { }

        public OrdrItmMatchTicket(int id) : base(id) { this.Init(); }

        private void Init()
        {
            try { MatchGuid = !string.IsNullOrEmpty(Remark) ? new Guid(Remark) : Guid.Empty; }
            catch { throw new Exception("Can't get the Partner of OrderItem_MatchTicket.Remark."); }

            DateTime _date = DateTime.MinValue;
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

            Product p = Product.Cache.Load(ProductGuid);

            if (!p.ProductType.Equals(ProductType.MatchTicket))
                throw new Exception("The OrderItem is not the type of MatchTicket.");
        }

        public override void Mapper(object obj)
        {
            base.Mapper(obj);
            this.Init();
        }

        public void Place(Member m, System.Data.SqlClient.SqlTransaction trans = null)
        {
            this.Remark = MatchGuid.ToString();
            this.Size = TravelDate.ToString("yyyy-MM-dd");

            Product product = Product.Cache.ProductList.Find(p =>
                p.ProductType.Equals(ProductType.MatchTicket));

            base.Place(m, product, trans);
        }

        #region Members and Properties

        public Guid MatchGuid { get; set; }

        public DateTime TravelDate { get; set; }

        #endregion
    }

    public class OrdrItm2012TicketBeijing : OrderItem
    {
        public OrdrItm2012TicketBeijing() { }

        public OrdrItm2012TicketBeijing(int id) : base(id) { this.Init(); }

        private void Init()
        {
            this.SeatLevel = Size;

            if (ProductGuid == null)
                throw new Exception("Loading OrderItem failed.");

            Product p = Product.Cache.Load(ProductGuid);

            if (!p.ProductType.Equals(ProductType.TicketBeijing))
                throw new Exception("The OrderItem is not the type of TicketBeijing.");
        }

        public override void Mapper(object obj)
        {
            base.Mapper(obj);
            this.Init();
        }

        public void Place(Member m, System.Data.SqlClient.SqlTransaction trans = null)
        {
            this.Size = SeatLevel;

            Product product = Product.Cache.ProductList.Find(p =>
                p.ProductType.Equals(ProductType.TicketBeijing));

            base.Place(m, product, trans);
        }

        #region Members and Properties

        public string SeatLevel { get; set; }

        #endregion
    }
}
