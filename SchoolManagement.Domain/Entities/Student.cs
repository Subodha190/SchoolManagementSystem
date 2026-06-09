using SchoolManagement.Domain.Common;
using SchoolManagement.Domain.ValueObjects;
using SchoolManagement.Domain.Enums;

namespace SchoolManagement.Domain.Entities
{
    public class Student : AuditableEntity
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string Address { get; set; }

        public int CourseId { get; set; }

        public Course Course { get; set; }

        public StudentStatus Status { get; set; }
    }

}
