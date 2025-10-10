using System;
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
    public sealed class PatientRepository : IPatientRepository
    {
        private readonly ISqlConnectionFactory _factory;
        public async Task<PatientDto?> GetAsync(int patientId, CancellationToken ct = default)
        {
            using var conn = await _factory.CreateOpenConnectionAsync(ct).ConfigureAwait(false);
            var row = await conn.QuerySingleOrDefaultAsync<PatientDto>(
                SqlConstants.SpGetPatientById,
                new { PatientId = patientId },
                commandType: CommandType.StoredProcedure);
            return row;
        }
        public PatientRepository(ISqlConnectionFactory factory) { _factory = factory; }

        public async Task<PatientDto> UpdateAsync(int patientId, string fullName, string phone, CancellationToken ct = default)
        {
            using var conn = await _factory.CreateOpenConnectionAsync(ct).ConfigureAwait(false);
            var row = await conn.QuerySingleAsync<PatientDto>(
                SqlConstants.SpUpdatePatient,
                new { PatientId = patientId, FullName = fullName, Phone = phone },
                commandType: CommandType.StoredProcedure);
            return row;
        }

        public async Task<int> DeleteSoftAsync(int patientId, CancellationToken ct = default)
        {
            using var conn = await _factory.CreateOpenConnectionAsync(ct).ConfigureAwait(false);
            var affected = await conn.ExecuteAsync(
                SqlConstants.SpDeletePatientSoft,
                new { PatientId = patientId },
                commandType: CommandType.StoredProcedure);
            return affected;
        }
        public async Task<IEnumerable<PatientDto>> ListPendingActivationAsync(CancellationToken ct = default)
        {
            using var conn = await _factory.CreateOpenConnectionAsync(ct).ConfigureAwait(false);
            var rows = await conn.QueryAsync<PatientDto>(
                SqlConstants.SpListPatientsPendingActivation,   
                commandType: CommandType.StoredProcedure);
            return rows;
        }
    }
}
