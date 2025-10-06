using System.Threading;
using System.Threading.Tasks;

namespace CST8002.Application.Interfaces.Services
{
    public interface IUserContextService
    {
        Task<int?> GetDoctorIdAsync(int userId, CancellationToken ct = default);
        Task<int?> GetPatientIdAsync(int userId, CancellationToken ct = default);
    }
}
