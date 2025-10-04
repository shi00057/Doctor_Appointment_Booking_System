using CST8002.Application.Abstractions;

namespace CST8002.Web.Infrastructure
{
    public sealed class SystemClock : IDateTimeProvider
    {
        public DateTime UtcNow => DateTime.UtcNow;
    }
}
