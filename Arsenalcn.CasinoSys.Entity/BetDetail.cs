using System;
using System.Data;

namespace Arsenalcn.CasinoSys.Entity
{
    public class BetDetail
    {
        public BetDetail() { }

        public BetDetail(int betDetailID)
        {
            //DataRow dr = DataAccess.Bet.GetBetByID(betID);
            DataRow dr = null;

            if (dr != null)
                InitBet(dr);
        }

        public BetDetail(DataRow dr)
        {
            InitBet(dr);
        }

        private void InitBet(DataRow dr)
        {
            if (dr != null)
            {

            }
            else
                throw new Exception("Unable to init BetDetail.");
        }

        public static DataTable GetBetDetailByBetID(int betID)
        {
            return DataAccess.BetDetail.GetBetDetailByBetID(betID);
        }
    }
}
