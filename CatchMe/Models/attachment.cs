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
    
    public partial class attachment
    {
        public int attachment_id { get; set; }
        public int task_id { get; set; }
        public int user_id { get; set; }
        public string filename { get; set; }
        public string mimetype { get; set; }
        public Nullable<int> content_length { get; set; }
        public Nullable<System.DateTime> created_on { get; set; }
        public bool is_disabled { get; set; }
        public string filepath { get; set; }
    
        public virtual task task { get; set; }
        public virtual user user { get; set; }
    }
}