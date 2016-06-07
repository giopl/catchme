using CatchMe.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CatchMe.Models
{
    public class taskHistMeta
    {
        public int task_hist_id { get; set; }
        public int task_id { get; set; }
        public Nullable<int> project_id { get; set; }
        public Nullable<int> status { get; set; }
        public Nullable<int> test_status { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string initiator { get; set; }
        public Nullable<int> complexity { get; set; }
        public Nullable<System.DateTime> due_date { get; set; }
        public Nullable<int> type { get; set; }
        public Nullable<int> severity { get; set; }
        public Nullable<int> priority { get; set; }
        public Nullable<int> created_by { get; set; }
        public Nullable<System.DateTime> created_on { get; set; }


    }


    [MetadataType(typeof(taskHistMeta))]
    public partial class taskHist
    {
        public string StatusDesc
        {
            get
            {

                if (status.HasValue)
                {

                    return AppEnums.DescEnum(Enum.ToObject(typeof(AppEnums.StatusEnum), status).ToString(), false);
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

                    return AppEnums.DescEnum(Enum.ToObject(typeof(AppEnums.TestStatusEnum), test_status).ToString(), false);
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


    }
}