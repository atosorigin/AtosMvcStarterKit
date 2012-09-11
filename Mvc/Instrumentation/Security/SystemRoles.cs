using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Customer.Project.Mvc.Instrumentation.Security
{
    [Flags]
    public enum SystemRoles
    {
        None = 0,
        Administrator = 1,
        DemoRole1 = 2,
        DemoRole2 = 4,
    }
}