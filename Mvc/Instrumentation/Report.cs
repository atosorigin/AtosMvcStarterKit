using System;
using System.Diagnostics;
using System.Web;
using System.Web.Mvc;
using Customer.Project.Application;
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
        public static void Error(ModelStateDictionary modelState, string errorDescription)
        {
            LogAndRaiseErrorImplementation(errorDescription);
            if (modelState != null)
                modelState.AddModelError("", errorDescription);
        }
        public static void Error(ModelStateDictionary modelState, Exception ex, string prefix, params object[] formatStringParameters)
        {
            _logger.Error(ex, prefix, formatStringParameters);

            ErrorSignal.FromCurrentContext().Raise(ex);

            if (modelState != null)
                modelState.AddModelError("", String.Format(prefix, formatStringParameters) + ex.Message);
        }
        public static void ReportError(this Controller controller, Exception ex, string messageFormatString, params object[] formatStringParameters)
        {
            string formattedMessage = String.Format(messageFormatString, formatStringParameters);
            Error(controller.ModelState, ex, formattedMessage);
        }

        public static void ReportError(this Controller controller, string messageFormatString, params object[] formatStringParameters)
        {
            string formattedMessage = String.Format(messageFormatString, formatStringParameters);
            Error(controller.ModelState, formattedMessage);
        }

        private static void LogAndRaiseAuthorizationError(Controller controller)
        {
            LogAndRaiseErrorImplementation(string.Format("Access to page '{0}' is denied for user {1})"
               , controller.Request.Url, LoggedOnUser.UserName));
        }
        public static void ReportAuthorizationError(this Controller controller)
        {
            LogAndRaiseAuthorizationError(controller);

            controller.Response.StatusCode = 403;
            controller.Response.Clear();

            new TransferResult("Unauthorized", "Error").ExecuteResult(controller.ControllerContext);

            controller.Response.End();
        }

        public static object ReturnJsonAuthorizationError(this Controller controller)
        {
            LogAndRaiseAuthorizationError(controller);
            return new { ResultText = "You are not authorized for the requested action, please contact the system administrator." };
        }
    }
}