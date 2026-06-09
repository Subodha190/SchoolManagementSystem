using System;

namespace SchoolManagement.Application.Common.Models
{
    public class CreateTeacherDto
    {
        public string EmployeeCode { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public decimal Salary { get; set; }
        public string SubjectSpecialization { get; set; } = string.Empty;
    }

    public class UpdateTeacherDto : CreateTeacherDto
    {
        public int Id { get; set; }
    }

    public class TeacherResponseDto
    {
        public int Id { get; set; }
        public string EmployeeCode { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public decimal Salary { get; set; }
        public string SubjectSpecialization { get; set; } = string.Empty;
    }
}
