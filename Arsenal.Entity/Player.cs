using System;
using System.Collections.Generic;
using System.Data;

namespace Arsenal.Entity
{
    public class Player
    {
        public Player() { }

        private Player(DataRow dr)
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

                if (!string.IsNullOrEmpty(dr["Position"].ToString()))
                    Position = (PlayerPostionType)Enum.Parse(typeof(PlayerPostionType), dr["Position"].ToString());
                else
                    Position = PlayerPostionType.Null;

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
                Left = dr["Left"].ToString();
                Debut = dr["Debut"].ToString();
                FirstGoal = dr["FirstGoal"].ToString();
                PreviousClubs = dr["PreviousClubs"].ToString();
                Profile = dr["Profile"].ToString();
            }
            else
                throw new Exception("Unable to init Player.");
        }

        public void Select()
        {
            DataRow dr = DataAccess.Player.GetPlayerByID(PlayerGuid);

            if (dr != null)
                InitPlayer(dr);
        }

        public void Update()
        {
            string _position = string.Empty;
            if (Position != PlayerPostionType.Null)
                _position = Position.ToString();

            DataAccess.Player.UpdatePlayer(PlayerGuid, FirstName, LastName, DisplayName, _position, SquadNumber, FaceURL, PhotoURL, Offset, IsLegend, IsLoan, Birthday, Born, Starts, Subs, Apps, Goals, JoinDate, Joined, Left, Debut, FirstGoal, PreviousClubs, Profile);
        }

        public void Insert()
        {
            string _position = string.Empty;
            if (Position != PlayerPostionType.Null)
                _position = Position.ToString();

            DataAccess.Player.InsertPlayer(PlayerGuid, FirstName, LastName, DisplayName, _position, SquadNumber, FaceURL, PhotoURL, Offset, IsLegend, IsLoan, Birthday, Born, Starts, Subs, Apps, Goals, JoinDate, Joined, Left, Debut, FirstGoal, PreviousClubs, Profile);
        }

        public void Delete()
        {
            DataAccess.Player.DeletePlayer(PlayerGuid);
        }

        public static List<Player> GetPlayers()
        {
            DataTable dt = DataAccess.Player.GetPlayers();
            List<Player> list = new List<Player>();

            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    list.Add(new Player(dr));
                }
            }

            return list;
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
                PlayerList = GetPlayers();

                PlayerList_HasSquadNumber = PlayerList.FindAll(delegate(Player p) { return p.SquadNumber > 0; });
                PlayerList_HasSquadNumber.Sort(delegate(Player p1, Player p2)
                {
                    if (p1.SquadNumber == p2.SquadNumber)
                        return Comparer<string>.Default.Compare(p1.DisplayName, p2.DisplayName);
                    else
                        return p1.SquadNumber - p2.SquadNumber;
                });

                ColList_SquadNumber = DataAccess.Player.GetPlayerDistColumn("SquadNumber", true);
                ColList_Position = DataAccess.Player.GetPlayerDistColumn("Position", false);
            }

            public static Player Load(Guid guid)
            {
                return PlayerList.Find(delegate(Player p) { return p.PlayerGuid.Equals(guid); });
            }

            // for Acn Club Hard Code
            public static DataRow GetInfo(Guid guid)
            {
                return DataAccess.Player.GetPlayerByID(guid);
            }

            public static List<Player> PlayerList;
            public static List<Player> PlayerList_HasSquadNumber;

            public static DataTable ColList_SquadNumber;
            public static DataTable ColList_Position;
        }

        #region Members and Properties

        public Guid PlayerGuid
        { get; set; }

        public string FirstName
        { get; set; }

        public string LastName
        { get; set; }

        public string DisplayName
        { get; set; }

        public PlayerPostionType Position
        { get; set; }

        public int SquadNumber
        { get; set; }

        public string FaceURL
        { get; set; }

        public string PhotoURL
        { get; set; }

        public int Offset
        { get; set; }

        public bool IsLegend
        { get; set; }

        public bool IsLoan
        { get; set; }

        public DateTime? Birthday
        { get; set; }

        public string Born
        { get; set; }

        public int Starts
        { get; set; }

        public int Subs
        { get; set; }

        public int Apps
        { get; set; }

        public int Goals
        { get; set; }

        public DateTime? JoinDate
        { get; set; }

        public string Joined
        { get; set; }

        public string Left
        { get; set; }

        public string Debut
        { get; set; }

        public string FirstGoal
        { get; set; }

        public string PreviousClubs
        { get; set; }

        public string Profile
        { get; set; }

        #endregion
    }

    public enum PlayerPostionType
    {
        Null,
        Goalkeeper,
        Defender,
        Midfielder,
        Forward,
        Coach
    }
}
