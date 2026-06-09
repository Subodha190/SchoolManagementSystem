using SchoolManagement.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace SchoolManagement.Domain.Entities
{
    public class Attendance : AuditableEntity
    {
        public int StudentId { get; set; }

        public Student Student { get; set; }

        public DateTime AttendanceDate { get; set; }

        public bool IsPresent { get; set; }
    }
}
