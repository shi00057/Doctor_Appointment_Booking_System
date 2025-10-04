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
    public sealed class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepository _repo;
        private readonly ICurrentUser _user;
        private readonly IDateTimeProvider _clock;

        public AppointmentService(IAppointmentRepository repo, ICurrentUser user, IDateTimeProvider clock)
        {
            _repo = repo;
            _user = user;
            _clock = clock;
        }

        public async Task<(long ApptId, string Status)> BookAsync(CreateAppointmentRequest req, CancellationToken ct = default)
        {
            if (req is null) throw new ArgumentNullException(nameof(req));
            if (req.DoctorId <= 0) throw new ArgumentOutOfRangeException(nameof(req.DoctorId));
            if (req.PatientId <= 0) throw new ArgumentOutOfRangeException(nameof(req.PatientId));
            if (req.StartUtc >= req.EndUtc) throw new ArgumentException("StartUtc must be earlier than EndUtc.");

            
            var result = await _repo.BookAsync(
                req.DoctorId, req.PatientId, req.StartUtc, req.EndUtc, _user.UserId, ct).ConfigureAwait(false);

            return result; // (ApptId, Status)
        }

        public Task CancelByPatientAsync(long apptId, int patientId, CancellationToken ct = default)
        {
            if (apptId <= 0) throw new ArgumentOutOfRangeException(nameof(apptId));
            if (patientId <= 0) throw new ArgumentOutOfRangeException(nameof(patientId));

            return _repo.CancelByPatientAsync(apptId, patientId, _user.UserId, ct);
        }

        public Task AdminCancelAsync(long apptId, string reason, CancellationToken ct = default)
        {
            if (apptId <= 0) throw new ArgumentOutOfRangeException(nameof(apptId));
            reason ??= string.Empty;

            return _repo.AdminCancelAsync(apptId, _user.UserId, reason, ct);
        }

        public Task<IEnumerable<AppointmentDto>> ListByDoctorAsync(int doctorId, DateTime? fromUtc, DateTime? toUtc, CancellationToken ct = default)
        {
            if (doctorId <= 0) throw new ArgumentOutOfRangeException(nameof(doctorId));
            if (fromUtc.HasValue && toUtc.HasValue && fromUtc > toUtc) throw new ArgumentException("fromUtc must be <= toUtc");

            return _repo.ListByDoctorAsync(doctorId, fromUtc, toUtc, ct);
        }

        public Task<IEnumerable<AppointmentDto>> ListByPatientAsync(int patientId, DateTime? fromUtc, DateTime? toUtc, CancellationToken ct = default)
        {
            if (patientId <= 0) throw new ArgumentOutOfRangeException(nameof(patientId));
            if (fromUtc.HasValue && toUtc.HasValue && fromUtc > toUtc) throw new ArgumentException("fromUtc must be <= toUtc");

            return _repo.ListByPatientAsync(patientId, fromUtc, toUtc, ct);
        }
    }
}
