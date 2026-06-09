using System;

namespace SchoolManagement.Application.Common.Models
{
    public class CreateEnrollmentDto
    {
        public int StudentId { get; set; }
        public int CourseId { get; set; }
    }

    public class UpdateEnrollmentDto : CreateEnrollmentDto
    {
        public int Id { get; set; }
    }

    public class EnrollmentResponseDto
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int CourseId { get; set; }
        public DateTime EnrollmentDate { get; set; }
        public int Grade { get; set; }
    }
}
