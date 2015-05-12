using System;
using System.Collections.Generic;
using System.Text;

using Arsenal.Entity.ServiceProvider;

namespace Arsenalcn.ClubSys.Service
{
    public static class Arsenal_Video
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
                var arrayVideos = svc.GetVideos();

                if (VideoList != null)
                { VideoList.Clear(); }
                else
                { VideoList = new List<Arsenal.Video>(); }

                if (arrayVideos != null && arrayVideos.Length > 0)
                {
                    foreach (Arsenal.Video v in arrayVideos)
                    {
                        VideoList.Add(v);
                    }
                }

                VideoList_Legend = VideoList.FindAll(delegate(Arsenal.Video v)
                {
                    if (v.GoalPlayerGuid.HasValue)
                        return Arsenal_Player.Cache.Load(v.GoalPlayerGuid.Value).IsLegend;
                    else
                        return false;
                });
            }

            public static Arsenal.Video Load(Guid guid)
            {
                return VideoList.Find(delegate(Arsenal.Video v) { return v.ID.Equals(guid); });
            }

            public static List<Arsenal.Video> GetAvailableVideosByRank(int GRank, int TRank)
            {
                // User can only get the Arsenal Player Video GoalRank <= 3 by usual way
                List<Arsenal.Video> list = VideoList_Legend.FindAll(delegate(Arsenal.Video v)
                { return Convert.ToInt16(v.GoalRank) <= 3; });

                list = list.FindAll(delegate(Arsenal.Video v)
                {
                    if (GRank > 0 && TRank > 0)
                    {
                        return v.GoalRank.Equals(GRank.ToString()) && v.TeamworkRank.Equals(TRank.ToString());
                    }
                    else if (GRank <= 0 && TRank > 0)
                    {
                        return v.TeamworkRank.Equals(TRank.ToString());
                    }
                    else if (GRank > 0 && TRank <= 0)
                    {
                        return v.GoalRank.Equals(GRank.ToString());
                    }
                    else
                    {
                        return true;
                    }
                });

                return list;
            }

            public static List<Arsenal.Video> VideoList;
            public static List<Arsenal.Video> VideoList_Legend;
        }
    }
}
