using System;
using System.Collections.Generic;
using System.Text;

using Arsenal.Entity.ServiceProvider;

namespace Arsenalcn.ClubSys.Service
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
                { MatchList = new List<Arsenal.Match>(); }

                if (arrayMatchs != null && arrayMatchs.Length > 0)
                {
                    foreach (Arsenal.Match m in arrayMatchs)
                    {
                        MatchList.Add(m);
                    }
                }
            }

            public static Arsenal.Match Load(Guid guid)
            {
                return MatchList.Find(delegate(Arsenal.Match m) { return m.MatchGuid.Equals(guid); });
            }

            public static List<Arsenal.Match> MatchList;
        }
    }
}
