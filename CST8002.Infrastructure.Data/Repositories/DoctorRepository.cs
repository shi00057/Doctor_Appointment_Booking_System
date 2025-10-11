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
    public sealed class DoctorRepository : IDoctorRepository
    {
        private readonly ISqlConnectionFactory _factory;
        public DoctorRepository(ISqlConnectionFactory factory) { _factory = factory; }

        public async Task<(int UserId, int DoctorId)> CreateAsync(string email, byte[] passwordHash, byte[] salt, string name, string specialty, string phone, bool isActive, CancellationToken ct = default)
        {
            using var conn = await _factory.CreateOpenConnectionAsync(ct).ConfigureAwait(false);
            var result = await conn.QuerySingleAsync<(int UserId, int DoctorId)>(
                SqlConstants.SpCreateDoctor,
                new { Email = email, PasswordHash = passwordHash, Salt = salt, Name = name, Specialty = specialty, Phone = phone, IsActive = isActive },
                commandType: CommandType.StoredProcedure);
            return result;
        }

        public async Task<DoctorDto> UpdateAsync(int doctorId, string name, string specialty, string email, string phone, bool? isActive, CancellationToken ct = default)
        {
            using var conn = await _factory.CreateOpenConnectionAsync(ct).ConfigureAwait(false);
            var row = await conn.QuerySingleAsync<DoctorDto>(
                SqlConstants.SpUpdateDoctor,
                new { DoctorId = doctorId, Name = name, Specialty = specialty, Email = email, Phone = phone, IsActive = isActive },
                commandType: CommandType.StoredProcedure);
            return row;
        }

        public async Task<int> DeleteSoftAsync(int doctorId, CancellationToken ct = default)
        {
            using var conn = await _factory.CreateOpenConnectionAsync(ct).ConfigureAwait(false);
            var affected = await conn.ExecuteAsync(
                SqlConstants.SpDeleteDoctorSoft,
                new { DoctorId = doctorId },
                commandType: CommandType.StoredProcedure);
            return affected;
        }

        public async Task<IEnumerable<DoctorDto>> ListBasicsAsync(CancellationToken ct = default)
        {
            using var conn = await _factory.CreateOpenConnectionAsync(ct).ConfigureAwait(false);
            var rows = await conn.QueryAsync<DoctorDto>(
                SqlConstants.SpListDoctorsBasic,
                commandType: CommandType.StoredProcedure);
            return rows;
        }
        public async Task<int> GetDoctorIdByUserIdAsync(int userId, CancellationToken ct = default)
        {
            using var conn = await _factory.CreateOpenConnectionAsync(ct).ConfigureAwait(false);
            var p = new DynamicParameters();
            p.Add("@UserId", userId, DbType.Int32);
            var id = await conn.ExecuteScalarAsync<int?>(SqlConstants.SpGetDoctorIdByUserId, p, commandType: CommandType.StoredProcedure);
            return id ?? 0;
        }

    }
}
