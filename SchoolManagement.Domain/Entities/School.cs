using SchoolManagement.Domain.Common;

namespace SchoolManagement.Domain.Entities
{
    public class School : AuditableEntity
    {
        public string SchoolCode { get; set; }

        public string SchoolName { get; set; }

        public string Email { get; set; }

        public bool IsActive { get; set; }

        public ICollection<ApplicationUser> Users { get; set; }
    }
}
