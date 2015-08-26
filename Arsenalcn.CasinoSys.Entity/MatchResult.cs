using System;
using System.Data;
using System.Data.SqlClient;

namespace Arsenalcn.CasinoSys.Entity
{
    public sealed class MatchResult : CasinoItem
    {
        internal MatchResult() { }

        protected override void BuildDetail()
        {
            var dr = DataAccess.MatchResult.GetMatchResult(ItemGuid.Value);

            if (dr != null)
            {
                if (Convert.IsDBNull(dr["Home"]))
                    Home = null;
                else
                    Home = Convert.ToInt16(dr["Home"]);

                if (Convert.IsDBNull(dr["Away"]))
                    Away = null;
                else
                    Away = Convert.ToInt16(dr["Away"]);
            }
        }

        public override Guid Save(SqlTransaction trans)
        {
            if (!ItemGuid.HasValue)
            {
                //insert

                var newGuid = base.Save(trans);

                DataAccess.MatchResult.InsertMatchResult(newGuid, trans);

                return newGuid;
            }
            else
            {
                //update

                if (Home.HasValue && Away.HasValue && Earning.HasValue)
                {
                    DataAccess.MatchResult.UpdateMatchResult(ItemGuid.Value, Home.Value, Away.Value, trans);

                    base.Save(trans);
                }

                return ItemGuid.Value;
            }
        }

        public short? Home
        { get; set; }

        public short? Away
        { get; set; }
    }

    public class MatchResultBetDetail
    {
        public static readonly string BetDetailHomeName = "Home";
        public static readonly string BetDetailAwayName = "Away";

        public MatchResultBetDetail() { }

        public MatchResultBetDetail(DataTable dt)
        {
            foreach (DataRow dr in dt.Rows)
            {
                var detailName = dr["DetailName"].ToString();

                if (detailName == BetDetailHomeName)
                {
                    Home = Convert.ToInt16(dr["DetailValue"]);
                }
                else if (detailName == BetDetailAwayName)
                {
                    Away = Convert.ToInt16(dr["DetailValue"]);
                }
            }
        }

        public void Save(int betID, SqlTransaction trans)
        {
            DataAccess.BetDetail.InsertBetDetail(betID, BetDetailHomeName, Home.ToString(), trans);
            DataAccess.BetDetail.InsertBetDetail(betID, BetDetailAwayName, Away.ToString(), trans);
        }

        public short Home
        { get; set; }

        public short Away
        { get; set; }
    }
}
