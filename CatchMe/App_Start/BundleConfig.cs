using System.Web;
using System.Web.Optimization;

namespace CatchMe
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));


            bundles.Add(new ScriptBundle("~/bundles/chart").Include(
                        
                        "~/Scripts/plugins/chart.js"

                        ));

            

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/bootstrap3-typeahead.js",
                      //https://bootstrap-datepicker.readthedocs.io/en/latest/options.html#autoclose
                      "~/Scripts/bootstrap-datepicker.js",
                      //https://select2.github.io/
                      "~/Scripts/plugins/chosen.jquery.js",
                      "~/Scripts/plugins/readmore.js",
                      "~/Scripts/plugins/moment.js",
                      "~/Scripts/sweetalert.js",
                      "~/Scripts/plugins/notify.js",
                      //"~/Scripts/plugins/highlight.js",
                      "~/Scripts/respond.js"

                      ));

            bundles.Add(new ScriptBundle("~/bundles/nicedit").Include(
                      "~/Scripts/plugins/nicEdit.js"
            ));

            bundles.Add(new ScriptBundle("~/bundles/datatables").Include(
                      "~/Scripts/plugins/DataTables.js"
            ));


            bundles.Add(new ScriptBundle("~/bundles/custom").Include(
                      "~/Scripts/custom/shared.js"
                      ));
            //http://vitalets.github.io/combodate/
            bundles.Add(new ScriptBundle("~/bundles/combodate").Include(
                      
                      "~/Scripts/plugins/combodate.js"
                      ));




            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/bootstrap-datepicker3.css",
                      "~/Content/font-awesome.css",
                      "~/Content/plugin/chosen.css",
                      "~/Content/DataTables.css",
                      "~/Content/sweetalert.css",

                      "~/Content/site.css"));

            // Set EnableOptimizations to false for debugging. For more information,
            // visit http://go.microsoft.com/fwlink/?LinkId=301862
            //BundleTable.EnableOptimizations = true;
        }
    }
}
