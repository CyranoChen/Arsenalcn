using System;
using System.Data.SqlClient;

namespace iArsenal.Service
{
    public class OrdrItmMembership : OrderItem
    {
        public void Init()
        {
            var para = Remark.Split('|');

            MemberCardNo = !string.IsNullOrEmpty(para[0]) ? para[0] : string.Empty;

            if (para.Length > 1)
            {
                AlterMethod = !string.IsNullOrEmpty(para[1]) ? para[1] : string.Empty;
            }
            else
            {
                AlterMethod = string.Empty;
            }

            DateTime date;
            if (!string.IsNullOrEmpty(Size) && DateTime.TryParse(Size, out date))
            {
                EndDate = date;
            }
            else
            {
                throw new Exception("Can't get EndDate of OrdrItmMemShip.Size");
            }

            Season = $"{EndDate.AddYears(-1).Year}/{EndDate.ToString("yy")}";
        }

        public override void Place(Member m, Product p, SqlTransaction trans = null)
        {
            Remark = !string.IsNullOrEmpty(AlterMethod) ? $"{MemberCardNo}|{AlterMethod}" : MemberCardNo;

            Size = EndDate.ToString("yyyy-MM-dd");

            base.Place(m, p, trans);
        }

        #region Members and Properties

        public string MemberCardNo { get; set; }

        public string AlterMethod { get; set; }

        public DateTime EndDate { get; set; }

        public string Season { get; private set; }

        #endregion
    }

    public class OrdrItmMemShipCore : OrdrItmMembership
    {
        public new void Init()
        {
            base.Init();

            if (ProductGuid == null)
                throw new Exception("Loading OrderItem failed.");

            var p = Product.Cache.Load(ProductGuid);

            if (!p.ProductType.Equals(ProductType.MembershipCore))
                throw new Exception("The OrderItem is not the type of MembershipCore.");
        }

        public void Place(Member m, SqlTransaction trans = null)
        {
            var product = Product.Cache.ProductList.Find(p =>
                p.ProductType.Equals(ProductType.MembershipCore));

            base.Place(m, product, trans);
        }
    }

    public class OrdrItmMemShipPremier : OrdrItmMembership
    {
        public new void Init()
        {
            base.Init();

            if (ProductGuid == null)
                throw new Exception("Loading OrderItem failed.");

            var p = Product.Cache.Load(ProductGuid);

            if (!p.ProductType.Equals(ProductType.MembershipPremier))
                throw new Exception("The OrderItem is not the type of MembershipPremier.");
        }

        public void Place(Member m, SqlTransaction trans = null)
        {
            var product = Product.Cache.ProductList.Find(p =>
                p.ProductType.Equals(ProductType.MembershipPremier));

            base.Place(m, product, trans);
        }
    }
}