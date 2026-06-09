using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using SchoolManagement.API.Middleware;

namespace SchoolManagement.API.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IApplicationBuilder UseApiMiddlewares(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionMiddleware>();
            app.UseMiddleware<TenantResolutionMiddleware>();
            return app;
        }
    }
}
