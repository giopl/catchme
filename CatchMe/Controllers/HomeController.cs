using CatchMe.Helpers;
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
        
        
        public ActionResult Index()
        {
            log.Debug("Hello World");

            var _user = User.Identity.Name;

            if(!string.IsNullOrWhiteSpace(_user))
            {                
                _user = _user.Replace("MCB\\","");
            }
            
            var user = db.users.Where(x=>x.username == _user).ToList();

            if (user.Count > 0)
            {
                var founduser = user.FirstOrDefault();

                UserSession.Current.IsValid = true;
                UserSession.Current.Role = founduser.role;
                UserSession.Current.Username = founduser.username;
                UserSession.Current.Fullname = founduser.fullname;
                UserSession.Current.Firstname = founduser.firstname;
            }
            else {

                UserSession.Current.Firstname = _user;

            }

            //return View();
            return RedirectToAction("Index","Tasks");
        }

    }
}