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

                    if (value.Length > 1 && value.Contains(">"))
                    {
                        string[] vals = value.Split('>');

                        if (vals.Length == 2)
                        {

                            int one = 0;
                            int two = 0;
                            Int32.TryParse(vals[0], out one);
                            Int32.TryParse(vals[1], out two);
                            
                            var oldval = AppEnums.DescEnum(Enum.ToObject(type, one).ToString(), false);
                            var newval = AppEnums.DescEnum(Enum.ToObject(type, two).ToString(), false);

                            return string.Concat(oldval, " > ", newval);

                        }

                    }


                    return AppEnums.DescEnum(Enum.ToObject(type, Convert.ToInt32(value)).ToString(), false);
                }
                else
                {
                    return string.Empty;
                }
     
    }
    public string HistDesc
    {
        get
        {
            if (!String.IsNullOrWhiteSpace(StatusDesc))
            {
                if (StatusDesc.Contains(">"))
                {

                    return StatusDesc.Substring(StatusDesc.LastIndexOf('>') + 1);

                }
                else
                {
                    return StatusDesc;
                }
            } return String.Empty;
        }
    }


        public string StatusDesc
        {
            get
            {
                return GetDesc(status, typeof(AppEnums.StatusEnum));
            }

        }

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
                if (!string.IsNullOrWhiteSpace(status))
                {
                    
                    switch (status.Last().ToString())
                    {
                        case "0": return "info";
                        case "4":
                        case "9": return "default";
                        case "1":
                        case "2": return "warning";
                        case "3": return "primary";
                        case "6":
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

        public string StatusDescLabel
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(status))
                {
                    switch (status)
                    {
                        case "0": return "info";
                        case "4":
                        case "9": return "default";
                        case "1":
                        case "2": return "warning";
                        case "3": return "primary";
                        case "6":
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
                    
                    return GetDesc(test_status, typeof(AppEnums.TestStatusEnum));
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

                  return  GetDesc(priority, typeof(AppEnums.PriorityEnum));
                    
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
                    return GetDesc(complexity, typeof(AppEnums.SeverityComplexityEnum));
                    

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
                    return GetDesc(type, typeof(AppEnums.TypeEnum));
                    
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
                    return GetDesc(severity, typeof(AppEnums.SeverityComplexityEnum));
                    
                }
                else
                {
                    return string.Empty;
                }


            }

        }


    }
}