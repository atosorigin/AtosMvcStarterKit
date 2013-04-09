using System.Web.Mvc;
using Customer.Project.Mvc.Models;

namespace Customer.Project.Mvc.Controllers
{
    public abstract class BaseController : Controller
    {
        protected ActionResult ResultMessage(ActionResultMessageViewModel model)
        {
            return View("ActionResultMessage", model);
        }
    }
}