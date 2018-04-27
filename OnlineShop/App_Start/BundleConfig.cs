using System.Web;
using System.Web.Optimization;

namespace OnlineShop
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {                     
            

            bundles.Add(new ScriptBundle("~/bundler/jsbase").Include(
                      "~/Assets/Client/js/jquery-3.3.1.min.js",
                      "~/Assets/Client/js/jquery-ui.js",
                      "~/Assets/Client/js/bootstrapValidator.min.js",
                      "~/Assets/Client/js/easyResponsiveTabs.js",
                      "~/Assets/Client/js/move-top.js",
                      "~/Assets/Client/js/easing.js",
                      "~/Assets/Client/js/startstop-slider.js",
                      "~/Scripts/jquery.validate.min.js"
                      ));

            bundles.Add(new ScriptBundle("~/bundler/jscontroller").Include(
                      "~/Assets/Client/js/controller/baseController.js"
                      ));


            bundles.Add(new StyleBundle("~/bundler/csscore").Include(
                      "~/Assets/Client/css/style.css",
                      "~/Assets/Client/css/slider.css",
                      "~/Assets/Client/css/bootstrap.css",
                      "~/Assets/Client/css/jquery-ui.css",
                      "~/Assets/Client/css/bootstrap-social.css"
                      ));

            BundleTable.EnableOptimizations = true;
        }
    }
}
