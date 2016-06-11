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
using IndexDto = Arsenal.Mobile.Models.Casino.IndexDto;
using Match = Arsenal.Service.Casino.Match;

namespace Arsenal.Mobile.Controllers
{
    [Authorize]
    public class CasinoController : Controller
    {
        private readonly IRepository _repo = new Repository();
        private readonly User _user = UserDto.GetSession();

        // ReSharper disable once InconsistentNaming
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
                .FindAll(x => !x.ResultHome.HasValue && !x.ResultAway.HasValue)
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
            var model = new MyCouponDto();

            var days = ConfigGlobal_AcnCasino.CasinoValidDays;

            var query = _repo.Query<MatchView>(
                x => x.PlayTime > DateTime.Now && x.PlayTime < DateTime.Now.AddDays(days))
                .FindAll(x => !x.ResultHome.HasValue && !x.ResultAway.HasValue)
                .OrderBy(x => x.PlayTime)
                .Many<MatchView, ChoiceOption, Guid>(t => t.CasinoItem.ID);

            var mapper = MatchDto.ConfigMapper().CreateMapper();

            var mList = mapper.Map<IEnumerable<MatchDto>>(query.AsEnumerable());

            mapper = new MapperConfiguration(cfg => cfg.CreateMap<MatchDto, CouponDto>()
                .ForMember(d => d.MatchGuid, opt => opt.MapFrom(s => s.ID))).CreateMapper();

            var list = mapper.Map<IEnumerable<CouponDto>>(mList).ToList();

            if (list.Count > 0)
            {
                // 查找当前用户的比分投注项
                var coupons = _repo.Query<CouponView>(x => x.UserID == AcnID);

                if (coupons.Count > 0)
                {
                    mapper = CouponDto.ConfigMapper().CreateMapper();

                    foreach (var c in coupons)
                    {
                        var i = list.FindIndex(x => x.MatchGuid.Equals(c.MatchGuid));

                        if (i >= 0)
                        {
                            list[i] = mapper.Map<CouponDto>(c);
                        }
                    }
                }
            }

            model.Coupons = list;
            model.CasinoValidDays = days;
            model.IsShowSubmitButton = list.Count > 0 && list.Any(x => !x.BetResultHome.HasValue && !x.BetResultAway.HasValue);

            return View(model);
        }


        // 比分投注单
        // POST: /Casino/MyCoupon

        [HttpPost]
        public JsonResult MyCoupon(MyCouponDto model)
        {
            if (model != null && model.Coupons.Any())
            {
                foreach (var c in model.Coupons)
                {
                    try
                    {
                        if (c.MatchGuid != Guid.Empty && c.BetResultHome.HasValue && c.BetResultAway.HasValue)
                        {
                            var bet = new Bet
                            {
                                UserID = AcnID,
                                UserName = User.Identity.Name
                            };

                            bet.Place(c.MatchGuid, c.BetResultHome.Value, c.BetResultAway.Value);

                            //投注成功
                        }
                    }
                    catch
                    {
                        // ignored
                    }
                }
            }

            // 不管保存结果，直接返回success后，客户端刷新页面
            return Json(new { result = "success" });
        }


        // 实时排行榜
        // GET: /Casino/Rank

