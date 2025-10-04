using System.Data;
using System.Threading;
using System.Threading.Tasks;
using CST8002.Application.Interfaces.Repositories;
using CST8002.Infrastructure.Data.Connection;

namespace CST8002.Infrastructure.Data.Transactions
{
    public sealed class SqlUnitOfWork : IUnitOfWork, System.IDisposable
    {
        private readonly ISqlConnectionFactory _factory;
        private IDbConnection _conn;
        private IDbTransaction _tx;

        public SqlUnitOfWork(ISqlConnectionFactory factory) => _factory = factory;

        public IUnitOfWorkScope Current => _tx is null ? null : new Scope(_conn, _tx);

        public async Task BeginAsync(CancellationToken ct = default)
        {
            if (_tx != null) return;
            _conn = await _factory.CreateOpenConnectionAsync(ct).ConfigureAwait(false);
            _tx = _conn.BeginTransaction();
        }

        public Task CommitAsync(CancellationToken ct = default)
        {
            _tx?.Commit();
            _tx?.Dispose();
            _conn?.Dispose();
            _tx = null;
            _conn = null;
            return Task.CompletedTask;
        }

        public Task RollbackAsync(CancellationToken ct = default)
        {
            _tx?.Rollback();
            _tx?.Dispose();
            _conn?.Dispose();
            _tx = null;
            _conn = null;
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _tx?.Dispose();
            _conn?.Dispose();
        }

        private sealed class Scope : IUnitOfWorkScope
        {
            public Scope(IDbConnection c, IDbTransaction t) { Connection = c; Transaction = t; }
            public IDbConnection Connection { get; }
            public IDbTransaction Transaction { get; }
        }
    }
}
