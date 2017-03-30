
using CatchMe.Models;
using CatchMe.Models.ViewModel;
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
            CurrentProjectRole = 0;
            MyProjects = null;
            ImpersonatedUser = string.Empty;
            SearchFilter searchFilter = new SearchFilter();
            Browser = string.Empty;
            HasInformational = false;

            
        }

        public static int count { get; set; }

        public SearchFilter searchFilter { get; set; }

        public string Browser { get; set; }

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
                catch (Exception e)
                {

                    throw;
                }

            }
        }

        //public bool IsTeamLeader { get; set; }

        public IList<project> MyProjects { get; set; }

        public string ImpersonatedUser { get; set; }
        public bool IsValid { get; set; }
        public bool ReadOnlyMode { get; set; }
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Fullname { get; set; }
        public string Firstname { get; set; }
        public string CurrentProject { get; set; }
        public int CurrentProjectRole { get; set; }

        public bool HasInformational { get; set; }


        public string UserImage { get; set; }
        public bool IsProjectLead {
            get
            {
                return CurrentProjectRole == 2;
            }
         }

        public bool IsBusiness
        {
            get
            {
                return CurrentProjectRole == 1;
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

        public int ProjectRole { get; set; }

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