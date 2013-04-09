using System.Web.Http;
using Customer.Project.Application;
using Customer.Project.Mvc.Instrumentation;
using Customer.Project.Mvc.Instrumentation.ElmahHandlers;
using Customer.Project.Utilities;

namespace Customer.Project.Mvc.App_Start
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                name: "ActionWebApiRoute",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional },
                constraints: new { action = new ExcludeGuidActionRouteConstraint() }
            );

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Filters.Add(new HandleWebApiErrorElmahAttribute(ServiceLocator.GetExisting<IFormatLogger>()));

            // For Web API:
            config.DependencyResolver = ServiceLocator.Get<StructureMapWebApiDependencyResolver>();

            // Disable the Xml formatter
            GlobalConfiguration.Configuration.Formatters.XmlFormatter.SupportedMediaTypes.Clear();

            // Can be removed after MVC 4 Beta

            // Create Json.Net formatter serializing DateTime using the ISO 8601 format
            //JsonSerializerSettings serializerSettings = new JsonSerializerSettings();
            //serializerSettings.Converters.Add(new IsoDateTimeConverter());
            //GlobalConfiguration.Configuration.Formatters[0] = new JsonNetFormatter(serializerSettings);
        }
    }
}
