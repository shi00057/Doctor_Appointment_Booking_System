using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CST8002.Application.Abstractions;
using CST8002.Application.DTOs;
using CST8002.Application.Interfaces.Repositories;
using CST8002.Application.Interfaces.Services;

namespace CST8002.Application.Services
{
    public sealed class ScheduleService : IScheduleService
    {
        private readonly IScheduleRepository _repo;
        private readonly ICurrentUser _user;

        public ScheduleService(IScheduleRepository repo, ICurrentUser user)
        {
            _repo = repo;
            _user = user;
        }

        public Task DoctorGenerateSlotsAsync(GenerateSlotsRequest req, CancellationToken ct = default)
        {
            ValidateSlotsRequest(req);
            return _repo.DoctorGenerateSlotsAsync(req.DoctorId, req.WorkDate.Date, req.StartHour, req.EndHour, _user.UserId, ct);
        }

        public Task AdminGenerateSlotsAsync(GenerateSlotsRequest req, CancellationToken ct = default)
        {
            ValidateSlotsRequest(req);
            return _repo.AdminGenerateSlotsAsync(req.DoctorId, req.WorkDate.Date, req.StartHour, req.EndHour, _user.UserId, ct);
        }

        public Task AdminGenerateSlotsRangeAsync(int doctorId, DateTime fromDate, DateTime toDate, byte startHour, byte endHour, CancellationToken ct = default)
        {
            ValidateRange(doctorId, fromDate, toDate, startHour, endHour);
            return _repo.AdminGenerateSlotsRangeAsync(doctorId, fromDate.Date, toDate.Date, startHour, endHour, _user.UserId, ct);
        }
        public async Task<IEnumerable<ScheduleSlotDto>> ListAvailableSlotsAsync(
            int doctorId, DateTime workDate, CancellationToken ct = default)
        {
            if (doctorId <= 0) throw new ArgumentOutOfRangeException(nameof(doctorId));
            var result = await _repo.ListAvailableSlotsAsync(doctorId, workDate.Date, ct);
            return result ?? Array.Empty<ScheduleSlotDto>();
        }




        public Task ClearSlotsRangeAsync(int doctorId, DateTime fromDate, DateTime toDate, CancellationToken ct = default)
        {
            if (doctorId <= 0) throw new ArgumentOutOfRangeException(nameof(doctorId));
            if (fromDate.Date > toDate.Date) throw new ArgumentException("fromDate must be <= toDate");
            return _repo.ClearSlotsRangeAsync(doctorId, fromDate.Date, toDate.Date, ct);
        }

        private static void ValidateSlotsRequest(GenerateSlotsRequest req)
        {
            if (req is null) throw new ArgumentNullException(nameof(req));
            ValidateRange(req.DoctorId, req.WorkDate.Date, req.WorkDate.Date, req.StartHour, req.EndHour);
        }

        private static void ValidateRange(int doctorId, DateTime fromDate, DateTime toDate, byte startHour, byte endHour)
        {
            if (doctorId <= 0) throw new ArgumentOutOfRangeException(nameof(doctorId));
            if (fromDate.Date > toDate.Date) throw new ArgumentException("fromDate must be <= toDate");
            if (startHour > 23 || endHour > 24) throw new ArgumentOutOfRangeException("Hour must be within [0,24]");
            if (startHour >= endHour) throw new ArgumentException("StartHour must be < EndHour");
        }
    }
}
