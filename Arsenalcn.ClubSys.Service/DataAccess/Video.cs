using System;
using System.Data;

using Microsoft.ApplicationBlocks.Data;
using Arsenalcn.Common;

namespace Arsenalcn.ClubSys.DataAccess
{
    public class Video
    {
        public static DataTable GetVideos()
        {
            string sql = "SELECT * FROM Arsenal_Video ORDER BY GoalYear DESC, GoalRank DESC, TeamworkRank DESC";

            DataSet ds = SqlHelper.ExecuteDataset(SQLConn.GetConnection(), CommandType.Text, sql);

            if (ds.Tables[0].Rows.Count == 0)
                return null;
            else
                return ds.Tables[0];
        }
    }
}
