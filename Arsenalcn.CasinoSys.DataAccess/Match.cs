using System;
using System.Data;
using System.Data.SqlClient;

using Arsenalcn.Common;
using Microsoft.ApplicationBlocks.Data;

namespace Arsenalcn.CasinoSys.DataAccess
{
    public class Match
    {
        public static DataRow GetMatchByID(Guid matchGuid)
        {
            string sql = "SELECT * FROM dbo.AcnCasino_Match WHERE MatchGuid = @guid";

            DataSet ds = SqlHelper.ExecuteDataset(SQLConn.GetConnection(), CommandType.Text, sql, new SqlParameter("@guid", matchGuid));

            if (ds.Tables[0].Rows.Count == 0)
                return null;
            else
                return ds.Tables[0].Rows[0];
        }

        public static Guid GetRandomOpenMatch()
        {
            string sql = "SELECT TOP 1 MatchGuid FROM dbo.AcnCasino_Match WHERE PlayTime > getdate() ORDER BY NEWID()";

            Guid? matchGuid = SqlHelper.ExecuteScalar(SQLConn.GetConnection(), CommandType.Text, sql) as Guid?;

            return matchGuid.GetValueOrDefault(Guid.Empty);
        }

        public static void UpdateMatch(Guid matchGuid, Guid home, Guid away, short? resultHome, short? resultAway, DateTime playTime, Guid leagueGuid, string leagueName, int? round, Guid? groupGuid)
        {
            string sql = "UPDATE dbo.AcnCasino_Match SET Home = @home, Away = @away, ResultHome = @resultHome, ResultAway = @resultAway, PlayTime = @playTime, LeagueGuid = @leagueGuid, LeagueName = @leagueName, Round = @round, GroupGuid = @groupGuid WHERE MatchGuid = @guid";

            SqlParameter[] para = new SqlParameter[10];

            para[0] = new SqlParameter("@guid", matchGuid);
            para[1] = new SqlParameter("@home", home);
            para[2] = new SqlParameter("@away", away);

            if (resultHome.HasValue)
                para[3] = new SqlParameter("@resultHome", resultHome);
            else
                para[3] = new SqlParameter("@resultHome", DBNull.Value);

            if (resultAway.HasValue)
                para[4] = new SqlParameter("@resultAway", resultAway);
            else
                para[4] = new SqlParameter("@resultAway", DBNull.Value);

            para[5] = new SqlParameter("@playTime", playTime);
            para[6] = new SqlParameter("@leagueGuid", leagueGuid);
            para[7] = new SqlParameter("@leagueName", leagueName);

            if (round.HasValue)
                para[8] = new SqlParameter("@round", round);
            else
                para[8] = new SqlParameter("@round", DBNull.Value);

            if (groupGuid.HasValue)
                para[9] = new SqlParameter("groupGuid", groupGuid);
            else
                para[9] = new SqlParameter("groupGuid", DBNull.Value);

            SqlHelper.ExecuteNonQuery(SQLConn.GetConnection(), CommandType.Text, sql, para);
        }

        public static void InsertMatch(Guid matchGuid, Guid home, Guid away, DateTime playTime, Guid leagueGuid, string leagueName, int? round, Guid? groupGuid, SqlTransaction trans)
        {
            string sql = "INSERT INTO dbo.AcnCasino_Match (MatchGuid,Home,Away,ResultHome,ResultAway,PlayTime,LeagueGuid,LeagueName,Round,GroupGuid) VALUES (@guid, @home, @away, null, null, @time, @league, @leagueName, @round, @groupGuid)";

            SqlParameter[] para = new SqlParameter[8];
            para[0] = new SqlParameter("@guid", matchGuid);
            para[1] = new SqlParameter("@home", home);
            para[2] = new SqlParameter("@away", away);
            para[3] = new SqlParameter("@time", playTime);
            para[4] = new SqlParameter("@league", leagueGuid);
            para[5] = new SqlParameter("@leagueName", leagueName);

            if (round.HasValue)
                para[6] = new SqlParameter("@round", round);
            else
                para[6] = new SqlParameter("@round", DBNull.Value);

            if (groupGuid.HasValue)
                para[7] = new SqlParameter("@groupGuid", groupGuid);
            else
                para[7] = new SqlParameter("@groupGuid", DBNull.Value);

            if (trans == null)
                SqlHelper.ExecuteNonQuery(SQLConn.GetConnection(), CommandType.Text, sql, para);
            else
                SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sql, para);
        }

        public static void DeleteMatch(Guid matchGuid)
        {
            string sql = "DELETE FROM dbo.AcnCasino_Match WHERE MatchGuid = @guid";

            SqlHelper.ExecuteNonQuery(SQLConn.GetConnection(), CommandType.Text, sql, new SqlParameter("@guid", matchGuid));
        }

        public static DataTable GetMatchs()
        {
            string sql = @"SELECT MatchGuid, Home, Away, ResultHome, ResultAway, PlayTime, LeagueGuid, LeagueName, Round, GroupGuid 
                               FROM dbo.AcnCasino_Match ORDER BY PlayTime DESC";

            DataSet ds = SqlHelper.ExecuteDataset(SQLConn.GetConnection(), CommandType.Text, sql);

            if (ds.Tables[0].Rows.Count == 0)
                return null;
            else
                return ds.Tables[0];
        }

