using System;
using System.Collections.Generic;
using System.Data;

namespace iArsenal.Entity
{
    public class Product
    {
        public Product() { }

        private Product(DataRow dr)
        {
            InitProduct(dr);
        }

        private void InitProduct(DataRow dr)
        {
            if (dr != null)
            {
                ProductGuid = (Guid)dr["ProductGuid"];
                Code = dr["Code"].ToString();
                Name = dr["Name"].ToString();
                DisplayName = dr["DisplayName"].ToString();
                ProductType = (ProductType)Enum.Parse(typeof(ProductType), dr["ProductType"].ToString());
                ImageURL = dr["ImageURL"].ToString();
                Material = dr["Material"].ToString();
                Colour = dr["Colour"].ToString();

                if (!string.IsNullOrEmpty(dr["Size"].ToString()))
                    Size = (ProductSizeType)Enum.Parse(typeof(ProductSizeType), dr["Size"].ToString());
                else
                    Size = ProductSizeType.Null;

                Currency = (ProductCurrencyType)Enum.Parse(typeof(ProductCurrencyType), dr["Currency"].ToString());
                Price = Convert.ToSingle(dr["Price"]);

                if (!Convert.IsDBNull(dr["Sale"]))
                    Sale = Convert.ToSingle(dr["Sale"]);
                else
                    Sale = null;

                CreateTime = (DateTime)dr["CreateTime"];
                Stock = Convert.ToInt32(dr["Stock"]);
                IsActive = Convert.ToBoolean(dr["IsActive"]);
                Description = dr["Description"].ToString();
                Remark = dr["Remark"].ToString();

                #region Generate Product Type Info

                switch (ProductType)
                {
                    case ProductType.ReplicaKitHome:
                        ProductTypeInfo = "主场球衣";
                        break;
                    case ProductType.ReplicaKitAway:
                        ProductTypeInfo = "客场球衣";
                        break;
                    case ProductType.PlayerName:
                        ProductTypeInfo = "印名字";
                        break;
                    case ProductType.PlayerNumber:
                        ProductTypeInfo = "印号码";
                        break;
                    case ProductType.ArsenalFont:
                        ProductTypeInfo = "特殊字体";
                        break;
                    case ProductType.PremiershipPatch:
                        ProductTypeInfo = "联赛袖标";
                        break;
                    case ProductType.ChampionshipPatch:
                        ProductTypeInfo = "欧冠袖标";
                        break;
                    case ProductType.TravelPlan:
                        ProductTypeInfo = "观赛计划";
                        break;
                    case ProductType.TravelPartner:
                        ProductTypeInfo = "观赛同伴";
                        break;
                    case ProductType.MatchTicket:
                        ProductTypeInfo = "主场球票";
                        break;
                    case ProductType.TicketBeijing:
                        ProductTypeInfo = "友谊赛球票";
                        break;
                    case ProductType.MemberShipCore:
                        ProductTypeInfo = "会员费(Core)";
                        break;
                    case ProductType.MemberShipPremier:
                        ProductTypeInfo = "会员费(Premier)";
                        break;
                    case ProductType.Other:
                        ProductTypeInfo = "/";
                        break;
                    default:
                        ProductTypeInfo = string.Empty;
                        break;
                }

                #endregion

                #region Generate CurrencyInfo Product PriceInfo & SaleInfo & PriceCNY

                float _unitPrice = default(float);

                if (Sale.HasValue)
                {
                    _unitPrice = Sale.Value;
                }
                else
                {
                    _unitPrice = Price;
                }

                switch (Currency)
                {
                    case ProductCurrencyType.GBP:
                        CurrencyInfo = "￡";
                        PriceCNY = _unitPrice * ConfigGlobal.ExchangeRateGBP;
                        break;
                    case ProductCurrencyType.CNY:
                        CurrencyInfo = "￥";
                        PriceCNY = _unitPrice;
                        break;
                    case ProductCurrencyType.USD:
                        CurrencyInfo = "＄";
                        PriceCNY = _unitPrice * ConfigGlobal.ExchangeRateUSD;
                        break;
                }

                if (Price >= 0)
                {
                    PriceInfo = CurrencyInfo + Price.ToString("f2");
                }
                else
                { PriceInfo = string.Empty; }

                if (Sale.HasValue && Sale >= 0)
                {
                    SaleInfo = CurrencyInfo + Sale.Value.ToString("f2");
                }
                else
                { SaleInfo = string.Empty; }

                #endregion

            }
            else
                throw new Exception("Unable to init Product.");
        }

