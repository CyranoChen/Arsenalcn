using System;
using System.Data;

using Arsenalcn.Core;

namespace Arsenal.Service.Casino
{
    [DbSchema("AcnCasino_Group", Key = "GroupGuid", Sort = "GroupOrder")]
    public class Group : Entity<Guid>
    {
        public Group() : base() { }

        public Group(DataRow dr) : base(dr) { }

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
