using System;
using System.Collections.Generic;
using System.Linq;

using iArsenal.Service.Arsenal;
using iArsenal.Service.ServiceProvider;

namespace iArsenal.Service
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

                MatchList = svc.GetMatchs().OrderByDescending(x => x.PlayTime).ToList();

                //var arrayMatchs = svc.GetMatchs();

                //if (MatchList != null)
                //{ MatchList.Clear(); }
                //else
                //{ MatchList = new List<Match>(); }

                //if (arrayMatchs != null && arrayMatchs.Length > 0)
                //{
                //    foreach (var m in arrayMatchs)
                //    {
                //        MatchList.Add(m);
                //    }
                //}
            }

            public static Match Load(Guid guid)
            {
                return MatchList.Find(x => x.ID.Equals(guid));
            }

            public static List<Match> MatchList;
        }
    }
}
