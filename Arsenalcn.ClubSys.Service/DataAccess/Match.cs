using System;
using System.Data;

using Microsoft.ApplicationBlocks.Data;
using Arsenalcn.Common;

namespace Arsenalcn.ClubSys.DataAccess
{
    public class Match
    {
        public static DataTable GetMatches()
        {
            string sql = "SELECT * FROM Arsenal_Match ORDER BY PlayTime DESC";

            DataSet ds = SqlHelper.ExecuteDataset(SQLConn.GetConnection(), CommandType.Text, sql);

            if (ds.Tables[0].Rows.Count == 0)
                return null;
            else
                return ds.Tables[0];
        }
    }
}
