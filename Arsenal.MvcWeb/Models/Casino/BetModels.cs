using System;
using System.Linq;

using AutoMapper;

using Arsenalcn.Core;
using Arsenal.Service.Casino;

namespace Arsenal.MvcWeb.Models.Casino
{
    public class BetDto
    {
        public BetDto() { }

        public static void CreateMap()
        {
            var map = Mapper.CreateMap<BetView, BetDto>();

            map.ConstructUsing(s => new BetDto
            {
                ItemType = s.CasinoItem.ItemType
            });

            map.ForMember(d => d.TeamHomeName, opt => opt.MapFrom(s => s.Home.TeamDisplayName));
            map.ForMember(d => d.TeamAwayName, opt => opt.MapFrom(s => s.Away.TeamDisplayName));

            map.ForMember(d => d.BetResultHome, opt =>
            {
                opt.Condition(s => s.CasinoItem.ItemType.Equals(CasinoType.MatchResult));
                opt.MapFrom(s => Convert.ToInt16(s.BetDetails.SingleOrDefault(x => x.DetailName.Equals("home", StringComparison.OrdinalIgnoreCase)).DetailValue));
            });

            map.ForMember(d => d.BetResultAway, opt =>
            {
                opt.Condition(s => s.CasinoItem.ItemType.Equals(CasinoType.MatchResult));
                opt.MapFrom(s => Convert.ToInt16(s.BetDetails.SingleOrDefault(x => x.DetailName.Equals("away", StringComparison.OrdinalIgnoreCase)).DetailValue));
            });

            map.ForMember(d => d.BetResult, opt =>
            {
                opt.Condition(s => s.CasinoItem.ItemType.Equals(CasinoType.SingleChoice));
                opt.MapFrom(s => Enum.Parse(typeof(BetResultType), s.BetDetails.FirstOrDefault().DetailName));
            });

            map.ForMember(d => d.BetIcon, opt => opt.ResolveUsing(s =>
            {
                var _icon = BetIconType.none;

                if (s.IsWin.HasValue)
                {
                    if (s.IsWin.Value)
                    {
                        if (s.CasinoItem.ItemType.Equals(CasinoType.SingleChoice))
                        { _icon = BetIconType.star; }
                        else
                        { _icon = BetIconType.check; }
                    }
                    else
                    {
                        _icon = BetIconType.delete;
                    }
                }
                else
                {
                    _icon = BetIconType.back;
                }

                return _icon;
            }));
        }

        public static BetDto Single(object key)
        {
            IRepository repo = new Repository();

            var instance = repo.Single<BetView>(key);

            instance.Many<BetDetail>(x => x.BetID == instance.ID);

            CreateMap();

            return Mapper.Map<BetDto>(instance);
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