
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CST8002.Application.DTOs;

namespace CST8002.Application.Interfaces.Repositories
{
    public interface IAppointmentRepository
    {
        Task<(long ApptId, string Status)> BookAsync(int doctorId, int patientId, DateTime startUtc, DateTime endUtc, int? byUserId, CancellationToken ct = default);
        Task CancelByPatientAsync(long apptId, int patientId, int? byUserId, CancellationToken ct = default);
        Task AdminCancelAsync(long apptId, int adminUserId, string reason, CancellationToken ct = default);
        Task<IEnumerable<AppointmentDto>> ListByDoctorAsync(int doctorId, DateTime? fromUtc, DateTime? toUtc, CancellationToken ct = default);
        Task<IEnumerable<AppointmentDto>> ListByPatientAsync(int patientId, DateTime? fromUtc, DateTime? toUtc, CancellationToken ct = default);
        Task<IEnumerable<DoctorAppointmentReportRowDto>> ReportDoctorAppointmentsAsync(int doctorId, DateTime fromUtc, DateTime toUtc, CancellationToken ct = default);
        Task<IEnumerable<DoctorAppointmentCsvRowDto>> ReportDoctorAppointmentsCsvAsync(int doctorId, DateTime fromUtc, DateTime toUtc, CancellationToken ct = default);
        Task<DoctorTotalsDto> ReportDoctorTotalsAsync(int doctorId, DateTime fromUtc, DateTime toUtc, CancellationToken ct = default);
    }
}
