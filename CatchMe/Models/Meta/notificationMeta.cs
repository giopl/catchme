using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CatchMe.Models
{
    public class notificationMeta
    {
        public string sender_name { get; set; }
    }



    [MetadataType(typeof(notificationMeta))]
    public partial class notification
    {
     
        

        public string sender_firstname
        {

            get
            {

                if (!string.IsNullOrWhiteSpace(sender_name))
                {

                    string[] names = sender_name.Split(' ');
                    if (names.Count() > 1)
                    {
                        return names[0];
                    }

                }

                return sender_name;

            }
        }
     
    }
}