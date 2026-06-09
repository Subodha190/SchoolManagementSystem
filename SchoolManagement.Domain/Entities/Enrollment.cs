using SchoolManagement.Domain.Common;
using SchoolManagement.Domain.ValueObjects;

namespace SchoolManagement.Domain.Entities
{
    public class Enrollment : AuditableEntity
    {
        public int StudentId { get; set; }

        public Student Student { get; set; }

        public int CourseId { get; set; }

        public Course Course { get; set; }

        public DateTime EnrollmentDate { get; set; }
    }
}
