using System.Collections.Generic;

namespace CST8002.Application.DTOs
{
    public sealed class PagedResult<T>
    {
        public IReadOnlyList<T> Items { get; init; }
        public int TotalCount { get; init; }
        public int PageNumber { get; init; }
        public int PageSize { get; init; }
    }
}