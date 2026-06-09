using SchoolManagement.Domain.Common;

namespace SchoolManagement.Domain.ValueObjects
{
    public class Email : ValueObject
    {
        public string Address { get; }
        public Email(string address) => Address = address;
    }
}
