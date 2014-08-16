using System;

namespace iArsenal.Entity
{
    public class OrderItem_ReplicaKitHome : OrderItemBase
    {
        public OrderItem_ReplicaKitHome() { }

        public OrderItem_ReplicaKitHome(int id)
            : base(id)
        {
            if (ProductGuid == null)
                throw new Exception("Loading OrderItem failed.");

            Product p = Product.Cache.Load(ProductGuid);

            if (!p.ProductType.Equals(ProductType.ReplicaKitHome))
                throw new Exception("The OrderItem is not the type of ReplicaKitHome.");
        }
    }

    public class OrderItem_ReplicaKitAway : OrderItemBase
    {
        public OrderItem_ReplicaKitAway() { }

        public OrderItem_ReplicaKitAway(int id)
            : base(id)
        {
            if (ProductGuid == null)
                throw new Exception("Loading OrderItem failed.");

            Product p = Product.Cache.Load(ProductGuid);

            if (!p.ProductType.Equals(ProductType.ReplicaKitAway))
                throw new Exception("The OrderItem is not the type of ReplicaKitAway.");
        }
    }

    public class OrderItem_ReplicaKitCup : OrderItemBase
    {
        public OrderItem_ReplicaKitCup() { }

        public OrderItem_ReplicaKitCup(int id)
            : base(id)
        {
            if (ProductGuid == null)
                throw new Exception("Loading OrderItem failed.");

            Product p = Product.Cache.Load(ProductGuid);

            if (!p.ProductType.Equals(ProductType.ReplicaKitCup))
                throw new Exception("The OrderItem is not the type of ReplicaKitCup.");
        }
    }

    public class OrderItem_PlayerNumber : OrderItemBase
    {
        public OrderItem_PlayerNumber() { }

        public OrderItem_PlayerNumber(int id)
            : base(id)
        {
            if (ProductGuid == null)
                throw new Exception("Loading OrderItem failed.");

            Product p = Product.Cache.Load(ProductGuid);

            if (!p.ProductType.Equals(ProductType.PlayerNumber))
                throw new Exception("The OrderItem is not the type of PlayerNumber.");
        }

        #region Members and Properties

        public string PrintingNumber
        {
            get { return Size; }
            set { Size = value; }
        }

        public Guid ArsenalPlayerGuid
        {
            get
            {
                if (!string.IsNullOrEmpty(Remark))
                {
                    try { return new Guid(Remark); }
                    catch { throw new Exception("Can't get the Partner of OrderItem_PlayerNumber.Remark"); }
                }
                else
                {
                    return Guid.Empty;
                }
            }
        }

        #endregion
    }

    public class OrderItem_PlayerName : OrderItemBase
    {
        public OrderItem_PlayerName() { }

        public OrderItem_PlayerName(int id)
            : base(id)
        {
            if (ProductGuid == null)
                throw new Exception("Loading OrderItem failed.");

            Product p = Product.Cache.Load(ProductGuid);

            if (!p.ProductType.Equals(ProductType.PlayerName))
                throw new Exception("The OrderItem is not the type of PlayerName.");
        }

        #region Members and Properties

        public string PrintingName
        {
            get { return Size; }
            set { Size = value; }
        }

        public Guid ArsenalPlayerGuid
        {
            get
            {
                if (!string.IsNullOrEmpty(Remark))
                {
                    try { return new Guid(Remark); }
                    catch { throw new Exception("Can't get the Partner of OrderItem_PlayerName.Remark"); }
                }
                else
                {
                    return Guid.Empty;
                }
            }
        }

        #endregion
    }

    public class OrderItem_ArsenalFont : OrderItemBase
    {
        public OrderItem_ArsenalFont() { }

        public OrderItem_ArsenalFont(int id)
            : base(id)
        {
            if (ProductGuid == null)
                throw new Exception("Loading OrderItem failed.");

            Product p = Product.Cache.Load(ProductGuid);

            if (!p.ProductType.Equals(ProductType.ArsenalFont))
                throw new Exception("The OrderItem is not the type of ArsenalFont.");
        }
    }

    public class OrderItem_PremiershipPatch : OrderItemBase
    {
        public OrderItem_PremiershipPatch() { }

        public OrderItem_PremiershipPatch(int id)
            : base(id)
        {
            if (ProductGuid == null)
                throw new Exception("Loading OrderItem failed.");

            Product p = Product.Cache.Load(ProductGuid);

            if (!p.ProductType.Equals(ProductType.PremiershipPatch))
                throw new Exception("The OrderItem is not the type of PremiershipPatch.");
        }
    }

    public class OrderItem_ChampionshipPatch : OrderItemBase
    {
        public OrderItem_ChampionshipPatch() { }

        public OrderItem_ChampionshipPatch(int id)
            : base(id)
        {
            if (ProductGuid == null)
                throw new Exception("Loading OrderItem failed.");

            Product p = Product.Cache.Load(ProductGuid);

            if (!p.ProductType.Equals(ProductType.ChampionshipPatch))
                throw new Exception("The OrderItem is not the type of ChampionshipPatch.");
        }
    }
}
