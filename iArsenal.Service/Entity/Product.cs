using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Arsenalcn.Core;
using DataReaderMapper;

namespace iArsenal.Service
{
    [DbSchema("iArsenal_Product", Key = "ProductGuid", Sort = "Code")]
    public class Product : Entity<Guid>
    {
        public static void CreateMap()
        {
            var map = Mapper.CreateMap<IDataReader, Product>();

            map.ForMember(d => d.ID, opt => opt.MapFrom(s => (Guid) s.GetValue("ProductGuid")));

            //map.ForMember(d => d.Size, opt => opt.MapFrom(s =>
            //    (ProductSizeType)Enum.Parse(typeof(ProductSizeType), s.GetValue("Size").ToString())));

            //map.ForMember(d => d.Currency, opt => opt.MapFrom(s =>
            //    (ProductCurrencyType)Enum.Parse(typeof(ProductCurrencyType), s.GetValue("Currency").ToString())));

            map.ForMember(d => d.ProductTypeInfo, opt => opt.ResolveUsing(s =>
            {
                #region Generate Product Type Info

                string retValue;

                switch ((ProductType) ((int) s.GetValue("ProductType")))
                {
                    case ProductType.ReplicaKitHome:
                        retValue = "主场球衣";
                        break;
                    case ProductType.ReplicaKitAway:
                        retValue = "客场球衣";
                        break;
                    case ProductType.ReplicaKitCup:
                        retValue = "杯赛球衣";
                        break;
                    case ProductType.PlayerName:
                        retValue = "印名字";
                        break;
                    case ProductType.PlayerNumber:
                        retValue = "印号码";
                        break;
                    case ProductType.ArsenalFont:
                        retValue = "特殊字体";
                        break;
                    case ProductType.PremiershipPatch:
                        retValue = "联赛袖标";
                        break;
                    case ProductType.ChampionshipPatch:
                        retValue = "欧冠袖标";
                        break;
                    case ProductType.TravelPlan:
                        retValue = "观赛计划";
                        break;
                    case ProductType.TravelPartner:
                        retValue = "观赛同伴";
                        break;
                    case ProductType.MatchTicket:
                        retValue = "主场球票";
                        break;
                    case ProductType.TicketBeijing:
                        retValue = "友谊赛球票";
                        break;
                    case ProductType.MembershipCore:
                        retValue = "会员费(Core)";
                        break;
                    case ProductType.MembershipPremier:
                        retValue = "会员费(Premier)";
                        break;
                    case ProductType.Other:
                        retValue = "/";
                        break;
                    default:
                        retValue = string.Empty;
                        break;
                }

                return retValue;

                #endregion
            }));

            map.ForMember(d => d.CurrencyInfo, opt => opt.ResolveUsing(s =>
            {
                #region Generate CurrencyInfo

                var retValue = string.Empty;

                switch ((ProductCurrencyType) ((int) s.GetValue("Currency")))
                {
                    case ProductCurrencyType.GBP:
                        retValue = "￡";
                        break;
                    case ProductCurrencyType.CNY:
                        retValue = "￥";
                        break;
                    case ProductCurrencyType.USD:
                        retValue = "＄";
                        break;
                }

                return retValue;

                #endregion
            }));

            map.ForMember(d => d.PriceCNY, opt => opt.ResolveUsing(s =>
            {
                #region Generate PriceCNY

                var retValue = default(double);
                var sale = (double?) s.GetValue("Sale");
                var price = (double) s.GetValue("Price");

                double unitPrice;

                if (sale.HasValue)
                {
                    unitPrice = sale.Value;
                }
                else
                {
                    unitPrice = price;
                }

                switch ((ProductCurrencyType) ((int) s.GetValue("Currency")))
                {
                    case ProductCurrencyType.GBP:
                        retValue = unitPrice*ConfigGlobal.ExchangeRateGBP;
                        break;
                    case ProductCurrencyType.CNY:
                        retValue = unitPrice;
                        break;
                    case ProductCurrencyType.USD:
                        retValue = unitPrice*ConfigGlobal.ExchangeRateUSD;
                        break;
                }

                return retValue;

                #endregion
            }));

            map.ForMember(d => d.PriceInfo, opt => opt.ResolveUsing(s =>
            {
                #region Generate PriceInfo

                string retValue;
                var price = (double) s.GetValue("Price");
                var currencyIcon = string.Empty;

                switch ((ProductCurrencyType) ((int) s.GetValue("Currency")))
                {
                    case ProductCurrencyType.GBP:
                        currencyIcon = "￡";
                        break;
                    case ProductCurrencyType.CNY:
                        currencyIcon = "￥";
                        break;
                    case ProductCurrencyType.USD:
                        currencyIcon = "＄";
                        break;
                }

                if (price >= 0)
                {
                    retValue = currencyIcon + price.ToString("f2");
                }
                else
                {
                    retValue = string.Empty;
                }

                return retValue;

                #endregion
            }));

            map.ForMember(d => d.SaleInfo, opt => opt.ResolveUsing(s =>
            {
                #region Generate SaleInfo

                string retValue;
                var sale = (double?) s.GetValue("Sale");
                var currencyIcon = string.Empty;

                switch ((ProductCurrencyType) ((int) s.GetValue("Currency")))
                {
                    case ProductCurrencyType.GBP:
                        currencyIcon = "￡";
                        break;
                    case ProductCurrencyType.CNY:
                        currencyIcon = "￥";
                        break;
                    case ProductCurrencyType.USD:
                        currencyIcon = "＄";
                        break;
                }

                if (sale.HasValue && sale >= 0)
                {
                    retValue = currencyIcon + sale.Value.ToString("f2");
                }
                else
                {
                    retValue = string.Empty;
                }

                return retValue;

                #endregion
            }));
        }

