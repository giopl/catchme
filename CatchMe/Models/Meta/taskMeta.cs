using CatchMe.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CatchMe.Models
{
    public class taskMeta    
    {

        

        [DisplayName("Task Id")]
        public int task_id { get; set; }

        [DisplayName("Projet")]
        public int project_id { get; set; }

        [DisplayName("Status")]
        public Nullable<int> status { get; set; }

        [DisplayName("Testing")]
        public Nullable<int> test_status { get; set; }

        [DisplayName("Title")]
        public string title { get; set; }

        [DisplayName("Description")]
        public string description { get; set; }

        [DisplayName("Created By")]
        public string initiator { get; set; }


        [DisplayName("Complexity")]
        public Nullable<int> complexity { get; set; }

        [DisplayName("Due Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}",
               ApplyFormatInEditMode = true)]
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


        public string StatusDescLabel
        {
            get
            {
                if (status.HasValue)
                {
                    switch (status.Value)
                    {
                        case 0:
                        case 6: return "default";
                        case 1:
                        case 2: return "primary";
                        case 3: return "warning";
                        case 4:
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


        public string TestStatusDesc
        {
            get
            {

                if (test_status.HasValue)
                {

                return AppEnums.DescEnum(Enum.ToObject(typeof(AppEnums.StatusEnum), test_status).ToString(), false);
                }
                else
                {
                    return string.Empty;
                }


            }

        }


        public string PriorityDesc
        {
            get
            {
                if (priority.HasValue)
                {

                return AppEnums.DescEnum(Enum.ToObject(typeof(AppEnums.StatusEnum), priority).ToString(), false);
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
                return AppEnums.DescEnum(Enum.ToObject(typeof(AppEnums.StatusEnum), complexity).ToString(), false);

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

                return AppEnums.DescEnum(Enum.ToObject(typeof(AppEnums.StatusEnum), type).ToString(), false);
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

                return AppEnums.DescEnum(Enum.ToObject(typeof(AppEnums.StatusEnum), severity).ToString(), false);
                }
                else
                {
                    return string.Empty;
                }


            }

        }



    }

}