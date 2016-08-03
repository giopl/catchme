using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CatchMe.Models
{
    public class projectUserRoleMeta
    {
        public int user_id { get; set; }
        public int project_id { get; set; }
        public int role { get; set; }

        public virtual project project { get; set; }
        public virtual user user { get; set; }
    }


    [MetadataType(typeof(projectUserRoleMeta))]
    public partial class projectUserRole
    {
     
        public string roleDesc
        {
            get
            {
                switch (role)
                {
                    case 0: return "Developer";
                    case 1: return "Business";
                    case 3: return "Team Leader";
                    default:
                        return "Developer";

                }
                
            }
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            var t = obj as projectUserRole;
            if (t == null)
                return false;
            if (user_id == t.user_id && project_id == t.project_id)
                return true;
            return false;
        }

        public override int GetHashCode()
        {
            int hash = 13;
            hash += (hash * 43) + user_id.GetHashCode() + project_id.GetHashCode();

            return hash;

        }

    }
}