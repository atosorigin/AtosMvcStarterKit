using System;
using System.Web;

namespace Customer.Project.Utilities.Web
{
    public static class StringExtensions
    {
        public static string ToSafeHtmlString(this string s)
        {
            string result = HttpUtility.HtmlEncode(s.Trim());
            return result
                            .Replace("  ", " ")
                            .Replace("\r\n", "<br/>");
        }
        public static bool Contains(this string source, string toCheck, StringComparison comp)
        {
            return source.IndexOf(toCheck, comp) >= 0;
        }
    }
}
