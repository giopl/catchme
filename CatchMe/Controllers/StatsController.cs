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
using System.Data.SqlClient;
using System.Text;

namespace CatchMe.Controllers
{
    public class StatsController : Controller
    {
        private CatchMeDBEntities db = new CatchMeDBEntities();

        // GET: Stats
        public ActionResult Index()
        {
            var proj = UserSession.Current.CurrentProjectId ;

            if (proj == 0)
                return RedirectToAction("Index", "Tasks");

            var figures = db.viewFigures.Where(p => p.project_id == proj);

            ViewBag.Figures = figures;

            var backlogs = GetBacklog(proj);

            var lastbacklogdate = backlogs.LastOrDefault().fulldate;
            StringBuilder sb = new StringBuilder();

            StringBuilder open  = new StringBuilder();

            StringBuilder closed = new StringBuilder();
              foreach(var l in backlogs)
                    {
                        sb.AppendFormat("'{0}'{1}", l.fulldate.ToString("dd-MM-yy"), (lastbacklogdate == l.fulldate ? "" : ","));
                        open.AppendFormat("{0}{1}", l.sum_opened, (lastbacklogdate == l.fulldate ? "" : ","));
                        closed.AppendFormat("{0}{1}", l.sum_closed, (lastbacklogdate == l.fulldate ? "" : ","));

                    }

              //var backloglabel = sb.ToString();
              ViewBag.BackLogLabel = sb.ToString();
              ViewBag.BackLogOpen = open.ToString();
              ViewBag.BackLogClosed = closed.ToString();


            ViewBag.Backlog = backlogs;


            ViewBag.OpenTasks = db.viewStatus.Where(x => x.project_id == proj ).ToList();

            return View(db.viewTasks.Where(x=>x.project_id == proj).ToList());
        }

        
        private IList<backlog> GetBacklog(int projectId)
        {
            try
            {

                IList<backlog> result = new List<backlog>();
                using (var ctx = new CatchMeDBEntities())
                {
                
                    var backlogs = db.Database.SqlQuery<backlog>(@"exec get_backlog @project_id"
                            , new object[]
                            {
                                 new SqlParameter("@project_id", projectId)
                            }
                            ).ToList<backlog>();

                    result = backlogs;
                }

                return result;

            }
            catch (Exception)
            {
                
                throw;
            }
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

