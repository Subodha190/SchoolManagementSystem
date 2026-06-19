using System;
using System.Collections.Generic;
using System.Text;

namespace SchoolManagement.Application.Common.Models.Auth
{
    public class AuthResponseDto
    {
        public string Token { get; set; }

        public string Email { get; set; }

        public string FullName { get; set; }

        public int SchoolId { get; set; }
    }
}
