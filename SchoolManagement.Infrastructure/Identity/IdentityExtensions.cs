using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using SchoolManagement.Domain.Entities;
using System;

namespace SchoolManagement.Infrastructure.Identity
{
    public static class IdentityExtensions
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection services)
        {
            services.AddIdentity<ApplicationUser, IdentityRole<int>>()
                .AddEntityFrameworkStores<SchoolManagement.Infrastructure.Persistence.ApplicationDbContext>()
                .AddDefaultTokenProviders();

            return services;
        }
    }
}
