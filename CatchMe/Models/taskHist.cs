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
    
    public partial class taskHist
    {
        public int task_hist_id { get; set; }
        public int task_id { get; set; }
        public Nullable<int> project_id { get; set; }
        public string status { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string initiator { get; set; }
        public string complexity { get; set; }
        public string due_date { get; set; }
        public string type { get; set; }
        public string severity { get; set; }
        public string priority { get; set; }
        public Nullable<int> created_by { get; set; }
        public Nullable<System.DateTime> created_on { get; set; }
        public Nullable<int> hist_status { get; set; }
        public string firstname { get; set; }
        public string fullname { get; set; }
    
        public virtual task task { get; set; }
    }
}
