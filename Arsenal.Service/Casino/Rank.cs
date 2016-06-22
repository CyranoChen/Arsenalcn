using System.Data;
using Arsenalcn.Core;
using DataReaderMapper;

namespace Arsenal.Service.Casino
{
    [DbSchema("AcnCasino_Rank", Sort = "RankYear DESC, RankMonth DESC")]
    public class Rank : Entity<int>
    {
        public static void CreateMap()
        {
            Mapper.CreateMap<IDataReader, Rank>();
        }

        public void Init(GamblerDW winner, GamblerDW loser, GamblerDW rper)
        {
            if (winner != null)
            {
                WinnerUserID = winner.UserID;
                WinnerUserName = winner.UserName;
                WinnerProfit = winner.Profit;
                WinnerTotalBet = winner.TotalBet;
            }

            if (loser != null)
            {
                LoserUserID = loser.UserID;
                LoserUserName = loser.UserName;
                LoserProfit = loser.Profit;
                LoserTotalBet = loser.TotalBet;
            }

            if (rper != null)
            {
                RPUserID = rper.UserID;
                RPUserName = rper.UserName;
                RPAmount = rper.RPBonus;
            }
            else
            {
                RPUserID = 0;
                RPUserName = "/";
                RPAmount = 0;
            }
        }

        #region Members and Properties

        [DbColumn("RankYear")]
        public int RankYear { get; set; }

        [DbColumn("RankMonth")]
        public int RankMonth { get; set; }

        [DbColumn("WinnerUserID")]
        public int WinnerUserID { get; set; }

        [DbColumn("WinnerUserName")]
        public string WinnerUserName { get; set; }

        [DbColumn("WinnerProfit")]
        public double WinnerProfit { get; set; }

        [DbColumn("WinnerTotalBet")]
        public double WinnerTotalBet { get; set; }

        [DbColumn("LoserUserID")]
        public int LoserUserID { get; set; }

        [DbColumn("LoserUserName")]
        public string LoserUserName { get; set; }

        [DbColumn("LoserProfit")]
        public double LoserProfit { get; set; }

        [DbColumn("LoserTotalBet")]
        public double LoserTotalBet { get; set; }

        [DbColumn("RPUserID")]
        public int RPUserID { get; set; }

        [DbColumn("RPUserName")]
        public string RPUserName { get; set; }

        [DbColumn("RPAmount")]
        public int RPAmount { get; set; }

        #endregion

    }

    public enum RankType
    {
        Winner = 1,
        Loser = 2,
        RP = 3
    }
}
