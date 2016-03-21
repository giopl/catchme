using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CatchMe.Models;

namespace CatchMe.Controllers
{
    public class projectUsersController : Controller
    {
        private CatchMeDBEntities db = new CatchMeDBEntities();

        // GET: projectUsers
        public ActionResult Index()
        {
            var projectUsers = db.projectUsers.Include(p => p.project).Include(p => p.user);
            return View(projectUsers.ToList());
        }

        // GET: projectUsers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            projectUser projectUser = db.projectUsers.Find(id);
            if (projectUser == null)
            {
                return HttpNotFound();
            }
            return View(projectUser);
        }

        // GET: projectUsers/Create
        public ActionResult Create()
        {
            ViewBag.project_id = new SelectList(db.projects, "project_id", "name");
            ViewBag.user_id = new SelectList(db.users, "user_id", "username");
            return View();
        }

        // POST: projectUsers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "user_id,project_id,role")] projectUser projectUser)
        {
            if (ModelState.IsValid)
            {
                db.projectUsers.Add(projectUser);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.project_id = new SelectList(db.projects, "project_id", "name", projectUser.project_id);
            ViewBag.user_id = new SelectList(db.users, "user_id", "username", projectUser.user_id);
            return View(projectUser);
        }

        // GET: projectUsers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            projectUser projectUser = db.projectUsers.Find(id);
            if (projectUser == null)
            {
                return HttpNotFound();
            }
            ViewBag.project_id = new SelectList(db.projects, "project_id", "name", projectUser.project_id);
            ViewBag.user_id = new SelectList(db.users, "user_id", "username", projectUser.user_id);
            return View(projectUser);
        }

        // POST: projectUsers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "user_id,project_id,role")] projectUser projectUser)
        {
            if (ModelState.IsValid)
            {
                db.Entry(projectUser).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.project_id = new SelectList(db.projects, "project_id", "name", projectUser.project_id);
            ViewBag.user_id = new SelectList(db.users, "user_id", "username", projectUser.user_id);
            return View(projectUser);
        }

        // GET: projectUsers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            projectUser projectUser = db.projectUsers.Find(id);
            if (projectUser == null)
            {
                return HttpNotFound();
            }
            return View(projectUser);
        }

        // POST: projectUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            projectUser projectUser = db.projectUsers.Find(id);
            db.projectUsers.Remove(projectUser);
            db.SaveChanges();
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
