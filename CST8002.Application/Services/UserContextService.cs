using System.Threading;
using System.Threading.Tasks;
using CST8002.Application.Interfaces.Repositories;
using CST8002.Application.Interfaces.Services;

namespace CST8002.Application.Services
{
    public sealed class UserContextService : IUserContextService
    {
        private readonly IUserRepository _users;

        public UserContextService(IUserRepository users) => _users = users;

        public Task<int?> GetDoctorIdAsync(int userId, CancellationToken ct = default)
            => _users.GetDoctorIdByUserIdAsync(userId, ct);

        public Task<int?> GetPatientIdAsync(int userId, CancellationToken ct = default)
            => _users.GetPatientIdByUserIdAsync(userId, ct);
    }
}
