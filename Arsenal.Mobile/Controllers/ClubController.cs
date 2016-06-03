using System;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;
using Arsenal.Mobile.Models;
using Arsenal.Mobile.Models.Club;
using Arsenal.Service;
using Arsenal.Service.Casino;
using Arsenal.Service.Club;
using Arsenalcn.Core;

namespace Arsenal.Mobile.Controllers
{
    [Authorize]
    public class ClubController : Controller
    {
        private readonly IRepository _repo = new Repository();
        private readonly User _user = UserDto.GetSession();

        public int AcnID
        {
            get
            {
                var acnId = 0;

                if (_user?.AcnID != null)
                {
                    acnId = _user.AcnID.Value;
                }

                return acnId;
            }
        }


        // 球会首页
        // GET: /Club

        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }


        // 每日签到
        // GET: /Club/SignInDaily

        public ActionResult SignInDaily()
        {
            var model = new SignInDailyDto();

            using (var conn = new SqlConnection(DataAccess.ConnectString))
            {
                conn.Open();
                var trans = conn.BeginTransaction();

                try
                {
                    var user = UserDto.GetSession();

                    if (user != null && ConfigGlobal_AcnClub.SignInActive)
                    {
                        var signInDays = LogSignIn.GetContinuousSignInDays(user.ID);

                        if (signInDays > 0 && user.AcnID.HasValue && user.AcnID == AcnID)
                        {
                            var gambler = _repo.Query<Gambler>(x => x.UserID == AcnID).FirstOrDefault();

                            if (gambler == null)
                            {
                                throw new Exception("无对应博彩玩家");
                            }

                            model.CurrentCash = gambler.Cash;

                            // do SignIn Bonus
                            var logSignIn = new LogSignIn();

                            var bonus = logSignIn.DoBonus(user.ID, "euro2016", trans);

                            // QSB
                            if (bonus > 0)
                            {
                                gambler.Cash += bonus * ConfigGlobal_AcnCasino.ExchangeRate;

                                _repo.Update(gambler, trans);
                            }

                            trans.Commit();

                            model.Bonus = bonus * ConfigGlobal_AcnCasino.ExchangeRate;
                            model.MyCash = gambler.Cash;
                        }

                        model.ContinuousSignInDays = signInDays;
                    }
                    else
                    {
                        throw new Exception("无对应用户信息");
                    }
                }
                catch (Exception ex)
                {
                    trans.Rollback();

                    model.ContinuousSignInDays = -1;
                    model.Tip = ex.Message;
                }
            }

            return View(model);
        }
    }
}