using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CatchMe.Models
{
    public class commentMeta
    {

        public int comment_id { get; set; }
        public int task_id { get; set; }
        public int user_id { get; set; }

        [DisplayName("Title")]
        public string title { get; set; }

        [DisplayName("Is Internal")]
        public bool is_internal { get; set; }


        [AllowHtml]
        [DataType(DataType.MultilineText)]
        [DisplayName("Description")]
        public string description { get; set; }
        public Nullable<System.DateTime> created_on { get; set; }

        public virtual user user { get; set; }
        public virtual task task { get; set; }
    }




    [MetadataType(typeof(commentMeta))]
    public partial class comment
    {
    }


}