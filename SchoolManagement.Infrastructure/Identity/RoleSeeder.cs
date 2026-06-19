using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace SchoolManagement.Infrastructure.Identity
{
    public static class RoleSeeder
    {
        public static async Task SeedRolesAsync(
            RoleManager<IdentityRole<int>> roleManager)
        {
            string[] roles =
            {
            "SuperAdmin",
            "SchoolAdmin",
            "Teacher",
            "Student",
            "Accountant"
        };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(
                        new IdentityRole<int>(role));
                }
            }
        }
    }
}
