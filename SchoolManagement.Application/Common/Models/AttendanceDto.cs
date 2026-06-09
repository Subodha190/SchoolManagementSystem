using System;

namespace SchoolManagement.Application.Common.Models
{
    public class CreateAttendanceDto
    {
        public int StudentId { get; set; }
        public DateTime AttendanceDate { get; set; }
        public bool IsPresent { get; set; }
    }

    public class UpdateAttendanceDto : CreateAttendanceDto
    {
        public int Id { get; set; }
    }

    public class AttendanceResponseDto
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public DateTime AttendanceDate { get; set; }
        public bool IsPresent { get; set; }
    }
}
