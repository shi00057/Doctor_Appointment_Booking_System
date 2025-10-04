#nullable enable
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CST8002.Application.Abstractions
{
    /// <summary>
    /// Publishes application-level notifications (email, in-app, etc.).
    /// This is an abstraction; concrete transports live in Infrastructure/Web.
    /// </summary>
    public interface INotificationPublisher
    {
        /// <summary>
        /// Publish a notification message.
        /// Keep the contract transport-agnostic (strings/metadata) to avoid coupling.
        /// </summary>
        /// <param name="type">Logical type, e.g. "AppointmentBooked", "SystemAlert".</param>
        /// <param name="title">Short human-readable title.</param>
        /// <param name="body">Message body (plain text or simple markdown).</param>
        /// <param name="severity">0=Info, 1=Warning, 2=Error, 3=Critical (caller-defined scale).</param>
        /// <param name="recipientUserIds">Optional target users; null means broadcast or implicit routing.</param>
        /// <param name="metadata">Optional key/value metadata for templating or links.</param>
        /// <param name="ct">Cancellation token.</param>
        Task PublishAsync(
            string type,
            string title,
            string body,
            int severity = 0,
            IEnumerable<int>? recipientUserIds = null,
            IReadOnlyDictionary<string, string>? metadata = null,
            CancellationToken ct = default);
    }
}
