using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace CST8002.Infrastructure.Data.Connection
{
    public interface ISqlConnectionFactory
    {
        int DefaultCommandTimeoutSeconds { get; }
        Task<IDbConnection> CreateOpenConnectionAsync(CancellationToken ct = default);
    }
}
