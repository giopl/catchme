using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CatchMe.Models.ViewModel
{
    public class SearchFilter
    {
        [DisplayName("ID")]
        public Nullable<int> id { get; set; }

        [DisplayName("Status")]
        public Nullable<int> status { get; set; }


        [DisplayName("Priority")]
        public Nullable<int> priority { get; set; }


        [DisplayName("Type")]
        public Nullable<int> type { get; set; }

        [DisplayName("Created By")]
        public Nullable<int> createdBy { get; set; }

        [DisplayName("Owner")]
        public Nullable<int> owner { get; set; }

        [DisplayName("AssignedTo")]
        public Nullable<int> assignedTo { get; set; }

        [DisplayName("Title")]
        public string title { get; set; }

        [DisplayName("Keywords")]
        public string keywords { get; set; }



        [DisplayName("Created Min Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}",
               ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> createdOnMin { get; set; }

        [DisplayName("Created Max Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}",
               ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> createdOnMax { get; set; }


        [DisplayName("Due Min Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}",
              ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> dueOnMin { get; set; }

        [DisplayName("Due Max Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}",
               ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> dueOnMax { get; set; }

        

    }
}