using CST8002.Application.Domain.Entities;
using CST8002.Application.DTOs;
using CST8002.Application.Interfaces.Repositories;
using CST8002.Infrastructure.Data.Configuration;
using CST8002.Infrastructure.Data.Connection;
using CST8002.Infrastructure.Data.Mappers;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace CST8002.Infrastructure.Data.Repositories
{
    public sealed class AppointmentRepository : IAppointmentRepository
    {
        private readonly ISqlConnectionFactory _factory;
        public AppointmentRepository(ISqlConnectionFactory factory) { _factory = factory; }

        public async Task<(long ApptId, string Status)> BookAsync(int doctorId, int patientId, DateTime startUtc, DateTime endUtc, int? byUserId, CancellationToken ct = default)
        {
            using var conn = await _factory.CreateOpenConnectionAsync(ct).ConfigureAwait(false);
            var result = await conn.QuerySingleAsync<(long ApptId, string Status)>(
                SqlConstants.SpBookAppointment,
                new { DoctorId = doctorId, PatientId = patientId, StartUtc = startUtc, EndUtc = endUtc, ByUserId = byUserId },
                commandType: CommandType.StoredProcedure);
            return result;
        }

        public async Task CancelByPatientAsync(long apptId, int patientId, int? byUserId, CancellationToken ct = default)
        {
            using var conn = await _factory.CreateOpenConnectionAsync(ct).ConfigureAwait(false);
            await conn.ExecuteAsync(
                SqlConstants.SpCancelAppointment,
                new { ApptId = apptId, PatientId = patientId, ByUserId = byUserId },
                commandType: CommandType.StoredProcedure);
        }

        public async Task AdminCancelAsync(long apptId, int adminUserId, string reason, CancellationToken ct = default)
        {
            using var conn = await _factory.CreateOpenConnectionAsync(ct).ConfigureAwait(false);
            await conn.ExecuteAsync(
                SqlConstants.SpAdminCancelAppointment,
                new { ApptId = apptId, AdminUserId = adminUserId, Reason = reason },
                commandType: CommandType.StoredProcedure);
        }
        public async Task<IEnumerable<Appointment>> ListByDoctorWithPatientAsync(
    int doctorId, DateTime? fromUtc, DateTime? toUtc, CancellationToken ct = default)
        {
            using var conn = await _factory.CreateOpenConnectionAsync(ct).ConfigureAwait(false);
            var rows = await conn.QueryAsync<Appointment, Patient, Appointment>(
                SqlConstants.SpListAppointmentsByDoctor,
                AppointmentMapper.WithPatient,
                new { DoctorId = doctorId, FromUtc = fromUtc ?? new DateTime(1900, 1, 1), ToUtc = toUtc ?? new DateTime(9999, 12, 31) },
                commandType: CommandType.StoredProcedure,
                splitOn: "PatKey"
            );
            return rows;
        }

        public async Task<IEnumerable<Appointment>> ListByPatientWithDoctorAsync(
            int patientId, DateTime? fromUtc, DateTime? toUtc, CancellationToken ct = default)
        {
            using var conn = await _factory.CreateOpenConnectionAsync(ct).ConfigureAwait(false);
            var rows = await conn.QueryAsync<Appointment, Doctor, Appointment>(
                SqlConstants.SpListAppointmentsByPatient,
                AppointmentMapper.WithDoctor,
                new { PatientId = patientId, FromUtc = fromUtc ?? new DateTime(1900, 1, 1), ToUtc = toUtc ?? new DateTime(9999, 12, 31) },
                commandType: CommandType.StoredProcedure,
                splitOn: "DocKey"
            );
            return rows;
        }


        public async Task<IEnumerable<AppointmentDto>> ListByDoctorAsync(int doctorId, DateTime? fromUtc, DateTime? toUtc, CancellationToken ct = default)
        {
            using var conn = await _factory.CreateOpenConnectionAsync(ct).ConfigureAwait(false);
            var rows = await conn.QueryAsync<AppointmentDto>(
                SqlConstants.SpListAppointmentsByDoctor,
                new { DoctorId = doctorId, FromUtc = fromUtc ?? new DateTime(1900, 1, 1), ToUtc = toUtc ?? new DateTime(9999, 12, 31) },
                commandType: CommandType.StoredProcedure);
            return rows;
        }

        public async Task<IEnumerable<AppointmentDto>> ListByPatientAsync(int patientId, DateTime? fromUtc, DateTime? toUtc, CancellationToken ct = default)
        {
            using var conn = await _factory.CreateOpenConnectionAsync(ct).ConfigureAwait(false);
            var rows = await conn.QueryAsync<AppointmentDto>(
                SqlConstants.SpListAppointmentsByPatient,
                new { PatientId = patientId, FromUtc = fromUtc ?? new DateTime(1900, 1, 1), ToUtc = toUtc ?? new DateTime(9999, 12, 31) },
                commandType: CommandType.StoredProcedure);
            return rows;
        }
        public async Task<IEnumerable<DoctorAppointmentReportRowDto>> ReportDoctorAppointmentsAsync(int doctorId, DateTime fromUtc, DateTime toUtc, CancellationToken ct = default)
        {
            using var conn = await _factory.CreateOpenConnectionAsync(ct).ConfigureAwait(false);
            return await conn.QueryAsync<DoctorAppointmentReportRowDto>(
                SqlConstants.SpReportDoctorAppointments,
                new { DoctorId = doctorId, FromUtc = fromUtc, ToUtc = toUtc },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<DoctorAppointmentCsvRowDto>> ReportDoctorAppointmentsCsvAsync(int doctorId, DateTime fromUtc, DateTime toUtc, CancellationToken ct = default)
        {
            using var conn = await _factory.CreateOpenConnectionAsync(ct).ConfigureAwait(false);
            return await conn.QueryAsync<DoctorAppointmentCsvRowDto>(
                SqlConstants.SpReportDoctorAppointmentsCsv,
                new { DoctorId = doctorId, FromUtc = fromUtc, ToUtc = toUtc },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<DoctorTotalsDto> ReportDoctorTotalsAsync(int doctorId, DateTime fromUtc, DateTime toUtc, CancellationToken ct = default)
        {
            using var conn = await _factory.CreateOpenConnectionAsync(ct).ConfigureAwait(false);
            return await conn.QuerySingleAsync<DoctorTotalsDto>(
                SqlConstants.SpReportDoctorTotals,
                new { DoctorId = doctorId, FromUtc = fromUtc, ToUtc = toUtc },
                commandType: CommandType.StoredProcedure);
        }
        //public async Task<IEnumerable<AppointmentDto>> ListByDoctorAsync(int doctorId, DateTime? fromUtc, DateTime? toUtc, CancellationToken ct = default)
        //{
        //    using var conn = await _factory.CreateOpenConnectionAsync(ct);
        //    return await conn.QueryAsync<AppointmentDto>(
        //        SqlConstants.SpListAppointments,
        //        new { DoctorId = doctorId, PatientId = (int?)null, FromUtc = fromUtc ?? new DateTime(1900, 1, 1), ToUtc = toUtc ?? new DateTime(9999, 12, 31) },
        //        commandType: CommandType.StoredProcedure);
        //}

        //public async Task<IEnumerable<AppointmentDto>> ListByPatientAsync(int patientId, DateTime? fromUtc, DateTime? toUtc, CancellationToken ct = default)
        //{
        //    using var conn = await _factory.CreateOpenConnectionAsync(ct);
        //    return await conn.QueryAsync<AppointmentDto>(
        //        SqlConstants.SpListAppointments,
        //        new { DoctorId = (int?)null, PatientId = patientId, FromUtc = fromUtc ?? new DateTime(1900, 1, 1), ToUtc = toUtc ?? new DateTime(9999, 12, 31) },
        //        commandType: CommandType.StoredProcedure);
        //}
    }


    public sealed class DoctorAppointmentReportRow
    {
        public long ApptId { get; set; }
        public DateTime StartUtc { get; set; }
        public DateTime EndUtc { get; set; }
        public int DurationMinutes { get; set; }
        public string Status { get; set; }
        public string PatientName { get; set; }
    }

    public sealed class DoctorAppointmentCsvRow
    {
        public long ApptId { get; set; }
        public DateTime StartUtc { get; set; }
        public DateTime EndUtc { get; set; }
        public int DurationMinutes { get; set; }
        public string Status { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
    }

    public sealed class DoctorTotalsRow
    {
        public int TotalAppointments { get; set; }
        public int? TotalMinutes { get; set; }
        public decimal? TotalHours { get; set; }
    }
}
