using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;

using Arsenalcn.Core;

namespace Arsenal.Service
{
    [AttrDbTable("Arsenal_Player", Key = "PlayerGuid", Sort = "IsLegend, IsLoan, SquadNumber, LastName")]
    public class Player : Entity<Guid>
    {
        public Player() : base() { }

        public Player(DataRow dr) : base(dr) { }

        public static class Cache
        {
            static Cache()
            {
                InitCache();
            }

            public static void RefreshCache()
            {
                InitCache();
            }

            private static void InitCache()
            {
                IRepository repo = new Repository();

                PlayerList = repo.All<Player>();

                PlayerList_HasSquadNumber = PlayerList.FindAll(x => x.SquadNumber > 0)
                    .OrderBy(x => x.SquadNumber).ThenBy(x => x.DisplayName).ToList();

                //ColList_SquadNumber = repo.All<Player>().GroupBy(x => x.SquadNumber)
                //    .Select(g => g.First()).OrderBy(x => x.SquadNumber)
                //    .Select(x => x.SquadNumber).AsEnumerable();

                ColList_SquadNumber = Repository.DistinctOrderBy(repo.All<Player>(), x => x.SquadNumber);

                ColList_Position = Repository.DistinctOrderBy(repo.Query<Player>(x => x.Position.HasValue), x => x.Position.Value.ToString());
            }

            public static Player Load(Guid guid)
            {
                return PlayerList.Find(x => x.ID.Equals(guid));
            }

            // HC: Acn Club API
            public static DataRow GetInfo(Guid guid)
            {
                IRepository repo = new Repository();

                var attr = Repository.GetTableAttr<Player>();

                string sql = string.Format("SELECT * FROM {0} WHERE {1} = @key", attr.Name, attr.Key);

                System.Data.SqlClient.SqlParameter[] para = { new System.Data.SqlClient.SqlParameter("@key", guid) };

                DataSet ds = DataAccess.ExecuteDataset(sql, para);

                if (ds.Tables[0].Rows.Count == 0)
                { return null; }
                else
                { return ds.Tables[0].Rows[0]; }
            }

            public static List<Player> PlayerList;
            public static List<Player> PlayerList_HasSquadNumber;

            public static IEnumerable<int> ColList_SquadNumber;
            public static IEnumerable<string> ColList_Position;
        }

        #region Members and Properties

        [AttrDbColumn("FirstName")]
        public string FirstName
        { get; set; }

        [AttrDbColumn("LastName")]
        public string LastName
        { get; set; }

        [AttrDbColumn("DisplayName")]
        public string DisplayName
        { get; set; }

        [AttrDbColumn("PrintingName")]
        public string PrintingName
        { get; set; }

        [AttrDbColumn("Position")]
        public PlayerPostionType? Position
        { get; set; }

        [AttrDbColumn("SquadNumber")]
        public int SquadNumber
        { get; set; }

        [AttrDbColumn("FaceURL")]
        public string FaceURL
        { get; set; }

        [AttrDbColumn("PhotoURL")]
        public string PhotoURL
        { get; set; }

        [AttrDbColumn("Offset")]
        public int Offset
        { get; set; }

        [AttrDbColumn("IsLegend")]
        public bool IsLegend
        { get; set; }

        [AttrDbColumn("IsLoan")]
        public bool IsLoan
        { get; set; }

        [AttrDbColumn("Birthday")]
        public DateTime? Birthday
        { get; set; }

        [AttrDbColumn("Born")]
        public string Born
        { get; set; }

        [AttrDbColumn("Starts")]
        public int Starts
        { get; set; }

        [AttrDbColumn("Subs")]
        public int Subs
        { get; set; }

        [AttrDbColumn("Apps")]
        public int Apps
        { get; set; }

        [AttrDbColumn("Goals")]
        public int Goals
        { get; set; }

        [AttrDbColumn("JoinDate")]
        public DateTime? JoinDate
        { get; set; }

        [AttrDbColumn("Joined")]
        public string Joined
        { get; set; }

        [AttrDbColumn("LeftYear")]
        public string LeftYear
        { get; set; }

        [AttrDbColumn("Debut")]
        public string Debut
        { get; set; }

        [AttrDbColumn("FirstGoal")]
        public string FirstGoal
        { get; set; }

        [AttrDbColumn("PreviousClubs")]
        public string PreviousClubs
        { get; set; }

        [AttrDbColumn("Profile")]
        public string Profile
        { get; set; }

        #endregion
    }

    public enum PlayerPostionType
    {
        Goalkeeper,
        Defender,
        Midfielder,
        Forward,
        Coach
    }
}
