using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CatchMe.Helpers
{

    public class UserSession
    {

        private UserSession()
        {
         
            IsValid = false;
            Username = string.Empty;
            Fullname = string.Empty;
            Firstname = string.Empty;
            ReadOnlyMode = false;
            UserId = 0;
            Role = 0;

        }



        public static UserSession Current
        {
            get
            {
                try
                {
                    UserSession session = (UserSession)HttpContext.Current.Session["__MySession__"];
                    if (session == null)
                    {
                        session = new UserSession();
                        HttpContext.Current.Session["__MySession__"] = session;
                    }
                    return session;
                }
                catch (Exception)
                {

                    throw;
                }

            }
        }

        //public bool IsTeamLeader { get; set; }
        public bool IsValid { get; set; }
        public bool ReadOnlyMode { get; set; }
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Fullname { get; set; }
        public string Firstname { get; set; }
        
        public string UserImage { get; set; }


        public int Role { get; set; }

        /// <summary>
        /// Clears the current session.
        /// </summary>
        public void ClearSession()
        {
            IsValid = false;
            HttpContext.Current.Session["__MySession__"] = null;
            HttpContext.Current.Session.Clear();
            HttpContext.Current.Session.Abandon();
        }



    }

}