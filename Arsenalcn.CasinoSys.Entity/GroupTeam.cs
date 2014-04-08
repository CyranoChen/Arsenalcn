using System;
using System.Data;
using System.Data.SqlClient;

namespace Arsenalcn.CasinoSys.Entity
{
    public class GroupTeam
    {
        public GroupTeam()
        {
            GroupGuid = Guid.Empty;
            TeamGuid = Guid.Empty;

            PositionNo = 0;
            TotalPlayed = 0;
            TotalPoints = 0;

            HomeWon = 0;
            HomeDraw = 0;
            HomeLost = 0;
            HomeGoalFor = 0;
            HomeGoalAgainst = 0;
            HomeGoalDiff = 0;
            HomePoints = 0;

            AwayWon = 0;
            AwayDraw = 0;
            AwayLost = 0;
            AwayGoalFor = 0;
            AwayGoalAgainst = 0;
            AwayGoalDiff = 0;
            AwayPoints = 0;
        }

        public GroupTeam(Guid groupGuid, Guid teamGuid, SqlTransaction trans)
        {
            DataRow dr = DataAccess.Group.GetGroupTeamInfo(groupGuid, teamGuid, trans);

            if (dr == null)
            {
                DataAccess.Group.InsertRelationGroupTeam(groupGuid, teamGuid);
                dr = DataAccess.Group.GetGroupTeamInfo(groupGuid, teamGuid, null);
            }

            InitGroupTeam(dr);
        }

        private void InitGroupTeam(DataRow dr)
        {
            if (dr != null)
            {
                GroupGuid = (Guid)dr["GroupGuid"];
                TeamGuid = (Guid)dr["TeamGuid"];

                if (Convert.IsDBNull(dr["PositionNo"]))
                    PositionNo = null;
                else
                    PositionNo = Convert.ToInt16(dr["PositionNo"]);

                if (Convert.IsDBNull(dr["TotalPlayed"]))
                    TotalPlayed = null;
                else
                    TotalPlayed = Convert.ToInt16(dr["TotalPlayed"]);

                if (Convert.IsDBNull(dr["TotalPoints"]))
                    TotalPoints = null;
                else
                    TotalPoints = Convert.ToInt16(dr["TotalPoints"]);

                #region Group Team Home Columns
                if (Convert.IsDBNull(dr["HomeWon"]))
                    HomeWon = null;
                else
                    HomeWon = Convert.ToInt16(dr["HomeWon"]);

                if (Convert.IsDBNull(dr["HomeDraw"]))
                    HomeDraw = null;
                else
                    HomeDraw = Convert.ToInt16(dr["HomeDraw"]);

                if (Convert.IsDBNull(dr["HomeLost"]))
                    HomeLost = null;
                else
                    HomeLost = Convert.ToInt16(dr["HomeLost"]);

                if (Convert.IsDBNull(dr["HomeGoalFor"]))
                    HomeGoalFor = null;
                else
                    HomeGoalFor = Convert.ToInt16(dr["HomeGoalFor"]);

                if (Convert.IsDBNull(dr["HomeGoalAgainst"]))
                    HomeGoalAgainst = null;
                else
                    HomeGoalAgainst = Convert.ToInt16(dr["HomeGoalAgainst"]);

                if (Convert.IsDBNull(dr["HomeGoalDiff"]))
                    HomeGoalDiff = null;
                else
                    HomeGoalDiff = Convert.ToInt16(dr["HomeGoalDiff"]);

                if (Convert.IsDBNull(dr["HomePoints"]))
                    HomePoints = null;
                else
                    HomePoints = Convert.ToInt16(dr["HomePoints"]);
                #endregion

                #region Group Team Away Columns
                if (Convert.IsDBNull(dr["AwayWon"]))
                    AwayWon = null;
                else
                    AwayWon = Convert.ToInt16(dr["AwayWon"]);

                if (Convert.IsDBNull(dr["AwayDraw"]))
                    AwayDraw = null;
                else
                    AwayDraw = Convert.ToInt16(dr["AwayDraw"]);

                if (Convert.IsDBNull(dr["AwayLost"]))
                    AwayLost = null;
                else
                    AwayLost = Convert.ToInt16(dr["AwayLost"]);

                if (Convert.IsDBNull(dr["AwayGoalFor"]))
                    AwayGoalFor = null;
                else
                    AwayGoalFor = Convert.ToInt16(dr["AwayGoalFor"]);

                if (Convert.IsDBNull(dr["AwayGoalAgainst"]))
                    AwayGoalAgainst = null;
                else
                    AwayGoalAgainst = Convert.ToInt16(dr["AwayGoalAgainst"]);

                if (Convert.IsDBNull(dr["AwayGoalDiff"]))
                    AwayGoalDiff = null;
                else
                    AwayGoalDiff = Convert.ToInt16(dr["AwayGoalDiff"]);

                if (Convert.IsDBNull(dr["AwayPoints"]))
                    AwayPoints = null;
                else
                    AwayPoints = Convert.ToInt16(dr["AwayPoints"]);
                #endregion
            }
            else
                throw new Exception("Unable to init GroupTeam.");
        }

