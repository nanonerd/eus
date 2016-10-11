using System.Web;
using System.Web.Optimization;
using System.Web.Optimization.React;

namespace eus.UI
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {           

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/external/jQuery/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/external/jQuery/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/external/modernizr/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/external/bootstrap/bootstrap.js",
                      "~/Scripts/external/bootstrap/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrapCerulean.css",
                      "~/Content/site.css",
                      "~/Scripts/external/kartikRating/css/star-rating.css"));

            bundles.Add(new ScriptBundle("~/bundles/eusCommon").Include(
                      "~/Scripts/internal/eus/api.js",
                      "~/Scripts/internal/eus/common.js"));

            bundles.Add(new ScriptBundle("~/bundles/react").Include(
                    "~/Scripts/external/react-15.1.0/react.js",
                    "~/Scripts/external/react-15.1.0/react-dom.js"
                    ));

            //bundles.Add(new ScriptBundle("~/bundles/react").Include(
            //        "~/Scripts/external/react-15.0.2/react.js",
            //        "~/Scripts/external/react-15.0.2/react-dom.js"
            //        ));

            bundles.Add(new BabelBundle("~/bundles/main").Include(
                // Add your JSX files here
                "~/Scripts/external/kartikRating/js/star-rating.min.js"
                //"~/Scripts/internal/vote/topicAnswers.jsx"
                
                // You can include regular JavaScript files in the bundle too
                //"~/Content/ajax.js"
            ));


            // Set EnableOptimizations to false for debugging. For more information,
            // visit http://go.microsoft.com/fwlink/?LinkId=301862
            BundleTable.EnableOptimizations = false;
        }
    }
}
