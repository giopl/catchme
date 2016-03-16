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


        public enum HotelTab
        {
            DETAILS = 0,
            ROOMS,
            CONTACTS,
            MARKETS,
            GUESTS_AGE,
            FEATURES,
            GALLERY,
            SUPPLEMENTS,
            CHARGES,
            PACKAGES

        }

        public enum RoomTab
        {
            UPLOAD,
            GALLERY,
            OCCUPANCIES,
            FEATURES,
            BASE_PRICE


        }

        public enum UserTab
        {
            GALLERY,
            RESETPASSWORD,
            EDITROLE
        }

        public enum VehicleTab
        {
            GALLERY
        }

        public enum ProviderTab
        {
            GALLERY,
            VEHICLES,
            ACTIVITIES,
            CONTACTS
        }

        public enum ActivityTab
        {
            GALLERY
        }

        public enum UserPreference
        {
            GALLERY,
            USERPREF,
            CHANGEPASSWORD
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