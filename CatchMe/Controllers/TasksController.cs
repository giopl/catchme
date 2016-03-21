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
    public class TasksController : Controller
    {
        private CatchMeDBEntities db = new CatchMeDBEntities();

        // GET: Tasks
        public ActionResult Index()
        {

            var user_id = UserSession.Current.UserId;

            var user = db.users.Find(user_id);

            var active_project = user.active_project.HasValue?  user.active_project.Value : 0;

            var myprojects = user.projects;

            if (!user.active_project.HasValue)
            {
                if (myprojects.Count > 0)
                {
                    active_project = myprojects.FirstOrDefault().project_id;
                    SetActiveProject(active_project);
                }
                else 
                {

                    return RedirectToAction("NoProject");
                }
            }


            var tasks = db.tasks.Include(t => t.project).Where(p=>p.project_id == active_project).ToList();
            return View(tasks);
        }


        public ActionResult NoProject()
        {
            return View();
        }

        private void SetActiveProject(int projectId)
        {
            var user = db.users.Find(UserSession.Current.UserId);

            user.active_project = projectId;
            
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                
            
            


        }


        // GET: Tasks/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            task task = db.tasks.Find(id);
            if (task == null)
            {
                return HttpNotFound();
            }
            return View(task);
        }

        // GET: Tasks/Create
        public ActionResult Create()
        {
            ViewBag.project_id = new SelectList(db.projects, "project_id", "name");
            return View();
        }

        // POST: Tasks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "task_id,project_id,status,test_status,title,description,creator,complexity,due_date")] task task)
        {
            if (ModelState.IsValid)
            {
                db.tasks.Add(task);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.project_id = new SelectList(db.projects, "project_id", "name", task.project_id);
            return View(task);
        }

        // GET: Tasks/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            task task = db.tasks.Find(id);
            if (task == null)
            {
                return HttpNotFound();
            }
            ViewBag.project_id = new SelectList(db.projects, "project_id", "name", task.project_id);
            return View(task);
        }

        // POST: Tasks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "task_id,project_id,status,test_status,title,description,creator,complexity,due_date")] task task)
        {
            if (ModelState.IsValid)
            {
                db.Entry(task).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.project_id = new SelectList(db.projects, "project_id", "name", task.project_id);
            return View(task);
        }

        // GET: Tasks/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            task task = db.tasks.Find(id);
            if (task == null)
            {
                return HttpNotFound();
            }
            return View(task);
        }

        // POST: Tasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            task task = db.tasks.Find(id);
            db.tasks.Remove(task);
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
