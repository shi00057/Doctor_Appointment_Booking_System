
namespace CST8002.Application.DTOs
{
    public sealed class NotificationDto
    {
        public long NotificationId { get; set; }
        public string Type { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public byte Severity { get; set; }
        public string RelatedEntityType { get; set; }
        public long? RelatedEntityId { get; set; }
        public System.DateTime CreatedUtc { get; set; }
        public bool IsRead { get; set; }
        public System.DateTime? ReadUtc { get; set; }
        public bool IsDeleted { get; set; }
        public System.DateTime? DeletedUtc { get; set; }
    }
}
