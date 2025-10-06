using CST8002.Application.DTOs;

namespace CST8002.Application.Interfaces.Services
{
    public interface IUserService
    {
        Task ActivateAsync(int userId, CancellationToken ct = default);
        Task<IEnumerable<PatientDto>> ListPendingPatientsAsync(CancellationToken ct = default);
        Task<DoctorDto?> GetDoctorByIdAsync(int doctorId, CancellationToken ct = default);
        Task<PatientDto?> GetPatientByIdAsync(int patientId, CancellationToken ct = default);
    }
}
