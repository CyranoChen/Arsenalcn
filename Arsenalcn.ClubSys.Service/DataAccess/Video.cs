using System.Data;
using Arsenalcn.Common;
using Microsoft.ApplicationBlocks.Data;

namespace Arsenalcn.ClubSys.DataAccess
{
    public class Video
    {
        public static DataTable GetVideos()
        {
            var sql = "SELECT * FROM Arsenal_Video ORDER BY GoalYear DESC, GoalRank DESC, TeamworkRank DESC";

            var ds = SqlHelper.ExecuteDataset(SQLConn.GetConnection(), CommandType.Text, sql);

            if (ds.Tables[0].Rows.Count == 0)
                return null;
            return ds.Tables[0];
        }
    }
}