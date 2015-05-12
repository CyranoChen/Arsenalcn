using System;
using System.Collections.Generic;
using System.Text;

using Arsenal.Entity.ServiceProvider;

namespace Arsenalcn.ClubSys.Service
{
    public static class Arsenal_Team
    {
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
                var svc = RemoteServiceProvider.GetWebService();
                var arrayTeams = svc.GetTeams();

                if (TeamList != null)
                { TeamList.Clear(); }
                else
                { TeamList = new List<Arsenal.Team>(); }

                if (arrayTeams != null && arrayTeams.Length > 0)
                {
                    foreach (Arsenal.Team t in arrayTeams)
                    {
                        TeamList.Add(t);
                    }
                }
            }

            public static Arsenal.Team Load(Guid guid)
            {
                return TeamList.Find(delegate(Arsenal.Team t) { return t.ID.Equals(guid); });
            }

            public static List<Arsenal.Team> GetTeamsByLeagueGuid(Guid guid)
            {
                var svc = RemoteServiceProvider.GetWebService();
                var arrayTeams = svc.GetTeamsByLeagueGuid(guid);
                var list = new List<Arsenal.Team>();

                if (arrayTeams != null && arrayTeams.Length > 0)
                {
                    foreach (Arsenal.Team t in arrayTeams)
                    {
                        list.Add(t);
                    }
                }

                return list;
            }

            public static List<Arsenal.Team> TeamList;
        }
    }
}
