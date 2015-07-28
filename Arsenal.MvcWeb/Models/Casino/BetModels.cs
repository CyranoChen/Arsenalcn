using System;
using System.ComponentModel.DataAnnotations;
using System.Data;

using Arsenalcn.Core;
using Arsenal.Service.Casino;

namespace Arsenal.MvcWeb.Models.Casino
{
    public class BetDto : Viewer
    {
        public BetDto() : base() { }

        public BetDto(DataRow dr)
            : base(dr)
        { }

        protected virtual void Init(DataRow dr)
        {
            //if (dr.Table.Columns.Contains("Home") && dr.Table.Columns.Contains("Away"))
            //{
            //    TeamHome = Arsenal_Team.Cache.Load((Guid)dr["Home"]);
            //    TeamAway = Arsenal_Team.Cache.Load((Guid)dr["Away"]);
            //}

            //Item = CasinoItem.GetCasinoItem((Guid)dr["CasinoItemGuid"]);

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
                    if (ItemType.Equals(CasinoType.SingleChoice))
                    { BetIcon = "star"; }
                    else if (ItemType.Equals(CasinoType.MatchResult))
                    { BetIcon = "check"; }
                }
                else
                { BetIcon = "delete"; }
            }
            else
            { BetIcon = "back"; }
        }

        private void InitBetDetail()
        {
            //DataTable dtBetDetail = Arsenalcn.CasinoSys.Entity.BetDetail.GetBetDetailByBetID(ID);

            //if (dtBetDetail != null)
            //{
            //    DataRow drBetDetail = dtBetDetail.Rows[0];

            //    switch (ItemType)
            //    {
            //        case CasinoType.SingleChoice:
            //            if (drBetDetail["DetailName"].ToString() == MatchChoiceOption.HomeWinValue)
            //                BetDetailInfo = "主队胜";
            //            else if (drBetDetail["DetailName"].ToString() == MatchChoiceOption.DrawValue)
            //                BetDetailInfo = "双方平";
            //            else if (drBetDetail["DetailName"].ToString() == MatchChoiceOption.AwayWinValue)
            //                BetDetailInfo = "客队胜";

            //            if (BetRate.HasValue && BetAmount.HasValue)
            //            {
            //                BetDetailInfo += string.Format("[{0}] {1}",
            //                        BetRate.Value.ToString("f2"), BetAmount.Value.ToString("N0"));
            //            }
            //            break;
            //        case CasinoItem.CasinoType.MatchResult:
            //            MatchResultBetDetail bd = new MatchResultBetDetail(dtBetDetail);
            //            BetDetailInfo = string.Format("{0}：{1}", bd.Home, bd.Away);
            //            break;
            //    }
            //}
        }

        public int ID { get; set; }

        public string TeamHomeDisplayName { get; set; }

        public string TeamAwayDisplayName { get; set; }

        public string BetIcon { get; set; }

        public CasinoType ItemType { get; set; }

        public string UserName { get; set; }

        public DateTime BetTime { get; set; }

        public bool? IsWin { get; set; }

        public string BetDetail { get; set; }

        public float BetRate { get; set; } 
    }
}