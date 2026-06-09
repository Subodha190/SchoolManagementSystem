using System;

namespace SchoolManagement.Domain.Common
{
    public abstract class AuditableEntity : BaseEntity
    {
        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        // Initialize string properties to non-null defaults to avoid NULL DB inserts
        public string CreatedBy { get; set; } = string.Empty;

        public string UpdatedBy { get; set; } = string.Empty;

        public bool IsDeleted { get; set; }

        public string TenantId { get; set; } = string.Empty;
    }
}
