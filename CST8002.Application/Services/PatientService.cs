using System;
using System.Threading;
using System.Threading.Tasks;
using CST8002.Application.DTOs;
using CST8002.Application.Interfaces.Repositories;
using CST8002.Application.Interfaces.Services;

namespace CST8002.Application.Services
{
    public sealed class PatientService : IPatientService
    {
        private readonly IPatientRepository _repo;

        public PatientService(IPatientRepository repo) => _repo = repo;
        public Task<PatientDto?> GetAsync(int patientId, CancellationToken ct = default)
        {
            if (patientId <= 0) throw new ArgumentOutOfRangeException(nameof(patientId));
            return _repo.GetAsync(patientId, ct);
        }

        public Task<PatientDto> UpdateAsync(PatientDto dto, CancellationToken ct = default)
        {
            if (dto is null) throw new ArgumentNullException(nameof(dto));
            if (dto.PatientId <= 0) throw new ArgumentOutOfRangeException(nameof(dto.PatientId));
            if (string.IsNullOrWhiteSpace(dto.FullName)) throw new ArgumentNullException(nameof(dto.FullName));

            return _repo.UpdateAsync(dto.PatientId, dto.FullName.Trim(), dto.Phone ?? string.Empty, ct);
        }

        public Task<int> DeleteSoftAsync(int patientId, CancellationToken ct = default)
        {
            if (patientId <= 0) throw new ArgumentOutOfRangeException(nameof(patientId));
            return _repo.DeleteSoftAsync(patientId, ct);
        }
    }
}
