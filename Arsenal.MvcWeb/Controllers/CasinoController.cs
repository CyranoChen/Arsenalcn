using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Mvc;
using System.Linq;

using Arsenal.MvcWeb.Models;
using Arsenal.MvcWeb.Models.Casino;
using Arsenalcn.CasinoSys.Entity;

namespace Arsenal.MvcWeb.Controllers
{
    public class CasinoController : Controller
    {
        // 可投注比赛
        // GET: /Casino

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
            var dt = Arsenalcn.CasinoSys.Entity.Bet.GetUserBetHistoryView(443);

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
            var dt = Arsenalcn.CasinoSys.Entity.Bet.GetUserBetHistoryView(443);

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
            var dtBet = Arsenalcn.CasinoSys.Entity.Bet.GetUserMatchAllBetTable(443, id);

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
        public ActionResult SingleChoice(Guid id, float betAmount)
        {
            try
            {
                // TODO: Add insert logic here

                var ba = betAmount;

                TempData["DataUrl"] = string.Format("data-url=/Casino/GameBet/{0}", id.ToString());
                return RedirectToAction("GameBet", new { id = id });
            }
            catch
            {
                var m = new MatchWithRateDto(id);

                return View(m);
            }
        }

        //
        // POST: /Casino/GameBet

        [HttpPost]
        public ActionResult GameBet(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("GameBet");
            }
            catch
            {
                return View();
            }
        }
    }
}
