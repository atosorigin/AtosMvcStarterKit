using System;
using System.Web.Mvc;
using Customer.Project.Utilities;
using Elmah;
using StructureMap.Attributes;

namespace Customer.Project.Mvc.Instrumentation
{
    public class LogRequestAttribute : ActionFilterAttribute, IExceptionFilter
    {
        [SetterProperty]
        public IFormatLogger Logger { get; set; }

        private LogLevel _logLevel = LogLevel.Info;
        /// <summary>
        /// Log level, default = LogLevel.Info
        /// </summary>
        public LogLevel LogLevel
        {
            get { return _logLevel; }
            set { _logLevel = value; }
        }

        public LogRequestAttribute()
        {
        }

        private void LogStart(ActionExecutedContext filterContext)
        {
            // only log when the modelstate is valid
            if (filterContext.Controller.ViewData.ModelState.IsValid)
            {
                string message = RequestDetailsParser.Get(filterContext.RouteData.Values
                    , filterContext.ActionDescriptor.ActionName
                    , filterContext.Controller.ControllerContext.Controller.GetType().Name
                    , RequestDetailsParser.GetRequestProperties(filterContext.HttpContext.Request)
                    , filterContext.HttpContext.Request);
                Logger.Log(this.LogLevel, message);
            }
        }

        private void LogError(ExceptionContext filterContext)
        {
            Logger.Error(filterContext.Exception
                , "Error in LogRequest action filter. " + RequestDetailsParser.GetRequestProperties(filterContext.RequestContext.HttpContext.Request));

            // throw generic error
            ErrorSignal.FromCurrentContext().Raise(filterContext.Exception);

            if (filterContext.Controller.ViewData.ModelState != null)
                filterContext.Controller.ViewData.ModelState.AddModelError("", filterContext.Exception.Message);
        }
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            LogStart(filterContext);
            base.OnActionExecuted(filterContext);
        }
        public void OnException(ExceptionContext filterContext)
        {
            LogError(filterContext);
        }
    }
}