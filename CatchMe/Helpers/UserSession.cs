
using CatchMe.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
            CurrentProject = string.Empty;
            CurrentProjectId = 0;
            MyProjects = null;
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

        public IList<project> MyProjects { get; set; }

        public bool IsValid { get; set; }
        public bool ReadOnlyMode { get; set; }
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Fullname { get; set; }
        public string Firstname { get; set; }
        public string CurrentProject { get; set; }
        
        public string UserImage { get; set; }
        public bool IsProjectLead {
            get
            {
                return Role == 2;
            }
                }

        public int Role { get; set; }

        public bool IsAdmin {

            get
            {
                return Role == 9;
            }
            
            }
        public int CurrentProjectId { get; set; }

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