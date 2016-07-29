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
            this.notifications = new HashSet<notification>();
            this.task_hist = new HashSet<taskHist>();
            this.task_user = new HashSet<taskUser>();
            this.attachments = new HashSet<attachment>();
            this.logs = new HashSet<log>();
        }
    
        public int task_id { get; set; }
        public int project_id { get; set; }
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
        public Nullable<int> assigned_to { get; set; }
        public Nullable<System.DateTime> updated_on { get; set; }
        public int state { get; set; }
    
        public virtual ICollection<comment> comments { get; set; }
        public virtual ICollection<notification> notifications { get; set; }
        public virtual project project { get; set; }
        public virtual user user { get; set; }
        public virtual ICollection<taskHist> task_hist { get; set; }
        public virtual user user1 { get; set; }
        public virtual ICollection<taskUser> task_user { get; set; }
        public virtual ICollection<attachment> attachments { get; set; }
        public virtual ICollection<log> logs { get; set; }
    }
}
