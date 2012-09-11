using System.Web.Optimization;

namespace Customer.Project.Mvc.App_Start
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/common").Include(
                            "~/Scripts/jquery-1.8.0.js"
                          , "~/Scripts/jquery-ui-1.8.23.js"
                          , "~/Scripts/jquery.validate.js"
                          , "~/Scripts/jquery.validate.unobtrusive.js"
                          , "~/Scripts/knockout-2.1.0.debug.js"
                          , "~/Scripts/knockout.mapping-2.1.2.js"
                          , "~/Scripts/common.js"
                          , "~/Scripts/date.format.js"
                          , "~/Scripts/date.js"
                          , "~/Scripts/jqueryslidemenu.js"
                ));

            //bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
            //            "~/Scripts/jquery.unobtrusive*",
            //            "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                  "~/Content/reset.css"
                , "~/Content/site.css"
                , "~/Content/jqueryslidemenu.css"
                , "~/Content/mobile.css"));

            bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
                        "~/Content/themes/base/jquery.ui.core.css",
                        "~/Content/themes/base/jquery.ui.resizable.css",
                        "~/Content/themes/base/jquery.ui.selectable.css",
                        "~/Content/themes/base/jquery.ui.accordion.css",
                        "~/Content/themes/base/jquery.ui.autocomplete.css",
                        "~/Content/themes/base/jquery.ui.button.css",
                        "~/Content/themes/base/jquery.ui.dialog.css",
                        "~/Content/themes/base/jquery.ui.slider.css",
                        "~/Content/themes/base/jquery.ui.tabs.css",
                        "~/Content/themes/base/jquery.ui.datepicker.css",
                        "~/Content/themes/base/jquery.ui.progressbar.css",
                        "~/Content/themes/base/jquery.ui.theme.css"));
        }
    }
}