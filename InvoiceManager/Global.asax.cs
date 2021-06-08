using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace InvoiceManager
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        //Metoda wywolywana przed kazdym requestem
        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            //Rozne dane regionalne np. dot. formatu itd.
            CultureInfo newCulture = (CultureInfo)Thread.CurrentThread.CurrentCulture.Clone();
            newCulture.DateTimeFormat.ShortDatePattern = "dd-MM-yyyy";
            newCulture.DateTimeFormat.DateSeparator = "-";
            newCulture.NumberFormat.NumberDecimalDigits = 2;  //dwa miejsca po przecinku
            newCulture.NumberFormat.NumberDecimalSeparator = ","; //przecinek
            newCulture.NumberFormat.NumberGroupSeparator= "";
            Thread.CurrentThread.CurrentCulture = newCulture;
        }
    }
}
