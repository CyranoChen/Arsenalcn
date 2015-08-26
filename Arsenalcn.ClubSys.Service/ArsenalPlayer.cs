using System;
using System.Collections.Generic;
using System.Data;

namespace Arsenalcn.ClubSys.Service
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
                ID = (Guid)dr["PlayerGuid"];
                FirstName = dr["FirstName"].ToString();
                LastName = dr["LastName"].ToString();
                DisplayName = dr["DisplayName"].ToString();
                PrintingName = dr["PrintingName"].ToString();
                Position = (PlayerPositionType)Enum.Parse(typeof(PlayerPositionType), dr["PlayerPosition"].ToString());
                SquadNumber = Convert.ToInt32(dr["SquadNumber"]);
                FaceURL = dr["FaceURL"].ToString();
                PhotoURL = dr["PhotoURL"].ToString();
                Offset = Convert.ToInt32(dr["Offset"]);
                IsLegend = Convert.ToBoolean(dr["IsLegend"]);
                IsLoan = Convert.ToBoolean(dr["IsLoan"]);

                if (Convert.IsDBNull(dr["Birthday"]))
                    Birthday = null;
                else
                    Birthday = Convert.ToDateTime(dr["Birthday"]);

                Born = dr["Born"].ToString();
                Starts = Convert.ToInt32(dr["Starts"]);
                Subs = Convert.ToInt32(dr["Subs"]);
                Apps = Convert.ToInt32(dr["Apps"]);
                Goals = Convert.ToInt32(dr["Goals"]);

                if (Convert.IsDBNull(dr["JoinDate"]))
                    JoinDate = null;
                else
                    JoinDate = Convert.ToDateTime(dr["JoinDate"]);

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

        public static List<Player> GetPlayers()
        {
            var dt = DataAccess.Player.GetPlayers();
            var list = new List<Player>();

            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    list.Add(new Player(dr));
                }
            }

            return list;
        }

        #region Members and Properties

        public Guid ID
        { get; set; }

        public string FirstName
        { get; set; }

        public string LastName
        { get; set; }

        public string DisplayName
        { get; set; }

        public string PrintingName
        { get; set; }

        public PlayerPositionType Position
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

        public string LeftYear
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

            }

            public static Player Load(Guid guid)
            {
                return PlayerList.Find(x => x.ID.Equals(guid));
            }

            public static List<Player> PlayerList;
        }
    }

    public enum PlayerPositionType
    {
        None = 0,
        Goalkeeper = 1,
        Defender = 2,
        Midfielder = 3,
        Forward = 4,
        Coach = 9
    }
}
