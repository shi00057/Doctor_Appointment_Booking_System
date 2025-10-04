using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CST8002.Application.DTOs;

namespace CST8002.Application.Interfaces.Services
{
    public interface IAppointmentService
    {
        Task<(long ApptId, string Status)> BookAsync(CreateAppointmentRequest req, CancellationToken ct = default);
        Task CancelByPatientAsync(long apptId, int patientId, CancellationToken ct = default);
        Task AdminCancelAsync(long apptId, string reason, CancellationToken ct = default);
        Task<IEnumerable<AppointmentDto>> ListByDoctorAsync(int doctorId, DateTime? fromUtc, DateTime? toUtc, CancellationToken ct = default);
        Task<IEnumerable<AppointmentDto>> ListByPatientAsync(int patientId, DateTime? fromUtc, DateTime? toUtc, CancellationToken ct = default);
    }
}
