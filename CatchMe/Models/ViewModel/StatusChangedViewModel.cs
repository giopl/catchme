using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CatchMe.Models.ViewModel
{
    public class StatusChangedViewModel
    {

        public string title { get; set; }

        [AllowHtml]
        public string comment { get; set; }

        public int taskId { get; set; }

        public int status { get; set; }

        public int assigned_to { get; set; }
    }
}