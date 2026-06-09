using System;
using System.Collections.Generic;
using System.Text;

namespace SchoolManagement.Application.Common.Exceptions
{
    public class AppValidationException : Exception
    {
        public IEnumerable<string> Errors { get; }

        public AppValidationException(
            IEnumerable<string> errors)
        {
            Errors = errors;
        }
    }
}
