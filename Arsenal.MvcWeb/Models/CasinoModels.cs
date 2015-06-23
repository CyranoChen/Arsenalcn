using System;
using System.Data;

using Arsenalcn.CasinoSys.Entity;
using ArsenalTeam = Arsenalcn.CasinoSys.Entity.Arsenal.Team;

namespace Arsenal.MvcWeb.Models
{
    public class CasinoMatch : Match
    {
        public CasinoMatch(Guid matchGuid)
            : base(matchGuid)
        {
            Init();
        }

        public CasinoMatch(DataRow dr)
            : base(dr)
        {
            Init();
        }

        private void Init()
        {
            TeamHome = Arsenal_Team.Cache.Load(Home);
            TeamAway = Arsenal_Team.Cache.Load(Away);
        }

        public ArsenalTeam TeamHome { get; set; }

        public ArsenalTeam TeamAway { get; set; }
    }

    public class CasinoBet : Bet
    {
        public CasinoBet(DataRow dr)
            : base(dr)
        {
            Init(dr);
        }

        private void Init(DataRow dr)
        {
            TeamHome = Arsenal_Team.Cache.Load((Guid)dr["Home"]);
            TeamAway = Arsenal_Team.Cache.Load((Guid)dr["Away"]);

            ItemType = (CasinoItem.CasinoType)Enum.Parse(typeof(CasinoItem.CasinoType), dr["ItemType"].ToString());

            if (IsWin.HasValue)
            {
                if (IsWin.Value)
                {
                    if (ItemType.Equals(CasinoItem.CasinoType.SingleChoice))
                    { BetIconInfo = "star"; }
                    else if (ItemType.Equals(CasinoItem.CasinoType.MatchResult))
                    { BetIconInfo = "check"; }
                }
                else
                { BetIconInfo = "delete"; }
            }
            else
            { BetIconInfo = "back"; }

            DataTable dtBetDetail = BetDetail.GetBetDetailByBetID(ID);

            if (dtBetDetail != null)
            {
                DataRow drBetDetail = dtBetDetail.Rows[0];

                switch (ItemType)
                {
                    case CasinoItem.CasinoType.SingleChoice:
                        if (drBetDetail["DetailName"].ToString() == MatchChoiceOption.HomeWinValue)
                            BetDetailInfo = "主队胜";
                        else if (drBetDetail["DetailName"].ToString() == MatchChoiceOption.DrawValue)
                            BetDetailInfo = "双方平";
                        else if (drBetDetail["DetailName"].ToString() == MatchChoiceOption.AwayWinValue)
                            BetDetailInfo = "客队胜";

                        if (BetRate.HasValue && BetAmount.HasValue)
                        {
                            BetDetailInfo += string.Format("[{0}] {1}",
                                    BetRate.Value.ToString("f2"), BetAmount.Value.ToString("N0"));
                        }
                        break;
                    case CasinoItem.CasinoType.MatchResult:
                        MatchResultBetDetail bd = new MatchResultBetDetail(dtBetDetail);
                        BetDetailInfo = string.Format("{0}：{1}", bd.Home, bd.Away);
                        break;
                }
            }
        }

        public ArsenalTeam TeamHome { get; set; }

        public ArsenalTeam TeamAway { get; set; }

        public CasinoItem.CasinoType ItemType { get; set; }

        public string BetDetailInfo { get; set; }

        public string BetIconInfo { get; set; }
    }
}