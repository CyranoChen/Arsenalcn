using System;
using System.Collections.Generic;
using System.Text;

using Arsenal.Entity.ServiceProvider;

namespace Arsenalcn.ClubSys.Service
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
                var arrayPlayers = svc.GetPlayers();

                if (PlayerList != null)
                { PlayerList.Clear(); }
                else
                { PlayerList = new List<Arsenal.Player>(); }

                if (arrayPlayers != null && arrayPlayers.Length > 0)
                {
                    foreach (Arsenal.Player p in arrayPlayers)
                    {
                        PlayerList.Add(p);
                    }
                }
            }

            public static Arsenal.Player Load(Guid guid)
            {
                return PlayerList.Find(delegate(Arsenal.Player p) { return p.PlayerGuid.Equals(guid); });
            }

            public static List<Arsenal.Player> PlayerList;
        }
    }
}
