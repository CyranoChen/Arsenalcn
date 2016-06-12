using System;
using System.Linq;
using Arsenal.Service;
using Arsenal.Service.Casino;
using Arsenalcn.Core;
using AutoMapper;

namespace Arsenal.Mobile.Models.Casino
{
    public class MatchDto
    {
        public static MapperConfiguration ConfigMapper()
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<MatchView, MatchDto>()
                .ConstructUsing(s => new MatchDto
                {
                    TeamHomeName = s.Home.TeamDisplayName,
                    TeamHomeLogo = ConfigGlobal_Arsenal.PluginAcnCasinoPath + s.Home.TeamLogo,
                    TeamAwayName = s.Away.TeamDisplayName,
                    TeamAwayLogo = ConfigGlobal_Arsenal.PluginAcnCasinoPath + s.Away.TeamLogo,
                })
                .ForMember(d => d.HomeRate, opt =>
                {
                    opt.Condition(s => s.ChoiceOptions != null);
                    opt.MapFrom(s => s.ChoiceOptions.SingleOrDefault(x =>
                        x.OptionName.Equals("home", StringComparison.OrdinalIgnoreCase)).OptionRate);
                })
                .ForMember(d => d.DrawRate, opt =>
                {
                    opt.Condition(s => s.ChoiceOptions != null);
                    opt.MapFrom(s => s.ChoiceOptions.SingleOrDefault(x =>
                        x.OptionName.Equals("draw", StringComparison.OrdinalIgnoreCase)).OptionRate);
                })
                .ForMember(d => d.AwayRate, opt =>
                {
                    opt.Condition(s => s.ChoiceOptions != null);
                    opt.MapFrom(s => s.ChoiceOptions.SingleOrDefault(x =>
                        x.OptionName.Equals("away", StringComparison.OrdinalIgnoreCase)).OptionRate);
                })
            );

            return config;
        }

        public static MatchDto Single(object key)
        {
            IRepository repo = new Repository();

            var instance = repo.Single<MatchView>(key);
            instance.Many<ChoiceOption>(x => x.CasinoItemGuid == instance.CasinoItem.ID);

            var mapper = ConfigMapper().CreateMapper();

            return mapper.Map<MatchDto>(instance);
        }

        #region Members and Properties

        // ReSharper disable once InconsistentNaming
        public Guid ID { get; set; }

        public string TeamHomeName { get; set; }

        public string TeamHomeLogo { get; set; }

        public string TeamAwayName { get; set; }

        public string TeamAwayLogo { get; set; }

        public string LeagueName { get; set; }

        public short? ResultHome { get; set; }

        public short? ResultAway { get; set; }

        public DateTime PlayTime { get; set; }

        public short? Round { get; set; }

        public double HomeRate { get; set; }

        public double DrawRate { get; set; }

        public double AwayRate { get; set; }

        #endregion
    }
}