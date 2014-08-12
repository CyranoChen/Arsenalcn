using System;
using System.Collections.Generic;
using System.Data;

namespace Arsenal.Entity
{
    public class Video
    {
        public Video() { }

        private Video(DataRow dr)
        {
            InitVideo(dr);
        }

        private void InitVideo(DataRow dr)
        {
            if (dr != null)
            {
                VideoGuid = (Guid)dr["VideoGuid"];
                FileName = dr["FileName"].ToString();

                if (!Convert.IsDBNull(dr["ArsenalMatchGuid"]))
                    ArsenalMatchGuid = (Guid)dr["ArsenalMatchGuid"];
                else
                    ArsenalMatchGuid = null;

                if (!Convert.IsDBNull(dr["GoalPlayerGuid"]))
                    GoalPlayerGuid = (Guid)dr["GoalPlayerGuid"];
                else
                    GoalPlayerGuid = null;

                if (!Convert.IsDBNull(dr["GoalPlayerName"]))
                    GoalPlayerName = dr["GoalPlayerName"].ToString();
                else
                    GoalPlayerName = null;

                if (!Convert.IsDBNull(dr["AssistPlayerGuid"]))
                    AssistPlayerGuid = (Guid)dr["AssistPlayerGuid"];
                else
                    AssistPlayerGuid = null;

                if (!Convert.IsDBNull(dr["AssistPlayerName"]))
                    AssistPlayerName = dr["AssistPlayerName"].ToString();
                else
                    AssistPlayerName = null;

                GoalRank = dr["GoalRank"].ToString();
                TeamworkRank = dr["TeamworkRank"].ToString();
                VideoLength = Convert.ToInt16(dr["VideoLength"]);
                VideoWidth = Convert.ToInt16(dr["VideoWidth"]);
                VideoHeight = Convert.ToInt16(dr["VideoHeight"]);
                GoalYear = dr["GoalYear"].ToString();
                Opponent = dr["Opponent"].ToString();

                // Fix the video width & height equal 0
                VideoWidth = VideoWidth > 0 ? VideoWidth : 480;
                VideoHeight = VideoHeight > 0 ? VideoHeight : 270;
            }
            else
                throw new Exception("Unable to init Video.");
        }

        public void Select()
        {
            DataRow dr = DataAccess.Video.GetVideoByID(VideoGuid);

            if (dr != null)
                InitVideo(dr);
        }

        public void Update()
        {
            DataAccess.Video.UpdateVideo(VideoGuid, FileName, ArsenalMatchGuid, GoalPlayerGuid, GoalPlayerName, AssistPlayerGuid, AssistPlayerName, GoalRank, TeamworkRank, VideoLength, VideoWidth, VideoHeight, GoalYear, Opponent);
        }

        public void Insert()
        {
            DataAccess.Video.InsertVideo(VideoGuid, FileName, ArsenalMatchGuid, GoalPlayerGuid, GoalPlayerName, AssistPlayerGuid, AssistPlayerName, GoalRank, TeamworkRank, VideoLength, VideoWidth, VideoHeight, GoalYear, Opponent);
        }

        public void Delete()
        {
            DataAccess.Video.DeleteVideo(VideoGuid);
        }

        public static List<Video> GetVideos()
        {
            DataTable dt = DataAccess.Video.GetVideos();
            List<Video> list = new List<Video>();

            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    list.Add(new Video(dr));
                }
            }

            return list;
        }

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
                VideoList = GetVideos();

                VideoList_Legend = VideoList.FindAll(delegate(Video v)
                {
                    if (v.GoalPlayerGuid.HasValue)
                        return Player.Cache.Load(v.GoalPlayerGuid.Value).IsLegend;
                    else
                        return false;
                });

                ColList_GoalYear = DataAccess.Video.GetVideoDistColumn("GoalYear", false);
            }

            public static Video Load(Guid guid)
            {
                return VideoList.Find(delegate(Video v) { return v.VideoGuid.Equals(guid); });
            }

            public static List<Video> GetAvailableVideosByRank(int GRank, int TRank)
            {
                // User can only get the Arsenal Player Video GoalRank <= 3 by usual way
                List<Video> list = VideoList_Legend.FindAll(delegate(Video v) { return Convert.ToInt16(v.GoalRank) <= 3; });

                list = list.FindAll(delegate(Video v)
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

            public static List<Video> VideoList;
            public static List<Video> VideoList_Legend;

            public static DataTable ColList_GoalYear;
        }

        #region Members and Properties

        public Guid VideoGuid
        { get; set; }

        public string FileName
        { get; set; }

        public Guid? ArsenalMatchGuid
        { get; set; }

        public Guid? GoalPlayerGuid
        { get; set; }

        public string GoalPlayerName
        { get; set; }

        public Guid? AssistPlayerGuid
        { get; set; }

        public string AssistPlayerName
        { get; set; }

        public string GoalRank
        { get; set; }

        public string TeamworkRank
        { get; set; }

        public int VideoLength
        { get; set; }

        public int VideoWidth
        { get; set; }

        public int VideoHeight
        { get; set; }

        public string GoalYear
        { get; set; }

        public string Opponent
        { get; set; }

        #endregion
    }
}
