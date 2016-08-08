using CatchMe.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace CatchMe.Models
{
    public class taskMeta    
    {

        

        [DisplayName("Task Id")]
        public int task_id { get; set; }

        /* user */
        [DisplayName("Assigned To")]
        public int assigned_to { get; set; }

        /* user3 */
        [DisplayName("Created By")]
        public Nullable<int> created_by { get; set; }

        /* user 1 */
        [DisplayName("Owner")]
        public int owner { get; set; }

        /* user 2 */
        [DisplayName("Updated By")]
        public int updated_by { get; set; }


        [DisplayName("Projet")]
        public int project_id { get; set; }

        [DisplayName("Status")]
        public Nullable<int> status { get; set; }

        //[DisplayName("Testing")]
        //public Nullable<int> test_status { get; set; }

        [DisplayName("Title")]
        [Required]
        public string title { get; set; }

        [AllowHtml]
        [DataType(DataType.MultilineText)]
        [DisplayName("Description")]
        public string description { get; set; }

        [DisplayName("Initiator")]
        public string initiator { get; set; }


        [DisplayName("Complexity")]
        public Nullable<int> complexity { get; set; }

        [DisplayName("Due Date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}",ApplyFormatInEditMode = true)]
        [Required]
        public Nullable<System.DateTime> due_date { get; set; }


        [DisplayName("Type")]
        public Nullable<int> type { get; set; }


        [DisplayName("Severity")]
        public Nullable<int> severity { get; set; }

        [DisplayName("Priority")]
        public Nullable<int> priority { get; set; }



    }



    [MetadataType(typeof(taskMeta))]
    public partial class task
    {

        public string LastUpdatedSince
        {

            get
            {
                try
                {
                    if (updated_on.HasValue)
                    {
                        TimeSpan t = DateTime.Now - updated_on.Value;
                        return timeDifferenceLabel(t);
                    }

                    return String.Empty;
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }


        public string OpenSince
        {

            get
            {
                try
                {
                    if (created_on.HasValue)
                    {
                        TimeSpan t = DateTime.Now - created_on.Value;
                        return timeDifferenceLabel(t);
                    }

                    return String.Empty;
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }

        public string TimeTaken
        {

            get
            {
                try
                {

                
                if (created_on.HasValue && updated_on.HasValue)
                {
                    TimeSpan t = updated_on.Value - created_on.Value;
                    return timeDifferenceLabel(t);
                }

                return String.Empty;
                }
                catch (StackOverflowException e)
                {

                    throw;
                }
            }
        }


        public bool isInNotificationList { get; set; }

        private string timeDifferenceLabel(TimeSpan timespan)
        {
            try
            {

            var seconds = timespan.TotalSeconds;
            var minutes = timespan.TotalMinutes;
            var hours = timespan.TotalHours;
            var days = timespan.TotalDays;

            StringBuilder result = new StringBuilder();

            if (seconds < 60.0)
            {
                result.AppendFormat("<span class='label label-danger'><b>{0}</b> sec{1}</label>", (int)seconds,seconds>1?"s":"");
            }

            if (seconds >= 60.0 && minutes < 60)
            {
                result.AppendFormat("<span class='label label-danger'><b>{0}</b> min{1}</label>", (int)minutes, minutes>1?"s":"");
            }

            if (minutes >= 60.0 && hours < 24)
            {
                result.AppendFormat("<span class='label label-warning'><b>{0}</b> hour{1}</label>", (int)hours, hours>1?"s":"");
            }
            if (hours >= 24.0 && days <= 5)
            {
                result.AppendFormat("<span class='label label-primary'><b>{0}</b> day{1}</label>", (int)days, days>1?"s":"");
            }

            if (days > 5 && days < 365)
            {
                result.AppendFormat("<span class='label label-default'><b>{0}</b> day{1}</label>", (int)days, days > 1 ? "s" : "");
            }


            return result.ToString();
            }
            catch (Exception e)
            {

                throw;
            }


        }



        public bool IsFilteredOn { get; set; }

        public IList<viewHistory> History { get; set; }

        /// <summary>
        /// http://stackoverflow.com/questions/29482/cast-int-to-enum-in-c-sharp
        /// </summary>
        public string StatusDesc 
        { 
            get 
            {

                if (status.HasValue)
                {

                return AppEnums.DescEnum(Enum.ToObject(typeof(AppEnums.StatusEnum), status).ToString(),false);
                }
                else
                {
                    return string.Empty;
                }

            }
        
        }

        public bool HasAttachment
        {
            get
            {
                return attachments.Count > 0;
            }
        }

        public bool IsDeleted { get
            {
                return state == 1;
            }
        }

        public bool IsArchived
        {
            get
            {
                return state == 2;
            }
        }

        public bool IsOverdue
        {
            get
            {
                if (due_date.HasValue)
                {
                    return status < 9 && DateTime.Now.AddDays(-1) > due_date.Value;

                }
                else return false;
            }
        }

        public string HiUser { get;set; }

        public string typeIcon
        {


            get
            {
                switch (type)
                {
                    case 1: return "wrench";
                        case 2: return "cogs";
                        case 3: return "bug";
                        case 4: return "exclamation";
                        case 5: return "thumbs-up";
                        case 6: return "search";
                        
                    default:
                        break;
                }
                return string.Empty;

            }
        }

        public string statusIcon
        {


            get
            {
                switch (status)
                {
                    case 0: return "asterisk";
                    case 1: return "play";
                    case 2: return "search";
                    case 3: return "check-circle";
                    case 4: return "pause";
                    case 5: return "bomb";
                    case 6: return "smile-o";                    
                    case 7: return "trophy";
                    case 8: return "warning";
                    case 9: return "folder";

                    default:
                        break;
                }
                return string.Empty;

            }
        }



        public string StateColor
        {
            get
            {
                if (status.HasValue)
                {

                    var StateColor = AppEnums.DescEnum(Enum.ToObject(typeof(AppEnums.StatusEnum), status).ToString(), false);
                    return StateColor.Replace(" ","").Trim().ToLower();
                }
                else
                {
                    return "none";
                }
            }

        }

        public bool IsReadonly
        {

            get
            {
                var result = false;
                if(status.HasValue)
                {
                    result = status.Value == 0 || status.Value == 4 || status.Value == 9;
                }
                    /*
                     * 
            New = 0,
            Action,
            Investigation,
            Completed,
            On_Hold,
            Problem,
            No_Issue,
            Passed,
            Failed,
            Closed
                     */
                return result;
            }
        }
        public string StatusDescLabel
        {
            get
            {
                if (status.HasValue)
                {
                    switch (status.Value)
                    {
                        case 0: return "info";
                        case 4:
                        case 9: return "default";
                        case 1:
                        case 2: return "warning";
                        case 3: return "primary";
                        case 6:
                        case 7: return "success";
                        case 5:
                        case 8: return "danger";

                        default:
                            break;
                    }

                }
                    return "default";



            }
        }


        //public string TestStatusDesc
        //{
        //    get
        //    {

        //        if (test_status.HasValue)
        //        {

        //        return AppEnums.DescEnum(Enum.ToObject(typeof(AppEnums.TestStatusEnum), test_status).ToString(), false);
        //        }
        //        else
        //        {
        //            return string.Empty;
        //        }


        //    }

        //}


        public string PriorityDesc
        {
            get
            {
                if (priority.HasValue)
                {

                return AppEnums.DescEnum(Enum.ToObject(typeof(AppEnums.PriorityEnum), priority).ToString(), false);
                }
                else
                {
                    return string.Empty;
                }


            }

        }

        public string ComplexityDesc
        {
            get
            {
                if (complexity.HasValue)
                {
                return AppEnums.DescEnum(Enum.ToObject(typeof(AppEnums.SeverityComplexityEnum), complexity).ToString(), false);

                }
                else
                {
                    return string.Empty;
                }


            }

        }

        public string TypeDesc
        {
            get
            {
                if (type.HasValue)
                {

                return AppEnums.DescEnum(Enum.ToObject(typeof(AppEnums.TypeEnum), type).ToString(), false);
                }
                else
                {
                    return string.Empty;
                }


            }

        }

        public string SeverityDesc
        {
            get
            {
                if (severity.HasValue)
                {

                return AppEnums.DescEnum(Enum.ToObject(typeof(AppEnums.SeverityComplexityEnum), severity).ToString(), false);
                }
                else
                {
                    return string.Empty;
                }


            }

        }





        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            var t = obj as task;
            if (t == null)
                return false;
            if (task_id == t.task_id)
                return true;
            return false;
        }

        public override int GetHashCode()
        {
            int hash = 13;
            hash += (hash * 43) + task_id.GetHashCode();

            return hash;

        }


    }

}