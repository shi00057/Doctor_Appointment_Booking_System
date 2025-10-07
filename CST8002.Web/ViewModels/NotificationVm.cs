using System;

namespace CST8002.Web.ViewModels
{
    public class NotificationVm
    {
        public long Id { get; init; }
        public string Message { get; init; } = "";
        public DateTime CreatedAt { get; init; }
        public bool IsRead { get; init; }
        public string? Severity { get; init; }
        public string? LinkText { get; init; }
        public string? LinkUrl { get; init; }
    }
}
