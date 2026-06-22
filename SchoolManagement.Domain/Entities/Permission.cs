using System;
using System.Collections.Generic;
using System.Text;

namespace SchoolManagement.Domain.Entities
{
    public class Permission
    {
        public int Id { get; set; }

        public string ModuleName { get; set; }

        public string ActionName { get; set; }

        public string PermissionKey { get; set; }
    }
}
