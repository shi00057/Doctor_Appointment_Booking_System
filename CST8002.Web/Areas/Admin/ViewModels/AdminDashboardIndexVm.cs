namespace CST8002.Web.Areas.Admin.ViewModels
{
    public sealed class AdminDashboardIndexVm
    {
        public string? UserName { get; init; }
        public DateTime Today { get; init; }
        public int PendingPatients { get; init; } 
        public int Doctors { get; init; }
        public int TodayAppointments { get; init; }
    }
}