        public void Select()
        {
            DataRow dr = DataAccess.Product.GetProductByID(ProductGuid);

            if (dr != null)
                InitProduct(dr);
        }

        public void Update()
        {
            string _size = string.Empty;
            if (Size != ProductSizeType.Null)
                _size = Size.ToString();

            DataAccess.Product.UpdateProduct(ProductGuid, Code, Name, DisplayName, (int)ProductType, ImageURL, Material, Colour, _size, Currency.ToString(), Price, Sale, CreateTime, Stock, IsActive, Description, Remark);
        }

        public void Insert()
        {
            string _size = string.Empty;
            if (Size != ProductSizeType.Null)
                _size = Size.ToString();

            DataAccess.Product.InsertProduct(ProductGuid, Code, Name, DisplayName, (int)ProductType, ImageURL, Material, Colour, _size, Currency.ToString(), Price, Sale, CreateTime, Stock, IsActive, Description, Remark);
        }

        public void Delete()
        {
            DataAccess.Product.DeleteProduct(ProductGuid);
        }

        public static List<Product> GetProducts()
        {
            DataTable dt = DataAccess.Product.GetProducts();
            List<Product> list = new List<Product>();

            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    list.Add(new Product(dr));
                }
            }

            return list;
        }

        public static class Cache
        {
            static Cache()
            {
                InitCache();
            }

            public static void RefreshCache()
            {
                InitCache();
            }

            private static void InitCache()
            {
                ProductList = GetProducts();
            }

            public static Product Load(Guid guid)
            {
                return ProductList.Find(p => p.ProductGuid.Equals(guid));
            }

            public static Product Load(string code)
            {
                return ProductList.Find(p => p.Code.Equals(code, StringComparison.OrdinalIgnoreCase));
            }

            public static List<Product> Load(ProductType pt)
            {
                return ProductList.FindAll(p => p.ProductType.Equals(pt));
            }

            //private static List<Product> GetActiveProductListByCodes(string[] availableCodes)
            //{
            //    if (availableCodes == null)
            //    {
            //        throw new ArgumentNullException("available code is null");
            //    }

            //    if (ProductList == null)
            //    {
            //        throw new Exception("product list is null");
            //    }

            //    List<Product> list = new List<Product>();
            //    foreach (string c in availableCodes)
            //    {
            //        var p = Load(c);
            //        if (p != null && p.IsActive)
            //            list.Add(p);
            //    }

            //    return list;
            //}

            public static List<Product> ProductList;
        }

        #region Members and Properties

        public Guid ProductGuid
        { get; set; }

        public string Code
        { get; set; }

        public string Name
        { get; set; }

        public string DisplayName
        { get; set; }

        public ProductType ProductType
        { get; set; }

        public string ImageURL
        { get; set; }

        public string Material
        { get; set; }

        public string Colour
        { get; set; }

        public ProductSizeType Size
        { get; set; }

        public ProductCurrencyType Currency
        { get; set; }

        public float Price
        { get; set; }

        public float? Sale
        { get; set; }

        public DateTime CreateTime
        { get; set; }

        public int Stock
        { get; set; }

        public Boolean IsActive
        { get; set; }

        public string Description
        { get; set; }

        public string Remark
        { get; set; }

        public string ProductTypeInfo
        { get; set; }

        public string CurrencyInfo
        { get; set; }

        public float PriceCNY
        { get; set; }

        public string PriceInfo
        { get; set; }

        public string SaleInfo
        { get; set; }

        #endregion
    }

    public enum ProductType
    {
        ReplicaKitHome = 1,
        ReplicaKitAway = 2,
        PlayerName = 3,
        PlayerNumber = 4,
        ArsenalFont = 5,
        PremiershipPatch = 6,
        ChampionshipPatch = 7,
        TravelPlan = 10,
        TravelPartner = 11,
        MatchTicket = 20,
        TicketBeijing = 21,
        MemberShipCore = 31,
        MemberShipPremier = 32,
        Other = 0
    }

    public enum ProductSizeType
    {
        Null,
        Childrens,
        Adults,
        Infants,
        MiniKit,
        Ladies
    }

    public enum ProductCurrencyType
    {
        GBP,
        CNY,
        USD
    }
}
