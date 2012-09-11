using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Customer.Project.Utilities
{
    public static class Extensions
    {
        public static string PropertiesToString(this object o)
        {
            return o.PropertiesToString(null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertiesToExclude">An array of property names that should not be added to the result string</param>
        public static string PropertiesToString(this object o, string[] propertiesToExclude)
        {
            StringBuilder propertiesString = new StringBuilder();
            foreach (var prop in o.GetType().GetProperties())
            {
                if (null != propertiesToExclude && propertiesToExclude.Contains(prop.Name))
                    continue;

                if (propertiesString.Length > 0)
                    propertiesString.Append(", ");

                propertiesString.Append(prop.Name);
                propertiesString.Append(": ");
                propertiesString.Append(prop.GetValue(o, null));
            }
            return propertiesString.ToString();
        }
    }
}
