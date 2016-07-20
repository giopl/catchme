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
    public class logsController : Controller
    {
        private CatchMeDBEntities db = new CatchMeDBEntities();

        // GET: logs
        public ActionResult Index()
        {
            var logs = db.logs.Include(l => l.user).Include(l => l.task);
            return View(logs.ToList());
        }

        // GET: logs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            log log = db.logs.Find(id);
            if (log == null)
            {
                return HttpNotFound();
            }
            return View(log);
        }

        // GET: logs/Create
        public ActionResult Create()
        {
            ViewBag.user_id = new SelectList(db.users, "user_id", "username");
            ViewBag.task_id = new SelectList(db.tasks, "task_id", "title");
            return View();
        }

        // POST: logs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "log_id,user_id,task_id,operation,type,logtime,description")] log log)
        {
            if (ModelState.IsValid)
            {
                db.logs.Add(log);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.user_id = new SelectList(db.users, "user_id", "username", log.user_id);
            ViewBag.task_id = new SelectList(db.tasks, "task_id", "title", log.task_id);
            return View(log);
        }

        // GET: logs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            log log = db.logs.Find(id);
            if (log == null)
            {
                return HttpNotFound();
            }
            ViewBag.user_id = new SelectList(db.users, "user_id", "username", log.user_id);
            ViewBag.task_id = new SelectList(db.tasks, "task_id", "title", log.task_id);
            return View(log);
        }

        // POST: logs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "log_id,user_id,task_id,operation,type,logtime,description")] log log)
        {
            if (ModelState.IsValid)
            {
                db.Entry(log).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.user_id = new SelectList(db.users, "user_id", "username", log.user_id);
            ViewBag.task_id = new SelectList(db.tasks, "task_id", "title", log.task_id);
            return View(log);
        }

        // GET: logs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            log log = db.logs.Find(id);
            if (log == null)
            {
                return HttpNotFound();
            }
            return View(log);
        }

        // POST: logs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            log log = db.logs.Find(id);
            db.logs.Remove(log);
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
