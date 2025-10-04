using System;

namespace CST8002.Application.Domain.ValueObjects
{
    public readonly struct TimeRange
    {
        public DateTime StartUtc { get; }
        public DateTime EndUtc { get; }

        public TimeRange(DateTime startUtc, DateTime endUtc)
        {
            StartUtc = startUtc;
            EndUtc = endUtc;
        }

        public bool Contains(DateTime utc) => utc >= StartUtc && utc <= EndUtc;
    }
}