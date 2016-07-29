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
using System.Net.Mail;
using System.Text;
using System.Data.Entity.Validation;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;

namespace CatchMe.Controllers
{
    public class TasksController : BaseController
    {
        log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
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
            if(user_id == 0)
            {
               return    RedirectToAction("Index", "Home");
            }

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
            var tasks = db.tasks.Include(t => t.project).Where(p => p.project_id == active_project && p.state == 0).ToList();

            return View(tasks);
        }


        public ActionResult NoProject()
        {
            return View();
        }



        [HttpPost]
        public ActionResult UploadFile(HttpPostedFileBase file, int taskid, int projectid)
        {
            try
            {

                attachment attachment = new attachment
                {
                    filename = file.FileName,
                    content_length = file.ContentLength,
                    mimetype = file.ContentType,
                    created_on = DateTime.Now,
                    task_id = taskid,
                    user_id = UserSession.Current.UserId,
                    filepath = string.Format("{0}/{1}/{2}", projectid, taskid, file.FileName)

                };

                if(SaveResourceToDisk(file,taskid))
                {

                    try
                    {
                    db.attachments.Add(attachment);
                    db.SaveChanges();

                        log log = new log(AppEnums.LogOperationEnum.CREATE, AppEnums.LogTypeEnum.ATTACHMENT, string.Format(" Attachment {0} - added by user {1}", file.FileName, UserSession.Current.Username), taskid);
                        CreateLog(log);



                    }
                    catch (DbEntityValidationException ex)
                    {
                        // Retrieve the error messages as a list of strings.
                        var errorMessages = ex.EntityValidationErrors
                                .SelectMany(x => x.ValidationErrors)
                                .Select(x => x.ErrorMessage);

                        // Join the list to a single string.
                        var fullErrorMessage = string.Join("; ", errorMessages);

                        // Combine the original exception message with the new one.
                        var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

                        // Throw a new DbEntityValidationException with the improved exception message.
                        throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
                    }
                

            }

                
                return RedirectToAction("EditTask", new { id = taskid });
            }
            catch (Exception e)
            {

                throw;
            }
        }
        public ActionResult DeleteAttachment(int id, int taskid)
        {
            try
            {
                if(DeleteResourceFromDisk(id))
                {
                    var file = db.attachments.Find(id);
                    var filename = file.filename;
                    db.attachments.Remove(file);

                    db.SaveChanges();


                    log log = new log(AppEnums.LogOperationEnum.DELETE, AppEnums.LogTypeEnum.ATTACHMENT, string.Format(" Attachment {0} - deleted by user {1}",  filename, UserSession.Current.Username), id);
                    CreateLog(log);


                }
                return RedirectToAction("EditTask", new { id = taskid });
            }
            catch (Exception e)
            {

                throw;
            }

        }

        private bool DeleteResourceFromDisk(int id)
        {
            try
            {
                var file = db.attachments.Find(id);

                string dirPath = System.Web.HttpContext.Current.Server.MapPath("~") + "uploads/" + file.filepath;

                bool exists = System.IO.File.Exists(dirPath);

                if (exists)
                    System.IO.File.Delete(dirPath);

                return true; 
            }

            catch (Exception)
            {

                return false;
            }
        }

