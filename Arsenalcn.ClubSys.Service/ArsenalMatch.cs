using System;
using System.Collections.Generic;
using System.Data;

namespace Arsenalcn.ClubSys.Service
{
    public class Match
    {
        public Match() { }

        private Match(DataRow dr)
        {
            InitMatch(dr);
        }

        private void InitMatch(DataRow dr)
        {
            if (dr != null)
            {
                ID = (Guid)dr["MatchGuid"];
                TeamGuid = (Guid)dr["TeamGuid"];
                TeamName = dr["TeamName"].ToString();
                IsHome = Convert.ToBoolean(dr["IsHome"]);

                if (Convert.IsDBNull(dr["ResultHome"]))
                    ResultHome = null;
                else
                    ResultHome = Convert.ToInt16(dr["ResultHome"]);

                if (Convert.IsDBNull(dr["ResultAway"]))
                    ResultAway = null;
                else
                    ResultAway = Convert.ToInt16(dr["ResultAway"]);

                PlayTime = Convert.ToDateTime(dr["PlayTime"]);

                if (Convert.IsDBNull(dr["LeagueGuid"]))
                    LeagueGuid = null;
                else
                    LeagueGuid = (Guid)dr["LeagueGuid"];

                LeagueName = dr["LeagueName"].ToString();

                if (Convert.IsDBNull(dr["Round"]))
                    Round = null;
                else
                    Round = Convert.ToInt16(dr["Round"]);

                IsActive = Convert.ToBoolean(dr["IsActive"]);
            }
            else
                throw new Exception("Unable to init Match.");
        }

        public static List<Match> GetMatches()
        {
            var dt = DataAccess.Match.GetMatches();
            var list = new List<Match>();

            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    list.Add(new Match(dr));
                }
            }

            return list;
        }

        #region Members and Properties

        public Guid ID
        { get; set; }

        public Guid TeamGuid
        { get; set; }

        public string TeamName
        { get; set; }

        public Boolean IsHome
        { get; set; }

        public short? ResultHome
        { get; set; }

        public short? ResultAway
        { get; set; }

        public DateTime PlayTime
        { get; set; }

        public Guid? LeagueGuid
        { get; set; }

        public string LeagueName
        { get; set; }

        public short? Round
        { get; set; }

        public Boolean IsActive
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
                MatchList = GetMatches();
            }

            public static Match Load(Guid guid)
            {
                return MatchList.Find(x => x.ID.Equals(guid));
            }

            public static List<Match> MatchList;
        }
    }
}
