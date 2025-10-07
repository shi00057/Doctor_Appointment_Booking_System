using System;
using System.Collections.Generic;
using CST8002.Web.ViewModels; 

namespace CST8002.Web.Areas.Doctor.ViewModels
{
    public sealed class DoctorDashboardIndexVm
    {
        public string DisplayName { get; init; } = "";
        public DateTime Today { get; init; } = DateTime.Today;
        public int UnreadCount { get; init; }
        public IReadOnlyList<NotificationVm> Unread { get; init; } = Array.Empty<NotificationVm>();
    }
}
