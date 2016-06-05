using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Arsenal.Mobile.Models;
using Arsenal.Mobile.Models.Casino;
using Arsenal.Service;
using Arsenal.Service.Casino;
using Arsenalcn.Core;
using AutoMapper;
using Match = Arsenal.Service.Casino.Match;

namespace Arsenal.Mobile.Controllers
{
    [Authorize]
    public class CasinoController : Controller
    {
        private readonly IRepository _repo = new Repository();
        private readonly User _user = UserDto.GetSession();

        public int AcnID
        {
            get
            {
                int acnId = 0;

                if (_user?.AcnID != null)
                {
                    acnId = _user.AcnID.Value;
                }

                return acnId;
            }
        }

        // 可投注比赛
        // GET: /Casino

        [AllowAnonymous]
        public ActionResult Index()
        {
            var model = new IndexDto();
            var days = ConfigGlobal_AcnCasino.CasinoValidDays;

            var query = _repo.Query<MatchView>(
                x => x.PlayTime > DateTime.Now && x.PlayTime < DateTime.Now.AddDays(days))
                .FindAll(x => !x.ResultHome.HasValue && !x.ResultHome.HasValue)
                .OrderBy(x => x.PlayTime)
                .Many<MatchView, ChoiceOption, Guid>(t => t.CasinoItem.ID);

            var mapper = MatchDto.ConfigMapper().CreateMapper();

            var list = mapper.Map<IEnumerable<MatchDto>>(query.AsEnumerable());

            model.Matches = list;
            model.CasinoValidDays = days;

            model.Gambler = AcnID > 0 ? _repo.Query<Gambler>(x => x.UserID == AcnID).FirstOrDefault() : null;

            return View(model);
        }


        // 比分投注单
        // GET: /Casino/MyCoupon

        public ActionResult MyCoupon()
        {
            // TODO

            return View();
        }


        // 我的中奖查询
        // GET: /Casino/Bet

        public ActionResult MyBet(Criteria criteria)
        {
            var model = new MyBetDto();

            var query = _repo.Query<BetView>(criteria, x => x.UserID == AcnID)
                .Many<BetView, BetDetail, int>(t => t.ID);

            var mapper = BetDto.ConfigMapper().CreateMapper();

            var list = mapper.Map<IEnumerable<BetDto>>(query.AsEnumerable());

            model.Criteria = criteria;
            model.Data = list;

            return View(model);
        }


        // 我的盈亏情况
        // GET: /Casino/Bonus

        public ActionResult MyBonus()
        {
            // TODO

            return View();
        }


        // 比赛结果
        // GET: /Casino/Result

        [AllowAnonymous]
        public ActionResult Result(Criteria criteria)
        {
            var model = new ResultDto();

            var query = _repo.Query<MatchView>(criteria, x => x.ResultHome.HasValue && x.ResultAway.HasValue);

            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<MatchView, MatchDto>()
                .ConstructUsing(s => new MatchDto
                {
                    ID = s.ID,
                    TeamHomeName = s.Home.TeamDisplayName,
                    TeamHomeLogo = s.Home.TeamLogo,
                    TeamAwayName = s.Away.TeamDisplayName,
                    TeamAwayLogo = s.Away.TeamLogo
                }))
                .CreateMapper();

            var list = mapper.Map<IEnumerable<MatchDto>>(query.AsEnumerable());

            model.Criteria = criteria;
            model.Data = list;

            return View(model);
        }


        // 中奖查询
        // GET: /Casino/Detail/5
        [AllowAnonymous]
        public ActionResult Detail(Guid id)
        {
            var model = new DetailDto { Match = MatchDto.Single(id) };

            var query = _repo.Query<BetView>(x => x.CasinoItem.MatchGuid == id)
                .Many<BetView, BetDetail, int>(t => t.ID);

            var mapper = BetDto.ConfigMapper().CreateMapper();

            var list = mapper.Map<IEnumerable<BetDto>>(query.AsEnumerable());

            model.Bets = list;

            return View(model);
        }


        // 我要投注
        // GET: /Casino/GameBet

        public ActionResult GameBet(Guid id)
        {
            var model = new GameBetDto();

            var gambler = _repo.Query<Gambler>(x => x.UserID == AcnID).FirstOrDefault();
            model.MyCash = gambler?.Cash ?? 0f;

            model.Match = MatchDto.Single(id);

            // model.MyBets
            var betsQuery = _repo.Query<BetView>(x =>
                x.UserID == AcnID && x.CasinoItem.MatchGuid == id)
                .Many<BetView, BetDetail, int>(t => t.ID);

            var mapperBetDto = BetDto.ConfigMapper().CreateMapper();

            var bList = mapperBetDto.Map<IEnumerable<BetDto>>(betsQuery.AsEnumerable());

            model.MyBets = bList;

            // model.HistoryMatches
            var match = _repo.Single<Match>(id);

            var matchesQuery = _repo.Query<MatchView>(x => x.ResultHome.HasValue && x.ResultAway.HasValue)
                .FindAll(x => x.Home.ID.Equals(match.Home) && x.Away.ID.Equals(match.Away) ||
                              x.Home.ID.Equals(match.Away) && x.Away.ID.Equals(match.Home));

            var mapperMatchDto = new MapperConfiguration(cfg => cfg.CreateMap<MatchView, MatchDto>()
                .ConstructUsing(s => new MatchDto
                {
                    ID = s.ID,
                    TeamHomeName = s.Home.TeamDisplayName,
                    TeamHomeLogo = s.Home.TeamLogo,
                    TeamAwayName = s.Away.TeamDisplayName,
                    TeamAwayLogo = s.Away.TeamLogo
                }))
                .CreateMapper();

            var mlist = mapperMatchDto.Map<IEnumerable<MatchDto>>(matchesQuery.AsEnumerable());

            model.HistoryMatches = mlist;

            return View(model);
        }


