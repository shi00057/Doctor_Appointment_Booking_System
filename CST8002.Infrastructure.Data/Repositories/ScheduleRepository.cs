using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using CST8002.Infrastructure.Data.Configuration;
using CST8002.Infrastructure.Data.Connection;
using CST8002.Application.Interfaces.Repositories;
using CST8002.Application.DTOs;

namespace CST8002.Infrastructure.Data.Repositories
{
    public sealed class ScheduleRepository : IScheduleRepository
    {
        private readonly ISqlConnectionFactory _factory;
        public ScheduleRepository(ISqlConnectionFactory factory) { _factory = factory; }

        public async Task AdminGenerateSlotsAsync(int doctorId, DateTime workDate, byte startHour, byte endHour, int? adminUserId, CancellationToken ct = default)
        {
            using var conn = await _factory.CreateOpenConnectionAsync(ct).ConfigureAwait(false);
            await conn.ExecuteAsync(
                SqlConstants.SpAdminGenerateSlots,
                new { DoctorId = doctorId, WorkDate = workDate.Date, StartHour = startHour, EndHour = endHour, AdminUserId = adminUserId },
                commandType: CommandType.StoredProcedure);
        }

        public async Task AdminGenerateSlotsRangeAsync(int doctorId, DateTime fromDate, DateTime toDate, byte startHour, byte endHour, int? adminUserId, CancellationToken ct = default)
        {
            using var conn = await _factory.CreateOpenConnectionAsync(ct).ConfigureAwait(false);
            await conn.ExecuteAsync(
                SqlConstants.SpAdminGenerateSlotsRange,
                new { DoctorId = doctorId, FromDate = fromDate.Date, ToDate = toDate.Date, StartHour = startHour, EndHour = endHour, AdminUserId = adminUserId },
                commandType: CommandType.StoredProcedure);
        }

        public async Task DoctorGenerateSlotsAsync(int doctorId, DateTime workDate, byte startHour, byte endHour, int? byUserId, CancellationToken ct = default)
        {
            using var conn = await _factory.CreateOpenConnectionAsync(ct).ConfigureAwait(false);
            await conn.ExecuteAsync(
                SqlConstants.SpDoctorGenerateSlots,
                new { DoctorId = doctorId, WorkDate = workDate.Date, StartHour = startHour, EndHour = endHour, ByUserId = byUserId, Source = "Doctor" },
                commandType: CommandType.StoredProcedure);
        }

        public async Task ClearSlotsRangeAsync(int doctorId, DateTime fromDate, DateTime toDate, CancellationToken ct = default)
        {
            using var conn = await _factory.CreateOpenConnectionAsync(ct).ConfigureAwait(false);
            await conn.ExecuteAsync(
                SqlConstants.SpClearSlotsRange,
                new { DoctorId = doctorId, FromDate = fromDate.Date, ToDate = toDate.Date },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<CST8002.Application.DTOs.ScheduleSlotDto>> ListAvailableSlotsAsync(int doctorId, DateTime dateUtc, CancellationToken ct = default)
        {
            using var conn = await _factory.CreateOpenConnectionAsync(ct).ConfigureAwait(false);
            var rows = await conn.QueryAsync<CST8002.Application.DTOs.ScheduleSlotDto>(
                SqlConstants.SpListAvailableSlots,
                new { DoctorId = doctorId, DateUtc = dateUtc },
                commandType: CommandType.StoredProcedure);
            return rows;
        }

    }

    public sealed class ScheduleSlotDto
    {
        public long SlotId { get; set; }
        public int DoctorId { get; set; }
        public DateTime StartUtc { get; set; }
        public DateTime EndUtc { get; set; }
    }
}
