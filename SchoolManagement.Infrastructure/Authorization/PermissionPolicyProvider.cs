using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using SchoolManagement.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace SchoolManagement.Infrastructure.Authorization
{
    // Dynamic policy provider that returns policies for "Permission:..." policies
    public class PermissionPolicyProvider : IAuthorizationPolicyProvider
    {
        private readonly DefaultAuthorizationPolicyProvider _fallbackPolicyProvider;
        private readonly IMemoryCache _cache;
        private readonly IServiceScopeFactory _scopeFactory;

        public PermissionPolicyProvider(IOptions<AuthorizationOptions> options, IServiceScopeFactory scopeFactory, IMemoryCache cache)
        {
            _fallbackPolicyProvider = new DefaultAuthorizationPolicyProvider(options);
            _scopeFactory = scopeFactory;
            _cache = cache;
        }

        public Task<AuthorizationPolicy> GetDefaultPolicyAsync() => _fallbackPolicyProvider.GetDefaultPolicyAsync();

        public Task<AuthorizationPolicy> GetFallbackPolicyAsync() => _fallbackPolicyProvider.GetFallbackPolicyAsync();

        public async Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
        {
            if (!policyName.StartsWith("Permission:"))
                return await _fallbackPolicyProvider.GetPolicyAsync(policyName);

            var permissionKey = policyName.Substring("Permission:".Length);

            // cache policy for short duration to avoid DB hit on every request
            return await _cache.GetOrCreateAsync(policyName, async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5);

                // Use a scope to resolve DbContext (scoped service) safely inside singleton policy provider
                using var scope = _scopeFactory.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                var exists = await db.Permissions.AnyAsync(p => p.PermissionKey == permissionKey);
                if (!exists)
                    return null;

                var policy = new AuthorizationPolicyBuilder()
                    .AddRequirements(new PermissionRequirement(permissionKey))
                    .Build();

                return policy;
            });
        }
    }
}
