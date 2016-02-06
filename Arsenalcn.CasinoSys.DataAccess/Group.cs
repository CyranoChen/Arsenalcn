using System;
using System.Data;
using System.Data.SqlClient;
using Arsenalcn.Common;
using Microsoft.ApplicationBlocks.Data;

namespace Arsenalcn.CasinoSys.DataAccess
{
    public class Group
    {
        public static DataRow GetGroupByID(Guid groupGuid)
        {
            var sql = "SELECT * FROM dbo.Arsenal_Group WHERE GroupGuid = @guid";

            var ds = SqlHelper.ExecuteDataset(SQLConn.GetConnection(), CommandType.Text, sql,
                new SqlParameter("@guid", groupGuid));

            if (ds.Tables[0].Rows.Count == 0)
                return null;
            return ds.Tables[0].Rows[0];
        }

        public static DataRow GetGroupTeamInfo(Guid groupGuid, Guid teamGuid, SqlTransaction trans)
        {
            var sql =
                "SELECT * FROM dbo.Arsenal_RelationGroupTeam WHERE GroupGuid = @groupGuid AND TeamGuid = @teamGuid";

            DataSet ds;
            if (trans == null)
                ds = SqlHelper.ExecuteDataset(SQLConn.GetConnection(), CommandType.Text, sql,
                    new SqlParameter("@groupGuid", groupGuid), new SqlParameter("@teamGuid", teamGuid));
            else
                ds = SqlHelper.ExecuteDataset(trans, CommandType.Text, sql, new SqlParameter("@groupGuid", groupGuid),
                    new SqlParameter("@teamGuid", teamGuid));

            if (ds.Tables[0].Rows.Count == 0)
                return null;
            return ds.Tables[0].Rows[0];
        }

        public static void UpdateGroup(Guid groupGuid, string groupName, int groupOrder, Guid leagueGuid, bool isTable)
        {
            var sql =
                "UPDATE dbo.Arsenal_Group SET GroupName = @groupName, GroupOrder = @groupOrder, LeagueGuid = @leagueGuid, IsTable = @isTable WHERE GroupGuid = @groupGuid";

            SqlParameter[] para =
            {
                new SqlParameter("@groupGuid", groupGuid), new SqlParameter("@groupName", groupName),
                new SqlParameter("@groupOrder", groupOrder), new SqlParameter("@leagueGuid", leagueGuid),
                new SqlParameter("@isTable", isTable)
            };

            SqlHelper.ExecuteNonQuery(SQLConn.GetConnection(), CommandType.Text, sql, para);
        }

        public static void InsertGroup(Guid groupGuid, string groupName, int groupOrder, Guid leagueGuid, bool isTable)
        {
            var sql =
                "INSERT INTO dbo.Arsenal_Group (GroupGuid, GroupName, GroupOrder, LeagueGuid, IsTable) VALUES (@groupGuid, @groupName, @groupOrder, @leagueGuid, @isTable)";

            SqlParameter[] para =
            {
                new SqlParameter("@groupGuid", groupGuid), new SqlParameter("@groupName", groupName),
                new SqlParameter("@groupOrder", groupOrder), new SqlParameter("@leagueGuid", leagueGuid),
                new SqlParameter("@isTable", isTable)
            };

            SqlHelper.ExecuteNonQuery(SQLConn.GetConnection(), CommandType.Text, sql, para);
        }

        public static void DeleteGroup(Guid groupGuid)
        {
            var sql = "DELETE dbo.Arsenal_Group WHERE GroupGuid = @groupGuid";

            SqlParameter[] para = {new SqlParameter("@groupGuid", groupGuid)};

            SqlHelper.ExecuteNonQuery(SQLConn.GetConnection(), CommandType.Text, sql, para);
        }

        public static DataTable GetGroups()
        {
            var sql = @"SELECT * FROM dbo.Arsenal_Group Order By LeagueGuid, GroupOrder";

            var ds = SqlHelper.ExecuteDataset(SQLConn.GetConnection(), CommandType.Text, sql);

            if (ds.Tables[0].Rows.Count == 0)
                return null;
            return ds.Tables[0];
        }

        public static DataTable GetLeagueGroup(Guid leagueGuid)
        {
            var sql =
                @"SELECT * FROM dbo.Arsenal_Group WHERE dbo.Arsenal_Group.LeagueGuid = @guid ORDER BY dbo.Arsenal_Group.GroupOrder";

            var ds = SqlHelper.ExecuteDataset(SQLConn.GetConnection(), CommandType.Text, sql,
                new SqlParameter("@guid", leagueGuid));

            if (ds.Tables[0].Rows.Count == 0)
                return null;
            return ds.Tables[0];
        }

