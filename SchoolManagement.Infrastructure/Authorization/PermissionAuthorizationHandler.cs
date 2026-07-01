using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SchoolManagement.Application.Common.Interfaces;
using SchoolManagement.Domain.Entities;
using SchoolManagement.Infrastructure.Persistence;
using System.Security.Claims;
using Microsoft.Extensions.DependencyInjection;

namespace SchoolManagement.Infrastructure.Authorization
{
    public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public PermissionAuthorizationHandler(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            var userIdClaim = context.User.FindFirst("UserId")?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
            {
                context.Fail();
                return;
            }

            var userId = int.Parse(userIdClaim);

            // Resolve scoped services inside a scope (UserManager and DbContext are scoped)
            using (var scope = _scopeFactory.CreateScope())
            {
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                var user = await userManager.FindByIdAsync(userId.ToString());
                if (user == null)
                {
                    context.Fail();
                    return;
                }

                // SuperAdmin bypass
                var roles = await userManager.GetRolesAsync(user);
                if (roles.Contains("SuperAdmin"))
                {
                    context.Succeed(requirement);
                    return;
                }

                // Get role ids for the current user's roles
                var roleIds = await db.Roles
                    .Where(r => roles.Contains(r.Name))
                    .Select(r => r.Id)
                    .ToListAsync();

                // Find the permission record for the requested permission key
                var permission = await db.Permissions
                    .FirstOrDefaultAsync(p => p.PermissionKey == requirement.PermissionKey);

                if (permission == null)
                {
                    context.Fail();
                    return;
                }

                // Check if any of the user's roles has the permission
                var has = await db.RolePermissions
                    .AnyAsync(rp => roleIds.Contains(rp.RoleId) && rp.PermissionId == permission.Id);

                if (has)
                    context.Succeed(requirement);
                else
                    context.Fail();
            }
        }
    }
}
