using System;
using System.Data;
using System.Data.SqlClient;

using Microsoft.ApplicationBlocks.Data;

namespace Arsenalcn.CasinoSys.DataAccess
{
    public class League
    {
        public static DataRow GetLeagueByID(Guid leagueGuid)
        {
            string sql = "SELECT * FROM dbo.Arsenal_League WHERE LeagueGuid = @guid";

            DataSet ds = SqlHelper.ExecuteDataset(SQLConn.GetConnection(), CommandType.Text, sql, new SqlParameter("@guid", leagueGuid));

            if (ds.Tables[0].Rows.Count == 0)
                return null;
            else
                return ds.Tables[0].Rows[0];
        }

        public static DataTable GetLeagueAllSeason(Guid league)
        {
            string sql = "SELECT * FROM dbo.Arsenal_League WHERE LeagueName in (SELECT LeagueName FROM dbo.Arsenal_League WHERE LeagueGuid = @guid) ORDER BY LeagueOrder, LeagueOrgName";

            DataSet ds = SqlHelper.ExecuteDataset(SQLConn.GetConnection(), CommandType.Text, sql, new SqlParameter("@guid", league));

            if (ds.Tables[0].Rows.Count == 0)
                return null;
            else
                return ds.Tables[0];
        }

        public static DataTable GetAllLeagues()
        {
            string sql = "SELECT * FROM dbo.Arsenal_League ORDER BY LeagueOrder, LeagueOrgName";

            DataSet ds = SqlHelper.ExecuteDataset(SQLConn.GetConnection(), CommandType.Text, sql);

            if (ds.Tables[0].Rows.Count == 0)
                return null;
            else
                return ds.Tables[0];
        }

        public static DataTable GetAllLeagues(bool isActive)
        {
            string sql = "SELECT * FROM dbo.Arsenal_League WHERE IsActive = @isActive ORDER BY LeagueOrder, LeagueOrgName";

            DataSet ds = SqlHelper.ExecuteDataset(SQLConn.GetConnection(), CommandType.Text, sql, new SqlParameter("@isActive", isActive));

            if (ds.Tables[0].Rows.Count == 0)
                return null;
            else
                return ds.Tables[0];
        }

        public static void UpdateLeague(Guid leagueGuid, string leagueName, string leagueOrgName, string leagueSeason, DateTime leagueTime, string leagueLogo, int leagueOrder, bool isActive)
        {
            string sql = "UPDATE dbo.Arsenal_League SET LeagueName = @leagueName, LeagueOrgName = @leagueOrgName, LeagueSeason = @leagueSeason, LeagueTime = @leagueTime, LeagueLogo = @leagueLogo, LeagueOrder = @LeagueOrder, IsActive = @isActive WHERE LeagueGuid = @guid";

            SqlParameter[] para = { new SqlParameter("@leagueName", leagueName), new SqlParameter("@leagueOrgName", leagueOrgName), new SqlParameter("@leagueSeason", leagueSeason), new SqlParameter("@leagueTime", leagueTime), new SqlParameter("@leagueLogo", leagueLogo), new SqlParameter("@leagueOrder", leagueOrder), new SqlParameter("@guid", leagueGuid), new SqlParameter("@isActive", isActive) };

            SqlHelper.ExecuteNonQuery(SQLConn.GetConnection(), CommandType.Text, sql, para);
        }

        public static void InsertLeague(string leagueName, string leagueOrgName, string leagueSeason, DateTime leagueTime, string leagueLogo, int leagueOrder, bool isActive)
        {
            string sql = "INSERT INTO dbo.Arsenal_League (LeagueGuid, LeagueName, LeagueOrgName, LeagueSeason, LeagueTime, LeagueLogo, LeagueOrder, IsActive) VALUES (@guid, @leagueName, @leagueOrgName, @leagueSeason, @leagueTime, @leagueLogo, @leagueOrder, @isActive)";

            Guid leagueGuid = Guid.NewGuid();

            SqlParameter[] para = { new SqlParameter("@leagueName", leagueName), new SqlParameter("@leagueOrgName", leagueOrgName), new SqlParameter("@leagueSeason", leagueSeason), new SqlParameter("@leagueTime", leagueTime), new SqlParameter("@leagueLogo", leagueLogo), new SqlParameter("@leagueOrder", leagueOrder), new SqlParameter("@guid", leagueGuid), new SqlParameter("@isActive", isActive) };

            SqlHelper.ExecuteNonQuery(SQLConn.GetConnection(), CommandType.Text, sql, para);
        }
    }
}
