using System;
using Arsenalcn.Core;

namespace Arsenal.Service
{
    [DbSchema("Arsenalcn_UserWeChat", Key = "UserGuid", Sort = "LastAuthorizeDate DESC")]
    public class UserWeChat : Entity<Guid>
    {
        #region Members and Properties

        [DbColumn("UserName")]
        public string UserName { get; set; }

        [DbColumn("LastAuthorizeDate")]
        public DateTime LastAuthorizeDate { get; set; }

        [DbColumn("AccessToken")]
        public string AccessToken { get; set; }

        [DbColumn("AccessTokenExpiredDate")]
        public DateTime AccessTokenExpiredDate { get; set; }

        [DbColumn("RefreshToken")]
        public string RefreshToken { get; set; }

        [DbColumn("RefreshTokenExpiredDate")]
        public DateTime RefreshTokenExpiredDate { get; set; }

        [DbColumn("Gender")]
        public short Gender { get; set; }

        [DbColumn("Province")]
        public string Province { get; set; }

        [DbColumn("City")]
        public string City { get; set; }

        [DbColumn("Country")]
        public string Country { get; set; }

        [DbColumn("HeadImgUrl")]
        public string HeadImgUrl { get; set; }

        [DbColumn("Privilege")]
        public string Privilege { get; set; }

        [DbColumn("UnionID")]
        // ReSharper disable once InconsistentNaming
        public string UnionID { get; set; }

        #endregion
    }
}
