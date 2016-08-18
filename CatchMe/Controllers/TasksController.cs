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
using System.Net.Mail;
using System.Text;
using System.Data.Entity.Validation;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using CatchMe.Models.ViewModel;

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


        public ActionResult ClearFilter()
        {
            UserSession.Current.searchFilter.keywords = string.Empty;
            return RedirectToAction("SetSearchFilter");
        }


        public ActionResult SetSearchFilter(SearchFilter searchFilter)
        {
            UserSession.Current.searchFilter = searchFilter;
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


            var yesterday = DateTime.Now.AddDays(-1);
            var myRecentNotifications = db.notifications.Where(x => x.send_to_id == UserSession.Current.UserId && x.sent_on > yesterday).Select(x=>x.task_id).ToList();
            List<task> tasknotifs = new List<task>();
            foreach(var n in myRecentNotifications)
            {
                task t = new task {task_id = n};
                tasknotifs.Add(t);
            }

            foreach(var t in tasks)
            {
                if(tasknotifs.Contains(t))
                {
                    t.isInNotificationList = true;
                }
            }

            

            //populate search filter
            var currentprojectid = UserSession.Current.CurrentProjectId;
            var users = db.users.Where(x => x.projects.Select(p => p.project_id).Contains(currentprojectid)).ToList();


            var defaultoption = new OptionItem { name = "Any", value = -1 };

            var statuses = getStatuses(-1);
            statuses.Add(defaultoption);
            statuses = statuses.OrderBy(x => x.value).ToList();


            var types = getTypes();
            types.Add(defaultoption);
            types = types.OrderBy(x => x.value).ToList();
            
            var priorities = getPriorities();
            priorities.Add(defaultoption);
            priorities = priorities.OrderBy(x => x.value).ToList();

            user defaultUser = new user { user_id = -1, firstname = "Any" };
            users.Remove(new user { user_id = 0 });
            users.Add(defaultUser);
            users = users.OrderBy(x => x.user_id).ToList();

            var session = UserSession.Current.searchFilter;

            ViewBag.status = new SelectList(statuses, "value", "name", defaultoption);
            ViewBag.type = new SelectList(types, "value", "name", defaultoption);            
            ViewBag.priority = new SelectList(priorities, "value", "name", defaultoption);
            ViewBag.createdBy= new SelectList(users, "user_id", "firstname", defaultUser);
            ViewBag.owner = new SelectList(users, "user_id", "firstname", defaultUser);
            ViewBag.assignedTo = new SelectList(users, "user_id", "firstname", defaultUser);


            var result = MarkAsFiltered(tasks);

            var filtered = result.Where(x => x.IsFilteredOn).ToList();
            return View(result);
        }


        private List<task>  MarkAsFiltered (List<task> tasks)                
        {
            try
            {
                var list = tasks;
 
                // stop words
                //a,able,about,across,after,all,almost,also,am,among,an,and,any,are,as,at,be,because,been,but,by,can,cannot,could,dear,did,do,does,either,else,ever,every,for,from,get,got,had,has,have,he,her,hers,him,his,how,however,i,if,in,into,is,it,its,just,least,let,like,likely,may,me,might,most,must,my,neither,no,nor,not,of,off,often,on,only,or,other,our,own,rather,said,say,says,she,should,since,so,some,than,that,the,their,them,then,there,these,they,this,tis,to,too,twas,us,wants,was,we,were,what,when,where,which,while,who,whom,why,will,with,would,yet,you,your

                string[] swordarray = new string[] {
                    "a","able","about","across","after","all","almost","also","am","among","an","and","any","are","as","at","be","because","been","but","by","can","cannot","could","dear","did","do","does","either","else","ever","every","for","from","get","got","had","has","have","he","her","hers","him","his","how","however","i","if","in","into","is","it","its","just","least","let","like","likely","may","me","might","most","must","my","neither","no","nor","not","of","off","often","on","only","or","other","our","own","rather","said","say","says","she","should","since","so","some","than","that","the","their","them","then","there","these","they","this","tis","to","too","twas","us","wants","was","we","were","what","when","where","which","while","who","whom","why","will","with","would","yet","you","your"
                };
                List<string> stopwords = new List<string>(swordarray);


                var searchFilter = UserSession.Current.searchFilter;
                var result = new List<task>();                              

                if (searchFilter != null)
                {
                        if(searchFilter.keywords != null)
                        {
                            var projectId = tasks.FirstOrDefault().project_id;
                            var comments = db.comments.Where(x => x.task.project_id == projectId);

                            string[] keywords = searchFilter.keywords.Split(' ');
                            List<comment> commentlist = new List<comment>();
                            List<task> tasklist = new List<task>();
                            foreach (var k in keywords)
                            {
                                if (!stopwords.Contains(k))
                                {
                                    var cl = comments.Where(x => x.description.ToLower().Contains(k.ToLower())).ToList();
                                    commentlist.AddRange(cl);
                                }
                            }


                            //var commentList = comments.Where(x => x.description.ToLower().Contains(searchFilter.keywords.ToLower())).ToList();

                            foreach (var k in keywords)
                            {
                                if (!stopwords.Contains(k))
                                {
                                    var tl = db.tasks.Where(x => x.title.ToLower().Contains(k.ToLower())).ToList();
                                    tasklist.AddRange(tl);
                                }
                            }

                        var foundTasks = new List<task>();
                        foreach(var tsk in tasklist)
                        {
                           foundTasks.Add(new task { task_id = tsk.task_id });
                        }
                        foreach (var comm in commentlist)
                            {
                                foundTasks.Add(new task { task_id = comm.task_id });
                            }

                            foreach(var item in list.ToList())
                            {
                                if(foundTasks.Contains(item))
                                {
                                    item.IsFilteredOn = true;
                                }
                            }

                            //return list;
                        }
                    
                    
                    if (searchFilter.assignedTo != null)
                    {
                        
                        foreach (var person in searchFilter.assignedTo)
                        {
                            var mylist = list.Where(x => x.assigned_to.Value == person).ToList();

                            result.AddRange(mylist);
                        }
                        list = result;
                     
                    }

                    if (searchFilter.owner != null)
                    {
                        result.Clear();
                        foreach (var person in searchFilter.owner)
                        {
                            var mylist = list.Where(x => x.owner == person).ToList();

                            result.AddRange(mylist);

                        }
                        list = result;

                    }

                    if (searchFilter.createdBy != null)
                    {
                        //list = list.Where(x => x.created_by == searchFilter.createdBy).ToList();
                        }

                    //if (searchFilter.status > 0)
                    //{
                    //    list = list.Where(x => x.status == searchFilter.status).ToList();
                    //    }


                    if (searchFilter.priority != null)
                    {
                       // list = list.Where(x => x.priority == searchFilter.priority).ToList();
                      }



                    if (searchFilter.type != null)
                    {
                        //list = list.Where(x => x.type == searchFilter.type).ToList();
                    }


                }

                //foreach (var item in list)
                //{
                //    item.IsFilteredOn = true;
                //}
                return list;

            }
            catch (Exception e)
            {

                throw;
            }
        }


        public ActionResult NoProject()
        {
            return View();
        }



        [HttpPost]
        public ActionResult UploadFile(HttpPostedFileBase file, int taskid, int projectid, int comment_id = -1, int information_id = -1)
        {
            try
            {

                var _filepath = string.Format("{0}/{1}/{2}", projectid, taskid, file.FileName);

                if(information_id > 0)
                {
                    _filepath = string.Format("{0}/information/{1}/{2}", projectid, information_id, file.FileName);
                }

                attachment attachment = new attachment
                {
                    filename = file.FileName,
                    content_length = file.ContentLength,
                    mimetype = file.ContentType,
                    created_on = DateTime.Now,
                    task_id = taskid,
                    user_id = UserSession.Current.UserId,
                  comment_id = comment_id,
                    information_id = information_id,
                    filepath = _filepath


                };

                if(SaveResourceToDisk(file,taskid,information_id))
                {

                    try
                    {
                    db.attachments.Add(attachment);
                    db.SaveChanges();


                        if(information_id > 0)
                        {
                            log log = new log(AppEnums.LogOperationEnum.CREATE, AppEnums.LogTypeEnum.ATTACHMENT, string.Format("{0} for info id {1} ",file.FileName, information_id), taskid);
                            CreateLog(log);

                            //updateTask(taskid);

                        }
                        else
                        {
                            log log = new log(AppEnums.LogOperationEnum.CREATE, AppEnums.LogTypeEnum.ATTACHMENT, file.FileName, taskid);
                            CreateLog(log);

                            updateTask(taskid);

                        }
                        

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

                if (information_id > 0)
                {
                    return RedirectToAction("ShowInfo", new { id = information_id });
                
                }
                
                return RedirectToAction("EditTask", new { id = taskid });
            }
            catch (Exception e)
            {

                throw;
            }
        }



        /// <summary>
        /// update task everytime an event is recorded
        /// </summary>
        /// <param name="taskid"></param>
        private void updateTask(int taskid)
        {
            //update item with changes.
            var task = db.tasks.Find(taskid);
            task.updated_on = DateTime.Now;
            task.updated_by = UserSession.Current.UserId;
            db.Entry(task).State = EntityState.Modified;
            db.SaveChanges();

        }

        public ActionResult DeleteAttachment(int id, int taskid, int information_id = -1)
        {
            try
            {
                if(DeleteResourceFromDisk(id))
                {
                    var file = db.attachments.Find(id);
                    var filename = file.filename;
                    db.attachments.Remove(file);

                    db.SaveChanges();


                    log log = new log(AppEnums.LogOperationEnum.DELETE, AppEnums.LogTypeEnum.ATTACHMENT, filename, taskid);
                    
                    CreateLog(log);


                }

                if (information_id > 0)
                {
                    return RedirectToAction("ShowInfo", new { id = information_id});
                
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

        private bool SaveResourceToDisk(HttpPostedFileBase mainFile, int taskid, int informationid =-1)
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

                    //if item is info derive project id from information instead
                    if(informationid > 0)
                    {
                        projectid = db.information.Find(informationid).project_id;
                    }

                    var serverpathprod = Helpers.ConfigurationHelper.GetServerPathProd();
                    var IsProd = Helpers.ConfigurationHelper.IsProd();


                    var serverpath = System.IO.Path.GetFullPath("/");
                    string id = Guid.NewGuid().ToString();

                

                    imageExt = System.IO.Path.GetExtension(mainFile.FileName);
                    imageName = System.IO.Path.GetFileName(mainFile.FileName);

                    string dirPath = System.Web.HttpContext.Current.Server.MapPath("~") + "/uploads/" + string.Format("{0}/", projectid) + string.Format("{0}/", taskid);

                    if(informationid > 0)
                    {
                         dirPath = System.Web.HttpContext.Current.Server.MapPath("~") + "/uploads/" + string.Format("{0}/information/", projectid) + string.Format("{0}/", informationid);
                    }
                    
                    

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

                var roles = project.project_user_role.Where(x => x.user_id == user.user_id).ToList();

                if (roles.Count()>0)
                {
                    UserSession.Current.CurrentProjectRole = roles.FirstOrDefault().role;
                }

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

            if (val == -1)
            {
                statuses.Add(new OptionItem { name = "New", value = 0 });
                statuses.Add(new OptionItem { name = "Action", value = 1 });
                statuses.Add(new OptionItem { name = "Investigation", value = 2 });
                statuses.Add(new OptionItem { name = "Completed", value = 3 });
                statuses.Add(new OptionItem { name = "On Hold", value = 4 });
                statuses.Add(new OptionItem { name = "Problem", value = 5 });
                statuses.Add(new OptionItem { name = "No Issue", value = 6 });
                statuses.Add(new OptionItem { name = "Test Passed", value = 7 });
                statuses.Add(new OptionItem { name = "Test Failed", value = 8 });
                statuses.Add(new OptionItem { name = "Closed", value = 9 });

                statuses.Add(new OptionItem { name = "For Test", value = 10 });



            }
        

            if (val == 0)
            {
                statuses.Add(new OptionItem { name = "New", value = 0 });
                statuses.Add(new OptionItem { name = "Action", value = 1 });
                statuses.Add(new OptionItem { name = "Investigate", value = 2 });
            }

            if (val == 1)
            {
                statuses.Add(new OptionItem { name = "Reset to New", value = 0 });
                statuses.Add(new OptionItem { name = "Action", value = 1 });
                statuses.Add(new OptionItem { name = "Completed", value = 3 });
                statuses.Add(new OptionItem { name = "On Hold", value = 4 });
                statuses.Add(new OptionItem { name = "Problem", value = 5 });
                statuses.Add(new OptionItem { name = "No Issue", value = 6 });
            }


            if (val == 2)
            {
                statuses.Add(new OptionItem { name = "Reset to New", value = 0 });
                statuses.Add(new OptionItem { name = "Start Development", value = 1 });
                statuses.Add(new OptionItem { name = "Investigation", value = 2 });
                statuses.Add(new OptionItem { name = "Completed", value = 3 });
                statuses.Add(new OptionItem { name = "On Hold", value = 4 });
                statuses.Add(new OptionItem { name = "Problem", value = 5 });
                statuses.Add(new OptionItem { name = "No Issue", value = 6 });
            }


            if (val == 3)            
            {
                statuses.Add(new OptionItem { name = "Re-Open", value = 1 });
                statuses.Add(new OptionItem { name = "Completed", value = 3 });
                statuses.Add(new OptionItem { name = "For Test", value = 10 });
                statuses.Add(new OptionItem { name = "Test Passed", value = 7 });
                statuses.Add(new OptionItem { name = "Test Failed", value = 8 });
            }

            if (val == 4)
            {
                statuses.Add(new OptionItem { name = "Resume", value = 1 });
                statuses.Add(new OptionItem { name = "On Hold", value = 4 });
            }

            if (val == 5)
            {
                statuses.Add(new OptionItem { name = "Resume", value = 1 });
                statuses.Add(new OptionItem { name = "Problem", value = 5 });
            }

            if (val == 6)
            {
                statuses.Add(new OptionItem { name = "Re-Open", value = 1 });
                statuses.Add(new OptionItem { name = "No Issue", value = 6 });
                statuses.Add(new OptionItem { name = "Close Task", value = 9 });
            }

            if (val == 7)
            {
                statuses.Add(new OptionItem { name = "Re-Open", value = 1 });
                statuses.Add(new OptionItem { name = "Passed", value = 7 });
                statuses.Add(new OptionItem { name = "Close Task", value = 9 });
            }

            if (val == 8)
            {
                statuses.Add(new OptionItem { name = "Re-Work", value = 1 });
                statuses.Add(new OptionItem { name = "Failed", value = 8 });
            }

            if (val == 9)
            {
                statuses.Add(new OptionItem { name = "Re-Open", value = 1 });
                statuses.Add(new OptionItem { name = "Closed", value = 9 });
            }

            if (val == 10)
            {                
                statuses.Add(new OptionItem { name = "For Test", value = 10 });               
                statuses.Add(new OptionItem { name = "Test Passed", value = 7 });
                statuses.Add(new OptionItem { name = "Test Failed", value = 8 });
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


        private List<OptionItem> getTypes(bool includeinfo=false)
        {

            List<OptionItem> types = new List<OptionItem>();
            //types.Add(new OptionItem { name = "None", value = 0 });
            types.Add(new OptionItem { name = "Development", value = 1 });
            types.Add(new OptionItem { name = "Change", value = 2 });
            types.Add(new OptionItem { name = "Bug", value = 3 });
            types.Add(new OptionItem { name = "Failure", value = 4 });
            types.Add(new OptionItem { name = "Test", value = 5 });
            types.Add(new OptionItem { name = "Investigation", value = 6 });
            types.Add(new OptionItem { name = "Setup/Configuration", value = 7 });
            if (includeinfo)
            {
                types.Add(new OptionItem { name = "Information", value = 8 });
            }
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

            ViewBag.ProjectDesc = currentproject.description;

            
            //http://stackoverflow.com/questions/13405568/linq-unable-to-create-a-constant-value-of-type-xxx-only-primitive-types-or-enu
            var users = db.users.Where(x=>x.projects.Select(p=>p.project_id).Contains(currentprojectid)).ToList();

            var unassigned = new user { user_id = 0, firstname = "Unassigned" };
            //users.Add(unassigned) ;

            //Where(l => l.Courses.Select(c => c.CourseId).Contains(courseId)

            
            ViewBag.assigned_to = new SelectList(users, "user_id", "firstname", unassigned);
            

            //ViewBag.status = new SelectList(getStatuses(), "value", "name");
            //ViewBag.test_status = new SelectList(getTestStatuses(), "value", "name");
            ViewBag.complexity = new SelectList(getComplexities(), "value", "name");
            ViewBag.type= new SelectList(getTypes(true), "value", "name");
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

                        log log = new log(AppEnums.LogOperationEnum.CREATE, AppEnums.LogTypeEnum.COMMENT, string.Format(" Comment Added: {0}-{1}", comment.comment_id, comment.title), comment.task_id);
                        CreateLog(log);

                        //update task last edited on
                        updateTask(comment.task_id);


                    }
                }
                return RedirectToAction("EditTask", new { id = comment.task_id });
                    
            }
            catch (Exception)
            {
                
                throw;
            }
        }


        public ActionResult ShowInfo(int? id)
        {
            try
            {
                if (id == null)
                {
                    return RedirectToAction("Index");
                    //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                var info = db.information.Find(id);

                ViewBag.importance = new SelectList(getComplexities(), "value", "name", info.importance);


                if (info == null)
                {
                    return HttpNotFound();
                }

                return View(info);

                

            }
            catch (Exception)
            {
                
                throw;
            }
        }


        // POST: information/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditInfo([Bind(Include = "information_id,project_id,title,description,importance,created_by,created_on,updated_by,updated_on,state")] information information)
        {
            if (ModelState.IsValid)
            {
                information.updated_on = DateTime.Now;
                information.updated_by = UserSession.Current.UserId;

                db.Entry(information).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ShowInfo", new { id = information.information_id });
            }
            ViewBag.importance = new SelectList(getComplexities(), "value", "name", information.importance);
            
            return View(information);
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


                if(task.type == 8 )
                {

                    information info = new information
                    {
                        project_id = task.project_id,
                        title = task.title,
                        description = task.description,
                        importance = task.complexity,
                        created_by = UserSession.Current.UserId,
                        updated_by = UserSession.Current.UserId,
                        created_on = DateTime.Now,
                        updated_on = DateTime.Now
                    };

                    db.information.Add(info);
                    db.SaveChanges();


                    log log = new log(AppEnums.LogOperationEnum.CREATE, AppEnums.LogTypeEnum.INFO, string.Format("Info Item {0}-{1} created for project {2}",info.information_id, info.project_id), -1);
                    CreateLog(log);

                    return RedirectToAction("ShowInfo", new { id = info.information_id});

                }
                else
                {


                

                task.created_by = UserSession.Current.UserId;
                task.owner= UserSession.Current.UserId;
                task.updated_by = UserSession.Current.UserId;

                task.created_on = DateTime.Now;
                task.updated_on = DateTime.Now;
                task.state = 0;
                if (ModelState.IsValid)
                {
                    db.tasks.Add(task);
                    db.SaveChanges();


                    comment _comment = new comment
                    {
                        task_id = task.task_id,
                        user_id = task.created_by.Value,
                        title = task.title,
                        created_on = task.created_on,
                        description = task.description
                    };

                    db.comments.Add(_comment);
                    db.SaveChanges();



                    log log = new log(AppEnums.LogOperationEnum.CREATE, AppEnums.LogTypeEnum.TASK, "Task created", task.task_id);
                    CreateLog(log);

                    return RedirectToAction("EditTask", new  { id=task.task_id });

                }
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

            

            task.History = db.viewHistories.Where(x => x.task_id == id).OrderByDescending(o=>o.logtime).ToList();
            //task.History = allhistory.Where(x => x.task_id == id).ToList();

            ViewBag.project_id = new SelectList(db.projects, "project_id", "name", task.project_id);
            var currentprojectid = UserSession.Current.CurrentProjectId;

            var currentproject = db.projects.Find(currentprojectid);


            //http://stackoverflow.com/questions/13405568/linq-unable-to-create-a-constant-value-of-type-xxx-only-primitive-types-or-enu
            var users = db.users.Where(x => x.projects.Select(p => p.project_id).Contains(currentprojectid)).ToList();
            //Where(l => l.Courses.Select(c => c.CourseId).Contains(courseId)

            ViewBag.EmailTo = users;


            var unassigned = new user { user_id = 0, firstname = "Unassigned" };
            //users.Add(unassigned);

            ViewBag.assigned_to = new SelectList(users, "user_id", "firstname", task.assigned_to);

            var listwithoutunassigned = users.ToList();
            listwithoutunassigned.Remove(new user { user_id = 0 });


            var listwithoutUnassignedAndCurrent = users.ToList();
            listwithoutUnassignedAndCurrent.Remove(new user { user_id = UserSession.Current.UserId });
            listwithoutUnassignedAndCurrent.Remove(new user { user_id = 0 });


            ViewBag.sendTo = new SelectList(listwithoutUnassignedAndCurrent, "user_id", "firstname");


            ViewBag.owner = new SelectList(listwithoutunassigned, "user_id", "firstname", task.owner);

            var statuslist = getStatuses(task.status.Value);
            
            if(task.user1.user_id != UserSession.Current.UserId && task.status.Value != 9 && task.status.Value != 10 ) 
            {
                                

                statuslist.Remove(new OptionItem { name = "Closed", value = 9 });
                
                if(task.status.Value != 8)
                statuslist.Remove(new OptionItem { name = "Failed", value = 8 });

                if(task.status.Value!= 7)
                statuslist.Remove(new OptionItem { name = "Passed", value = 7 });
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
        public ActionResult EditTask([Bind(Include = "task_id,project_id,status,test_status,type,title,description,initiator,complexity,priority,due_date,created_on,created_by,assigned_to,owner")] task task)
        {
            var oldtask = db.tasks.AsNoTracking().Where(x=>x.task_id == task.task_id).FirstOrDefault();

            if (ModelState.IsValid)
            {


                //if the task is marked as completed or No Issue, assign it back to the owner
                if (task.status == 3)
                {
                    task.assigned_to = task.owner;
                }

                task.updated_on = DateTime.Now;
                task.updated_by = UserSession.Current.UserId;
                task.state = 0;
                db.Entry(task).State = EntityState.Modified;
                db.SaveChanges();

                logChanges(oldtask, task);
                
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
                    log log = new log(AppEnums.LogOperationEnum.UPDATE, AppEnums.LogTypeEnum.STATUS, string.Format(" {0} changed", Utils.EnumToString(AppEnums.LogTypeEnum.STATUS.ToString(),true) ), oldtask.StatusDesc, newtask.StatusDesc, newtask.task_id);
                    CreateLog(log);
                }

                //assignee

                if (oldtask.assigned_to!= newtask.assigned_to)
                {
                    log log = new log(AppEnums.LogOperationEnum.UPDATE, AppEnums.LogTypeEnum.ASSIGNEE,
                        string.Format(" {0} changed", Utils.EnumToString(AppEnums.LogTypeEnum.ASSIGNEE.ToString(), true))
                        , oldtask.assigned_to.ToString(), newtask.assigned_to.ToString(), newtask.task_id);
                    CreateLog(log);
                }



                //title
                if (!oldtask.title.Equals(newtask.title))
                {
                    log log = new log(AppEnums.LogOperationEnum.UPDATE, AppEnums.LogTypeEnum.TITLE,
                        string.Format(" {0} changed by", Utils.EnumToString(AppEnums.LogTypeEnum.TITLE.ToString(), true)), 
                        oldtask.title, newtask.title, newtask.task_id);
                    CreateLog(log);
                }

                //type
                if (oldtask.type!=newtask.type)
                {
                    log log = new log(AppEnums.LogOperationEnum.UPDATE, AppEnums.LogTypeEnum.TYPE,
                        string.Format(" {0} changed", Utils.EnumToString(AppEnums.LogTypeEnum.TYPE.ToString(), true)),
                         oldtask.TypeDesc, newtask.TypeDesc, newtask.task_id);
                    CreateLog(log);
                }

                //severity
                if (oldtask.severity != newtask.severity)
                {
                    log log = new log(AppEnums.LogOperationEnum.UPDATE, AppEnums.LogTypeEnum.SEVERITY,
                        string.Format(" {0} changed", Utils.EnumToString(AppEnums.LogTypeEnum.SEVERITY.ToString(), true)),
                         oldtask.SeverityDesc, newtask.SeverityDesc, newtask.task_id);
                    CreateLog(log);
                }
                //priority

                if (oldtask.priority != newtask.priority)
                {
                    log log = new log(AppEnums.LogOperationEnum.UPDATE, AppEnums.LogTypeEnum.PRIORITY,
                        string.Format(" {0} changed", Utils.EnumToString(AppEnums.LogTypeEnum.PRIORITY.ToString(), true)), 
                        oldtask.PriorityDesc, newtask.PriorityDesc, newtask.task_id);
                    CreateLog(log);
                }

                //complexity

                if (oldtask.complexity != newtask.complexity)
                {
                    log log = new log(AppEnums.LogOperationEnum.UPDATE, AppEnums.LogTypeEnum.COMPLEXITY,
                        string.Format(" {0} changed", Utils.EnumToString(AppEnums.LogTypeEnum.COMPLEXITY.ToString(), true)),
                         oldtask.ComplexityDesc, newtask.ComplexityDesc, newtask.task_id);
                    CreateLog(log);
                }

                //due date
                if (oldtask.due_date!= newtask.due_date)
                {
                    log log = new log(AppEnums.LogOperationEnum.UPDATE, AppEnums.LogTypeEnum.DATE,
                        string.Format(" {0} changed", Utils.EnumToString(AppEnums.LogTypeEnum.DATE.ToString(), true)),
                        oldtask.due_date.HasValue? oldtask.due_date.Value.ToString("dd-MM-yyyy"): string.Empty,
                        newtask.due_date.HasValue ? newtask.due_date.Value.ToString("dd-MM-yyyy") : string.Empty
                        , newtask.task_id);
                    CreateLog(log);
                }

                //owner

                if (oldtask.owner != newtask.owner)
                {
                    log log = new log(AppEnums.LogOperationEnum.UPDATE, AppEnums.LogTypeEnum.OWNER,
                        string.Format(" {0} changed", Utils.EnumToString(AppEnums.LogTypeEnum.OWNER.ToString(), true)),
                         oldtask.owner.ToString(), newtask.owner.ToString(), newtask.task_id);
                    CreateLog(log);
                }



            }
            catch (Exception e)
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
        public ActionResult NotifyUsers(int taskId, int projectId, int[] notify, int SendTo=0)
        {

            try
            {
                

                List<EmailRecipient> recipients = new List<EmailRecipient>();
                var task = db.tasks.Find(taskId);

                if(SendTo > 0)
                {

                    int pos = Array.IndexOf(notify, SendTo);
                    if (pos < 0)
                    {
                        int newLength = notify.Length + 1;

                        int[] result = new int[newLength];

                        for (int i = 0; i < notify.Length; i++)
                            result[i] = notify[i];

                        result[newLength - 1] = SendTo;

                        //swap back results
                        notify = result;
                        // the array contains the string and the pos variable
                        // will have its position in the array
                    }

                }

                task.HiUser = "Team";

                for (int i = 0; i < notify.Count(); i++)
                {
                
                    var user = db.users.Find(notify[i]);
                    

                    var assigned_user = task.assigned_to.HasValue ? task.assigned_to.Value:  0;
                    if (user != null && !string.IsNullOrWhiteSpace(user.email))
                    {
                        var isTo = false;
                        
                        if(user.user_id == assigned_user || notify.Count() == 1 )
                        {

                            if(SendTo == 0)
                            {
                                isTo = true;
                                task.HiUser = user.firstname;
                            }
                            
                        }

                        if (SendTo == user.user_id)
                        {
                            isTo = true;
                            var userTo = db.users.Find(SendTo);
                            task.HiUser = userTo.firstname;
                        }


                        recipients.Add(new EmailRecipient { RecipientEmail = user.email, RecipientId = user.user_id, RecipientUserId = user.username, RecipientName = user.fullname , isTo = isTo });

                    }

                }



                // in case no recient is explicityly chosen, set it to the UserTo (either assigned user or default user if there is only one user_
                if (SendTo == 0)
                {
                    var sendtolist = recipients.Where(x => x.isTo).ToList();

                    if (sendtolist.Count > 0)
                    {
                        SendTo = sendtolist.FirstOrDefault().RecipientId;
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

                    //sender
                    //var sender = db.users.Find(UserSession.Current.UserId);

                    var senderid = UserSession.Current.UserId;
                    var sender = db.users.Find(senderid);
                    mail.AddBccRecipient(sender.email);

                    

                    //Generates the email body
                         mail.Body = RenderRazorViewToString(this.ControllerContext, "_notificationBody", task);

                         var subj = string.Format("CatchMe! #{0} - {1}", task.task_id, task.title);


                

                    MailAddress senderAddress = new MailAddress(sender.email);
                
                
                        mail.SenderMail = senderAddress;


                        if (ConfigurationHelper.SendEmail())
                        {
                            mail.SendMail(subj, mail.Body, true);
                        }

                StringBuilder _recipients = new StringBuilder();
                

                foreach(var r in recipients)
                {
                    _recipients.AppendFormat("{0};", r.RecipientName);
                    

                }


                var _recipient_ids = string.Join(";", notify);

                if(_recipients.Length > 1)
                {
                    _recipients.Length--;                    
                }

                notification notif = new notification { sender_id = UserSession.Current.UserId, sender_name = UserSession.Current.Fullname, task_id = taskId, sent_on = DateTime.Now, 
                    recipients = _recipients.ToString() , recipients_id = _recipient_ids.ToString(), send_to_id = SendTo};

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

                    log log = new log(AppEnums.LogOperationEnum.UPDATE, AppEnums.LogTypeEnum.COMMENT, string.Format(" Comment Edited: {0}-{1}",comment.comment_id, comment.title), comment.task_id);
                    CreateLog(log);

                    updateTask(comment.task_id);
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

               log log = new log(AppEnums.LogOperationEnum.DELETE, AppEnums.LogTypeEnum.COMMENT, string.Concat("Comment Deleted: ",comment.comment_id, "-", comment.description), comment.task_id);
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
