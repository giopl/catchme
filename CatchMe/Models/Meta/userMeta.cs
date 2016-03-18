using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CatchMe.Models
{
    public class userMeta
    {

        

        public int user_id { get; set; }


        [Required]
        [DisplayName("Username")]
        public string username { get; set; }

        [DisplayName("Firstname")]
        public string firstname { get; set; }

        [DisplayName("Lastname")]
        public string lastname { get; set; }
        public string job_title { get; set; }
        public string team { get; set; }
        public int role { get; set; }
        public Nullable<int> num_logins { get; set; }
        public bool is_active { get; set; }
        public string email { get; set; }
        public Nullable<int> active_project { get; set; }

        public virtual ICollection<comment> comments { get; set; }
        public virtual project project { get; set; }
        public virtual ICollection<task_user> task_user { get; set; }
        public virtual ICollection<log> logs { get; set; }


    }


    [MetadataType(typeof(userMeta))]
    public partial class user
    {
        public string fullname
        {
            get
            {
                return string.Concat(firstname, " ", lastname);
            }
        }

    
    
    }
}