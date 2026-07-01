using System;

namespace SchoolManagement.Application.Common.Exceptions
{
    public class ConflictException : Exception
    {
        public ConflictException(string message = "Conflict") : base(message) { }
    }
}
