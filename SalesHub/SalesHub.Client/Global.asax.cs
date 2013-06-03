using System;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using SalesHub.Client.App_Start;
using SalesHub.Data;

namespace SalesHub.Client
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : HttpApplication
    {
        private static DateTime _lastMaintenanceTime;
        private static bool _isInMaintenanceMode;

        protected void Application_Start()
        {
            _lastMaintenanceTime = DateTime.Now;

            Database.SetInitializer<SalesHubDbContext>(new SalesHubDbInitializer());

            AreaRegistration.RegisterAllAreas();

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            if (_isInMaintenanceMode && Request.Url.AbsolutePath != "/Maintenance")
            {
                Response.Redirect("~/Maintenance", true);
            }

            DateTime now = DateTime.Now;
            if (now > _lastMaintenanceTime.AddHours(24))
            {
                _lastMaintenanceTime = now;
                _isInMaintenanceMode = true;

                Response.Redirect("~/Maintenance", false);
                Response.Flush();

                var dbContext = new SalesHubDbContext();
                dbContext.Database.Delete();
                var initializer = new SalesHubDbInitializer();
                initializer.InitializeDatabase(dbContext);

                _isInMaintenanceMode = false;
            }
        }
    }
}