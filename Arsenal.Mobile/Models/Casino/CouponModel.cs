using System;
using Arsenal.Service;
using Arsenal.Service.Casino;
using AutoMapper;

namespace Arsenal.Mobile.Models.Casino
{
    public class CouponDto
    {
        public static MapperConfiguration ConfigMapper()
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<CouponView, CouponDto>()
                .ConstructUsing(s => new CouponDto
                {
                    TeamHomeName = s.Home.TeamDisplayName,
                    TeamHomeLogo = ConfigGlobal_Arsenal.PluginAcnCasinoPath + s.Home.TeamLogo,
                    TeamAwayName = s.Away.TeamDisplayName,
                    TeamAwayLogo = ConfigGlobal_Arsenal.PluginAcnCasinoPath + s.Away.TeamLogo,
                }));

            return config;
        }

        #region Members and Properties

        public Guid MatchGuid { get; set; }

        public string TeamHomeName { get; set; }

        public string TeamHomeLogo { get; set; }

        public string TeamAwayName { get; set; }

        public string TeamAwayLogo { get; set; }

        public string LeagueName { get; set; }

        public short? BetResultHome { get; set; }

        public short? BetResultAway { get; set; }

        public DateTime PlayTime { get; set; }

        #endregion
    }
}