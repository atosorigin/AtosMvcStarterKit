using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using AtosOrigin.NetLibrary.Components.Core;

namespace Customer.Project.Utilities.Web
{
    public static class ControllerExtensions
    {
        public static string RenderPartialView(this Controller controller,
                                       string viewName, object model)
        {
            Check.Require(!string.IsNullOrEmpty(viewName));

            controller.ViewData.Model = model;

            using (var sw = new StringWriter())
            {
                ViewEngineResult viewResult =
                   ViewEngines.Engines.FindPartialView(
                   controller.ControllerContext, viewName);

                var viewContext = new ViewContext(controller.ControllerContext,
                    viewResult.View, controller.ViewData, controller.TempData, sw);

                viewResult.View.Render(viewContext, sw);

                return sw.GetStringBuilder().ToString();
            }
        }

        public static IEnumerable<string> GetErrorsFromModelState(this Controller controller)
        {
            return controller.ModelState.SelectMany(x => x.Value.Errors.Select(error => error.ErrorMessage));
        }

    }
}
