using System;

namespace SchoolManagement.Application.Common.Models
{
    public class CreateSchoolDto
    {
        public string SchoolCode { get; set; }

        public string SchoolName { get; set; }

        public string Email { get; set; }

        public bool IsActive { get; set; }
    }

    public class UpdateSchoolDto : CreateSchoolDto
    {
        public int Id { get; set; }
    }

    public class SchoolResponseDto
    {
        public int Id { get; set; }
        public string SchoolCode { get; set; }

        public string SchoolName { get; set; }

        public string Email { get; set; }

        public bool IsActive { get; set; }
    }
}
