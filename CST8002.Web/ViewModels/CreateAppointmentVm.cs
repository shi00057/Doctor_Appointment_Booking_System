using System.Collections.Generic;

namespace CST8002.Web.ViewModels
{
    public class CreateAppointmentVm
    {
        public int DoctorId { get; set; }
        public List<SlotVm> Slots { get; set; } = new();
    }
}
