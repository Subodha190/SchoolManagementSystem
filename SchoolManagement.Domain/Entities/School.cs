using SchoolManagement.Domain.Common;

namespace SchoolManagement.Domain.Entities
{
    public class School : AuditableEntity
    {
        public string Name { get; set; } = string.Empty;
    }
}
