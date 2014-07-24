using System;
using System.Data;
using System.Data.SqlClient;

using Microsoft.ApplicationBlocks.Data;

namespace Arsenalcn.CasinoSys.DataAccess
{
    public class CasinoItem
    {
        public static DataRow GetCasinoItemByID(Guid casinoItemGuid)
        {
            string sql = "SELECT * FROM dbo.AcnCasino_CasinoItem WHERE CasinoItemGuid = @casinoItemGuid";

            DataSet ds = SqlHelper.ExecuteDataset(SQLConn.GetConnection(), CommandType.Text, sql, new SqlParameter("@casinoItemGuid", casinoItemGuid));

            if (ds.Tables[0].Rows.Count == 0)
                return null;
            else
                return ds.Tables[0].Rows[0];
        }

        public static Guid InsertCasinoItem(string itemType, Guid? matchGuid, string itemTitle, string itemBody, DateTime publishTime, DateTime closeTime, Guid bankerID, string bankerName, int ownerID, string ownerName, SqlTransaction trans)
        {
            Guid casinoItemGuid = Guid.NewGuid();

            string sql = "INSERT INTO dbo.AcnCasino_CasinoItem VALUES (@itemGuid, @itemType, @matchGuid, @itemTitle, @itemBody, getdate(), @publishTime, @closeTime, @bankerID, @bankerName, null, @ownerID, @ownerName)";

            SqlParameter[] para = { new SqlParameter("@itemGuid", casinoItemGuid), new SqlParameter("@itemType", itemType), new SqlParameter("@matchGuid", matchGuid), new SqlParameter("@itemTitle", itemTitle), new SqlParameter("@itemBody", itemBody), new SqlParameter("@publishTime", publishTime), new SqlParameter("@closeTime", closeTime), new SqlParameter("@bankerID", bankerID), new SqlParameter("@bankerName", bankerName), new SqlParameter("ownerID", ownerID), new SqlParameter("ownerName", ownerName) };

            if (trans != null)
            {
                SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sql, para);
            }
            else
            {
                SqlHelper.ExecuteNonQuery(SQLConn.GetConnection(), CommandType.Text, sql, para);
            }

            return casinoItemGuid;
        }

