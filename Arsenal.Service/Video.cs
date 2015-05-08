using System;
using System.Collections.Generic;
using System.Data;

using Arsenalcn.Core;

namespace Arsenal.Service
{
    [AttrDbTable("Arsenal_Video", Key = "VideoGuid")]
    public class Video : Entity
    {
        public Video() { }

        public Video(DataRow dr)
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
                VideoType = (VideoFileType)Enum.Parse(typeof(VideoFileType), dr["VideoType"].ToString());
                VideoLength = Convert.ToInt16(dr["VideoLength"]);
                VideoWidth = Convert.ToInt16(dr["VideoWidth"]);
                VideoHeight = Convert.ToInt16(dr["VideoHeight"]);
                GoalYear = dr["GoalYear"].ToString();
                Opponent = dr["Opponent"].ToString();

                // Generate Video File Path
                //if (!string.IsNullOrEmpty(FileName))
                //{
                //    VideoFilePath = FileName.ToLower();
                //}
                //else
                //{
                //    VideoFilePath = string.Format("{0}{1}.{2}", ConfigGlobal.ArsenalVideoUrl, VideoGuid.ToString(), VideoType.ToString()).ToLower();
                //}

                VideoFilePath = string.Format("{0}{1}.{2}", ConfigGlobal.ArsenalVideoUrl, VideoGuid.ToString(), VideoType.ToString()).ToLower();

                // TEMP: Fix the video width & height equal 0
                VideoWidth = VideoWidth > 0 ? VideoWidth : 480;
                VideoHeight = VideoHeight > 0 ? VideoHeight : 270;
            }
            else
                throw new Exception("Unable to init Video.");
        }

        //public void Select()
        //{
        //    DataRow dr = DataAccess.Video.GetVideoByID(VideoGuid);

        //    if (dr != null)
        //        InitVideo(dr);
        //}

        //public void Update()
        //{
        //    DataAccess.Video.UpdateVideo(VideoGuid, FileName, ArsenalMatchGuid, GoalPlayerGuid, GoalPlayerName, AssistPlayerGuid, AssistPlayerName, GoalRank, TeamworkRank, VideoType.ToString(), VideoLength, VideoWidth, VideoHeight, GoalYear, Opponent);
        //}

        //public void Insert()
        //{
        //    DataAccess.Video.InsertVideo(VideoGuid, FileName, ArsenalMatchGuid, GoalPlayerGuid, GoalPlayerName, AssistPlayerGuid, AssistPlayerName, GoalRank, TeamworkRank, VideoType.ToString(), VideoLength, VideoWidth, VideoHeight, GoalYear, Opponent);
        //}

        //public void Delete()
        //{
        //    DataAccess.Video.DeleteVideo(VideoGuid);
        //}

        //public static List<Video> GetVideos()
        //{
        //    DataTable dt = DataAccess.Video.GetVideos();
        //    List<Video> list = new List<Video>();

        //    if (dt != null)
        //    {
        //        foreach (DataRow dr in dt.Rows)
        //        {
        //            list.Add(new Video(dr));
        //        }
        //    }

        //    return list;
        //}

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
                VideoList = new Video().All<Video>();

                VideoList_Legend = VideoList.FindAll(delegate(Video v)
                {
                    if (v.GoalPlayerGuid.HasValue)
                        return Player.Cache.Load(v.GoalPlayerGuid.Value).IsLegend;
                    else
                        return false;
                });

                //ColList_GoalYear = DataAccess.Video.GetVideoDistColumn("GoalYear", false);
            }

            public static Video Load(Guid guid)
            {
                return VideoList.Find(delegate(Video v) { return v.VideoGuid.Equals(guid); });
            }

            public static List<Video> VideoList;
            public static List<Video> VideoList_Legend;

            public static DataTable ColList_GoalYear;
        }

        #region Members and Properties

        [AttrDbColumn("VideoGuid", IsKey = true)]
        public Guid VideoGuid
        { get; set; }

        [AttrDbColumn("FileName")]
        public string FileName
        { get; set; }

        [AttrDbColumn("ArsenalMatchGuid")]
        public Guid? ArsenalMatchGuid
        { get; set; }

        [AttrDbColumn("GoalPLayerGuid")]
        public Guid? GoalPlayerGuid
        { get; set; }

        [AttrDbColumn("GoalPlayerName")]
        public string GoalPlayerName
        { get; set; }

        [AttrDbColumn("AssistPlayerGuid")]
        public Guid? AssistPlayerGuid
        { get; set; }

        [AttrDbColumn("AssistPlayerName")]
        public string AssistPlayerName
        { get; set; }

        [AttrDbColumn("GoalRank")]
        public string GoalRank
        { get; set; }

        [AttrDbColumn("TeamworkRank")]
        public string TeamworkRank
        { get; set; }

        [AttrDbColumn("VideoType")]
        public VideoFileType VideoType
        { get; set; }

        [AttrDbColumn("VideoLength")]
        public int VideoLength
        { get; set; }

        [AttrDbColumn("VideoWidth")]
        public int VideoWidth
        { get; set; }

        [AttrDbColumn("VideoHeight")]
        public int VideoHeight
        { get; set; }

        [AttrDbColumn("GoalYear")]
        public string GoalYear
        { get; set; }

        [AttrDbColumn("Opponent")]
        public string Opponent
        { get; set; }

        public string VideoFilePath
        { get; set; }

        #endregion
    }

    public enum VideoFileType
    {
        flv,
        mp4
    }
}