        public static DataTable GetLeagueGroup(Guid leagueGuid, bool isTable)
        {
            var sql = @"SELECT * FROM dbo.Arsenal_Group 
                           WHERE dbo.Arsenal_Group.LeagueGuid = @guid AND dbo.Arsenal_Group.IsTable = @isTable
                           ORDER BY dbo.Arsenal_Group.GroupOrder";

            var ds = SqlHelper.ExecuteDataset(SQLConn.GetConnection(), CommandType.Text, sql,
                new SqlParameter("@guid", leagueGuid), new SqlParameter("@isTable", isTable));

            if (ds.Tables[0].Rows.Count == 0)
                return null;
            return ds.Tables[0];
        }

        public static int GetRelationGroupTeamCount(Guid groupGuid, Guid teamGuid)
        {
            var sql =
                "SELECT COUNT(*) FROM dbo.Arsenal_RelationGroupTeam WHERE TeamGuid = @teamGuid AND GroupGuid = @groupGuid";

            var obj = SqlHelper.ExecuteScalar(SQLConn.GetConnection(), CommandType.Text, sql,
                new SqlParameter("@groupGuid", groupGuid), new SqlParameter("@teamGuid", teamGuid));

            return obj.Equals(DBNull.Value) ? 0 : Convert.ToInt32(obj);
        }

        public static DataTable GetRelationGroupTeamByGroupGuid(Guid groupGuid)
        {
            var sql =
                "SELECT * FROM dbo.Arsenal_RelationGroupTeam WHERE GroupGuid = @groupGuid ORDER BY TotalPoints DESC, ISNULL(HomeGoalDiff, 0) + ISNULL(AwayGoalDiff, 0) DESC, ISNULL(HomeGoalFor, 0) + ISNULL(AwayGoalFor, 0) DESC";

            var ds = SqlHelper.ExecuteDataset(SQLConn.GetConnection(), CommandType.Text, sql,
                new SqlParameter("@groupGuid", groupGuid));

            if (ds.Tables[0].Rows.Count == 0)
                return null;
            return ds.Tables[0];
        }

        public static DataTable GetTableGroupTeamByGroupGuid(Guid groupGuid)
        {
            var sql =
                @"SELECT GroupTeam.GroupGuid, GroupTeam.TeamGuid, GroupTeam.PositionNo, GroupTeam.TotalPlayed, GroupTeam.HomeWon, 
                      GroupTeam.HomeDraw, GroupTeam.HomeLost, GroupTeam.HomeGoalFor, GroupTeam.HomeGoalAgainst, GroupTeam.HomeGoalDiff, 
                      GroupTeam.HomePoints, GroupTeam.AwayWon, GroupTeam.AwayDraw, GroupTeam.AwayLost, GroupTeam.AwayGoalFor, 
                      GroupTeam.AwayGoalAgainst, GroupTeam.AwayGoalDiff, GroupTeam.AwayPoints, GroupTeam.TotalPoints,
                      ISNULL(GroupTeam.HomeWon, 0) + ISNULL(GroupTeam.AwayWon, 0) AS TotalWon, 
                      ISNULL(GroupTeam.HomeDraw, 0) + ISNULL(GroupTeam.AwayDraw, 0) AS TotalDraw, 
                      ISNULL(GroupTeam.HomeLost, 0) + ISNULL(GroupTeam.AwayLost, 0) AS TotalLost, 
                      ISNULL(GroupTeam.HomeGoalFor, 0) + ISNULL(GroupTeam.AwayGoalFor, 0) AS TotalGoalFor, 
                      ISNULL(GroupTeam.HomeGoalAgainst, 0) + ISNULL(GroupTeam.AwayGoalAgainst, 0) AS TotalGoalAgainst, 
                      ISNULL(GroupTeam.HomeGoalDiff, 0) + ISNULL(GroupTeam.AwayGoalDiff, 0) AS TotalGoalDiff 
                      FROM dbo.Arsenal_RelationGroupTeam AS GroupTeam
                      WHERE GroupTeam.GroupGuid = @groupGuid ORDER BY GroupTeam.PositionNo";

            var ds = SqlHelper.ExecuteDataset(SQLConn.GetConnection(), CommandType.Text, sql,
                new SqlParameter("@groupGuid", groupGuid));

            if (ds.Tables[0].Rows.Count == 0)
                return null;
            return ds.Tables[0];
        }

