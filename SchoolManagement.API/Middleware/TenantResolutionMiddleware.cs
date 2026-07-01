using Microsoft.AspNetCore.Http;
using SchoolManagement.Application.Common.Interfaces;
using SchoolManagement.Infrastructure.Persistence;

namespace SchoolManagement.API.Middleware
{
    public class TenantResolutionMiddleware
    {
        private readonly RequestDelegate _next;
        public TenantResolutionMiddleware(RequestDelegate next) => _next = next;

        public async Task InvokeAsync(HttpContext context, ICurrentTenantService tenantService, ApplicationDbContext dbContext)
        {
            try
            {
                var schoolId = tenantService?.SchoolId ?? 0;
                dbContext?.SetCurrentSchoolId(schoolId);
            }
            catch
            {
                // swallow any tenant resolution errors to not break request pipeline here
            }

            await _next(context);
        }
    }
}
