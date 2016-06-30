using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CatchMe.Models;
using CatchMe.Helpers;

namespace CatchMe.Controllers
{
    public class StatsController : Controller
    {
        private CatchMeDBEntities db = new CatchMeDBEntities();

        // GET: Stats
        public ActionResult Index()
        {
            var proj = UserSession.Current.CurrentProjectId ;
            return View(db.viewTasks.Where(x=>x.project_id == proj).ToList());
        }

        // GET: Stats/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            viewTasks viewTasks = db.viewTasks.Find(id);
            if (viewTasks == null)
            {
                return HttpNotFound();
            }
            return View(viewTasks);
        }

        // GET: Stats/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Stats/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "project_id,task_id,initiator,complexity_code,complexity,type_code,type,status_code,status,priority_code,priority,created_by,creator_fn,creator_ln,created_on,assigned_to,assignee_fn,assignee_ln,updated_on,due_date")] viewTasks viewTasks)
        {
            if (ModelState.IsValid)
            {
                db.viewTasks.Add(viewTasks);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(viewTasks);
        }

        // GET: Stats/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            viewTasks viewTasks = db.viewTasks.Find(id);
            if (viewTasks == null)
            {
                return HttpNotFound();
            }
            return View(viewTasks);
        }

        // POST: Stats/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "project_id,task_id,initiator,complexity_code,complexity,type_code,type,status_code,status,priority_code,priority,created_by,creator_fn,creator_ln,created_on,assigned_to,assignee_fn,assignee_ln,updated_on,due_date")] viewTasks viewTasks)
        {
            if (ModelState.IsValid)
            {
                db.Entry(viewTasks).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(viewTasks);
        }

        // GET: Stats/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            viewTasks viewTasks = db.viewTasks.Find(id);
            if (viewTasks == null)
            {
                return HttpNotFound();
            }
            return View(viewTasks);
        }

        // POST: Stats/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            viewTasks viewTasks = db.viewTasks.Find(id);
            db.viewTasks.Remove(viewTasks);
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
