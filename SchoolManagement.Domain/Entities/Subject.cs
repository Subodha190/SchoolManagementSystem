using SchoolManagement.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace SchoolManagement.Domain.Entities
{
    public class Subject : AuditableEntity
    {
        public string SubjectCode { get; set; }

        public string Name { get; set; }

        public int Credits { get; set; }
    }
}
