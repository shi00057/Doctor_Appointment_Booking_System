using System.Collections.Generic;

namespace CST8002.Web.ViewModels
{
    public class PagedResultVm<T>
    {
        public IReadOnlyList<T> Items { get; set; }
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
