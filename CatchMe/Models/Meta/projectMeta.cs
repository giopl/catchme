using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CatchMe.Models
{
    public class projectMeta
    {


        public int project_id { get; set; }

        [DisplayName("Project Name")]
        [Required]
        [StringLength(100)]
        public string name { get; set; }

        [DisplayName("Project Desc")]
        [StringLength(300)]
        [DataType(DataType.MultilineText)]
        public string description { get; set; }

        [DisplayName("Project Active")]
        public bool is_active { get; set; }

        [DisplayName("Project Tasks")]
        public virtual ICollection<task> tasks { get; set; }


        [DisplayName("Project Users")]
        public virtual ICollection<user> users { get; set; }

    }



    [MetadataType(typeof(projectMeta))]
    public partial class project
    {
 


    }



}