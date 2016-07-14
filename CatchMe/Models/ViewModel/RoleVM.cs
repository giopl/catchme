using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CatchMe.Models.ViewModel
{
    public class RoleVM
    {

        public RoleVM(int roleId, string n)
        {
            role_id = roleId;
            name = n;
        }


        public int role_id { get; set; }
        public string name { get; set; }
    }
}