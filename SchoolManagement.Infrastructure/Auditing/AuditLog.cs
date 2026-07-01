using System;

namespace SchoolManagement.Infrastructure.Auditing
{
    public class AuditLog
    {
        public int Id { get; set; }

        public int SchoolId { get; set; }

        public int? UserId { get; set; }

        public string UserName { get; set; }

        public string Action { get; set; }

        public string EntityName { get; set; }

        public string EntityId { get; set; }

        public string OldValues { get; set; }

        public string NewValues { get; set; }

        public string IPAddress { get; set; }

        public string UserAgent { get; set; }

        public string TraceId { get; set; }

        public string CorrelationId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
