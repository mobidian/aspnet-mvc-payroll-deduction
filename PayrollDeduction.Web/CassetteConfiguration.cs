using Cassette.Configuration;
using Cassette.Scripts;
using Cassette.Stylesheets;

namespace PayrollDeduction.Web
{
    /// <summary>
    /// Configures the Cassette asset modules for the web application.
    /// </summary>
    public class CassetteConfiguration : ICassetteConfiguration
    {
        public void Configure(BundleCollection bundles, CassetteSettings settings)
        {
            // Please read http://getcassette.net/documentation/configuration

            bundles.Add<StylesheetBundle>("session", new[] {
                "~/Assets/bootstrap/css/bootstrap.min.css",
                "~/Assets/css/application.css",
                "~/Assets/css/session.css"
            });

            bundles.Add<StylesheetBundle>("application", new[] {
                "~/Assets/bootstrap/css/bootstrap.min.css",
                "~/Assets/css/application.css"
            });

            bundles.Add<ScriptBundle>("application", new[] {
                "~/Assets/js/jquery-1.7.2.min.js",
                "~/Assets/bootstrap/js/bootstrap.min.js",
                "~/Assets/js/waypoints.min.js",
                "~/Assets/js/pagination.js",
                "~/Assets/js/global.js",
                "~/Assets/js/accounts.js"
            });
        }
    }
}