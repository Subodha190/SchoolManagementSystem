using System;

namespace SchoolManagement.Domain.Entities
{
    public class RefreshToken
    {
        public int Id { get; set; }

        // Only store hash of the token in the database for security
        public string TokenHash { get; set; }

        // Transient raw token - not mapped to DB. Used to return raw token to callers upon creation/rotation.
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]
        public string Token { get; set; }

        public DateTime Expires { get; set; }

        public DateTime Created { get; set; } = DateTime.UtcNow;

        public string CreatedByIp { get; set; }

        public DateTime? Revoked { get; set; }

        public string? RevokedByIp { get; set; }

        public string? ReplacedByToken { get; set; }

        public int? ApplicationUserId { get; set; }

        // Tenant (School) association for easier filtering and isolation
        public int SchoolId { get; set; }

        // navigation
        public ApplicationUser ApplicationUser { get; set; }

        public bool IsActive => Revoked == null && !IsExpired;

        public bool IsExpired => DateTime.UtcNow >= Expires;

        public DateTime? LastUsedAt { get; set; }
    }
}
