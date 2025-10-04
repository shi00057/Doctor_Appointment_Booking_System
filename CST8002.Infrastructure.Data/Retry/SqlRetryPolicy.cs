using System;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace CST8002.Infrastructure.Data.Retry
{
    public interface ISqlRetryPolicy
    {
        Task<T> ExecuteAsync<T>(Func<CancellationToken, Task<T>> action, CancellationToken ct = default);
        Task ExecuteAsync(Func<CancellationToken, Task> action, CancellationToken ct = default);
    }

    public sealed class SqlRetryPolicy : ISqlRetryPolicy
    {
        private readonly int _maxRetries;
        private readonly TimeSpan _initialDelay;

        public SqlRetryPolicy(int maxRetries = 3, TimeSpan? initialDelay = null)
        {
            _maxRetries = Math.Max(0, maxRetries);
            _initialDelay = initialDelay ?? TimeSpan.FromMilliseconds(200);
        }

        public async Task<T> ExecuteAsync<T>(Func<CancellationToken, Task<T>> action, CancellationToken ct = default)
        {
            int attempt = 0;
            Exception? last = null;

            while (attempt <= _maxRetries && !ct.IsCancellationRequested)
            {
                try { return await action(ct).ConfigureAwait(false); }
                catch (Exception ex) when (IsTransient(ex))
                {
                    last = ex;
                    if (attempt == _maxRetries) break;
                    await Task.Delay(Backoff(attempt), ct).ConfigureAwait(false);
                    attempt++;
                }
            }
            throw last ?? new TimeoutException("SQL retry exceeded.");
        }

        public Task ExecuteAsync(Func<CancellationToken, Task> action, CancellationToken ct = default) =>
            ExecuteAsync<object>(async c => { await action(c).ConfigureAwait(false); return default!; }, ct);

        private TimeSpan Backoff(int attempt) => TimeSpan.FromMilliseconds(_initialDelay.TotalMilliseconds * Math.Pow(2, attempt));

        private static bool IsTransient(Exception ex)
        {
            if (ex is TimeoutException) return true;
            if (ex is DbException) return true;
            if (ex is SqlException se)
            {
                
                foreach (SqlError err in se.Errors)
                {
                    if (err.Number is -2 or 4060 or 10928 or 10929 or 40197 or 40501 or 40613)
                        return true;
                }
            }
            return false;
        }
    }
}
