using System;

namespace iArsenal.Service
{
    public class OrdrItmReplicaKit : OrderItem
    {
    }

    public class OrdrItmReplicaKitHome : OrdrItmReplicaKit
    {
        public void Init()
        {
            if (ProductGuid == null)
                throw new Exception("Loading OrderItem failed.");

            var p = Product.Cache.Load(ProductGuid);

            if (!p.ProductType.Equals(ProductType.ReplicaKitHome))
                throw new Exception("The OrderItem is not the type of ReplicaKitHome.");
        }

        public void Place(Member m)
        {
            var product = Product.Cache.ProductList.Find(p =>
                p.ProductType.Equals(ProductType.ReplicaKitHome));

            base.Place(m, product);
        }
    }

    public class OrdrItmReplicaKitAway : OrdrItmReplicaKit
    {
        public void Init()
        {
            if (ProductGuid == null)
                throw new Exception("Loading OrderItem failed.");

            var p = Product.Cache.Load(ProductGuid);

            if (!p.ProductType.Equals(ProductType.ReplicaKitAway))
                throw new Exception("The OrderItem is not the type of ReplicaKitAway.");
        }

        public void Place(Member m)
        {
            var product = Product.Cache.ProductList.Find(p =>
                p.ProductType.Equals(ProductType.ReplicaKitAway));

            base.Place(m, product);
        }
    }

    public class OrdrItmReplicaKitCup : OrdrItmReplicaKit
    {
        public void Init()
        {
            if (ProductGuid == null)
                throw new Exception("Loading OrderItem failed.");

            var p = Product.Cache.Load(ProductGuid);

            if (!p.ProductType.Equals(ProductType.ReplicaKitCup))
                throw new Exception("The OrderItem is not the type of ReplicaKitCup.");
        }

        public void Place(Member m)
        {
            var product = Product.Cache.ProductList.Find(p =>
                p.ProductType.Equals(ProductType.ReplicaKitCup));

            base.Place(m, product);
        }
    }

    public class OrdrItmPlayerNumber : OrderItem
    {
        public void Init()
        {
            if (ProductGuid == null)
                throw new Exception("Loading OrderItem failed.");

            var p = Product.Cache.Load(ProductGuid);

            if (!p.ProductType.Equals(ProductType.PlayerNumber))
                throw new Exception("The OrderItem is not the type of PlayerNumber.");
        }

        public void Place(Member m)
        {
            var product = Product.Cache.ProductList.Find(p =>
                p.ProductType.Equals(ProductType.PlayerNumber));

            base.Place(m, product);
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
                    return new Guid(Remark);
                }
                return Guid.Empty;
            }
            set { Remark = value.ToString(); }
        }

        #endregion
    }

    public class OrdrItmPlayerName : OrderItem
    {
        public void Init()
        {
            if (ProductGuid == null)
                throw new Exception("Loading OrderItem failed.");

            var p = Product.Cache.Load(ProductGuid);

            if (!p.ProductType.Equals(ProductType.PlayerName))
                throw new Exception("The OrderItem is not the type of PlayerName.");
        }

        public void Place(Member m)
        {
            var product = Product.Cache.ProductList.Find(p =>
                p.ProductType.Equals(ProductType.PlayerName));

            base.Place(m, product);
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
                    return new Guid(Remark);
                }
                return Guid.Empty;
            }
            set { Remark = value.ToString(); }
        }

        #endregion
    }

    public class OrdrItmArsenalFont : OrderItem
    {
        public void Init()
        {
            if (ProductGuid == null)
                throw new Exception("Loading OrderItem failed.");

            var p = Product.Cache.Load(ProductGuid);

            if (!p.ProductType.Equals(ProductType.ArsenalFont))
                throw new Exception("The OrderItem is not the type of ArsenalFont.");
        }

        public void Place(Member m)
        {
            var product = Product.Cache.ProductList.Find(p =>
                p.ProductType.Equals(ProductType.ArsenalFont));

            base.Place(m, product);
        }
    }

    public class OrdrItmPremiershipPatch : OrderItem
    {
        public void Init()
        {
            if (ProductGuid == null)
                throw new Exception("Loading OrderItem failed.");

            var p = Product.Cache.Load(ProductGuid);

            if (!p.ProductType.Equals(ProductType.PremiershipPatch))
                throw new Exception("The OrderItem is not the type of PremiershipPatch.");
        }

        public void Place(Member m)
        {
            var product = Product.Cache.ProductList.Find(p =>
                p.ProductType.Equals(ProductType.PremiershipPatch));

            base.Place(m, product);
        }
    }

    public class OrdrItmChampionshipPatch : OrderItem
    {
        public void Init()
        {
            if (ProductGuid == null)
                throw new Exception("Loading OrderItem failed.");

            var p = Product.Cache.Load(ProductGuid);

            if (!p.ProductType.Equals(ProductType.ChampionshipPatch))
                throw new Exception("The OrderItem is not the type of ChampionshipPatch.");
        }

        public void Place(Member m)
        {
            var product = Product.Cache.ProductList.Find(p =>
                p.ProductType.Equals(ProductType.ChampionshipPatch));

            base.Place(m, product);
        }
    }
}