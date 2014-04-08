using System;
using System.Data;
using System.Data.SqlClient;

using Microsoft.ApplicationBlocks.Data;

namespace Arsenal.DataAccess
{
    public class Team
    {
        public static DataRow GetTeamByID(Guid teamGuid)
        {
            string sql = "SELECT * FROM dbo.Arsenal_Team WHERE TeamGuid = @teamGuid";

            DataSet ds = SqlHelper.ExecuteDataset(SQLConn.GetConnection(), CommandType.Text, sql, new SqlParameter("@teamGuid", teamGuid));

            if (ds.Tables[0].Rows.Count == 0)
                return null;
            else
                return ds.Tables[0].Rows[0];
        }

        public static void UpdateTeam(Guid teamGuid, string englishName, string displayName, string logo, string nickName, string founded, string ground, int? capacity, string chairman, string manager, Guid leagueGuid)
        {
            string sql = "UPDATE dbo.Arsenal_Team SET TeamEnglishName = @englishName, TeamDisplayName = @displayName, TeamLogo = @logo, TeamNickName = @nickName, Founded = @founded, Ground = @ground, Capacity = @capacity, Chairman = @chairman, Manager = @manager, LeagueGuid = @leagueGuid WHERE TeamGuid = @teamGuid";

            SqlParameter[] para = { new SqlParameter("@teamGuid", teamGuid), new SqlParameter("@englishName", englishName), new SqlParameter("@displayName", displayName), new SqlParameter("@logo", logo), new SqlParameter("@nickName", nickName), new SqlParameter("@founded", founded), new SqlParameter("@ground", ground), new SqlParameter("@capacity", capacity), new SqlParameter("@chairman", chairman), new SqlParameter("@manager", manager), new SqlParameter("@leagueGuid", leagueGuid) };

            SqlHelper.ExecuteNonQuery(SQLConn.GetConnection(), CommandType.Text, sql, para);
        }

        public static void InsertTeam(Guid teamGuid, string englishName, string displayName, string logo, string nickName, string founded, string ground, int? capacity, string chairman, string manager, Guid leagueGuid)
        {
            string sql = "INSERT INTO dbo.Arsenal_Team (TeamEnglishName, TeamDisplayName, TeamLogo, TeamNickName, Founded, Ground, Capacity, Chairman, Manager, LeagueGuid) VALUES (@englishName, @displayName, @logo, @nickName, @founded, @ground, @capacity, @chairman, @manager, @leagueGuid)";

            SqlParameter[] para = { new SqlParameter(), new SqlParameter("@englishName", englishName), new SqlParameter("@displayName", displayName), new SqlParameter("@logo", logo), new SqlParameter("@nickName", nickName), new SqlParameter("@founded", founded), new SqlParameter("@ground", ground), new SqlParameter("@capacity", capacity), new SqlParameter("@chairman", chairman), new SqlParameter("@manager", manager), new SqlParameter("@leagueGuid", leagueGuid) };

            SqlHelper.ExecuteNonQuery(SQLConn.GetConnection(), CommandType.Text, sql, para);
        }

        public static void DeleteTeam(Guid teamGuid)
        {
            string sql = "DELETE dbo.Arsenal_Team WHERE TeamGuid = @teamGuid";

            SqlParameter[] para = { new SqlParameter("@teamGuid", teamGuid) };

            SqlHelper.ExecuteNonQuery(SQLConn.GetConnection(), CommandType.Text, sql, para);
        }

        public static DataTable GetTeams()
        {
            string sql = @"SELECT TeamGuid, TeamEnglishName, TeamDisplayName, TeamLogo, TeamNickName, Founded, Ground, Capacity, Chairman, Manager, LeagueGuid FROM Arsenal_Team ORDER BY TeamEnglishName";

            DataSet ds = SqlHelper.ExecuteDataset(SQLConn.GetConnection(), CommandType.Text, sql);

            if (ds.Tables[0].Rows.Count == 0)
                return null;
            else
                return ds.Tables[0];
        }

//        public static DataTable GetLeagueTeams(Guid leagueGuid)
//        {
//            string sql = @"SELECT team.* FROM dbo.Arsenal_Team team
//                        INNER JOIN dbo.Arsenal_RelationLeagueTeam relation
//                        ON team.TeamGuid = relation.TeamGuid
//                        INNER JOIN dbo.Arsenal_League league
//                        ON league.LeagueGuid = relation.LeagueGuid
//                        WHERE relation.LeagueGuid = @guid ORDER BY TeamEnglishName";

//            DataSet ds = SqlHelper.ExecuteDataset(Common.GetConnection(), CommandType.Text, sql, new SqlParameter("@guid", leagueGuid));

//            if (ds.Tables[0].Rows.Count == 0)
//                return null;
//            else
//                return ds.Tables[0];
//        }

//        public static int GetRelationLeagueCountByTeamGuid(Guid teamGuid)
//        {
//            string sql = "SELECT COUNT(*) FROM dbo.Arsenal_RelationLeagueTeam WHERE TeamGuid = @teamGuid";

//            Object obj = SqlHelper.ExecuteScalar(Common.GetConnection(), CommandType.Text, sql, new SqlParameter("@teamGuid", teamGuid));

//            return Convert.ToInt32(obj);
//        }

//        public static int GetRelationLeagueTeamCount(Guid leagueGuid, Guid teamGuid)
//        {
//            string sql = "SELECT COUNT(*) FROM dbo.Arsenal_RelationLeagueTeam WHERE TeamGuid = @teamGuid AND LeagueGuid = @leagueGuid";

//            Object obj = SqlHelper.ExecuteScalar(Common.GetConnection(), CommandType.Text, sql, new SqlParameter("@leagueGuid", leagueGuid), new SqlParameter("@teamGuid", teamGuid));

//            return Convert.ToInt32(obj);
//        }
    }
}
