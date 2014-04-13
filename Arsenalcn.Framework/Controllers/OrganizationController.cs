using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Arsenalcn.Framework.Models;
using Arsenalcn.Framework.DataAccess;

namespace Arsenalcn.Framework.Controllers
{
    public class OrganizationController : Controller
    {
        private FrameworkContext db = new FrameworkContext();

        // GET: /Organization/
        public ActionResult Index(string order, string where)
        {
            order = order ?? string.Empty;
            ViewBag.ParaSortName = order.Equals("Name") ? "Name_DESC" : "Name";
            ViewBag.ParaSortCreateTime = order.Equals("Date") ? "Date_DESC" : "Date";

            var organizations = db.Organizations.Select(o => o);

            if (!string.IsNullOrEmpty(where))
            {
                organizations = organizations.Where(o => o.Name.ToLower().Contains(where.Trim().ToLower()));
            }

            switch (order)
            {
                case "Name":
                    organizations = organizations.OrderBy(o => o.Name);
                    break;
                case "Name_DESC":
                    organizations = organizations.OrderByDescending(o => o.Name);
                    break;
                case "Date":
                    organizations = organizations.OrderBy(o => o.CreateTime);
                    break;
                case "Date_DESC":
                    organizations = organizations.OrderByDescending(s => s.CreateTime);
                    break;
                default:
                    organizations = organizations.OrderByDescending(o => o.ID);
                    break;
            }

            return View(organizations.ToList());
        }

        // GET: /Organization/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Organization organization = db.Organizations.Find(id);
            if (organization == null)
            {
                return HttpNotFound();
            }
            return View(organization);
        }

        // GET: /Organization/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Organization/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name")] Organization organization)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    organization.CreateTime = DateTime.Now;

                    db.Organizations.Add(organization);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (DataException /* dex */)
            {
                //Log the error (uncomment dex variable name after DataException and add a line here to write a log.
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }

            return View(organization);
        }

        // GET: /Organization/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Organization organization = db.Organizations.Find(id);
            if (organization == null)
            {
                return HttpNotFound();
            }
            return View(organization);
        }

        // POST: /Organization/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name")] Organization organization)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(organization).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (DataException /* dex */)
            {
                //Log the error (uncomment dex variable name after DataException and add a line here to write a log.
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            return View(organization);
        }

        // GET: /Organization/Delete/5
        public ActionResult Delete(int? id, bool? saveChangesError = false)
        {
            if (saveChangesError.GetValueOrDefault())
            {
                ViewBag.ErrorMessage = "Delete failed. Try again, and if the problem persists see your system administrator.";
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Organization organization = db.Organizations.Find(id);
            if (organization == null)
            {
                return HttpNotFound();
            }
            return View(organization);
        }

        // POST: /Organization/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            try
            {
                Organization organizationToDelete = new Organization() { ID = id };
                db.Entry(organizationToDelete).State = EntityState.Deleted;
                //Organization organization = db.Organizations.Find(id);
                //db.Organizations.Remove(organization);
                db.SaveChanges();
            }
            catch (DataException/* dex */)
            {
                // uncomment dex and log error. 
                return RedirectToAction("Delete", new { id = id, saveChangesError = true });
            }

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