        public void Insert()
        {
            DataAccess.Group.InsertRelationGroupTeam(GroupGuid, TeamGuid);
        }

        public static bool IsExistRelationGroupTeam(Guid groupGuid, Guid teamGuid)
        {
            return Convert.ToBoolean(DataAccess.Group.GetRelationGroupTeamCount(groupGuid, teamGuid) > 0);
        }

        public static bool IsExistRelationGroupTeamByLeagueGuid(Guid leagueGuid, Guid teamGuid)
        {
            DataTable dtGroupTeam = DataAccess.Group.GetRelationGroupTeamByLeagueGuid(leagueGuid, teamGuid);

            return (dtGroupTeam != null);
        }

        public static void UpdateGroupTeamByGroupMatch(Guid groupGuid, Guid teamGuid, DataTable dtGroupMatch)
        {
            GroupTeam gt = new GroupTeam();
            gt.GroupGuid = groupGuid;
            gt.TeamGuid = teamGuid;
            gt.PositionNo = 0;

            foreach (DataRow dr in dtGroupMatch.Rows)
            {
                Match match = new Match((Guid)dr["MatchGuid"]);

                if (match != null)
                {
                    if (match.Home == gt.TeamGuid)
                    {
                        gt.TotalPlayed++;

                        if (match.ResultHome > match.ResultAway)
                        {
                            gt.HomeWon++;
                            gt.HomePoints += 3;
                        }
                        else if (match.ResultHome == match.ResultAway)
                        {
                            gt.HomeDraw++;
                            gt.HomePoints += 1;
                        }
                        else
                            gt.HomeLost++;

                        gt.HomeGoalFor += match.ResultHome;
                        gt.HomeGoalAgainst += match.ResultAway;
                    }
                    else if (match.Away == gt.TeamGuid)
                    {
                        gt.TotalPlayed++;

                        if (match.ResultAway > match.ResultHome)
                        {
                            gt.AwayWon++;
                            gt.AwayPoints += 3;
                        }
                        else if (match.ResultAway == match.ResultHome)
                        {
                            gt.AwayDraw++;
                            gt.AwayPoints += 1;
                        }
                        else
                            gt.AwayLost++;

                        gt.AwayGoalFor += match.ResultAway;
                        gt.AwayGoalAgainst += match.ResultHome;
                    }
                }
            }

            gt.HomeGoalDiff = Convert.ToInt16(gt.HomeGoalFor - gt.HomeGoalAgainst);
            gt.AwayGoalDiff = Convert.ToInt16(gt.AwayGoalFor - gt.AwayGoalAgainst);
            gt.TotalPoints = Convert.ToInt16(gt.HomePoints + gt.AwayPoints);

            gt.Update();
        }

        public void Update()
        {
            DataAccess.Group.UpdateRelationGroupTeam(this.GroupGuid, TeamGuid, PositionNo, TotalPlayed, HomeWon, HomeDraw, HomeLost, HomeGoalFor, HomeGoalAgainst, HomeGoalDiff, HomePoints, AwayWon, AwayDraw, AwayLost, AwayGoalFor, AwayGoalAgainst, AwayGoalDiff, AwayPoints, TotalPoints);
        }

        public Guid GroupGuid
        { get; set; }

        public Guid TeamGuid
        { get; set; }

        public short? PositionNo
        { get; set; }

        public short? TotalPlayed
        { get; set; }

        public short? TotalPoints
        { get; set; }

        public short? HomeWon
        { get; set; }

        public short? HomeDraw
        { get; set; }

        public short? HomeLost
        { get; set; }

        public short? HomeGoalFor
        { get; set; }

        public short? HomeGoalAgainst
        { get; set; }

        public short? HomeGoalDiff
        { get; set; }

        public short? HomePoints
        { get; set; }

        public short? AwayWon
        { get; set; }

        public short? AwayDraw
        { get; set; }

        public short? AwayLost
        { get; set; }

        public short? AwayGoalFor
        { get; set; }

        public short? AwayGoalAgainst
        { get; set; }

        public short? AwayGoalDiff
        { get; set; }

        public short? AwayPoints
        { get; set; }
    }
}
