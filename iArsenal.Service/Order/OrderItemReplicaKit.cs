using System;
using System.Data;

namespace iArsenal.Service
{
    public class OrdrItmReplicaKit : OrderItem
    {
        public OrdrItmReplicaKit() { }

        public OrdrItmReplicaKit(DataRow dr) : base(dr) { }
    }

    public class OrdrItmReplicaKitHome : OrdrItmReplicaKit
    {
        public OrdrItmReplicaKitHome() { }

        public OrdrItmReplicaKitHome(DataRow dr) : base(dr) { this.Init(); }

        private void Init()
        {
            if (ProductGuid == null)
                throw new Exception("Loading OrderItem failed.");

            Product p = Product.Cache.Load(ProductGuid);

            if (!p.ProductType.Equals(ProductType.ReplicaKitHome))
                throw new Exception("The OrderItem is not the type of ReplicaKitHome.");
        }

        public override void Mapper(object obj)
        {
            base.Mapper(obj);
            this.Init();
        }

        public void Place(Member m, System.Data.SqlClient.SqlTransaction trans = null)
        {
            Product product = Product.Cache.ProductList.Find(p =>
                p.ProductType.Equals(ProductType.ReplicaKitHome));

            base.Place(m, product, trans);
        }
    }

    public class OrdrItmReplicaKitAway : OrdrItmReplicaKit
    {
        public OrdrItmReplicaKitAway() { }

        public OrdrItmReplicaKitAway(DataRow dr) : base(dr) { this.Init(); }

        private void Init()
        {
            if (ProductGuid == null)
                throw new Exception("Loading OrderItem failed.");

            Product p = Product.Cache.Load(ProductGuid);

            if (!p.ProductType.Equals(ProductType.ReplicaKitAway))
                throw new Exception("The OrderItem is not the type of ReplicaKitAway.");
        }

        public override void Mapper(object obj)
        {
            base.Mapper(obj);
            this.Init();
        }

        public void Place(Member m, System.Data.SqlClient.SqlTransaction trans = null)
        {
            Product product = Product.Cache.ProductList.Find(p =>
                p.ProductType.Equals(ProductType.ReplicaKitAway));

            base.Place(m, product, trans);
        }
    }

    public class OrdrItmReplicaKitCup : OrdrItmReplicaKit
    {
        public OrdrItmReplicaKitCup() { }

        public OrdrItmReplicaKitCup(DataRow dr) : base(dr) { this.Init(); }

        private void Init()
        {
            if (ProductGuid == null)
                throw new Exception("Loading OrderItem failed.");

            Product p = Product.Cache.Load(ProductGuid);

            if (!p.ProductType.Equals(ProductType.ReplicaKitCup))
                throw new Exception("The OrderItem is not the type of ReplicaKitCup.");
        }

        public override void Mapper(object obj)
        {
            base.Mapper(obj);
            this.Init();
        }

        public void Place(Member m, System.Data.SqlClient.SqlTransaction trans = null)
        {
            Product product = Product.Cache.ProductList.Find(p =>
                p.ProductType.Equals(ProductType.ReplicaKitCup));

            base.Place(m, product, trans);
        }
    }

    public class OrdrItmPlayerNumber : OrderItem
    {
        public OrdrItmPlayerNumber() { }

        public OrdrItmPlayerNumber(DataRow dr) : base(dr) { this.Init(); }

        private void Init()
        {
            if (ProductGuid == null)
                throw new Exception("Loading OrderItem failed.");

            Product p = Product.Cache.Load(ProductGuid);

            if (!p.ProductType.Equals(ProductType.PlayerNumber))
                throw new Exception("The OrderItem is not the type of PlayerNumber.");
        }

        public override void Mapper(object obj)
        {
            base.Mapper(obj);
            this.Init();
        }

