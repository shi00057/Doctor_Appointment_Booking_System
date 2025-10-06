using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CST8002.Application.DTOs;

namespace CST8002.Application.Interfaces.Services
{
    public interface INotificationQueryService
    {
        //Task<IReadOnlyList<NotificationDto>> ListUnreadAsync(int userId, CancellationToken ct = default);
        Task<IReadOnlyList<NotificationDto>> ListForUserAsync(int userId, bool unreadOnly, int top, CancellationToken ct = default);
        Task MarkReadAsync(int userId, long notificationId, CancellationToken ct = default);
        Task MarkAllAsync(int userId, CancellationToken ct = default);
    }
}
