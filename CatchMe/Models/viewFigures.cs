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
    
    public partial class viewFigures
    {
        public int project_id { get; set; }
        public Nullable<int> total { get; set; }
        public Nullable<int> open { get; set; }
        public Nullable<int> overdue { get; set; }
        public Nullable<int> closed_ontime { get; set; }
        public Nullable<int> closed_late { get; set; }
    }
}