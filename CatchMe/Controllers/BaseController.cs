using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CatchMe.Controllers
{
    public class BaseController : Controller
    {
        // GET: Base
        

        protected bool IsPostback
        {
            get { return Request.HttpMethod == "POST"; }
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
    }
}