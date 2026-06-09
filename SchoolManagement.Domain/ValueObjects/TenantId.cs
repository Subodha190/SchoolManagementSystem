using SchoolManagement.Domain.Common;

namespace SchoolManagement.Domain.ValueObjects
{
    public class TenantId : ValueObject
    {
        public Guid Value { get; }
        public TenantId(Guid value) => Value = value;
    }
}
