using System.Data;
using System.Threading;
using System.Threading.Tasks;
using CST8002.Infrastructure.Data.Configuration;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;

namespace CST8002.Infrastructure.Data.Connection
{
    public sealed class SqlConnectionFactory : ISqlConnectionFactory
    {
        private readonly DbOptions _options;

        public SqlConnectionFactory(IOptions<DbOptions> options)
        {
            _options = options.Value;
            _options.Validate();
        }

        public int DefaultCommandTimeoutSeconds => _options.CommandTimeoutSeconds;

        public async Task<IDbConnection> CreateOpenConnectionAsync(CancellationToken ct = default)
        {
            var conn = new SqlConnection(_options.ConnectionString);
            await conn.OpenAsync(ct).ConfigureAwait(false);
            return conn;
        }
    }
}
