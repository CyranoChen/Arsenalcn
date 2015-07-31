using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

using Arsenalcn.Core;

namespace iArsenal.Service
{
    [DbSchema("iArsenal_Product", Key = "ProductGuid", Sort = "Code")]
    public class Product : Entity<Guid>
    {
        public Product() : base() { }

        public Product(DataRow dr)
        {
            #region Generate Product Type Info

            switch (ProductType)
            {
                case ProductType.ReplicaKitHome:
                    ProductTypeInfo = "主场球衣";
                    break;
                case ProductType.ReplicaKitAway:
                    ProductTypeInfo = "客场球衣";
                    break;
                case ProductType.ReplicaKitCup:
                    ProductTypeInfo = "杯赛球衣";
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
                IRepository repo = new Repository();

                ProductList = repo.All<Product>().ToList();
            }

            public static Product Load(Guid guid)
            {
                return ProductList.Find(x => x.ID.Equals(guid));
            }

            public static Product Load(string code)
            {
                return ProductList.Find(x => x.Code.Equals(code, StringComparison.OrdinalIgnoreCase));
            }

            public static List<Product> Load(ProductType pt)
            {
                return ProductList.FindAll(x => x.ProductType.Equals(pt));
            }

            public static List<Product> ProductList;
        }

        #region Members and Properties

        [DbColumn("Code")]
        public string Code
        { get; set; }

        [DbColumn("Name")]
        public string Name
        { get; set; }

        [DbColumn("DisplayName")]
        public string DisplayName
        { get; set; }

        [DbColumn("ProductType")]
        public ProductType ProductType
        { get; set; }

        [DbColumn("ImageURL")]
        public string ImageURL
        { get; set; }

        [DbColumn("Material")]
        public string Material
        { get; set; }

        [DbColumn("Colour")]
        public string Colour
        { get; set; }

        [DbColumn("Size")]
        public ProductSizeType? Size
        { get; set; }

        [DbColumn("Currency")]
        public ProductCurrencyType Currency
        { get; set; }

        [DbColumn("Price")]
        public float Price
        { get; set; }

        [DbColumn("Sale")]
        public float? Sale
        { get; set; }

        [DbColumn("CreateTime")]
        public DateTime CreateTime
        { get; set; }

        [DbColumn("Stock")]
        public int Stock
        { get; set; }

        [DbColumn("IsActive")]
        public Boolean IsActive
        { get; set; }

        [DbColumn("Description")]
        public string Description
        { get; set; }

        [DbColumn("Remark")]
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
        ReplicaKitCup = 8,
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
