using System;
using System.Collections.Generic;
using System.Data;

using Arsenalcn.Core;

namespace Arsenal.Service
{
    [AttrDbTable("Arsenal_Player", Key = "PlayerGuid")]
    public class Player : Entity
    {
        public Player() { }

        public Player(DataRow dr)
        {
            InitPlayer(dr);
        }

        private void InitPlayer(DataRow dr)
        {
            if (dr != null)
            {
                PlayerGuid = (Guid)dr["PlayerGuid"];

                if (!Convert.IsDBNull(dr["FirstName"]))
                    FirstName = dr["FirstName"].ToString();
                else
                    FirstName = null;

                if (!Convert.IsDBNull(dr["LastName"]))
                    LastName = dr["LastName"].ToString();
                else
                    LastName = null;

                DisplayName = dr["DisplayName"].ToString();

                if (!Convert.IsDBNull(dr["PrintingName"]))
                    PrintingName = dr["PrintingName"].ToString();
                else
                    PrintingName = null;

                if (!string.IsNullOrEmpty(dr["Position"].ToString()))
                    Position = (PlayerPostionType)Enum.Parse(typeof(PlayerPostionType), dr["Position"].ToString());
                else
                    Position = null;

                SquadNumber = Convert.ToInt16(dr["SquadNumber"]);
                FaceURL = dr["FaceURL"].ToString();
                PhotoURL = dr["PhotoURL"].ToString();
                Offset = Convert.ToInt16(dr["Offset"]);
                IsLegend = Convert.ToBoolean(dr["IsLegend"]);
                IsLoan = Convert.ToBoolean(dr["IsLoan"]);

                if (!Convert.IsDBNull(dr["Birthday"]))
                    Birthday = Convert.ToDateTime(dr["Birthday"]);
                else
                    Birthday = null;

                Born = dr["Born"].ToString();
                Starts = Convert.ToInt16(dr["Starts"]);
                Subs = Convert.ToInt16(dr["Subs"]);
                Apps = Convert.ToInt16(dr["Apps"]);
                Goals = Convert.ToInt16(dr["Goals"]);

                if (!Convert.IsDBNull(dr["JoinDate"]))
                    JoinDate = Convert.ToDateTime(dr["JoinDate"]);
                else
                    JoinDate = null;

                Joined = dr["Joined"].ToString();
                LeftYear = dr["LeftYear"].ToString();
                Debut = dr["Debut"].ToString();
                FirstGoal = dr["FirstGoal"].ToString();
                PreviousClubs = dr["PreviousClubs"].ToString();
                Profile = dr["Profile"].ToString();
            }
            else
                throw new Exception("Unable to init Player.");
        }

        //public void Select()
        //{
        //    DataRow dr = DataAccess.Player.GetPlayerByID(PlayerGuid);

        //    if (dr != null)
        //        InitPlayer(dr);
        //}

        //public override void Update<Player>(Player instance)
        //{
        //    string _position = string.Empty;
        //    if (Position != PlayerPostionType.Null)
        //        _position = Position.ToString();

        //}

        //public void Insert()
        //{
        //    string _position = string.Empty;
        //    if (Position != PlayerPostionType.Null)
        //        _position = Position.ToString();

        //    DataAccess.Player.InsertPlayer(PlayerGuid, FirstName, LastName, DisplayName, PrintingName, _position, SquadNumber, FaceURL, PhotoURL, Offset, IsLegend, IsLoan, Birthday, Born, Starts, Subs, Apps, Goals, JoinDate, Joined, Left, Debut, FirstGoal, PreviousClubs, Profile);
        //}

        //public void Delete()
        //{
        //    DataAccess.Player.DeletePlayer(PlayerGuid);
        //}

        //public static List<Player> GetPlayers()
        //{
        //    DataTable dt = DataAccess.Player.GetPlayers();
        //    List<Player> list = new List<Player>();

        //    if (dt != null)
        //    {
        //        foreach (DataRow dr in dt.Rows)
        //        {
        //            list.Add(new Player(dr));
        //        }
        //    }

        //    return list;
        //}

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
                PlayerList = new Player().All<Player>();

                PlayerList_HasSquadNumber = PlayerList.FindAll(delegate(Player p) { return p.SquadNumber > 0; });
                PlayerList_HasSquadNumber.Sort(delegate(Player p1, Player p2)
                {
                    if (p1.SquadNumber == p2.SquadNumber)
                        return Comparer<string>.Default.Compare(p1.DisplayName, p2.DisplayName);
                    else
                        return p1.SquadNumber - p2.SquadNumber;
                });

                //ColList_SquadNumber = DataAccess.Player.GetPlayerDistColumn("SquadNumber", true);
                //ColList_Position = DataAccess.Player.GetPlayerDistColumn("Position", false);
            }

            public static Player Load(Guid guid)
            {
                return PlayerList.Find(delegate(Player p) { return p.PlayerGuid.Equals(guid); });
            }

            // for Acn Club Hard Code
            public static DataRow GetInfo(Guid guid)
            {
                return new Repository().Select<Player>(guid);
            }

            public static List<Player> PlayerList;
            public static List<Player> PlayerList_HasSquadNumber;

            public static DataTable ColList_SquadNumber;
            public static DataTable ColList_Position;
        }

        #region Members and Properties

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
