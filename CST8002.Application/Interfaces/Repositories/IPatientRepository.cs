
using System.Threading;
using System.Threading.Tasks;
using CST8002.Application.DTOs;

namespace CST8002.Application.Interfaces.Repositories
{
    public interface IPatientRepository
    {
        Task<PatientDto> UpdateAsync(int patientId, string fullName, string phone, CancellationToken ct = default);
        Task<int> DeleteSoftAsync(int patientId, CancellationToken ct = default);
        Task<IEnumerable<PatientDto>> ListPendingActivationAsync(CancellationToken ct = default);
        Task<PatientDto?> GetAsync(int patientId, CancellationToken ct = default);
    }
}
