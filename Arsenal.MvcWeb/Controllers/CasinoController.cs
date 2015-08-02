using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;

using AutoMapper;

using Arsenal.MvcWeb.Models;
using Arsenal.MvcWeb.Models.Casino;
using Arsenal.Service.Casino;
using Arsenalcn.Core;

namespace Arsenal.MvcWeb.Controllers
{
    [Authorize]
    public class CasinoController : Controller
    {
        private readonly int acnID = UserDto.GetSession() != null ? UserDto.GetSession().AcnID.Value : 0;
        private IRepository repo = new Repository();

        // 可投注比赛
        // GET: /Casino
        [AllowAnonymous]
        public ActionResult Index()
        {
            var model = new IndexDto();
            var days = 7;

            var query = repo.Query<MatchView>(
                predicate: x => !x.ResultHome.HasValue && !x.ResultAway.HasValue
                && x.PlayTime > DateTime.Now && x.PlayTime < DateTime.Now.AddDays(days))
                .OrderBy(x => x.PlayTime)
                .Many<MatchView, ChoiceOption>((tOne, tMany) => tOne.CasinoItem.ID.Equals(tMany.CasinoItemGuid));

            MatchDto.CreateMap();

            var list = Mapper.Map<IEnumerable<MatchDto>>(source: query.AsEnumerable());

            model.Matches = list;
            model.CasinoValidDays = days;

            return View(model);
        }

        // 我的中奖查询
        // GET: /Casino/Bet

        public ActionResult MyBet(Criteria criteria)
        {
            var model = new MyBetDto();

            var whereBy = new Hashtable();
            whereBy.Add("UserID", this.acnID);

            var query = repo.Query<BetView>(whereBy)
                .Many<BetView, BetDetail>((tOne, tMany) => tOne.ID.Equals(tMany.BetID));

            BetDto.CreateMap();

            var list = Mapper.Map<IEnumerable<BetDto>>(source: query.AsEnumerable());

            model.Criteria = criteria;
            model.Search(list);

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

            var query = repo.All<MatchView>()
                .FindAll(x => x.ResultHome.HasValue && x.ResultAway.HasValue);

            var map = Mapper.CreateMap<MatchView, MatchDto>();

            map.ConstructUsing(s => new MatchDto
            {
                ID = s.ID,
                TeamHomeName = s.Home.TeamDisplayName,
                TeamHomeLogo = s.Home.TeamLogo,
                TeamAwayName = s.Away.TeamDisplayName,
                TeamAwayLogo = s.Away.TeamLogo,
            });

            var list = Mapper.Map<IEnumerable<MatchDto>>(source: query.AsEnumerable());

            model.Criteria = criteria;
            model.Search(list);

            return View(model);
        }

        // 中奖查询
        // GET: /Casino/Detail/5
        [AllowAnonymous]
        public ActionResult Detail(Guid id)
        {
            var model = new DetailDto();

            model.Match = MatchDto.Single(id);

            var query = repo.Query<BetView>(x => x.CasinoItem.MatchGuid.Equals(id))
                .Many<BetView, BetDetail>((tOne, tMany) => tOne.ID.Equals(tMany.BetID));

            BetDto.CreateMap();

            var list = Mapper.Map<IEnumerable<BetDto>>(source: query.AsEnumerable());

            model.Bets = list;

            return View(model);
        }

        // 我要投注
        // GET: /Casino/GameBet

