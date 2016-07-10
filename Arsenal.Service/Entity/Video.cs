using System;
using System.Collections.Generic;
using System.Data;
using Arsenalcn.Core;
using DataReaderMapper;

namespace Arsenal.Service
{
    [DbSchema("Arsenal_Video", Key = "VideoGuid", Sort = "GoalYear DESC, GoalRank DESC, TeamworkRank DESC")]
    public class Video : Entity<Guid>
    {
        public static void CreateMap()
        {
            var map = Mapper.CreateMap<IDataReader, Video>();

            map.ForMember(d => d.ID, opt => opt.MapFrom(s => (Guid) s.GetValue("VideoGuid")));
            //map.ForMember(d => d.VideoType, opt => opt.MapFrom(s =>
            //    (VideoFileType)Enum.Parse(typeof(VideoFileType), s.GetValue("VideoType").ToString())));

            map.ForMember(d => d.VideoFilePath, opt => opt.MapFrom(s =>
                $"{ConfigGlobal_Arsenal.ArsenalVideoUrl}{s.GetValue("VideoGuid").ToString()}.{((VideoFileType) Enum.Parse(typeof (VideoFileType), s.GetValue("VideoType").ToString())).ToString()}"));
        }

        public static class Cache
        {
            public static List<Video> VideoList;
            public static List<Video> VideoList_Legend;

            public static IEnumerable<string> ColList_GoalYear;

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

                ColList_GoalYear = repo.All<Video>().FindAll(x =>
                    !string.IsNullOrEmpty(x.GoalYear)).DistinctOrderBy(x => x.GoalYear);
            }

            public static Video Load(Guid guid)
            {
                return VideoList.Find(x => x.ID.Equals(guid));
            }
        }

        #region Members and Properties

        [DbColumn("FileName")]
        public string FileName { get; set; }

        [DbColumn("ArsenalMatchGuid")]
        public Guid? ArsenalMatchGuid { get; set; }

        [DbColumn("GoalPLayerGuid")]
        public Guid? GoalPlayerGuid { get; set; }

        [DbColumn("GoalPlayerName")]
        public string GoalPlayerName { get; set; }

        [DbColumn("AssistPlayerGuid")]
        public Guid? AssistPlayerGuid { get; set; }

        [DbColumn("AssistPlayerName")]
        public string AssistPlayerName { get; set; }

        [DbColumn("GoalRank")]
        public string GoalRank { get; set; }

        [DbColumn("TeamworkRank")]
        public string TeamworkRank { get; set; }

        [DbColumn("VideoType")]
        public VideoFileType VideoType { get; set; }

        [DbColumn("VideoLength")]
        public int VideoLength { get; set; }

        [DbColumn("VideoWidth")]
        public int VideoWidth { get; set; }

        [DbColumn("VideoHeight")]
        public int VideoHeight { get; set; }

        [DbColumn("GoalYear")]
        public string GoalYear { get; set; }

        [DbColumn("Opponent")]
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