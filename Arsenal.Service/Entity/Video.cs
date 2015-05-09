using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

using Arsenalcn.Core;

namespace Arsenal.Service
{
    [AttrDbTable("Arsenal_Video", Key = "VideoGuid")]
    public class Video : Entity
    {
        public Video() : base() { }

        public Video(DataRow dr)
            : base(dr)
        {
            // Generate Video File Path
            VideoFilePath = string.Format("{0}{1}.{2}", ConfigGlobal.ArsenalVideoUrl, VideoGuid.ToString(), VideoType.ToString()).ToLower();

            // TEMP: Fix the video width & height equal 0
            VideoWidth = VideoWidth > 0 ? VideoWidth : 480;
            VideoHeight = VideoHeight > 0 ? VideoHeight : 270;
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
                VideoList = new Video().All<Video>().ToList();

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
                return VideoList.Find(v => v.VideoGuid.Equals(guid));
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
