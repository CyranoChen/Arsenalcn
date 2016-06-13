using System;
using Arsenal.Service;
using Arsenal.Service.Casino;
using AutoMapper;

namespace Arsenal.Mobile.Models.Casino
{
    public class BonusDto
    {
        public static MapperConfiguration ConfigMapper()
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<BonusView, BonusDto>()
                .ConstructUsing(s => new BonusDto
                {
                    TeamHomeName = s.Home.TeamDisplayName,
                    TeamHomeLogo = ConfigGlobal_Arsenal.PluginAcnCasinoPath + s.Home.TeamLogo,
                    TeamAwayName = s.Away.TeamDisplayName,
                    TeamAwayLogo = ConfigGlobal_Arsenal.PluginAcnCasinoPath + s.Away.TeamLogo,
                })
                .ForMember(d => d.BetIcon, opt => opt.ResolveUsing(s =>
                {
                    var icon = BetIconType.none;

                    // SinglceChoice 盈
                    if (s.Earning.HasValue && s.TotalBet.HasValue && s.Earning.Value > s.TotalBet.Value)
                    {
                        // MatchResult 中
                        if (s.RPBonus.HasValue && s.RPBonus.Value == 1)
                        {
                            icon = BetIconType.star;
                        }
                        // MatchResult 不中
                        else
                        {
                            icon = BetIconType.check;
                        }
                    }
                    // MatchResult 中
                    else if (s.RPBonus.HasValue && s.RPBonus == 1)
                    {
                        icon = BetIconType.check;
                    }
                    else
                    {
                        icon = BetIconType.delete;
                    }

                    return icon;
                })));

            return config;
        }

        #region Members and Properties

        public Guid MatchGuid { get; set; }

        public string TeamHomeName { get; set; }

        public string TeamHomeLogo { get; set; }

        public string TeamAwayName { get; set; }

        public string TeamAwayLogo { get; set; }

        public string LeagueName { get; set; }

        public DateTime PlayTime { get; set; }

        public int Win { get; set; }

        public int Lose { get; set; }

        public double? Earning { get; set; }

        public double? TotalBet { get; set; }

        public int? RPBonus { get; set; }

        public BetIconType BetIcon { get; set; }

        #endregion
    }
}