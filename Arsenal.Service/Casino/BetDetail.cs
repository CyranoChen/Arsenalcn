using System.Data.SqlClient;

using Arsenalcn.Core;

namespace Arsenal.Service.Casino
{
    [DbSchema("AcnCasino_BetDetail", Sort = "ID DESC")]
    public class BetDetail : Entity<int>
    {
        public BetDetail() : base() { }

        public static void Clean(SqlTransaction trans = null)
        {
            //DELETE FROM AcnCasino_BetDetail WHERE (BetID NOT IN (SELECT ID FROM AcnCasino_Bet))
            var sql = string.Format(@"DELETE FROM {0} WHERE (BetID NOT IN (SELECT ID FROM {1}))",
                   Repository.GetTableAttr<BetDetail>().Name,
                   Repository.GetTableAttr<Bet>().Name);

            DataAccess.ExecuteNonQuery(sql, null, trans);
        }

        #region Members and Properties

        [DbColumn("BetID")]
        public int BetID
        { get; set; }

        [DbColumn("DetailName")]
        public string DetailName
        { get; set; }

        [DbColumn("DetailValue")]
        public string DetailValue
        { get; set; }

        #endregion
    }
}
