using System;

namespace SchoolManagement.Domain.Entities
{
    public class RefreshToken
    {
        public int Id { get; set; }

        public string Token { get; set; }

        public DateTime Expires { get; set; }

        public DateTime Created { get; set; } = DateTime.UtcNow;

        public string CreatedByIp { get; set; }

        public DateTime? Revoked { get; set; }

        public string? RevokedByIp { get; set; }

        public string? ReplacedByToken { get; set; }

        public int? ApplicationUserId { get; set; }

        // navigation
        public ApplicationUser ApplicationUser { get; set; }

        public bool IsActive => Revoked == null && !IsExpired;

        public bool IsExpired => DateTime.UtcNow >= Expires;
    }
}
