using Microsoft.AspNetCore.Http;
using SchoolManagement.Application.Common.Interfaces;

namespace SchoolManagement.Infrastructure.MultiTenancy
{
    public class CurrentTenantService
    : ICurrentTenantService
    {
        private readonly IHttpContextAccessor _context;

        public CurrentTenantService(
            IHttpContextAccessor context)
        {
            _context = context;
        }

        public int SchoolId
        {
            get
            {
                var schoolId =
                    _context.HttpContext?
                    .User?
                    .FindFirst("SchoolId")
                    ?.Value;

                return string.IsNullOrEmpty(schoolId)
                    ? 0
                    : Convert.ToInt32(schoolId);
            }
        }
    }
}
