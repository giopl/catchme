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
    public class informationController : Controller
    {
        private CatchMeDBEntities db = new CatchMeDBEntities();

        // GET: information
        public ActionResult Index()
        {
            var information = db.information.Include(i => i.project).Include(i => i.user).Include(i => i.user1);
            return View(information.ToList());
        }

        // GET: information/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            information information = db.information.Find(id);
            if (information == null)
            {
                return HttpNotFound();
            }
            return View(information);
        }

        // GET: information/Create
        public ActionResult Create()
        {
            ViewBag.project_id = new SelectList(db.projects, "project_id", "name");
            ViewBag.updated_by = new SelectList(db.users, "user_id", "username");
            ViewBag.created_by = new SelectList(db.users, "user_id", "username");
            return View();
        }

        // POST: information/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "information_id,project_id,title,description,importance,created_by,created_on,updated_by,updated_on,state")] information information)
        {
            if (ModelState.IsValid)
            {
                db.information.Add(information);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.project_id = new SelectList(db.projects, "project_id", "name", information.project_id);
            ViewBag.updated_by = new SelectList(db.users, "user_id", "username", information.updated_by);
            ViewBag.created_by = new SelectList(db.users, "user_id", "username", information.created_by);
            return View(information);
        }

        // GET: information/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            information information = db.information.Find(id);
            if (information == null)
            {
                return HttpNotFound();
            }
            ViewBag.project_id = new SelectList(db.projects, "project_id", "name", information.project_id);
            ViewBag.updated_by = new SelectList(db.users, "user_id", "username", information.updated_by);
            ViewBag.created_by = new SelectList(db.users, "user_id", "username", information.created_by);
            return View(information);
        }

        // POST: information/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "information_id,project_id,title,description,importance,created_by,created_on,updated_by,updated_on,state")] information information)
        {
            if (ModelState.IsValid)
            {
                db.Entry(information).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.project_id = new SelectList(db.projects, "project_id", "name", information.project_id);
            ViewBag.updated_by = new SelectList(db.users, "user_id", "username", information.updated_by);
            ViewBag.created_by = new SelectList(db.users, "user_id", "username", information.created_by);
            return View(information);
        }

        // GET: information/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            information information = db.information.Find(id);
            if (information == null)
            {
                return HttpNotFound();
            }
            return View(information);
        }

        // POST: information/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            information information = db.information.Find(id);
            db.information.Remove(information);
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