        private bool SaveResourceToDisk(HttpPostedFileBase mainFile, int taskid)
        {
            try
            {
                log.Info("Saving resource to disk");
                bool saved = false;

                string imageExt = string.Empty;
                string imageName = string.Empty;

                bool hasResourceFile = mainFile != null && mainFile.ContentLength > 0;

                if (hasResourceFile)
                {

                    log.Info("Saving resource to disk - Resource file found");
                    var projectid = db.tasks.Find(taskid).project_id;
                    var serverpathprod = Helpers.ConfigurationHelper.GetServerPathProd();
                    var IsProd = Helpers.ConfigurationHelper.IsProd();


                    var serverpath = System.IO.Path.GetFullPath("/");
                    string id = Guid.NewGuid().ToString();

                

                    imageExt = System.IO.Path.GetExtension(mainFile.FileName);
                    imageName = System.IO.Path.GetFileName(mainFile.FileName);



                    string dirPath = System.Web.HttpContext.Current.Server.MapPath("~") + "/uploads/" + string.Format("{0}/",projectid) + string.Format("{0}/", taskid) ;

                    if (IsProd)
                    {
                      //  dirPath = System.Web.HttpContext.Current.Server.MapPath(serverpathprod) + "/uploads/" + string.Format("{0}/", projectid) + string.Format("{0}/", taskid);
                    }

                    
                    log.DebugFormat("dirpath: {0}", dirPath);

                    //bool exists = System.IO.Directory.Exists(Server.MapPath(dirPath));
                    bool exists = System.IO.Directory.Exists(dirPath);

                    if (!exists)
                        System.IO.Directory.CreateDirectory(dirPath);

                    string path = System.IO.Path.Combine(dirPath, imageName);

                    bool isValidItem = Helpers.ConfigurationHelper.AuthorizedImagesExt().Contains(imageExt.ToLower().Replace(".",""));
                    int sizeKb = Convert.ToInt32(Math.Round(Convert.ToDecimal(mainFile.ContentLength / 1024), 0));

                    int maxAllowedSize = Helpers.ConfigurationHelper.MaxUploadSize();

                    
                    //int maxAllowedSize = 0;
                    

                    if (isValidItem && (sizeKb <= maxAllowedSize))
                    {
                        if(System.IO.Directory.Exists(path))
                        {
                            saved = false;
                        } else
                        {
                            saved = true;
                        }

                        mainFile.SaveAs(path);
                        //saved = true;
                    
                    }
                }
                return saved;
            }
            catch (Exception e)
            {
                log.ErrorFormat("Error saving resource to disk {0}", e.ToString());
                return false;
    
            }
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


        private List<OptionItem> getStatuses(int val)
        {
            
            List<OptionItem> statuses = new List<OptionItem>();

            if (val == 0)
            {
                statuses.Add(new OptionItem { name = "New", value = 0 });
                statuses.Add(new OptionItem { name = "Action", value = 1 });
                statuses.Add(new OptionItem { name = "Investigation", value = 2 });
            }

            if (val == 1)
            {
                statuses.Add(new OptionItem { name = "New", value = 0 });
                statuses.Add(new OptionItem { name = "Action", value = 1 });
                statuses.Add(new OptionItem { name = "Completed", value = 3 });
                statuses.Add(new OptionItem { name = "On Hold", value = 4 });
                statuses.Add(new OptionItem { name = "Problem", value = 5 });
                statuses.Add(new OptionItem { name = "No Issue", value = 6 });
            }


            if (val == 2)
            {
                statuses.Add(new OptionItem { name = "New", value = 0 });
                statuses.Add(new OptionItem { name = "Action", value = 1 });
                statuses.Add(new OptionItem { name = "Investigation", value = 2 });
                statuses.Add(new OptionItem { name = "Completed", value = 3 });
                statuses.Add(new OptionItem { name = "On Hold", value = 4 });
                statuses.Add(new OptionItem { name = "Problem", value = 5 });
                statuses.Add(new OptionItem { name = "No Issue", value = 6 });
            }


            if (val == 3)            
            {
                statuses.Add(new OptionItem { name = "Action", value = 1 });
                statuses.Add(new OptionItem { name = "Completed", value = 3 });
                statuses.Add(new OptionItem { name = "Passed", value = 7 });
                statuses.Add(new OptionItem { name = "Failed", value = 8 });
            }

            if (val == 4)
            {
                statuses.Add(new OptionItem { name = "Action", value = 1 });
                statuses.Add(new OptionItem { name = "On Hold", value = 4 });
            }

            if (val == 5)
            {
                statuses.Add(new OptionItem { name = "Action", value = 1 });
                statuses.Add(new OptionItem { name = "Problem", value = 5 });
            }

            if (val == 6)
            {
                statuses.Add(new OptionItem { name = "Action", value = 1 });
                statuses.Add(new OptionItem { name = "No Issue", value = 6 });
                statuses.Add(new OptionItem { name = "Closed", value = 9 });
            }

            if (val == 7)
            {
                statuses.Add(new OptionItem { name = "Action", value = 1 });
                statuses.Add(new OptionItem { name = "Passed", value = 7 });
                statuses.Add(new OptionItem { name = "Closed", value = 9 });
            }

            if (val == 8)
            {
                statuses.Add(new OptionItem { name = "Action", value = 1 });
                statuses.Add(new OptionItem { name = "Failed", value = 8 });
            }

            if (val == 9)
            {
                statuses.Add(new OptionItem { name = "Action", value = 1 });
                statuses.Add(new OptionItem { name = "Closed", value = 9 });
            }
            
            

            //statuses.Add(new OptionItem { name = "New", value = 0 });
            //statuses.Add(new OptionItem { name = "Action", value = 1 });
            //statuses.Add(new OptionItem { name = "Investigation", value = 2 });
            //statuses.Add(new OptionItem { name = "Completed", value = 3 });
            //statuses.Add(new OptionItem { name = "On Hold", value = 4 });
            //statuses.Add(new OptionItem { name = "Problem", value = 5 });
            //statuses.Add(new OptionItem { name = "No Issue", value = 6 });
            //statuses.Add(new OptionItem { name = "Passed", value = 7 });
            //statuses.Add(new OptionItem { name = "Failed", value = 8 });
            //statuses.Add(new OptionItem { name = "Closed", value = 9 });

            return statuses;
        }


        
        private List<OptionItem> getComplexities()
        {

            List<OptionItem> complexities = new List<OptionItem>();
            //complexities.Add(new OptionItem { name = "None", value = 0 });
            complexities.Add(new OptionItem { name = "Low", value = 1 });
            complexities.Add(new OptionItem { name = "Medium", value = 2 });
            complexities.Add(new OptionItem { name = "High", value = 3 });
            complexities.Add(new OptionItem { name = "Very High", value = 4 });
            return complexities;


        }


        private List<OptionItem> getTypes()
        {

            List<OptionItem> types = new List<OptionItem>();
            //types.Add(new OptionItem { name = "None", value = 0 });
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
            //severities.Add(new OptionItem { name = "None", value = 0 });
            severities.Add(new OptionItem { name = "Low", value = 1 });
            severities.Add(new OptionItem { name = "Medium", value = 2 });
            severities.Add(new OptionItem { name = "High", value = 3 });
            severities.Add(new OptionItem { name = "Critical", value = 4 });

            return severities;
        }

        private List<OptionItem> getPriorities()
        {

            List<OptionItem> priorities = new List<OptionItem>();
            //priorities.Add(new OptionItem { name = "None", value = 0 });
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


            var currentprojectid = UserSession.Current.CurrentProjectId;

            var currentproject = db.projects.Find(currentprojectid);
            
            
            //http://stackoverflow.com/questions/13405568/linq-unable-to-create-a-constant-value-of-type-xxx-only-primitive-types-or-enu
            var users = db.users.Where(x=>x.projects.Select(p=>p.project_id).Contains(currentprojectid)).ToList();

            var unassigned = new user { user_id = 0, firstname = "Unassigned" };
            //users.Add(unassigned) ;

            //Where(l => l.Courses.Select(c => c.CourseId).Contains(courseId)

            
            ViewBag.assigned_to = new SelectList(users, "user_id", "firstname", unassigned);
            

            //ViewBag.status = new SelectList(getStatuses(), "value", "name");
            //ViewBag.test_status = new SelectList(getTestStatuses(), "value", "name");
            ViewBag.complexity = new SelectList(getComplexities(), "value", "name");
            ViewBag.type= new SelectList(getTypes(), "value", "name");
            ViewBag.severity = new SelectList(getSeverities(), "value", "name");
            ViewBag.priority = new SelectList(getPriorities(), "value", "name");



            return View(new task { due_date = DateTime.Now.AddDays(1) });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateComment([Bind(Include = "task_id,title,description")] comment comment)
        {
            try
            {
                comment.user_id = UserSession.Current.UserId;
                comment.created_on = DateTime.Now;

                if (!string.IsNullOrWhiteSpace(comment.description) || !string.IsNullOrWhiteSpace(comment.title))
                { 
                
                    if (ModelState.IsValid)
                    {
                        db.comments.Add(comment);
                        db.SaveChanges();

                        log log = new log(AppEnums.LogOperationEnum.CREATE, AppEnums.LogTypeEnum.COMMENT, string.Format(" Comment {0}-{1}-Added by user {2}", comment.comment_id, comment.title, UserSession.Current.Username), comment.task_id);

                        CreateLog(log);

                    }
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
        public ActionResult CreateTask([Bind(Include = "task_id,project_id,status,test_status,title,description,initiator,type,complexity,severity,priority,due_date,assigned_to")] task task)
        {
            try
            {

                task.created_by = UserSession.Current.UserId;
                task.created_on = DateTime.Now;
                task.state = 0;
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



                    log log = new log(AppEnums.LogOperationEnum.CREATE, AppEnums.LogTypeEnum.TASK, string.Format(" Task created by user {0}", UserSession.Current.Username), task.task_id);
                    CreateLog(log);


                    //taskHist hist = new taskHist
                    //{

                    //    task_id = task.task_id,

                    //    project_id = task.project_id,
                    //    status = "0" ,
                    //    title = task.title,
                    //    description = task.description,
                    //    initiator = task.initiator ,
                    //    complexity = task.complexity.HasValue? task.complexity.Value.ToString():"0" ,
                    //    priority = task.priority.HasValue? task.priority.Value.ToString():"0" ,
                    //    due_date = task.due_date.HasValue?task.due_date.Value.ToString("yyyy MM dd") : "",
                    //    created_on = task.created_on,
                    //    created_by = task.created_by,
                    //    firstname = UserSession.Current.Firstname,
                    //    fullname = UserSession.Current.Fullname,
                    //    hist_status = 0

                    //};
                    //db.taskHists.Add(hist);
                    //db.SaveChanges();



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
                return RedirectToAction("Index");
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            task task = db.tasks.Find(id);

            if(task.IsDeleted || task.IsArchived)
            {
                return HttpNotFound();
            }

            ViewBag.project_id = new SelectList(db.projects, "project_id", "name", task.project_id);
            var currentprojectid = UserSession.Current.CurrentProjectId;

            var currentproject = db.projects.Find(currentprojectid);


            //http://stackoverflow.com/questions/13405568/linq-unable-to-create-a-constant-value-of-type-xxx-only-primitive-types-or-enu
            var users = db.users.Where(x => x.projects.Select(p => p.project_id).Contains(currentprojectid)).ToList();
            //Where(l => l.Courses.Select(c => c.CourseId).Contains(courseId)

            ViewBag.EmailTo = users;


            var unassigned = new user { user_id = 0, firstname = "Unassigned" };
            users.Add(unassigned);

            ViewBag.assigned_to = new SelectList(users, "user_id", "firstname", task.assigned_to);

            var statuslist = getStatuses(task.status.Value);
            
            if(task.user1.user_id != UserSession.Current.UserId) 
            {
                                
                statuslist.Remove(new OptionItem { name = "Closed", value = 9 });
            }
                

            ViewBag.status = new SelectList(statuslist, "value", "name", task.status );

            
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
        public ActionResult EditTask([Bind(Include = "task_id,project_id,status,test_status,type,title,description,initiator,complexity,priority,due_date,created_on,created_by,assigned_to")] task task)
        {
            var oldtask = db.tasks.AsNoTracking().Where(x=>x.task_id == task.task_id).FirstOrDefault();

            if (ModelState.IsValid)
            {

                task.updated_on = DateTime.Now;
                task.state = 0;
                db.Entry(task).State = EntityState.Modified;
                db.SaveChanges();

                logChanges(oldtask, task);

                //taskHist hist = new taskHist
                //{
                    
                //    task_id = task.task_id,

                //    project_id = task.project_id,
                //    status = oldtask.status != task.status ? string.Concat(oldtask.status, ">", task.status) : task.status.Value.ToString(),
                //    title = string.Concat(oldtask.title,">",task.title),
                //    description = string.Concat(oldtask.description,">",task.description),
                //    initiator = oldtask.initiator != task.initiator? string.Concat(oldtask.initiator,">",task.initiator) : null,
                //    complexity = oldtask.complexity != task.complexity ? string.Concat(oldtask.complexity,">",task.complexity) : null,
                //    priority = oldtask.priority != task.priority ? string.Concat(oldtask.priority,">",task.priority) : null,
                //    due_date = oldtask.due_date != task.due_date ? 
                //        string.Concat((oldtask.due_date.HasValue?oldtask.due_date.Value.ToString("yyyy MM dd"):""),
                //                        ">",
                //        (task.due_date.HasValue?task.due_date.Value.ToString("yyyy MM dd"):""))
                //        : null,
                //    created_on = DateTime.Now,
                //    created_by = UserSession.Current.UserId,
                //    firstname = UserSession.Current.Firstname,
                //    fullname = UserSession.Current.Fullname,
                //    hist_status = 1
                    
                //};
                //db.taskHists.Add(hist);
                //db.SaveChanges();

                return RedirectToAction("EditTask", new { id = task.task_id });
            }

            ViewBag.project_id = new SelectList(db.projects, "project_id", "name", task.project_id);
            ViewBag.status = new SelectList(getStatuses(task.status.Value), "value", "name", task.status);
            //ViewBag.test_status = new SelectList(getTestStatuses(), "value", "name", task.test_status);
            ViewBag.complexity = new SelectList(getComplexities(), "value", "name", task.complexity);
            ViewBag.type = new SelectList(getTypes(), "value", "name", task.type);
            ViewBag.severity = new SelectList(getSeverities(), "value", "name", task.severity);
            ViewBag.priority = new SelectList(getPriorities(), "value", "name", task.priority);


            return View(task);
        }

        private void logChanges(task oldtask, task newtask)
        {
            try 
	        {
                //status 
                if (oldtask.status != newtask.status)
                {
                    log log = new log(AppEnums.LogOperationEnum.UPDATE, AppEnums.LogTypeEnum.STATUS, string.Format(" {0} changed by user {1}", Utils.EnumToString(AppEnums.LogTypeEnum.STATUS.ToString(),true) , UserSession.Current.Username), oldtask.StatusDesc, newtask.StatusDesc, newtask.task_id);
                    CreateLog(log);
                }

                //owner

                if (oldtask.assigned_to!= newtask.assigned_to)
                {
                    log log = new log(AppEnums.LogOperationEnum.UPDATE, AppEnums.LogTypeEnum.ASSIGNEE,
                        string.Format(" {0} changed by user {1}", Utils.EnumToString(AppEnums.LogTypeEnum.ASSIGNEE.ToString(), true),
                        UserSession.Current.Username), oldtask.assigned_to.ToString(), newtask.assigned_to.ToString(), newtask.task_id);
                    CreateLog(log);
                }



                //title
                if (!oldtask.title.Equals(newtask.title))
                {
                    log log = new log(AppEnums.LogOperationEnum.UPDATE, AppEnums.LogTypeEnum.TITLE,
                        string.Format(" {0} changed by user {1}", Utils.EnumToString(AppEnums.LogTypeEnum.TITLE.ToString(), true),
                        UserSession.Current.Username), oldtask.title, newtask.title, newtask.task_id);
                    CreateLog(log);
                }

                //type
                if (oldtask.type!=newtask.type)
                {
                    log log = new log(AppEnums.LogOperationEnum.UPDATE, AppEnums.LogTypeEnum.TYPE,
                        string.Format(" {0} changed by user {1}", Utils.EnumToString(AppEnums.LogTypeEnum.TYPE.ToString(), true),
                        UserSession.Current.Username), oldtask.TypeDesc, newtask.TypeDesc, newtask.task_id);
                    CreateLog(log);
                }

                //severity
                if (oldtask.severity != newtask.severity)
                {
                    log log = new log(AppEnums.LogOperationEnum.UPDATE, AppEnums.LogTypeEnum.SEVERITY,
                        string.Format(" {0} changed by user {1}", Utils.EnumToString(AppEnums.LogTypeEnum.SEVERITY.ToString(), true),
                        UserSession.Current.Username), oldtask.SeverityDesc, newtask.SeverityDesc, newtask.task_id);
                    CreateLog(log);
                }
                //priority

                if (oldtask.priority != newtask.priority)
                {
                    log log = new log(AppEnums.LogOperationEnum.UPDATE, AppEnums.LogTypeEnum.PRIORITY,
                        string.Format(" {0} changed by user {1}", Utils.EnumToString(AppEnums.LogTypeEnum.PRIORITY.ToString(), true),
                        UserSession.Current.Username), oldtask.PriorityDesc, newtask.PriorityDesc, newtask.task_id);
                    CreateLog(log);
                }

                //complexity

                if (oldtask.complexity != newtask.complexity)
                {
                    log log = new log(AppEnums.LogOperationEnum.UPDATE, AppEnums.LogTypeEnum.COMPLEXITY,
                        string.Format(" {0} changed by user {1}", Utils.EnumToString(AppEnums.LogTypeEnum.COMPLEXITY.ToString(), true),
                        UserSession.Current.Username), oldtask.ComplexityDesc, newtask.ComplexityDesc, newtask.task_id);
                    CreateLog(log);
                }

                //due date
                if (oldtask.due_date!= newtask.due_date)
                {
                    log log = new log(AppEnums.LogOperationEnum.UPDATE, AppEnums.LogTypeEnum.DATE,
                        string.Format(" {0} changed by user {1}", Utils.EnumToString(AppEnums.LogTypeEnum.DATE.ToString(), true),
                        UserSession.Current.Username), oldtask.due_date.HasValue? oldtask.due_date.Value.ToString("dd-MM-yyyy"): string.Empty,
                        newtask.due_date.HasValue ? newtask.due_date.Value.ToString("dd-MM-yyyy") : string.Empty
                        , newtask.task_id);
                    CreateLog(log);
                }

                //owner

                if (oldtask.owner != newtask.owner)
                {
                    log log = new log(AppEnums.LogOperationEnum.UPDATE, AppEnums.LogTypeEnum.OWNER,
                        string.Format(" {0} changed by user {1}", Utils.EnumToString(AppEnums.LogTypeEnum.OWNER.ToString(), true),
                        UserSession.Current.Username), oldtask.owner.ToString(), newtask.owner.ToString(), newtask.task_id);
                    CreateLog(log);
                }



            }
            catch (Exception)
	        {

		        throw;
	        }

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

            try
            {
                task task = db.tasks.Find(id);


                //state 1 = deleted, 2 = archived, 0 = active
                task.state = 1;
                db.Entry(task).State = EntityState.Modified;
                db.SaveChanges();


                log log = new log(AppEnums.LogOperationEnum.DELETE, AppEnums.LogTypeEnum.TASK, string.Format(" Task deleted by user {0}", UserSession.Current.Username), id);
                CreateLog(log);

            }
            catch (Exception ex)
            {

                HandleException(ex);
            }
            



            return RedirectToAction("TaskList");
        }

        public virtual void HandleException(Exception exception)
        {
            DbUpdateConcurrencyException concurrencyEx = exception as DbUpdateConcurrencyException;
            if (concurrencyEx != null)
            {
                // A custom exception of yours for concurrency issues
                throw new DBConcurrencyException();
            }

            DbUpdateException dbUpdateEx = exception as DbUpdateException;
            if (dbUpdateEx != null)
            {
                if (dbUpdateEx != null
                        && dbUpdateEx.InnerException != null
                        && dbUpdateEx.InnerException.InnerException != null)
                {
                    SqlException sqlException = dbUpdateEx.InnerException.InnerException as SqlException;
                    if (sqlException != null)
                    {
                        switch (sqlException.Number)
                        {
                            case 2627:  // Unique constraint error
                            case 547:   // Constraint check violation
                            case 2601:  // Duplicated key row error
                                // Constraint violation exception
                                throw new DBConcurrencyException();   // A custom exception of yours for concurrency issues

                            default:
                                // A custom exception of yours for other DB issues
                                throw new DbUpdateException(dbUpdateEx.Message, dbUpdateEx.InnerException);
                        }
                    }

                    throw new DbUpdateException(dbUpdateEx.Message, dbUpdateEx.InnerException);
                }
            }

            // If we're here then no exception has been thrown
            // So add another piece of code below for other exceptions not yet handled...
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NotifyUsers(int taskId, int projectId, int[] notify)
        {

            try
            {
                

                List<EmailRecipient> recipients = new List<EmailRecipient>();
                var task = db.tasks.Find(taskId);

                task.HiUser = "Team";

                for (int i = 0; i < notify.Count(); i++)
                {
                
                    var user = db.users.Find(notify[i]);
                    

                    var assigned_user = task.assigned_to.HasValue ? task.assigned_to.Value:  0;
                    if (user != null && !string.IsNullOrWhiteSpace(user.email))
                    {
                        var isTo = false;
                        
                        if(user.user_id == assigned_user || notify.Count() == 1)
                        {
                            isTo = true;

                            task.HiUser = user.firstname;
                        
                        }

                        recipients.Add(new EmailRecipient { RecipientEmail = user.email, RecipientUserId = user.username, RecipientName = user.fullname , isTo = isTo });

                    }
                }
                    Emailer mail = new Emailer();


                    foreach (var item in recipients)
                    {
                        //mail.AddRecipient(String.Concat(item.RecipientName, " <", item.RecipientEmail, ">"));
                        

                        if(item.isTo)
                        {

                        mail.AddRecipient(item.RecipientEmail);
                        }
                        else
                        {
                            mail.AddCcRecipient(item.RecipientEmail);
                        }

                        
                    };

                    

                    var senderid = UserSession.Current.UserId;
                    var sender = db.users.Find(senderid);

                    

                    //Generates the email body
                         mail.Body = RenderRazorViewToString(this.ControllerContext, "_notificationBody", task);

                         var subj = string.Format("CatchMe! #{0} - {1}", task.task_id, task.title);


                

                    MailAddress senderAddress = new MailAddress(sender.email);
                    mail.SenderMail = senderAddress;



                    mail.SendMail(subj, mail.Body, true);

                StringBuilder _recipients = new StringBuilder();

                foreach(var r in recipients)
                {
                    _recipients.AppendFormat("{0};", r.RecipientName);

                }

                if(_recipients.Length > 1)
                {
                    _recipients.Length--;
                }

                notification notif = new notification { sender_id = UserSession.Current.UserId, sender_name = UserSession.Current.Fullname, task_id = taskId, sent_on = DateTime.Now, recicipents = _recipients.ToString() };

                db.notifications.Add(notif);
                db.SaveChanges();

                    return RedirectToAction("EditTask", new { id = taskId });


                
            }
            catch (Exception e)
            {
                //log.Error("[SendEmail] - Exception Caught" + e.ToString());
                //TempData["errorLog"] = new ErrorLog(e);
                return RedirectToAction("ShowError", "Error");
            }


        }


        [HttpPost]
        public ActionResult EditComment(comment comment)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    db.Entry(comment).State = EntityState.Modified;
                    db.SaveChanges();

                    log log = new log(AppEnums.LogOperationEnum.UPDATE, AppEnums.LogTypeEnum.COMMENT, string.Format(" Comment {0}-{1}-Edited by user {2}",comment.comment_id, comment.title, UserSession.Current.Username), comment.task_id);
                    CreateLog(log);
                }


                return RedirectToAction("EditTask", new { id = comment.task_id });
                }
            catch (Exception e)
            {

                throw;
            }
        }

        
        public ActionResult DeleteComment(int id, int taskId)
        {
            try
            {
                comment comment = db.comments.Find(id);
                db.comments.Remove(comment);
                db.SaveChanges();

               log log = new log(AppEnums.LogOperationEnum.DELETE, AppEnums.LogTypeEnum.COMMENT, string.Concat(comment.comment_id, "-", comment.description), comment.task_id);
                CreateLog(log);
                return RedirectToAction("EditTask", new { id=taskId });

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
