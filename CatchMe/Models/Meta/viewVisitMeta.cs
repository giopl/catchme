using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CatchMe.Models
{
    public class viewVisitMeta
    {
 
           public int user_id { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public Nullable<System.DateTime> lastlogin { get; set; }
    }


    [MetadataType(typeof(viewVisitMeta))]
     public partial class viewVisit
    {

        public int since
        {
            get
            {
                TimeSpan t = DateTime.Now - lastlogin.Value;
                var seconds = t.TotalSeconds;

                if (seconds <= 600)
                    return 0;

                if (seconds > 600 && seconds <= 1800)
                    return 1;

                return 2;


            }
        }


    }

}