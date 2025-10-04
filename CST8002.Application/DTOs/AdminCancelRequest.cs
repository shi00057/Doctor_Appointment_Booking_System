namespace CST8002.Application.DTOs
{
    public sealed class AdminCancelRequest
    {
        public long ApptId { get; set; }
        public string Reason { get; set; }
    }
}
