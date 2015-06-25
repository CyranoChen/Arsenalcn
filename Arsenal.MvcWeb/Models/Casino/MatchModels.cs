using System;
using System.Data;

using Arsenalcn.CasinoSys.Entity;
using ArsenalTeam = Arsenalcn.CasinoSys.Entity.Arsenal.Team;

namespace Arsenal.MvcWeb.Models.Casino
{
    public class MatchDto : Match
    {
        public MatchDto(Guid matchGuid)
            : base(matchGuid)
        {
            Init();
        }

        public MatchDto(DataRow dr)
            : base(dr)
        {
            Init();
        }

        protected virtual void Init()
        {
            TeamHome = Arsenal_Team.Cache.Load(Home);
            TeamAway = Arsenal_Team.Cache.Load(Away);
        }

        public ArsenalTeam TeamHome { get; set; }

        public ArsenalTeam TeamAway { get; set; }
    }

    public class MatchWithRateDto : MatchDto
    {
        public MatchWithRateDto(Guid matchGuid)
            : base(matchGuid)
        {
            Init();
        }

        public MatchWithRateDto(DataRow dr)
            : base(dr)
        {
            Init();
        }

        protected override void Init()
        {
            TeamHome = Arsenal_Team.Cache.Load(Home);
            TeamAway = Arsenal_Team.Cache.Load(Away);

            Guid? guid = CasinoItem.GetCasinoItemGuidByMatch(MatchGuid, CasinoItem.CasinoType.SingleChoice);

            if (guid.HasValue)
            {
                CasinoItem item = CasinoItem.GetCasinoItem(guid.Value);

                if (item != null)
                {
                    var options = ((SingleChoice)item).Options;

                    ChoiceOption winOption = options.Find(x => x.OptionValue.Equals(MatchChoiceOption.HomeWinValue));
                    ChoiceOption drawOption = options.Find(x => x.OptionValue.Equals(MatchChoiceOption.DrawValue));
                    ChoiceOption loseOption = options.Find(x => x.OptionValue.Equals(MatchChoiceOption.AwayWinValue));

                    WinRate = winOption.OptionRate.Value;
                    DrawRate = drawOption.OptionRate.Value;
                    LoseRate = loseOption.OptionRate.Value;
                }
            }
        }

        public float WinRate { get; set; }

        public float DrawRate { get; set; }

        public float LoseRate { get; set; }
    }
}