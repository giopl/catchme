using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CatchMe.Helpers
{
    public class SelectItems
    {

        public static List<SelectListItem> PriorityItemList(string selected = "")
        {
            List<SelectListItem> List = new List<SelectListItem>();
            List.Add(new SelectListItem() { Text = "None", Value = "0", Selected = selected == "0" });
            List.Add(new SelectListItem() { Text = "Low", Value = "1", Selected = selected == "1" });
            List.Add(new SelectListItem() { Text = "Medium", Value = "2", Selected = selected == "2" });
            List.Add(new SelectListItem() { Text = "High", Value = "3", Selected = selected == "3" });
            List.Add(new SelectListItem() { Text = "Immediate", Value = "4", Selected = selected == "4" });

            return List;
        }

        public static List<SelectListItem> ComplexityItemList(string selected = "")
        {
            List<SelectListItem> List = new List<SelectListItem>();
            List.Add(new SelectListItem() { Text = "None", Value = "0", Selected = selected == "0" });
            List.Add(new SelectListItem() { Text = "Low", Value = "1", Selected = selected == "1" });
            List.Add(new SelectListItem() { Text = "Medium", Value = "2", Selected = selected == "2" });
            List.Add(new SelectListItem() { Text = "High", Value = "3", Selected = selected == "3" });
            List.Add(new SelectListItem() { Text = "Very High", Value = "4", Selected = selected == "4" });

            return List;
        }

        public static List<SelectListItem> StatusItemList(string selected = "")
        {
            List<SelectListItem> List = new List<SelectListItem>();
            List.Add(new SelectListItem() { Text = "None", Value = "0", Selected = selected == "0" });
            List.Add(new SelectListItem() { Text = "New", Value = "1", Selected = selected == "1" });
            List.Add(new SelectListItem() { Text = "Open", Value = "2", Selected = selected == "2" });
            List.Add(new SelectListItem() { Text = "In Progress", Value = "3", Selected = selected == "3" });
            List.Add(new SelectListItem() { Text = "Completed", Value = "4", Selected = selected == "4" });
            List.Add(new SelectListItem() { Text = "On Hold", Value = "5", Selected = selected == "5" });
            List.Add(new SelectListItem() { Text = "Cancelled", Value = "6", Selected = selected == "6" });
            List.Add(new SelectListItem() { Text = "Closed", Value = "7", Selected = selected == "7" });
            List.Add(new SelectListItem() { Text = "Problem", Value = "8", Selected = selected == "8" });

            return List;
        }

        public static List<SelectListItem> SeverityItemList(string selected = "")
        {
            List<SelectListItem> List = new List<SelectListItem>();
            List.Add(new SelectListItem() { Text = "None", Value = "0", Selected = selected == "0" });
            List.Add(new SelectListItem() { Text = "Low", Value = "1", Selected = selected == "1" });
            List.Add(new SelectListItem() { Text = "Medium", Value = "2", Selected = selected == "2" });
            List.Add(new SelectListItem() { Text = "High", Value = "3", Selected = selected == "3" });
            List.Add(new SelectListItem() { Text = "Very High", Value = "4", Selected = selected == "4" });

            return List;
        }



        public static List<SelectListItem> TypeItemList(string selected = "")
        {
            List<SelectListItem> List = new List<SelectListItem>();
            List.Add(new SelectListItem() { Text = "None", Value = "0", Selected = selected == "0" });
            List.Add(new SelectListItem() { Text = "Development Task", Value = "1", Selected = selected == "1" });
            List.Add(new SelectListItem() { Text = "Change", Value = "2", Selected = selected == "2" });
            List.Add(new SelectListItem() { Text = "Bug", Value = "3", Selected = selected == "3" });
            List.Add(new SelectListItem() { Text = "Failure", Value = "4", Selected = selected == "4" });
            List.Add(new SelectListItem() { Text = "Test", Value = "5", Selected = selected == "5" });
            List.Add(new SelectListItem() { Text = "Investigation", Value = "6", Selected = selected == "6" });

            return List;
        }





        public static List<SelectListItem> TestStatusItemList(string selected = "")
        {
            List<SelectListItem> List = new List<SelectListItem>();
            List.Add(new SelectListItem() { Text = "None", Value = "0", Selected = selected == "0" });
            List.Add(new SelectListItem() { Text = "Not Tested", Value = "1", Selected = selected == "1" });
            List.Add(new SelectListItem() { Text = "Ready for Test", Value = "2", Selected = selected == "2" });
            List.Add(new SelectListItem() { Text = "To Re-test", Value = "3", Selected = selected == "3" });
            List.Add(new SelectListItem() { Text = "Passed", Value = "4", Selected = selected == "4" });
            List.Add(new SelectListItem() { Text = "Failed", Value = "5", Selected = selected == "5" });
            List.Add(new SelectListItem() { Text = "Incomplete", Value = "6", Selected = selected == "6" });
            List.Add(new SelectListItem() { Text = "Cannot Test", Value = "7", Selected = selected == "7" });
            

            return List;
        }



        public static List<SelectListItem> RoleItemList(string selected = "")
        {
            List<SelectListItem> List = new List<SelectListItem>();
            List.Add(new SelectListItem() { Text = "Business", Value = "0", Selected = selected == "0" });
            List.Add(new SelectListItem() { Text = "Developer", Value = "1", Selected = selected == "1" });
            List.Add(new SelectListItem() { Text = "Admin", Value = "2", Selected = selected == "2" });
            

            return List;
        }



    }



}