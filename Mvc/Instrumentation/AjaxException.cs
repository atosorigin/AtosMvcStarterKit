using System;
using System.Net;
using System.Web.Mvc;

namespace Customer.Project.Mvc.Instrumentation
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class AjaxException : ActionFilterAttribute, IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            if (!filterContext.HttpContext.Request.IsAjaxRequest()) return;

            Report.LogAndRaiseErrorImplementation("Ajax call error: " + filterContext.Exception.Message);
            filterContext.Result = AjaxError(filterContext.Exception.Message, filterContext);

            //Let the system know that the exception has been handled
            filterContext.ExceptionHandled = true;
        }

        protected JsonResult AjaxError(string message, ExceptionContext filterContext)
        {
            //If message is null or empty, then fill with generic message
            if (String.IsNullOrEmpty(message))
                message = "Something went wrong while processing your request. Please refresh the page and try again.";

            //Set the response status code to 500
            filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            //Needed for IIS7.0
            filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;

            return new JsonResult
            {
                Data = new { ErrorMessage = message },
                ContentEncoding = System.Text.Encoding.UTF8,
                JsonRequestBehavior = JsonRequestBehavior.DenyGet
            };
        }
    }
}