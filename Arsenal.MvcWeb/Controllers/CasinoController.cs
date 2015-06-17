using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Mvc;
using System.Linq;

using Arsenal.MvcWeb.Models;
using Arsenalcn.CasinoSys.Entity;

namespace Arsenal.MvcWeb.Controllers
{
    public class CasinoController : Controller
    {
        // 可投注比赛
        // GET: /Casino

        public ActionResult Index()
        {
            var list = new List<CasinoMatch>();
            var dt = CasinoItem.GetMatchCasinoItemView(true);

            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    list.Add(new CasinoMatch(dr));
                }
            }

            return View(list);
        }

        // 我的中奖查询
        // GET: /Casino/Bet

        public ActionResult Bet()
        {
            var list = new List<CasinoBet>();
            var dt = Arsenalcn.CasinoSys.Entity.Bet.GetUserBetHistoryView(443);

            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    list.Add(new CasinoBet(dr));
                }
            }

            return View(list);
        }

        // 我的盈亏情况
        // GET: /Casino/Bonus

        public ActionResult Bonus()
        {
            var list = new List<CasinoBet>();
            var dt = Arsenalcn.CasinoSys.Entity.Bet.GetUserBetHistoryView(443);

            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    list.Add(new CasinoBet(dr));
                }
            }

            return View(list);
        }

        // 比赛结果
        // GET: /Casino/Result

        public ActionResult Result()
        {
            var list = new List<CasinoMatch>();
            var dt = CasinoItem.GetEndViewByMatch();

            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    list.Add(new CasinoMatch(dr));
                }
            }

            return View(list.Skip(1).Take(20));
        }

        // 中奖查询
        // GET: /Casino/Detail/5

        public ActionResult Detail(Guid id)
        {
            var list = Arsenalcn.CasinoSys.Entity.Bet.GetMatchAllBet(id);

            return View(list);
        }

        // 我要投注
        // GET: /Casino/GameBet

        public ActionResult GameBet(Guid id)
        {
            CasinoMatch m = new CasinoMatch(id);

            return View(m);
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

        ////
        //// GET: /Casino/Edit/5

        //public ActionResult Edit(int id)
        //{
        //    return View();
        //}

        ////
        //// POST: /Casino/Edit/5

        //[HttpPost]
        //public ActionResult Edit(int id, FormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add update logic here

        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        ////
        //// GET: /Casino/Delete/5

        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        ////
        //// POST: /Casino/Delete/5

        //[HttpPost]
        //public ActionResult Delete(int id, FormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add delete logic here

        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}
    }
}
