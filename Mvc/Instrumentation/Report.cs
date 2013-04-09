using System;
using System.Diagnostics;
using System.Web;
using System.Web.Mvc;
using Customer.Project.Application;
using Customer.Project.Mvc.Controllers;
using Customer.Project.Mvc.Instrumentation.Security;
using Customer.Project.Utilities;
using Elmah;

namespace Customer.Project.Mvc.Instrumentation
{
    public static class Report
    {
        private static IFormatLogger _logger = ServiceLocator.Get<IFormatLogger>();
        public static void LogAndRaiseErrorImplementation(string errorDescription)
        {
            //WebUILogging.WebUILog(LoggingAndNotification.UnexpectedError, errorDescription);
            Trace.WriteLine(errorDescription);
            ErrorSignal.FromCurrentContext().Raise(new Exception(errorDescription));
        }

        /// <summary>
        /// Logs error to IFormatLogger and Elmah
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="prefix"></param>
        /// <param name="formatStringParameters"></param>
        public static void LogInfo(Exception ex, string prefix, params object[] formatStringParameters)
        {
            _logger.Info(ex, prefix, formatStringParameters);

            //ErrorSignal.FromCurrentContext().Raise(ex);
        }

        /// <summary>
        /// Logs error to IFormatLogger and Elmah and adds a non descriptive error message to the modelstate
        /// </summary>
        public static void Error(BaseController controller, string errorDescription)
        {
            LogAndRaiseErrorImplementation(errorDescription);
            if (controller.ModelState != null)
                controller.ModelState.AddModelError(String.Empty
                    , string.Format("Ooops, something whent wrong. {0}. The administrator has been notified. ", errorDescription));
        }
        /// <summary>
        /// Logs error to IFormatLogger and Elmah and adds a non descriptive error message to the modelstate
        /// </summary>
        public static void Error(BaseController controller, Exception ex, string prefix, params object[] formatStringParameters)
        {
            _logger.Error(ex, prefix, formatStringParameters);

            ErrorSignal.FromCurrentContext().Raise(ex);
            string message = String.Format(prefix, formatStringParameters);
            string error =
                string.Format("Ooops, something whent wrong. {0}. The administrator has been notified. ", message);
            if (controller.ModelState != null)
            {
                controller.ModelState.AddModelError(String.Empty, error);
            }
            else
            {
                controller.ViewBag.Message = error;
            }
        }

        private static void LogAndRaiseAuthorizationError(BaseController controller)
        {
            LogAndRaiseErrorImplementation(string.Format("Access to page '{0}' is denied for user {1})"
                , controller.Request.Url, controller.User.Identity.IsAuthenticated ? controller.User.Identity.Name : "?"));
        }
        public static void ReportAuthorizationError(this BaseController controller)
        {
            LogAndRaiseAuthorizationError(controller);

            controller.Response.StatusCode = 403;
            controller.Response.Clear();

            new TransferResult("Unauthorized", "Error").ExecuteResult(controller.ControllerContext);

            controller.Response.End();
        }

        public static object ReturnJsonAuthorizationError(this BaseController controller)
        {
            LogAndRaiseAuthorizationError(controller);
            return new { ResultText = "You are not authorized for the requested action, please contact the system administrator." };
        }
    }
}