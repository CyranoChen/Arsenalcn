using System;
using System.Data;
using System.Data.SqlClient;

using Arsenalcn.Common;
using Microsoft.ApplicationBlocks.Data;

namespace Arsenal.DataAccess
{
    public class RelationLeagueTeam
    {
        public static DataRow GetRelationLeagueTeamByID(Guid teamGuid, Guid leagueGuid)
        {
            string sql = "SELECT * FROM dbo.Arsenal_RelationLeagueTeam WHERE TeamGuid = @teamGuid AND LeagueGuid = @leagueGuid";

            DataSet ds = SqlHelper.ExecuteDataset(SQLConn.GetConnection(), CommandType.Text, sql, new SqlParameter("@teamGuid", teamGuid), new SqlParameter("@leagueGuid", leagueGuid));

            if (ds.Tables[0].Rows.Count == 0)
                return null;
            else
                return ds.Tables[0].Rows[0];
        }

        public static void InsertRelationLeagueTeam(Guid teamGuid, Guid leagueGuid)
        {
            string sql = " INSERT INTO dbo.Arsenal_RelationLeagueTeam (TeamGuid, LeagueGuid) VALUES (@teamGuid, @leagueGuid)";

            SqlParameter[] para = { new SqlParameter("@teamGuid", teamGuid), new SqlParameter("@leagueGuid", leagueGuid) };

            SqlHelper.ExecuteNonQuery(SQLConn.GetConnection(), CommandType.Text, sql, para);
        }

        public static void DeleteRelationLeagueTeam(Guid teamGuid, Guid leagueGuid)
        {
            string sql = "DELETE FROM dbo.Arsenal_RelationLeagueTeam WHERE TeamGuid = @teamGuid AND LeagueGuid = @leagueGuid";

            SqlParameter[] para = { new SqlParameter("@teamGuid", teamGuid), new SqlParameter("@leagueGuid", leagueGuid) };

            SqlHelper.ExecuteNonQuery(SQLConn.GetConnection(), CommandType.Text, sql, para);
        }

        public static DataTable GetRelationLeagueTeams()
        {
            string sql = @"SELECT TeamGuid, LeagueGuid FROM dbo.Arsenal_RelationLeagueTeam";

            DataSet ds = SqlHelper.ExecuteDataset(SQLConn.GetConnection(), CommandType.Text, sql);

            if (ds.Tables[0].Rows.Count == 0)
                return null;
            else
                return ds.Tables[0];
        }

        public static void CleanRelationLeagueTeam()
        {
            string sql = @"DELETE FROM Arsenal_RelationLeagueTeam WHERE (TeamGuid NOT IN (SELECT TeamGuid FROM Arsenal_Team)) OR
                               (LeagueGuid NOT IN (SELECT LeagueGuid FROM Arsenal_League))";

            SqlHelper.ExecuteNonQuery(SQLConn.GetConnection(), CommandType.Text, sql);
        }
    }
}
