using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Arsenal.Mobile.Models;
using Arsenal.Mobile.Models.Club;
using Arsenal.Service;
using Arsenal.Service.Casino;
using Arsenal.Service.Club;
using Arsenalcn.Core;
using Arsenalcn.Core.Dapper;

namespace Arsenal.Mobile.Controllers
{
    [Authorize]
    public class ClubController : Controller
    {
        private readonly IRepository _repo = new Repository();
        private readonly User _user = UserDto.GetSession();

        // ReSharper disable once InconsistentNaming
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
            var user = UserDto.GetSession();

            using (var dapper = DapperHelper.GetInstance())
            {
                var trans = dapper.BeginTransaction();

                try
                {

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

                            var bonus = logSignIn.DoBonus(user.ID, ConfigGlobal_AcnClub.SignInKeyword);

                            // QSB
                            if (bonus > 0)
                            {
                                gambler.Cash += bonus * ConfigGlobal_AcnCasino.ExchangeRate;

                                _repo.Update(gambler);
                            }

                            trans.Commit();

                            model.Bonus = bonus * ConfigGlobal_AcnCasino.ExchangeRate;
                            model.MyCash = gambler.Cash;
                        }

                        model.ContinuousSignInDays = signInDays;
                    }
                    else
                    {
                        throw new Exception("当前无法签到");
                    }
                }
                catch (Exception ex)
                {
                    trans.Rollback();

                    model.ContinuousSignInDays = -1;
                    model.Tip = ex.Message;
                }
            }

            // 确定是否有活动补助金
            model.IsContestBonus =
                !_repo.Any<LogSignIn>(x => x.UserGuid == user.ID && x.Description == ConfigGlobal_AcnClub.SignInKeywordBonus);

            return View(model);
        }


        // 领取补助金
        // GET: /Club/ContestBonus

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ContestBonus()
        {
            using (var dapper = DapperHelper.GetInstance())
            {
                var trans = dapper.BeginTransaction();

                try
                {
                    var user = UserDto.GetSession();

                    if (user != null && ConfigGlobal_AcnClub.SignInActive)
                    {
                        if (user.AcnID.HasValue && user.AcnID == AcnID &&
                            !_repo.Any<LogSignIn>(x => x.UserGuid == user.ID && x.Description == ConfigGlobal_AcnClub.SignInKeywordBonus))
                        {
                            var gambler = _repo.Query<Gambler>(x => x.UserID == AcnID).FirstOrDefault();

                            if (gambler == null)
                            {
                                throw new Exception("无对应博彩玩家");
                            }

                            var bonus = SignInDailyDto.BonusAmount;

                            // do Contest Bonus
                            var logSignIn = new LogSignIn
                            {
                                UserGuid = user.ID,
                                UserName = user.UserName,
                                SignInTime = DateTime.Now,
                                Bonus = bonus,
                                SignInDays = -1,
                                Description = ConfigGlobal_AcnClub.SignInKeywordBonus
                            };

                            _repo.Insert(logSignIn);

                            // QSB
                            if (bonus > 0)
                            {
                                gambler.Cash += bonus * ConfigGlobal_AcnCasino.ExchangeRate;

                                _repo.Update(gambler);
                            }

                            trans.Commit();
                        }
                    }
                    else
                    {
                        throw new Exception("当前无法领取活动补助金");
                    }
                }
                catch
                {
                    trans.Rollback();
                }
            }

            TempData["DataUrl"] = "data-url=/Club/MyLogSignIn";
            return RedirectToAction("MyLogSignIn");
        }


        // 我的签到记录
        // GET: /Club/MyLogSignIn

        public ActionResult MyLogSignIn(Criteria criteria)
        {
            var model = new MyLogSignInDto();

            var query = _repo.Query<LogSignIn>(criteria, x => x.UserGuid == _user.ID && x.SignInTime > DateTime.Now.AddMonths(-1));

            var mapper = LogSignInDto.ConfigMapper().CreateMapper();

            var list = mapper.Map<IEnumerable<LogSignInDto>>(query.AsEnumerable());

            model.Criteria = criteria;
            model.Data = list;

            return View(model);
        }
    }
}