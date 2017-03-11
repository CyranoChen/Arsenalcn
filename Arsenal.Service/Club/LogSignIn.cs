using System;
using System.Data;
using System.Linq;
using Arsenalcn.Core;

namespace Arsenal.Service.Club
{
    [DbSchema("AcnClub_LogSignIn", Sort = "SignInTime DESC")]
    public class LogSignIn : Entity<int>
    {
        //public static void CreateMap()
        //{
        //    var map = Mapper.CreateMap<IDataReader, LogSignIn>();

        //    map.ForMember(d => d.Bonus, opt => opt.MapFrom(s => s.GetValue("Bonus")));
        //}

        public double DoBonus(Guid userGuid, string keyword, IDbTransaction trans = null)
        {
            IRepository repo = new Repository();

            var membership = repo.Single<Membership>(userGuid);

            if (membership == null)
            { throw new Exception("无对应用户信息"); }

            var rate = GetContinuousSignInDays(userGuid);

            if (rate <= 0)
            { throw new Exception("您已经领取过今天的签到奖励"); }

            // set SignInDay in advance
            SignInDays = rate;

            if (rate > ConfigGlobal_AcnClub.SignInMaxRate)
            { rate = ConfigGlobal_AcnClub.SignInMaxRate; }

            // do signIn bonus
            UserGuid = membership.ID;
            UserName = membership.UserName;
            SignInTime = DateTime.Now;

            Bonus = ConfigGlobal_AcnClub.SignInBonus * rate;
            Description = keyword;

            repo.Insert(this, trans);

            return Bonus;
        }

        public static int GetContinuousSignInDays(Guid userGuid)
        {
            IRepository repo = new Repository();

            var retValue = 1;

            var lastSignIn = repo.Query<LogSignIn>(x => x.UserGuid == userGuid && x.SignInDays > 0).FirstOrDefault();

            if (lastSignIn != null)
            {
                if (lastSignIn.SignInTime.Date == DateTime.Now.AddDays(-1).Date)
                {
                    retValue = lastSignIn.SignInDays + 1;
                }
                else if (lastSignIn.SignInTime.Date == DateTime.Now.Date)
                {
                    retValue = 0;
                }
            }

            return retValue;
        }

        #region Members and Properties

        [DbColumn("UserGuid")]
        public Guid UserGuid { get; set; }

        [DbColumn("UserName")]
        public string UserName { get; set; }

        [DbColumn("SignInTime")]
        public DateTime SignInTime { get; set; }

        [DbColumn("Bonus")]
        public double Bonus { get; set; }

        [DbColumn("SignInDays")]
        public int SignInDays { get; set; }

        [DbColumn("Description")]
        public string Description { get; set; }

        #endregion
    }
}