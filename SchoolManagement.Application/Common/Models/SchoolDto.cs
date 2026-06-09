using System;

namespace SchoolManagement.Application.Common.Models
{
    public class CreateSchoolDto
    {
        public string Name { get; set; } = string.Empty;
    }

    public class UpdateSchoolDto : CreateSchoolDto
    {
        public int Id { get; set; }
    }

    public class SchoolResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
