using System.Web.Optimization;
using Common.Util;

namespace Website
{
    public class BundleConfig : IBootstrapperTask
    {
        public void Execute()
        {
            RegisterBundles(BundleTable.Bundles);
        }

        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/global").Include(
                        "~/Scripts/modernizr-*",
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/bootstrap*"
                    ));

            bundles.Add(new ScriptBundle("~/bundles/validation").Include(
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate*"
                    ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                        "~/Content/bootstrap*",
                        "~/Content/Auctions.css",
                        "~/Content/Details.css",
                        "~/Content/Featured.css"
                    ));
        }
    }
}