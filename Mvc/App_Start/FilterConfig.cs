using System.Web.Mvc;
using AtosOrigin.NetLibrary.Components.Core;
using Customer.Project.Application;
using Customer.Project.Mvc.Instrumentation.Elmah;
using Customer.Project.Utilities;

namespace Customer.Project.Mvc.App_Start
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            IFormatLogger logger = ServiceLocator.GetExisting<IFormatLogger>();

            //filters.Add(new HandleErrorAttribute());
            filters.Add(new HandleErrorElmahAttribute(logger));
        }
    }
}