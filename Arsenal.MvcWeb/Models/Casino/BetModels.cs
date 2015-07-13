using System;
using System.ComponentModel.DataAnnotations;
using System.Data;

using Arsenalcn.CasinoSys.Entity;
using ArsenalTeam = Arsenalcn.CasinoSys.Entity.Arsenal.Team;

namespace Arsenal.MvcWeb.Models.Casino
{
    public class BetDto : Bet
    {
        public BetDto() : base() { }

        public BetDto(DataRow dr)
            : base(dr)
        {
            Init(dr);
        }

        protected virtual void Init(DataRow dr)
        {
            if (dr.Table.Columns.Contains("Home") && dr.Table.Columns.Contains("Away"))
            {
                TeamHome = Arsenal_Team.Cache.Load((Guid)dr["Home"]);
                TeamAway = Arsenal_Team.Cache.Load((Guid)dr["Away"]);
            }

            Item = CasinoItem.GetCasinoItem((Guid)dr["CasinoItemGuid"]);

            // TODO: improve performance
            InitBetIcon();
            InitBetDetail();
        }

        private void InitBetIcon()
        {
            if (IsWin.HasValue)
            {
                if (IsWin.Value)
                {
                    if (Item.ItemType.Equals(CasinoItem.CasinoType.SingleChoice))
                    { BetIconInfo = "star"; }
                    else if (Item.ItemType.Equals(CasinoItem.CasinoType.MatchResult))
                    { BetIconInfo = "check"; }
                }
                else
                { BetIconInfo = "delete"; }
            }
            else
            { BetIconInfo = "back"; }
        }

        private void InitBetDetail()
        {
            DataTable dtBetDetail = BetDetail.GetBetDetailByBetID(ID);

            if (dtBetDetail != null)
            {
                DataRow drBetDetail = dtBetDetail.Rows[0];

                switch (Item.ItemType)
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

        public CasinoItem Item { get; set; }

        public string BetDetailInfo { get; set; }

        public string BetIconInfo { get; set; }
    }

    //public class SingleChoiceModel
    //{
    //    [Required]
    //    [Display(Name = "投注选项")]
    //    public string SeletedOption { get; set; }

    //    [Required]
    //    [DataType(DataType.Currency)]
    //    [Display(Name = "投注金额")]
    //    public string BetAmount { get; set; }
    //}
}