using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CatchMe.Models
{
    public class OptionItem
    {
        public string name { get; set; }
        public int value { get; set; }



        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            var t = obj as OptionItem;
            if (t == null)
                return false;
            if (value == t.value)
                return true;
            return false;
        }

        public override int GetHashCode()
        {
            int hash = 13;
            hash += (hash * 43) + value.GetHashCode();

            return hash;

        }

    }
}