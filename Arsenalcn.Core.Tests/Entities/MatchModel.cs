using System;
using System.Collections.Generic;

using Arsenal.Service.Casino;
using AutoMapper;
using System.Linq;

namespace Arsenalcn.Core.Tests
{
    public class MatchDto
    {
        public MatchDto() { }

        public MatchDto(object key)
        {
            this.MatchGuid = (Guid)key;
            this.Init();
        }

        public static void CreateMap()
        {
            var map = Mapper.CreateMap<MatchView, MatchDto>();

            map.ConstructUsing(s => new MatchDto
            {
                MatchGuid = s.ID,
                TeamHomeName = s.Home.TeamDisplayName,
                TeamHomeLogo = s.Home.TeamLogo,
                TeamAwayName = s.Away.TeamDisplayName,
                TeamAwayLogo = s.Away.TeamLogo,
                HomeRate = s.ListChoiceOption.Single(x => x.OptionName.Equals("home", StringComparison.OrdinalIgnoreCase)).OptionRate,
                DrawRate = s.ListChoiceOption.Single(x => x.OptionName.Equals("draw", StringComparison.OrdinalIgnoreCase)).OptionRate,
                AwayRate = s.ListChoiceOption.Single(x => x.OptionName.Equals("away", StringComparison.OrdinalIgnoreCase)).OptionRate,
            });
        }

        public void Init()
        {
            IRepository repo = new Repository();

            var instance = repo.Single<MatchView>(this.MatchGuid);
            instance.Many<ChoiceOption>(instance.CasinoItem.ID);

            CreateMap();

            Mapper.Map<MatchView, MatchDto>(instance, this);
        }

        public static IEnumerable<MatchDto> All()
        {
            IRepository repo = new Repository();

            var query = repo.All<MatchView>().FindAll(x => x.ResultHome.HasValue && x.ResultAway.HasValue)
                .Many<MatchView, ChoiceOption>((tSource, t) => tSource.CasinoItem.ID.Equals(t.CasinoItemGuid));

            var map = Mapper.CreateMap<MatchView, MatchDto>();

            map.ConstructUsing(s => new MatchDto
            {
                MatchGuid = s.ID,
                TeamHomeName = s.Home.TeamDisplayName,
                TeamHomeLogo = s.Home.TeamLogo,
                TeamAwayName = s.Away.TeamDisplayName,
                TeamAwayLogo = s.Away.TeamLogo,
                HomeRate = s.ListChoiceOption.Single(x => x.OptionName.Equals("home", StringComparison.OrdinalIgnoreCase)).OptionRate,
                DrawRate = s.ListChoiceOption.Single(x => x.OptionName.Equals("draw", StringComparison.OrdinalIgnoreCase)).OptionRate,
                AwayRate = s.ListChoiceOption.Single(x => x.OptionName.Equals("away", StringComparison.OrdinalIgnoreCase)).OptionRate,
            });

            return Mapper.Map<IEnumerable<MatchDto>>(source: query.AsEnumerable());
        }

        #region Members and Properties

        public Guid MatchGuid { get; set; }

        public string TeamHomeName { get; set; }

        public string TeamHomeLogo { get; set; }

        public string TeamAwayName { get; set; }

        public string TeamAwayLogo { get; set; }

        public string LeagueName { get; set; }

        public short? ResultHome { get; set; }

        public short? ResultAway { get; set; }

        public DateTime PlayTime { get; set; }

        public float HomeRate { get; set; }

        public float DrawRate { get; set; }

        public float AwayRate { get; set; }

        #endregion
    }
}