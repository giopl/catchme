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
    
    public partial class projectUser
    {
        public int user_id { get; set; }
        public int project_id { get; set; }
        public Nullable<int> role { get; set; }
    
        public virtual project project { get; set; }
        public virtual user user { get; set; }
    }
}
