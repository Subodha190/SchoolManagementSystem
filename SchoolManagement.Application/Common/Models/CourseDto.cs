using System;

namespace SchoolManagement.Application.Common.Models
{
    public class CreateCourseDto
    {
        public string CourseCode { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Fees { get; set; }
        public int DurationInMonths { get; set; }
    }

    public class UpdateCourseDto : CreateCourseDto
    {
        public int Id { get; set; }
    }

    public class CourseResponseDto
    {
        public int Id { get; set; }
        public string CourseCode { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Fees { get; set; }
        public int DurationInMonths { get; set; }
    }
}
