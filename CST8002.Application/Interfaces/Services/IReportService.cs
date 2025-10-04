using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CST8002.Application.DTOs;

namespace CST8002.Application.Interfaces.Services
{
    public interface IReportService
    {
        Task<IEnumerable<DoctorAppointmentReportRowDto>> ReportDoctorAppointmentsAsync(int doctorId, DateTime fromUtc, DateTime toUtc, CancellationToken ct = default);
        Task<IEnumerable<DoctorAppointmentCsvRowDto>> ReportDoctorAppointmentsCsvAsync(int doctorId, DateTime fromUtc, DateTime toUtc, CancellationToken ct = default);
        Task<DoctorTotalsDto> ReportDoctorTotalsAsync(int doctorId, DateTime fromUtc, DateTime toUtc, CancellationToken ct = default);
    }
}
