﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CatchMe.Models;
using System.Text;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using log4net;
using System.Data.Entity.Core;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using CatchMe.Models.ViewModel;
using CatchMe.Helpers;

namespace CatchMe.Controllers
{
    public class AdminController : BaseController
    {
        private CatchMeDBEntities db = new CatchMeDBEntities();
        log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        // GET: Projects
        public ActionResult Index()
        {
            return RedirectToAction("Home");
        }


        public ActionResult Home()
        {
            return View();
        }

        public ActionResult Restore(int id)
        {
            try
            {
                var task = db.tasks.Find(id);

                task.state = 0;

                db.Entry(task).State = EntityState.Modified;
                db.SaveChanges();


                log log = new log(AppEnums.LogOperationEnum.RECOVER, AppEnums.LogTypeEnum.TASK, string.Format(" Task recovered by user {0}", UserSession.Current.Username), id);
                CreateLog(log);


                


                return RedirectToAction("ListDeleted");
            }
            catch (Exception)
            {

                throw;
            }
        }



        public ActionResult ListProjects()
        {
            try
            {

            var users = db.users.AsNoTracking().ToList();

            ViewBag.Users = users;

            return View(db.projects.ToList());

            }
            catch (Exception)
            {

                throw;
            }

        }


        public ActionResult ListDeleted()
        {
            try
            {
                var deleted = db.tasks.Where(x => x.state == 1).ToList();
                return View(deleted);

            }
            catch (Exception)
            {
                throw;
            }
        }


        public ActionResult ListArchived()
        {
            try
            {
                var archived = db.tasks.Where(x => x.state == 2).ToList();
                return View(archived);

            }
            catch (Exception)
            {
                throw;
            }
        }

        public ActionResult ListUsers()
        {
            try
            {
            return View(db.users.ToList());

            }
            catch (Exception)
            {

                throw;
            }
        }

        


        public ActionResult AddUser(string user)
        {

            try
            {

                if (string.IsNullOrWhiteSpace(user))
                {

                    return RedirectToAction("FindUser");
                }

                employee emp = db.employees.Where(x => x.user_id == user).FirstOrDefault();

                //if ldap is on


                var founduser = new user
                {
                    firstname = emp.common_name,
                    lastname = emp.fam_name,
                    username = emp.user_id,
                    //email = string.Format("{0}@local.mcb", emp.user_id),
                    email = getUserCommonName(emp.user_id),
                    job_title = emp.position_title,
                    team = emp.team
                };




                ViewBag.role = new SelectList(getRoles(), "role_id", "name");

                ViewBag.project_id = new SelectList(db.projects, "project_id", "name");


                return View(founduser);
            }
            catch (Exception)
            {

                throw;
            }

        }

        private List<RoleVM> getRoles()
        {
            List<RoleVM> roles = new List<RoleVM>();
            roles.Add(new RoleVM(0, "Business"));
            roles.Add(new RoleVM(1, "User"));
            roles.Add(new RoleVM(2, "Admin"));
            return roles;

        }



        private string getUserCommonName(string user)
        {
            
                var email = string.Empty;

            try
            {
                using (var context = new PrincipalContext(ContextType.Domain))
                {
                    //log.Info(string.Concat("connected to server: ", context.ConnectedServer));
                    var principal = UserPrincipal.FindByIdentity(context, user);


                    var firstName = principal.GivenName;
                    var lastName = principal.Surname;
                     email = principal.EmailAddress;

                    return email;
                }
            }
            catch (Exception e)
            {
                log.Error("[getUserCommonName] - Exception Caught" + e.ToString());
                //throw;
                return email;
            }
            
        }

   
        // GET: Users/Edit/5
        public ActionResult EditUser(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            user user = db.users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            ViewBag.role = new SelectList(getRoles(), "role_id", "name", user.role);
            ViewBag.active_project = new SelectList(user.projects, "project_id", "name", user.active_project);

            var allprojects = db.projects.ToList();
            var userprojects = user.projects;
            var otherprojects = allprojects.Except(userprojects);

            ViewBag.NewProjects = otherprojects;
            
            ViewBag.project_id = new SelectList(otherprojects, "project_id", "name");

            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditUser([Bind(Include = "user_id,username,firstname,lastname,job_title,team,role,num_logins,is_active,email,active_project")] user user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(user).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                ViewBag.project_id = new SelectList(db.projects, "project_id", "name");

                return View(user);

            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw;
                //saveFailed = true;

                // Update the values of the entity that failed to save from the store 
                //ex.Entries.Single().Reload();
                //db.SaveChanges();
                
            } 

            catch (Exception)
            {
                
                throw;
            }
            


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


            var employee = db.employees.Where(x => x.user_id == addeduser.username).ToList() ;


            if (employee != null && employee.Count == 1 )
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
        public ActionResult CreateProject([Bind(Include = "project_id,name,description")] project project)
        {
            
            if (ModelState.IsValid)
            {
                project.is_active = true;
                db.projects.Add(project);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(project);
        }

        // GET: Projects/Edit/5
        public ActionResult EditProject(int? id, int tab=1)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            project project = db.projects.Find(id);


            //var pu=  project.project_user.ToList();

            ViewBag.Tab = tab;
                       
            var project_users = project.users;

            var user_id = db.users.ToList();


            //remove existing users from list
            user_id = user_id.Except(project_users).ToList();

            ViewBag.user_id = new SelectList(user_id, "user_id", "fullname");



            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }


        // POST: projectUsers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddUserToProject(int project_id, int user_id, bool fromuser = false)
        {

            var user = db.users.Find(user_id);
            var project = db.projects.Find(project_id);
            project.users.Add(user);
            if (ModelState.IsValid)
            {


                db.Entry(project).State = EntityState.Modified;
                db.SaveChanges();
                
                if(fromuser)
                {
                    return RedirectToAction("EditUser", new { id = user_id });

                }
                else
                {
                return RedirectToAction("EditProject", new { id = project_id, tab = 2 });

                }
                
            }

            ViewBag.project_id = new SelectList(db.projects, "project_id", "name",project_id);
            ViewBag.user_id = new SelectList(db.users, "user_id", "username", user_id);
            return View(project);
        }


        // POST: projectUsers/Delete/5
        

        public ActionResult RemoveUserFromProject(int project_id, int user_id, bool fromuser = false)
        {


            var user = db.users.Find(user_id);
            var project = db.projects.Find(project_id);
            project.users.Remove(user);
            if (ModelState.IsValid)
            {

                db.Entry(project).State = EntityState.Modified;
                db.SaveChanges();

                if(fromuser)
                {
                    return RedirectToAction("EditUser", new { id = user_id });

                } else
                {
                return RedirectToAction("EditProject", new { id = project_id, tab = 2 });
                }
            }

            ViewBag.project_id = new SelectList(db.projects, "project_id", "name", project_id);
            ViewBag.user_id = new SelectList(db.users, "user_id", "username", user_id);
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
                return RedirectToAction("ListProjects");
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
