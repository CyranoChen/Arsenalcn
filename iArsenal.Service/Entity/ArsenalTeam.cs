using System;
using System.Collections.Generic;
using System.Linq;
using iArsenal.Service.Arsenal;
using iArsenal.Service.ServiceProvider;

namespace iArsenal.Service
{
    public static class Arsenal_Team
    {
        public static class Cache
        {
            public static List<Team> TeamList;

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

                TeamList = svc.GetTeams().OrderBy(x => x.TeamEnglishName).ToList();
            }

            public static Team Load(Guid guid)
            {
                return TeamList.Find(t => t.ID.Equals(guid));
            }
        }
    }
}