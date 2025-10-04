using System;
using System.Collections.Generic;

namespace CST8002.Web.ViewModels
{
    public class DoctorDashboardVm
    {
        public string UserName { get; set; } = string.Empty;
        public DateTime Today { get; set; }
        public List<NotificationVm> Notifications { get; set; } = new();
    }
}
