using System.Collections.Generic;
using System.Web.Optimization;

namespace SS.Mvc.AxosoftApi
{
    public static class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            // scripts bundles
            bundles.Add(new ScriptBundle("~/bundles/angular").Include("~/Scripts/angular-bundle.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/app").Include("~/Scripts/app-bundle.min.js"));

            // style bundles
            bundles.Add(new StyleBundle("~/Content/css/vendor").Include("~/Content/css/css-bundle.min.css"));
            bundles.Add(new StyleBundle("~/Content/css/app.css").Include(
                    "~/Content/css/style.css"
                ));
        }
    }
}