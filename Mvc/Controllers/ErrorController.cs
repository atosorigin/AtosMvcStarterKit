using System.Web.Mvc;

namespace Customer.Project.Mvc.Controllers
{
    public class ErrorController : BaseController
    {
        public ActionResult Error()
        {
            return View();
        }

        //[CkmsLog(
        //    SystemRole = SystemRoles.CkmsAdministrator, ByEmail = true,
        //    SuccessMessageResourceType = typeof(LoggingAndNotification), SuccessMessageResourceName = "ErrorUnauthorized",
        //    ErrorMessageResourceType = typeof(ResourceStrings), ErrorMessageResourceName = "ErrorUnauthorized")]
        public ActionResult Unauthorized()
        {
            return View();
        }

        public ActionResult NotFound()
        {
            return View();
        }
    }
}