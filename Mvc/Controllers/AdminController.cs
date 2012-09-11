using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Customer.Project.Mvc.Instrumentation.Security;

namespace Customer.Project.Mvc.Controllers
{
    [Authorize]
    public class AdminController : BaseController
    {
        // Authorization example
        // the page is only accessible for users with the Administrator system role
        [RoleAuthorization(SystemRoles = new SystemRoles[] { SystemRoles.Administrator})]
        public ActionResult Index()
        {
            return View();
        }
    }
}