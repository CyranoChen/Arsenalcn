using System;
using System.Collections.Generic;
using System.Linq;

using iArsenal.Entity.ServiceProvider;

namespace iArsenal.Entity
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

                PlayerList = svc.GetPlayers().ToList<Arsenal.Player>();

                PlayerList.Sort(delegate(Arsenal.Player p1, Arsenal.Player p2)
                {
                    if (p1.SquadNumber == p2.SquadNumber)
                        return Comparer<string>.Default.Compare(p1.DisplayName, p2.DisplayName);
                    else
                        return p1.SquadNumber - p2.SquadNumber;
                });
            }

            public static Arsenal.Player Load(Guid guid)
            {
                return PlayerList.Find(p => p.PlayerGuid.Equals(guid));
            }

            public static List<Arsenal.Player> PlayerList;
        }
    }
}