        [AllowAnonymous]
        public ActionResult Rank()
        {
            var model = new RankDto();

            var league = League.Cache.Load(ConfigGlobal_AcnCasino.DefaultLeagueID);

            if (league != null)
            {
                var list = GamblerDW.All(league.ID);

                if (list != null && list.Count > 0)
                {
                    // 进入最终名次排行榜的标准（评选要求）同时满足以下3个条件：
                    //1、赛季中必须投注博采币次数达到5个单场及以上（反复多次投注同一场比赛只能算是1次）；
                    //2、赛季中参与累计投注量达到5,000菠菜币及以上；
                    //3、赛季中并且获得RP+3及以上，即猜对本赛季3场以上的比赛比分。
                    if (!ConfigGlobal_AcnCasino.ContestLimitIgnore)
                    {
                        list = list.FindAll(g => g.MatchBet >= ConfigGlobal_AcnCasino.RankCondition[0]
                                                 && g.TotalBet >= ConfigGlobal_AcnCasino.RankCondition[1] &&
                                                 g.RPBonus >= ConfigGlobal_AcnCasino.RankCondition[2]);
                    }

                    var tbs = ConfigGlobal_AcnCasino.TotalBetStandard;

                    var listUpper = list.FindAll(g => g.TotalBet > tbs);

                    var listLower = list.FindAll(g => g.TotalBet <= tbs);

                    if (listUpper.Count > 0)
                    {
                        listUpper = GamblerDW.SortRank(listUpper);
                    }

                    if (listLower.Count > 0)
                    {
                        listLower = GamblerDW.SortRank(listLower);
                    }

                    model.UpperGamblers = listUpper.Take(6);
                    model.LowerGamblers = listLower.Take(6);
                }

                model.RankCondition = ConfigGlobal_AcnCasino.RankCondition;
                model.ContestLeague = league;
            }

            return View(model);
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

            var mapper = MatchDto.ConfigMapper().CreateMapper();

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

            var mapper = BetDto.ConfigMapper().CreateMapper();

            var bList = mapper.Map<IEnumerable<BetDto>>(betsQuery.AsEnumerable());

            model.MyBets = bList;

            // model.HistoryMatches
            var match = _repo.Single<Match>(id);

            var matchesQuery = _repo.Query<MatchView>(x => x.ResultHome.HasValue && x.ResultAway.HasValue)
                .FindAll(x => x.Home.ID.Equals(match.Home) && x.Away.ID.Equals(match.Away) ||
                              x.Home.ID.Equals(match.Away) && x.Away.ID.Equals(match.Home));

            mapper = MatchDto.ConfigMapper().CreateMapper();

            var mlist = mapper.Map<IEnumerable<MatchDto>>(matchesQuery.AsEnumerable());

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

            // 判断玩家是否有单注限制
            var leagueGuid = _repo.Single<Match>(id)?.LeagueGuid;

            if (leagueGuid.HasValue)
            {
                model.BetLimit = GamblerDW.GetGamblerBetLimit(AcnID, leagueGuid.Value);
            }
            else
            {
                model.BetLimit = model.MyCash;
            }

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
            var id = model.MatchGuid;
            var leagueGuid = _repo.Single<Match>(id)?.LeagueGuid;

            if (ModelState.IsValid)
            {
                try
                {
                    //Gambler in Lower could not bet above the SingleBetLimit of DefaultLeague (Contest)
                    if (leagueGuid != null && leagueGuid.Equals(ConfigGlobal_AcnCasino.DefaultLeagueID))
                    {
                        var g = GamblerDW.Single(AcnID, leagueGuid.Value);

                        // 如果没有投过注，或投注量小于标准，判断是否在下半赛区
                        if (g == null || g.TotalBet < ConfigGlobal_AcnCasino.TotalBetStandard)
                        {
                            var singleBetLimit = ConfigGlobal_AcnCasino.SingleBetLimit;
                            double? alreadyMatchBet = null;

                            // 获取单场比赛当前用户投注总额
                            var item = _repo.Query<CasinoItem>(x => x.MatchGuid == id
                                && x.ItemType == CasinoType.SingleChoice).FirstOrDefault();
                            var myBets = _repo.Query<Bet>(x => x.CasinoItemGuid == item.ID && x.UserID == AcnID);

                            if (myBets.Count > 0)
                            {
                                alreadyMatchBet = myBets.Sum(x => x.BetAmount);
                            }

                            // 已投注量+本次投注量不可超过单场比赛限额
                            if ((alreadyMatchBet ?? 0) + model.BetAmount > singleBetLimit)
                            {
                                throw new Exception($"下半赛区每个玩家单场投注总量不能超过{singleBetLimit.ToString("N0")}博彩币");
                            }
                        }
                    }

                    //if (model.BetAmount > ConfigGlobal_AcnCasino.SingleBetLimit)
                    //{ throw new Exception($"移动版单场不能超过{ConfigGlobal_AcnCasino.SingleBetLimit.ToString("f0")}博彩币"); }

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

            var gambler = _repo.Query<Gambler>(x => x.UserID == AcnID).FirstOrDefault();

            model.MyCash = gambler?.Cash ?? 0f;

            if (leagueGuid.HasValue)
            {
                model.BetLimit = GamblerDW.GetGamblerBetLimit(AcnID, leagueGuid.Value);
            }
            else
            {
                model.BetLimit = model.MyCash;
            }

            model.Match = MatchDto.Single(model.MatchGuid);

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