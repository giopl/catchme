﻿using CatchMe.Helpers;
using CatchMe.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CatchMe.Controllers
{
    public class HomeController : Controller
    {
        private CatchMeDBEntities db = new CatchMeDBEntities();
        log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);




        private void CreateLog(log log)
        {
            if (ModelState.IsValid)
            {
                      db.logs.Add(log);
                   // db.SaveChanges();
                //return RedirectToAction("Index");
            }

        }


        public ActionResult Index()
        {
            log.Debug("Hello World");

            string _user = string.Empty;
            if (string.IsNullOrWhiteSpace(UserSession.Current.ImpersonatedUser))
            { 
                 _user = User.Identity.Name;
            }
            else
            {
                _user = UserSession.Current.ImpersonatedUser;
            }
            
                        

            if(!string.IsNullOrWhiteSpace(_user))
            {                
                //_user = _user.Replace("MCB\\","");
                _user = _user.Substring(_user.IndexOf('\\') + 1);
            }
            
            var user = db.users.Where(x=>x.username == _user).ToList();

            if (user.Count > 0)
            {
                var founduser = user.FirstOrDefault();

                UserSession.Current.IsValid = true;
                UserSession.Current.Role = founduser.role;
                UserSession.Current.Username = founduser.username;
                UserSession.Current.UserId = founduser.user_id;

                

                UserSession.Current.Fullname = founduser.fullname;
                UserSession.Current.Firstname = founduser.firstname;
                UserSession.Current.CurrentProjectId = founduser.active_project.HasValue?founduser.active_project.Value:0;

                var myProjects = founduser.projects.ToList();

                

                UserSession.Current.MyProjects = myProjects;
                if(UserSession.Current.CurrentProjectId  > 0)
                {
                    var project = db.projects.Find(UserSession.Current.CurrentProjectId);
                    UserSession.Current.CurrentProject = project.name;



                    var roles = project.project_user_role.Where(x => x.user_id == UserSession.Current.UserId).ToList();
                    if(roles.Count() > 0)
                    {
                        UserSession.Current.CurrentProjectRole = roles.FirstOrDefault().role;
                    }


                    ViewBag.project_id = new SelectList(myProjects, "project_id", "name");
                    
                }
            }
            else {

                UserSession.Current.Firstname = _user;
                return RedirectToAction("NoAccess");
                
            }

            //return View();
            return RedirectToAction("Index","Tasks");
        }






        public ActionResult NoAccess()
        {
            return View();
        }
    }
}