using CatchMe.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CatchMe.Models
{
    public class logMeta
    {
        public int log_id { get; set; }
        public int user_id { get; set; }
        public int task_id { get; set; }
        public string operation { get; set; }
        public string type { get; set; }
        public Nullable<System.DateTime> logtime { get; set; }
        public string description { get; set; }

        public virtual task task { get; set; }
        public virtual user user { get; set; }
    }




    [MetadataType(typeof(logMeta))]
    public partial class log
    {


        public log()
        { 
        }
        public log(AppEnums.LogOperationEnum operation, AppEnums.LogTypeEnum type,  string description, int task_id = 0)
        {

            this.user_id = UserSession.Current.UserId;
            this.task_id = task_id;
            this.operation = operation.ToString();
            this.type = type.ToString();
            this.description = description;
            this.logtime = DateTime.Now;
        
        }

    }

}