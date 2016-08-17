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
    
    public partial class user
    {
        public user()
        {
            this.task_user = new HashSet<taskUser>();
            this.projects = new HashSet<project>();
            this.logs = new HashSet<log>();
            this.tasks = new HashSet<task>();
            this.tasks1 = new HashSet<task>();
            this.tasks2 = new HashSet<task>();
            this.tasks3 = new HashSet<task>();
            this.project_user_role = new HashSet<projectUserRole>();
            this.comments = new HashSet<comment>();
            this.attachments = new HashSet<attachment>();
            this.information = new HashSet<information>();
            this.information1 = new HashSet<information>();
        }
    
        public int user_id { get; set; }
        public string username { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string job_title { get; set; }
        public string team { get; set; }
        public int role { get; set; }
        public Nullable<int> num_logins { get; set; }
        public bool is_active { get; set; }
        public string email { get; set; }
        public Nullable<int> active_project { get; set; }
        public string nickname { get; set; }
    
        public virtual ICollection<taskUser> task_user { get; set; }
        public virtual ICollection<project> projects { get; set; }
        public virtual ICollection<log> logs { get; set; }
        public virtual ICollection<task> tasks { get; set; }
        public virtual ICollection<task> tasks1 { get; set; }
        public virtual ICollection<task> tasks2 { get; set; }
        public virtual ICollection<task> tasks3 { get; set; }
        public virtual ICollection<projectUserRole> project_user_role { get; set; }
        public virtual ICollection<comment> comments { get; set; }
        public virtual ICollection<attachment> attachments { get; set; }
        public virtual ICollection<information> information { get; set; }
        public virtual ICollection<information> information1 { get; set; }
    }
}
