using SchoolManagement.Domain.Common;

namespace SchoolManagement.Domain.Entities
{
    public class Course : AuditableEntity
    {
        public string CourseCode { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Fees { get; set; }

        public int DurationInMonths { get; set; }

        public ICollection<Student> Students { get; set; }
    }
}
