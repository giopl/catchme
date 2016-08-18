using CatchMe.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CatchMe.Models
{
    public class informationMeta
    {
        public int information_id { get; set; }
        public int project_id { get; set; }
        public string title { get; set; }

        [AllowHtml]
        public string description { get; set; }
        public Nullable<int> importance { get; set; }
        public Nullable<int> created_by { get; set; }
        public Nullable<System.DateTime> created_on { get; set; }
        public int updated_by { get; set; }
        public Nullable<System.DateTime> updated_on { get; set; }
        public int state { get; set; }

        public virtual project project { get; set; }
        
        //updated  by
        public virtual user user { get; set; }

        //created by
        public virtual user user1 { get; set; }

    }


    [MetadataType(typeof(informationMeta))]
    public partial class information
    {

        public bool HasAttachment { 
            get
            {
                return attachments.Count > 0;
            }
          }

        public string ImportanceDesc
        {
            get
            {

                if (importance.HasValue)
                {

                    return AppEnums.DescEnum(Enum.ToObject(typeof(AppEnums.SeverityComplexityEnum), importance).ToString(), false);
                }
                else
                {
                    return string.Empty;
                }

            }

        }


    }
}