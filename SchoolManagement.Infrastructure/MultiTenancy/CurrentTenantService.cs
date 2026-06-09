using SchoolManagement.Application.Common.Interfaces;

namespace SchoolManagement.Infrastructure.MultiTenancy
{
    public class CurrentTenantService : ICurrentTenantService
    {
        public Guid? TenantId => null; // placeholder
    }
}
