using System.Threading;
using System.Threading.Tasks;
using CST8002.Application.DTOs;

namespace CST8002.Application.Interfaces.Services
{
    public interface IDoctorService
    {
        Task<(int UserId, int DoctorId)> CreateAsync(string email, byte[] passwordHash, byte[] salt, string name, string specialty, string phone, bool isActive, CancellationToken ct = default);
        Task<DoctorDto> UpdateAsync(DoctorDto dto, CancellationToken ct = default);
        Task<int> DeleteSoftAsync(int doctorId, CancellationToken ct = default);
        Task<IEnumerable<DoctorDto>> ListBasicsAsync(CancellationToken ct = default);
        Task<int> GetDoctorIdByUserIdAsync(CancellationToken ct = default);
    }
}
