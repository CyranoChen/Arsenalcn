using System;
using System.Collections.Generic;
using System.Text;

using iArsenal.Entity.Arsenal;
using iArsenal.Entity.ServiceProvider;

namespace iArsenal.Entity
{
    public static class Arsenal_Match
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
                var arrayMatchs = svc.GetMatchs();

                if (MatchList != null)
                { MatchList.Clear(); }
                else
                { MatchList = new List<Match>(); }

                if (arrayMatchs != null && arrayMatchs.Length > 0)
                {
                    foreach (Match m in arrayMatchs)
                    {
                        MatchList.Add(m);
                    }
                }
            }

            public static Match Load(Guid guid)
            {
                return MatchList.Find(delegate(Match m) { return m.MatchGuid.Equals(guid); });
            }

            public static List<Match> MatchList;
        }
    }
}
