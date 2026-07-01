using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using SchoolManagement.Application.Common.Interfaces;

namespace SchoolManagement.API.Middleware
{
    public class RequestContextMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestContextMiddleware(RequestDelegate next) => _next = next;

        public async Task InvokeAsync(HttpContext context, IRequestContext requestContext)
        {
            // Trace and correlation IDs
            requestContext.TraceId = Activity.Current?.Id ?? context.TraceIdentifier;
            requestContext.CorrelationId = context.Request.Headers["X-Correlation-ID"].FirstOrDefault() ?? Guid.NewGuid().ToString();

            // IP address
            requestContext.IPAddress = context.Connection.RemoteIpAddress?.ToString() ?? string.Empty;

            // User-Agent
            requestContext.UserAgent = context.Request.Headers["User-Agent"].FirstOrDefault() ?? string.Empty;

            // User identity
            if (context.User?.Identity?.IsAuthenticated == true)
            {
                var userId = context.User.FindFirst("UserId")?.Value;
                if (int.TryParse(userId, out var uid)) requestContext.UserId = uid;

                requestContext.UserName = context.User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value ?? string.Empty;

                var schoolId = context.User.FindFirst("SchoolId")?.Value;
                if (int.TryParse(schoolId, out var sid)) requestContext.SchoolId = sid;

                requestContext.Role = context.User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value ?? string.Empty;
            }

            await _next(context);
        }
    }
}
