using CST8002.Application.Domain.Entities;

namespace CST8002.Infrastructure.Data.Mappers
{
    public static class AppointmentMapper
    {
        public static Appointment WithPatient(Appointment a, Patient p)
        {
            a.Patient = p;
            return a;
        }

        public static Appointment WithDoctor(Appointment a, Doctor d)
        {
            a.Doctor = d;
            return a;
        }
    }
}
