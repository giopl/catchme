﻿using System;
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
    public class TasksController : BaseController
    {
        private CatchMeDBEntities db = new CatchMeDBEntities();

        public ActionResult Index()
        {
            return RedirectToAction("TaskList");
        }

        // GET: Tasks
        public ActionResult TaskList(int? id=null)
        {

            //find current user
            var user_id = UserSession.Current.UserId;
            var user = db.users.Find(user_id);

            //find user's active project
            var active_project = user.active_project.HasValue?  user.active_project.Value : 0;

            //find user's projects
            var myprojects = user.projects;


            // if no project found set user's active project by using the first one on the list
            if (!user.active_project.HasValue)
            {
                if (myprojects.Count > 0)
                {
                    active_project = myprojects.FirstOrDefault().project_id;
                    SetActiveProject(active_project);
                }
                    //if no  projects found for user redirect to page NoProject
                else 
                {

                    return RedirectToAction("NoProject");
                }
            }



            

            if(id.HasValue && id.Value != active_project)
            {
                SetActiveProject(id.Value);
                active_project = id.Value;
            }


            //return list of projects for user
            ViewBag.project_id = new SelectList(myprojects, "project_id", "name",active_project);


        
            // find list of tasks for active project
            var tasks = db.tasks.Include(t => t.project).Where(p => p.project_id == active_project).ToList();

            return View(tasks);
        }


        public ActionResult NoProject()
        {
            return View();
        }

        private void SetActiveProject(int projectId)
        {
            try
            {
                var user = db.users.Find(UserSession.Current.UserId);

                user.active_project = projectId;

                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();

                // change the user session accordingly
                var project = db.projects.Find(projectId);
                UserSession.Current.CurrentProject = project.name;
                UserSession.Current.CurrentProjectId = projectId;
                
            }
            catch (Exception)
            {
                
                throw;
            }
            
            
            


        }


        // GET: Tasks/Details/5
        public ActionResult TaskDetails(int? id)
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


        private List<OptionItem> getStatuses()
        {
            List<OptionItem> statuses = new List<OptionItem>();
            statuses.Add(new OptionItem { name = "None", value = 0 });
            statuses.Add(new OptionItem { name = "New", value = 1 });
            statuses.Add(new OptionItem { name = "Open", value = 2 });
            statuses.Add(new OptionItem { name = "In Progress", value = 3 });
            statuses.Add(new OptionItem { name = "Completed", value = 4 });
            statuses.Add(new OptionItem { name = "On Hold", value = 5 });
            statuses.Add(new OptionItem { name = "Cancelled", value = 6 });
            statuses.Add(new OptionItem { name = "Closed", value = 7 });
            statuses.Add(new OptionItem { name = "Problem", value = 8 });
            return statuses;
        }


        private List<OptionItem> getTestStatuses()        
        {

            List<OptionItem> teststatuses = new List<OptionItem>();
            teststatuses.Add(new OptionItem { name = "None", value = 0 });
            teststatuses.Add(new OptionItem { name = "Not Tested", value = 1 });
            teststatuses.Add(new OptionItem { name = "Ready to Test", value = 2 });
            teststatuses.Add(new OptionItem { name = "Re-Test", value = 3 });
            teststatuses.Add(new OptionItem { name = "Passed", value = 4 });
            teststatuses.Add(new OptionItem { name = "Failed", value = 5 });
            teststatuses.Add(new OptionItem { name = "Incomplete", value = 6 });
            teststatuses.Add(new OptionItem { name = "Cannot Test", value = 7 });
            return teststatuses;

        }

        private List<OptionItem> getComplexities()
        {

            List<OptionItem> complexities = new List<OptionItem>();
            complexities.Add(new OptionItem { name = "None", value = 0 });
            complexities.Add(new OptionItem { name = "Low", value = 1 });
            complexities.Add(new OptionItem { name = "Medium", value = 2 });
            complexities.Add(new OptionItem { name = "High", value = 3 });
            complexities.Add(new OptionItem { name = "Very High", value = 4 });
            return complexities;


        }


        private List<OptionItem> getTypes()
        {

            List<OptionItem> types = new List<OptionItem>();
            types.Add(new OptionItem { name = "None", value = 0 });
            types.Add(new OptionItem { name = "Development", value = 1 });
            types.Add(new OptionItem { name = "Change", value = 2 });
            types.Add(new OptionItem { name = "Bug", value = 3 });
            types.Add(new OptionItem { name = "Failure", value = 4 });
            types.Add(new OptionItem { name = "Test", value = 5 });
            types.Add(new OptionItem { name = "Investigation", value = 6 });
            return types;
        }



        private List<OptionItem> getSeverities()
        {


            List<OptionItem> severities = new List<OptionItem>();
            severities.Add(new OptionItem { name = "None", value = 0 });
            severities.Add(new OptionItem { name = "Low", value = 1 });
            severities.Add(new OptionItem { name = "Medium", value = 2 });
            severities.Add(new OptionItem { name = "High", value = 3 });
            severities.Add(new OptionItem { name = "Very High", value = 4 });

            return severities;
        }

        private List<OptionItem> getPriorities()
        {

            List<OptionItem> priorities = new List<OptionItem>();
            priorities.Add(new OptionItem { name = "None", value = 0 });
            priorities.Add(new OptionItem { name = "Low", value = 1 });
            priorities.Add(new OptionItem { name = "Medium", value = 2 });
            priorities.Add(new OptionItem { name = "High", value = 3 });
            priorities.Add(new OptionItem { name = "Immediate", value = 4 });

            return priorities;
        }





        // GET: Tasks/Create
        public ActionResult CreateTask()
        {
            ViewBag.project_id = new SelectList(db.projects, "project_id", "name");
            ViewBag.status = new SelectList(getStatuses(), "value", "name");
            ViewBag.test_status = new SelectList(getTestStatuses(), "value", "name");
            ViewBag.complexity = new SelectList(getComplexities(), "value", "name");
            ViewBag.type= new SelectList(getTypes(), "value", "name");
            ViewBag.severity = new SelectList(getSeverities(), "value", "name");
            ViewBag.priority = new SelectList(getPriorities(), "value", "name");



            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateComment([Bind(Include = "task_id,title,description")] comment comment)
        {
            try
            {
                comment.user_id = UserSession.Current.UserId;
                comment.created_on = DateTime.Now;

                if (ModelState.IsValid)
                {
                    db.comments.Add(comment);
                    db.SaveChanges();

                }

                return RedirectToAction("EditTask", new { id = comment.task_id });
                    
            }
            catch (Exception)
            {
                
                throw;
            }
        }


        // POST: Tasks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateTask([Bind(Include = "task_id,project_id,status,test_status,title,description,creator,complexity,priority,due_date")] task task)
        {
            try
            {

                task.created_by = UserSession.Current.UserId;
                task.created_on = DateTime.Now;

                if (ModelState.IsValid)
                {
                    db.tasks.Add(task);
                    db.SaveChanges();


                    comment comment = new comment
                    {
                        task_id = task.task_id,
                        user_id = task.created_by.Value,
                        title = task.title,
                        created_on = task.created_on,
                        description = task.description
                    };

                    db.comments.Add(comment);
                    db.SaveChanges();



                    return RedirectToAction("Index");
                }

                
                ViewBag.project_id = new SelectList(db.projects, "project_id", "name", task.project_id);
                return View(task);
            }
            catch (Exception e)
            {
                
                throw;
            }
            
        }

        // GET: Tasks/Edit/5
        public ActionResult EditTask(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            task task = db.tasks.Find(id);


            ViewBag.project_id = new SelectList(db.projects, "project_id", "name", task.project_id);
            ViewBag.status = new SelectList(getStatuses(), "value", "name", task.status );
            ViewBag.test_status = new SelectList(getTestStatuses(), "value", "name", task.test_status);
            ViewBag.complexity = new SelectList(getComplexities(), "value", "name", task.complexity);
            ViewBag.type = new SelectList(getTypes(), "value", "name", task.type);
            ViewBag.severity = new SelectList(getSeverities(), "value", "name", task.severity);
            ViewBag.priority = new SelectList(getPriorities(), "value", "name", task.priority);



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
        public ActionResult EditTask([Bind(Include = "task_id,project_id,status,test_status,title,description,initiator,complexity,priority,due_date,created_on,created_by")] task task)
        {
                

            var oldtask = db.tasks.AsNoTracking().Where(x=>x.task_id == task.task_id).FirstOrDefault();



            if (ModelState.IsValid)
            {
                db.Entry(task).State = EntityState.Modified;
                db.SaveChanges();

                taskHist hist = new taskHist
                {
                    
                    task_id = task.task_id,

                    project_id = task.project_id,
                    status = oldtask.status != task.status ? oldtask.status : null,
                    test_status = oldtask.test_status != task.test_status ? oldtask.test_status : null,
                    title = oldtask.title,
                    description = oldtask.description,
                    initiator = oldtask.initiator != task.initiator? oldtask.initiator : null,
                    complexity = oldtask.complexity != task.complexity ? oldtask.complexity : null,
                    priority = oldtask.priority != task.priority ? oldtask.priority : null,
                    due_date = oldtask.due_date != task.due_date ? oldtask.due_date : null,
                    created_on = DateTime.Now,
                    created_by = UserSession.Current.UserId
                    
                };
                db.taskHists.Add(hist);
                db.SaveChanges();

                return RedirectToAction("EditTask", new { id = task.task_id });
            }
            ViewBag.project_id = new SelectList(db.projects, "project_id", "name", task.project_id);
            ViewBag.status = new SelectList(getStatuses(), "value", "name", task.status);
            ViewBag.test_status = new SelectList(getTestStatuses(), "value", "name", task.test_status);
            ViewBag.complexity = new SelectList(getComplexities(), "value", "name", task.complexity);
            ViewBag.type = new SelectList(getTypes(), "value", "name", task.type);
            ViewBag.severity = new SelectList(getSeverities(), "value", "name", task.severity);
            ViewBag.priority = new SelectList(getPriorities(), "value", "name", task.priority);


            return View(task);
        }

        // GET: Tasks/Delete/5
        public ActionResult DeleteTask(int? id)
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
        [HttpPost, ActionName("DeleteTask")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteTaskConfirmed(int id)
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
