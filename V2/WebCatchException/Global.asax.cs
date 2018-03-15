using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace WebCatchException
{
    public class MvcApplication : System.Web.HttpApplication
    {

        public static  DateTime? buildDateTime = null;

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //Smtp.NotificationSmtp.ServeurInfo = new Smtp.ServeurInfo();

            Smtp.NotificationSmtp.MessageSmtp = new Smtp.MessageSmtp(@"CatchException@Test.zz", new string[] { @"colin@fransbonhomme.fr" , @"langlais@fransbonhomme.fr" }, "CatchException.");


            var version = System.Reflection.Assembly.GetAssembly(typeof(MvcApplication))?.GetName()?.Version;
            if(version != null)
                buildDateTime = new DateTime(2000, 1, 1).Add(new TimeSpan(
                TimeSpan.TicksPerDay * version.Build + // days since 1 January 2000
                TimeSpan.TicksPerSecond * 2 * version.Revision)); // seconds since midnight, (multiply by 2 to get original)

        }
    }
}
