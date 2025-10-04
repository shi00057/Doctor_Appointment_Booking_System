using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CST8002.Application.DTOs;

namespace CST8002.Application.Interfaces.Services
{
    public interface IScheduleService
    {
        Task DoctorGenerateSlotsAsync(GenerateSlotsRequest req, CancellationToken ct = default);
        Task AdminGenerateSlotsAsync(GenerateSlotsRequest req, CancellationToken ct = default);
        Task AdminGenerateSlotsRangeAsync(int doctorId, DateTime fromDate, DateTime toDate, byte startHour, byte endHour, CancellationToken ct = default);
        Task<IEnumerable<ScheduleSlotDto>> ListAvailableSlotsAsync(int doctorId, DateTime workDate, CancellationToken ct = default);
        Task ClearSlotsRangeAsync(int doctorId, DateTime fromDate, DateTime toDate, CancellationToken ct = default);
    }
}
