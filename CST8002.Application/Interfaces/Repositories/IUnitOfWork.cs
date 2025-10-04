using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace CST8002.Application.Interfaces.Repositories
{
    public interface IUnitOfWork
    {
        IUnitOfWorkScope Current { get; }
        Task BeginAsync(CancellationToken ct = default);
        Task CommitAsync(CancellationToken ct = default);
        Task RollbackAsync(CancellationToken ct = default);
    }

    public interface IUnitOfWorkScope
    {
        IDbConnection Connection { get; }
        IDbTransaction Transaction { get; }
    }
}