        public static DataTable GetRelationGroupTeamByLeagueGuid(Guid leagueGuid, Guid teamGuid)
        {
            var sql = @"SELECT * FROM dbo.Arsenal_RelationGroupTeam WHERE TeamGuid = @teamGuid AND GroupGuid IN 
                           (SELECT GroupGuid FROM dbo.Arsenal_Group WHERE LeagueGuid = @leagueGuid)";

            var ds = SqlHelper.ExecuteDataset(SQLConn.GetConnection(), CommandType.Text, sql,
                new SqlParameter("@leagueGuid", leagueGuid), new SqlParameter("@teamGuid", teamGuid));

            if (ds.Tables[0].Rows.Count == 0)
                return null;
            return ds.Tables[0];
        }

        public static void InsertRelationGroupTeam(Guid groupGuid, Guid teamGuid)
        {
            var sql = " INSERT INTO dbo.Arsenal_RelationGroupTeam (GroupGuid, TeamGuid) VALUES (@groupGuid, @teamGuid)";

            SqlParameter[] para = {new SqlParameter("@groupGuid", groupGuid), new SqlParameter("@teamGuid", teamGuid)};

            SqlHelper.ExecuteNonQuery(SQLConn.GetConnection(), CommandType.Text, sql, para);
        }

        public static void DeleteRelationGroupTeam(Guid groupGuid, Guid teamGuid)
        {
            var sql = "DELETE FROM dbo.Arsenal_RelationGroupTeam WHERE GroupGuid = @groupGuid AND TeamGuid = @teamGuid";

            SqlParameter[] para = {new SqlParameter("@groupGuid", groupGuid), new SqlParameter("@teamGuid", teamGuid)};

            SqlHelper.ExecuteNonQuery(SQLConn.GetConnection(), CommandType.Text, sql, para);
        }

        public static void UpdateRelationGroupTeam(Guid groupGuid, Guid teamGuid, short? positionNo, short? totalPlayed,
            short? homeWon, short? homeDraw, short? homeLost, short? homeGoalFor, short? homeGoalAgainst,
            short? homeGoalDiff, short? homePoints, short? awayWon, short? awayDraw, short? awayLost, short? awayGoalFor,
            short? awayGoalAgainst, short? awayGoalDiff, short? awayPoints, short? totalPoints)
        {
            var sql =
                "UPDATE dbo.Arsenal_RelationGroupTeam SET PositionNo = @positionNo, TotalPlayed = @totalPlayed, HomeWon = @homeWon, HomeDraw = @homeDraw, HomeLost = @homeLost, HomeGoalFor = @homeGoalFor, HomeGoalAgainst = @homeGoalAgainst, HomeGoalDiff = @homeGoalDiff, HomePoints = @homePoints, AwayWon = @awayWon, AwayDraw = @awayDraw, AwayLost = @awayLost, AwayGoalFor = @awayGoalFor, AwayGoalAgainst = @awayGoalAgainst, AwayGoalDiff = @awayGoalDiff, AwayPoints = @awayPoints, TotalPoints = @totalPoints WHERE GroupGuid = @groupGuid AND TeamGuid = @teamGuid";

            SqlParameter[] para =
            {
                new SqlParameter("@groupGuid", groupGuid), new SqlParameter("@teamGuid", teamGuid),
                new SqlParameter("@positionNo", positionNo), new SqlParameter("@totalPlayed", totalPlayed),
                new SqlParameter("@homeWon", homeWon), new SqlParameter("@homeDraw", homeDraw),
                new SqlParameter("@homeLost", homeLost), new SqlParameter("@homeGoalFor", homeGoalFor),
                new SqlParameter("@homeGoalAgainst", homeGoalAgainst), new SqlParameter("@homeGoalDiff", homeGoalDiff),
                new SqlParameter("@homePoints", homePoints), new SqlParameter("@awayWon", awayWon),
                new SqlParameter("@awayDraw", awayDraw), new SqlParameter("@awayLost", awayLost),
                new SqlParameter("@awayGoalFor", awayGoalFor), new SqlParameter("@awayGoalAgainst", awayGoalAgainst),
                new SqlParameter("@awayGoalDiff", awayGoalDiff), new SqlParameter("@awayPoints", awayPoints),
                new SqlParameter("@totalPoints", totalPoints)
            };

            SqlHelper.ExecuteNonQuery(SQLConn.GetConnection(), CommandType.Text, sql, para);
        }

        public static void RemoveRelationGroupAllTeam(Guid groupGuid)
        {
            var sql = "DELETE FROM dbo.Arsenal_RelationGroupTeam WHERE GroupGuid = @groupGuid";

            SqlParameter[] para = {new SqlParameter("@groupGuid", groupGuid)};

            SqlHelper.ExecuteNonQuery(SQLConn.GetConnection(), CommandType.Text, sql, para);
        }
    }
}