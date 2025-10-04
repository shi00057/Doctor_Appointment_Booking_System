using CST8002.Application.DTOs;
using CST8002.Application.Interfaces.Repositories;
using CST8002.Infrastructure.Data.Configuration;
using CST8002.Infrastructure.Data.Connection;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace CST8002.Infrastructure.Data.Repositories
{
    public sealed class UserRepository : IUserRepository
    {
        private readonly ISqlConnectionFactory _factory;
        public UserRepository(ISqlConnectionFactory factory) { _factory = factory; }

        public async Task<int> RegisterPatientAsync(string email, byte[] passwordHash, byte[] salt, string fullName, string phone, CancellationToken ct = default)
        {
            using var conn = await _factory.CreateOpenConnectionAsync(ct).ConfigureAwait(false);
            var userId = await conn.QuerySingleAsync<int>(
                SqlConstants.SpRegisterPatient,
                new { Email = email, PasswordHash = passwordHash, Salt = salt, FullName = fullName, Phone = phone },
                commandType: CommandType.StoredProcedure);
            return userId;
        }

        //public async Task<(int UserId, string Role, bool IsActive)> LoginAsync(string email, byte[] passwordHash, CancellationToken ct = default)
        //{
        //    using var conn = await _factory.CreateOpenConnectionAsync(ct).ConfigureAwait(false);
        //    var row = await conn.QuerySingleAsync<(int UserId, string Role, bool IsActive)>(
        //        SqlConstants.SpLogin,
        //        new { Email = email, PasswordHash = passwordHash },
        //        commandType: CommandType.StoredProcedure);
        //    return row;
        //}
        //public async Task<byte[]> GetSaltByEmailAsync(string email, CancellationToken ct = default)
        //{
        //    using var conn = await _factory.CreateOpenConnectionAsync(ct).ConfigureAwait(false);
        //    var salt = await conn.QuerySingleOrDefaultAsync<byte[]>(
        //        SqlConstants.SpGetUserSaltByEmail,
        //        new { Email = email },
        //        commandType: CommandType.StoredProcedure);
        //    return salt ?? System.Array.Empty<byte>();
        //}
        public async Task<(int UserId, string Role, bool IsActive)> LoginAsync(string email, byte[] passwordHash, CancellationToken ct = default)
        {
            using var conn = await _factory.CreateOpenConnectionAsync(ct).ConfigureAwait(false);
            var p = new DynamicParameters();
            p.Add("@Email", email, DbType.String, size: 320);
            p.Add("@PasswordHash", passwordHash, DbType.Binary, size: 64);
            var row = await conn.QuerySingleAsync<(int UserId, string Role, bool IsActive)>(
                SqlConstants.SpLogin, p, commandType: CommandType.StoredProcedure);
            return row;
        }

        public async Task<byte[]> GetSaltByEmailAsync(string email, CancellationToken ct = default)
        {
            using var conn = await _factory.CreateOpenConnectionAsync(ct).ConfigureAwait(false);
            var p = new DynamicParameters();
            p.Add("@Email", email, DbType.String, size: 320);
            var salt = await conn.QuerySingleOrDefaultAsync<byte[]>(
                SqlConstants.SpGetUserSaltByEmail, p, commandType: CommandType.StoredProcedure);
            return salt ?? Array.Empty<byte>();
        }


        public async Task<int> ActivateUserAsync(int userId, bool isActive, CancellationToken ct = default)
        {
            using var conn = await _factory.CreateOpenConnectionAsync(ct).ConfigureAwait(false);
            var affected = await conn.ExecuteAsync(
                SqlConstants.SpActivateUser,
                new { UserId = userId, IsActive = isActive },
                commandType: CommandType.StoredProcedure);
            return affected;
        }

        public async Task<int> NotificationsCountUnreadAsync(int userId, CancellationToken ct = default)
        {
            using var conn = await _factory.CreateOpenConnectionAsync(ct).ConfigureAwait(false);
            var count = await conn.QuerySingleAsync<int>(
                SqlConstants.SpNotificationsCountUnread,
                new { UserId = userId },
                commandType: CommandType.StoredProcedure);
            return count;
        }
        public async Task<IEnumerable<NotificationDto>> NotificationsListForUserAsync(int userId, bool onlyUnread, int top, CancellationToken ct = default)
        {
            using var conn = await _factory.CreateOpenConnectionAsync(ct).ConfigureAwait(false);
            var rows = await conn.QueryAsync<NotificationDto>(
                SqlConstants.SpNotificationsListForUser,
                new { UserId = userId, OnlyUnread = onlyUnread, Top = top },
                commandType: CommandType.StoredProcedure);
            return rows;
        }
        public async Task NotificationsMarkReadAsync(int userId, long notificationId, CancellationToken ct = default)
        {
            using var conn = await _factory.CreateOpenConnectionAsync(ct).ConfigureAwait(false);
            await conn.ExecuteAsync(
                SqlConstants.SpNotificationsMarkRead,
                new { UserId = userId, NotificationId = notificationId },
                commandType: CommandType.StoredProcedure);
        }

        public async Task NotificationsDeleteAsync(int userId, long notificationId, CancellationToken ct = default)
        {
            using var conn = await _factory.CreateOpenConnectionAsync(ct).ConfigureAwait(false);
            await conn.ExecuteAsync(
                SqlConstants.SpNotificationsDelete,
                new { UserId = userId, NotificationId = notificationId },
                commandType: CommandType.StoredProcedure);
        }


    }

    public sealed class NotificationRow
    {
        public long NotificationId { get; set; }
        public string Type { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public byte Severity { get; set; }
        public string RelatedEntityType { get; set; }
        public long? RelatedEntityId { get; set; }
        public DateTime CreatedUtc { get; set; }
        public bool IsRead { get; set; }
        public DateTime? ReadUtc { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedUtc { get; set; }
    }
}
