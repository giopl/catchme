using CatchMe.Helpers;
using CatchMe.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace CatchMe.Controllers
{
    public class HomeController : Controller
    {
        private CatchMeDBEntities db = new CatchMeDBEntities();
        log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);




        private void CreateLog(log log)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    db.logs.Add(log);
                     db.SaveChanges();
                }
            }
            catch (Exception e)
            {                
                throw;
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


                UserSession.Current.HasInformational = db.information.Where(x => x.project_id == UserSession.Current.CurrentProjectId).Count() > 0;



                var myProjects = founduser.projects.ToList();

                //find browser
                var userAgent = HttpContext.Request.UserAgent;
                var userBrowser = new HttpBrowserCapabilities { Capabilities = new Hashtable { { string.Empty, userAgent } } };
                var factory = new BrowserCapabilitiesFactory();
                factory.ConfigureBrowserCapabilities(new NameValueCollection(), userBrowser);

                var Browser = string.Format("{0} {1}", userBrowser.Browser, userBrowser.Version);
                UserSession.Current.Browser = Browser;

                log alog = new log(AppEnums.LogOperationEnum.LOGIN, AppEnums.LogTypeEnum.USER, string.Format("{0} {1}", UserSession.Current.Username, UserSession.Current.Browser ));
                CreateLog(alog);

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