using System.Collections.Generic;
using System.Configuration;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;
using System.Text;
using System.Web.UI;
using System.Globalization;
using System.Web.UI.HtmlControls;

namespace Customer.Project.Mvc.HtmlHelpers
{
    public static class MenuHelper
    {
        /// <summary>
        /// </summary>
        /// <param name="html">extends HtmlHelper</param>
        /// <returns>Html to render the menu</returns>
        public static MvcHtmlString Menu(this HtmlHelper html)
        {
            return Menu(html, "items", true);
        }

        /// <summary>
        /// Creates a PIP compliant menu based on the configured SiteMap
        /// </summary>
        /// <param name="html">extends HtmlHelper</param>
        /// <param name="itemArrayName">the name of the javascript itemArray</param>
        /// <param name="hideLeftNavigation">Indicates if the navigation is shown on the left side or not</param>
        /// <returns>Html to render the menu</returns>
        private static MvcHtmlString Menu(this HtmlHelper html, string itemArrayName, bool hideLeftNavigation)
        {
            Menu menu = new Menu();
            IHierarchicalEnumerable dataSource = SiteMap.RootNode.ChildNodes;
            return MvcHtmlString.Create(menu.Render((IHierarchicalEnumerable)dataSource));
        }
    }

    internal class Menu
    {
        #region Constants

        private const string PagesCollection = "PagesCollection";

        #endregion
        
        /// <summary>
        /// Gets the user accessible pages.
        /// </summary>
        /// <value>The user accessible sites.</value>
        public Dictionary<string, string> Pages
        {
            get
            {
                if (HttpContext.Current.Cache[PagesCollection] == null)
                {
                    HttpContext.Current.Cache.Add(PagesCollection,
                            new Dictionary<string, string>(), null, Cache.NoAbsoluteExpiration,
                            Cache.NoSlidingExpiration, CacheItemPriority.Default, null);
                }
                return HttpContext.Current.Cache[PagesCollection] as Dictionary<string, string>;
            }
        }

        /// <summary>
        /// Renders the menu
        /// </summary>
        /// <param name="menuItems"></param>
        /// <returns></returns>
        public string Render(IHierarchicalEnumerable menuItems)
        {
            StringBuilder menuDiv = new StringBuilder();
            menuDiv.AppendLine("<div id=\"myslidemenu\" class=\"jqueryslidemenu\">");
            RenderMenuItems(menuDiv, menuItems, 0);
            menuDiv.AppendLine("<br style=\"clear: left\" />");
            menuDiv.AppendLine("</div>");
            return menuDiv.ToString();
        }

        private void RenderMenuItems(StringBuilder sb, IHierarchicalEnumerable menuItems, int count)
        {
            if (menuItems == null)
                return;
    
            sb.AppendLine("<ul>");

            foreach (SiteMapNode node in menuItems)
            {
                sb.AppendFormat(CultureInfo.InvariantCulture, "<li><a href=\"{0}\">{1}</a>", node.Url, node.Title);
                sb.AppendLine();

                if (!Pages.ContainsKey(node.Key))
                {
                    Pages.Add(node.Key, count.ToString(new NumberFormatInfo()));
                }

                if (node.HasChildNodes)
                {
                    RenderMenuItems(sb, node.ChildNodes, 1);
                }

                if (node.HasChildNodes)
                    sb.AppendLine("</li>");

                count++;
            }
            sb.AppendLine("</ul>");

        }
    }
}
