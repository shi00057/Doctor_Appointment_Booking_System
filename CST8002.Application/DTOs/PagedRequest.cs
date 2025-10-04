namespace CST8002.Application.DTOs
{
    public sealed class PagedRequest
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
        public string SortBy { get; set; }
        public bool Desc { get; set; }
    }
}