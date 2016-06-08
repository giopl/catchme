using CatchMe.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CatchMe.Models
{
    public class taskHistMeta
    {
        public int task_hist_id { get; set; }
        public int task_id { get; set; }
        public String project_id { get; set; }
        public String status { get; set; }
        public String test_status { get; set; }
        public string title { get; set; }


        [AllowHtml]
        [DataType(DataType.MultilineText)]
        public string description { get; set; }
        public string initiator { get; set; }
        public String complexity { get; set; }
        public String due_date { get; set; }
        public String type { get; set; }
        public String severity { get; set; }
        public String priority { get; set; }
        public Nullable<int> created_by { get; set; }
        public Nullable<System.DateTime> created_on { get; set; }


    }




    [MetadataType(typeof(taskHistMeta))]
    public partial class taskHist
    {

        
    private string GetDesc(string value, System.Type type)
    {

          if (!string.IsNullOrWhiteSpace(value))
                {

                    return AppEnums.DescEnum(Enum.ToObject(type, test_status).ToString(), false);
                }
                else
                {
                    return string.Empty;
                }
     
    }



        public string StatusDesc
        {
            get
            {
                return GetDesc(status, typeof(AppEnums.StatusEnum));
            }

        }


        public string StatusDescLabel
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(status))
                {
                    switch (status)
                    {
                        case "0":
                        case "6": return "default";
                        case "1":
                        case "2": return "primary";
                        case "3": return "warning";
                        case "4":
                        case "7": return "success";
                        case "5":
                        case "8": return "danger";


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

                if (!string.IsNullOrWhiteSpace(test_status))
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
                if (!string.IsNullOrWhiteSpace(priority))
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
                if (!string.IsNullOrWhiteSpace(complexity))
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
                if (!string.IsNullOrWhiteSpace(type))
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
                if (!string.IsNullOrWhiteSpace(severity))
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