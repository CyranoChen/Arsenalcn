using System;
using System.Data;
using System.Data.SqlClient;

using Microsoft.ApplicationBlocks.Data;

namespace Arsenal.DataAccess
{
    public class Match
    {
        public static DataRow GetMatchByID(Guid matchGuid)
        {
            string sql = "SELECT * FROM dbo.Arsenal_Match WHERE MatchGuid = @matchGuid";

            DataSet ds = SqlHelper.ExecuteDataset(SQLConn.GetConnection(), CommandType.Text, sql, new SqlParameter("@matchGuid", matchGuid));

            if (ds.Tables[0].Rows.Count == 0)
                return null;
            else
                return ds.Tables[0].Rows[0];
        }

        public static void UpdateMatch(Guid matchGuid, Guid teamGuid, string teamName, Boolean isHome, int? resultHome, int? resultAway, DateTime playTime, Guid? leagueGuid, string leagueName, int? round, Guid? groupGuid, Guid? casinoMatchGuid, string reportImageURL, string reportURL, string topicURL, Boolean isActive, string remark)
        {
            string sql = @"UPDATE dbo.Arsenal_Match SET TeamGuid = @teamGuid, TeamName = @teamName, IsHome = @isHome, ResultHome = @resultHome, ResultAway = @resultAway, PlayTime = @playTime, LeagueGuid = @leagueGuid, LeagueName = @leagueName, 
                               Round = @round, GroupGuid = @groupGuid, CasinoMatchGuid = @casinoMatchGuid, ReportImageURL = @reportImageURL, ReportURL = @reportURL, TopicURL = @topicURL, IsActive = @isActive, Remark = @remark WHERE MatchGuid = @matchGuid";

            SqlParameter[] para = new SqlParameter[17];

            para[0] = new SqlParameter("@matchGuid", matchGuid);
            para[1] = new SqlParameter("@teamGuid", teamGuid);
            para[2] = new SqlParameter("@teamName", teamName);
            para[3] = new SqlParameter("@isHome", isHome);
            para[4] = new SqlParameter("@resultHome", !resultHome.HasValue ? (object)DBNull.Value : (object)resultHome.Value);
            para[5] = new SqlParameter("@resultAway", !resultAway.HasValue ? (object)DBNull.Value : (object)resultAway.Value);
            para[6] = new SqlParameter("@playTime", playTime);
            para[7] = new SqlParameter("@leagueGuid", !leagueGuid.HasValue ? (object)DBNull.Value : (object)leagueGuid.Value);
            para[8] = new SqlParameter("@leagueName", leagueName);
            para[9] = new SqlParameter("@round", !round.HasValue ? (object)DBNull.Value : (object)round.Value);
            para[10] = new SqlParameter("@groupGuid", !groupGuid.HasValue ? (object)DBNull.Value : (object)groupGuid.Value);
            para[11] = new SqlParameter("@casinoMatchGuid", !casinoMatchGuid.HasValue ? (object)DBNull.Value : (object)casinoMatchGuid.Value);
            para[12] = new SqlParameter("@reportImageURL", reportImageURL);
            para[13] = new SqlParameter("@reportURL", reportURL);
            para[14] = new SqlParameter("@topicURL", topicURL);
            para[15] = new SqlParameter("@isActive", isActive);
            para[16] = new SqlParameter("@remark", remark);

            SqlHelper.ExecuteNonQuery(SQLConn.GetConnection(), CommandType.Text, sql, para);
        }

        public static void InsertMatch(Guid matchGuid, Guid teamGuid, string teamName, Boolean isHome, int? resultHome, int? resultAway, DateTime playTime, Guid? leagueGuid, string leagueName, int? round, Guid? groupGuid, Guid? casinoMatchGuid, string reportImageURL, string reportURL, string topicURL, Boolean isActive, string remark)
        {
            string sql = @"INSERT INTO dbo.Arsenal_Match (TeamGuid, TeamName, IsHome, ResultHome, ResultAway, PlayTime, LeagueGuid, LeagueName, Round, GroupGuid, CasinoMatchGuid, ReportImageURL, ReportURL, TopicURL, IsActive, Remark) 
                               VALUES (@teamGuid, @teamName, @isHome, @resultHome, @resultAway, @playTime, @leagueGuid, @leagueName, @round, @groupGuid, @casinoMatchGuid, @reportImageURL, @reportURL, @topicURL, @isActive, @remark)";

            SqlParameter[] para = new SqlParameter[17];

            para[0] = new SqlParameter();
            para[1] = new SqlParameter("@teamGuid", teamGuid);
            para[2] = new SqlParameter("@teamName", teamName);
            para[3] = new SqlParameter("@isHome", isHome);
            para[4] = new SqlParameter("@resultHome", !resultHome.HasValue ? (object)DBNull.Value : (object)resultHome.Value);
            para[5] = new SqlParameter("@resultAway", !resultAway.HasValue ? (object)DBNull.Value : (object)resultAway.Value);
            para[6] = new SqlParameter("@playTime", playTime);
            para[7] = new SqlParameter("@leagueGuid", !leagueGuid.HasValue ? (object)DBNull.Value : (object)leagueGuid.Value);
            para[8] = new SqlParameter("@leagueName", leagueName);
            para[9] = new SqlParameter("@round", !round.HasValue ? (object)DBNull.Value : (object)round.Value);
            para[10] = new SqlParameter("@groupGuid", !groupGuid.HasValue ? (object)DBNull.Value : (object)groupGuid.Value);
            para[11] = new SqlParameter("@casinoMatchGuid", !casinoMatchGuid.HasValue ? (object)DBNull.Value : (object)casinoMatchGuid.Value);
            para[12] = new SqlParameter("@reportImageURL", reportImageURL);
            para[13] = new SqlParameter("@reportURL", reportURL);
            para[14] = new SqlParameter("@topicURL", topicURL);
            para[15] = new SqlParameter("@isActive", isActive);
            para[16] = new SqlParameter("@remark", remark);

            SqlHelper.ExecuteNonQuery(SQLConn.GetConnection(), CommandType.Text, sql, para);
        }

        public static void DeleteMatch(Guid matchGuid)
        {
            string sql = "DELETE dbo.Arsenal_Match WHERE MatchGuid = @matchGuid";

            SqlParameter[] para = { new SqlParameter("@matchGuid", matchGuid) };

            SqlHelper.ExecuteNonQuery(SQLConn.GetConnection(), CommandType.Text, sql, para);
        }

        public static DataTable GetMatchs()
        {
            string sql = @"SELECT MatchGuid, TeamGuid, TeamName, IsHome, ResultHome, ResultAway, PlayTime, LeagueGuid, LeagueName, Round, GroupGuid, 
                               CasinoMatchGuid, ReportImageURL, ReportURL, TopicURL, IsActive, Remark FROM dbo.Arsenal_Match ORDER BY PlayTime DESC";

            DataSet ds = SqlHelper.ExecuteDataset(SQLConn.GetConnection(), CommandType.Text, sql);

            if (ds.Tables[0].Rows.Count == 0)
                return null;
            else
                return ds.Tables[0];
        }
    }
}
