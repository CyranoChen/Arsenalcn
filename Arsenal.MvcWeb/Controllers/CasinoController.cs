﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Mvc;
using System.Linq;

using Arsenal.MvcWeb.Models;
using Arsenal.MvcWeb.Models.Casino;
using Arsenalcn.CasinoSys.Entity;

namespace Arsenal.MvcWeb.Controllers
{
    [Authorize]
    public class CasinoController : Controller
    {
        private readonly int acnID = MembershipDto.GetSession() != null ? MembershipDto.GetSession().AcnID.Value : 0;

        // 可投注比赛
        // GET: /Casino
        [AllowAnonymous]
        public ActionResult Index()
        {
            var list = new List<MatchWithRateDto>();
            var dt = CasinoItem.GetMatchCasinoItemView(true);

            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    list.Add(new MatchWithRateDto(dr));
                }
            }

            ViewBag.CasinoValidDays = 7;

            return View(list);
        }

        // 我的中奖查询
        // GET: /Casino/Bet

        public ActionResult Bet()
        {
            var list = new List<BetDto>();
            var dt = Arsenalcn.CasinoSys.Entity.Bet.GetUserBetHistoryView(acnID);

            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    list.Add(new BetDto(dr));
                }
            }

            return View(list);
        }

        // 我的盈亏情况
        // GET: /Casino/Bonus

        public ActionResult Bonus()
        {
            var list = new List<BetDto>();
            var dt = Arsenalcn.CasinoSys.Entity.Bet.GetUserBetHistoryView(acnID);

            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    list.Add(new BetDto(dr));
                }
            }

            return View(list);
        }

        // 比赛结果
        // GET: /Casino/Result
        [AllowAnonymous]
        public ActionResult Result()
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

            // HARDCODE
            return View(list.Take(20));
        }

        // 中奖查询
        // GET: /Casino/Detail/5
        [AllowAnonymous]
        public ActionResult Detail(Guid id)
        {
            var list = new List<BetDto>();
            var dt = Arsenalcn.CasinoSys.Entity.Bet.GetMatchAllBetTable(id);

            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    list.Add(new BetDto(dr));
                }
            }

            ViewBag.MatchDto = new MatchWithRateDto(id);

            return View(list);
        }

        // 我要投注
        // GET: /Casino/GameBet

        public ActionResult GameBet(Guid id)
        {
            var m = new MatchWithRateDto(id);

            var bList = new List<BetDto>();
            var dtBet = Arsenalcn.CasinoSys.Entity.Bet.GetUserMatchAllBetTable(acnID, id);

            if (dtBet != null)
            {
                foreach (DataRow dr in dtBet.Rows)
                {
                    bList.Add(new BetDto(dr));
                }
            }

            ViewBag.BetList = bList;

            var mlist = new List<MatchDto>();
            var dtMatch = CasinoItem.GetHistoryViewByMatch(id);

            if (dtMatch != null)
            {
                foreach (DataRow dr in dtMatch.Rows)
                {
                    mlist.Add(new MatchDto(dr));
                }
            }

            ViewBag.MatchDtoList = mlist;

            //if (bList.Count > 0)
            //{
            //    ViewBag.IsMyBetCollapsed = "data-collapsed=\"false\"";
            //}
            //else if (mlist.Count > 0)
            //{
            //    ViewBag.IsHistoryCollapsed = "data-collapsed=\"false\"";
            //}

            return View(m);
        }

        // 投输赢
        // GET: /Casino/SingleChoice/id

        public ActionResult SingleChoice(Guid id)
        {
            var m = new MatchWithRateDto(id);

            return View(m);
        }

        // 投输赢
        // POST: /Casino/SingleChoice

        [HttpPost]
        public ActionResult SingleChoice(Guid id, string option, float betAmount)
        {
            try
            {
                var op = option;
                float ba = betAmount;

                Guid? guid = CasinoItem.GetCasinoItemGuidByMatch(id, CasinoItem.CasinoType.SingleChoice);

                if (guid.HasValue)
                {
                    if (CasinoItem.GetCasinoItem(guid.Value).CloseTime < DateTime.Now)
                    { throw new Exception("已超出投注截止时间"); }

                    //Gambler in Lower could not bet above the SingleBetLimit of DefaultLeague (Contest)
                    Match m = new Match(id);

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
                    ChoiceOption seletedOption = item.Options.Find(x => x.OptionValue.Equals(op, StringComparison.OrdinalIgnoreCase));

                    Arsenalcn.CasinoSys.Entity.Bet bet = new Arsenalcn.CasinoSys.Entity.Bet();
                    bet.BetAmount = ba;
                    bet.BetRate = seletedOption.OptionRate;
                    bet.CasinoItemGuid = guid.Value;
                    bet.UserID = this.acnID;
                    bet.UserName = User.Identity.Name;

                    bet.Insert(seletedOption.OptionValue);

                    //投注成功
                }

                TempData["DataUrl"] = string.Format("data-url=/Casino/GameBet/{0}", id.ToString());
                return RedirectToAction("GameBet", new { id = id });
            }
            catch
            {
                var m = new MatchWithRateDto(id);

                return View(m);
            }
        }
    }
}
