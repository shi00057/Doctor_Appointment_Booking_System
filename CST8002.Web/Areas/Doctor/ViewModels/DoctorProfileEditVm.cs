namespace CST8002.Web.Areas.Doctor.ViewModels
{
    public sealed class DoctorProfileEditVm
    {
        public int DoctorId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Specialty { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public bool IsActive { get; set; }
    }
}
