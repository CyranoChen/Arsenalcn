using System;
using System.Collections.Generic;
using System.Text;

using Arsenal.Entity.ServiceProvider;

namespace Arsenalcn.CasinoSys.Entity
{
    public static class League
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
                LeagueList.Clear();

                var svc = RemoteServiceProvider.GetWebService();
                var arrayLeagues = svc.GetLeagues();

                if (arrayLeagues != null && arrayLeagues.Length > 0)
                {
                    foreach (Arsenal.League l in arrayLeagues)
                    {
                        LeagueList.Add(l);
                    }
                }

                LeagueList_Active = LeagueList.FindAll(delegate(Arsenal.League l) { return l.IsActive; });
            }

            public static Arsenal.League Load(Guid guid)
            {
                return LeagueList.Find(delegate(Arsenal.League l) { return l.LeagueGuid.Equals(guid); });
                //return LeagueList.Find(l => l.LeagueGuid.Equals(guid));
            }

            public static List<Arsenal.League> GetSeasonsByLeagueGuid(Guid guid)
            {
                return LeagueList.FindAll(delegate(Arsenal.League l) { return l.LeagueName.Equals(Load(guid).LeagueName); });
            }

            public static List<Arsenal.League> LeagueList;
            public static List<Arsenal.League> LeagueList_Active;
        }
    }
}
