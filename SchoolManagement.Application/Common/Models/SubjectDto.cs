using System;

namespace SchoolManagement.Application.Common.Models
{
    public class CreateSubjectDto
    {
        public string SubjectCode { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int Credits { get; set; }
    }

    public class UpdateSubjectDto : CreateSubjectDto
    {
        public int Id { get; set; }
    }

    public class SubjectResponseDto
    {
        public int Id { get; set; }
        public string SubjectCode { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int Credits { get; set; }
    }
}