        // 投输赢
        // GET: /Casino/SingleChoice/id

        public ActionResult SingleChoice(Guid id)
        {
            var model = new SingleChoiceDto();

            var gambler = _repo.Query<Gambler>(x => x.UserID == AcnID).FirstOrDefault();
            model.MyCash = gambler?.Cash ?? 0f;

            model.BetLimit = model.MyCash < 50000f ? model.MyCash : 50000f;

            model.Match = MatchDto.Single(id);
            model.MatchGuid = id;

            return View(model);
        }


        // 投输赢
        // POST: /Casino/SingleChoice

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SingleChoice(SingleChoiceDto model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var id = model.MatchGuid;

                    //Gambler in Lower could not bet above the SingleBetLimit of DefaultLeague (Contest)

                    // TODO
                    //if (m.LeagueGuid.Equals(ConfigGlobal_AcnCasino.DefaultLeagueID))
                    //{
                    //    if (Gambler.GetGamblerTotalBetByUserID(this.acnID, m.LeagueGuid) < ConfigGlobal_AcnCasino.TotalBetStandard)
                    //    {
                    //        float _alreadyMatchBet = Arsenalcn.CasinoSys.Entity.Bet.GetUserMatchTotalBet(this.acnID, id);

                    //        if (_alreadyMatchBet + ba > ConfigGlobal_AcnCasino.SingleBetLimit)
                    //        { throw new Exception(
                    //            $"下半赛区博彩玩家单场投注不能超过{ConfigGlobal_AcnCasino.SingleBetLimit.ToString("f2")}博彩币"); }
                    //    }
                    //}

                    if (model.BetAmount > ConfigGlobal_AcnCasino.SingleBetLimit)
                    { throw new Exception($"移动版单场不能超过{ConfigGlobal_AcnCasino.SingleBetLimit.ToString("f0")}博彩币"); }

                    var bet = new Bet
                    {
                        UserID = AcnID,
                        UserName = User.Identity.Name,
                        BetAmount = model.BetAmount
                    };

                    bet.Place(id, model.SelectedOption);

                    //投注成功

                    TempData["DataUrl"] = $"data-url=/Casino/GameBet/{id}";
                    return RedirectToAction("GameBet", new { id });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("Warn", ex.Message);
                }
            }
            else
            {
                ModelState.AddModelError("Warn", "请正确填写投注单");
            }

            model.Match = MatchDto.Single(model.MatchGuid);
            //model.MatchGuid = model.MatchGuid;

            return View(model);
        }


        // 猜比分
        // GET: /Casino/MatchResult/id

        public ActionResult MatchResult(Guid id)
        {
            var model = new MatchResultDto
            {
                Match = MatchDto.Single(id),
                MatchGuid = id
            };

            return View(model);
        }


        // 猜比分
        // POST: /Casino/MatchResult

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MatchResult(MatchResultDto model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var id = model.MatchGuid;

                    var bet = new Bet
                    {
                        UserID = AcnID,
                        UserName = User.Identity.Name
                    };

                    bet.Place(id, model.ResultHome, model.ResultAway);

                    //投注成功

                    TempData["DataUrl"] = $"data-url=/Casino/GameBet/{id}";
                    return RedirectToAction("GameBet", new { id });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("Warn", ex.Message);
                }
            }
            else
            {
                ModelState.AddModelError("Warn", "请正确填写比赛结果比分");
            }

            model.Match = MatchDto.Single(model.MatchGuid);
            //model.MatchGuid = model.MatchGuid;

            return View(model);
        }


        // 退还投注
        // GET: /Casino/ReturnBet/id

        public ActionResult ReturnBet(int id)
        {
            var model = BetDto.Single(id);

            return View(model);
        }


        // 退还投注
        // POST: /Casino/ReturnBet

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ReturnBet(BetDto model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var bet = _repo.Single<Bet>(model.ID);

                    if (bet != null)
                    {
                        if (bet.IsWin.HasValue) { throw new Exception("此投注已经发放了奖励"); }

                        if (bet.UserID != AcnID) { throw new Exception("此投注非当前用户的投注"); }

                        bet.ReturnBet();

                        //退注成功
                    }

                    TempData["DataUrl"] = "data-url=/Casino/MyBet";
                    return RedirectToAction("MyBet");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("Warn", ex.Message);
                }
            }
            else
            {
                ModelState.AddModelError("Warn", "请正确填写比赛结果比分");
            }

            model = BetDto.Single(model.ID);

            return View(model);
        }
    }
}