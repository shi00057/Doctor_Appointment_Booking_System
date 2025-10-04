using System;

namespace CST8002.Infrastructure.Data.Configuration
{
    public sealed class DbOptions
    {
        public string ConnectionString { get; set; } = string.Empty;
        public int CommandTimeoutSeconds { get; set; } = 30;
        public bool MatchNamesWithUnderscores { get; set; } = true;

        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(ConnectionString))
                throw new InvalidOperationException("Database:ConnectionString is missing.");
            if (CommandTimeoutSeconds <= 0) CommandTimeoutSeconds = 30;
        }
    }
}
