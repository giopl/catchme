using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace CatchMe.Helpers
{

    public class ConfigurationHelper
    {
        


        public static string GetEnvironment()
        {
            return ConfigurationManager.AppSettings["Environment"];
        }


        public static string GetApplicatioName()
        {
            return ConfigurationManager.AppSettings["ApplicationName"];
        }


        #region "connection strings"

        public static string ConnectionStringLocal()
        {
            return ConfigurationManager.AppSettings["ConnectionStringLocal"];
        }
        public static string ConnectionStringDebug()
        {
            return ConfigurationManager.AppSettings["ConnectionStringDebug"];
        }
        public static string ConnectionStringRelease()
        {
            return ConfigurationManager.AppSettings["ConnectionStringRelease"];
        }

        public static string ConnectionStringTest()
        {
            return ConfigurationManager.AppSettings["ConnectionStringTest"];
        }

        #endregion

        public static bool LogQueries()
        {
            return Convert.ToBoolean(ConfigurationManager.AppSettings["LogQueries"]);
        }



        public static string AdminEmail()
        {
            return ConfigurationManager.AppSettings["AdminEmail"];
        }

        public static string WebEmail()
        {
            return ConfigurationManager.AppSettings["WebappMail"];
        }



        public static string WebEmailPwd()
        {
            return ConfigurationManager.AppSettings["WebappPwd"];
        }

        public static string SmtpServer()
        {
            return ConfigurationManager.AppSettings["smtpserver"];
        }

        public static bool ErrorDebug()
        {
            return Convert.ToBoolean(ConfigurationManager.AppSettings["ErrorDebug"]);
        }

        public static bool GetIsPasswordEnabled()
        {
            return Convert.ToBoolean(ConfigurationManager.AppSettings["PasswordEnabled"]);
        }

        public static string GetTestPassword()
        {
            return ConfigurationManager.AppSettings["TestPassword"];
        }

        public static bool UseCache()
        {
            return Convert.ToBoolean(ConfigurationManager.AppSettings["UseCache"]);
        }


        public static string[] GetSupportEmailsDevelopment()
        {

            return Convert.ToString(ConfigurationManager.AppSettings["SupportEmailDevelopment"]).Split(';');
        }


        public static string GetSaltKey()
        {
            return ConfigurationManager.AppSettings["SaltKey"];
        }


        public static List<String> AuthorizedImagesExt()
        {
            List<String> result = new List<String>();
            string[] settings = ConfigurationManager.AppSettings["authorizedImagesExt"].ToString().Split(';');
            foreach (var item in settings)
            {
                if (!string.IsNullOrWhiteSpace(item))
                {
                    result.Add(item.ToLower());
                }
            }
            return result;
        }


        public static string GetUserImgPath()
        {
            return ConfigurationManager.AppSettings["UserImgPath"];
        }

        public static string GetProviderImgPath()
        {
            return ConfigurationManager.AppSettings["ProviderImgPath"];
        }

        public static string GetRoomImgPath()
        {
            return ConfigurationManager.AppSettings["RoomImgPath"];
        }

        public static string GetActivityImgPath()
        {
            return ConfigurationManager.AppSettings["ActivityImgPath"];
        }

        public static string GetVehicleImgPath()
        {
            return ConfigurationManager.AppSettings["VehicleImgPath"];
        }


        public static string PlaceholderImgPath
        {
            get
            {
                return ConfigurationManager.AppSettings["PlaceholderImgPath"];
            }
        }





        public static int MaxUploadSizeUser()
        {
            return Convert.ToInt32(ConfigurationManager.AppSettings["MaxUploadSizeUser"]);
        }

        public static int MaxUploadSizeProvider()
        {
            return Convert.ToInt32(ConfigurationManager.AppSettings["MaxUploadSizeProvider"]);
        }

        public static int MaxUploadSizeRoom()
        {
            return Convert.ToInt32(ConfigurationManager.AppSettings["MaxUploadSizeRoom"]);
        }

        public static int MaxUploadSizeVehicle()
        {
            return Convert.ToInt32(ConfigurationManager.AppSettings["MaxUploadSizeVehicle"]);
        }

        public static int MaxUploadSizeActivity()
        {
            return Convert.ToInt32(ConfigurationManager.AppSettings["MaxUploadSizeActivity"]);
        }

        #region email


        public static bool SendEmail()
        {
            return Convert.ToBoolean(ConfigurationManager.AppSettings["SendEmail"]);
        }

        public static bool SendErrorEmail()
        {
            return Convert.ToBoolean(ConfigurationManager.AppSettings["SendErrorEmail"]);
        }


        public static string GetSmtpHostIp()
        {
            return ConfigurationManager.AppSettings["SmtpHostIp"];
        }


        public static string GetSmtpHostDns()
        {
            return ConfigurationManager.AppSettings["SmtpHostDns"];
        }


        public static bool IsSendErrorEmail()
        {
            return Convert.ToBoolean(ConfigurationManager.AppSettings["SendErrorEmail"]);
        }

        public static string[] GetSupportEmails()
        {
            return Convert.ToString(ConfigurationManager.AppSettings["SupportEmail"]).Split(';');
        }

        public static string GetSupportEmailGroup()
        {
            return Convert.ToString(ConfigurationManager.AppSettings["SupportEmailGroup"]);
        }

        #endregion

    }


}