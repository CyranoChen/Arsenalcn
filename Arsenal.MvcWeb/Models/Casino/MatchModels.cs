﻿using System;
using System.Linq;

using Arsenal.Service.Casino;
using Arsenalcn.Core;
using AutoMapper;

namespace Arsenal.MvcWeb.Models.Casino
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

        private void Init()
        {
            IRepository repo = new Repository();

            var instance = repo.Single<MatchView>(this.MatchGuid);
            instance.Many<ChoiceOption>(instance.CasinoItem.ID);

            CreateMap();

            Mapper.Map<MatchView, MatchDto>(instance, this);
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