namespace Customer.Project.Mvc.Instrumentation
{
    /// <summary>
    /// Loaded by IIS 7.5 for initializing the application pool once the IIS server has started
    /// http://weblogs.asp.net/scottgu/archive/2009/09/15/auto-start-asp-net-applications-vs-2010-and-net-4-0-series.aspx
    /// </summary>
    public class PreWarmCache : System.Web.Hosting.IProcessHostPreloadClient
    {
        public void Preload(string[] parameters)
        {
            // Perform initialization and cache loading logic here...
            MvcApplication.Initialize();
        }
    }
}