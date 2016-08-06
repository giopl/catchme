using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CatchMe.Models
{
    public class viewStatusMeta
    {
        public int project_id { get; set; }
        public int user_id { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public Nullable<int> newtask { get; set; }
        public Nullable<int> action { get; set; }
        public Nullable<int> investigation { get; set; }
        public Nullable<int> completed { get; set; }
        public Nullable<int> onhold { get; set; }
        public Nullable<int> problem { get; set; }
        public Nullable<int> no_issue { get; set; }
        public Nullable<int> passed { get; set; }
        public Nullable<int> failed { get; set; }
        public Nullable<int> total_open { get; set; }
        public Nullable<int> closed { get; set; }
        public Nullable<int> total { get; set; }

    }

    [MetadataType(typeof(viewStatusMeta))]
    public partial class viewStatus
    {

        public string FmtNum(int? val)
        {
            if(val.HasValue)
            {
                if(val.Value == 0)
                {
                    return string.Empty;
                  //  return string.Format("<span class='badge'>{0}</span>", val.Value);

                }
                if (val.Value > 0 && val.Value <5)
                {
                    return string.Format("<span class='badge new'>{0}</span>", val.Value);
                }

                if (val.Value >= 5 && val.Value < 10)
                {
                    return string.Format("<span class='badge action'>{0}</span>", val.Value);
                }
                if (val.Value >= 10)
                {
                    return string.Format("<span class='badge problem'>{0}</span>", val.Value);
                }
            }
                return string.Empty;

        }
    }

}