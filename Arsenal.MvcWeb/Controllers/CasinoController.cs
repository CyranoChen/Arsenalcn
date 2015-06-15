using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Mvc;

using Arsenal.MvcWeb.Models;
using Arsenalcn.CasinoSys.Entity;

namespace Arsenal.MvcWeb.Controllers
{
    public class CasinoController : Controller
    {
        //
        // GET: /Casino/

        public ActionResult Index()
        {
            var list = new List<CasinoMatch>();
            var dtMatch = CasinoItem.GetMatchCasinoItemView(true);

            if (dtMatch != null)
            {
                foreach (DataRow dr in dtMatch.Rows)
                {
                    list.Add(new CasinoMatch(dr));
                }
            }

            return View(list);
        }

        //
        // GET: /Casino/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Casino/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Casino/Create

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Casino/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Casino/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Casino/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Casino/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
