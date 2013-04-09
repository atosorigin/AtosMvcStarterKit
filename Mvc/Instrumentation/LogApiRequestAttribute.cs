using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Customer.Project.Application;
using Customer.Project.Mvc.Instrumentation;
using Customer.Project.Utilities;

namespace Wwp.Mvc.Instrumentation
{
    public class LogApiRequestAttribute : ActionFilterAttribute
    {
        // does not work for web api
        //[SetterProperty]
        //public IFormatLogger Logger { get; set; }

        private LogLevel _logLevel = LogLevel.Info;
        /// <summary>
        /// Log level, default = LogLevel.Info
        /// </summary>
        public LogLevel LogLevel
        {
            get { return _logLevel; }
            set { _logLevel = value; }
        }
        public LogApiRequestAttribute()
        {
        }
        private void LogStart(HttpActionContext actionContext)
        {
            // only log when the modelstate is valid
            if (actionContext.ModelState.IsValid)
            {
                var request = (actionContext.Request.Properties["MS_HttpContext"] as HttpContextWrapper).Request;
                string message = RequestDetailsParser.Get(actionContext.ControllerContext.RouteData.Values
                    , actionContext.ActionDescriptor.ActionName
                    , actionContext.ControllerContext.Controller.GetType().Name
                    , RequestDetailsParser.GetRequestProperties(request)
                    , request);

                ServiceLocator.GetExisting<IFormatLogger>().Log(this.LogLevel, message);
            }
        }
        public override void OnActionExecuting(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            LogStart(actionContext);
            base.OnActionExecuting(actionContext);
        }
    }
}