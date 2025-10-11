using System;
using System.Threading;
using System.Threading.Tasks;
using CST8002.Application.Abstractions;
using CST8002.Application.DTOs;
using CST8002.Application.Interfaces.Repositories;
using CST8002.Application.Interfaces.Services;

namespace CST8002.Application.Services
{
    public sealed class DoctorService : IDoctorService
    {
        private readonly IDoctorRepository _repo;
        private readonly IDateTimeProvider _clock;
        private readonly ICurrentUser _user;

        public DoctorService(IDoctorRepository repo, ICurrentUser user, IDateTimeProvider clock)
        {
            _repo = repo;
            _user = user;
            _clock = clock;
        }

        public Task<(int UserId, int DoctorId)> CreateAsync(
            string email, byte[] passwordHash, byte[] salt,
            string name, string specialty, string phone, bool isActive,
            CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(email)) throw new ArgumentNullException(nameof(email));
            if (passwordHash is null || passwordHash.Length == 0) throw new ArgumentNullException(nameof(passwordHash));
            if (salt is null || salt.Length == 0) throw new ArgumentNullException(nameof(salt));
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));

            
            return _repo.CreateAsync(email.Trim(), passwordHash, salt, name.Trim(),
                specialty ?? string.Empty, phone ?? string.Empty, isActive, ct);
        }

        public Task<DoctorDto> UpdateAsync(DoctorDto dto, CancellationToken ct = default)
        {
            if (dto is null) throw new ArgumentNullException(nameof(dto));
            if (dto.DoctorId <= 0) throw new ArgumentOutOfRangeException(nameof(dto.DoctorId));
            if (string.IsNullOrWhiteSpace(dto.Name)) throw new ArgumentNullException(nameof(dto.Name));

            return _repo.UpdateAsync(dto.DoctorId, dto.Name.Trim(), dto.Specialty ?? string.Empty,
                                     dto.Email ?? string.Empty, dto.Phone ?? string.Empty, dto.IsActive, ct);
        }

        public Task<int> DeleteSoftAsync(int doctorId, CancellationToken ct = default)
        {
            if (doctorId <= 0) throw new ArgumentOutOfRangeException(nameof(doctorId));
            return _repo.DeleteSoftAsync(doctorId, ct);
        }

        public Task<IEnumerable<DoctorDto>> ListBasicsAsync(CancellationToken ct = default)
        {
            return _repo.ListBasicsAsync(ct);
        }
        public DoctorService(IDoctorRepository repo, ICurrentUser user)
        {
            _repo = repo;
            _user = user;
        }

        public Task<int> GetDoctorIdByUserIdAsync(CancellationToken ct = default)
            => _repo.GetDoctorIdByUserIdAsync(_user.UserId, ct);

    }
}
