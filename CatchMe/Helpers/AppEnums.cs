using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CatchMe.Helpers
{
    public static class AppEnums
    {

        #region cacheEnums
        public enum CacheExpiration
        {
            Midnight = 0,
            Fix = 1,
            Sliding = 2,
            In_2_Hours = 3

        }

        #endregion


        public enum StatusEnum
        {
            None = 0,
            New,
            Open,
            In_Progress,
            Completed,
            On_Hold,
            Cancelled,
            Closed,
            Problem

        }

        public enum TestStatusEnum
        {
            None = 0,
            Not_Tested,
            Ready_To_Test,
            ReTest,
            Passed,
            Failed,
            Incomplete,
            Cannot_Test
            

        }

        public enum PriorityEnum
        {
            None = 0,
            Low,
            Medium,
            High,
            Immediate
        }


        public enum SeverityComplexityEnum
        {
            None = 0,
            Low,
            Medium,
            High,
            Very_High
        }

        



        public enum TypeEnum
        {
            None = 0,
            Development,
            Change,
            Bug,
            Failure,
            Test,
            Investigation
        }


        public enum RoleEnum
        {
            Business = 0,
            Developer,
            Admin


        }


        public static string DescEnum(string text, bool toTitleCase)
        {

            System.Globalization.TextInfo myTI = new System.Globalization.CultureInfo("en-US", false).TextInfo;

            if (String.IsNullOrEmpty(text))
                return string.Empty;

            if (toTitleCase)
            {
                return myTI.ToTitleCase(text.Trim().ToLower()).Replace('_', ' ');
            }
            else
            {
                return text.Replace('_', ' ');
            }
        }


    }



}