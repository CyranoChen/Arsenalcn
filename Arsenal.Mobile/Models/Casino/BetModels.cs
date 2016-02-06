using System;
using System.Linq;
using Arsenal.Service.Casino;
using Arsenalcn.Core;
using AutoMapper;

namespace Arsenal.Mobile.Models.Casino
{
    public class BetDto
    {
        public static MapperConfiguration ConfigMapper()
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<BetView, BetDto>()
                .ConstructUsing(s => new BetDto {ItemType = s.CasinoItem.ItemType})
                .ForMember(d => d.TeamHomeName, opt => opt.MapFrom(s => s.Home.TeamDisplayName))
                .ForMember(d => d.TeamAwayName, opt => opt.MapFrom(s => s.Away.TeamDisplayName))
                .ForMember(d => d.BetResultHome, opt =>
                {
                    opt.Condition(s => s.CasinoItem.ItemType.Equals(CasinoType.MatchResult));
                    opt.MapFrom(
                        s =>
                            Convert.ToInt16(
                                s.BetDetails.SingleOrDefault(
                                    x => x.DetailName.Equals("home", StringComparison.OrdinalIgnoreCase)).DetailValue));
                })
                .ForMember(d => d.BetResultAway, opt =>
                {
                    opt.Condition(s => s.CasinoItem.ItemType.Equals(CasinoType.MatchResult));
                    opt.MapFrom(
                        s =>
                            Convert.ToInt16(
                                s.BetDetails.SingleOrDefault(
                                    x => x.DetailName.Equals("away", StringComparison.OrdinalIgnoreCase)).DetailValue));
                })
                .ForMember(d => d.BetResult, opt =>
                {
                    opt.Condition(s => s.CasinoItem.ItemType.Equals(CasinoType.SingleChoice));
                    opt.MapFrom(s => Enum.Parse(typeof (BetResultType), s.BetDetails.FirstOrDefault().DetailName));
                })
                .ForMember(d => d.BetIcon, opt => opt.ResolveUsing(s =>
                {
                    var icon = BetIconType.none;

                    if (s.IsWin.HasValue)
                    {
                        if (s.IsWin.Value)
                        {
                            if (s.CasinoItem.ItemType.Equals(CasinoType.SingleChoice))
                            {
                                icon = BetIconType.star;
                            }
                            else
                            {
                                icon = BetIconType.check;
                            }
                        }
                        else
                        {
                            icon = BetIconType.delete;
                        }
                    }
                    else
                    {
                        icon = BetIconType.back;
                    }

                    return icon;
                })));

            return config;
        }

        public static BetDto Single(object key)
        {
            IRepository repo = new Repository();

            var instance = repo.Single<BetView>(key);

            instance.Many<BetDetail>(x => x.BetID == instance.ID);

            var mapper = ConfigMapper().CreateMapper();

            return mapper.Map<BetDto>(instance);
        }

        #region Members and Properties

        public int ID { get; set; }

        public int UserID { get; set; }

        public string UserName { get; set; }

        public CasinoType ItemType { get; set; }

        public string TeamHomeName { get; set; }

        public string TeamAwayName { get; set; }

        public BetIconType BetIcon { get; set; }

        public DateTime BetTime { get; set; }

        public double? BetAmount { get; set; }

        public BetResultType BetResult { get; set; }

        public double? BetRate { get; set; }

        public string EarningDesc { get; set; }

        public bool? IsWin { get; set; }

        public short? BetResultHome { get; set; }

        public short? BetResultAway { get; set; }

        #endregion
    }

    public enum BetResultType
    {
        Home,
        Away,
        Draw
    }

    public enum BetIconType
    {
        none,
        star,
        check,
        delete,
        back
    }
}