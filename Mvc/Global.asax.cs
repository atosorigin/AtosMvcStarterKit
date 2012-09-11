using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using AtosOrigin.NetLibrary.Components.Core;
using Customer.Project.Application;
using Customer.Project.Mvc.App_Start;
using Customer.Project.Mvc.Instrumentation;
using Customer.Project.Mvc.Instrumentation.Elmah;
using Customer.Project.Utilities;
using Customer.Project.Utilities.Web;
using Elmah;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using StackExchange.Profiling;
using log4net;

namespace Customer.Project.Mvc
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        private static bool _initialized;
        protected static object _initializeLock = new object();

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        protected void Application_Start()
        {
            Initialize();
        }

        public static void Initialize()
        {
            lock (_initializeLock)
            {
                if (!_initialized)
                {
                    // initalize inversion of control with Log4Net ILogger
                    ServiceLocator.InitializeContainer(expression => expression.For<ILog>().Singleton().Use(
                        LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType))
                        );

                    AreaRegistration.RegisterAllAreas();

                    WebApiConfig.Register(GlobalConfiguration.Configuration);
                    FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
                    RouteConfig.RegisterRoutes(RouteTable.Routes);
                    BundleConfig.RegisterBundles(BundleTable.Bundles);
                    AuthConfig.RegisterAuth();

                    // initialize structuremap container for MVC controllers
                    var dependencyResolver = ServiceLocator.Get<StructureMapDependencyResolver>();
                    DependencyResolver.SetResolver(dependencyResolver);

                    // register attribute filter provider for resolving filterattribute attributes
                    FilterProviders.Providers.Remove(
                        FilterProviders.Providers.Single(f => f is FilterAttributeFilterProvider));
                    FilterProviders.Providers.Add(ServiceLocator.GetExisting<StructureMapFilterAttributeFilterProvider>());

                    // recreate entity framework database if required
                    //System.Data.Entity.Database.SetInitializer(new System.Data.Entity.DropCreateDatabaseIfModelChanges<MvcContext>());
                    //new MvcContext().Users.Find(new object[] {Guid.NewGuid()});

                    //Tell the application to use log4net and get config settings from the mentioned path.
                    string xmlConfigFilelog4Net = HttpContext.Current.Server.MapPath("~/log4net.config");
                    log4net.Config.XmlConfigurator.Configure(new FileInfo(xmlConfigFilelog4Net));

                    IFormatLogger logger = ServiceLocator.GetExisting<IFormatLogger>();
                    logger.Info("Atos Mvc Web Application initialized successfully.");

                    _initialized = true;                    
                }
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        protected void Session_Start()
        {
            IFormatLogger logger = ServiceLocator.Get<IFormatLogger>();
            logger.Info("A new user logged on successfully to Atos Mvc Web Application as {0}", LoggedOnUser.UserName);
        }

        protected void Application_BeginRequest()
        {
            if (Request.IsLocal)
            {
                MiniProfiler.Start();
            }
        }

        protected void Application_EndRequest(object sender, EventArgs e)
        {
            try
            {
                if (MiniProfiler.Current != null)
                    MiniProfiler.Stop();

                // Close Unit of work if it was created
                //NHibernateSessionManager.Instance.CommitTransaction();
            }
            catch (Exception ex)
            {
                Report.Error(null, ex, "NHibernate commit transaction failed");
            }
        }

        // ELMAH Filtering
        protected void ErrorLog_Filtering(object sender, ExceptionFilterEventArgs e)
        {
            FilterError404(e);
        }
        // ELMAH Filtering
        protected void ErrorMail_Filtering(object sender, ExceptionFilterEventArgs e)
        {
            FilterError404(e);
        }

        // Dismiss 404 errors for ELMAH
        private void FilterError404(ExceptionFilterEventArgs e)
        {
            if (e.Exception.GetBaseException() is HttpException)
            {
                HttpException ex = (HttpException)e.Exception.GetBaseException();
                if (ex.GetHttpCode() == 404)
                    e.Dismiss();
            }
        }

    }
}