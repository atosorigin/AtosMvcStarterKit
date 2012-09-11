using System;
using System.Web.Mvc;
using Customer.Project.Mvc.Instrumentation;
using Customer.Project.Mvc.Instrumentation.Security;
using Customer.Project.Mvc.Models;
using Customer.Project.Utilities;
using StackExchange.Profiling;

namespace Customer.Project.Mvc.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IFormatLogger _logger;

        // dependencies are automatically injected by StructureMap IoC
        public HomeController(IFormatLogger logger)
        {
            _logger = logger;
        }

        // Cache example, Profiles are configured in Web.config
        [AcceptVerbs(HttpVerbs.Get), OutputCache(CacheProfile = "HomeContent")]
        [LogRequest]
        public ActionResult Index()
        {
            using(MiniProfiler.Current.Step("Example of a MiniProfiler step. Setting ViewBag.Message"))
            {
                ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";                
            }
            return View();
        }
        [LogRequest(LogLevel = LogLevel.Debug)]
        public ActionResult About()
        {
            ViewBag.Message = "Your quintessential app description page.";

            return View();
        }
        public ActionResult Contact()
        {
            ViewBag.Message = "Your quintessential contact page.";

            return View();
        }

        public ActionResult ErrorHandlingExample()
        {
            throw new Exception("FAILURE!!");
        }

        // Example for Ajax calls
        [AjaxException]
        public JsonResult AjaxExample(bool getOne)
        {
            if (getOne)
                return Json(new { ResultNumber = 1, Success = true });
            else
                return Json(new { ResultNumber = 2, Success = true });
        }

        public ActionResult ActionResultMessageExample()
        {
            return View("ActionResultMessage", new ActionResultMessageViewModel() { Message = "Example of a simple result message with the ActionResultMessage view." });
        }
    }
}
