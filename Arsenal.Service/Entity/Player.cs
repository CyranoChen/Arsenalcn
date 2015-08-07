using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

using Arsenalcn.Core;

namespace Arsenal.Service
{
    [DbSchema("Arsenal_Player", Key = "PlayerGuid", Sort = "IsLegend, IsLoan, SquadNumber, LastName")]
    public class Player : Entity<Guid>
    {
        public Player() : base() { }

        public static void CreateMap()
        {
            var map = AutoMapper.Mapper.CreateMap<IDataReader, Player>();

            map.ForMember(d => d.ID, opt => opt.MapFrom(s => (Guid)s.GetValue("PlayerGuid")));
            map.ForMember(d => d.Position, opt => opt.MapFrom(s => (int)s.GetValue("PlayerPosition")));
        }

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

                ColList_SquadNumber = repo.All<Player>().DistinctOrderBy(x => x.SquadNumber);

                ColList_Position = repo.All<Player>().DistinctOrderBy(x => x.Position.ToString());
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

        [DbColumn("FirstName")]
        public string FirstName
        { get; set; }

        [DbColumn("LastName")]
        public string LastName
        { get; set; }

        [DbColumn("DisplayName")]
        public string DisplayName
        { get; set; }

        [DbColumn("PrintingName")]
        public string PrintingName
        { get; set; }

        [DbColumn("PlayerPostion")]
        public PlayerPostionType Position
        { get; set; }

        [DbColumn("SquadNumber")]
        public int SquadNumber
        { get; set; }

        [DbColumn("FaceURL")]
        public string FaceURL
        { get; set; }

        [DbColumn("PhotoURL")]
        public string PhotoURL
        { get; set; }

        [DbColumn("Offset")]
        public int Offset
        { get; set; }

        [DbColumn("IsLegend")]
        public bool IsLegend
        { get; set; }

        [DbColumn("IsLoan")]
        public bool IsLoan
        { get; set; }

        [DbColumn("Birthday")]
        public DateTime? Birthday
        { get; set; }

        [DbColumn("Born")]
        public string Born
        { get; set; }

        [DbColumn("Starts")]
        public int Starts
        { get; set; }

        [DbColumn("Subs")]
        public int Subs
        { get; set; }

        [DbColumn("Apps")]
        public int Apps
        { get; set; }

        [DbColumn("Goals")]
        public int Goals
        { get; set; }

        [DbColumn("JoinDate")]
        public DateTime? JoinDate
        { get; set; }

        [DbColumn("Joined")]
        public string Joined
        { get; set; }

        [DbColumn("LeftYear")]
        public string LeftYear
        { get; set; }

        [DbColumn("Debut")]
        public string Debut
        { get; set; }

        [DbColumn("FirstGoal")]
        public string FirstGoal
        { get; set; }

        [DbColumn("PreviousClubs")]
        public string PreviousClubs
        { get; set; }

        [DbColumn("Profile")]
        public string Profile
        { get; set; }

        #endregion
    }

    public enum PlayerPostionType
    {
        None = 0,
        Goalkeeper = 1,
        Defender = 2,
        Midfielder = 3,
        Forward = 4,
        Coach = 9
    }
}
