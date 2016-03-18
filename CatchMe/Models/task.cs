//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CatchMe.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class task
    {
        public task()
        {
            this.comments = new HashSet<comment>();
            this.logs = new HashSet<log>();
            this.task_user = new HashSet<task_user>();
            this.categories = new HashSet<category>();
        }
    
        public int task_id { get; set; }
        public int project_id { get; set; }
        public Nullable<int> status { get; set; }
        public Nullable<int> test_status { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string creator { get; set; }
        public Nullable<int> complexity { get; set; }
        public Nullable<System.DateTime> due_date { get; set; }
        public Nullable<int> type { get; set; }
        public Nullable<int> severity { get; set; }
    
        public virtual ICollection<comment> comments { get; set; }
        public virtual project project { get; set; }
        public virtual ICollection<log> logs { get; set; }
        public virtual ICollection<task_user> task_user { get; set; }
        public virtual ICollection<category> categories { get; set; }
    }
}
