using System;
using System.Data;
using Arsenalcn.Core;
using DataReaderMapper;

namespace Arsenal.Service
{
    [DbSchema("Arsenalcn_UserWeChat", Key = "UserGuid", Sort = "LastAuthorizeDate DESC")]
    public class UserWeChat : Entity<Guid>
    {
        public static void CreateMap()
        {
            var map = Mapper.CreateMap<IDataReader, UserWeChat>();

            map.ForMember(d => d.ID, opt => opt.MapFrom(s => (Guid)s.GetValue("UserGuid")));
        }

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
        public bool? Gender { get; set; }

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
