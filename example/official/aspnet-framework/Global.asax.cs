using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MvcBarcodeQRCodeFramework
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            // Add native Dynamsoft DLL directories to the process PATH so P/Invoke can find them
            string x64 = Server.MapPath("~/bin/x64");
            string x86 = Server.MapPath("~/bin/x86");
            string current = Environment.GetEnvironmentVariable("PATH") ?? string.Empty;
            if (!current.Contains(x64))
                Environment.SetEnvironmentVariable("PATH", x64 + ";" + x86 + ";" + current);

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}