        public static void UpdateCasinoItem(Guid casinoItemGuid, float earning, SqlTransaction trans)
        {
            string sql = "UPDATE dbo.AcnCasino_CasinoItem SET Earning = @earning WHERE CasinoItemGuid = @guid";

            if (trans != null)
            {
                SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sql, new SqlParameter("@earning", earning), new SqlParameter("@guid", casinoItemGuid));
            }
            else
            {
                SqlHelper.ExecuteNonQuery(SQLConn.GetConnection(), CommandType.Text, sql, new SqlParameter("@earning", earning), new SqlParameter("@guid", casinoItemGuid));
            }
        }

        public static void UpdateCasinoItemCloseTime(Guid matchGuid, DateTime closeTime)
        {
            string sql = "UPDATE AcnCasino_CasinoItem SET CloseTime = @closeTime WHERE MatchGuid = @guid";

            SqlHelper.ExecuteNonQuery(SQLConn.GetConnection(), CommandType.Text, sql, new SqlParameter("@closeTime", closeTime), new SqlParameter("@guid", matchGuid));
        }

        public static void DeleteCasinoItem(Guid matchGuid)
        {
            string sql = "DELETE FROM AcnCasino_CasinoItem WHERE MatchGuid = @guid";

            SqlHelper.ExecuteNonQuery(SQLConn.GetConnection(), CommandType.Text, sql, new SqlParameter("@guid", matchGuid));
        }

        public static int GetMatchCasinoItemCount()
        {
            string sql = "SELECT DISTINCT MatchGuid FROM dbo.AcnCasino_CasinoItem WHERE MatchGuid IS NOT NULL";

            DataSet ds = SqlHelper.ExecuteDataset(SQLConn.GetConnection(), CommandType.Text, sql);

            return ds.Tables[0].Rows.Count;
        }

        public static int GetOtherCasinoItemCount()
        {
            string sql = "SELECT COUNT(*) FROM dbo.AcnCasino_CasinoItem WHERE MatchGuid IS NULL";

            Object obj = SqlHelper.ExecuteScalar(SQLConn.GetConnection(), CommandType.Text, sql);

            return obj.Equals(DBNull.Value) ? 0 : Convert.ToInt32(obj);
        }

        public static DataTable GetOpenMatchView(int casinoValidDays)
        {
            string sql = @"SELECT match.MatchGuid, match.Home, match.Away, match.ResultHome, match.ResultAway, match.PlayTime, match.LeagueGuid, match.LeagueName, 
                      match.Round, teamH.TeamDisplayName AS HomeDisplay, teamH.TeamEnglishName AS HomeEng, teamH.TeamLogo AS HomeLogo, 
                      teamA.TeamDisplayName AS AwayDisplay, teamA.TeamEnglishName AS AwayEng, teamA.TeamLogo AS AwayLogo, teamH.Ground as Ground, teamH.Capacity as Capacity, 
                      league.LeagueName, league.LeagueSeason, league.LeagueGuid, league.LeagueLogo
                      FROM (SELECT DISTINCT MatchGuid FROM AcnCasino_CasinoItem
                      WHERE (MatchGuid IS NOT NULL) AND (CloseTime > GETDATE()) AND (CloseTime < DATEADD(d,@casinoValidDays,GETDATE())) AND (Earning IS NULL)) AS item INNER JOIN
                      AcnCasino_Match AS match ON match.MatchGuid = item.MatchGuid AND match.ResultHome IS NULL AND match.ResultAway IS NULL INNER JOIN
                      Arsenal_Team AS teamH ON match.Home = teamH.TeamGuid INNER JOIN
                      Arsenal_Team AS teamA ON match.Away = teamA.TeamGuid INNER JOIN
                      Arsenal_League AS league ON match.LeagueGuid = league.LeagueGuid ORDER BY match.PlayTime, HomeEng";

            DataSet ds = SqlHelper.ExecuteDataset(SQLConn.GetConnection(), CommandType.Text, sql, new SqlParameter("@casinoValidDays", casinoValidDays));

            if (ds.Tables[0].Rows.Count == 0)
                return null;
            else
                return ds.Tables[0];
        }

        public static DataTable GetAllMatchView()
        {
            string sql = @"SELECT match.*, teamH.TeamDisplayName as HomeDisplay, teamH.TeamEnglishName as HomeEng, teamH.TeamLogo as HomeLogo, 
                        teamA.TeamDisplayName as AwayDisplay, teamA.TeamEnglishName as AwayEng, teamA.TeamLogo as AwayLogo, league.LeagueName, league.LeagueSeason 
                        FROM (SELECT DISTINCT MatchGuid FROM dbo.AcnCasino_CasinoItem WHERE MatchGuid IS NOT NULL) item
                        INNER JOIN acncasino_match match
                        ON match.MatchGuid = item.MatchGuid
                        INNER JOIN arsenal_team teamH
                        ON match.home = teamH.TeamGuid
                        INNER JOIN arsenal_team teamA
                        ON match.away = teamA.TeamGuid
                        INNER JOIN arsenal_league league
                        ON match.LeagueGuid = league.LeagueGuid
                        ORDER BY match.PlayTime DESC, HomeEng DESC";

            DataSet ds = SqlHelper.ExecuteDataset(SQLConn.GetConnection(), CommandType.Text, sql);

            if (ds.Tables[0].Rows.Count == 0)
                return null;
            else
                return ds.Tables[0];
        }

        public static DataTable GetEndMatchView()
        {
            string sql = @"SELECT match.*, teamH.TeamDisplayName as HomeDisplay, teamH.TeamEnglishName as HomeEng, teamH.TeamLogo as HomeLogo, 
                        teamH.Capacity as Capacity, teamH.Ground as Ground,  teamA.TeamLogo as AwayLogo, teamA.TeamDisplayName as AwayDisplay, teamA.TeamEnglishName as AwayEng 
                        FROM (SELECT DISTINCT MatchGuid FROM dbo.AcnCasino_CasinoItem WHERE MatchGuid IS NOT NULL) item
                        INNER JOIN acncasino_match match
                        ON match.MatchGuid = item.MatchGuid
                        INNER JOIN arsenal_team teamH
                        ON match.home = teamH.TeamGuid
                        INNER JOIN arsenal_team teamA
                        ON match.away = teamA.TeamGuid
                        INNER JOIN arsenal_league league
                        ON match.LeagueGuid = league.LeagueGuid
                        WHERE match.ResultHome IS NOT NULL AND match.ResultAway IS NOT NULL
                        ORDER BY match.PlayTime DESC";

            DataSet ds = SqlHelper.ExecuteDataset(SQLConn.GetConnection(), CommandType.Text, sql);

            if (ds.Tables[0].Rows.Count == 0)
                return null;
            else
                return ds.Tables[0];
        }

        public static DataTable GetEndMatchView(Guid leagueGuid)
        {
            string sql = @"SELECT match.*, teamH.TeamDisplayName as HomeDisplay, teamH.TeamEnglishName as HomeEng, teamH.TeamLogo as HomeLogo, teamA.TeamLogo as AwayLogo,
                        teamH.Ground as Ground, teamH.Capacity as Capacity, teamA.TeamDisplayName as AwayDisplay, teamA.TeamEnglishName as AwayEng from
                        (select distinct MatchGuid from dbo.AcnCasino_CasinoItem where MatchGuid IS NOT NULL) item
                        INNER JOIN acncasino_match match
                        ON match.MatchGuid = item.MatchGuid
                        INNER JOIN arsenal_team teamH
                        ON match.home = teamH.TeamGuid
                        INNER JOIN arsenal_team teamA
                        ON match.away = teamA.TeamGuid
                        INNER JOIN arsenal_league league
                        ON match.LeagueGuid = league.LeagueGuid
                        WHERE match.LeagueGuid = @leagueGuid AND match.ResultHome IS NOT NULL AND match.ResultAway IS NOT NULL
                        ORDER BY match.PlayTime DESC";

            DataSet ds = SqlHelper.ExecuteDataset(SQLConn.GetConnection(), CommandType.Text, sql, new SqlParameter("@leagueGuid", leagueGuid));

            if (ds.Tables[0].Rows.Count == 0)
                return null;
            else
                return ds.Tables[0];
        }

        public static DataTable GetEndMatchView(Guid leagueGuid, Guid groupGuid, bool isTable)
        {
            string sql = string.Empty;
            if (!isTable)
                sql = @"SELECT match.*, teamH.TeamDisplayName as HomeDisplay, teamH.TeamEnglishName as HomeEng, teamH.TeamLogo as HomeLogo, teamA.TeamLogo as AwayLogo,
                        teamH.Ground as Ground, teamH.Capacity as Capacity, teamA.TeamDisplayName as AwayDisplay, teamA.TeamEnglishName as AwayEng from
                        (select distinct MatchGuid from dbo.AcnCasino_CasinoItem where MatchGuid IS NOT NULL) item
                        INNER JOIN acncasino_match match
                        ON match.MatchGuid = item.MatchGuid
                        INNER JOIN arsenal_team teamH
                        ON match.home = teamH.TeamGuid
                        INNER JOIN arsenal_team teamA
                        ON match.away = teamA.TeamGuid
                        INNER JOIN arsenal_league league
                        ON match.LeagueGuid = league.LeagueGuid
                        WHERE match.LeagueGuid = @leagueGuid AND match.GroupGuid = @groupGuid AND match.ResultHome IS NOT NULL AND match.ResultAway IS NOT NULL
                        ORDER BY match.PlayTime DESC";
            else
                sql = @"SELECT match.*, teamH.TeamDisplayName as HomeDisplay, teamH.TeamEnglishName as HomeEng, teamH.TeamLogo as HomeLogo, teamA.TeamLogo as AwayLogo,
                        teamH.Ground as Ground, teamH.Capacity as Capacity, teamA.TeamDisplayName as AwayDisplay, teamA.TeamEnglishName as AwayEng from
                        (select distinct MatchGuid from dbo.AcnCasino_CasinoItem where MatchGuid IS NOT NULL) item
                        INNER JOIN acncasino_match match
                        ON match.MatchGuid = item.MatchGuid
                        INNER JOIN arsenal_team teamH
                        ON match.home = teamH.TeamGuid
                        INNER JOIN arsenal_team teamA
                        ON match.away = teamA.TeamGuid
                        INNER JOIN arsenal_league league
                        ON match.LeagueGuid = league.LeagueGuid
                        WHERE match.LeagueGuid = @leagueGuid AND match.ResultHome IS NOT NULL AND match.ResultAway IS NOT NULL AND
                        (match.home IN (SELECT TeamGuid FROM dbo.Arsenal_RelationGroupTeam AS GroupTeam1 WHERE GroupGuid = @groupGuid)) AND
                        (match.away IN (SELECT TeamGuid FROM dbo.Arsenal_RelationGroupTeam AS GroupTeam2 WHERE GroupGuid = @groupGuid))
                        ORDER BY match.PlayTime DESC";

            DataSet ds = SqlHelper.ExecuteDataset(SQLConn.GetConnection(), CommandType.Text, sql, new SqlParameter("@leagueGuid", leagueGuid), new SqlParameter("@groupGuid", groupGuid));

            if (ds.Tables[0].Rows.Count == 0)
                return null;
            else
                return ds.Tables[0];
        }

        public static DataTable GetEndMatchViewByTeamGuid(Guid teamGuid)
        {
            string sql = @"SELECT match.*, teamH.TeamDisplayName as HomeDisplay, teamH.TeamEnglishName as HomeEng, teamH.TeamLogo as HomeLogo, teamA.TeamLogo as AwayLogo,
                        teamH.Ground as Ground, teamH.Capacity as Capacity, teamA.TeamDisplayName as AwayDisplay, teamA.TeamEnglishName as AwayEng,
                        league.LeagueLogo AS LeagueLogo, league.LeagueName AS LeagueName, league.LeagueSeason AS LeagueSeason FROM
                        (select distinct MatchGuid from dbo.AcnCasino_CasinoItem where MatchGuid IS NOT NULL) item
                        INNER JOIN acncasino_match match
                        ON match.MatchGuid = item.MatchGuid
                        INNER JOIN arsenal_team teamH
                        ON match.home = teamH.TeamGuid
                        INNER JOIN arsenal_team teamA
                        ON match.away = teamA.TeamGuid
                        INNER JOIN arsenal_league league
                        ON match.LeagueGuid = league.LeagueGuid
                        WHERE (match.Home = @teamGuid OR match.Away = @teamGuid) AND 
                        match.ResultHome IS NOT NULL AND match.ResultAway IS NOT NULL
                        ORDER BY match.PlayTime DESC";

            DataSet ds = SqlHelper.ExecuteDataset(SQLConn.GetConnection(), CommandType.Text, sql, new SqlParameter("@teamGuid", teamGuid));

            if (ds.Tables[0].Rows.Count == 0)
                return null;
            else
                return ds.Tables[0];
        }

        public static DataTable GetEndMatchViewByTeams(Guid teamGuidA, Guid teamGuidB)
        {
            string sql = @"SELECT match.*, teamH.TeamDisplayName as HomeDisplay, teamH.TeamEnglishName as HomeEng, teamH.TeamLogo as HomeLogo, teamA.TeamLogo as AwayLogo,
                        teamH.Ground as Ground, teamH.Capacity as Capacity, teamA.TeamDisplayName as AwayDisplay, teamA.TeamEnglishName as AwayEng,
                        league.LeagueLogo AS LeagueLogo, league.LeagueName AS LeagueName, league.LeagueSeason AS LeagueSeason FROM
                        (select distinct MatchGuid from dbo.AcnCasino_CasinoItem where MatchGuid IS NOT NULL) item
                        INNER JOIN acncasino_match match
                        ON match.MatchGuid = item.MatchGuid
                        INNER JOIN Arsenal_Team teamH
                        ON match.home = teamH.TeamGuid
                        INNER JOIN Arsenal_Team teamA
                        ON match.away = teamA.TeamGuid
                        INNER JOIN Arsenal_League league
                        ON match.LeagueGuid = league.LeagueGuid
                        WHERE ((match.Home = @teamGuidA AND match.Away = @teamGuidB) OR 
                        (match.Home = @teamGuidB AND match.Away = @teamGuidA)) AND 
                        match.ResultHome IS NOT NULL AND match.ResultAway IS NOT NULL
                        ORDER BY match.PlayTime DESC";

            DataSet ds = SqlHelper.ExecuteDataset(SQLConn.GetConnection(), CommandType.Text, sql, new SqlParameter("@teamGuidA", teamGuidA), new SqlParameter("@teamGuidB", teamGuidB));

            if (ds.Tables[0].Rows.Count == 0)
                return null;
            else
                return ds.Tables[0];
        }

        public static DataTable GetTopMatchEarning(out int months)
        {
            DateTime iDay = DateTime.Today;
            months = 0;

            do
            {
                DateTime monthStart = iDay.AddDays(1 - iDay.Day);
                DateTime nextStart = iDay.AddMonths(1);

                string sql = string.Format(@"SELECT TOP 5 dbo.AcnCasino_CasinoItem.MatchGuid, TeamH.TeamDisplayName AS HomeDisplay, TeamA.TeamDisplayName AS AwayDisplay, 
                      TeamH.TeamEnglishName AS HomeEng, TeamA.TeamEnglishName AS AwayEng, dbo.AcnCasino_Match.PlayTime, 
                      dbo.AcnCasino_CasinoItem.Earning, dbo.AcnCasino_Match.Round, dbo.Arsenal_League.LeagueName
                      FROM dbo.AcnCasino_CasinoItem INNER JOIN
                      dbo.AcnCasino_Match ON dbo.AcnCasino_CasinoItem.MatchGuid = dbo.AcnCasino_Match.MatchGuid INNER JOIN
                      dbo.Arsenal_League ON dbo.AcnCasino_Match.LeagueGuid = dbo.Arsenal_League.LeagueGuid INNER JOIN
                      dbo.Arsenal_Team AS TeamH ON dbo.AcnCasino_Match.Home = TeamH.TeamGuid INNER JOIN
                      dbo.Arsenal_Team AS TeamA ON dbo.AcnCasino_Match.Away = TeamA.TeamGuid
                      WHERE (dbo.AcnCasino_CasinoItem.Earning IS NOT NULL) AND (dbo.AcnCasino_CasinoItem.ItemType = 'SingleChoice') AND (dbo.AcnCasino_Match.PlayTime >= '{0}') AND (dbo.AcnCasino_Match.PlayTime < '{1}')
                      ORDER BY dbo.AcnCasino_CasinoItem.Earning DESC", monthStart, nextStart);

                DataSet ds = SqlHelper.ExecuteDataset(SQLConn.GetConnection(), CommandType.Text, sql);

                if (ds.Tables[0].Rows.Count > 0 || months < -12)
                    return ds.Tables[0];
                else
                {
                    months--;
                    iDay = monthStart.AddMonths(-1);
                }
            }
            while (true);
        }

        public static DataTable GetTopMatchLoss(out int months)
        {
            DateTime iDay = DateTime.Today;
            months = 0;

            do
            {
                DateTime monthStart = iDay.AddDays(1 - iDay.Day);
                DateTime nextStart = iDay.AddMonths(1);

                string sql = string.Format(@"SELECT TOP 5 dbo.AcnCasino_CasinoItem.MatchGuid, TeamH.TeamDisplayName AS HomeDisplay, TeamA.TeamDisplayName AS AwayDisplay, 
                      TeamH.TeamEnglishName AS HomeEng, TeamA.TeamEnglishName AS AwayEng, dbo.AcnCasino_Match.PlayTime, 
                      dbo.AcnCasino_CasinoItem.Earning, dbo.AcnCasino_Match.Round, dbo.Arsenal_League.LeagueName
                      FROM dbo.AcnCasino_CasinoItem INNER JOIN
                      dbo.AcnCasino_Match ON dbo.AcnCasino_CasinoItem.MatchGuid = dbo.AcnCasino_Match.MatchGuid INNER JOIN
                      dbo.Arsenal_League ON dbo.AcnCasino_Match.LeagueGuid = dbo.Arsenal_League.LeagueGuid INNER JOIN
                      dbo.Arsenal_Team AS TeamH ON dbo.AcnCasino_Match.Home = TeamH.TeamGuid INNER JOIN
                      dbo.Arsenal_Team AS TeamA ON dbo.AcnCasino_Match.Away = TeamA.TeamGuid
                      WHERE (dbo.AcnCasino_CasinoItem.Earning IS NOT NULL) AND (dbo.AcnCasino_CasinoItem.ItemType = 'SingleChoice') AND (dbo.AcnCasino_Match.PlayTime >= '{0}') AND (dbo.AcnCasino_Match.PlayTime < '{1}')
                      ORDER BY dbo.AcnCasino_CasinoItem.Earning", monthStart, nextStart);

                DataSet ds = SqlHelper.ExecuteDataset(SQLConn.GetConnection(), CommandType.Text, sql);

                if (ds.Tables[0].Rows.Count > 0 || months < -12)
                    return ds.Tables[0];
                else
                {
                    months--;
                    iDay = monthStart.AddMonths(-1);
                }
            }
            while (true);
        }

        public static Guid? GetCasinoItemGuidByMatch(Guid matchGuid, string type, SqlTransaction trans)
        {
            string sql = "SELECT CasinoItemGuid FROM AcnCasino_CasinoItem WHERE MatchGuid = @guid AND ItemType = @type";

            Object obj;
            if (trans == null)
                obj = SqlHelper.ExecuteScalar(SQLConn.GetConnection(), CommandType.Text, sql, new SqlParameter("@guid", matchGuid), new SqlParameter("@type", type.ToString()));
            else
                obj = SqlHelper.ExecuteScalar(trans, CommandType.Text, sql, new SqlParameter("@guid", matchGuid), new SqlParameter("@type", type.ToString()));

            if (Convert.IsDBNull(obj) || obj == null)
                return null;

            return (Guid)obj;
        }

        public static DataTable GetActiveCasinoItem()
        {
            string sql = @"SELECT CasinoItemGuid FROM dbo.AcnCasino_CasinoItem INNER JOIN
                          dbo.AcnCasino_Match ON dbo.AcnCasino_CasinoItem.MatchGuid = dbo.AcnCasino_Match.MatchGuid INNER JOIN
                          dbo.Arsenal_League ON dbo.AcnCasino_Match.LeagueGuid = dbo.Arsenal_League.LeagueGuid
                          WHERE (dbo.AcnCasino_Match.ResultHome IS NOT NULL) AND (dbo.AcnCasino_Match.ResultAway IS NOT NULL) AND (dbo.Arsenal_League.IsActive = 1) AND 
                          (AcnCasino_CasinoItem.ItemType = 'SingleChoice') AND (dbo.AcnCasino_CasinoItem.Earning IS NOT NULL)";

            DataSet ds = SqlHelper.ExecuteDataset(SQLConn.GetConnection(), CommandType.Text, sql);

            if (ds.Tables[0].Rows.Count == 0)
                return null;
            else
                return ds.Tables[0];
        }
    }
}
