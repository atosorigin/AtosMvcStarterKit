using System;
using System.Web;
using System.Web.Routing;

namespace Customer.Project.Mvc.App_Start
{
	/// <summary>
	/// Route constraint that excludes the route if the action parameter is a Guid
	/// </summary>
	public class ExcludeGuidActionRouteConstraint : IRouteConstraint
	{
        public ExcludeGuidActionRouteConstraint()
		{
		}
		public bool Match(HttpContextBase httpContext, Route route, string parameterName, 
			RouteValueDictionary values, RouteDirection routeDirection)
		{
		    Guid g;
		    return !Guid.TryParse(values[parameterName].ToString(), out g);
		}
	}
}