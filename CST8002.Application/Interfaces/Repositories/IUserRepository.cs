using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CST8002.Application.DTOs;

namespace CST8002.Application.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<int> RegisterPatientAsync(string email, byte[] passwordHash, byte[] salt, string fullName, string phone, CancellationToken ct = default);
        Task<(int UserId, string Role, bool IsActive)> LoginAsync(string email, byte[] passwordHash, CancellationToken ct = default);
        Task<byte[]> GetSaltByEmailAsync(string email, CancellationToken ct = default);
        Task<int> ActivateUserAsync(int userId, bool isActive, CancellationToken ct = default);
        Task<int> NotificationsCountUnreadAsync(int userId, CancellationToken ct = default);
        Task<IEnumerable<NotificationDto>> NotificationsListForUserAsync(int userId, bool onlyUnread, int top, CancellationToken ct = default);
        Task NotificationsMarkReadAsync(int userId, long notificationId, CancellationToken ct = default);
        Task NotificationsDeleteAsync(int userId, long notificationId, CancellationToken ct = default);
    }
}
