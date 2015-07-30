using System;
using System.Linq;

using AutoMapper;

using Arsenal.Service.Casino;
using Arsenalcn.Core;

namespace Arsenal.MvcWeb.Models.Casino
{
    public class MatchDto
    {
        public MatchDto() { }

        public static void CreateMap()
        {
            var map = Mapper.CreateMap<MatchView, MatchDto>();

            map.ConstructUsing(s => new MatchDto
            {
                TeamHomeName = s.Home.TeamDisplayName,
                TeamHomeLogo = s.Home.TeamLogo,
                TeamAwayName = s.Away.TeamDisplayName,
                TeamAwayLogo = s.Away.TeamLogo,
                HomeRate = s.ListChoiceOption.SingleOrDefault(x => x.OptionName.Equals("home", StringComparison.OrdinalIgnoreCase)).OptionRate,
                DrawRate = s.ListChoiceOption.SingleOrDefault(x => x.OptionName.Equals("draw", StringComparison.OrdinalIgnoreCase)).OptionRate,
                AwayRate = s.ListChoiceOption.SingleOrDefault(x => x.OptionName.Equals("away", StringComparison.OrdinalIgnoreCase)).OptionRate,
            });
        }

        public static MatchDto Single(object key)
        {
            IRepository repo = new Repository();

            var instance = repo.Single<MatchView>(key);
            instance.Many<ChoiceOption>(instance.CasinoItem.ID);

            CreateMap();

            return Mapper.Map<MatchDto>(instance);
        }

        #region Members and Properties

        public Guid ID { get; set; }

        public string TeamHomeName { get; set; }

        public string TeamHomeLogo { get; set; }

        public string TeamAwayName { get; set; }

        public string TeamAwayLogo { get; set; }

        public string LeagueName { get; set; }

        public short? ResultHome { get; set; }

        public short? ResultAway { get; set; }

        public DateTime PlayTime { get; set; }

        public double HomeRate { get; set; }

        public double DrawRate { get; set; }

        public double AwayRate { get; set; }

        #endregion
    }
}