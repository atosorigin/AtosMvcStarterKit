using System;

namespace Customer.Project.Utilities.Web.Paging
{
    public class PagingInfo
    {
        public int TotalItems { get; set; }
        public int ItemsPerPage { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages
        {
            get { return (int)Math.Ceiling((decimal)TotalItems / ItemsPerPage); }
        }

        /// <summary>
        /// puts CurrentPage in range 1 << CurrentPage << TotalPages
        /// </summary>
        public void EnsureValidCurrentPage()
        {
            if (CurrentPage < 1 || TotalPages == 0)
                CurrentPage = 1;
            else if (CurrentPage >= TotalPages)
                CurrentPage = TotalPages;
        }
    }
}