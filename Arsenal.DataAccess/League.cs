using System;
using System.Data;
using System.Data.SqlClient;

using Arsenalcn.Common;
using Microsoft.ApplicationBlocks.Data;

namespace Arsenal.DataAccess
{
    public class League
    {
        public static DataRow GetLeagueByID(Guid leagueGuid)
        {
            string sql = "SELECT * FROM dbo.Arsenal_League WHERE LeagueGuid = @leagueGuid";

            DataSet ds = SqlHelper.ExecuteDataset(SQLConn.GetConnection(), CommandType.Text, sql, new SqlParameter("@leagueGuid", leagueGuid));

            if (ds.Tables[0].Rows.Count == 0)
                return null;
            else
                return ds.Tables[0].Rows[0];
        }

        //public static DataTable GetLeagueAllSeason(Guid league)
        //{
        //    string sql = "SELECT * FROM dbo.Arsenal_League WHERE LeagueName in (SELECT LeagueName FROM dbo.Arsenal_League WHERE LeagueGuid = @guid) ORDER BY LeagueOrder, LeagueOrgName";

        //    DataSet ds = SqlHelper.ExecuteDataset(Common.GetConnection(), CommandType.Text, sql, new SqlParameter("@guid", league));

        //    if (ds.Tables[0].Rows.Count == 0)
        //        return null;
        //    else
        //        return ds.Tables[0];
        //}

        //public static DataTable GetAllLeagues(bool isActive)
        //{
        //    string sql = "SELECT * FROM dbo.Arsenal_League WHERE IsActive = @isActive ORDER BY LeagueOrder, LeagueOrgName";

        //    DataSet ds = SqlHelper.ExecuteDataset(Common.GetConnection(), CommandType.Text, sql, new SqlParameter("@isActive", isActive));

        //    if (ds.Tables[0].Rows.Count == 0)
        //        return null;
        //    else
        //        return ds.Tables[0];
        //}

        public static void UpdateLeague(Guid leagueGuid, string leagueName, string leagueOrgName, string leagueSeason, DateTime leagueTime, string leagueLogo, int leagueOrder, bool isActive)
        {
            string sql = "UPDATE dbo.Arsenal_League SET LeagueName = @leagueName, LeagueOrgName = @leagueOrgName, LeagueSeason = @leagueSeason, LeagueTime = @leagueTime, LeagueLogo = @leagueLogo, LeagueOrder = @LeagueOrder, IsActive = @isActive WHERE LeagueGuid = @leagueGuid";

            SqlParameter[] para = { new SqlParameter("@leagueGuid", leagueGuid), new SqlParameter("@leagueName", leagueName), new SqlParameter("@leagueOrgName", leagueOrgName), new SqlParameter("@leagueSeason", leagueSeason), new SqlParameter("@leagueTime", leagueTime), new SqlParameter("@leagueLogo", leagueLogo), new SqlParameter("@leagueOrder", leagueOrder), new SqlParameter("@isActive", isActive) };

            SqlHelper.ExecuteNonQuery(SQLConn.GetConnection(), CommandType.Text, sql, para);
        }

        public static void InsertLeague(Guid leagueGuid, string leagueName, string leagueOrgName, string leagueSeason, DateTime leagueTime, string leagueLogo, int leagueOrder, bool isActive)
        {
            string sql = "INSERT INTO dbo.Arsenal_League (LeagueName, LeagueOrgName, LeagueSeason, LeagueTime, LeagueLogo, LeagueOrder, IsActive) VALUES (@leagueName, @leagueOrgName, @leagueSeason, @leagueTime, @leagueLogo, @leagueOrder, @isActive)";

            SqlParameter[] para = { new SqlParameter(), new SqlParameter("@leagueName", leagueName), new SqlParameter("@leagueOrgName", leagueOrgName), new SqlParameter("@leagueSeason", leagueSeason), new SqlParameter("@leagueTime", leagueTime), new SqlParameter("@leagueLogo", leagueLogo), new SqlParameter("@leagueOrder", leagueOrder), new SqlParameter("@isActive", isActive) };

            SqlHelper.ExecuteNonQuery(SQLConn.GetConnection(), CommandType.Text, sql, para);
        }

        public static void DeleteLeague(Guid leagueGuid)
        {
            string sql = "DELETE dbo.Arsenal_League WHERE LeagueGuid = @leagueGuid";

            SqlParameter[] para = { new SqlParameter("@leagueGuid", leagueGuid) };

            SqlHelper.ExecuteNonQuery(SQLConn.GetConnection(), CommandType.Text, sql, para);
        }

        public static DataTable GetLeagues()
        {
            string sql = @"SELECT LeagueGuid, LeagueName, LeagueOrgName, LeagueSeason, LeagueTime, LeagueLogo, IsActive, LeagueOrder
                                FROM Arsenal_League ORDER BY LeagueOrder, LeagueOrgName";

            DataSet ds = SqlHelper.ExecuteDataset(SQLConn.GetConnection(), CommandType.Text, sql);

            if (ds.Tables[0].Rows.Count == 0)
                return null;
            else
                return ds.Tables[0];
        }
    }
}
