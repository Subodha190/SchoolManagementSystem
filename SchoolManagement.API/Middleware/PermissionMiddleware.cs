using SchoolManagement.Infrastructure.Persistence;
using System.Security.Claims;

namespace SchoolManagement.API.Middleware
{
    public class PermissionMiddleware
    {
        private readonly RequestDelegate _next;

        public PermissionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(
            HttpContext context,
            ApplicationDbContext db)
        {
            if (!context.User.Identity.IsAuthenticated)
            {
                await _next(context);
                return;
            }

            var role =
                context.User.Claims
                    .FirstOrDefault(x =>
                        x.Type == ClaimTypes.Role)
                    ?.Value;

            var controller =
                context.Request.RouteValues["controller"]
                    ?.ToString();

            var method =
                context.Request.Method;

            var permissionKey =
                $"{controller}.{method}";
        }
    }
}
