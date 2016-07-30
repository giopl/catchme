using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CatchMe.Models
{
    public class viewHistoryMeta
    {
        public int log_id { get; set; }
        public int user_id { get; set; }
        public string username { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public int task_id { get; set; }
        public string operation { get; set; }
        public string type { get; set; }
        public string description { get; set; }
        public Nullable<System.DateTime> logtime { get; set; }
        public string old_val { get; set; }
        public string new_val { get; set; }
        public int StatusChange { get; set; }
    }


    [MetadataType(typeof(viewHistoryMeta))]
    public partial class viewHistory
    {


        /*
            0-New
            1-Action
            2-Investigation
            3-Completed
            4-On Hold
            5-Problem
            6-No Issue
            7-Passed
            8-Failed
            9-Closed
         */

        public string HistDescLabel
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(new_val))
                {

                    switch (new_val.Trim().ToLower())
                    {
                        case "new": return "info";
                        case "on hold":
                        case "closed": return "default";
                        case "action":
                        case "investigation": return "warning";
                        case "completed": return "primary";
                        case "no issue":
                        case "passed": return "success";
                        case "problem":
                        case "failed": return "danger";


                        default:
                            break;
                    }

                }
                return "default";

            }
        }

        public string typeIcon
        {
            get
            {
                switch (type.Trim().ToLower())
                {
                    case "assignee": return "fa fa-hand-o-right";
                    case "attachment": return "fa fa-paperclip";
                    case "comment": return "fa fa-comment";
                    case "complexity": return "fa fa-cogs";
                    case "date": return "fa fa-calendar";
                    case "priority": return "fa fa-sort-amount-asc";
                    case "status": return "fa fa-info-circle";
                    case "task": return "fa fa-tasks";
                    case "type": return "fa fa-flask";
                    case "mail": return "fa fa-envelope";



                    default:
                        break;
                }

                return string.Empty;
            }
        }

        public string htmlDesc
        {
            get
            {
                if(type=="TASK" && operation=="CREATE")
                {
                    return string.Format("<b>{0}</b> <span class='label label-info'>created</span>Task", firstname);
                }


                if (type == "TASK" && operation == "DELETE")
                {
                    return string.Format("<b>{0}</b> <span class='label label-danger'>deleted</span> task", firstname);
                }


                if (type == "TASK" && operation == "RECOVER")
                {
                    return string.Format("<b>{0}</b> <span class='label label-info'>recovered</span> task", firstname);
                }


                if ((type == "PRIORITY" || type == "COMPLEXITY" || type == "SEVERITY" || type == "TYPE") && operation == "UPDATE")
                {
                    return string.Format("<span class='label label-info'>{0}</span> updated from <i>{1}</i> to <b>{2}</b>", type, old_val, new_val);
                }


                if (type == "DATE"  && operation == "UPDATE")
                {
                    return string.Format("<span class='label label-info'>{0}</span> updated from <i>{1}</i> to <b>{2}</b>", type, old_val, new_val);
                }

                if (type == "COMMENT" && operation == "UPDATE")
                {
                    return string.Format("<span class='label label-info'>{0}</span> updated", type);
                }


                if (type == "COMMENT" && operation == "CREATE")
                {
                    return string.Format("<span class='label label-info'>{0}</span> added", type);
                }


                if (type == "COMMENT" && operation == "DELETE")
                {
                    return string.Format("<span class='label label-info'>{0}</span> deleted", type);
                }

                if (type == "ATTACHMENT" && operation == "CREATE")
                {
                    return string.Format("<span class='label label-info'>{0}</span> {1} added", type, description);
                }

                if (type == "ATTACHMENT" && operation == "DELETE")
                {
                    return string.Format("<span class='label label-info'>{0}</span> {1} deleted", type, description);
                }

                if (type == "ASSIGNEE" && operation == "UPDATE")
                {
                    return string.Format("Task assigned to {0}", fname_new);
                }

                if (type == "OWNER" && operation == "UPDATE")
                {
                    return string.Format("Task ownership given to {0}", fname_new);
                }


                if (operation == "NOTIFY")
                {
                    return string.Format("<b>Notification</b> sent by {0}", firstname);
                }



                if (type=="STATUS" && operation=="UPDATE")
                {
                    if(new_val.ToLower()=="new")
                    {
                        return string.Format("Task reset to <b>new</b>");
                    }

                    if (new_val.ToLower() == "action")
                    {
                        return string.Format("<span class='label label-warning'>Development</span> in progress");
                    }

                    if (new_val.ToLower() == "closed")
                    {
                        return string.Format("Task <span class='label label-default'>closed</span>");
                    }

                    if (new_val.ToLower() == "completed")
                    {
                        return string.Format("Task <span class='label label-primary'>Completed</span>");
                    }


                    if (new_val.ToLower() == "investigation")
                    {
                        return string.Format("<span class='label label-warning'>Investigation</span> in progress");
                    }

                    if (new_val.ToLower() == "failed")
                    {
                        return string.Format("Test <span class='label label-danger'>Failed</span>");
                    }

                    if (new_val.ToLower() == "passed")
                    {
                        return string.Format("Test <span class='label label-success'>Passed</span>");
                    }

                    if (new_val.ToLower() == "no issue")
                    {
                        return string.Format("<span class='label label-success'>No Issue</span>found");
                    }

                    if (new_val.ToLower() == "on hold")
                    {
                        return string.Format("Task placed <span class='label label-default'>On Hold</span>");
                    }

                    if (new_val.ToLower() == "problem")
                    {
                        return string.Format("Task changed to <span class='label label-danger'>Problem</span>");
                    }

                }


                return string.Empty;
            }

        }

    }

}