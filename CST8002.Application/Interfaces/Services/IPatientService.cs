using System.Threading;
using System.Threading.Tasks;
using CST8002.Application.DTOs;

namespace CST8002.Application.Interfaces.Services
{
    public interface IPatientService
    {
        Task<PatientDto> UpdateAsync(PatientDto dto, CancellationToken ct = default);
        Task<int> DeleteSoftAsync(int patientId, CancellationToken ct = default);
        Task<PatientDto?> GetAsync(int patientId, CancellationToken ct = default);
    }
}
