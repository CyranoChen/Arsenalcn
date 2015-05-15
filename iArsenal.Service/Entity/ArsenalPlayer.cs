using System;
using System.Collections.Generic;
using System.Linq;

using iArsenal.Service.Arsenal;
using iArsenal.Service.ServiceProvider;

namespace iArsenal.Service
{
    public static class Arsenal_Player
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

                PlayerList = svc.GetPlayers().OrderBy(x => x.SquadNumber).ThenBy(x => x.DisplayName).ToList();
            }

            public static Player Load(Guid guid)
            {
                return PlayerList.Find(p => p.ID.Equals(guid));
            }

            public static List<Player> PlayerList;
        }
    }
}
