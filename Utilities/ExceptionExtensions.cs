using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Customer.Project.Utilities
{
    public static class ExceptionExtensions
    {
        public static string ToFullException(this Exception ex)
        {
            System.String message = String.Empty;

            message = String.Format("{0}", ex.Message);
            if (ex.InnerException != null)
            {
                message += String.Format(", {0}", ToFullException(ex.InnerException));
            }
            return message;
        }
    }
}