        public static class Cache
        {
            public static List<Product> ProductList;

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
        }

        #region Members and Properties

        [DbColumn("Code")]
        public string Code { get; set; }

        [DbColumn("Name")]
        public string Name { get; set; }

        [DbColumn("DisplayName")]
        public string DisplayName { get; set; }

        [DbColumn("ProductType")]
        public ProductType ProductType { get; set; }

        [DbColumn("ImageUrl")]
        public string ImageUrl { get; set; }

        [DbColumn("QrCodeUrl")]
        public string QrCodeUrl { get; set; }

        [DbColumn("Material")]
        public string Material { get; set; }

        [DbColumn("Colour")]
        public string Colour { get; set; }

        [DbColumn("Size")]
        public ProductSizeType Size { get; set; }

        [DbColumn("Currency")]
        public ProductCurrencyType Currency { get; set; }

        [DbColumn("Price")]
        public double Price { get; set; }

        [DbColumn("Sale")]
        public double? Sale { get; set; }

        [DbColumn("CreateTime")]
        public DateTime CreateTime { get; set; }

        [DbColumn("Stock")]
        public int Stock { get; set; }

        [DbColumn("IsActive")]
        public bool IsActive { get; set; }

        [DbColumn("Description")]
        public string Description { get; set; }

        [DbColumn("Remark")]
        public string Remark { get; set; }

        public string ProductTypeInfo { get; set; }

        public string CurrencyInfo { get; set; }

        public double PriceCNY { get; set; }

        public string PriceInfo { get; set; }

        public string SaleInfo { get; set; }

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
        MembershipCore = 31,
        MembershipPremier = 32,
        Other = 0
    }

    public enum ProductSizeType
    {
        None = 0,
        Childrens = 2,
        Adults = 1,
        Infants = 3,
        MiniKit = 4,
        Ladies = 5
    }

    public enum ProductCurrencyType
    {
        GBP = 0,
        CNY = 1,
        USD = 2
    }
}