        public static void UpdateMatchResult(Guid matchGuid, short resultHome, short resultAway)
        {
            string sql = "UPDATE dbo.AcnCasino_Match SET ResultHome = @resultHome, ResultAway = @resultAway WHERE MatchGuid = @guid";

            SqlHelper.ExecuteNonQuery(SQLConn.GetConnection(), CommandType.Text, sql, new SqlParameter("@resultHome", resultHome), new SqlParameter("@resultAway", resultAway), new SqlParameter("@guid", matchGuid));
        }

        public static void RemoveMatchGroupGuid(Guid groupGuid)
        {
            string sql = "UPDATE dbo.AcnCasino_Match SET GroupGuid = NULL WHERE GroupGuid = @groupGuid";

            SqlHelper.ExecuteNonQuery(SQLConn.GetConnection(), CommandType.Text, sql, new SqlParameter("@groupGuid", groupGuid));
        }

        public static void UpdateMatchGroupGuid(Guid groupGuid, Guid leagueGuid)
        {
            string sql = @"UPDATE dbo.AcnCasino_Match SET GroupGuid = @groupGuid WHERE LeagueGuid = @leagueGuid 
                          AND Home in (Select TeamGuid from dbo.Arsenal_RelationGroupTeam where GroupGuid = @groupGuid)
                          AND Away in (Select TeamGuid from dbo.Arsenal_RelationGroupTeam where GroupGuid = @groupGuid)";

            SqlHelper.ExecuteNonQuery(SQLConn.GetConnection(), CommandType.Text, sql, new SqlParameter("@groupGuid", groupGuid), new SqlParameter("@leagueGuid", leagueGuid));
        }

        public static DataTable GetResultMatchByLeagueGuid(Guid leagueGuid)
        {
            string sql = @"SELECT * FROM dbo.AcnCasino_Match WHERE (LeagueGuid = @leagueGuid) AND
                          (ResultHome IS NOT NULL) AND (ResultAway IS NOT NULL) ORDER BY PlayTime DESC";

            DataSet ds = SqlHelper.ExecuteDataset(SQLConn.GetConnection(), CommandType.Text, sql, new SqlParameter("@leagueGuid", leagueGuid));

            if (ds.Tables[0].Rows.Count == 0)
                return null;
            else
                return ds.Tables[0];
        }

        public static DataTable GetResultMatchByGroupGuid(Guid groupGuid, bool isTable)
        {
            string sql = string.Empty;
            if (!isTable)
                sql = @"SELECT * FROM dbo.AcnCasino_Match WHERE (GroupGuid =@groupGuid) AND (ResultHome IS NOT NULL) AND (ResultAway IS NOT NULL) 
                        AND (LeagueGuid = (SElECT LeagueGuid FROM dbo.Arsenal_Group WHERE GroupGuid = @groupGuid)) ORDER BY PlayTime DESC";
            else
                sql = @"SELECT * FROM dbo.AcnCasino_Match WHERE (ResultHome IS NOT NULL) AND (ResultAway IS NOT NULL) AND 
                        (Home IN (SELECT TeamGuid FROM dbo.Arsenal_RelationGroupTeam AS GroupTeam1 WHERE GroupGuid = @groupGuid)) AND 
                        (Away IN (SELECT TeamGuid FROM dbo.Arsenal_RelationGroupTeam AS GroupTeam2 WHERE GroupGuid = @groupGuid)) AND
                        (LeagueGuid = (SELECT LeagueGuid FROM dbo.Arsenal_Group WHERE GroupGuid = @groupGuid))
                        ORDER BY PlayTime DESC";

            DataSet ds = SqlHelper.ExecuteDataset(SQLConn.GetConnection(), CommandType.Text, sql, new SqlParameter("@groupGuid", groupGuid));

            if (ds.Tables[0].Rows.Count == 0)
                return null;
            else
                return ds.Tables[0];
        }

        public static DataTable GetAllMatchByGroupGuid(Guid groupGuid, bool isTable)
        {
            string sql = string.Empty;
            if (!isTable)
                sql = @"SELECT * FROM dbo.AcnCasino_Match WHERE (GroupGuid =@groupGuid) AND 
                        (LeagueGuid = (SElECT LeagueGuid FROM dbo.Arsenal_Group WHERE GroupGuid = @groupGuid)) ORDER BY PlayTime DESC";
            else
                sql = @"SELECT * FROM dbo.AcnCasino_Match WHERE  
                        (Home IN (SELECT TeamGuid FROM dbo.Arsenal_RelationGroupTeam AS GroupTeam1 WHERE GroupGuid = @groupGuid)) AND 
                        (Away IN (SELECT TeamGuid FROM dbo.Arsenal_RelationGroupTeam AS GroupTeam2 WHERE GroupGuid = @groupGuid)) AND
                        (LeagueGuid = (SELECT LeagueGuid FROM dbo.Arsenal_Group WHERE GroupGuid = @groupGuid))
                        ORDER BY PlayTime DESC";

            DataSet ds = SqlHelper.ExecuteDataset(SQLConn.GetConnection(), CommandType.Text, sql, new SqlParameter("@groupGuid", groupGuid));

            if (ds.Tables[0].Rows.Count == 0)
                return null;
            else
                return ds.Tables[0];
        }
    }
}
