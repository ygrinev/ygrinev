using System.Web;
using System.Web.Optimization;

namespace AwesomeAngularMVCApp_02
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/AwesomeAngularMVCApp")
                .IncludeDirectory("~/Scripts/Controllers", "*.js")
                .Include("~/Scripts/AwesomeAngularMVCApp.js")); 
            
            bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/site.css"));

            BundleTable.EnableOptimizations = true;
        }
    }
}
