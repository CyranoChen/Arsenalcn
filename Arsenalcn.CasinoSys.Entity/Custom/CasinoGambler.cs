using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Arsenalcn.CasinoSys.Entity
{
    public class CasinoGambler
    {
        public CasinoGambler() { }

        private CasinoGambler(DataRow dr)
        {
            InitCasinoGambler(dr);
        }

        private void InitCasinoGambler(DataRow dr)
        {
            if (dr != null)
            {
                UserID = Convert.ToInt32(dr["UserID"]);
                UserName = dr["UserName"].ToString();
                Win = Convert.ToInt16(dr["Win"]);
                Lose = Convert.ToInt16(dr["Lose"]);
                Earning = Convert.ToSingle(dr["Earning"]);
                TotalBet = Convert.ToSingle(dr["TotalBet"]);

                if (!Convert.IsDBNull(dr["RPBet"]))
                    RPBet = Convert.ToInt16(dr["RPBet"]);
                else
                    RPBet = null;

                if (!Convert.IsDBNull(dr["RPBonus"]))
                    RPBonus = Convert.ToInt16(dr["RPBonus"]);
                else
                    RPBonus = null;

                Profit = Earning - TotalBet;

                if (TotalBet > 0f)
                { ProfitRate = Profit / TotalBet * 100; }
                else
                { ProfitRate = 0; }

                if (RPBet.HasValue && RPBet.Value > 0
                    && RPBonus.HasValue && RPBonus.Value >= 0)
                { RPRate = RPBonus / RPBet * 100; }
                else
                { RPRate = null; }
            }
            else
                throw new Exception("Unable to init CasinoGambler.");
        }

        public static List<CasinoGambler> GetCasinoGamblers()
        {
            DataTable dt = DataAccess.Gambler.GetGamblerProfitView();
            List<CasinoGambler> list = new List<CasinoGambler>();

            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    list.Add(new CasinoGambler(dr));
                }
            }

            return list;
        }

        public static List<CasinoGambler> GetCasinoGamblers(Guid leagueGuid)
        {
            DataTable dt = DataAccess.Gambler.GetGamblerProfitView(leagueGuid);
            List<CasinoGambler> list = new List<CasinoGambler>();

            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    list.Add(new CasinoGambler(dr));
                }
            }

            return list;
        }

        #region Members and Properties

        public int UserID
        { get; set; }

        public string UserName
        { get; set; }

        public int Win
        { get; set; }

        public int Lose
        { get; set; }

        public float Earning
        { get; set; }

        public float TotalBet
        { get; set; }

        public int? RPBet
        { get; set; }

        public int? RPBonus
        { get; set; }

        public float Profit
        { get; set; }

        public float ProfitRate
        { get; set; }

        public float? RPRate
        { get; set; }

        public int Rank
        { get; set; }

        #endregion
    }
}
