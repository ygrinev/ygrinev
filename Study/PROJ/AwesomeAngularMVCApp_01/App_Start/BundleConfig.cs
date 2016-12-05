using System.Web;
using System.Web.Optimization;

namespace AwesomeAngularMVCApp
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            //bundles.Add(new StyleBundle("~/Content/css").Include(
            //          "~/Content/Site.css"));
            bundles.Add(new ScriptBundle("~/bundles/AwesomeAngularMVCApp")
                .IncludeDirectory("~/Scripts/Controllers", "*.js")
                .Include("~/Scripts/AwesomeAngularMVCApp.js"));
            bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/site.css"));

            // Set EnableOptimizations to false for debugging. For more information,
            // visit http://go.microsoft.com/fwlink/?LinkId=301862
            BundleTable.EnableOptimizations = true;
        }
    }
}
