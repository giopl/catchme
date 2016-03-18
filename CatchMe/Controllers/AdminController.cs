using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CatchMe.Models;
using System.Text;

namespace CatchMe.Controllers
{
    public class AdminController : Controller
    {
        private CatchMeDBEntities db = new CatchMeDBEntities();

        // GET: Projects
        public ActionResult Index()
        {

            var users = db.users.ToList();

            ViewBag.Users = users;

            return View(db.projects.ToList());


        }


        


        public ActionResult AddUser(string user)
        {


            if (string.IsNullOrWhiteSpace(user))
            {

                return RedirectToAction("FindUser");
            }

            employee emp = db.employees.Where(x=>x.user_id == user).FirstOrDefault();


            var founduser = new user
            {
                firstname = emp.common_name,
                lastname = emp.fam_name,
                username = emp.user_id,
                email = string.Format("{0}@local.mcb", emp.user_id),
                job_title = emp.position_title,
                team = emp.team
            };


            

            ViewBag.project_id = new SelectList(db.projects, "project_id", "name");


            return View(founduser);
        }


        public ActionResult FindUser()
        {

            return View();
        }


        [HttpPost]
        public ActionResult FindUser(user addeduser)
        {

            
            var isUserExist = db.users.Where(x => x.username == addeduser.username).FirstOrDefault();

            if (isUserExist != null)
            {
                ViewBag.Error = "User Exists";
                return View();
            }


            var employee =db.employees.Where(x => x.user_id == addeduser.username );
            if (employee != null )
            {

                return RedirectToAction("AddUser", new { user = addeduser.username });

            }

            ViewBag.Error = "User Not Found";
            return View();

        }



        //public ActionResult FindUser(string q)
        //{
        //    ContentResult result = new ContentResult();

        //    var users = db.employees.Where(x => x.user_id.StartsWith(q)).ToList();


        //    var userlist = new List<string>();


        //    StringBuilder sb = new StringBuilder();
        //    sb.Append("[");
        //    foreach (var u in users)
        //    {

        //        var fullname = string.Concat(u.user_id, "- ", u.common_name, " ", u.fam_name.Replace("'", ""));

        //        userlist.Add(fullname);
        //        sb.AppendFormat(@"'{0} - {1}',", u.user_id, fullname );

        //    }

        //    sb.Length--;
        //    sb.Append("]");

        //   // result.Content = @"['Amsterdam','Washington', 'Sydney', 'Beijing', 'Cairo']";



        //    //return Json(userlist, JsonRequestBehavior.AllowGet);
 
            
        //    result.Content = sb.ToString();

        //    return result;

        //}







        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddUser([Bind(Include = "username,firstname,lastname,job_title,team, role,email,active_project")] user user)
        {
            if (ModelState.IsValid)
            {
                user.is_active = true;
                db.users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.project_id = new SelectList(db.projects, "project_id", "name");
            return View(user);
        }


        







        // GET: Projects/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            project project = db.projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // GET: Projects/Create
        public ActionResult CreateProject()
        {
            return View();
        }

        // POST: Projects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateProject([Bind(Include = "project_id,name,description,is_active")] project project)
        {
            if (ModelState.IsValid)
            {
                db.projects.Add(project);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(project);
        }

        // GET: Projects/Edit/5
        public ActionResult EditProject(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            project project = db.projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditProject([Bind(Include = "project_id,name,description,is_active")] project project)
        {
            if (ModelState.IsValid)
            {
                db.Entry(project).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(project);
        }

        // GET: Projects/Delete/5
        public ActionResult DeleteProject(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            project project = db.projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("DeleteProject")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteProjectConfirmed(int id)
        {
            project project = db.projects.Find(id);
            db.projects.Remove(project);
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
