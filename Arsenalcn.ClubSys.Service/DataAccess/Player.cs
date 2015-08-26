using System.Data;

using Microsoft.ApplicationBlocks.Data;
using Arsenalcn.Common;

namespace Arsenalcn.ClubSys.DataAccess
{
    public class Player
    {
        public static DataTable GetPlayers()
        {
            var sql = "SELECT * FROM Arsenal_Player ORDER BY IsLegend, IsLoan, SquadNumber, LastName";

            var ds = SqlHelper.ExecuteDataset(SQLConn.GetConnection(), CommandType.Text, sql);

            if (ds.Tables[0].Rows.Count == 0)
                return null;
            else
                return ds.Tables[0];
        }
    }
}