        public void Place(Member m, System.Data.SqlClient.SqlTransaction trans = null)
        {
            Product product = Product.Cache.ProductList.Find(p =>
                p.ProductType.Equals(ProductType.PlayerNumber));

            base.Place(m, product, trans);
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
                else
                {
                    return Guid.Empty;
                }
            }
        }

        #endregion
    }

    public class OrdrItmPlayerName : OrderItem
    {
        public OrdrItmPlayerName() { }

        public OrdrItmPlayerName(DataRow dr) : base(dr) { this.Init(); }

        private void Init()
        {
            if (ProductGuid == null)
                throw new Exception("Loading OrderItem failed.");

            Product p = Product.Cache.Load(ProductGuid);

            if (!p.ProductType.Equals(ProductType.PlayerName))
                throw new Exception("The OrderItem is not the type of PlayerName.");
        }

        public override void Mapper(object obj)
        {
            base.Mapper(obj);
            this.Init();
        }

        public void Place(Member m, System.Data.SqlClient.SqlTransaction trans = null)
        {
            Product product = Product.Cache.ProductList.Find(p =>
                p.ProductType.Equals(ProductType.PlayerName));

            base.Place(m, product, trans);
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
                else
                {
                    return Guid.Empty;
                }
            }
        }

        #endregion
    }

    public class OrdrItmArsenalFont : OrderItem
    {
        public OrdrItmArsenalFont() { }

        public OrdrItmArsenalFont(DataRow dr) : base(dr) { this.Init(); }

        private void Init()
        {
            if (ProductGuid == null)
                throw new Exception("Loading OrderItem failed.");

            Product p = Product.Cache.Load(ProductGuid);

            if (!p.ProductType.Equals(ProductType.ArsenalFont))
                throw new Exception("The OrderItem is not the type of ArsenalFont.");
        }

        public override void Mapper(object obj)
        {
            base.Mapper(obj);
            this.Init();
        }

        public void Place(Member m, System.Data.SqlClient.SqlTransaction trans = null)
        {
            Product product = Product.Cache.ProductList.Find(p =>
                p.ProductType.Equals(ProductType.ArsenalFont));

            base.Place(m, product, trans);
        }
    }

    public class OrdrItmPremiershipPatch : OrderItem
    {
        public OrdrItmPremiershipPatch() { }

        public OrdrItmPremiershipPatch(DataRow dr) : base(dr) { this.Init(); }

        private void Init()
        {
            if (ProductGuid == null)
                throw new Exception("Loading OrderItem failed.");

            Product p = Product.Cache.Load(ProductGuid);

            if (!p.ProductType.Equals(ProductType.PremiershipPatch))
                throw new Exception("The OrderItem is not the type of PremiershipPatch.");
        }

        public override void Mapper(object obj)
        {
            base.Mapper(obj);
            this.Init();
        }

        public void Place(Member m, System.Data.SqlClient.SqlTransaction trans = null)
        {
            Product product = Product.Cache.ProductList.Find(p =>
                p.ProductType.Equals(ProductType.PremiershipPatch));

            base.Place(m, product, trans);
        }
    }

    public class OrdrItmChampionshipPatch : OrderItem
    {
        public OrdrItmChampionshipPatch() { }

        public OrdrItmChampionshipPatch(DataRow dr) : base(dr) { this.Init(); }

        private void Init()
        {
            if (ProductGuid == null)
                throw new Exception("Loading OrderItem failed.");

            Product p = Product.Cache.Load(ProductGuid);

            if (!p.ProductType.Equals(ProductType.ChampionshipPatch))
                throw new Exception("The OrderItem is not the type of ChampionshipPatch.");
        }

        public override void Mapper(object obj)
        {
            base.Mapper(obj);
            this.Init();
        }

        public void Place(Member m, System.Data.SqlClient.SqlTransaction trans = null)
        {
            Product product = Product.Cache.ProductList.Find(p =>
                p.ProductType.Equals(ProductType.ChampionshipPatch));

            base.Place(m, product, trans);
        }
    }
}
