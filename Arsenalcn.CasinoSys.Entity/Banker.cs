using System;
using System.Data;
using System.Data.SqlClient;

namespace Arsenalcn.CasinoSys.Entity
{
    public class Banker
    {
        public static readonly Guid DefaultBankerID = ConfigGlobal.DefaultBankerID;

        public Banker()
        {
        }

        public Banker(Guid key)
        {
            var dr = DataAccess.Banker.GetBankerById(key);

            if (dr != null)
                InitBanker(dr);
        }

        #region Members and Properties

        public Guid BankerGuid { get; set; }

        public string BankerName { get; set; }

        public int? ClubID { get; set; }

        public float Cash { get; set; }

        public bool IsActive { get; set; }

        #endregion

        private void InitBanker(DataRow dr)
        {
            if (dr != null)
            {
                BankerGuid = (Guid)dr["ID"];
                BankerName = Convert.ToString(dr["BankerName"]);

                if (Convert.IsDBNull(dr["ClubID"]))
                    ClubID = null;
                else
                    ClubID = Convert.ToInt32(dr["ClubID"]);

                Cash = Convert.ToSingle(dr["Cash"]);
                IsActive = Convert.ToBoolean(dr["IsActive"]);
            }
            else
                throw new Exception("Unable to init Banker");
        }

        public void Update(SqlTransaction trans)
        {
            DataAccess.Banker.UpdateBankerCash(BankerGuid, Cash, trans);
        }

        //public static void ActiveBankerStatistics()
        //{
        //    var dt = DataAccess.Banker.GetAllBankers(true);

        //    if (dt != null)
        //    {
        //        foreach (DataRow dr in dt.Rows)
        //        {
        //            var banker = new Banker((Guid)dr["ID"]);
        //            banker.Cash = DataAccess.Bet.GetTotalEarningByBankerGuid((Guid)dr["ID"]);
        //            banker.Update(null);
        //        }
        //    }
        //}
    }
}