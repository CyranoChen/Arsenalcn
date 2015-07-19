using System;
using System.Data;

using Arsenalcn.Core;

namespace Arsenal.Service
{
    [DbSchema("Arsenalcn_User", Key = "UserGuid", Sort = "LastActivityDate DESC")]
    public class User : Entity<Guid>
    {
        public User() : base() { }

        public User(DataRow dr) : base(dr) { }

        #region Members and Properties

        [DbColumn("UserName")]
        public string UserName
        { get; set; }

        [DbColumn("IsAnonymous")]
        public bool IsAnonymous
        { get; set; }

        [DbColumn("LastActivityDate")]
        public DateTime LastActivityDate
        { get; set; }

        [DbColumn("AcnID")]
        public int? AcnID
        { get; set; }

        [DbColumn("AcnUserName")]
        public string AcnUserName
        { get; set; }

        [DbColumn("MemberID")]
        public int? MemberID
        { get; set; }

        [DbColumn("MemberName")]
        public string MemberName
        { get; set; }

        [DbColumn("WeChatOpenID")]
        public string WeChatOpenID
        { get; set; }

        [DbColumn("WeChatNickName")]
        public string WeChatNickName
        { get; set; }

        #endregion
    }
}
