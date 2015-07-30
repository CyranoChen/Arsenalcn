using System;
using System.Data;

using Arsenalcn.Core;

namespace Arsenal.Service.Casino
{
    [DbSchema("AcnCasino_Group", Key = "GroupGuid", Sort = "GroupOrder")]
    public class Group : Entity<Guid>
    {
        public Group() : base() { }

        public static void CreateMap()
        {
            var map = AutoMapper.Mapper.CreateMap<IDataReader, Group>();

            map.ForMember(d => d.ID, opt => opt.MapFrom(s => (Guid)s.GetValue("GroupGuid")));
        }

        #region Members and Properties

        [DbColumn("GroupName")]
        public string GroupName
        { get; set; }

        [DbColumn("GroupOrder")]
        public int GroupOrder
        { get; set; }

        [DbColumn("LeagueGuid")]
        public Guid LeagueGuid
        { get; set; }

        [DbColumn("IsTable")]
        public bool IsTable
        { get; set; }

        #endregion
    }
}
