using CatchMe.Helpers;
using CatchMe.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CatchMe.Controllers
{
    public class HomeController : BaseController
    {
        private CatchMeDBEntities db = new CatchMeDBEntities();
        log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        
        
        public ActionResult Index()
        {
            log.Debug("Hello World");

            var _user = User.Identity.Name;

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

                log Alog = new log { user_id = founduser.user_id, logtime = DateTime.Now, description = "Login", operation = "LOGIN", type = "USER" };

                CreateLog(Alog);


                UserSession.Current.Fullname = founduser.fullname;
                UserSession.Current.Firstname = founduser.firstname;
                UserSession.Current.CurrentProjectId = founduser.active_project.HasValue?founduser.active_project.Value:0;

                var myProjects = founduser.projects.ToList();

                

                UserSession.Current.MyProjects = myProjects;
                if(UserSession.Current.CurrentProjectId  > 0)
                {
                    UserSession.Current.CurrentProject = db.projects.Find(UserSession.Current.CurrentProjectId).name;
                    

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