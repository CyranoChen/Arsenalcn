using System.Data;
using Arsenalcn.Common;
using Microsoft.ApplicationBlocks.Data;

namespace Arsenalcn.ClubSys.DataAccess
{
    public class Match
    {
        public static DataTable GetMatches()
        {
            var sql = "SELECT * FROM Arsenal_Match ORDER BY PlayTime DESC";

            var ds = SqlHelper.ExecuteDataset(SQLConn.GetConnection(), CommandType.Text, sql);

            if (ds.Tables[0].Rows.Count == 0)
                return null;
            return ds.Tables[0];
        }
    }
}