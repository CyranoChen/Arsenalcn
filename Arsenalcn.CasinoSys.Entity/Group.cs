using System;
using System.Data;

namespace Arsenalcn.CasinoSys.Entity
{
    public class Group
    {
        //private Group(DataRow dr)
        //{
        //    InitGroup(dr);
        //}

        public Group(Guid groupGuid)
        {
            var dr = DataAccess.Group.GetGroupById(groupGuid);

            if (dr != null)
                InitGroup(dr);
        }

        #region Members and Properties

        public Guid GroupGuid { get; set; }

        public string GroupName { get; set; }

        public int GroupOrder { get; set; }

        public Guid LeagueGuid { get; set; }

        public bool IsTable { get; set; }

        public RankMethodType RankMethod { get; set; }

        #endregion

        private void InitGroup(DataRow dr)
        {
            if (dr != null)
            {
                GroupGuid = (Guid)dr["GroupGuid"];
                GroupName = Convert.ToString(dr["GroupName"]);
                GroupOrder = Convert.ToInt32(dr["GroupOrder"]);
                LeagueGuid = (Guid)dr["LeagueGuid"];
                IsTable = Convert.ToBoolean(dr["IsTable"]);
                RankMethod = (RankMethodType)Enum.Parse(typeof(RankMethodType), dr["RankMethod"].ToString());
            }
            else
                throw new Exception("Unable to init Group.");
        }

        //public void Insert()
        //{
        //    DataAccess.Group.InsertGroup(GroupGuid, GroupName, GroupOrder, LeagueGuid, IsTable, (int)RankMethod);
        //}

        //public void Update()
        //{
        //    DataAccess.Group.UpdateGroup(GroupGuid, GroupName, GroupOrder, LeagueGuid, IsTable, (int)RankMethod);
        //}

        //public static List<Group> GetGroups()
        //{
        //    var dt = DataAccess.Group.GetGroups();
        //    var list = new List<Group>();

        //    if (dt != null)
        //    {
        //        foreach (DataRow dr in dt.Rows)
        //        {
        //            list.Add(new Group(dr));
        //        }
        //    }

        //    return list;
        //}

        //public static void RemoveGroup(Guid groupGuid)
        //{
        //    DataAccess.Group.RemoveRelationGroupAllTeam(groupGuid);
        //    DataAccess.Match.RemoveMatchGroupGuid(groupGuid);
        //    DataAccess.Group.DeleteGroup(groupGuid);
        //}

        //public static void RemoveRelationGroupAllTeam(Guid groupGuid)
        //{
        //    DataAccess.Group.RemoveRelationGroupAllTeam(groupGuid);
        //}

        public static DataTable GetGroupByLeague(Guid leagueGuid)
        {
            return DataAccess.Group.GetLeagueGroup(leagueGuid);
        }

        public static DataTable GetGroupByLeague(Guid leagueGuid, bool isTable)
        {
            return DataAccess.Group.GetLeagueGroup(leagueGuid, isTable);
        }

        //public static DataTable GetRelationGroupTeam(Guid groupGuid)
        //{
        //    return DataAccess.Group.GetRelationGroupTeamByGroupGuid(groupGuid);
        //}

        public static DataTable GetTableGroupTeam(Guid groupGuid)
        {
            return DataAccess.Group.GetTableGroupTeamByGroupGuid(groupGuid);
        }

        public static bool IsExistGroupByLeague(Guid leagueGuid, bool isTable)
        {
            var dtGroup = DataAccess.Group.GetLeagueGroup(leagueGuid, isTable);

            return dtGroup != null;
        }

        //public static int GetResultMatchCount(Guid groupGuid)
        //{
        //    var group = new Group(groupGuid);
        //    var dtGroupMatch = DataAccess.Match.GetResultMatchByGroupGuid(group.GroupGuid, group.IsTable);

        //    if (dtGroupMatch != null)
        //        return dtGroupMatch.Rows.Count;
        //    return 0;
        //}

        //public static int GetAllMatchCount(Guid groupGuid)
        //{
        //    var group = new Group(groupGuid);
        //    var dtGroupMatch = DataAccess.Match.GetAllMatchByGroupGuid(group.GroupGuid, group.IsTable);

        //    if (dtGroupMatch != null)
        //        return dtGroupMatch.Rows.Count;
        //    return 0;
        //}

        //public static void SetGroupMatch(Guid groupGuid)
        //{
        //    var group = new Group(groupGuid);

        //    DataAccess.Match.UpdateMatchGroupGuid(group.GroupGuid, group.LeagueGuid);
        //}

        //public static void GroupTableStatistics(Guid groupGuid)
        //{
        //    var group = new Group(groupGuid);
        //    var dtGroupTeam = DataAccess.Group.GetRelationGroupTeamByGroupGuid(groupGuid);
        //    var dtGroupMatch = DataAccess.Match.GetResultMatchByGroupGuid(group.GroupGuid, group.IsTable);

        //    if (dtGroupTeam != null && dtGroupMatch != null)
        //    {
        //        foreach (DataRow dr in dtGroupTeam.Rows)
        //        {
        //            var gt = new GroupTeam(groupGuid, (Guid)dr["TeamGuid"], null);
        //            GroupTeam.UpdateGroupTeamByGroupMatch(gt.GroupGuid, gt.TeamGuid, dtGroupMatch);
        //        }
        //    }

        //    //对球队进行排行
        //    dtGroupTeam = DataAccess.Group.GetRelationGroupTeamByGroupGuid(groupGuid);
        //    short positionNo = 0;

        //    if (dtGroupTeam != null)
        //    {
        //        foreach (DataRow dr in dtGroupTeam.Rows)
        //        {
        //            var gt = new GroupTeam(groupGuid, (Guid)dr["TeamGuid"], null);
        //            gt.PositionNo = ++positionNo;
        //            gt.Update();
        //        }
        //    }
        //}
    }

    public enum RankMethodType
    {
        VersusHist = 0,
        GoalDiff = 1
    }
}