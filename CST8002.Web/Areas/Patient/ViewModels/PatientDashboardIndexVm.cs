using System;

namespace CST8002.Web.Areas.Patient.ViewModels
{
    public sealed class PatientDashboardIndexVm
    {
        public string DisplayName { get; init; } = string.Empty;
        public DateTime Today { get; init; } = DateTime.Now;
        public int UnreadCount { get; init; }
    }
}
