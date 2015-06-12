using System;
using System.Data;

namespace iArsenal.Service
{
    public class OrdrItmMemberShip : OrderItem
    {
        public OrdrItmMemberShip() { }

        public OrdrItmMemberShip(DataRow dr) : base(dr) { this.Init(); }

        protected void Init()
        {
            String[] _para = Remark.Split('|');

            MemberCardNo = !string.IsNullOrEmpty(_para[0]) ? _para[0] : string.Empty;

            if (_para.Length > 1)
            {
                AlterMethod = !string.IsNullOrEmpty(_para[1]) ? _para[1] : string.Empty;
            }
            else
            {
                AlterMethod = string.Empty;
            }

            DateTime _date;
            if (!string.IsNullOrEmpty(Size) && DateTime.TryParse(Size, out _date))
            {
                EndDate = _date;
            }
            else
            {
                throw new Exception("Can't get EndDate of OrdrItmMemShip.Size");
            }

            Season = string.Format("{0}/{1}", EndDate.AddYears(-1).Year.ToString(), EndDate.ToString("yy"));
        }

        public override void Place(Member m, Product p, System.Data.SqlClient.SqlTransaction trans = null)
        {
            if (!string.IsNullOrEmpty(AlterMethod))
            {
                this.Remark = string.Format("{0}|{1}", MemberCardNo, AlterMethod);
            }
            else
            {
                this.Remark = MemberCardNo;
            }

            this.Size = EndDate.ToString("yyyy-MM-dd");

            base.Place(m, p, trans);
        }

        #region Members and Properties

        public string MemberCardNo { get; set; }

        public string AlterMethod { get; set; }

        public DateTime EndDate { get; set; }

        public string Season { get; private set; }

        #endregion
    }

    public class OrdrItmMemShipCore : OrdrItmMemberShip
    {
        public OrdrItmMemShipCore() { }

        public OrdrItmMemShipCore(DataRow dr) : base(dr) { this.Init(); }

        private new void Init()
        {
            base.Init();

            if (ProductGuid == null)
                throw new Exception("Loading OrderItem failed.");

            Product p = Product.Cache.Load(ProductGuid);

            if (!p.ProductType.Equals(ProductType.MemberShipCore))
                throw new Exception("The OrderItem is not the type of MemberShipCore.");
        }

        public override void Mapper(object obj)
        {
            base.Mapper(obj);
            this.Init();
        }

        public void Place(Member m, System.Data.SqlClient.SqlTransaction trans = null)
        {
            Product product = Product.Cache.ProductList.Find(p =>
                p.ProductType.Equals(ProductType.MemberShipCore));

            base.Place(m, product, trans);
        }
    }

    public class OrdrItmMemShipPremier : OrdrItmMemberShip
    {
        public OrdrItmMemShipPremier() { }

        public OrdrItmMemShipPremier(DataRow dr) : base(dr) { this.Init(); }

        private new void Init()
        {
            base.Init();

            if (ProductGuid == null)
                throw new Exception("Loading OrderItem failed.");

            Product p = Product.Cache.Load(ProductGuid);

            if (!p.ProductType.Equals(ProductType.MemberShipPremier))
                throw new Exception("The OrderItem is not the type of MemberShipPremier.");
        }

        public override void Mapper(object obj)
        {
            base.Mapper(obj);
            this.Init();
        }

        public void Place(Member m, System.Data.SqlClient.SqlTransaction trans = null)
        {
            Product product = Product.Cache.ProductList.Find(p =>
                p.ProductType.Equals(ProductType.MemberShipPremier));

            base.Place(m, product, trans);
        }
    }
}
