using SchoolManagement.Domain.Common;

namespace SchoolManagement.Domain.ValueObjects
{
    public class Grade : ValueObject
    {
        public int Value { get; private set; }

        // Parameterless constructor required by EF Core for materialization
        private Grade() { }

        public Grade(int value) => Value = value;
    }
}
