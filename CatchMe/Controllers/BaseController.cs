using CatchMe.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CatchMe.Controllers
{
    public class BaseController : Controller
    {
        // GET: Base
        private CatchMeDBEntities db = new CatchMeDBEntities();

        protected bool IsPostback
        {
            get { return Request.HttpMethod == "POST"; }
        }

        public void CreateLog(log log)
        {
            if (ModelState.IsValid)
            {
                db.logs.Add(log);
                db.SaveChanges();                
            }            
        }




        protected override void OnActionExecuting(ActionExecutingContext ctx)
        {
            base.OnActionExecuting(ctx);
            //check if session is valid
            if (!Helpers.UserSession.Current.IsValid)
            {
                //check if it is an ajax request
                if (!Request.IsAjaxRequest())
                {
                    ctx.Result = RedirectToAction("Index", "Home");
                }
                else
                {
                    ctx.RequestContext.HttpContext.Response.StatusCode = 401;
                    //to test if the ajax call is redirected to this contentresult
                    //if it does not work, redirect to home and check in JS if status code is 401
                    ctx.Result = RedirectToAction("AjaxSessionExpired", "Home");
                }
            }
        }

        /// <summary>
        /// allows to send content of a view to a string
        /// </summary>
        /// <param name="controllerContext"></param>
        /// <param name="viewName"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        protected static String RenderRazorViewToString(ControllerContext controllerContext, String viewName, Object model)
        {
            controllerContext.Controller.ViewData.Model = model;

            using (var sw = new StringWriter())
            {
                var ViewResult = ViewEngines.Engines.FindPartialView(controllerContext, viewName);
                var ViewContext = new ViewContext(controllerContext, ViewResult.View, controllerContext.Controller.ViewData, controllerContext.Controller.TempData, sw);
                ViewResult.View.Render(ViewContext, sw);
                ViewResult.ViewEngine.ReleaseView(controllerContext, ViewResult.View);
                return sw.GetStringBuilder().ToString();
            }
        }


        /// <summary>
    }
}