using System;
using System.Collections.Generic;
using System.Data;
using Arsenalcn.ClubSys.Entity;

namespace Arsenalcn.ClubSys.Service
{
    public class Video
    {
        public Video()
        {
        }

        private Video(DataRow dr)
        {
            InitVideo(dr);
        }

        private void InitVideo(DataRow dr)
        {
            if (dr != null)
            {
                ID = (Guid) dr["VideoGuid"];
                FileName = dr["FileName"].ToString();

                if (!Convert.IsDBNull(dr["ArsenalMatchGuid"]))
                    ArsenalMatchGuid = (Guid) dr["ArsenalMatchGuid"];
                else
                    ArsenalMatchGuid = null;

                if (!Convert.IsDBNull(dr["GoalPlayerGuid"]))
                    GoalPlayerGuid = (Guid) dr["GoalPlayerGuid"];
                else
                    GoalPlayerGuid = null;

                if (!Convert.IsDBNull(dr["GoalPlayerName"]))
                    GoalPlayerName = dr["GoalPlayerName"].ToString();
                else
                    GoalPlayerName = null;

                if (!Convert.IsDBNull(dr["AssistPlayerGuid"]))
                    AssistPlayerGuid = (Guid) dr["AssistPlayerGuid"];
                else
                    AssistPlayerGuid = null;

                if (!Convert.IsDBNull(dr["AssistPlayerName"]))
                    AssistPlayerName = dr["AssistPlayerName"].ToString();
                else
                    AssistPlayerName = null;

                GoalRank = dr["GoalRank"].ToString();
                TeamworkRank = dr["TeamworkRank"].ToString();
                VideoType = (VideoFileType) Enum.Parse(typeof (VideoFileType), dr["VideoType"].ToString());
                VideoLength = Convert.ToInt16(dr["VideoLength"]);
                VideoWidth = Convert.ToInt16(dr["VideoWidth"]);
                VideoHeight = Convert.ToInt16(dr["VideoHeight"]);
                GoalYear = dr["GoalYear"].ToString();
                Opponent = dr["Opponent"].ToString();

                // Generate Video File Path
                VideoFilePath = string.Format("{0}{1}.{2}", ConfigGlobal.ArsenalVideoUrl, ID,
                    VideoType.ToString().ToLower());
            }
            else
                throw new Exception("Unable to init Video.");
        }

        public static List<Video> GetVideos()
        {
            var dt = DataAccess.Video.GetVideos();
            var list = new List<Video>();

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
            public static List<Video> VideoList;
            public static List<Video> VideoList_Legend;

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
                VideoList_Legend = VideoList.FindAll(x =>
                {
                    if (x.GoalPlayerGuid.HasValue)
                        return Player.Cache.Load(x.GoalPlayerGuid.Value).IsLegend;
                    return false;
                });
            }

            public static Video Load(Guid guid)
            {
                return VideoList.Find(x => x.ID.Equals(guid));
            }

            public static List<Video> GetAvailableVideosByRank(int GRank, int TRank)
            {
                // User can only get the Arsenal Player Video GoalRank <= 3 by usual way
                var list = VideoList_Legend.FindAll(x => Convert.ToInt16(x.GoalRank) <= 3);

                list = list.FindAll(x =>
                {
                    if (GRank > 0 && TRank > 0)
                    {
                        return x.GoalRank.Equals(GRank.ToString()) && x.TeamworkRank.Equals(TRank.ToString());
                    }
                    if (GRank <= 0 && TRank > 0)
                    {
                        return x.TeamworkRank.Equals(TRank.ToString());
                    }
                    if (GRank > 0 && TRank <= 0)
                    {
                        return x.GoalRank.Equals(GRank.ToString());
                    }
                    return true;
                });

                return list;
            }
        }

        #region Members and Properties

        public Guid ID { get; set; }

        public string FileName { get; set; }

        public Guid? ArsenalMatchGuid { get; set; }

        public Guid? GoalPlayerGuid { get; set; }

        public string GoalPlayerName { get; set; }

        public Guid? AssistPlayerGuid { get; set; }

        public string AssistPlayerName { get; set; }

        public string GoalRank { get; set; }

        public string TeamworkRank { get; set; }

        public VideoFileType VideoType { get; set; }

        public int VideoLength { get; set; }

        public int VideoWidth { get; set; }

        public int VideoHeight { get; set; }

        public string GoalYear { get; set; }

        public string Opponent { get; set; }

        public string VideoFilePath { get; set; }

        #endregion
    }

    public enum VideoFileType
    {
        flv = 0,
        mp4 = 1
    }
}