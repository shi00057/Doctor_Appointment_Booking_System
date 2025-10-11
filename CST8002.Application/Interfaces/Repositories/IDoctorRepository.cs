
using System.Threading;
using System.Threading.Tasks;
using CST8002.Application.DTOs;
using System.Collections.Generic;

namespace CST8002.Application.Interfaces.Repositories
{
    public interface IDoctorRepository
    {
        Task<(int UserId, int DoctorId)> CreateAsync(string email, byte[] passwordHash, byte[] salt, string name, string specialty, string phone, bool isActive, CancellationToken ct = default);
        Task<DoctorDto> UpdateAsync(int doctorId, string name, string specialty, string email, string phone, bool? isActive, CancellationToken ct = default);
        Task<int> DeleteSoftAsync(int doctorId, CancellationToken ct = default);
        Task<IEnumerable<DoctorDto>> ListBasicsAsync(CancellationToken ct = default);
        Task<int> GetDoctorIdByUserIdAsync(int userId, CancellationToken ct = default);
    }
}
