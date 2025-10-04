using System;

namespace CST8002.Application.Abstractions
{
    public interface IDateTimeProvider
    {
        DateTime UtcNow { get; }
    }
}
