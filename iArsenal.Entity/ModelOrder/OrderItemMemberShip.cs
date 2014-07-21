using System;

namespace iArsenal.Entity
{
    public class OrderItem_MemberShip : OrderItemBase
    {
        public OrderItem_MemberShip() { }

        public OrderItem_MemberShip(int id)
            : base(id)
        {
            if (ProductGuid == null)
                throw new Exception("Loading OrderItem failed.");

            Product p = Product.Cache.Load(ProductGuid);

            if (!p.ProductType.Equals(ProductType.MemberShipCore) && !p.ProductType.Equals(ProductType.MemberShipPremier))
                throw new Exception("The OrderItem is not the type of MemberShip.");
        }

        #region Members and Properties

        public string MemberCardNo
        {
            get { return Remark; }
            set { Remark = value; }
        }

        public DateTime EndDate
        {
            get
            {
                DateTime _tmpDate;
                if (!string.IsNullOrEmpty(Size) && DateTime.TryParse(Size, out _tmpDate))
                {
                    return _tmpDate;
                }
                else
                {
                    throw new Exception("Can't get EndDate of OrderItem_Core.Size");
                }
            }
            set { Size = value.ToString("yyyy-MM-dd"); }
        }

        public string Season
        {
            get { return string.Format("{0}/{1}", EndDate.AddYears(-1).Year.ToString(), EndDate.ToString("yy")); }
        }

        #endregion
    }

    public class OrderItem_Core : OrderItem_MemberShip
    {
        public OrderItem_Core() { }

        public OrderItem_Core(int id)
            : base(id)
        {
            if (ProductGuid == null)
                throw new Exception("Loading OrderItem failed.");

            Product p = Product.Cache.Load(ProductGuid);

            if (!p.ProductType.Equals(ProductType.MemberShipCore))
                throw new Exception("The OrderItem is not the type of MemberShipCore.");
        }
    }

    public class OrderItem_Premier : OrderItem_MemberShip
    {
        public OrderItem_Premier() { }

        public OrderItem_Premier(int id)
            : base(id)
        {
            if (ProductGuid == null)
                throw new Exception("Loading OrderItem failed.");

            Product p = Product.Cache.Load(ProductGuid);

            if (!p.ProductType.Equals(ProductType.MemberShipPremier))
                throw new Exception("The OrderItem is not the type of MemberShipPremier.");
        }
    }
}
