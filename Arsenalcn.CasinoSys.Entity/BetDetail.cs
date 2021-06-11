using System;
using System.Collections.Generic;
using System.Data;

namespace Arsenalcn.CasinoSys.Entity
{
    public class BetDetail
    {
        private BetDetail(DataRow dr)
        {
            InitBetDetail(dr);
        }

        public int ID { get; set; }

        public Guid CasinoItemGuid { get; set; }

        public float? BetAmount { get; set; }

        public string DetailName { get; set; }

        public static DataTable GetBetDetailByBetId(int id)
        {
            return DataAccess.BetDetail.GetBetDetailByBetId(id);
        }

        public static List<BetDetail> GetBetDetails(Guid itemGuid)
        {
            var dt = DataAccess.BetDetail.GetBetDetailByCasinoItemGuid(itemGuid);
            var list = new List<BetDetail>();

            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    list.Add(new BetDetail(dr));
                }
            }

            return list;
        }

        private void InitBetDetail(DataRow dr)
        {
            if (dr != null)
            {
                ID = (int)dr["ID"];
                CasinoItemGuid = (Guid)dr["CasinoItemGuid"];

                if (!Convert.IsDBNull(dr["BetAmount"]))
                    BetAmount = Convert.ToSingle(dr["BetAmount"]);
                else
                    BetAmount = null;

                DetailName = dr["DetailName"].ToString();
            }
            else
                throw new Exception("Unable to init BetDetail.");
        }
    }
}