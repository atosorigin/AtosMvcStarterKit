using System;
using System.Text;
using System.Web.Mvc;

namespace Customer.Project.Utilities.Web.Paging
{
    public static class PagingHelpers
    {
        public static MvcHtmlString PageLinks(this HtmlHelper html,
        PagingInfo pagingInfo,
        Func<int, string> pageUrl)
        {
            StringBuilder result = new StringBuilder();
            for (int i = 1; i <= pagingInfo.TotalPages; i++)
            {
                TagBuilder tag = new TagBuilder("a"); // Construct an <a> tag
                tag.MergeAttribute("href", pageUrl(i));
                if (i == pagingInfo.CurrentPage)
                { 
                    tag.AddCssClass("selected");
                }
                tag.InnerHtml = i.ToString();

                result.Append(tag.ToString());
            }

            if (pagingInfo.TotalPages > 0)
            {
                // Previous page
                TagBuilder tagPrevious = new TagBuilder("a"); // Construct an <a> tag
                if (pagingInfo.CurrentPage > 1)
                    tagPrevious.MergeAttribute("href", pageUrl(pagingInfo.CurrentPage - 1));
                tagPrevious.InnerHtml = "&#60;";

                result.Insert(0, tagPrevious.ToString());

                // Next page
                TagBuilder tagNext = new TagBuilder("a"); // Construct an <a> tag

                if (pagingInfo.CurrentPage < pagingInfo.TotalPages)
                    tagNext.MergeAttribute("href", pageUrl(pagingInfo.CurrentPage + 1));

                tagNext.InnerHtml = "&#62;";

                result.Append(tagNext.ToString());

            }
            return MvcHtmlString.Create(result.ToString());
        }
    }
}