namespace CST8002.Web.Areas.Patient.ViewModels
{
    public sealed class PatientProfileEditVm
    {
        public int PatientId { get; set; }
        public string FullName { get; set; } = "";
        public string Phone { get; set; } = "";
    }
}
