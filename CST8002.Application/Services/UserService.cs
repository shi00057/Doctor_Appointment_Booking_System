using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CST8002.Application.DTOs;
using CST8002.Application.Interfaces.Repositories;
using CST8002.Application.Interfaces.Services;

namespace CST8002.Application.Services
{
    public sealed class UserService : IUserService
    {
        private readonly IUserRepository _users;
        private readonly IDoctorRepository _doctors;
        private readonly IPatientRepository _patients;

        public UserService(
            IUserRepository users,
            IDoctorRepository doctors,
            IPatientRepository patients)
        {
            _users = users;
            _doctors = doctors;
            _patients = patients;
        }

        public async Task ActivateAsync(int userId, CancellationToken ct = default)
        {
            await _users.ActivateUserAsync(userId, true, ct);
        }

        public async Task<IEnumerable<PatientDto>> ListPendingPatientsAsync(CancellationToken ct = default)
        {
            var rows = await _patients.ListPendingActivationAsync(ct);
            return rows ?? Enumerable.Empty<PatientDto>();
        }

        public async Task<DoctorDto?> GetDoctorByIdAsync(int doctorId, CancellationToken ct = default)
        {
     
            var rows = await _doctors.ListBasicsAsync(ct);
            return rows?.FirstOrDefault(d => d.DoctorId == doctorId);
        }

        public Task<PatientDto?> GetPatientByIdAsync(int patientId, CancellationToken ct = default)
        {
            return _patients.GetAsync(patientId, ct);
        }
    }
}
