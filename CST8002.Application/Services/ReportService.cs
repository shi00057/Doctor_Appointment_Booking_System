using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CST8002.Application.DTOs;
using CST8002.Application.Interfaces.Repositories;
using CST8002.Application.Interfaces.Services;

namespace CST8002.Application.Services
{
    public sealed class ReportService : IReportService
    {
        private readonly IAppointmentRepository _repo;

        public ReportService(IAppointmentRepository repo) { _repo = repo; }

        public Task<IEnumerable<DoctorAppointmentReportRowDto>> ReportDoctorAppointmentsAsync(int doctorId, DateTime fromUtc, DateTime toUtc, CancellationToken ct = default)
        {
            if (doctorId <= 0) throw new ArgumentOutOfRangeException(nameof(doctorId));
            if (fromUtc > toUtc) throw new ArgumentException("fromUtc must be <= toUtc");

            return _repo.ReportDoctorAppointmentsAsync(doctorId, fromUtc, toUtc, ct);
        }

        public Task<IEnumerable<DoctorAppointmentCsvRowDto>> ReportDoctorAppointmentsCsvAsync(int doctorId, DateTime fromUtc, DateTime toUtc, CancellationToken ct = default)
        {
            if (doctorId <= 0) throw new ArgumentOutOfRangeException(nameof(doctorId));
            if (fromUtc > toUtc) throw new ArgumentException("fromUtc must be <= toUtc");

            return _repo.ReportDoctorAppointmentsCsvAsync(doctorId, fromUtc, toUtc, ct);
        }

        public Task<DoctorTotalsDto> ReportDoctorTotalsAsync(int doctorId, DateTime fromUtc, DateTime toUtc, CancellationToken ct = default)
        {
            if (doctorId <= 0) throw new ArgumentOutOfRangeException(nameof(doctorId));
            if (fromUtc > toUtc) throw new ArgumentException("fromUtc must be <= toUtc");

            return _repo.ReportDoctorTotalsAsync(doctorId, fromUtc, toUtc, ct);
        }
    }
}
