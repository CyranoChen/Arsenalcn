using System.Data;

namespace Arsenalcn.CasinoSys.Entity
{
    public static class Rank
    {
        public static DataTable GetTopGamblerMonthly()
        {
            return DataAccess.Rank.GetTopGamblerMonthly();
        }

        public static DataTable GetTopGamblerProfit(out int months)
        {
            return DataAccess.Rank.GetTopGamblerProfit(out months);
        }

        // ReSharper disable once InconsistentNaming
        public static DataTable GetTopGamblerRP(out int months)
        {
            return DataAccess.Rank.GetTopGamblerRP(out months);
        }

        public static DataTable GetTopGamblerTotalBet(out int months)
        {
            return DataAccess.Rank.GetTopGamblerTotalBet(out months);
        }
    }
}