        public ActionResult GameBet(Guid id)
        {
            var model = new GameBetDto();

            model.Match = MatchDto.Single(id);

            // model.MyBets
            var whereBy = new Hashtable();
            whereBy.Add("UserID", this.acnID);

            var betsQuery = repo.Query<BetView>(whereBy)
                .FindAll(x => x.CasinoItem.MatchGuid.Equals(id))
                .Many<BetView, BetDetail>((tOne, tMany) => tOne.ID.Equals(tMany.BetID));

            BetDto.CreateMap();

            var bList = Mapper.Map<IEnumerable<BetDto>>(source: betsQuery.AsEnumerable());

            model.MyBets = bList;

            // model.HistoryMatches
            var match = repo.Single<Arsenal.Service.Casino.Match>(id);

            var matchesQuery = repo.All<MatchView>()
                .FindAll(x => x.ResultHome.HasValue && x.ResultAway.HasValue &&
                (x.Home.ID.Equals(match.Home) && x.Away.ID.Equals(match.Away) ||
                x.Home.ID.Equals(match.Away) && x.Away.ID.Equals(match.Home)));

            var map = Mapper.CreateMap<MatchView, MatchDto>();

            map.ConstructUsing(s => new MatchDto
            {
                ID = s.ID,
                TeamHomeName = s.Home.TeamDisplayName,
                TeamHomeLogo = s.Home.TeamLogo,
                TeamAwayName = s.Away.TeamDisplayName,
                TeamAwayLogo = s.Away.TeamLogo,
            });

            var mlist = Mapper.Map<IEnumerable<MatchDto>>(source: matchesQuery.AsEnumerable());

            model.HistoryMatches = mlist;

            return View(model);
        }

        // 投输赢
        // GET: /Casino/SingleChoice/id

        public ActionResult SingleChoice(Guid id)
        {
            var model = new SingleChoiceDto();

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
                    //if (m.LeagueGuid.Equals(Arsenalcn.CasinoSys.Entity.ConfigGlobal.DefaultLeagueID))
                    //{
                    //    if (Gambler.GetGamblerTotalBetByUserID(this.acnID, m.LeagueGuid) < ConfigGlobal.TotalBetStandard)
                    //    {
                    //        float _alreadyMatchBet = Arsenalcn.CasinoSys.Entity.Bet.GetUserMatchTotalBet(this.acnID, id);

                    //        if (_alreadyMatchBet + ba > ConfigGlobal.SingleBetLimit)
                    //        { throw new Exception(string.Format("下半赛区博彩玩家单场投注不能超过{0}博彩币", ConfigGlobal.SingleBetLimit.ToString("f2"))); }
                    //    }
                    //}

                    var bet = new Bet();
                    bet.UserID = this.acnID;
                    bet.UserName = User.Identity.Name;
                    //bet.CasinoItemGuid = item.ID;
                    bet.BetAmount = model.BetAmount;

                    bet.Place(id, model.SelectedOption);

                    //投注成功

                    TempData["DataUrl"] = string.Format("data-url=/Casino/GameBet/{0}", id.ToString());
                    return RedirectToAction("GameBet", new { id = id });
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
            var model = new MatchResultDto();

            model.Match = MatchDto.Single(id);
            model.MatchGuid = id;

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

                    //    Guid? guid = CasinoItem.GetCasinoItemGuidByMatch(id, CasinoType.MatchResult);

                    //    if (guid.HasValue)
                    //    {
                    //        if (CasinoItem.GetCasinoItem(guid.Value).CloseTime < DateTime.Now)
                    //        {
                    //            ModelState.AddModelError("Warn", "已超出投注截止时间");
                    //        }
                    //        else if (Arsenalcn.CasinoSys.Entity.Bet.GetUserCasinoItemAllBet(this.acnID, guid.Value).Count > 0)
                    //        {
                    //            ModelState.AddModelError("Warn", "已经投过此注，不能重复猜比分");
                    //        }
                    //        else
                    //        {
                    //            Bet bet = new Bet();
                    //            bet.BetAmount = null;
                    //            bet.BetRate = null;
                    //            bet.CasinoItemGuid = guid.Value;
                    //            bet.UserID = this.acnID;
                    //            bet.UserName = User.Identity.Name;

                    //            MatchResultBetDetail matchResult = new MatchResultBetDetail();
                    //            matchResult.Home = model.ResultHome;
                    //            matchResult.Away = model.ResultAway;

                    //            bet.Insert(matchResult);


                    var bet = new Bet();
                    bet.UserID = this.acnID;
                    bet.UserName = User.Identity.Name;
                    //bet.CasinoItemGuid = item.ID;

                    bet.Place(id, model.ResultHome, model.ResultAway);

                    //投注成功

                    TempData["DataUrl"] = string.Format("data-url=/Casino/GameBet/{0}", id.ToString());
                    return RedirectToAction("GameBet", new { id = id });
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
    }
}
