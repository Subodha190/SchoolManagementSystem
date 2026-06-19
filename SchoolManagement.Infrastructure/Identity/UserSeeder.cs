using Microsoft.AspNetCore.Identity;
using SchoolManagement.Domain.Entities;
using SchoolManagement.Infrastructure.Persistence;

namespace SchoolManagement.Infrastructure.Identity
{
    public static class UserSeeder
    {
        public static async Task SeedSuperAdminAsync(
            RoleManager<IdentityRole<int>> roleManager,
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext context)
        {
            var email = "superadmin@school.com";

            var existingUser =
                await userManager.FindByEmailAsync(email);

            if (existingUser != null)
                return;

            // ✅ STEP 1: Ensure School exists
            var school = context.Schools.FirstOrDefault();

            if (school == null)
            {
                school = new School
                {
                    SchoolName = "Default School",
                    SchoolCode = "SCH001",
                    Email = "school@demo.com",
                    IsActive = true
                };

                context.Schools.Add(school);
                await context.SaveChangesAsync();
            }

            // ✅ STEP 2: Create User WITH SchoolId
            var user = new ApplicationUser
            {
                Email = email,
                UserName = email,
                FullName = "Super Admin",
                EmailConfirmed = true,
                SchoolId = school.Id   // 🔥 IMPORTANT FIX
            };

            var result =
                await userManager.CreateAsync(user, "Admin@123");

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, "SuperAdmin");
            }
        }
    }
}