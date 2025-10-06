using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using CST8002.Application.DTOs;
using CST8002.Application.Interfaces.Repositories;
using CST8002.Application.Interfaces.Services;

namespace CST8002.Application.Services
{
    public sealed class NotificationQueryService : INotificationQueryService
    {
        private readonly IUserRepository _users;

        public NotificationQueryService(IUserRepository users)
        {
            _users = users;
        }

        public async Task<IReadOnlyList<NotificationDto>> ListForUserAsync(int userId, bool unreadOnly, int top, CancellationToken ct = default)
        {
            var items = await _users.NotificationsListForUserAsync(userId, unreadOnly, top, ct);
            return items is IReadOnlyList<NotificationDto> ro ? ro : items.ToList();
        }

        public Task MarkReadAsync(int userId, long notificationId, CancellationToken ct = default)
        {
            return _users.NotificationsMarkReadAsync(userId, notificationId, ct);
        }

        public Task MarkAllAsync(int userId, CancellationToken ct = default)
        {
            return _users.NotificationsMarkAllAsync(userId, ct);
        }
    }
}
