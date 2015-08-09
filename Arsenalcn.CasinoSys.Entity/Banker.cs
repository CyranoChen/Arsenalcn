using System;
using System.Data;
using System.Data.SqlClient;

namespace Arsenalcn.CasinoSys.Entity
{
    public class Banker
    {
        public static readonly Guid DefaultBankerID = ConfigGlobal.DefaultBankerID;

        public Banker() { }

        public Banker(Guid bankerID)
        {
            DataRow dr = DataAccess.Banker.GetBankerByID(bankerID);

            if (dr != null)
                InitBanker(dr);
        }

        private void InitBanker(DataRow dr)
        {
            if (dr != null)
            {
                BankerID = (Guid)dr["ID"];
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
            DataAccess.Banker.UpdateBankerCash(BankerID, Cash, trans);
        }

        public static void ActiveBankerStatistics()
        {
            DataTable dt = DataAccess.Banker.GetAllBankers(true);

            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    Banker banker = new Banker((Guid)dr["ID"]);
                    banker.Cash = DataAccess.Bet.GetTotalEarningByBankerGuid((Guid)dr["ID"]);
                    banker.Update(null);
                }
            }
        }

        public Guid BankerID
        { get; private set; }

        public string BankerName
        { get; private set; }

        public int? ClubID
        { get; private set; }

        public float Cash
        { get; set; }

        public bool IsActive
        { get; private set; }
    }
}
