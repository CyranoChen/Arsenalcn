using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

using Arsenalcn.Core;
using System.Linq.Expressions;

namespace Arsenal.Service
{
    [AttrDbTable("Arsenal_Player", Key = "PlayerGuid")]
    public class Player : Entity
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
                PlayerList = new Player().All<Player>().ToList();

                PlayerList_HasSquadNumber = PlayerList.FindAll(p => p.SquadNumber > 0)
                    .OrderBy(p => p.SquadNumber).ThenBy(p => p.DisplayName).ToList();

                //ColList_SquadNumber = DataAccess.Player.GetPlayerDistColumn("SquadNumber", true);
                //ColList_Position = DataAccess.Player.GetPlayerDistColumn("Position", false);
            }

            public static Player Load(Guid guid)
            {
                return PlayerList.Find(p => p.PlayerGuid.Equals(guid));
            }

            // for Acn Club Hard Code
            public static DataRow GetInfo(Guid guid)
            {
                IRepository repo = new Repository();
                return repo.Select<Player>(guid);
            }

            public static List<Player> PlayerList;
            public static List<Player> PlayerList_HasSquadNumber;

            public static DataTable ColList_SquadNumber;
            public static DataTable ColList_Position;
        }

        #region Members and Properties

        public readonly Expression<Func<Player, object>> orderPredicate = p => p.DisplayName;

        [AttrDbColumn("PlayerGuid", IsKey = true)]
        public Guid PlayerGuid
        { get; set; }

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
