using System;

namespace CST8002.Web.ViewModels
{
    public class NotificationVm
    {
        public int NotificationId { get; set; }
        public string Message { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}
