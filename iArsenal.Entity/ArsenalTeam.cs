using System;
using System.Collections.Generic;
using System.Linq;

using iArsenal.Entity.ServiceProvider;

namespace iArsenal.Entity
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

                TeamList = svc.GetTeams().ToList<Arsenal.Team>();
            }

            public static Arsenal.Team Load(Guid guid)
            {
                return TeamList.Find(t => t.TeamGuid.Equals(guid));
            }

            public static List<Arsenal.Team> TeamList;
        }
    }
}
