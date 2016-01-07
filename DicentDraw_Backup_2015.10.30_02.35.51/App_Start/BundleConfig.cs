using System.Web;
using System.Web.Optimization;

namespace DicentDraw
{
    public class BundleConfig
    {
        // 如需「搭配」的詳細資訊，請瀏覽 http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            //bundles.UseCdn = true;

            //const string kendoVersion = "2015.1.429";

            //const string kendoCommonCssPath =
            //    "http://cdn.kendostatic.com/" + kendoVersion +
            //    "/styles/kendo.common.min.css";

            //const string kendoBootstrapCssPath =
            //    "http://cdn.kendostatic.com/" + kendoVersion +
            //    "/styles/kendo.bootstrap.min.css";

            //const string kendoAllJsPath =
            //    "http://cdn.kendostatic.com/" + kendoVersion +
            //    "/js/kendo.all.min.js";

            //const string kendoMvcJsPath =
            //                "http://cdn.kendostatic.com/" + kendoVersion +
            //                "/js/kendo.aspnetmvc.min.js";

            //bundles.Add(new
            //    ScriptBundle("~/bundles/kendo/all/js")
            //    .Include("~/Scripts/kendo/kendo.all.js"));

            //bundles.Add(new
            //    ScriptBundle("~/bundles/kendo/mvc/js")
            //    .Include("~/Scripts/kendo/kendo.aspnetmvc.js"));

            //bundles.Add(new
            //    StyleBundle("~/bundles/kendo/common/css")
            //    .Include("~/Content/kendo/kendo.common.css"));

            //bundles.Add(new
            //    StyleBundle("~/bundles/kendo/bootstrap/css")
            //    .Include("~/Content/kendo/kendo.bootstrap.css"));

            bundles.Add(new ScriptBundle("~/bundles/kendo").Include(
            "~/Scripts/kendo/kendo.all.min.js",
                // "~/Scripts/kendo/kendo.timezones.min.js", // uncomment if using the Scheduler
            "~/Scripts/kendo/kendo.aspnetmvc.min.js"));

            bundles.Add(new StyleBundle("~/Content/kendo/css").Include(
            "~/Content/kendo/kendo.common-bootstrap.min.css",
            "~/Content/kendo/kendo.bootstrap.min.css"));

            // 使用開發版本的 Modernizr 進行開發並學習。然後，當您
            // 準備好實際執行時，請使用 http://modernizr.com 上的建置工具，只選擇您需要的測試。
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));
            bundles.IgnoreList.Clear();
        }
    }
}
