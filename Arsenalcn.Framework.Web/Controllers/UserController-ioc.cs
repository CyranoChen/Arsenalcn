using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

using Arsenalcn.Framework.Entity;
using Arsenalcn.Framework.DataAccess.CodeFirst;

namespace Arsenalcn.Framework.Web.Controllers
{
    public class UserController : Controller
    {
        private readonly IRepository repo;

        public UserController(IRepository repository)
        {
            repo = repository;
        }

        // GET: /User/
        public ActionResult Index()
        {
            return View(repo.All<User>().ToList());
        }

        // GET: /User/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            User user = repo.Single<User>(id);

            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: /User/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /User/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="ID,Name,DisplayName,Mobile,Email,KEY")] User user)
        {
            if (ModelState.IsValid)
            {
                repo.Add<User>(user);

                return RedirectToAction("Index");
            }

            return View(user);
        }

        // GET: /User/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            User user = repo.Single<User>(id);

            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: /User/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="ID,Name,DisplayName,Mobile,Email,KEY")] User user)
        {
            if (ModelState.IsValid)
            {
                repo.Update<User>(user);
            }

            return View(user);
        }

        // GET: /User/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            User user = repo.Single<User>(id);

            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: /User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            repo.Remove<User>(id);

            return RedirectToAction("Index");
        }
    }
}
