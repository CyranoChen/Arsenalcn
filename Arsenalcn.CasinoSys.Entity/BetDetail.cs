using System.Data;

namespace Arsenalcn.CasinoSys.Entity
{
    public class BetDetail
    {
        public static DataTable GetBetDetailByBetId(int id)
        {
            return DataAccess.BetDetail.GetBetDetailByBetId(id);
        }
    }
}