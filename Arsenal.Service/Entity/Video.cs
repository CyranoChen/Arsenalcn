using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

using Arsenalcn.Core;

namespace Arsenal.Service
{
    [DbTable("Arsenal_Video", Key = "VideoGuid", Sort = "GoalYear DESC, GoalRank DESC, TeamworkRank DESC")]
    public class Video : Entity<Guid>
    {
        public Video() : base() { }

        public Video(DataRow dr)
            : base(dr)
        {
            // Generate Video File Path
            VideoFilePath = string.Format("{0}{1}.{2}",
                ConfigGlobal.ArsenalVideoUrl, this.ID.ToString(), VideoType.ToString()).ToLower();

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
                IRepository repo = new Repository();

                VideoList = repo.All<Video>();

                VideoList_Legend = VideoList.FindAll(x =>
                    x.GoalPlayerGuid.HasValue ? Player.Cache.Load(x.GoalPlayerGuid.Value).IsLegend : false);

                ColList_GoalYear = Repository.DistinctOrderBy(repo.Query<Video>(x => !string.IsNullOrEmpty(x.GoalYear)), x => x.GoalYear);
            }

            public static Video Load(Guid guid)
            {
                return VideoList.Find(x => x.ID.Equals(guid));
            }

            public static List<Video> VideoList;
            public static List<Video> VideoList_Legend;

            public static IEnumerable<string> ColList_GoalYear;
        }

        #region Members and Properties

        [DbColumn("FileName")]
        public string FileName
        { get; set; }

        [DbColumn("ArsenalMatchGuid")]
        public Guid? ArsenalMatchGuid
        { get; set; }

        [DbColumn("GoalPLayerGuid")]
        public Guid? GoalPlayerGuid
        { get; set; }

        [DbColumn("GoalPlayerName")]
        public string GoalPlayerName
        { get; set; }

        [DbColumn("AssistPlayerGuid")]
        public Guid? AssistPlayerGuid
        { get; set; }

        [DbColumn("AssistPlayerName")]
        public string AssistPlayerName
        { get; set; }

        [DbColumn("GoalRank")]
        public string GoalRank
        { get; set; }

        [DbColumn("TeamworkRank")]
        public string TeamworkRank
        { get; set; }

        [DbColumn("VideoType")]
        public VideoFileType VideoType
        { get; set; }

        [DbColumn("VideoLength")]
        public int VideoLength
        { get; set; }

        [DbColumn("VideoWidth")]
        public int VideoWidth
        { get; set; }

        [DbColumn("VideoHeight")]
        public int VideoHeight
        { get; set; }

        [DbColumn("GoalYear")]
        public string GoalYear
        { get; set; }

        [DbColumn("Opponent")]
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
