
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CST8002.Application.DTOs;

namespace CST8002.Application.Interfaces.Repositories
{
    public interface IScheduleRepository
    {
        Task AdminGenerateSlotsAsync(int doctorId, DateTime workDate, byte startHour, byte endHour, int? adminUserId, CancellationToken ct = default);
        Task AdminGenerateSlotsRangeAsync(int doctorId, DateTime fromDate, DateTime toDate, byte startHour, byte endHour, int? adminUserId, CancellationToken ct = default);
        Task DoctorGenerateSlotsAsync(int doctorId, DateTime workDate, byte startHour, byte endHour, int? byUserId, CancellationToken ct = default);
        Task<IEnumerable<ScheduleSlotDto>> ListAvailableSlotsAsync(int doctorId, DateTime workDate, CancellationToken ct = default);
        Task ClearSlotsRangeAsync(int doctorId, DateTime fromDate, DateTime toDate, CancellationToken ct = default);
    }
}
