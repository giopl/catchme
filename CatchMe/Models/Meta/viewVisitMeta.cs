using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
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


        public string LastUpdatedSince
        {

            get
            {
                StringBuilder result = new StringBuilder();
                if (lastlogin.HasValue)
                {



                    TimeSpan t = DateTime.Now - lastlogin.Value;

                    var seconds = t.TotalSeconds;
                    var minutes = t.TotalMinutes;
                    var hours = t.TotalHours;
                    var days = t.TotalDays;


                    if (seconds < 60.0)
                    {
                        result.AppendFormat("<span class='label label-danger'><b>{0}</b>s ago</label>", (int)seconds);
                    }

                    if (seconds >= 60.0 && minutes < 60)
                    {
                        result.AppendFormat("<span class='label label-danger'><b>{0}</b>m ago</label>", (int)minutes);
                    }

                    if (minutes >= 60.0 && hours < 24)
                    {
                        result.AppendFormat("<span class='label label-warning'><b>{0}</b>h ago</label>", (int)hours);
                    }
                    if (hours >= 24.0 && days <= 5)
                    {
                        result.AppendFormat("<span class='label label-primary'><b>{0}</b> day(s) ago</label>", (int)days);
                    }
                }
                return result.ToString();

            }


        }



    }

}