using SchoolManagement.Domain.Common;

namespace SchoolManagement.Domain.Entities
{
    public class Teacher : AuditableEntity
    {
        public string EmployeeCode { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public decimal Salary { get; set; }

        public string SubjectSpecialization { get; set; }
    }
}
