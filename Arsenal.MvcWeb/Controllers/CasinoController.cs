using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Mvc;
using System.Linq;

using Arsenal.MvcWeb.Models;
using Arsenal.MvcWeb.Models.Casino;
using Arsenalcn.CasinoSys.Entity;
using Arsenalcn.Core;

namespace Arsenal.MvcWeb.Controllers
{
    [Authorize]
    public class CasinoController : Controller
    {
        private readonly int acnID = UserDto.GetSession() != null ? UserDto.GetSession().AcnID.Value : 0;

        // 可投注比赛
        // GET: /Casino
        [AllowAnonymous]
        public ActionResult Index()
        {
            var model = new IndexDto();

            var list = new List<MatchWithRateDto>();
            var dt = CasinoItem.GetMatchCasinoItemView(true);

            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    list.Add(new MatchWithRateDto(dr));
                }
            }

            model.Matches = list;
            model.CasinoValidDays = 7;

            return View(model);
        }

        // 我的中奖查询
        // GET: /Casino/Bet

        public ActionResult MyBet(Criteria criteria)
        {
            var list = new List<BetDto>();
            var dt = Arsenalcn.CasinoSys.Entity.Bet.GetUserBetHistoryView(this.acnID);

            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    list.Add(new BetDto(dr));
                }
            }

            // Populate the view model
            var model = new MyBetDto();

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
            var list = new List<MatchDto>();
            var dt = CasinoItem.GetEndViewByMatch();

            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    list.Add(new MatchDto(dr));
                }
            }

            // Populate the view model
            var model = new ResultDto();

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

            var list = new List<BetDto>();
            var dt = Arsenalcn.CasinoSys.Entity.Bet.GetMatchAllBetTable(id);

            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    list.Add(new BetDto(dr));
                }
            }

            model.Bets = list;
            model.Match =  new MatchWithRateDto(id);

            return View(model);
        }

        // 我要投注
        // GET: /Casino/GameBet

        public ActionResult GameBet(Guid id)
        {
            var model = new GameBetDto();

            model.Match = new MatchWithRateDto(id);

            var bList = new List<BetDto>();
            var dtBet = Arsenalcn.CasinoSys.Entity.Bet.GetUserMatchAllBetTable(acnID, id);

            if (dtBet != null)
            {
                foreach (DataRow dr in dtBet.Rows)
                {
                    bList.Add(new BetDto(dr));
                }
            }

            model.MyBets = bList;

            var mlist = new List<MatchDto>();
            var dtMatch = CasinoItem.GetHistoryViewByMatch(id);

            if (dtMatch != null)
            {
                foreach (DataRow dr in dtMatch.Rows)
                {
                    mlist.Add(new MatchDto(dr));
                }
            }

            model.HistoryMatches = mlist;

            return View(model);
        }

        // 投输赢
        // GET: /Casino/SingleChoice/id

        public ActionResult SingleChoice(Guid id)
        {
            var model = new SingleChoiceDto();

            model.Match = new MatchWithRateDto(id);
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
                var id = model.MatchGuid;

                Guid? guid = CasinoItem.GetCasinoItemGuidByMatch(id, CasinoType.SingleChoice);

                if (guid.HasValue)
                {
                    if (CasinoItem.GetCasinoItem(guid.Value).CloseTime < DateTime.Now)
                    {
                        ModelState.AddModelError("Warn", "已超出投注截止时间");
                    }
                    else
                    {

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

                        //get selected option
                        SingleChoice item = (SingleChoice)CasinoItem.GetCasinoItem(guid.Value);
                        ChoiceOption seletedOption = item.Options.Find(x =>
                            x.OptionValue.Equals(model.SelectedOption, StringComparison.OrdinalIgnoreCase));

                        Arsenalcn.CasinoSys.Entity.Bet bet = new Arsenalcn.CasinoSys.Entity.Bet();
                        bet.BetAmount = model.BetAmount;
                        bet.BetRate = seletedOption.OptionRate;
                        bet.CasinoItemGuid = guid.Value;
                        bet.UserID = this.acnID;
                        bet.UserName = User.Identity.Name;

                        bet.Insert(seletedOption.OptionValue);

                        //投注成功
                    }
                }

                TempData["DataUrl"] = string.Format("data-url=/Casino/GameBet/{0}", id.ToString());
                return RedirectToAction("GameBet", new { id = id });
            }
            else
            {
                model.Match = new MatchWithRateDto(model.MatchGuid);

                return View(model);
            }
        }

        // 猜比分
        // GET: /Casino/MatchResult/id

        public ActionResult MatchResult(Guid id)
        {
            var model = new MatchResultDto();

            model.Match = new MatchWithRateDto(id);
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
                var id = model.MatchGuid;
                Guid? guid = CasinoItem.GetCasinoItemGuidByMatch(id, CasinoType.MatchResult);

                if (guid.HasValue)
                {
                    if (CasinoItem.GetCasinoItem(guid.Value).CloseTime < DateTime.Now)
                    {
                        ModelState.AddModelError("Warn", "已超出投注截止时间");
                    }
                    else if (Arsenalcn.CasinoSys.Entity.Bet.GetUserCasinoItemAllBet(this.acnID, guid.Value).Count > 0)
                    {
                        ModelState.AddModelError("Warn", "已经投过此注，不能重复猜比分");
                    }
                    else
                    {
                        Bet bet = new Bet();
                        bet.BetAmount = null;
                        bet.BetRate = null;
                        bet.CasinoItemGuid = guid.Value;
                        bet.UserID = this.acnID;
                        bet.UserName = User.Identity.Name;

                        MatchResultBetDetail matchResult = new MatchResultBetDetail();
                        matchResult.Home = model.ResultHome;
                        matchResult.Away = model.ResultAway;

                        bet.Insert(matchResult);

                        //投注成功

                        TempData["DataUrl"] = string.Format("data-url=/Casino/GameBet/{0}", id.ToString());
                        return RedirectToAction("GameBet", new { id = id });
                    }
                }
            }
            else
            {
                ModelState.AddModelError("Warn", "请正确填写比赛结果比分");
            }

            model.Match = new MatchWithRateDto(model.MatchGuid);

            return View(model);
        }
    }
}
