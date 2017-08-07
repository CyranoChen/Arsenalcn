using System;
using Arsenalcn.Core;
using Arsenalcn.Core.Dapper;

namespace Arsenal.Service.Casino
{
    [DbSchema("AcnCasino_Banker")]
    public class Banker : Entity<Guid>
    {
        public void Statistic()
        {
            var sql =
                $@"SELECT ISNULL(SUM(b.BetAmount), 0) - ISNULL(SUM(b.Earning), 0) AS BankerCash
                           FROM {Repository.GetTableAttr<CasinoItem>().Name} c 
                           INNER JOIN {Repository.GetTableAttr<Bet>().Name} b ON c.CasinoItemGuid = b.CasinoItemGuid
                           WHERE (c.BankerID = @key)";

            using (IDapperHelper dapper = DapperHelper.GetInstance())
            {
                Cash = dapper.ExecuteScalar<double>(sql, new { key = ID });
            }

            using (IRepository repo = new Repository())
            {
                repo.Update(this);
            }
        }

        #region Members and Properties

        [DbColumn("BankerName")]
        public string BankerName { get; set; }

        [DbColumn("ClubID")]
        public int? ClubID { get; set; }

        [DbColumn("Cash")]
        public double Cash { get; set; }

        [DbColumn("IsActive")]
        public bool IsActive { get; set; }

        #endregion
    }
}