using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CatchMe.Helpers
{
    public class EmailRecipient
    {

        public int RecipientId { get; set; }

        public string RecipientUserId { get; set; }
        public string RecipientName { get; set; }
        public string RecipientEmail { get; set; }
        public string SendType { get; set; }

        public int DisplayOrder { get; set; }

        public bool IsActive { get; set; }

        //used in view for saving object (insert or update)
        public string Action { get; set; }

        public bool isTo { get; set; }
        public bool isCc { get; set; }


        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            var t = obj as EmailRecipient;
            if (t == null)
                return false;
            if (RecipientEmail == t.RecipientEmail)
                return true;
            return false;
        }

        public override int GetHashCode()
        {
            int hash = 13;
            hash += (hash * 31) + RecipientEmail.GetHashCode();

            return hash;

        }
    }
}